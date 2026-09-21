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
}