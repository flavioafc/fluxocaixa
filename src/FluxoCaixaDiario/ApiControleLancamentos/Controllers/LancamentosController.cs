using ApiControleLancamentos.Models;
using ApiControleLancamentos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class LancamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public LancamentosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /api/lancamentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lancamento>>> GetLancamentos()
    {
        return await _context.Lancamentos
            .Where(l => l.Status == StatusLancamento.Ativo) // Filtra apenas lançamentos ativos
            .ToListAsync();
    }

    // GET: /api/lancamentos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Lancamento>> GetLancamento(int id)
    {
        var lancamento = await _context.Lancamentos.FindAsync(id);

        if (lancamento == null)
        {
            return NotFound();
        }

        return lancamento;
    }

    // POST: /api/lancamentos
    [HttpPost]
    public async Task<ActionResult<Lancamento>> PostLancamento(Lancamento lancamento)
    {
        lancamento.Status = StatusLancamento.Ativo; // Define como "Ativo" por padrão
        _context.Lancamentos.Add(lancamento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLancamento), new { id = lancamento.Id }, lancamento);
    }

    // PUT: /api/lancamentos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLancamento(int id, Lancamento lancamento)
    {
        if (id != lancamento.Id)
        {
            return BadRequest();
        }

        var existingLancamento = await _context.Lancamentos.FindAsync(id);
        if (existingLancamento == null)
        {
            return NotFound();
        }

        existingLancamento.Descricao = lancamento.Descricao;
        existingLancamento.Valor = lancamento.Valor;
        existingLancamento.Tipo = lancamento.Tipo;
        existingLancamento.Data = lancamento.Data;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: /api/lancamentos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLancamento(int id)
    {
        var lancamento = await _context.Lancamentos.FindAsync(id);
        if (lancamento == null)
        {
            return NotFound();
        }

        lancamento.Status = StatusLancamento.Cancelado; // Marca como "Cancelado"
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
