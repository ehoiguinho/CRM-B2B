public class AtividadeResponse
{
    public int Id { get; set; }

    public int OportunidadeId { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public DateTime DataAgendada { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool Ativo { get; set; }  

    public DateTime CriadoEm { get; set; }
}