public class ProdutoServico
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public string Tipo { get; set; } = "SERVICO";

    public decimal Valor { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }
}