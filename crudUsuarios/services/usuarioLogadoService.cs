using System.Security.Claims;

public class UsuarioLogadoService
{
    private readonly IHttpContextAccessor _httpContextAcessor;
    public UsuarioLogadoService (IHttpContextAccessor httpContextAcessor)
    {
        _httpContextAcessor = httpContextAcessor;
    }
    public int GetUsuarioId()
    {
        var usuarioId = _httpContextAcessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.Parse(usuarioId!);
    }
}