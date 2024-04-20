using CleanArchitecture.Application.Contracts.Infraestructure;
using CleanArchitecture.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CleanArchitecture.Infraestructure.Email;

public class EmailService : IEmailService
{
    public EmailSettings _emailSettings { get; }
    public ILogger<EmailService> _logger { get; }

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmail(Application.Models.Email email)
    {
        email.From.Email = _emailSettings.FromAddress;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        string data = JsonSerializer.Serialize(email, options);

        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _emailSettings.Token);

        StringContent? content = new StringContent(data, Encoding.UTF8, "application/json");

        HttpResponseMessage? respuesta = await client.PostAsync(_emailSettings.Url, content);

        if (respuesta.IsSuccessStatusCode)
        {
            return true;
        }

        string error = await respuesta.Content.ReadAsStringAsync();
        string mensajeError = $"Error al enviar correo: {respuesta.StatusCode} {error}";
        _logger.LogError(mensajeError);
        return false;
    }
}
