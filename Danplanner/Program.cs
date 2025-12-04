using Danplanner.Client.Pages;
using Danplanner.Client.Services;
using Danplanner.Components;
using Danplanner.Data;
using Danplanner.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;


// 👇 Tilføj logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();



// Blazor setup – Interactive render modes
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


// Her sørger vi for at enums sendes/læses som tekst i JSON
builder.Services.AddControllers();
    //.AddJsonOptions(o =>
    //{
    //    o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    //    o.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

    //    // 👇 Vigtigt: sørger for at enums sendes/læses som tekst
    //    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    //});


// Database setup - SQL Server

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));



builder.Services.AddHttpClient("EF", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["EfApiBaseAddress"]!);
});

builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthApiBaseAddress"]!);
});


builder.Services.AddHttpClient();
builder.Services.AddSingleton<Authservice>();
builder.Services.AddScoped<ICampingSitesService, CampingSitesService>();
builder.Services.AddScoped<ICampingSiteDataService, CampingSiteDataService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingDataService, BookingDataService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDataService, ProductDataService>();
builder.Services.AddScoped<ICottageService, CottageService>();
builder.Services.AddScoped<ICottageDataService, CottageDataService>();
builder.Services.AddScoped<IGrassFieldService, GrassFieldService>();
builder.Services.AddScoped<IGrassFieldDataService, GrassFieldDataService>();
builder.Services.AddScoped<IAddonService, AddonService>();
builder.Services.AddScoped<IAddonDataService, AddonDataService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();



builder.Services.AddScoped<CampingSiteSeeder>();


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

}

// Map controllers
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
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
    .AddAdditionalAssemblies(typeof(Danplanner.Client.Pages.Camping).Assembly)
    .AllowAnonymous();

app.Run();
