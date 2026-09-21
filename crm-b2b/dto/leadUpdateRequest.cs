using System.ComponentModel.DataAnnotations;

public class LeadUpdateRequest
{
    [Required]
    [StringLength(20)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Empresa { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string Origem { get; set; } = string.Empty;

    public string? Observacao { get; set; }
}