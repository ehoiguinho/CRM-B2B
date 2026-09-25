using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("oportunidades/{oportunidadeId}/itens")]
public class OportunidadeItemController : ControllerBase
{
    private readonly OportunidadeItemService _itemService;

    public OportunidadeItemController(OportunidadeItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PostItem(int oportunidadeId, OportunidadeItemRequest itemRequest)
    {
        var item = await _itemService.PostItem(oportunidadeId, itemRequest);

        return CreatedAtAction("GetItem", new{oportunidadeId,itemId = item.Id}, new
            {
                mensagem = "Item adicionado à oportunidade com sucesso.",
                item
            }
        );
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetItens(int oportunidadeId)
    {
        var itens = await _itemService.GetItens(oportunidadeId);

        if (itens.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhum item encontrado para esta oportunidade."
            });
        }

        return Ok(itens);
    }

    [HttpGet("{itemId}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetItem(int oportunidadeId, int itemId)
    {
        var item = await _itemService.GetItemId(oportunidadeId, itemId);

        if (item == null)
        {
            return NotFound(new
            {
                mensagem = "Item não encontrado na oportunidade."
            });
        }

        return Ok(item);
    }

    [HttpPut("{itemId}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> PutItem(int oportunidadeId, int itemId, OportunidadeItemUpdateRequest itemRequest)
    {
        var itemAlterado = await _itemService.PutItem(oportunidadeId, itemId, itemRequest);

        if (itemAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Item não encontrado na oportunidade."
            });
        }

        return Ok(itemAlterado);
    }

    [HttpDelete("{itemId}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteItem(int oportunidadeId, int itemId)
    {
        var itemRemovido = await _itemService.DeleteItem(oportunidadeId, itemId);

        if (!itemRemovido)
        {
            return NotFound(new
            {
                mensagem = "Item não encontrado na oportunidade."
            });
        }

        return Ok(new
        {
            mensagem = "Item removido da oportunidade com sucesso."
        });
    }
}