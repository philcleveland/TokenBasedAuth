using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public interface IAuthRepository
    {
        Task<UserModel> FindUser(string userName, string password);
        Task<IdentityResult> RegisterUser(UserModel user, string password);
    }
}
