using System;
using System.Threading.Tasks;
using deepu_dating.API.Models;
namespace deepu_dating.API.Repo
{
    public interface IAuthRepository
    {
         Task<userData> Register(userData user,string password);

         Task<userData> Login(string user,string password);

         Task<bool> UserExists(string user);
    }
}