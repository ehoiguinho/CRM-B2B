using System.ComponentModel.DataAnnotations;

public class ClienteUpdateRequest
{
    [Required]
    [StringLength(30)]
    public string RazaoSocial { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string NomeFantasia { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }
}