using System.ComponentModel.DataAnnotations;

public class AtividadeStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}