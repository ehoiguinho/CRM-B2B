using System.ComponentModel.DataAnnotations;

public class ContratoStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}