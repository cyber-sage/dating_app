using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using deepu_dating.API.DTO;
using deepu_dating.API.Models;
using deepu_dating.API.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace deepu_dating.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {

        private readonly ILikesRepository likesRepository;
        private readonly IUserRepository userRepository;
        private readonly DataContext context;

        public LikesController(ILikesRepository likesRepository, IUserRepository userRepository, DataContext context)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.likesRepository = likesRepository;

        }
        [Authorize]
        [HttpPost("likeUser/{userName}")]
        public async Task<ActionResult> LikeUser(string userName)
        {

            var SourceUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var LikedUser = await userRepository.GetUserByUsernameNondtoAsync(userName);

            var SourceUser = await likesRepository.getUserById(SourceUserId);

            var userAlreadyLiked = await likesRepository.getLikedUser(SourceUserId, LikedUser.Id);

            if (LikedUser == null) return NotFound();

            if (userAlreadyLiked != null) return BadRequest("Already Liked the User");

            if (SourceUser.UserName == userName) return BadRequest("You Cannot Like Yourself");

            var userLikeObject = new UserLike
            {
                sourceUserId = SourceUserId,
                likedUserId = LikedUser.Id
            };

            await context.userlike.AddAsync(userLikeObject);

            if(await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Unable To Like User");

        }

        [HttpGet("getUsers/{predicate}")]
        public async Task<IEnumerable<LikesDto>> getUser(string predicate){

          var UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var requiredUserResults =await likesRepository.getUser(predicate,UserId);

            return requiredUserResults;
        }


    }
}