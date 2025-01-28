using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using WorkerConsolidado.Models;
using WorkerConsolidado.Services;

namespace WorkerConsolidado
{
    public class WorkerConsolidado : BackgroundService
    {
        private readonly ILogger<WorkerConsolidado> _logger;
        private readonly FluxoDeCaixaService _fluxoDeCaixaService;

        private IConnection _connection;
        private IChannel _channel;

        // Nome da fila que iremos consumir
        private const string QUEUE_NAME = "fila-fluxo-de-caixa";

        public WorkerConsolidado(
            ILogger<WorkerConsolidado> logger,
            FluxoDeCaixaService fluxoDeCaixaService)
        {
            _logger = logger;
            _fluxoDeCaixaService = fluxoDeCaixaService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Conecta ao RabbitMQ e prepara o consumo
                await IniciarRabbitMQ();

                _logger.LogInformation("WorkerConsolidado iniciado e aguardando mensagens...");

                // Fica em execução até o serviço ser parado
                while (!stoppingToken.IsCancellationRequested)
                {
                    // Podemos inserir alguma lógica adicional se necessário,
                    // ou simplesmente aguardar as mensagens chegarem.

                    // Se quiser evitar spin sem necessidade, aguarde um pouco
                    await Task.Delay(500, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro no WorkerConsolidado.");
                throw;
            }
            finally
            {
                // Se o Worker for finalizado, fechamos a conexão
                if (_channel != null)
                {
                    await _channel.CloseAsync(stoppingToken);
                }
                if (_connection != null)
                {
                    await _connection.CloseAsync(stoppingToken);
                }
            }
        }

        private async Task IniciarRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",   // Ajuste conforme seu ambiente
                UserName = "guest",
                Password = "guest",
                // Port = 5672, etc...
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Declara a fila (caso ela não exista, será criada)
            await _channel.QueueDeclareAsync(
                queue: QUEUE_NAME,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Prefetch para controlar quantas mensagens o worker consome por vez
            await _channel.BasicQosAsync(0, 1, false);

            // Cria o consumidor
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                // Quando receber a mensagem:
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Mensagem recebida da fila {0}: {1}", QUEUE_NAME, message);

                    // Converte a mensagem em objeto (JSON -> Classe MovimentoFinanceiro)
                    var movimento = JsonSerializer.Deserialize<MovimentoFinanceiro>(message);

                    // Processa a lógica de fluxo de caixa
                    if (movimento != null)
                    {
                        _fluxoDeCaixaService.ProcessarMovimento(movimento);
                    }

                    // Faz o ack manual da mensagem, garantindo que só será removida após o sucesso
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem da fila.");
                    // Caso ocorra erro, podemos fazer um Nack/Requeue, ou Nack/DeadLetter
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            // Inicia o consumo
            await _channel.BasicConsumeAsync(
                queue: QUEUE_NAME,
                autoAck: false, // false para controlar manualmente o ack
                consumer: consumer
            );

            _logger.LogInformation("Conectado ao RabbitMQ e aguardando mensagens na fila {0}.", QUEUE_NAME);
        }
    }
}
