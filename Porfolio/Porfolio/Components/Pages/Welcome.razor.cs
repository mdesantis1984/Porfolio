using Microsoft.AspNetCore.Components;
using Porfolio.Client.Pages;
using Porfolio.Code.Entidades;

using System.IO.Pipes;

namespace Porfolio.Components.Pages;


public partial class Welcome
{
    [Inject] NavigationManager url { get; set; }

    public User currentUser { get; set; } = null;

    public bool IsInicialized { get; set; } = false;


  
  
}
