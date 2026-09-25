public class InadimplenciaResponse
{
    public int Id { get; set; }

    public int FaturaId { get; set; }

    public string NumeroFatura { get; set; } = string.Empty;

    public decimal ValorEmAberto { get; set; }

    public DateTime DataInadimplencia { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? DataRegularizacao { get; set; }

    public DateTime CriadoEm { get; set; }
}