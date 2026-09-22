using System.ComponentModel.DataAnnotations;

public class OportunidadeEtapaRequest
{
    [Required]
    public string Etapa { get; set; } = string.Empty;
}