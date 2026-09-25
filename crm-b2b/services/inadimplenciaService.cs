using Microsoft.EntityFrameworkCore;

public class InadimplenciaService
{
    private readonly AppDbContext _context;

    public InadimplenciaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> VerificarInadimplencias()
    {
        var agora = DateTime.UtcNow;

        var faturasVencidas = await _context.Faturas.Where(f =>f.Status == "PENDENTE" && f.DataVencimento < agora).ToListAsync();

        if (faturasVencidas.Count == 0)
        {
            return 0;
        }

        var faturasIds = faturasVencidas.Select(f => f.Id).ToList();

        var inadimplenciasExistentes = await _context.Inadimplencias.Where(i => faturasIds.Contains(i.FaturaId))
            .Select(i => i.FaturaId)
            .ToListAsync();

        var quantidadeCriada = 0;

        foreach (var fatura in faturasVencidas)
        {
            // evita criar inadimplencia duplicada
            if (inadimplenciasExistentes.Contains(fatura.Id))
            {
                continue;
            }

            fatura.Status = "VENCIDA";

            var inadimplencia = new Inadimplencia
            {
                FaturaId = fatura.Id,
                ValorEmAberto = fatura.Valor,
                DataInadimplencia = agora,
                Status = "ABERTA"
            };

            await _context.Inadimplencias.AddAsync(inadimplencia);

            quantidadeCriada++;
        }

        await _context.SaveChangesAsync();

        return quantidadeCriada;
    }
    public async Task<List<InadimplenciaResponse>> GetInadimplencias()
    {
        return await _context.Inadimplencias.AsNoTracking().Include(i => i.Fatura).Select(i => new InadimplenciaResponse
            {
                Id = i.Id,
                FaturaId = i.FaturaId,
                NumeroFatura = i.Fatura.Numero,
                ValorEmAberto = i.ValorEmAberto,
                DataInadimplencia = i.DataInadimplencia,
                Status = i.Status,
                DataRegularizacao = i.DataRegularizacao,
                CriadoEm = i.CriadoEm

            }).ToListAsync();
    }
    public async Task<InadimplenciaResponse?> GetInadimplenciaId(int id)
    {
        var inadimplencia = await _context.Inadimplencias.AsNoTracking().Include(i => i.Fatura).FirstOrDefaultAsync(i => i.Id == id);

        if (inadimplencia == null)
        {
            return null;
        }

        var response = new InadimplenciaResponse
        {
            Id = inadimplencia.Id,
            FaturaId = inadimplencia.FaturaId,
            NumeroFatura = inadimplencia.Fatura.Numero,
            ValorEmAberto = inadimplencia.ValorEmAberto,
            DataInadimplencia = inadimplencia.DataInadimplencia,
            Status = inadimplencia.Status,
            DataRegularizacao = inadimplencia.DataRegularizacao,
            CriadoEm = inadimplencia.CriadoEm
        };

        return response;
    }

    // PATCH/{id}/status - Alterar status
    public async Task<InadimplenciaResponse?> AlterarStatus(int id, InadimplenciaStatusRequest inadimplenciaRequest)
    {
        var inadimplencia = await _context.Inadimplencias.Include(i => i.Fatura).FirstOrDefaultAsync(i => i.Id == id);

        if (inadimplencia == null)
        {
            return null;
        }

        var transicoesPermitidas = new Dictionary<string, string[]>
        {
            ["ABERTA"] = new[] { "REGULARIZADA", "CANCELADA" }
        };

        if (!transicoesPermitidas.TryGetValue(
                inadimplencia.Status,
                out var proximosStatus))
        {
            throw new BusinessException(
                $"Não é possível alterar a inadimplência que está com status {inadimplencia.Status}."
            );
        }

        if (!proximosStatus.Contains(inadimplenciaRequest.Status))
        {
            throw new BusinessException(
                $"Não é permitido alterar a inadimplência de {inadimplencia.Status} para {inadimplenciaRequest.Status}."
            );
        }

        inadimplencia.Status = inadimplenciaRequest.Status;

        if (inadimplenciaRequest.Status == "REGULARIZADA")
        {
            inadimplencia.DataRegularizacao = DateTime.UtcNow;

            if (inadimplencia.Fatura.Status == "VENCIDA")
            {
                inadimplencia.Fatura.Status = "PAGA";
            }
        }

        if (inadimplenciaRequest.Status == "CANCELADA")
        {
            inadimplencia.DataRegularizacao = null;
        }

        await _context.SaveChangesAsync();

        var response = new InadimplenciaResponse
        {
            Id = inadimplencia.Id,
            FaturaId = inadimplencia.FaturaId,
            NumeroFatura = inadimplencia.Fatura.Numero,
            ValorEmAberto = inadimplencia.ValorEmAberto,
            DataInadimplencia = inadimplencia.DataInadimplencia,
            Status = inadimplencia.Status,
            DataRegularizacao = inadimplencia.DataRegularizacao,
            CriadoEm = inadimplencia.CriadoEm
        };

        return response;
    }
    public async Task<bool> DeleteInadimplencia(int id)
    {
        var inadimplencia = await _context.Inadimplencias.FirstOrDefaultAsync(i => i.Id == id);

        if (inadimplencia == null)
        {
            return false;
        }

        if (inadimplencia.Status == "CANCELADA")
        {
            throw new BusinessException(
                "A inadimplência já está cancelada."
            );
        }

        if (inadimplencia.Status == "REGULARIZADA")
        {
            throw new BusinessException(
                "Uma inadimplência regularizada não pode ser cancelada."
            );
        }

        inadimplencia.Status = "CANCELADA";

        await _context.SaveChangesAsync();

        return true;
    }

}