public class FaturaResponse
{
    public int Id { get; set; }

    public int ContratoId { get; set; }

    public string Numero { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataEmissao { get; set; }

    public DateTime DataVencimento { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}
