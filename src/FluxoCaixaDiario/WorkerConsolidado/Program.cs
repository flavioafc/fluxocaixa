using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerConsolidado.Services;

namespace WorkerConsolidado
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Aqui podemos configurar logs, etc.
                .ConfigureServices((hostContext, services) =>
                {
                    // Registra o WorkerConsolidado como servi�o hospedado
                    services.AddHostedService<WorkerConsolidado>();

                    // Se tiver algum servi�o adicional, registrar aqui
                    services.AddSingleton<FluxoDeCaixaService>();
                });
    }
}
