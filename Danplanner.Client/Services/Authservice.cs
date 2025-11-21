using Danplanner.Shared.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Danplanner.Client.Services
{
    public class Authservice
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private string? _token;

        public Authservice(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("Auth");
        }

        // ---------- REGISTER ----------
        // ---------- REGISTER ----------
        public async Task<bool> RegisterAsync(
            string password,
            string name,
            string email,
            string address,
            string phone,
            string country,
            string language)
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

            return response.IsSuccessStatusCode;
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

            if (!response.IsSuccessStatusCode) return null;

            var result = JsonSerializer.Deserialize<LoginResponse>(body, _jsonOptions);
            _token = result?.Token; // 👈 token gemmes korrekt

            Console.WriteLine($"DEBUG Parsed Token: {_token}");

            return _token;
        }



        // ---------- GET USERS ----------
        public async Task<List<UserDto>> GetUsersAsync()
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("Ingen token – log ind først!");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);

            var users = await _http.GetFromJsonAsync<List<UserDto>>("auth/users", _jsonOptions);
            return users ?? new List<UserDto>();
        }
    }

    // ---------- DTOs ----------
    public class AuthRequest
    {
        public string Email { get; set; } = string.Empty;   // login-identitet
        public string Password { get; set; } = string.Empty;   // adgangskode

        // Nye felter til campist
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

}