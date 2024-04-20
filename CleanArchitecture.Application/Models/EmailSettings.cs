namespace CleanArchitecture.Application.Models;

public class EmailSettings
{
    public string Url { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
}
