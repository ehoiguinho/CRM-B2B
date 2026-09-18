using System.ComponentModel.DataAnnotations;

public class UsuarioRequest
{   
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Nome {get; set;} = string.Empty;
    [Required]
    [EmailAddress]
    [StringLength(50, MinimumLength = 10)]
    public string Email {get; set;} = string.Empty;
    [Required]
    [StringLength(15, MinimumLength = 5)]
    public string Senha {get; set;} = string.Empty;

    public string? Telefone {get; set;}
}