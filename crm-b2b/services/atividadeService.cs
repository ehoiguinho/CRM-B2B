using Microsoft.EntityFrameworkCore;

public class AtividadeService
{
    private readonly AppDbContext _context;

    public AtividadeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AtividadeResponse> PostAtividade(
        AtividadeRequest atividadeRequest)
    {
        var oportunidadeExiste = await _context.Oportunidades
            .AnyAsync(o => o.Id == atividadeRequest.OportunidadeId);

        if (!oportunidadeExiste)
        {
            throw new BusinessException(
                "Oportunidade não encontrada no sistema.",
                404
            );
        }

        var atividade = new Atividade
        {
            OportunidadeId = atividadeRequest.OportunidadeId,
            Tipo = atividadeRequest.Tipo,
            Titulo = atividadeRequest.Titulo,
            Descricao = atividadeRequest.Descricao,
            DataAgendada = atividadeRequest.DataAgendada
        };

        await _context.Atividades.AddAsync(atividade);
        await _context.SaveChangesAsync();

        var response = new AtividadeResponse
        {
            Id = atividade.Id,
            OportunidadeId = atividade.OportunidadeId,
            Tipo = atividade.Tipo,
            Titulo = atividade.Titulo,
            Descricao = atividade.Descricao,
            DataAgendada = atividade.DataAgendada,
            Status = atividade.Status,
            Ativo = atividade.Ativo,
            CriadoEm = atividade.CriadoEm
        };

        return response;
    }

    public async Task<List<AtividadeResponse>> GetAtividades()
    {
        var atividades = await _context.Atividades.Where(a => a.Ativo).ToListAsync();

        var responses = atividades.Select(a => new AtividadeResponse
        {
            Id = a.Id,
            OportunidadeId = a.OportunidadeId,
            Tipo = a.Tipo,
            Titulo = a.Titulo,
            Descricao = a.Descricao,
            DataAgendada = a.DataAgendada,
            Status = a.Status,
            Ativo = a.Ativo,
            CriadoEm = a.CriadoEm
        }).ToList();

        return responses;
    }

    public async Task<AtividadeResponse?> GetAtividadeId(int id)
    {
        var atividade = await _context.Atividades
            .FirstOrDefaultAsync(a => a.Id == id && a.Ativo);

        if (atividade == null)
        {
            return null;
        }

        var response = new AtividadeResponse
        {
            Id = atividade.Id,
            OportunidadeId = atividade.OportunidadeId,
            Tipo = atividade.Tipo,
            Titulo = atividade.Titulo,
            Descricao = atividade.Descricao,
            DataAgendada = atividade.DataAgendada,
            Status = atividade.Status,
            Ativo = atividade.Ativo,
            CriadoEm = atividade.CriadoEm
        };

        return response;
    }

    public async Task<AtividadeResponse?>PutAtividade(int id, AtividadeUpdateRequest atividadeRequest)
    {
        var atividadeExistente = await _context.Atividades.FirstOrDefaultAsync(a => a.Id == id && a.Ativo);
        if(atividadeExistente == null)
        {
            return null;
        }

        atividadeExistente.Tipo = atividadeRequest.Tipo;
        atividadeExistente.Titulo = atividadeRequest.Titulo;
        atividadeExistente.Descricao = atividadeRequest.Descricao;
        atividadeExistente.DataAgendada = atividadeRequest.DataAgendada;

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
            Ativo = atividadeExistente.Ativo,
            CriadoEm = atividadeExistente.CriadoEm
        };

        return response;
    }

    public async Task<AtividadeResponse?> AlterarStatus(int id, AtividadeStatusRequest atividadeRequest)
    {
        var atividadeExistente = await _context.Atividades.FirstOrDefaultAsync(a => a.Id == id && a.Ativo);

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

        return new AtividadeResponse
        {
            Id = atividadeExistente.Id,
            OportunidadeId = atividadeExistente.OportunidadeId,
            Tipo = atividadeExistente.Tipo,
            Titulo = atividadeExistente.Titulo,
            Descricao = atividadeExistente.Descricao,
            DataAgendada = atividadeExistente.DataAgendada,
            Status = atividadeExistente.Status,
            Ativo = atividadeExistente.Ativo,
            CriadoEm = atividadeExistente.CriadoEm
        };
    }

    public async Task<bool>DeleteAtividade(int id)
    {
        var atividade = await _context.Atividades.FirstOrDefaultAsync(a => a.Id == id && a.Ativo);
        if(atividade == null)
        {
            return false;
        }

        atividade.Ativo = false;
        await _context.SaveChangesAsync();

        return true;
    }
}