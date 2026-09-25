using System.ComponentModel.DataAnnotations;

public class OportunidadeItemRequest
{
    [Required]
    public int ProdutoServicoId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; }
}
