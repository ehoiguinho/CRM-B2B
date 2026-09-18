using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("clientes")]
public class ClienteController : ControllerBase{

    private readonly ClienteService _clienteService;

    public ClienteController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }
    [HttpPost][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>PostCliente(ClienteRequest clienteRequest)
    {
        var cliente = await _clienteService.PostCliente(clienteRequest);
        return CreatedAtAction("GetCliente", new {id = cliente.Id}, new
        {
            mensagem = "Cliente cadastrado com sucesso.", cliente
        });
        
    }
    [HttpGet][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>GetClientes()
    {
        var cliente = await _clienteService.GetClientes();
        {
            if(cliente.Count == 0)
            {
                return NotFound(new
                {
                    mensagem = "Nenhum cliente encontrado."
                });
            }
            
            return Ok(cliente);
            
        }
    }
    [HttpGet("{id}")][Authorize(Roles = "ADMIN")]

    public async Task<IActionResult>GetCliente(int id)
    {
        var cliente = await _clienteService.GetClienteId(id);
        if(cliente == null)
        {
            return NotFound(new
            {
                mensagem = "Cliente não encontrado."
            });
        }
        
        return Ok(cliente);
    }
    [HttpPut("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>PutCliente(int id, ClienteRequest clienteRequest)
    {
        var clienteAlterado = await _clienteService.PutCliente(id, clienteRequest);
        if(clienteAlterado == null){
            return NotFound(new
            {
                mensagem = "Cliente não encontrado."
            });
        }

        return Ok(clienteAlterado);
    }
    [HttpDelete("{id}")][Authorize(Roles = "ADMIN")]
    public async Task<IActionResult>DeleteCliente(int id)
    {
        var clienteCancelado = await _clienteService.DeleteCliente(id);
        if(clienteCancelado == null)
        {
            return NotFound(new
            {
                mensagem = "Cliente não encontrado."
            });
        }

        return Ok(new
        {
            mensagem = "Cliente cancelado com sucesso."
            
        });
    }
}