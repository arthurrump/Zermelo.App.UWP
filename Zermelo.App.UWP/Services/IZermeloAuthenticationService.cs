using System.Threading.Tasks;
using Zermelo.API;

namespace Zermelo.App.UWP.Services
{
    public interface IZermeloAuthenticationService
    {
        Task<Authentication> GetAuthentication(string host, string code);
    }
}
