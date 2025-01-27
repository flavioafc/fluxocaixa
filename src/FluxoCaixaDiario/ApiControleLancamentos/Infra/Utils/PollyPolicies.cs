using Polly;
using Polly.Retry;
using System;

namespace ApiControleLancamentos.Infra.Utils
{
    public static class PollyPolicies
    {
        public static AsyncRetryPolicy RetryPolicy =>
            Policy
                .Handle<Exception>() // Re-tenta em qualquer tipo de exceção
                .WaitAndRetryAsync(
                    3, // Número de tentativas
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Intervalo entre tentativas (backoff exponencial)
                    (exception, timeSpan, retryCount, context) =>
                    {
                        // Log para acompanhar as tentativas (opcional)
                        Console.WriteLine($"Tentativa {retryCount} falhou. Tentando novamente em {timeSpan.TotalSeconds}s. Erro: {exception.Message}");
                    });
    }
}
