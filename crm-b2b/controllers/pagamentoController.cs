using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("pagamentos")]
public class PagamentoController : ControllerBase
{
    private readonly PagamentoService _pagamentoService;

    public PagamentoController(PagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostPagamento(PagamentoRequest pagamentoRequest)
    {
        var pagamento = await _pagamentoService.PostPagamento(pagamentoRequest);

        return CreatedAtAction("GetPagamento",new { id = pagamento.Id }, new
            {
                mensagem = "Pagamento registrado com sucesso.",
                pagamento
            }
        );
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetPagamentos()
    {
        var pagamentos = await _pagamentoService.GetPagamentos();

        if (pagamentos.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhum pagamento encontrado."
            });
        }

        return Ok(pagamentos);
    }

    [HttpGet("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetPagamento(int id)
    {
        var pagamento = await _pagamentoService.GetPagamentoId(id);

        if (pagamento == null)
        {
            return NotFound(new
            {
                mensagem = "Pagamento não encontrado."
            });
        }

        return Ok(pagamento);
    }

    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PutPagamento(int id, PagamentoUpdateRequest pagamentoRequest)
    {
        var pagamentoAlterado = await _pagamentoService.PutPagamento(id, pagamentoRequest);

        if (pagamentoAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Pagamento não encontrado para alteração."
            });
        }

        return Ok(pagamentoAlterado);
    }

    [HttpPatch("{id}/status")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PatchStatusPagamento(int id, PagamentoStatusRequest pagamentoRequest)
    {
        var statusAlterado = await _pagamentoService.AlterarStatus(id, pagamentoRequest);

        if (statusAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Pagamento não encontrado para alteração de status."
            });
        }

        return Ok(statusAlterado);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeletePagamento(int id)
    {
        var pagamentoCancelado = await _pagamentoService.DeletePagamento(id);

        if (!pagamentoCancelado)
        {
            return NotFound(new
            {
                mensagem = "Pagamento não encontrado para cancelamento."
            });
        }

        return Ok(new
        {
            mensagem = "Pagamento cancelado com sucesso."
        });
    }
}
