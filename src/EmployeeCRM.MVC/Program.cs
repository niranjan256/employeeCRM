using EmployeeCRM.MVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure base URL for API Services
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/api/";

// Configure HttpClient to ignore SSL errors for local development
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};

// Register HttpClients for our API Services
builder.Services.AddHttpClient<AuthApiService>(client => client.BaseAddress = new Uri(apiBaseUrl)).ConfigurePrimaryHttpMessageHandler(() => handler);
builder.Services.AddHttpClient<EmployeeApiService>(client => client.BaseAddress = new Uri(apiBaseUrl)).ConfigurePrimaryHttpMessageHandler(() => handler);
builder.Services.AddHttpClient<ClientApiService>(client => client.BaseAddress = new Uri(apiBaseUrl)).ConfigurePrimaryHttpMessageHandler(() => handler);
builder.Services.AddHttpClient<TaskApiService>(client => client.BaseAddress = new Uri(apiBaseUrl)).ConfigurePrimaryHttpMessageHandler(() => handler);
builder.Services.AddHttpClient<PerformanceApiService>(client => client.BaseAddress = new Uri(apiBaseUrl)).ConfigurePrimaryHttpMessageHandler(() => handler);
builder.Services.AddHttpClient<ReportApiService>(client => client.BaseAddress = new Uri(apiBaseUrl)).ConfigurePrimaryHttpMessageHandler(() => handler);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Important: must be before UseAuthorization

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
