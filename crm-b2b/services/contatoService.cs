using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class ContatoService
{
    private readonly AppDbContext _context;

    public ContatoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ContatoResponse>PostContato(ContatoRequest contatoRequest)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == contatoRequest.ClienteId);
        if(!clienteExiste)
        {
           throw new BusinessException("Cliente não encontrado no sistema.", 404);
        }

        var contato = new Contato
        {
            ClienteId = contatoRequest.ClienteId,
            Nome = contatoRequest.Nome,
            Email = contatoRequest.Email,
            Telefone = contatoRequest.Telefone,
            Cargo = contatoRequest.Cargo
        };

        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();

        var response = new ContatoResponse
        {
            Id = contato.Id,
            ClienteId = contato.ClienteId,
            Nome = contato.Nome,
            Email = contato.Email,
            Telefone = contato.Telefone,
            Cargo = contato.Cargo,
            CriadoEm = contato.CriadoEm
        };

        return response;
    }
    public async Task<List<ContatoResponse>>GetContato()
    {
        var contatos = await _context.Contatos.ToListAsync();
        
        var responses = contatos.Select (c => new ContatoResponse
        {
            Id = c.Id,
            ClienteId = c.ClienteId,
            Nome = c.Nome,
            Email = c.Email,
            Telefone = c.Telefone,
            Cargo = c.Cargo,
            CriadoEm = c.CriadoEm
        }).ToList();

        return responses;
    }

    public async Task<ContatoResponse?> GetContatoId(int id)
{
    var contato = await _context.Contatos
        .FirstOrDefaultAsync(c => c.Id == id);

    if (contato == null)
    {
        return null;
    }

    var response = new ContatoResponse
    {
        Id = contato.Id,
        ClienteId = contato.ClienteId,
        Nome = contato.Nome,
        Email = contato.Email,
        Telefone = contato.Telefone,
        Cargo = contato.Cargo,
        CriadoEm = contato.CriadoEm
    };

    return response;
}   
    public async Task<ContatoResponse?>PutContato(int id, ContatoUpdateRequest contatoRequest)
    {
        var contatoExistente = await _context.Contatos.FirstOrDefaultAsync(c => c.Id == id);
        if(contatoExistente == null)
        {
            return null;
        }

        contatoExistente.Nome = contatoRequest.Nome;
        contatoExistente.Email = contatoRequest.Email;
        contatoExistente.Telefone = contatoRequest.Telefone;
        contatoExistente.Cargo = contatoRequest.Cargo;

        await _context.SaveChangesAsync();

        var response = new ContatoResponse
    {
        Id = contatoExistente.Id,
        ClienteId = contatoExistente.ClienteId,
        Nome = contatoExistente.Nome,
        Email = contatoExistente.Email,
        Telefone = contatoExistente.Telefone,
        Cargo = contatoExistente.Cargo,
        CriadoEm = contatoExistente.CriadoEm
    };

    return response;

}

    public async Task<bool>DeleteContato(int id)
    {
        var contato = await _context.Contatos.FirstOrDefaultAsync(c => c.Id == id);
        if(contato == null)
        {
            return false;
        }

        _context.Contatos.Remove(contato);
        await _context.SaveChangesAsync();

        return true;
        
    }
     
    
}