using BlazorApp1.Components;
using BlazorApp1.Data.Vendor;
using BlazorApp1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionContext>(builder.
    Configuration.GetSection("ConnectionStrings"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTelerikBlazor();

builder.Services.AddSingleton<IDatabaseVendor, SqliteVendor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();