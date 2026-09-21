using System.ComponentModel.DataAnnotations;

public class ClienteRequest

{   [Required]
    [StringLength (30)]
    public string RazaoSocial {get; set;} = string.Empty;

    [Required]
    [StringLength (30)]

    public string NomeFantasia {get; set;} = string.Empty;

    [Required]
    [RegularExpression(@"^\d{14}$")]

    public string Cnpj {get; set;} = string.Empty;

    [Required][EmailAddress]
    public string Email {get; set;} = string.Empty;

    public string? Telefone {get; set;}
    
   
}