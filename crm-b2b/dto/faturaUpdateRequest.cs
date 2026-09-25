using System.ComponentModel.DataAnnotations;

public class FaturaUpdateRequest
{
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
