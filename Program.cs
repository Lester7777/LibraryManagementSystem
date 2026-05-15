using LibraryManagement.Components;
using LibraryManagement.Features.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using LibraryManagement.Features.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddHttpContextAccessor();

// Add HttpClient for Blazor Server to call its own Minimal APIs
builder.Services.AddScoped<HttpClient>(sp =>
{
    var navigationManager = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});

// Authentication services with cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "LibraryManagement.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseMySql(
 builder.Configuration.GetConnectionString("LibraryManagement"),
 ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("LibraryManagement"))
 ));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<CurrentUserService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/api/auth/login", async (HttpContext context, [Microsoft.AspNetCore.Mvc.FromForm] string Email, [Microsoft.AspNetCore.Mvc.FromForm] string Password, IAuthService authService) =>
{
    var member = await authService.LoginAsync(Email, Password);
    if (member == null)
    {
        return Results.Redirect("/login?error=invalid_credentials");
    }

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
        new Claim(ClaimTypes.Email, member.Email),
        new Claim(ClaimTypes.Name, member.FirstName),
        new Claim(ClaimTypes.Surname, member.LastName),
        new Claim("IsAdmin", member.IsAdmin.ToString())
    };

    ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    AuthenticationProperties authProperties = new()
    {
        IsPersistent = true,
        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
    };

    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimsIdentity),
        authProperties);

    return Results.Redirect("/dashboard");
});

app.MapPost("/api/auth/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
});

app.MapGet("/api/auth/currentuser", (HttpContext context) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        return Results.Json(new
        {
            Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Email = context.User.FindFirst(ClaimTypes.Email)?.Value,
            FirstName = context.User.FindFirst(ClaimTypes.Name)?.Value,
            LastName = context.User.FindFirst(ClaimTypes.Surname)?.Value,
            IsAuthenticated = true
        });
    }
    return Results.Json(new { IsAuthenticated = false });
});

app.Run();
