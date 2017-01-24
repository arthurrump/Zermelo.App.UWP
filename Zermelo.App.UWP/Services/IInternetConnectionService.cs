using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zermelo.App.UWP.Services
{
    public interface IInternetConnectionService
    {
        bool IsConnected();
    }
}
