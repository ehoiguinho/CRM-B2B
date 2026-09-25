using System.ComponentModel.DataAnnotations;

public class FaturaRequest
{
    [Required]
    public int ContratoId { get; set; }

    [Required]
    [StringLength(50)]
    public string Numero { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Descricao { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [Required]
    public DateTime DataEmissao { get; set; }

    [Required]
    public DateTime DataVencimento { get; set; }
}
