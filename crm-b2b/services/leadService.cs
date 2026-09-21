using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
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

}