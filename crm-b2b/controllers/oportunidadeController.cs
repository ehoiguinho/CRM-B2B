using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("oportunidades")]
public class OportunidadeController : ControllerBase
{
    private readonly OportunidadeService _oportunidadeService;

    public OportunidadeController(OportunidadeService oportunidadeService)
    {
        _oportunidadeService = oportunidadeService;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostOportunidade(OportunidadeRequest oportunidadeRequest)
    {
        var oportunidade = await _oportunidadeService.PostOportunidade(oportunidadeRequest);

        return CreatedAtAction(
            "GetOportunidade",
            new { id = oportunidade.Id },
            new
            {
                mensagem = "Oportunidade cadastrada com sucesso.",
                oportunidade
            }
        );
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetOportunidades()
    {
        var oportunidades = await _oportunidadeService.GetOportunidades();

        if (oportunidades.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhuma oportunidade encontrada."
            });
        }

        return Ok(oportunidades);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetOportunidade(int id)
    {
        var oportunidade = await _oportunidadeService.GetOportunidadeId(id);

        if (oportunidade == null)
        {
            return NotFound(new
            {
                mensagem = "Oportunidade não encontrada."
            });
        }

        return Ok(oportunidade);
    }

    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PutOportunidade(int id, OportunidadeUpdateRequest oportunidadeRequest)
    {
        var oportunidadeAlterada = await _oportunidadeService.PutOportunidade(id, oportunidadeRequest);
        if(oportunidadeAlterada == null)
        {
            return NotFound(new
            {
                mensagem = "Oportunidade não encontrada para alteração."
            });
        }

        return Ok(oportunidadeAlterada);

    }

    [HttpPatch("{id}/etapa")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>PatchEtapaOportunidade(int id, OportunidadeEtapaRequest oportunidadeRequest)
    {
        var oportunidadeExistente = await _oportunidadeService.AlterarEtapa(id, oportunidadeRequest);
        if(oportunidadeExistente == null)
        {
            return NotFound(new
            {
                mensagem = "Oportunidade não encontrada no sistema, para realizar a mudanca de etapa."
            });
        }
        
        return Ok(oportunidadeExistente);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteOportunidade(int id)
    {
        var oportunidadeExistente = await _oportunidadeService.DeleteOportunidade(id);
        if(oportunidadeExistente == false)
        {
            return NotFound(new
            {
                mensagem = "Oportunidade não encontrada para deleção"
            });
        }

        return Ok(new
        {
            mensagem = "Oportunidade deletada com sucesso."
        });
    }
}