using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

public class UsuarioService{

    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context){

        _context = context;
    }

    public async Task <List<UsuarioResponse>> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();

        var responses = usuarios.Select(u => new UsuarioResponse{

            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Telefone = u.Telefone
        }).ToList();

        return responses;
    }

    public async Task <UsuarioResponse?> GetUsuarioId(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if(usuario == null)
        {
            return null;
        }

        var response = new UsuarioResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Telefone = usuario.Telefone
        };

        return response;
    }
    public async Task <UsuarioResponse>PostUsuario(UsuarioRequest usuarioRequest)
    {

        var usuario = new Usuario
        {
            Nome = usuarioRequest.Nome,
            Email = usuarioRequest.Email,
            Senha = usuarioRequest.Senha,
            Telefone = usuarioRequest.Telefone
        };

        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        var response = new UsuarioResponse  
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Telefone = usuario.Telefone
        };

        return response;
        
    }
    public async Task <Usuario?>PutUsuario(int id, Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        
        if (usuarioExistente == null)
        {
            return null;
        }

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.Senha = usuario.Senha;
        usuarioExistente.Telefone = usuario.Telefone;

        await _context.SaveChangesAsync();

        return usuarioExistente;
    }
    public async Task <Usuario?>DeleteUsuario(int id)
    {
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if(usuarioExistente == null)
        {
            return null;
        }

        _context.Usuarios.Remove(usuarioExistente);

        await _context.SaveChangesAsync();

        return usuarioExistente;
        
    }

}