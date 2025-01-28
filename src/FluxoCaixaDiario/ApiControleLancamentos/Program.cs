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

var builder = WebApplication.CreateBuilder(args);

// Configura��o do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Cria uma fila associada � exchange do evento publicado
        cfg.ReceiveEndpoint("lancamento-criado-queue", e =>
        {
            // Mesmo que n�o tenha consumidor, isso vincula a fila � exchange
            e.Bind("ApiControleLancamentos.Domain.Events:LancamentoCriadoEvent");
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

//Servi�os
builder.Services.AddScoped<LancamentoService>();


// Configura��o de controllers
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

// Outros middlewares
app.UseHttpsRedirection();
app.UseAuthorization();


app.UseRouting();
app.MapControllers();

app.Run();
