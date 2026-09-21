using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("leads")]
public class LeadController : ControllerBase
{
    private readonly LeadService _leadService;

    public LeadController(LeadService leadService)
    {
        _leadService = leadService;
    }
    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostLead(LeadRequest leadRequest)
        {
        var lead = await _leadService.PostLead(leadRequest);
        return CreatedAtAction("GetLeads", new {id = lead.Id}, new
        {
            mensagem = "Lead cadastrado com sucesso.", lead
        });
        
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>GetLeads()
    {
        var lead = await _leadService.GetLeads();
        {
            if(lead.Count == 0)
            {
                return NotFound(new
                {
                    mensagem = "Nenhum lead encontrado."
                });
            }
            
            return Ok(lead);
            
        }
    }

    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>PutLead(int id, LeadUpdateRequest leadRequest)
    {
        var leadAlterado = await _leadService.PutLead(id, leadRequest);
        if(leadAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Lead não encontrado para alteração."
            });
        }

        return Ok(leadAlterado);
    }

    [HttpPatch("{id}/status")][Authorize (Roles = "ADMIN")]
    public async Task<IActionResult>PatchStatusLead(int id, LeadStatusRequest leadRequest)
    {
        var statusAlterado = await _leadService.StatusLead(id, leadRequest);
        if(statusAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Lead nao encontrado para alteração de status."
            });
        }

        return Ok(statusAlterado);
    }
    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteLead(int id)
    {
        var leadDeletado = await _leadService.DeleteLead(id);

        if (!leadDeletado)
        {
            return NotFound(new
            {
                mensagem = "Lead não encontrado para exclusão."
            });
        }

        return Ok(new
        {
            mensagem = "Lead deletado com sucesso."
        });
    }
}