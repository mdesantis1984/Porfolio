namespace Porfolio.Components.Layout;

using Themes;

public partial class MainLayout
{
    private bool _drawerOpen = true;

    Themes themes = new Themes(); 

    public void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
