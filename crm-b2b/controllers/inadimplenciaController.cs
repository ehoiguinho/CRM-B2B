using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("inadimplencias")]
public class InadimplenciaController : ControllerBase
{
    private readonly InadimplenciaService _inadimplenciaService;

    public InadimplenciaController(
        InadimplenciaService inadimplenciaService)
    {
        _inadimplenciaService = inadimplenciaService;
    }

    [HttpPost("verificar")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> VerificarInadimplencias()
    {
        var quantidadeCriada = await _inadimplenciaService.VerificarInadimplencias();

        return Ok(new
        {
            mensagem = quantidadeCriada > 0 ? "Inadimplências verificadas e registradas com sucesso." 
            : "Nenhuma nova inadimplência encontrada.",
            quantidadeCriada
        });
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetInadimplencias()
    {
        var inadimplencias = await _inadimplenciaService.GetInadimplencias();

        if (inadimplencias.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhuma inadimplência encontrada."
            });
        }

        return Ok(inadimplencias);
    }

    [HttpGet("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetInadimplencia(int id)
    {
        var inadimplencia = await _inadimplenciaService.GetInadimplenciaId(id);

        if (inadimplencia == null)
        {
            return NotFound(new
            {
                mensagem = "Inadimplência não encontrada."
            });
        }

        return Ok(inadimplencia);
    }

    [HttpPatch("{id}/status")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PatchStatusInadimplencia(int id, InadimplenciaStatusRequest inadimplenciaRequest)
    {
        var statusAlterado = await _inadimplenciaService.AlterarStatus(id, inadimplenciaRequest);

        if (statusAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Inadimplência não encontrada para alteração de status."
            });
        }

        return Ok(statusAlterado);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteInadimplencia(int id)
    {
        var inadimplenciaCancelada = await _inadimplenciaService.DeleteInadimplencia(id);

        if (!inadimplenciaCancelada)
        {
            return NotFound(new
            {
                mensagem = "Inadimplência não encontrada para cancelamento."
            });
        }

        return Ok(new
        {
            mensagem = "Inadimplência cancelada com sucesso."
        });
    }
}