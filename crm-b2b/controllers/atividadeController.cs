using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("atividades")]
public class AtividadeController : ControllerBase
{
    private readonly AtividadeService _atividadeService;

    public AtividadeController(AtividadeService atividadeService)
    {
        _atividadeService = atividadeService;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostAtividade(
        AtividadeRequest atividadeRequest)
    {
        var atividade = await _atividadeService
            .PostAtividade(atividadeRequest);

        return CreatedAtAction(
            "GetAtividade",
            new { id = atividade.Id },
            new
            {
                mensagem = "Atividade cadastrada com sucesso.",
                atividade
            }
        );
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetAtividades()
    {
        var atividades = await _atividadeService
            .GetAtividades();

        if (atividades.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhuma atividade encontrada."
            });
        }

        return Ok(atividades);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetAtividade(int id)
    {
        var atividade = await _atividadeService
            .GetAtividadeId(id);

        if (atividade == null)
        {
            return NotFound(new
            {
                mensagem = "Atividade não encontrada."
            });
        }

        return Ok(atividade);
    }

    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>PutAtividade(int id, AtividadeUpdateRequest atividadeRequest)
    {
        var atividadeAlterada = await _atividadeService.PutAtividade(id, atividadeRequest);
        if(atividadeAlterada == null)
        {
            return NotFound(new
            {
                mensagem = "Atividade não encontrada para alteração."
            });
        }

        return Ok(atividadeAlterada);
    }

    [HttpPatch("{id}/status")][Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PatchStatusAtividade(int id, AtividadeStatusRequest atividadeRequest)
        {

            var statusAlterado = await _atividadeService.AlterarStatus(id, atividadeRequest);
            if (statusAlterado == null)
            {
                return NotFound(new
                {
                    mensagem = "Atividade não encontrada para alteração de status."
                });
            }

            return Ok(statusAlterado);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>DeleteAtividade(int id)
    {
        var atividadeDeletada = await _atividadeService.DeleteAtividade(id);
        if(atividadeDeletada == false)
        {
            return NotFound(new
            {
                mensagem = "Atividade não encontrada para deleção."
            });
        }

        return Ok(new
        {
            mensagem = "Atividade deletada com sucesso."
        });
    }
}