using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.Data;

namespace CulinaryCommand.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        // constructor
        public EmailSender(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var apiKey = _config["Resend:ApiKey"];
            var fromEmail = "no-reply@culinary-command.com";

            var payload = new
            {
                from = fromEmail,
                to = new[] {toEmail},
                subject = subject,
                html = body
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Email send failed: {error}");
            }
        }
    }
}