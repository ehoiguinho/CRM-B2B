using System.ComponentModel.DataAnnotations;

public class FaturaStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}

