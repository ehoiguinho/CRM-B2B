using System.ComponentModel.DataAnnotations;

public class PagamentoStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
