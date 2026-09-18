using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;

[ApiController]
[Route("usuarios")]

public class UsuarioController : ControllerBase
{   
    private readonly UsuarioService _usuarioService;
    private readonly JwtService _jwtService;
    private readonly UsuarioLogadoService _usuarioLogadoService;
    public UsuarioController(UsuarioService usuarioService, JwtService jwtService, UsuarioLogadoService usuarioLogadoService)
    {
        _usuarioService = usuarioService;      
        _jwtService = jwtService;  
        _usuarioLogadoService = usuarioLogadoService;
    }

    [HttpGet][Authorize]
    public async Task <IActionResult> GetUsuarios()
    {
        var usuarios = await _usuarioService.GetUsuarios();

        if(usuarios.Count == 0)
        {
            return NotFound(new
            {
                Mensagem = "Nenhum usuário encontrado."
            });
        }

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetUsuario(int id)
    {
            var usuario = await _usuarioService.GetUsuarioId(id);
        if(usuario == null)
        {
            return NotFound(new
            {
                mensagem = "Usuário não encontrado."
            });
        }
            return Ok(usuario);
    }

    [HttpPost]
    public async Task <IActionResult> PostUsuario(UsuarioRequest usuarioRequest)
    {
       var usuario = await _usuarioService.PostUsuario(usuarioRequest);

        return CreatedAtAction("GetUsuario", new { id = usuario.Id }, new
        {
            mensagem = "Usuario cadastrado com sucesso.", usuario
        });
        
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> PutUsuario(int id, UsuarioRequest usuarioRequest)
    {
        var usuarioAlterado = await _usuarioService.PutUsuario(id, usuarioRequest);
       
       if(usuarioAlterado == null)
        {
        return NotFound(new
        {
            mensagem = "Usuário não encontrado."
        });

        }
        return Ok(usuarioAlterado);


    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuarioExistente = await _usuarioService.DeleteUsuario(id);
        if(usuarioExistente == false)
        {
            return NotFound(new
            {
                mensagem = "Usuário não encontrado."
            });
        }

        return Ok(new
        {
            mensagem = "Usuário deletado com sucesso!"
        });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var usuario = await _usuarioService.Login(loginRequest);

        if(usuario == null)
        {
            return Unauthorized(new
            {
                mensagem = "E-mail ou senha inválidos."
            });
        }

        var token = _jwtService.GerarToken(usuario);

        return Ok(new
        {
            mensagem = "Login realizado com sucesso.",
            token,
            usuario = new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone
            }
        });
    }
    [HttpGet("me")][Authorize]
    public async Task<IActionResult> GetUsuarioLogado()
    {
        var usuarioId = _usuarioLogadoService.GetUsuarioId();
         
        var usuario = await _usuarioService.GetUsuarioId(usuarioId);

        if(usuario == null)
        {
            return NotFound(new
            {
                mensagem = "Usuário não encontrado."
            });
        }
        return Ok(usuario);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("admin")]
    public IActionResult AreaAdmin()
    {
        return Ok(new
        {
            mensagem = "Você possui acesso de administrador."
        });
    }
    


}