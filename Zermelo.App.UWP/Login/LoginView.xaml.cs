using System.Threading.Tasks;
using Autofac;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Login
{
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
        }

        LoginViewModel _viewModel;
        public LoginViewModel ViewModel => _viewModel ?? (_viewModel = (LoginViewModel)DataContext);

        private async void Code_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (LoginButton.Focus(FocusState.Programmatic))
                {
                    await Task.Delay(200);
                    ViewModel.LogIn.Execute();
                }
            }
        }
    }
}
