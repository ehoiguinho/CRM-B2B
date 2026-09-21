using System.ComponentModel.DataAnnotations;

public class ContatoRequest
{
    public int ClienteId {get; set;}
    [Required]
    [StringLength (15)]
    public string Nome {get; set;} = string.Empty;

    [Required][EmailAddress]
    public string Email {get; set;} = string.Empty;
    public string? Telefone {get; set;}

    [Required]
    [StringLength (20)]
    public string Cargo {get; set;} = string.Empty;
}