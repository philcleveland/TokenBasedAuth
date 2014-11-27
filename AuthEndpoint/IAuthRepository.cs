using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public interface IAuthRepository
    {
        Task<IdentityUser> FindUser(string userName, string password);
        Task<IdentityResult> RegisterUser(string userName, string password, string email);
    }
}
