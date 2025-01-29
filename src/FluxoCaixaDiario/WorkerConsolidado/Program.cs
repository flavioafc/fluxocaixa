using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerConsolidado.Options;
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
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    var env = hostContext.HostingEnvironment;

                    // Carrega appsettings e variáveis de ambiente
                    configBuilder
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();

                    // Se quiser Key Vault:
                    var tempConfig = configBuilder.Build();
                    var keyVaultUrl = tempConfig["KeyVaultUrl"]; // Ex.: "https://meuKeyVault.vault.azure.net"
                    if (!string.IsNullOrEmpty(keyVaultUrl))
                    {
                        var credential = new DefaultAzureCredential();
                        configBuilder.AddAzureKeyVault(new Uri(keyVaultUrl), credential);
                    }
                })

                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<RabbitMQOptions>(
                        hostContext.Configuration.GetSection("RabbitMQ"));

                    services.AddHostedService<WorkerConsolidado>();

                    services.AddSingleton<FluxoDeCaixaService>();
                });
    }
}
