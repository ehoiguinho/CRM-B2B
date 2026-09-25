using System.ComponentModel.DataAnnotations;

public class InadimplenciaStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}