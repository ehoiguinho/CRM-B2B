using System.ComponentModel.DataAnnotations;

public class LeadStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}