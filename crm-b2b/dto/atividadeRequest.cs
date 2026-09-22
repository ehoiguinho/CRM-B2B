using System.ComponentModel.DataAnnotations;

public class AtividadeRequest
{
    [Required]
    public int OportunidadeId { get; set; }

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