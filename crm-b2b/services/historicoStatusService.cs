
public class HistoricoStatusService
{
    private readonly AppDbContext _context;
    private readonly UsuarioLogadoService _usuarioLogadoService;
    public HistoricoStatusService(AppDbContext context, UsuarioLogadoService usuarioLogadoService)
    {
        _context = context;
        _usuarioLogadoService = usuarioLogadoService;
    }

    public async Task Registrar(string entidade, int entidadeId, string? statusAnterior, string statusNovo)
    {
        var usuarioId = _usuarioLogadoService.GetUsuarioId();

        var historico = new HistoricoStatus
        {
            Entidade = entidade,
            EntidadeId = entidadeId,
            StatusAnterior = statusAnterior,
            StatusNovo = statusNovo,
            UsuarioId = usuarioId
        };

        await _context.HistoricoStatus.AddAsync(historico);
    }
}