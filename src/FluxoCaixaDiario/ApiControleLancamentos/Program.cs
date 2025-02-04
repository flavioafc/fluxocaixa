using ApiControleLancamentos.Application.Interfaces;
using ApiControleLancamentos.Application.UseCases.AtualizarLancamento;
using ApiControleLancamentos.Application.UseCases.CancelarLancamento;
using ApiControleLancamentos.Application.UseCases.ListarLancamentos;
using ApiControleLancamentos.Application.UseCases.RegistrarLancamento;
using ApiControleLancamentos.Infra.Messaging;
using ApiControleLancamentos.Infra.Persistence;
using ApiControleLancamentos.Infra.Persistence.Repositories;
using ApiControleLancamentos.Infra.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prometheus;
using Serilog;
using Serilog.Sinks.Elasticsearch;


var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitOptions = context.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

        // cfg.Host(...) usando os valores do appsettings
        cfg.Host(rabbitOptions.Host, rabbitOptions.VirtualHost, h =>
        {
            h.Username(rabbitOptions.UserName);
            h.Password(rabbitOptions.Password);
        });

        // Antes: "lancamento-criado-queue" estava fixo
        //        "ApiControleLancamentos.Domain.Events:LancamentoCriadoEvent" estava fixo
        // Agora: pegando do rabbitOptions
        cfg.ReceiveEndpoint(rabbitOptions.QueueName, e =>
        {
            e.PrefetchCount = 20;
            e.ConcurrentMessageLimit = 10;
            e.Bind(rabbitOptions.ExchangeName);
        });
    });

});

//Infraestrutura
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();

//Handlers
builder.Services.AddScoped<RegistrarLancamentoHandler>();
builder.Services.AddScoped<ListarLancamentosHandler>();
builder.Services.AddScoped<AtualizarLancamentoHandler>();
builder.Services.AddScoped<CancelarLancamentoHandler>();

//Serviços
builder.Services.AddScoped<LancamentoService>();

// Configurar Serilog para Logs Estruturados
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("Application", "ApiControleLancamentos")
    .WriteTo.Console(formatter: new Serilog.Formatting.Json.JsonFormatter())
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "log-api-{0:yyyy.MM.dd}"
    })
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddApplicationInsightsTelemetry();


// Configuração de controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}


// Adicionar suporte ao servidor de métricas
app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseRouting();
app.MapControllers();

app.Run();
