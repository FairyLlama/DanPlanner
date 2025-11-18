// using System.Net.Http.Json;
// using System.Text;
// using System.Text.Json;
// using Danplanner.Shared.Models;

// namespace Danplanner.Client.Services
// {
//     public class Authservice
//     {
      
//             private readonly HttpClient _http;
//             private readonly JsonSerializerOptions _jsonOptions = new()
//             {
//                 PropertyNamingPolicy = null // 👈 behold PascalCase
//             };

//             public Authservice(IHttpClientFactory factory)
//             {
//                 // Brug den HttpClient vi har registreret som "Auth"
//                 _http = factory.CreateClient("Auth");
//             }

//             public async Task<bool> RegisterAsync(string username, string password)
//             {
//                 var request = new AuthRequest { Username = username, Password = password };

//                 var json = JsonSerializer.Serialize(request, _jsonOptions);
//                 Console.WriteLine($"DEBUG Payload: {json}");

//                 var content = new StringContent(json, Encoding.UTF8, "application/json");

//                 var response = await _http.PostAsync("auth/register", content);

//                 var body = await response.Content.ReadAsStringAsync();
//                 Console.WriteLine($"DEBUG Register: Status={response.StatusCode}, Body={body}");

//                 return response.IsSuccessStatusCode;
//             }

//             public async Task<string?> LoginAsync(string username, string password)
//             {
//                 var request = new AuthRequest { Username = username, Password = password };

//                 var json = JsonSerializer.Serialize(request, _jsonOptions);
//                 var content = new StringContent(json, Encoding.UTF8, "application/json");

//                 var response = await _http.PostAsync("auth/login", content);

//                 var body = await response.Content.ReadAsStringAsync();
//                 Console.WriteLine($"DEBUG Login: Status={response.StatusCode}, Body={body}");

//                 if (!response.IsSuccessStatusCode) return null;

//                 var result = JsonSerializer.Deserialize<LoginResponse>(body, _jsonOptions);
//                 return result?.token;
//             }

//             public async Task<List<UserDto>> GetUsersAsync()
//             {
//                 try
//                 {
//                     var users = await _http.GetFromJsonAsync<List<UserDto>>("auth/users", _jsonOptions);
//                     return users ?? new List<UserDto>();
//                 }
//                 catch (Exception ex)
//                 {
//                     Console.WriteLine($"DEBUG GetUsers failed: {ex.Message}");
//                     return new List<UserDto>();
//                 }
//             }
//         }

//         public class AuthRequest
//         {
//             public string Username { get; set; } = string.Empty;
//             public string Password { get; set; } = string.Empty;
//         }

//         public record LoginResponse(string token);

//     }

