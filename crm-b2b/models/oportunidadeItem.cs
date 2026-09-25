public class OportunidadeItem
{
    public int Id { get; set; }

    public int OportunidadeId { get; set; }

    public int ProdutoServicoId { get; set; }

    public int Quantidade { get; set; }

    public decimal ValorUnitario { get; set; }

    public Oportunidade Oportunidade { get; set; } = null!;

    public ProdutoServico ProdutoServico { get; set; } = null!;
}
