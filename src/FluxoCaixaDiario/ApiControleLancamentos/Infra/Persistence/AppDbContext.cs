using ApiControleLancamentos.Domain.Entities;
using ApiControleLancamentos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApiControleLancamentos.Infra.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Lancamento> Lancamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.Property(e => e.Valor).HasPrecision(18, 2); // Define precisão
        });

        modelBuilder.Entity<Lancamento>()
        .HasIndex(l => l.Status);


        modelBuilder.Entity<Lancamento>().HasData(
            new Lancamento
            {
                Id = 1,
                Valor = 100.50M,
                Tipo = TipoLancamento.Credito,
                Descricao = "Lançamento Inicial",
                Status = StatusLancamento.Ativo,
                Data = new DateTime(2025, 1, 1)
            }
        );

    }

}
