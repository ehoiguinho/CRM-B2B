using System.ComponentModel.DataAnnotations;

public class ContatoUpdateRequest
{
    [Required]
    [StringLength(20)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    [Required]
    public string Cargo { get; set;} = string.Empty;
}