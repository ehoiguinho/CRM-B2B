using System.ComponentModel.DataAnnotations;

public class ProdutoRequest
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    [Required]
    public string Tipo { get; set; } = "SERVICO";

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }
}