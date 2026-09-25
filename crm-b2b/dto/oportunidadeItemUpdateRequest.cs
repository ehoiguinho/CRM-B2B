using System.ComponentModel.DataAnnotations;

public class OportunidadeItemUpdateRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; }
}
