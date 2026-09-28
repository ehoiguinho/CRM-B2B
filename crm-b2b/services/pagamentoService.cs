using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class PagamentoService
{
    private readonly AppDbContext _context;
    private readonly HistoricoStatusService _historicoStatusService;

    public PagamentoService(AppDbContext context, HistoricoStatusService historicoStatusService)
    {
        _context = context;
        _historicoStatusService = historicoStatusService;
    }

    private static string GerarHash(PagamentoRequest request)
    {
        var dados = new
        {
            request.FaturaId,
            request.Valor,
            request.DataPagamento,
            request.FormaPagamento
        };

        var json = JsonSerializer.Serialize(dados);

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));

        return Convert.ToHexString(bytes);
    }
    public async Task<PagamentoResponse> PostPagamento(PagamentoRequest pagamentoRequest, string chaveIdempotencia)
    {
        var requestHash = GerarHash(pagamentoRequest);

        var registroExistente = await _context.Idempotencias.FirstOrDefaultAsync(i => i.Chave == chaveIdempotencia);

        if (registroExistente != null)
        {
            if (registroExistente.RequestHash != requestHash)
            {
            throw new BusinessException(
                "A Idempotency-Key já foi utilizada com dados diferentes.",
                409
            );
            }

            var respostaAnterior = JsonSerializer.Deserialize<IdempotencyResponse>(registroExistente.ResponseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
            
            return respostaAnterior!.Response;
        }
            await using var transaction = await _context.Database.BeginTransactionAsync();

        try{

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

        var responseBody = JsonSerializer.Serialize(new
        {
            mensagem = "Pagamento realizado com sucesso.", response
        });


         var idempotencia = new Idempotencia
        {
            Chave = chaveIdempotencia,
            RequestHash = requestHash,
            StatusCode = 201,
            ResponseBody = responseBody
        };

        _context.Idempotencias.Add(idempotencia);

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return response;
        }

        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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

        var statusAnterior = pagamento.Status;
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

        await _historicoStatusService.Registrar("PAGAMENTO", pagamento.Id, statusAnterior, pagamento.Status);
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

        var statusAnterior = pagamento.Status;
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

        await _historicoStatusService.Registrar("PAGAMENTO", pagamento.Id, statusAnterior, pagamento.Status);
        await _context.SaveChangesAsync();

        return true;
    }


}