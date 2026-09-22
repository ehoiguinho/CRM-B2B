public class Atividade
{
    public int Id { get; set; }

    public int OportunidadeId { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public DateTime DataAgendada { get; set; }

    public string Status { get; set; } = "PENDENTE";
    
    public DateTime CriadoEm { get; set; }

    public bool Ativo { get; set; } = true;

    public Oportunidade Oportunidade { get; set; } = null!;
}