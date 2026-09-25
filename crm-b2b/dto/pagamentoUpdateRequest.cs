using System.ComponentModel.DataAnnotations;

public class PagamentoUpdateRequest
{
    [Required]
    public DateTime DataPagamento { get; set; }

    [Required]
    [StringLength(30)]
    public string FormaPagamento { get; set; } = string.Empty;
}

