using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
