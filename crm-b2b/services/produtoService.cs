using Microsoft.EntityFrameworkCore;

public class ProdutoService
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProdutoResponse> PostProduto(ProdutoRequest produtoRequest)
    {
        var nomeExistente = await _context.ProdutosServicos.AnyAsync(p => p.Nome == produtoRequest.Nome);

        if (nomeExistente)
        {
            throw new BusinessException(
                "Já existe um produto ou serviço com este nome.",
                409
            );
        }

        if (produtoRequest.Tipo != "PRODUTO" &&
            produtoRequest.Tipo != "SERVICO")
        {
            throw new BusinessException(
                "O tipo deve ser PRODUTO ou SERVICO."
            );
        }

        var produto = new ProdutoServico
        {
            Nome = produtoRequest.Nome,
            Descricao = produtoRequest.Descricao,
            Tipo = produtoRequest.Tipo,
            Valor = produtoRequest.Valor,
            Ativo = true
        };

        await _context.ProdutosServicos.AddAsync(produto);
        await _context.SaveChangesAsync();

        var response = new ProdutoResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Tipo = produto.Tipo,
            Valor = produto.Valor,
            Ativo = produto.Ativo,
            CriadoEm = produto.CriadoEm
        };

        return response;
    }
    public async Task<List<ProdutoResponse>> GetProdutos()
    {
        return await _context.ProdutosServicos.AsNoTracking().Where(p => p.Ativo)
        .Select(p => new ProdutoResponse
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Tipo = p.Tipo,
                Valor = p.Valor,
                Ativo = p.Ativo,
                CriadoEm = p.CriadoEm
            })
            .ToListAsync();
    }
    public async Task<ProdutoResponse?> GetProdutoId(int id)
    {
        var produto = await _context.ProdutosServicos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            return null;
        }

        var response = new ProdutoResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Tipo = produto.Tipo,
            Valor = produto.Valor,
            Ativo = produto.Ativo,
            CriadoEm = produto.CriadoEm
        };

        return response;
    }
    public async Task<ProdutoResponse?> PutProduto(int id, ProdutoUpdateRequest produtoRequest)
    {
        var produto = await _context.ProdutosServicos.FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            return null;
        }

        if (!produto.Ativo)
        {
            throw new BusinessException(
                "Um produto ou serviço inativo não pode ser alterado."
            );
        }

        var nomeExistente = await _context.ProdutosServicos.AnyAsync(p =>p.Id != id &&p.Nome == produtoRequest.Nome);

        if (nomeExistente)
        {
            throw new BusinessException(
                "Já existe outro produto ou serviço com este nome.",
                409
            );
        }

        produto.Nome = produtoRequest.Nome;
        produto.Descricao = produtoRequest.Descricao;
        produto.Valor = produtoRequest.Valor;

        await _context.SaveChangesAsync();

        var response = new ProdutoResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Tipo = produto.Tipo,
            Valor = produto.Valor,
            Ativo = produto.Ativo,
            CriadoEm = produto.CriadoEm
        };

        return response;
    }


    public async Task<ProdutoResponse?> AlterarStatus(int id, bool ativo)
    {
        var produto = await _context.ProdutosServicos.FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            return null;
        }

        if (produto.Ativo == ativo)
        {
            throw new BusinessException(
                ativo
                    ? "O produto ou serviço já está ativo."
                    : "O produto ou serviço já está inativo."
            );
        }

        produto.Ativo = ativo;

        await _context.SaveChangesAsync();

        var response = new ProdutoResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Tipo = produto.Tipo,
            Valor = produto.Valor,
            Ativo = produto.Ativo,
            CriadoEm = produto.CriadoEm
        };

        return response;
    }
    public async Task<bool> DeleteProduto(int id)
    {
        var produto = await _context.ProdutosServicos.FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            return false;
        }

        if (!produto.Ativo)
        {
            throw new BusinessException(
                "O produto ou serviço já está inativo."
            );
        }

        produto.Ativo = false;

        await _context.SaveChangesAsync();

        return true;
    }

}