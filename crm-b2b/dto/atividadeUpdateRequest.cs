using System.ComponentModel.DataAnnotations;

public class AtividadeUpdateRequest
{
    [Required]
    [StringLength(30)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    [Required]
    public DateTime DataAgendada { get; set; }
}