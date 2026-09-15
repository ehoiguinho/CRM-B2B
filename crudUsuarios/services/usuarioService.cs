using Microsoft.EntityFrameworkCore;

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

}