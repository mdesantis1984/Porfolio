namespace Porfolio.Components.Layout;

using Microsoft.AspNetCore.Components;
using Porfolio.Code.Services;
using System.Security.Claims;
using Themes;

public partial class MainLayout
{

    [Inject] UserService UserService { get; set; }



    private bool _drawerOpen = true;

    Themes themes = new Themes(); 

    public void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }


    private ClaimsPrincipal user;
    private string userName;
    private string userEmail;

    protected override async Task OnInitializedAsync()
    {
        user = await UserService.GetUserAsync();
        userName = await UserService.GetUserNameAsync();
        userEmail = await UserService.GetUserEmailAsync();
    }

}
