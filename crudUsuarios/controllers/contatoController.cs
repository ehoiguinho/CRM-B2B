using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("contatos")]
public class ContatoController : ControllerBase
{
    private readonly ContatoService _contatoService;

    public ContatoController(ContatoService contatoService)
    {
        _contatoService = contatoService;
    }
    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>PostContato(ContatoRequest contatoRequest)
    {
        var contato = await _contatoService.PostContato(contatoRequest);
        return CreatedAtAction("getContatos", new {id = contato.Id}, new
        {
            mensagem = "Cliente cadastrado com sucesso.", contato
        });
        
    }

    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetContatos()
    {
        var contatos = await _contatoService.GetContato();
        if(contatos.Count == 0)
        {
            return NotFound(new
            {
                mensagem = "Nenhum contato encontrado."
            });
        }
        return Ok(contatos);
    }

    [HttpGet("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>GetContato(int id)
    {
        var contato = await _contatoService.GetContatoId(id);
        if(contato == null)
        {
            return NotFound(new
            {
                mensagem = "Contato não encontrado."
            });
        }
        return Ok(contato);
    }
    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>PutContato(int id, ContatoUpdateRequest contatoRequest)
    {
        var contatoAlterado = await _contatoService.PutContato(id, contatoRequest);
        if(contatoAlterado == null)
        {
            return NotFound(new
            {
                mensagem = "Contato não encontrado."
            });

        }
        return Ok(contatoAlterado);
    }

    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>DeleteContato(int id)
    {
        var contatoDeletado = await _contatoService.DeleteContato(id);
        if(contatoDeletado == false)
        {
            return NotFound(new
            {
                mensagem = "Contato não encontrado."
            });
        }
            return Ok(new
            {
            mensagem = "Contato deletado com sucesso."
            }
            
        );
    }
}