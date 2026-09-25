using Microsoft.EntityFrameworkCore;

public class FaturaService
{
    private readonly AppDbContext _context;

    public FaturaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FaturaResponse> PostFatura(FaturaRequest faturaRequest)
    {
        var contrato = await _context.Contratos.FirstOrDefaultAsync(c => c.Id == faturaRequest.ContratoId);

        if(contrato == null)
        {
            throw new BusinessException(
                "Contrato não encontrado no sistema.",
                404
            );
        }
        if (contrato.Status == "CANCELADO")
        {
            throw new BusinessException(
                "Não é possível criar uma fatura para um contrato cancelado."
            );
        }

        var numeroExistente = await _context.Faturas.AnyAsync(f => f.Numero == faturaRequest.Numero);

        if (numeroExistente)
        {
            throw new BusinessException(
                "Já existe uma fatura com este número.",
                409
            );
        }

        if (faturaRequest.DataVencimento < faturaRequest.DataEmissao)
        {
            throw new BusinessException(
                "A data de vencimento não pode ser anterior à data de emissão."
            );
        }

        var fatura = new Fatura
        {
            ContratoId = faturaRequest.ContratoId,
            Numero = faturaRequest.Numero,
            Descricao = faturaRequest.Descricao,
            Valor = faturaRequest.Valor,
            DataEmissao = faturaRequest.DataEmissao,
            DataVencimento = faturaRequest.DataVencimento
        };

        await _context.Faturas.AddAsync(fatura);

        await _context.SaveChangesAsync();

         var response = new FaturaResponse
        {
            Id = fatura.Id,
            ContratoId = fatura.ContratoId,
            Numero = fatura.Numero,
            Descricao = fatura.Descricao,
            Valor = fatura.Valor,
            DataEmissao = fatura.DataEmissao,
            DataVencimento = fatura.DataVencimento,
            Status = fatura.Status,
            CriadoEm = fatura.CriadoEm
        };

        return response;
    }
    public async Task<List<FaturaResponse>> GetFaturas()
    {
        return await _context.Faturas.AsNoTracking().Select(f => new FaturaResponse
            {
                Id = f.Id,
                ContratoId = f.ContratoId,
                Numero = f.Numero,
                Descricao = f.Descricao,
                Valor = f.Valor,
                DataEmissao = f.DataEmissao,
                DataVencimento = f.DataVencimento,
                Status = f.Status,
                CriadoEm = f.CriadoEm
            }).ToListAsync();
    }
    public async Task<FaturaResponse?> GetFaturaId(int id)
    {
        var fatura = await _context.Faturas.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

        if (fatura == null)
        {
            return null;
        }

         var response = new FaturaResponse
        {
            Id = fatura.Id,
            ContratoId = fatura.ContratoId,
            Numero = fatura.Numero,
            Descricao = fatura.Descricao,
            Valor = fatura.Valor,
            DataEmissao = fatura.DataEmissao,
            DataVencimento = fatura.DataVencimento,
            Status = fatura.Status,
            CriadoEm = fatura.CriadoEm
        };

        return response;
    }
    public async Task<FaturaResponse?> PutFatura(int id, FaturaUpdateRequest faturaRequest)
    {
        var fatura = await _context.Faturas.FirstOrDefaultAsync(f => f.Id == id);

        if (fatura == null)
        {
            return null;
        }

        if (fatura.Status == "PAGA" || fatura.Status == "CANCELADA")
        {
            throw new BusinessException(
                "Faturas pagas ou canceladas não podem ser alteradas."
            );
        }

        if (faturaRequest.DataVencimento < faturaRequest.DataEmissao)
        {
            throw new BusinessException(
                "A data de vencimento não pode ser anterior à data de emissão."
            );
        }

        fatura.Descricao = faturaRequest.Descricao;
        fatura.Valor = faturaRequest.Valor;
        fatura.DataEmissao = faturaRequest.DataEmissao;
        fatura.DataVencimento = faturaRequest.DataVencimento;

        await _context.SaveChangesAsync();

        var response = new FaturaResponse
        {
            Id = fatura.Id,
            ContratoId = fatura.ContratoId,
            Numero = fatura.Numero,
            Descricao = fatura.Descricao,
            Valor = fatura.Valor,
            DataEmissao = fatura.DataEmissao,
            DataVencimento = fatura.DataVencimento,
            Status = fatura.Status,
            CriadoEm = fatura.CriadoEm
        };

        return response;
    }
    public async Task<FaturaResponse?> AlterarStatus(int id, FaturaStatusRequest faturaRequest)
    {
        var fatura = await _context.Faturas.FirstOrDefaultAsync(f => f.Id == id);

        if (fatura == null)
        {
            return null;
        }

        var transicoesPermitidas = new Dictionary<string, string[]>
        {
            ["PENDENTE"] = new[] { "PAGA", "VENCIDA", "CANCELADA" },
            ["VENCIDA"] = new[] { "PAGA", "CANCELADA" }
        };

        if (!transicoesPermitidas.TryGetValue(
                fatura.Status,
                out var proximosStatus))
        {
            throw new BusinessException(
                $"Não é possível alterar a fatura que está com status {fatura.Status}."
            );
        }

        if (!proximosStatus.Contains(faturaRequest.Status))
        {
            throw new BusinessException(
                $"Não é permitido alterar a fatura de {fatura.Status} para {faturaRequest.Status}."
            );
        }

        fatura.Status = faturaRequest.Status;

        await _context.SaveChangesAsync();

        var response = new FaturaResponse
        {
            Id = fatura.Id,
            ContratoId = fatura.ContratoId,
            Numero = fatura.Numero,
            Descricao = fatura.Descricao,
            Valor = fatura.Valor,
            DataEmissao = fatura.DataEmissao,
            DataVencimento = fatura.DataVencimento,
            Status = fatura.Status,
            CriadoEm = fatura.CriadoEm
        };

        return response;
    }
    public async Task<bool> DeleteFatura(int id)
    {
        var fatura = await _context.Faturas.FirstOrDefaultAsync(f => f.Id == id);

        if (fatura == null)
        {
            return false;
        }

        if (fatura.Status == "PAGA")
        {
            throw new BusinessException(
                "Uma fatura paga não pode ser cancelada."
            );
        }

        if (fatura.Status == "CANCELADA")
        {
            throw new BusinessException(
                "A fatura já está cancelada."
            );
        }

        fatura.Status = "CANCELADA";

        await _context.SaveChangesAsync();

        return true;
    }
    
}
