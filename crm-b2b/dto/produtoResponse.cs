public class ProdutoResponse
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public bool Ativo { get; set; }

    public DateTime CriadoEm { get; set; }
}