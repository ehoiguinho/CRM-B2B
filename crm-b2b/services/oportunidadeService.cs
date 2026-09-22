using Microsoft.EntityFrameworkCore;

public class OportunidadeService
{
    private readonly AppDbContext _context;

    public OportunidadeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OportunidadeResponse> PostOportunidade(
        OportunidadeRequest oportunidadeRequest)
    {
        var clienteExiste = await _context.Clientes
            .AnyAsync(c => c.Id == oportunidadeRequest.ClienteId);

        if (!clienteExiste)
        {
            throw new BusinessException(
                "Cliente não encontrado no sistema.",
                404
            );
        }

        var oportunidade = new Oportunidade
        {
            ClienteId = oportunidadeRequest.ClienteId,
            Titulo = oportunidadeRequest.Titulo,
            Descricao = oportunidadeRequest.Descricao,
            Valor = oportunidadeRequest.Valor
        };

        await _context.Oportunidades.AddAsync(oportunidade);
        await _context.SaveChangesAsync();

        var response = new OportunidadeResponse
        {
            Id = oportunidade.Id,
            ClienteId = oportunidade.ClienteId,
            Titulo = oportunidade.Titulo,
            Descricao = oportunidade.Descricao,
            Valor = oportunidade.Valor,
            Etapa = oportunidade.Etapa,
            CriadoEm = oportunidade.CriadoEm
        };

        return response;
    }

    public async Task<List<OportunidadeResponse>> GetOportunidades()
    {
        var oportunidades = await _context.Oportunidades
            .ToListAsync();

        var responses = oportunidades.Select(o => new OportunidadeResponse
        {
            Id = o.Id,
            ClienteId = o.ClienteId,
            Titulo = o.Titulo,
            Descricao = o.Descricao,
            Valor = o.Valor,
            Etapa = o.Etapa,
            CriadoEm = o.CriadoEm
        }).ToList();

        return responses;
    }

    public async Task<OportunidadeResponse?> GetOportunidadeId(int id)
    {
        var oportunidade = await _context.Oportunidades
            .FirstOrDefaultAsync(o => o.Id == id);

        if (oportunidade == null)
        {
            return null;
        }

        var response = new OportunidadeResponse
        {
            Id = oportunidade.Id,
            ClienteId = oportunidade.ClienteId,
            Titulo = oportunidade.Titulo,
            Descricao = oportunidade.Descricao,
            Valor = oportunidade.Valor,
            Etapa = oportunidade.Etapa,
            CriadoEm = oportunidade.CriadoEm
        };

        return response;
    }

    public async Task<OportunidadeResponse?>PutOportunidade(int id, OportunidadeUpdateRequest oportunidadeRequest)
    {
        var oportunidadeExistente = await _context.Oportunidades.FirstOrDefaultAsync(o => o.Id == id);
        if(oportunidadeExistente == null)
        {
            return null;
        }

        oportunidadeExistente.Titulo = oportunidadeRequest.Titulo;
        oportunidadeExistente.Descricao = oportunidadeRequest.Descricao;
        oportunidadeExistente.Valor = oportunidadeRequest.Valor;

        await _context.SaveChangesAsync();

        var response = new OportunidadeResponse
        {
            Id = oportunidadeExistente.Id,
            ClienteId = oportunidadeExistente.ClienteId,
            Titulo = oportunidadeExistente.Titulo,
            Descricao = oportunidadeExistente.Descricao,
            Valor = oportunidadeExistente.Valor,
            Etapa = oportunidadeExistente.Etapa,
            CriadoEm = oportunidadeExistente.CriadoEm
        };

        return response;
    }

    public async Task<OportunidadeResponse?>AlterarEtapa(int id, OportunidadeEtapaRequest oportunidadeRequest)
    {
        var oportunidadeExistente = await _context.Oportunidades.FirstOrDefaultAsync(o => o.Id == id);
        if(oportunidadeExistente == null)
        {
            return null;
        }

        var transicoesPermitidas = new Dictionary<string, string[]>
        {

        ["ABERTA"] = new[] { "QUALIFICACAO" },
        ["QUALIFICACAO"] = new[] { "PROPOSTA" },
        ["PROPOSTA"] = new[] { "NEGOCIACAO" },
        ["NEGOCIACAO"] = new[] { "GANHA", "PERDIDA"}
        };

        if (!transicoesPermitidas.TryGetValue(oportunidadeExistente.Etapa, out var etapasPermitidas) || !etapasPermitidas.Contains(oportunidadeRequest.Etapa))
        {
        throw new BusinessException(
            "Transição de etapa não permitida."
        );
        }
        oportunidadeExistente.Etapa = oportunidadeRequest.Etapa;
        await _context.SaveChangesAsync();

        var response = new OportunidadeResponse
        {
            Id = oportunidadeExistente.Id,
            ClienteId = oportunidadeExistente.ClienteId,
            Titulo = oportunidadeExistente.Titulo,
            Descricao = oportunidadeExistente.Descricao,
            Valor = oportunidadeExistente.Valor,
            Etapa = oportunidadeExistente.Etapa,
            CriadoEm = oportunidadeExistente.CriadoEm
        };

        return response;
    }

        public async Task<bool> DeleteOportunidade(int id)
        {
            var oportunidade = await _context.Oportunidades.FirstOrDefaultAsync(o => o.Id == id);
            if(oportunidade == null)
            {
                return false;
            }

            _context.Oportunidades.Remove(oportunidade);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AtividadeResponse?> AlterarStatus(int id, AtividadeStatusRequest atividadeRequest)
        {

            var atividadeExistente = await _context.Atividades.FirstOrDefaultAsync(a => a.Id == id);

            if (atividadeExistente == null)
            {
                return null;
            }

            var transicoesPermitidas = new Dictionary<string, string[]>
            {
                ["PENDENTE"] = new[] { "CONCLUIDA" }
            };

            if (!transicoesPermitidas.TryGetValue(
                    atividadeExistente.Status,
                    out var statusPermitidos)
                || !statusPermitidos.Contains(atividadeRequest.Status))
            {
                throw new BusinessException(
                    "Transição de status não permitida."
                );
            }

            atividadeExistente.Status = atividadeRequest.Status;

            await _context.SaveChangesAsync();

            var response = new AtividadeResponse
            {
                Id = atividadeExistente.Id,
                OportunidadeId = atividadeExistente.OportunidadeId,
                Tipo = atividadeExistente.Tipo,
                Titulo = atividadeExistente.Titulo,
                Descricao = atividadeExistente.Descricao,
                DataAgendada = atividadeExistente.DataAgendada,
                Status = atividadeExistente.Status,
                CriadoEm = atividadeExistente.CriadoEm
            };

        return response;
    }

}