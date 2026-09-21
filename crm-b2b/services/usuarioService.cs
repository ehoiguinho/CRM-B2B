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

        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuarioRequest.Email);
        if (emailExiste)
        {
            throw new BusinessException("Esse E-mail já foi cadastrado.", 409);
        }

        var usuario = new Usuario
        {
            Nome = usuarioRequest.Nome,
            Email = usuarioRequest.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(usuarioRequest.Senha),
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
    public async Task <UsuarioResponse?>PutUsuario(int id, UsuarioRequest usuarioRequest)
    {
        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuarioRequest.Email && u.Id == id);
        if(emailExiste)
        {
            throw new BusinessException("E-mail já cadastrado.", 409);
        }
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id != id);
        
        if (usuarioExistente == null)
        {
            return null;
        }

        usuarioExistente.Nome = usuarioRequest.Nome;
        usuarioExistente.Email = usuarioRequest.Email;
        usuarioExistente.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioRequest.Senha);
        usuarioExistente.Telefone = usuarioRequest.Telefone;

        await _context.SaveChangesAsync();

        var response = new UsuarioResponse
        {
            Id = usuarioExistente.Id,
            Nome = usuarioExistente.Nome,
            Email = usuarioExistente.Email,
            Telefone = usuarioExistente.Telefone
        };

        return response;
    }
    public async Task <bool>DeleteUsuario(int id)
    {
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if(usuarioExistente == null)
        {
            return false;
            
        }
       
        _context.Usuarios.Remove(usuarioExistente);
        await _context.SaveChangesAsync();

        return true;
        
    }

    public async Task <Usuario?>Login(LoginRequest loginRequest)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (usuario == null)
        {
            return null;
        }

        var senhaValida = BCrypt.Net.BCrypt.Verify(loginRequest.Senha, usuario.Senha);

        if (!senhaValida)
        {
            return null;
        }

        return usuario;
    }

}