
public class HistoricoStatus
{
    public int Id {get; set;}
    public string Entidade {get; set;} = string.Empty;
    public int EntidadeId {get; set;}
    public string? StatusAnterior {get; set;}
    public string StatusNovo {get; set;} = string.Empty;
    public int UsuarioId {get; set;}
    public DateTime CriadoEm {get; set;}
    public  Usuario Usuario {get; set;} = null!;

}