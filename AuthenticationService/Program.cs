using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using AuthenticationService.Models; // DTO'er ligger her

var builder = WebApplication.CreateBuilder(args);

// Global camelCase JSON
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Hent config
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
              ?? Environment.GetEnvironmentVariable("ConnectionStrings__MySql");
var jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("Jwt__Key")!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("Jwt__Issuer")!;
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("Jwt__Audience")!;

// JWT setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
app.UseAuthentication();
app.UseAuthorization();

// Seed admin user før app.Run()
await SeedAdmin(connStr);

app.MapPost("/auth/register", async (AuthRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
        return Results.Json(new { Error = "Email/password required" }, statusCode: 400);

    using var conn = new MySqlConnection(connStr);
    await conn.OpenAsync();

    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(req.Password)));

    using var cmd = conn.CreateCommand();
    cmd.CommandText = @"INSERT INTO Users 
        (Email, PasswordHash, Name, Address, Phone, Country, Language, Role) 
        VALUES (@e, @p, @n, @a, @ph, @c, @l, 'Customer')";
    cmd.Parameters.AddWithValue("@e", req.Email);
    cmd.Parameters.AddWithValue("@p", hash);
    cmd.Parameters.AddWithValue("@n", req.Name);
    cmd.Parameters.AddWithValue("@a", req.Address);
    cmd.Parameters.AddWithValue("@ph", req.Phone);
    cmd.Parameters.AddWithValue("@c", req.Country);
    cmd.Parameters.AddWithValue("@l", req.Language);

    try
    {
        await cmd.ExecuteNonQueryAsync();
        return Results.Json(new { Success = true, Email = req.Email });
    }
    catch (MySqlException ex) when (ex.Number == 1062)
    {
        return Results.Json(new { Error = "Email already exists" }, statusCode: 409);
    }
});

app.MapPost("/auth/login", async (AuthRequest req) =>
{
    using var conn = new MySqlConnection(connStr);
    await conn.OpenAsync();

    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(req.Password)));

    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT Id, Role FROM Users WHERE Email=@e AND PasswordHash=@p";
    cmd.Parameters.AddWithValue("@e", req.Email);
    cmd.Parameters.AddWithValue("@p", hash);

    using var reader = await cmd.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
        return Results.Json(new { Error = "Invalid email or password" }, statusCode: 401);

    var userId = reader.GetInt32(0);
    var role = reader.GetString(1);

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new System.Security.Claims.Claim("sub", req.Email),
        new System.Security.Claims.Claim("sid", userId.ToString()),
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role)
    };

    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: jwtIssuer,
        audience: jwtAudience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(12),
        signingCredentials: creds);

    var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);

    return Results.Json(new LoginResponse(jwt));
});

app.MapGet("/auth/users", [Authorize(Roles = "Admin")] async () =>
{
    var users = new List<User>();

    using var conn = new MySqlConnection(connStr);
    await conn.OpenAsync();

    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT Id, Email, Name, Address, Phone, Country, Language, Role FROM Users";

    using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        users.Add(new User
        {
            Id = reader.GetInt32(0),
            Email = reader.GetString(1),
            Name = reader.GetString(2),
            Address = reader.GetString(3),
            Phone = reader.GetString(4),
            Country = reader.GetString(5),
            Language = reader.GetString(6),
            Role = reader.GetString(7)
        });
    }

    return Results.Json(users);
});

app.MapGet("/ping", () => $"Hello from {Environment.MachineName}");
app.MapControllers();

app.Run();

// Seed-metode
static async Task SeedAdmin(string connStr)
{
    Console.WriteLine("[INIT] SeedAdmin starting...");
    if (string.IsNullOrWhiteSpace(connStr))
    {
        Console.WriteLine("[INIT][ERROR] Connection string is null/empty.");
        return;
    }

    const int maxAttempts = 20;
    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();

            using var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Email='admin@example.com'";
            var exists = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());

            if (exists == 0)
            {
                var rng = RandomNumberGenerator.Create();
                var bytes = new byte[20];
                rng.GetBytes(bytes);
                var plainPassword = Convert.ToBase64String(bytes);

                var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(plainPassword)));

                using var insertCmd = conn.CreateCommand();
                insertCmd.CommandText = @"INSERT INTO Users 
    (Email, PasswordHash, Name, Address, Phone, Country, Language, Role) 
    VALUES ('admin@example.com', @p, 'Admin User', '', '', '', '', 'Admin')";
                insertCmd.Parameters.AddWithValue("@p", hash);
                await insertCmd.ExecuteNonQueryAsync();

                Console.WriteLine($"[INIT] Admin user created with email=admin@example.com and password: {plainPassword}");
            }
            else
            {
                Console.WriteLine("[INIT] Admin already exists.");
            }

            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[INIT][WARN] Attempt {attempt}/{maxAttempts} failed: {ex.Message}");
            await Task.Delay(1500);
        }
    }

    Console.WriteLine("[INIT][ERROR] Could not connect to MySQL after retries.");
}