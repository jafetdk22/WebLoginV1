using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OtraPrueba;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Agregar servicios de localización
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("es"),
        new CultureInfo("en")
    };

    options.DefaultRequestCulture = new RequestCulture("es");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();



// Obtener el parámetro desde la configuración
var someParameterValue = builder.Configuration["ApiLoginUrl"];
builder.Services.AddHttpClient();
// Registrar ApiLogin con el parámetro string
builder.Services.AddScoped<ApiLoginClient>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    return new ApiLoginClient(someParameterValue, httpClient);
});

builder.Services.AddScoped<IViewLocalizer, ViewLocalizer>();

var app = builder.Build();

// Configurar el middleware de localización
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();