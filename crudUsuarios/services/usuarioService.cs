using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

public class UsuarioService{

    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context){

        _context = context;
    }

    public async Task <List<Usuario>> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();

        return usuarios;
    }

    public async Task <Usuario?> GetUsuarioId(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
       
        return usuario;

    }
    public async Task <Usuario>PostUsuario(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);

        await _context.SaveChangesAsync();

        return usuario;
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