using Microsoft.AspNetCore.Components;
using Porfolio.Client.Pages;
using Porfolio.Code.Entidades;
using Porfolio.Code.Service;
using System.IO.Pipes;

namespace Porfolio.Components.Pages;


public partial class Welcome
{
    [Inject] LinkedinService linkedinService { get; set; }
    [Inject] NavigationManager url { get; set; }

    public User currentUser { get; set; } = null;

    public bool IsInicialized { get; set; } = false;


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                IsInicialized = true;
                await WaitInizialized();

                if (currentUser != null)
                {
                    url.NavigateTo("/Home");
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
    async Task WaitInizialized()
    {
        if (IsInicialized)
        {
            currentUser = await linkedinService.GetProfile();
            StateHasChanged();
        }
    }
}
