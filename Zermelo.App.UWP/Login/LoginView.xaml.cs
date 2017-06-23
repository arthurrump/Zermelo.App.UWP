using Autofac;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Login
{
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();

            ViewModel = (App.Current as App).Container.Resolve<LoginViewModel>();
        }

        public LoginViewModel ViewModel { get; }
    }
}
