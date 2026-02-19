using System.Net.Http.Headers;
using System.Text.Json;

namespace CulinaryCommand.Vendor.Services
{
    public record LogoDevResult(string Name, string Domain, string? Logo_Url);

    public class LogoDevService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _config;

        public LogoDevService(IHttpClientFactory httpFactory, IConfiguration config)
        {
            _httpFactory = httpFactory;
            _config = config;
        }

        public async Task<List<LogoDevResult>> SearchAsync(string query, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                return new();

            var secretKey = _config["LogoDev:SecretKey"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(secretKey))
                return new();

            var client = _httpFactory.CreateClient("LogoDev");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", secretKey);

            var response = await client.GetAsync(
                $"https://api.logo.dev/search?q={Uri.EscapeDataString(query)}&strategy=suggest", ct);

            if (!response.IsSuccessStatusCode)
                return new();

            var json = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<List<LogoDevResult>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }

        public string BuildLogoUrl(string domain, string publishableKey) =>
            $"https://img.logo.dev/{domain}?token={publishableKey}&size=64&format=webp";
    }
}
