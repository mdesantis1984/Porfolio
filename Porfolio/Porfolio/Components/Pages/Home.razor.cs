using Microsoft.AspNetCore.Components;
using Porfolio.Code.Entidades;
using Porfolio.Code.Service;

namespace Porfolio.Components.Pages;

public partial class Home
{
    public User CurrentUserLinkedin { get; set; } = new User();

	[Inject] LinkedinService LinkedinService { get; set; }
  

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
		try
		{
            if (firstRender)
            {
                //var user = await LinkedinService.GetProfile();
            }
            
        }
		catch (Exception)
		{

			throw;
		}
    }

}
