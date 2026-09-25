using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("contratos")]
public class ContratoController : ControllerBase
{
    private readonly ContratoService _contratoService;

    public ContratoController(ContratoService contratoService)
    {
        _contratoService = contratoService;
    }

    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostContrato(
        ContratoRequest contratoRequest)
    {
        var contrato = await _contratoService
            .PostContrato(contratoRequest);

        return CreatedAtAction(
            "GetContrato",
            new { id = contrato.Id },
            new
            {
                mensagem = "Contrato cadastrado com sucesso.",
                contrato
            }
        );
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetContratos()
    {
        var contratos = await _contratoService
            .GetContratos();

        if (contratos.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhum contrato encontrado."
            });
        }

        return Ok(contratos);
    }

    [HttpGet("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetContrato(int id)
    {
        var contrato = await _contratoService
            .GetContratoId(id);

        if (contrato == null)
        {
            return NotFound(new
            {
                mensagem = "Contrato não encontrado."
            });
        }

        return Ok(contrato);
    }

    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PutContrato(
        int id,
        ContratoUpdateRequest contratoRequest)
    {
        var contratoAlterado = await _contratoService
            .PutContrato(id, contratoRequest);

        if (contratoAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Contrato não encontrado para alteração."
            });
        }

        return Ok(contratoAlterado);
    }

    [HttpPatch("{id}/status")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PatchStatusContrato(
        int id,
        ContratoStatusRequest contratoRequest)
    {
        var statusAlterado = await _contratoService
            .AlterarStatus(id, contratoRequest);

        if (statusAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Contrato não encontrado para alteração de status."
            });
        }

        return Ok(statusAlterado);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteContrato(int id)
    {
        var contratoCancelado = await _contratoService
            .DeleteContrato(id);

        if (!contratoCancelado)
        {
            return NotFound(new
            {
                mensagem = "Contrato não encontrado para cancelamento."
            });
        }

        return Ok(new
        {
            mensagem = "Contrato cancelado com sucesso."
        });
    }
}
