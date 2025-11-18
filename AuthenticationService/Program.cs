using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Hent config
var connStr = builder.Configuration.GetConnectionString("MySql")
              ?? Environment.GetEnvironmentVariable("ConnectionStrings__MySql");
var jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("Jwt__Key")!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("Jwt__Issuer")!;
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("Jwt__Audience")!;

// Controllers (til evt. UsersController senere)
builder.Services.AddControllers();

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

// Endpoint: register
app.MapPost("/auth/register", async (AuthRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
        return Results.Json(new { Error = "Username/password required" }, statusCode: 400);

    using var conn = new MySqlConnection(connStr);
    await conn.OpenAsync();

    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(req.Password)));

    using var cmd = conn.CreateCommand();
    cmd.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES (@u, @p)";
    cmd.Parameters.AddWithValue("@u", req.Username);
    cmd.Parameters.AddWithValue("@p", hash);

    try
    {
        await cmd.ExecuteNonQueryAsync();
        return Results.Json(new { Success = true, Username = req.Username });
    }
    catch (MySqlException ex) when (ex.Number == 1062)
    {
        return Results.Json(new { Error = "Username already exists" }, statusCode: 409);
    }
});

// Endpoint: login
app.MapPost("/auth/login", async (AuthRequest req) =>
{
    using var conn = new MySqlConnection(connStr);
    await conn.OpenAsync();

    var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(req.Password)));

    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT Id FROM Users WHERE Username=@u AND PasswordHash=@p";
    cmd.Parameters.AddWithValue("@u", req.Username);
    cmd.Parameters.AddWithValue("@p", hash);

    var result = await cmd.ExecuteScalarAsync();
    if (result is null)
        return Results.Json(new { Error = "Invalid username or password" }, statusCode: 401);

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new System.Security.Claims.Claim("sub", req.Username),
        new System.Security.Claims.Claim("sid", result.ToString()!)
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


// Endpoint: get all users
app.MapGet("/auth/users", async () =>
{
    var users = new List<object>();

    using var conn = new MySqlConnection(connStr);
    await conn.OpenAsync();

    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT Id, Username FROM Users";

    using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        users.Add(new {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1)
        });
    }

    return Results.Json(users, new JsonSerializerOptions
{
    PropertyNamingPolicy = null // behold Id/Username
});

    
});

app.MapGet("/ping", () => $"Hello from {Environment.MachineName}");

// Map controllers (så du kan tilføje UsersController senere)
app.MapControllers();

app.Run();