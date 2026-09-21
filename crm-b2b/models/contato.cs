

public class Contato
{
    public int Id {get; set;}
    public Cliente Cliente {get; set;} = null!;
    public int ClienteId {get; set;}
    public string Nome {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string? Telefone {get; set;}
    public string Cargo {get; set;} = string.Empty;
    public DateTime CriadoEm {get; set;}
}