namespace CleanArchitecture.Application.Models;

//Estructura solicitada por MailerSend
public class Email
{
    public DireccionEmail From { get; set; } = new();
    public List<DireccionEmail> To { get; set; } = [];
    public string? Subject { get; set; }
    public string html { get; set; } = string.Empty;
}

public class DireccionEmail
{
    public string Email { get; set; } = string.Empty;
}
