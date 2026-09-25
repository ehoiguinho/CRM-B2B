using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("faturas")]
public class FaturaController : ControllerBase
{
    private readonly FaturaService _faturaService;

    public FaturaController(FaturaService faturaService)
    {
        _faturaService = faturaService;
    }

    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostFatura(FaturaRequest faturaRequest)
    {
        var fatura = await _faturaService.PostFatura(faturaRequest);

        return CreatedAtAction("GetFatura",
            new { id = fatura.Id },
            new
            {
                mensagem = "Fatura cadastrada com sucesso.",
                fatura
            }
        );
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetFaturas()
    {
        var faturas = await _faturaService.GetFaturas();

        if (faturas.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhuma fatura encontrada."
            });
        }

        return Ok(faturas);
    }

    [HttpGet("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetFatura(int id)
    {
        var fatura = await _faturaService.GetFaturaId(id);

        if (fatura == null)
        {
            return NotFound(new
            {
                mensagem = "Fatura não encontrada."
            });
        }

        return Ok(fatura);
    }

    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PutFatura(int id, FaturaUpdateRequest faturaRequest)
    {
        var faturaAlterada = await _faturaService.PutFatura(id, faturaRequest);

        if (faturaAlterada == null)
        {
            return NotFound(new
            {
                mensagem = "Fatura não encontrada para alteração."
            });
        }

        return Ok(faturaAlterada);
    }

    [HttpPatch("{id}/status")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PatchStatusFatura(int id, FaturaStatusRequest faturaRequest)
    {
        var statusAlterado = await _faturaService.AlterarStatus(id, faturaRequest);

        if (statusAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Fatura não encontrada para alteração de status."
            });
        }

        return Ok(statusAlterado);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteFatura(int id)
    {
        var faturaCancelada = await _faturaService.DeleteFatura(id);

        if (!faturaCancelada)
        {
            return NotFound(new
            {
                mensagem = "Fatura não encontrada para cancelamento."
            });
        }

        return Ok(new
        {
            mensagem = "Fatura cancelada com sucesso."
        });
    }
}
