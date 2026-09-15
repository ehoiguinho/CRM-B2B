using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("usuarios")]

public class UsuarioController : ControllerBase
{   
    private readonly AppDbContext _context;

    public UsuarioController(AppDbContext context)
    {
        _context = context;
        
    }
    [HttpGet]
    public async Task <IActionResult> GetUsuario()
    {
        var usuarios = await _context.Usuarios.ToListAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetUsuario(int id)
    {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if(usuario == null)
        {
            return NotFound();
        }
            return Ok(usuario);
    }
    [HttpPost]
    public async Task <IActionResult> PostUsuario(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUsuario", new { id = usuario.Id}, usuario);
        
    }
    [HttpPut("{id}")]
    public async Task <IActionResult> PutUsuario(int id, Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync( u => u.Id == id);
        if(usuarioExistente  == null)
        {
            return NotFound();
        }
        // usuario que veio da url é comparado com o usuario encontrado no banco
        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.Senha = usuario.Senha;
        usuarioExistente.Telefone = usuario.Telefone;

        await _context.SaveChangesAsync();

        return Ok(usuarioExistente);

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync( u => u.Id == id);
        if(usuarioExistente == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuarioExistente);

        await _context.SaveChangesAsync();

        return Ok();
    }

    


}