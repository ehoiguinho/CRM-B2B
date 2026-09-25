public class Inadimplencia
{
    public int Id { get; set; }

    public int FaturaId { get; set; }

    public decimal ValorEmAberto { get; set; }

    public DateTime DataInadimplencia { get; set; }

    public string Status { get; set; } = "ABERTA";

    public DateTime? DataRegularizacao { get; set; }

    public DateTime CriadoEm { get; set; }

    public Fatura Fatura { get; set; } = null!;
}