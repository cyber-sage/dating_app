using System.Collections.Generic;
using System.Threading.Tasks;
using deepu_dating.API.DTO;
using deepu_dating.API.Models;

namespace deepu_dating.API.Repo
{
    public interface IUserRepository
    {
         void Update(userData user);

         Task<bool> SaveAllAsync();

         Task<IEnumerable<MemberDto>> GetUserAsync();

         Task<userData> GetUserByIdAsync(int id);

         Task<MemberDto> GetUserByUsernameAsync(string username); 
         Task<userData> GetUserByUsernameNondtoAsync(string username);
    }
}