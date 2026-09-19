using System.ComponentModel.DataAnnotations;

public class ContatoRequest
{
    public int ClienteId {get; set;}
    [Required]
    [StringLength (30, MinimumLength = 10)]
    public string Nome {get; set;} = string.Empty;

    [Required][EmailAddress]
    public string Email {get; set;} = string.Empty;
    public string? Telefone {get; set;}
    
    [Required]
    [StringLength (30, MinimumLength = 10)]
    public string Cargo {get; set;} = string.Empty;
}