using Microsoft.EntityFrameworkCore;

public class OportunidadeItemService
{
    private readonly AppDbContext _context;

    public OportunidadeItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OportunidadeItemResponse> PostItem(int oportunidadeId, OportunidadeItemRequest itemRequest)
    {
        var oportunidade = await _context.Oportunidades.FirstOrDefaultAsync(o => o.Id == oportunidadeId);

        if (oportunidade == null)
        {
            throw new BusinessException(
                "Oportunidade não encontrada no sistema.",
                404
            );
        }

        if (oportunidade.Etapa == "GANHA" ||
            oportunidade.Etapa == "PERDIDA")
        {
            throw new BusinessException(
                "Não é possível alterar os itens de uma oportunidade encerrada."
            );
        }

        var produto = await _context.ProdutosServicos.FirstOrDefaultAsync(p => p.Id == itemRequest.ProdutoServicoId);

        if (produto == null)
        {
            throw new BusinessException(
                "Produto ou serviço não encontrado.",
                404
            );
        }

        if (!produto.Ativo)
        {
            throw new BusinessException(
                "Não é possível adicionar um produto ou serviço inativo."
            );
        }

        var itemExistente = await _context.OportunidadeItens.FirstOrDefaultAsync(i =>i.OportunidadeId == oportunidadeId && i.ProdutoServicoId == itemRequest.ProdutoServicoId);

        if (itemExistente != null)
        {
            throw new BusinessException(
                "Este produto ou serviço já está adicionado à oportunidade."
            );
        }

        var item = new OportunidadeItem
        {
            OportunidadeId = oportunidadeId,
            ProdutoServicoId = produto.Id,
            Quantidade = itemRequest.Quantidade,
            ValorUnitario = produto.Valor
        };

        await _context.OportunidadeItens.AddAsync(item);

        await _context.SaveChangesAsync();

        await AtualizarValorOportunidade(oportunidadeId);

        return MapearParaResponse(item, produto.Nome);
    }
    public async Task<List<OportunidadeItemResponse>> GetItens(int oportunidadeId)
    {
        var oportunidadeExiste = await _context.Oportunidades.AnyAsync(o => o.Id == oportunidadeId);

        if (!oportunidadeExiste)
        {
            throw new BusinessException(
                "Oportunidade não encontrada no sistema.",
                404
            );
        }

        return await _context.OportunidadeItens.AsNoTracking().Where(i => i.OportunidadeId == oportunidadeId)
            .Select(i => new OportunidadeItemResponse
            {
                Id = i.Id,
                OportunidadeId = i.OportunidadeId,
                ProdutoServicoId = i.ProdutoServicoId,
                ProdutoServicoNome = i.ProdutoServico.Nome,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.Quantidade * i.ValorUnitario
                
            }).ToListAsync();
    }
    public async Task<OportunidadeItemResponse?> GetItemId(int oportunidadeId, int itemId)
    {
        var item = await _context.OportunidadeItens.AsNoTracking().Include(i => i.ProdutoServico).FirstOrDefaultAsync(i =>i.Id == itemId && i.OportunidadeId == oportunidadeId);

        if (item == null)
        {
            return null;
        }

        return MapearParaResponse(
            item,
            item.ProdutoServico.Nome
        );
    }
    public async Task<OportunidadeItemResponse?> PutItem(int oportunidadeId, int itemId, OportunidadeItemUpdateRequest itemRequest)
    {
        var oportunidade = await _context.Oportunidades.FirstOrDefaultAsync(o => o.Id == oportunidadeId);

        if (oportunidade == null)
        {
            throw new BusinessException(
                "Oportunidade não encontrada no sistema.",
                404
            );
        }

        if (oportunidade.Etapa == "GANHA" ||
            oportunidade.Etapa == "PERDIDA")
        {
            throw new BusinessException(
                "Não é possível alterar os itens de uma oportunidade encerrada."
            );
        }

        var item = await _context.OportunidadeItens.Include(i => i.ProdutoServico).FirstOrDefaultAsync(i => i.Id == itemId && i.OportunidadeId == oportunidadeId);

        if (item == null)
        {
            return null;
        }

        item.Quantidade = itemRequest.Quantidade;

        await _context.SaveChangesAsync();

        await AtualizarValorOportunidade(oportunidadeId);

        return MapearParaResponse(
            item,
            item.ProdutoServico.Nome
        );
    }
    public async Task<bool> DeleteItem(int oportunidadeId, int itemId)
    {
        var oportunidade = await _context.Oportunidades.FirstOrDefaultAsync(o => o.Id == oportunidadeId);

        if (oportunidade == null)
        {
            throw new BusinessException(
                "Oportunidade não encontrada no sistema.",
                404
            );
        }

        if (oportunidade.Etapa == "GANHA" ||
            oportunidade.Etapa == "PERDIDA")
        {
            throw new BusinessException(
                "Não é possível remover itens de uma oportunidade encerrada."
            );
        }

        var item = await _context.OportunidadeItens.FirstOrDefaultAsync(i => i.Id == itemId && i.OportunidadeId == oportunidadeId);

        if (item == null)
        {
            return false;
        }

        _context.OportunidadeItens.Remove(item);

        await _context.SaveChangesAsync();

        await AtualizarValorOportunidade(oportunidadeId);

        return true;
    }
    private async Task AtualizarValorOportunidade(int oportunidadeId)
    {
        var valorTotal = await _context.OportunidadeItens.Where(i => i.OportunidadeId == oportunidadeId) .SumAsync(i => i.Quantidade * i.ValorUnitario);

        var oportunidade = await _context.Oportunidades.FirstOrDefaultAsync(o => o.Id == oportunidadeId);

        if (oportunidade == null)
        {
            return;
        }

        oportunidade.Valor = valorTotal;

        await _context.SaveChangesAsync();
    }

    private static OportunidadeItemResponse MapearParaResponse(
        OportunidadeItem item,
        string produtoNome)
    {
        return new OportunidadeItemResponse
        {
            Id = item.Id,
            OportunidadeId = item.OportunidadeId,
            ProdutoServicoId = item.ProdutoServicoId,
            ProdutoServicoNome = produtoNome,
            Quantidade = item.Quantidade,
            ValorUnitario = item.ValorUnitario,
            ValorTotal = item.Quantidade * item.ValorUnitario
        };
    }
}
