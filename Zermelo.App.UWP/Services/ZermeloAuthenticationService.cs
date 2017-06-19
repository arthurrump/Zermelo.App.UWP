using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zermelo.API;

namespace Zermelo.App.UWP.Services
{
    public class ZermeloAuthenticationService : IZermeloAuthenticationService
    {
        ZermeloAuthenticator _authenticator;

        public ZermeloAuthenticationService()
        {
            _authenticator = new ZermeloAuthenticator();
        }

        public async Task<Authentication> GetAuthentication(string host, string code) 
            => await _authenticator.GetAuthenticationAsync(host, code);
    }
}
