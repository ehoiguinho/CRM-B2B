using System.ComponentModel.DataAnnotations;

public class ContratoRequest
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    [StringLength(50)]
    public string Numero { get; set; } = string.Empty;

    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [Required]
    public DateTime DataInicio { get; set; }

    [Required]
    public DateTime DataFim { get; set; }
}