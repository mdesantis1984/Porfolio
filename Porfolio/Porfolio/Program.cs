using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using MudBlazor.Services;
using Porfolio.Components;
using System.Security.Claims;
using Porfolio.Code.Interface;
using Porfolio.Code.Entidades;
using Porfolio.Code.Implementacion;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Porfolio.Code.Services;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthenticationCore();

// Agregar servicios de autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "LinkedIn";
})
.AddCookie()
.AddOAuth("LinkedIn", options =>
{
    options.ClientId = builder.Configuration["OAuth2Services:OAuth2:client_id"];
    options.ClientSecret = builder.Configuration["OAuth2Services:OAuth2:client_secret"];
    options.CallbackPath = new PathString("/signin-linkedin");

    options.AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization";
    options.TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken";
    options.UserInformationEndpoint = "https://api.linkedin.com/v2/userinfo";

    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "givenName");
    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

    options.BackchannelHttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<JsonElement>();
            context.RunClaimActions(user);

            var firstName = user.GetProperty("name").GetString();
            var lastName = user.GetProperty("given_name").GetString();
            context.Identity.AddClaim(new Claim(ClaimTypes.Name, $"{firstName} {lastName}"));
        }
    };
});

builder.Services.AddScoped<UserService>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/pepelogin";
    options.LogoutPath = "/pepelogout";
    options.AccessDeniedPath = "/access-denied";
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();


builder.Services.Configure<OAuth2Services>(builder.Configuration.GetSection("OAuth2Services"));
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
builder.Services.AddLogging();


builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();



app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Porfolio.Client._Imports).Assembly);
    

app.Run();
