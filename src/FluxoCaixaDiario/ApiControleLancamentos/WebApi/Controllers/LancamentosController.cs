using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiControleLancamentos.Application.UseCases.RegistrarLancamento;
using ApiControleLancamentos.Application.UseCases.ListarLancamentos;
using ApiControleLancamentos.Application.UseCases.AtualizarLancamento;
using ApiControleLancamentos.Application.UseCases.CancelarLancamento;
using ApiControleLancamentos.Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class LancamentosController : ControllerBase
{
    private readonly RegistrarLancamentoHandler _registrarHandler;
    private readonly ListarLancamentosHandler _listarHandler;
    private readonly AtualizarLancamentoHandler _atualizarHandler;
    private readonly CancelarLancamentoHandler _cancelarHandler;

    public LancamentosController(
        RegistrarLancamentoHandler registrarHandler,
        ListarLancamentosHandler listarHandler,
        AtualizarLancamentoHandler atualizarHandler,
        CancelarLancamentoHandler cancelarHandler)
    {
        _registrarHandler = registrarHandler;
        _listarHandler = listarHandler;
        _atualizarHandler = atualizarHandler;
        _cancelarHandler = cancelarHandler;
    }

    // GET: /api/lancamentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetLancamentos()
    {
        var lancamentos = await _listarHandler.Handle();
        return Ok(lancamentos);
    }

    // GET: /api/lancamentos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<LancamentoDto>> GetLancamento(int id)
    {
        var lancamento = await _listarHandler.HandleById(id);
        if (lancamento == null)
        {
            return NotFound();
        }
        return Ok(lancamento);
    }

    // POST: /api/lancamentos
    [HttpPost]
    public async Task<ActionResult<LancamentoDto>> PostLancamento(RegistrarLancamentoCommand command)
    {
        var lancamentoId = await _registrarHandler.Handle(command);
        return CreatedAtAction(nameof(GetLancamento), new { id = lancamentoId }, lancamentoId);
    }

    // PUT: /api/lancamentos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLancamento(int id, AtualizarLancamentoCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        var result = await _atualizarHandler.Handle(command);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: /api/lancamentos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLancamento(int id)
    {
        var command = new CancelarLancamentoCommand { Id = id };
        var result = await _cancelarHandler.Handle(command);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
