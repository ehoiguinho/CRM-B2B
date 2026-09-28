
public class Idempotencia
{
    public int Id { get; set; }
    public string Chave { get; set; } = string.Empty;
    public string RequestHash { get; set; } = string.Empty;
    public int StatusCode { get; set;}
    public string ResponseBody { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}