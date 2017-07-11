using System.Collections.ObjectModel;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Shell
{
    public sealed partial class ShellView : Page
    {
        public static ShellView Instance { get; private set; }

        public ShellView(INavigationService navigationService)
        {
            Instance = this;
            InitializeComponent();
            HamburgerMenu.NavigationService = navigationService;
        }

        public ObservableCollection<HamburgerButtonInfo> HamburgerPrimaryButtons => HamburgerMenu.PrimaryButtons;
    }
}
