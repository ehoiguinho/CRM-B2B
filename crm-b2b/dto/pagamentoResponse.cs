public class PagamentoResponse
{
    public int Id { get; set; }

    public int FaturaId { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public string FormaPagamento { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}
