using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Shell
{
    public sealed partial class ShellView : Page
    {
        public ShellView(INavigationService navigationService)
        {
            InitializeComponent();
            HamburgerMenu.NavigationService = navigationService;
        }
    }
}
