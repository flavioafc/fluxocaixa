using ApiControleLancamentos.Application.Interfaces;
using ApiControleLancamentos.Application.UseCases.AtualizarLancamento;
using ApiControleLancamentos.Application.UseCases.CancelarLancamento;
using ApiControleLancamentos.Application.UseCases.ListarLancamentos;
using ApiControleLancamentos.Application.UseCases.RegistrarLancamento;
using ApiControleLancamentos.Infra.Messaging;
using ApiControleLancamentos.Infra.Persistence;
using ApiControleLancamentos.Infra.Persistence.Repositories;
using ApiControleLancamentos.Infra.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Injeção de dependências

builder.Services.AddScoped<LancamentoService>();
builder.Services.AddScoped<RegistrarLancamentoHandler>();
builder.Services.AddScoped<ListarLancamentosHandler>();
builder.Services.AddScoped<AtualizarLancamentoHandler>();
builder.Services.AddScoped<CancelarLancamentoHandler>();
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();




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

// Outros middlewares
app.UseHttpsRedirection();
app.UseAuthorization();


app.UseRouting();
app.MapControllers();

app.Run();
