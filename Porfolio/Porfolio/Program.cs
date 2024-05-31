using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using MudBlazor.Services;
using Porfolio.Client.Pages;
using Porfolio.Components;
using System.Security.Claims;

using Porfolio.Code.Interface;
using Porfolio.Code.Entidades;
using Porfolio.Code.Implementacion;
using Porfolio.Code.Service;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();


builder.Services.Configure<OAuth2Services>(builder.Configuration.GetSection("OAuth2Services"));
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<LinkedinService>();


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

//app.UseAuthentication();
//app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Porfolio.Client._Imports).Assembly);

app.Run();
