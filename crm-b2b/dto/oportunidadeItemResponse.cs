public class OportunidadeItemResponse
{
    public int Id { get; set; }

    public int OportunidadeId { get; set; }

    public int ProdutoServicoId { get; set; }

    public string ProdutoServicoNome { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal ValorUnitario { get; set; }

    public decimal ValorTotal { get; set; }
}
