using ColorlibHQ.AdminLTE.AspNetCore;
using ColorlibHQ.AdminLTE.AspNetCore.Menu;
using TesteTecnico.Web.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAdminLte(options =>
{
    options.BrandText = string.Empty;
    options.LogoImage = "/Anime%20API%20Logo%20%26%20Favicon%20(1).png";
    options.SidebarTheme = "dark";
    options.DefaultColorMode = "auto";
    options.Menu = new List<MenuItem>
    {
        new() { Header = "Navegação" },
        new() { Text = "Dashboard", Url = "/", Icon = "bi bi-speedometer" },
        new() { Text = "Animes", Url = "/animes", Icon = "bi bi-film" },
        new() { Text = "Diretores", Url = "/diretores", Icon = "bi bi-person" }
    };
});

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl configuration is not set.");

builder.Services.AddHttpClient<IAnimeApiClient, AnimeApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseProxy = false
});
builder.Services.AddHttpClient<IDiretorApiClient, DiretorApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseProxy = false
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
