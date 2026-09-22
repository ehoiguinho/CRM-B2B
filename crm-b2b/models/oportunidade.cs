public class Oportunidade
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public decimal Valor { get; set; }

    public string Etapa { get; set; } = "ABERTA";

    public DateTime CriadoEm { get; set; }

    public Cliente Cliente { get; set; } = null!;
}