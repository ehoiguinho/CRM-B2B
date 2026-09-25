public class ContratoResponse
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public string Numero { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Valor { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}