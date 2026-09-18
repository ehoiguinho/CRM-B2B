

using Microsoft.EntityFrameworkCore;

public class ClienteService
{
    private readonly AppDbContext _context;

    public ClienteService(AppDbContext context){
        _context = context;
    }

    public async Task <ClienteResponse> PostCliente(ClienteRequest clienteRequest)
    {
        var cnpjExiste = await _context.Clientes.AnyAsync(u => u.Cnpj == clienteRequest.Cnpj);
        if (cnpjExiste)
        {
            throw new BusinessException("CNPJ Já cadastrado no sistema.", 409);
        }

        var cliente = new Cliente
        {
            RazaoSocial = clienteRequest.RazaoSocial,
            NomeFantasia = clienteRequest.NomeFantasia,
            Cnpj = clienteRequest.Cnpj,
            Email = clienteRequest.Email,
            Telefone = clienteRequest.Telefone,

        };

        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();

        var response = new ClienteResponse
        {
            Id = cliente.Id,
            RazaoSocial = cliente.RazaoSocial,
            NomeFantasia = cliente.NomeFantasia,
            Cnpj = cliente.Cnpj,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            Status = cliente.Status,
            CriadoEm = cliente.CriadoEm
        };

        return response;
        
    }
    public async Task <List<ClienteResponse>> GetClientes()
    {
        var clientes = await _context.Clientes.ToListAsync();

        var responses = clientes.Select(u => new ClienteResponse
        {
            Id = u.Id,
            RazaoSocial = u.RazaoSocial,
            NomeFantasia = u.NomeFantasia,
            Cnpj = u.Cnpj,
            Email = u.Email,
            Telefone = u.Telefone,
            Status = u.Status,
            CriadoEm = u.CriadoEm
        }).ToList();

        return responses;
    }
    public async Task<ClienteResponse?> GetClienteId(int id)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(u => u.Id == id);
        if(cliente == null)
        {
            return null;
        }

        var response = new ClienteResponse
        {
            Id = cliente.Id,
            RazaoSocial = cliente.RazaoSocial,
            NomeFantasia = cliente.NomeFantasia,
            Cnpj = cliente.Cnpj,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            Status = cliente.Status,
            CriadoEm = cliente.CriadoEm
        };

        return response;
        
    }
    public async Task<ClienteResponse?> PutCliente(int id, ClienteRequest clienteRequest)
    {
        var emailExiste = await _context.Clientes.AnyAsync(u => u.Email == clienteRequest.Email & u.Id != id);
        if(emailExiste)
        {
            throw new BusinessException("E-mail já cadastrado no sistema", 409);
        }
        var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(u => u.Id == id);
        if(clienteExistente == null)
        {
            return null;
        }
            Console.WriteLine($"CriadoEm: {clienteExistente.CriadoEm}");

            clienteExistente.RazaoSocial = clienteRequest.RazaoSocial;
            clienteExistente.NomeFantasia = clienteRequest.NomeFantasia;
            clienteExistente.Email = clienteRequest.Email;
            clienteExistente.Telefone = clienteRequest.Telefone;

            await _context.SaveChangesAsync();

        var response = new ClienteResponse
        {
            Id = clienteExistente.Id,
            RazaoSocial = clienteExistente.RazaoSocial,
            NomeFantasia = clienteExistente.NomeFantasia,
            Cnpj = clienteExistente.Cnpj,
            Email = clienteExistente.Email,
            Telefone = clienteExistente.Telefone,
            Status = clienteExistente.Status,
            CriadoEm = clienteExistente.CriadoEm
        };
        
        return response;
    }
    public async Task<bool>DeleteCliente(int id)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(u => u.Id == id);
        if(cliente == null)
        {
            return false;
        }
        cliente.Status = "CANCELADO";

        await _context.SaveChangesAsync();

        return true;
    }
}