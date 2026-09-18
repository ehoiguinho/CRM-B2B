

public class Cliente
{
    public int Id {get; set;} 
    public string RazaoSocial {get; set;} = string.Empty;
    public string NomeFantasia {get; set;} = string.Empty;
    public string Cnpj {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string? Telefone {get; set;}
    public string Status {get; set;} = "ATIVO";
    public DateTime CriadoEm {get; set;}

}