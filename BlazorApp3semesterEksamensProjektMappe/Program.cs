using BlazorApp3semesterEksamensProjektMappe.Client.Pages;
using BlazorApp3semesterEksamensProjektMappe.Client.Services;
using BlazorApp3semesterEksamensProjektMappe.Components;
using BlazorApp3semesterEksamensProjektMappe.Data;
using BlazorApp3semesterEksamensProjektMappe.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Blazor setup – Interactive render modes
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


// Controllers (REST API endpoints i serveren)

builder.Services.AddControllers();

// Database setup - SQL Server

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Database setup - MySQL
builder.Services.AddDbContext<MySqlDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 35)) 
    ));


//builder.Services.AddHttpClient("EF", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7225/");
//    // eller den port din server kører på
//});



builder.Services.AddHttpClient("EF", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["EfApiBaseAddress"]!);
});

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthApiBaseAddress"]!);
});

// Dine services
builder.Services.AddHttpClient();
builder.Services.AddSingleton<Authservice>();
builder.Services.AddScoped<ICampingSitesService, CampingSitesService>();
builder.Services.AddScoped<ICampingSiteDataService, CampingSiteDataService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingDataService, BookingDataService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDataService, ProductDataService>();
builder.Services.AddScoped<IHutService, HutService>();
builder.Services.AddScoped<IHutDataService, HutDataService>();


builder.Services.AddScoped<CampingSiteSeeder>();
builder.Services.AddScoped<BookingSeeder>();



// Antiforgery setup
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "__Host-Antiforgery";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<CampingSiteSeeder>();
    await seeder.SeedAsync();
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<BookingSeeder>();
    await seeder.SeedAsync();
}

// Map controllers
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// 👇 Her bruger vi kun MapRazorComponents – ingen _Host, ingen MapBlazorHub
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp3semesterEksamensProjektMappe.Client.Pages.Camping).Assembly)
    .AllowAnonymous();

app.Run();
