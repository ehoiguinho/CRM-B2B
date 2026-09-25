using System.ComponentModel.DataAnnotations;

public class PagamentoRequest
{
    [Required]
    public int FaturaId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [Required]
    public DateTime DataPagamento { get; set; }

    [Required]
    [StringLength(30)]
    public string FormaPagamento { get; set; } = string.Empty;
}

