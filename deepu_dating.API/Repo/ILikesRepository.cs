using System.Collections.Generic;
using System.Threading.Tasks;
using deepu_dating.API.DTO;
using deepu_dating.API.Models;

namespace deepu_dating.API.Repo
{
    public interface ILikesRepository
    {
         Task<UserLike> getLikedUser(int sourceUserId, int likedUserId);

         Task<IEnumerable<LikesDto>> getUser(string predicate, int userId); 

         Task<userData> getUserById(int userId);
    }
}