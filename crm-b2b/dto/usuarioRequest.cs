using System.ComponentModel.DataAnnotations;

public class UsuarioRequest
{   
    [Required]
    [StringLength(20)]
    public string Nome {get; set;} = string.Empty;

    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Required]
    [StringLength(15, MinimumLength = 5)]
    public string Senha {get; set;} = string.Empty;

    public string? Telefone {get; set;}
}