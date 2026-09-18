using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

public class LoginRequest
{
    [Required]
    [EmailAddress]
     public string Email {get; set;} = string.Empty;

     [Required]
     public string Senha {get; set;} = string.Empty;
}