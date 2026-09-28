public class IdempotencyResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public PagamentoResponse Response { get; set; } = null!;
}