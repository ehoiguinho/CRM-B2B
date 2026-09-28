using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class LeadService
{
    private readonly AppDbContext _context;
    private readonly HistoricoStatusService _historicoStatusService;

    public LeadService(AppDbContext context, HistoricoStatusService historicoStatusService)
    {
        _context = context;
        _historicoStatusService = historicoStatusService;
    }

    public async Task<LeadResponse>PostLead(LeadRequest leadRequest)
    {
        var usuarioResponsavel = await _context.Usuarios.Where(u => u.Perfil == "USER").OrderBy(u => _context.Leads.Count(l => l.UsuarioResponsavelId == u.Id)).FirstOrDefaultAsync();
        
        if (usuarioResponsavel == null)
        {
        throw new BusinessException(
            "Não existem usuários disponíveis para receber o Lead."
        );
        }
        
        var lead = new Lead
        {
            Nome = leadRequest.Nome,
            Empresa = leadRequest.Empresa,
            Email = leadRequest.Email,
            Telefone = leadRequest.Telefone,
            Origem = leadRequest.Origem,
            Observacao = leadRequest.Observacao,
            UsuarioResponsavelId = usuarioResponsavel.Id
        };

        await _context.Leads.AddAsync(lead);
        await _context.SaveChangesAsync();

        var response = new LeadResponse
        {
            Id = lead.Id,
            Nome = lead.Nome,
            Empresa = lead.Empresa,
            Email = lead.Email,
            Telefone = lead.Telefone,
            Origem = lead.Origem,
            Status = lead.Status,
            Observacao = lead.Observacao,
            CriadoEm = lead.CriadoEm,
            UsuarioResponsavelId = lead.UsuarioResponsavelId
        };

        return response;
    }

    public async Task<List<LeadResponse>>GetLeads()
    {
        var leads = await _context.Leads.ToListAsync();

        var response = leads.Select(l => new LeadResponse
        {
            Id = l.Id,
            Nome = l.Nome,
            Empresa = l.Empresa,
            Email = l.Email,
            Telefone = l.Telefone,
            Origem = l.Origem,
            Status = l.Status,
            Observacao = l.Observacao,
            CriadoEm = l.CriadoEm,
            UsuarioResponsavelId = l.UsuarioResponsavelId

        }).ToList();

        return response;
    }

    public async Task<LeadResponse?> PutLead(int id, LeadUpdateRequest leadRequest)
    {
        var leadExistente = await _context.Leads.FirstOrDefaultAsync(l => l.Id == id);
        if(leadExistente == null)
        {
            return null;
        }

        leadExistente.Nome = leadRequest.Nome;
        leadExistente.Empresa = leadRequest.Empresa;
        leadExistente.Email = leadRequest.Email;
        leadExistente.Telefone = leadRequest.Telefone;
        leadExistente.Origem = leadRequest.Origem;
        leadExistente.Observacao = leadRequest.Observacao;

        await _context.SaveChangesAsync();

        var response = new LeadResponse
        {
            Id = leadExistente.Id,
            Nome = leadExistente.Nome,
            Empresa = leadExistente.Empresa,
            Email = leadExistente.Email,
            Telefone = leadExistente.Telefone,
            Origem = leadExistente.Origem,
            Status = leadExistente.Status,
            Observacao = leadExistente.Observacao,
            CriadoEm = leadExistente.CriadoEm
        };

        return response;
    }

    public async Task<LeadResponse?> StatusLead(int id, LeadStatusRequest leadRequest)
    {
        var leadExistente = await _context.Leads.FirstOrDefaultAsync(l => l.Id == id);
        if(leadExistente == null)
        {
            return null;
        }
        var transicoesPermitidas = new Dictionary<string, string[]>
        {

        ["NOVO"] = new[] { "CONTATADO" },
        ["CONTATADO"] = new[] { "QUALIFICADO" },
        ["QUALIFICADO"] = new[] { "CONVERTIDO", "DESCARTADO" }
        };

        if (!transicoesPermitidas.TryGetValue(leadExistente.Status, out var statusPermitidos) || !statusPermitidos.Contains(leadRequest.Status))
        {
        throw new BusinessException(
            "Transição de status não permitida."
        );
        }  

        var statusAnterior = leadExistente.Status;
        leadExistente.Status = leadRequest.Status;

        await _historicoStatusService.Registrar(
            "LEAD",
            leadExistente.Id,
            statusAnterior,
            leadExistente.Status
        );
        
        await _context.SaveChangesAsync();

        var response = new LeadResponse
        {
            Id = leadExistente.Id,
            Nome = leadExistente.Nome,
            Empresa = leadExistente.Empresa,
            Email = leadExistente.Email,
            Telefone = leadExistente.Telefone,
            Origem = leadExistente.Origem,
            Status = leadExistente.Status,
            Observacao = leadExistente.Observacao,
            CriadoEm = leadExistente.CriadoEm
        };

        return response;
    }

    public async Task<bool> DeleteLead(int id)
    {
        var lead = await _context.Leads.FirstOrDefaultAsync(l => l.Id == id);

        if (lead == null)
        {
            return false;
        }
        if(lead.Status == "DESCARTADO" || lead.Status == "CONVERTIDO")
        {
            throw new BusinessException("Não é possível descartar um Lead que já foi descartado ou convertido.");
        }

        var statusAnterior = lead.Status;
        lead.Status = "DESCARTADO";

        await _historicoStatusService.Registrar("LEAD", lead.Id, statusAnterior, lead.Status);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<LeadResponse?> AlterarResponsavel(int id, LeadResponsavelRequest leadRequest)
    {
        var lead = await _context.Leads.FirstOrDefaultAsync(l => l.Id == id);
        if(lead == null)
        {
            return null;
        }

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == leadRequest.UsuarioResponsavelId);
        if(usuario == null)
        {
            throw new BusinessException("Usuário responsavel não encontrado.", 404);
        }

        if(usuario.Perfil != "USER")
        {
            throw new BusinessException("O responsavel pelo lead deve possuir o perfil de USER");
        }

        if (lead.UsuarioResponsavelId == usuario.Id)
        {
            throw new BusinessException(
                "O usuário informado já é o responsável pelo Lead."
            );
        }

        lead.UsuarioResponsavelId = usuario.Id;

        await _context.SaveChangesAsync();

        var response = new LeadResponse
        {
          Id = lead.Id,
            Nome = lead.Nome,
            Empresa = lead.Empresa,
            Email = lead.Email,
            Telefone = lead.Telefone,
            Origem = lead.Origem,
            Status = lead.Status,
            Observacao = lead.Observacao,
            CriadoEm = lead.CriadoEm,
            UsuarioResponsavelId = lead.UsuarioResponsavelId
        };

        return response;
    }

}