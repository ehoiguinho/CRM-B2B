

public class Usuario
{
    public int Id {get; set; }
    public string Nome {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Senha {get; set;} = string.Empty;
    public string? Telefone {get; set;}
    public string Perfil {get; set;} = "USER";

}