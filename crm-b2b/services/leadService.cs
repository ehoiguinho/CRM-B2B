using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class LeadService
{
    private readonly AppDbContext _context;

    public LeadService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LeadResponse>PostLead(LeadRequest leadRequest)
    {
        var lead = new Lead
        {
            Nome = leadRequest.Nome,
            Empresa = leadRequest.Empresa,
            Email = leadRequest.Email,
            Telefone = leadRequest.Telefone,
            Origem = leadRequest.Origem,
            Observacao = leadRequest.Observacao

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

        leadExistente.Status = leadRequest.Status;
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
        var lead = await _context.Leads
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lead == null)
        {
            return false;
        }

        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();

        return true;
    }   

}