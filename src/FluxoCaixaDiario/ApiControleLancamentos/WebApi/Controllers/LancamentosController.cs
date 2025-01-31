using Microsoft.AspNetCore.Mvc;

using ApiControleLancamentos.Application.UseCases.RegistrarLancamento;
using ApiControleLancamentos.Application.UseCases.ListarLancamentos;
using ApiControleLancamentos.Application.UseCases.AtualizarLancamento;
using ApiControleLancamentos.Application.UseCases.CancelarLancamento;
using ApiControleLancamentos.Application.DTOs;
using ApiControleLancamentos.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class LancamentosController : ControllerBase
{
    private readonly RegistrarLancamentoHandler _registrarHandler;
    private readonly ListarLancamentosHandler _listarHandler;
    private readonly AtualizarLancamentoHandler _atualizarHandler;
    private readonly CancelarLancamentoHandler _cancelarHandler;
    private readonly ILogger<LancamentosController> _logger;

    public LancamentosController(
        RegistrarLancamentoHandler registrarHandler,
        ListarLancamentosHandler listarHandler,
        AtualizarLancamentoHandler atualizarHandler,
        CancelarLancamentoHandler cancelarHandler,
        ILogger<LancamentosController> logger)
    {
        _registrarHandler = registrarHandler;
        _listarHandler = listarHandler;
        _atualizarHandler = atualizarHandler;
        _cancelarHandler = cancelarHandler;
        _logger = logger;
    }

    // GET: /api/lancamentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetLancamentos()
    {
        var correlationId = Guid.NewGuid().ToString();
        _logger.LogInformation("Requisição recebida",
            new
            {
                Endpoint = "GET /api/lancamentos",
                Timestamp = DateTime.UtcNow,
                Service = "ApiControleLancamentos",
                CorrelationId = correlationId
            });

        var lancamentos = await _listarHandler.Handle();

        _logger.LogInformation("Consulta de lançamentos concluída",
            new
            {
                Quantidade = lancamentos.Count(),
                CorrelationId = correlationId
            });

        return Ok(lancamentos);
    }

    // GET: /api/lancamentos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<LancamentoDto>> GetLancamento(int id)
    {
        var correlationId = Guid.NewGuid().ToString();
        _logger.LogInformation("Requisição recebida", 
            new { 
                Endpoint = "GET /api/lancamentos/{id}", 
                Timestamp = DateTime.UtcNow, Service = "ApiControleLancamentos", 
                CorrelationId = correlationId, Id = id 
            });

        var lancamento = await _listarHandler.HandleById(id);
        if (lancamento == null)
        {
            _logger.LogWarning("Lançamento não encontrado", new { Id = id, CorrelationId = correlationId });
            return NotFound();
        }

        _logger.LogInformation("Lançamento encontrado", new { Lancamento = lancamento, CorrelationId = correlationId });
        return Ok(lancamento);
    }

    // POST: /api/lancamentos
    [HttpPost]
    public async Task<ActionResult<LancamentoDto>> PostLancamento(RegistrarLancamentoCommand command)
    {
        var correlationId = Guid.NewGuid().ToString();
        _logger.LogInformation("Requisição recebida", new { Endpoint = "POST /api/lancamentos", Timestamp = DateTime.UtcNow, Service = "ApiControleLancamentos", CorrelationId = correlationId, Dados = command });

        var lancamentoId = await _registrarHandler.Handle(command);
        _logger.LogInformation("Lançamento criado com sucesso", new { LancamentoId = lancamentoId, CorrelationId = correlationId });

        return CreatedAtAction(nameof(GetLancamento), new { id = lancamentoId }, lancamentoId);
    }

    // PUT: /api/lancamentos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLancamento(int id, AtualizarLancamentoCommand command)
    {
        var correlationId = Guid.NewGuid().ToString();
        if (id != command.Id)
        {
            _logger.LogWarning("ID do payload não corresponde ao ID da URL", new { UrlId = id, PayloadId = command.Id, CorrelationId = correlationId });
            return BadRequest();
        }

        _logger.LogInformation("Requisição recebida", new { Endpoint = "PUT /api/lancamentos/{id}", Timestamp = DateTime.UtcNow, Service = "ApiControleLancamentos", CorrelationId = correlationId, Dados = command });
        var result = await _atualizarHandler.Handle(command);
        if (!result)
        {
            _logger.LogWarning("Lançamento não encontrado para atualização", new { Id = id, CorrelationId = correlationId });
            return NotFound();
        }

        _logger.LogInformation("Lançamento atualizado com sucesso", new { Id = id, CorrelationId = correlationId });
        return NoContent();
    }

    // DELETE: /api/lancamentos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLancamento(int id)
    {
        var correlationId = Guid.NewGuid().ToString();
        _logger.LogInformation("Requisição recebida", new { Endpoint = "DELETE /api/lancamentos/{id}", Timestamp = DateTime.UtcNow, Service = "ApiControleLancamentos", CorrelationId = correlationId, Id = id });

        var command = new CancelarLancamentoCommand { Id = id };
        var result = await _cancelarHandler.Handle(command);
        if (!result)
        {
            _logger.LogWarning("Tentativa de cancelamento de lançamento inexistente", new { Id = id, CorrelationId = correlationId });
            return NotFound();
        }

        _logger.LogInformation("Lançamento cancelado com sucesso", new { Id = id, CorrelationId = correlationId });
        return NoContent();
    }
}
