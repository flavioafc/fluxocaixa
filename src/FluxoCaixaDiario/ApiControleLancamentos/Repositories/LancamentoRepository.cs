using ApiControleLancamentos.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiControleLancamentos.Repositories;

public class LancamentoRepository : ILancamentoRepository
{
    private readonly AppDbContext _context;

    public LancamentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Lancamento>> GetAll()
    {
        return await _context.Lancamentos.ToListAsync();
    }

    public async Task<Lancamento> GetById(int id)
    {
        return await _context.Lancamentos.FindAsync(id);
    }

    public async Task Add(Lancamento lancamento)
    {
        _context.Lancamentos.Add(lancamento);
        await _context.SaveChangesAsync();
    }
}
