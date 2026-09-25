public class Pagamento
{
    public int Id { get; set; }

    public int FaturaId { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public string FormaPagamento { get; set; } = string.Empty;

    public string Status { get; set; } = "CONFIRMADO";

    public DateTime CriadoEm { get; set; }

    public Fatura Fatura { get; set; } = null!;
}
