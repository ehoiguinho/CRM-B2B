public class LeadResponse
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Empresa { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    public string Origem { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? Observacao { get; set; }

    public DateTime CriadoEm { get; set; }
}