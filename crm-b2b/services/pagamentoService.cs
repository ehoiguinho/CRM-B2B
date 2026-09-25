using Microsoft.EntityFrameworkCore;

public class PagamentoService
{
    private readonly AppDbContext _context;

    public PagamentoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagamentoResponse> PostPagamento(PagamentoRequest pagamentoRequest)
    {
        var fatura = await _context.Faturas.FirstOrDefaultAsync(f => f.Id == pagamentoRequest.FaturaId);

        if (fatura == null)
        {
            throw new BusinessException(
                "Fatura não encontrada no sistema.",
                404
            );
        }

        if (fatura.Status == "CANCELADA")
        {
            throw new BusinessException(
                "Não é possível registrar um pagamento para uma fatura cancelada."
            );
        }

        if (fatura.Status == "PAGA")
        {
            throw new BusinessException(
                "A fatura já está paga."
            );
        }

        if (pagamentoRequest.Valor != fatura.Valor)
        {
            throw new BusinessException(
                "O valor do pagamento deve ser igual ao valor da fatura."
            );
        }

        if (pagamentoRequest.DataPagamento < fatura.DataEmissao)
        {
            throw new BusinessException(
                "A data do pagamento não pode ser anterior à data de emissão da fatura."
            );
        }

        var pagamento = new Pagamento
        {
            FaturaId = pagamentoRequest.FaturaId,
            Valor = pagamentoRequest.Valor,
            DataPagamento = pagamentoRequest.DataPagamento,
            FormaPagamento = pagamentoRequest.FormaPagamento,
            Status = "CONFIRMADO"
        };

        await _context.Pagamentos.AddAsync(pagamento);

        fatura.Status = "PAGA";

        await _context.SaveChangesAsync();

        var response = new PagamentoResponse
        {
            Id = pagamento.Id,
            FaturaId = pagamento.FaturaId,
            Valor = pagamento.Valor,
            DataPagamento = pagamento.DataPagamento,
            FormaPagamento = pagamento.FormaPagamento,
            Status = pagamento.Status,
            CriadoEm = pagamento.CriadoEm
        };

        return response; 
    }


    public async Task<List<PagamentoResponse>> GetPagamentos()
    {
        return await _context.Pagamentos.AsNoTracking().Select(p => new PagamentoResponse
            {
                Id = p.Id,
                FaturaId = p.FaturaId,
                Valor = p.Valor,
                DataPagamento = p.DataPagamento,
                FormaPagamento = p.FormaPagamento,
                Status = p.Status,
                CriadoEm = p.CriadoEm

            }).ToListAsync();
    }
    public async Task<PagamentoResponse?> GetPagamentoId(int id)
    {
        var pagamento = await _context.Pagamentos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        if (pagamento == null)
        {
            return null;
        }

        var response = new PagamentoResponse
        {
            Id = pagamento.Id,
            FaturaId = pagamento.FaturaId,
            Valor = pagamento.Valor,
            DataPagamento = pagamento.DataPagamento,
            FormaPagamento = pagamento.FormaPagamento,
            Status = pagamento.Status,
            CriadoEm = pagamento.CriadoEm
        };

        return response; 
    }
    public async Task<PagamentoResponse?> PutPagamento(int id, PagamentoUpdateRequest pagamentoRequest)
    {
        var pagamento = await _context.Pagamentos.FirstOrDefaultAsync(p => p.Id == id);

        if (pagamento == null)
        {
            return null;
        }

        if (pagamento.Status == "CANCELADO")
        {
            throw new BusinessException(
                "Um pagamento cancelado não pode ser alterado."
            );
        }

        var fatura = await _context.Faturas
            .FirstOrDefaultAsync(f => f.Id == pagamento.FaturaId);

        if (fatura == null)
        {
            throw new BusinessException(
                "Fatura vinculada ao pagamento não encontrada.",
                404
            );
        }

        if (pagamentoRequest.DataPagamento < fatura.DataEmissao)
        {
            throw new BusinessException(
                "A data do pagamento não pode ser anterior à data de emissão da fatura."
            );
        }

        pagamento.DataPagamento = pagamentoRequest.DataPagamento;
        pagamento.FormaPagamento = pagamentoRequest.FormaPagamento;

        await _context.SaveChangesAsync();

        var response = new PagamentoResponse
        {
            Id = pagamento.Id,
            FaturaId = pagamento.FaturaId,
            Valor = pagamento.Valor,
            DataPagamento = pagamento.DataPagamento,
            FormaPagamento = pagamento.FormaPagamento,
            Status = pagamento.Status,
            CriadoEm = pagamento.CriadoEm
        };

        return response; 
    }
    public async Task<PagamentoResponse?> AlterarStatus(int id, PagamentoStatusRequest pagamentoRequest)
    {
        var pagamento = await _context.Pagamentos.FirstOrDefaultAsync(p => p.Id == id);

        if (pagamento == null)
        {
            return null;
        }

        var transicoesPermitidas = new Dictionary<string, string[]>
        {
            ["CONFIRMADO"] = new[] { "CANCELADO" }
        };

        if (!transicoesPermitidas.TryGetValue(pagamento.Status, out var proximosStatus))
        {
            throw new BusinessException(
                $"Não é possível alterar o pagamento que está com status {pagamento.Status}."
            );
        }

        if (!proximosStatus.Contains(pagamentoRequest.Status))
        {
            throw new BusinessException(
                $"Não é permitido alterar o pagamento de {pagamento.Status} para {pagamentoRequest.Status}."
            );
        }

        pagamento.Status = pagamentoRequest.Status;

        if (pagamentoRequest.Status == "CANCELADO")
        {
            var fatura = await _context.Faturas
                .FirstOrDefaultAsync(f => f.Id == pagamento.FaturaId);

            if (fatura == null)
            {
                throw new BusinessException(
                    "Fatura vinculada ao pagamento não encontrada.",
                    404
                );
            }

            if (fatura.Status == "PAGA")
            {
                if (DateTime.UtcNow > fatura.DataVencimento)
                {
                    fatura.Status = "VENCIDA";
                }
                else
                {
                    fatura.Status = "PENDENTE";
                }
            }
        }

        await _context.SaveChangesAsync();

        var response = new PagamentoResponse
        {
            Id = pagamento.Id,
            FaturaId = pagamento.FaturaId,
            Valor = pagamento.Valor,
            DataPagamento = pagamento.DataPagamento,
            FormaPagamento = pagamento.FormaPagamento,
            Status = pagamento.Status,
            CriadoEm = pagamento.CriadoEm
        };

        return response; 
    }
    public async Task<bool> DeletePagamento(int id)
    {
        var pagamento = await _context.Pagamentos.FirstOrDefaultAsync(p => p.Id == id);

        if (pagamento == null)
        {
            return false;
        }

        if (pagamento.Status == "CANCELADO")
        {
            throw new BusinessException(
                "O pagamento já está cancelado."
            );
        }

        pagamento.Status = "CANCELADO";

        var fatura = await _context.Faturas
            .FirstOrDefaultAsync(f => f.Id == pagamento.FaturaId);

        if (fatura == null)
        {
            throw new BusinessException(
                "Fatura vinculada ao pagamento não encontrada.",
                404
            );
        }

        if (fatura.Status == "PAGA")
        {
            if (DateTime.UtcNow > fatura.DataVencimento)
            {
                fatura.Status = "VENCIDA";
            }
            else
            {
                fatura.Status = "PENDENTE";
            }
        }

        await _context.SaveChangesAsync();

        return true;
    }

}