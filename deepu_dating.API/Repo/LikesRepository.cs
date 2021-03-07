using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Threading.Tasks;
using deepu_dating.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using deepu_dating.API.DTO;
using deepu_dating.API.Extension;

namespace deepu_dating.API.Repo
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext context;

         public LikesRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<UserLike> getLikedUser(int sourceUserId, int likedUserId)
        {
           return await context.userlike.FindAsync(sourceUserId,likedUserId);
        }

        public async Task<IEnumerable<LikesDto>> getUser(string predicate, int userId)
        {
               var likes =context.userlike.AsQueryable();

               var users = context.userdata.OrderBy(c => c.UserName).AsQueryable();


            if(predicate=="likedByMe"){
                 likes = likes.Where(c => c.sourceUserId == userId);
                 users = likes.Select(like => like.likedUser);

                
            }
            else if(predicate == "likesMe"){
                likes = likes.Where(c => c.likedUserId == userId);
                 users = likes.Select(like => like.sourceUser);
            }
            return await users.Select(user => new LikesDto{
                userId=user.Id,
                userName= user.UserName,
                knownAs = user.KnownAs,
                age = user.DateOfBirth.CalculateAge(),
                Url= user.Photos.SingleOrDefault(x => x.IsMain).Url,
                City = user.City
            }).ToListAsync();

        }

        public async Task<userData> getUserById(int userId)
        {
            return await context.userdata.Include(c => c.LikedByUsers).SingleOrDefaultAsync(c =>c.Id == userId);
        }
    }
}