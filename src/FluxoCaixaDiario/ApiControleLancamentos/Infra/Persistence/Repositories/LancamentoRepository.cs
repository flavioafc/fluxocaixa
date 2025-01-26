using ApiControleLancamentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiControleLancamentos.Infra.Persistence.Repositories;

public class LancamentoRepository : ILancamentoRepository
{
    private readonly AppDbContext _context;

    public LancamentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Lancamento>> GetAllAsync()
    {
        return await _context.Lancamentos.ToListAsync();
    }

    public async Task<Lancamento?> GetByIdAsync(int id)
    {
        return await _context.Lancamentos.FindAsync(id);
    }

    public async Task AddAsync(Lancamento lancamento)
    {
        await _context.Lancamentos.AddAsync(lancamento);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lancamento lancamento)
    {
        _context.Lancamentos.Update(lancamento);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Lancamento lancamento)
    {
        _context.Lancamentos.Remove(lancamento);
        await _context.SaveChangesAsync();
    }
}
