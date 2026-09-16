using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("usuarios")]

public class UsuarioController : ControllerBase
{   
    private readonly UsuarioService _usuarioService;
    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;        
    }
    [HttpGet]
    public async Task <IActionResult> GetUsuarios()
    {
        var usuarios = await _usuarioService.GetUsuarios();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetUsuario(int id)
    {
            var usuario = await _usuarioService.GetUsuarioId(id);
        if(usuario == null)
        {
            return NotFound();
        }
            return Ok(usuario);
    }

    [HttpPost]
    public async Task <IActionResult> PostUsuario(UsuarioRequest usuarioRequest)
    {
       var usuario = await _usuarioService.PostUsuario(usuarioRequest);

        return CreatedAtAction("GetUsuario", new { id = usuario.Id}, usuario);
        
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> PutUsuario(int id, UsuarioRequest usuarioRequest)
    {
        var usuarioAlterado = await _usuarioService.PutUsuario(id, usuarioRequest);
       
       if(usuarioAlterado == null)
        {
        return NotFound();

        }
        return Ok(usuarioAlterado);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuarioExistente = await _usuarioService.DeleteUsuario(id);
        if(usuarioExistente == false)
        {
            return NotFound();
        }

        return NoContent();
    }

    


}