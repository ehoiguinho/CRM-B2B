using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("produtos-servicos")]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoService _produtoService;

    public ProdutoController(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostProduto(ProdutoRequest produtoRequest)
    {
        var produto = await _produtoService.PostProduto(produtoRequest);

        return CreatedAtAction("GetProduto", new { id = produto.Id }, new
            {
                mensagem = "Produto/serviço cadastrado com sucesso.",
                produto
            }
        );
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _produtoService.GetProdutos();

        if (produtos.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhum produto ou serviço encontrado."
            });
        }

        return Ok(produtos);
    }

    [HttpGet("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetProduto(int id)
    {
        var produto = await _produtoService.GetProdutoId(id);

        if (produto == null)
        {
            return NotFound(new
            {
                mensagem = "Produto ou serviço não encontrado."
            });
        }

        return Ok(produto);
    }

    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PutProduto(int id, ProdutoUpdateRequest produtoRequest)
    {
        var produtoAlterado = await _produtoService.PutProduto(id, produtoRequest);

        if (produtoAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Produto ou serviço não encontrado para alteração."
            });
        }

        return Ok(produtoAlterado);
    }

    [HttpPatch("{id}/status")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PatchStatusProduto(int id, [FromQuery] bool ativo)
    {
        var produtoAlterado = await _produtoService.AlterarStatus(id, ativo);

        if (produtoAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Produto ou serviço não encontrado para alteração de status."
            });
        }

        return Ok(produtoAlterado);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var produtoDesativado = await _produtoService.DeleteProduto(id);

        if (!produtoDesativado)
        {
            return NotFound(new
            {
                mensagem = "Produto ou serviço não encontrado para desativação."
            });
        }

        return Ok(new
        {
            mensagem = "Produto ou serviço desativado com sucesso."
        });
    }
}
