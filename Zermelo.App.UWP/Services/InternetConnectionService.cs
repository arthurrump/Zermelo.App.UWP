using Windows.Networking.Connectivity;

namespace Zermelo.App.UWP.Services
{
    public class InternetConnectionService : IInternetConnectionService
    {
        public bool IsConnected() 
            => NetworkInformation.GetInternetConnectionProfile()?.GetNetworkConnectivityLevel() 
                == NetworkConnectivityLevel.InternetAccess;
    }
}
