using Danplanner.Shared.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Danplanner.Client.Services
{
    public class Authservice(IHttpClientFactory factory)
    {
        private readonly HttpClient _http = factory.CreateClient("Auth");
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private string? _token;

        // Event til UI-opdatering
        public event Action? OnUserChanged;

        public CurrentUser? CurrentUser { get; private set; }

        // 👇 Property til beskeder
        public string? LastMessage { get; private set; }

        private void RaiseUserChanged()
        {
            OnUserChanged?.Invoke();
        }

        // ---------- REGISTER ----------
        public async Task<int?> RegisterAsync(string password, string name, string email,
            string address, string phone, string country, string language)
        {
            var request = new AuthRequest
            {
                Password = password,
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                Country = country,
                Language = language
            };

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("auth/register", content);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"DEBUG Register: Status={response.StatusCode}, Body={body}");

            if (!response.IsSuccessStatusCode) return null;

            using var doc = JsonDocument.Parse(body);
            return doc.RootElement.TryGetProperty("id", out var idProp) ? idProp.GetInt32() : (int?)null;
        }

        public async Task<bool> RegisterOkAsync(string password, string name, string email,
            string address, string phone, string country, string language)
        {
            var id = await RegisterAsync(password, name, email, address, phone, country, language);
            return id.HasValue && id.Value > 0;
        }

        // ---------- LOGIN ----------
        public async Task<string?> LoginAsync(string email, string password)
        {
            var request = new AuthRequest { Email = email, Password = password };
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("auth/login", content);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"DEBUG Login: Status={response.StatusCode}, Body={body}");

            if (!response.IsSuccessStatusCode)
            {
                LastMessage = "Login failed";
                return null;
            }

            var result = JsonSerializer.Deserialize<LoginResponse>(body, _jsonOptions);
            _token = result?.Token;

            if (_token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(_token);

                var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? email;
                var roleClaim = jwt.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.Role || c.Type == "role")?.Value ?? "";

                CurrentUser = new CurrentUser
                {
                    Email = emailClaim,
                    Role = roleClaim
                };

                // 👇 Gem beskeden permanent
                LastMessage = "Login successful";

                Console.WriteLine($"DEBUG CurrentUser: {CurrentUser.Email}, Role={CurrentUser.Role}");

                RaiseUserChanged();
            }

            return _token;
        }

        // ---------- GET USERS ----------
        public async Task<List<UserDto>> GetUsersAsync()
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);

            var response = await _http.GetAsync("auth/users");
            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException();

            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>(_jsonOptions);
            return users ?? new List<UserDto>();
        }
    }

    // ---------- DTOs ----------
    public class AuthRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }

    public class CurrentUser
    {
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
    }
}