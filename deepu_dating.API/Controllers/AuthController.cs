using System.Diagnostics;
using System.Buffers.Text;
using System.Reflection.Metadata;
using System.IO;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using deepu_dating.API.DTO;
using deepu_dating.API.Models;
using deepu_dating.API.Repo;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace deepu_dating.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository Repo;
        private readonly DataContext context;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService photoRepo;
        public AuthController(IAuthRepository Repo, DataContext context, IUserRepository userRepository, IMapper mapper, IPhotoService photoRepo)
        {
            this.photoRepo = photoRepo;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.context = context;
            this.Repo = Repo;

        }
        [Authorize]
        [HttpPut("updateuser")]
        public async Task<IActionResult> updateUser([FromBody] updateDto data)
        {
            var user = await context.userdata.Include(c => c.Photos).SingleOrDefaultAsync(c => c.UserName == data.userName);
            mapper.Map(data, user);
            userRepository.Update(user);
            if (await userRepository.SaveAllAsync()) return NoContent();
            else return BadRequest("You are Doomed");

        }



        [Authorize]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetNames()
        {



            var names = await userRepository.GetUserAsync();

            return Ok(names);


        }
        [Authorize]
        [HttpGet("GetMember/{username}", Name = "GetUserByUsername")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {

            return Ok(await userRepository.GetUserByUsernameAsync(username));
        }

        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] AuthLoginDto logindto)
        {

            var userDataRepo = await Repo.Login(logindto.username.ToLower(), logindto.password);

            if (userDataRepo == null)
            {
                return Unauthorized();
            }

            //generate Token

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("Super secret key");

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,userDataRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name,userDataRepo.UserName)


                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var User = new loginReturnDto
            {
                username = userDataRepo.UserName,
                token = tokenString,
                userImageUrl = userDataRepo.Photos.SingleOrDefault(m=>m.IsMain).Url
            };

            return Ok(User);


        }


        [AllowAnonymous]
        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {


            registerDto.UserName = registerDto.UserName.ToLower();

            if (await Repo.UserExists(registerDto.UserName))
            {
                //  return BadRequest("User Already Exits");
                ModelState.AddModelError("user", "Username is already taken");

            }



            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }







          var userToCreate = mapper.Map<userData>(registerDto);

            // var userToCreate = new userData
            // {
            //     UserName = userDataAuth.user
            // };

            var userReturn = await Repo.Register(userToCreate, registerDto.Password);
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("Super secret key");

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,userReturn.Id.ToString()),
                    new Claim(ClaimTypes.Name,userReturn.UserName)


                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var User = new loginReturnDto();
            // {
            //     username = userReturn.UserName,
            //     token = tokenString,
            //     userImageUrl = userReturn.Photos.SingleOrDefault(m=>m.IsMain).Url
            // };
            User.username = userReturn.UserName;
            User.token = tokenString;
           // User.userImageUrl = userReturn.Photos.SingleOrDefault(m=>m.IsMain).Url;

            

            
           

            return Ok(User);



        }
        [Authorize]

        [HttpPost("photo-upload")]
        public async Task<ActionResult<PhotoDto>> PhotoUpload(IFormFile file)
        {

            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            var user = await userRepository.GetUserByUsernameNondtoAsync(userName);

            var imageUpload = await photoRepo.AddPhotoAsync(file);

            if(imageUpload.Error != null) return BadRequest(imageUpload.Error.Message);

            var photo = new Photo{
                Url = imageUpload.Url.AbsoluteUri,
                PublicId = imageUpload.PublicId
            };

            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await userRepository.SaveAllAsync()){
                
               return  CreatedAtRoute("GetUserByUsername", new {username=user.UserName},mapper.Map<PhotoDto>(photo));
                
            }

            return BadRequest("Problem Loading the Photo");



        }
        [HttpPut("main-photo-edit/{photoId}")]
        public async Task<ActionResult> photoMainUpload(int photoId){

          var username = User.FindFirst(ClaimTypes.Name)?.Value;
        

          var user =await userRepository.GetUserByUsernameNondtoAsync(username);

          var photo = user.Photos.FirstOrDefault(x=>x.id==photoId);

          if(photo.IsMain) BadRequest("Given Photo is already a Main Photo");

          var currentMain = user.Photos.SingleOrDefault(x=>x.IsMain);

          if(currentMain != null) currentMain.IsMain = false;

          photo.IsMain = true;

          await userRepository.SaveAllAsync();

          return NoContent();

          



        }
        [HttpDelete("photo-delete/{photoId}")]
        public async Task<ActionResult> PhotoDelete(int photoId){
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
        

          var user =await userRepository.GetUserByUsernameNondtoAsync(username);

          var photo = user.Photos.FirstOrDefault(c=>c.id==photoId);

          if(photo == null) return NoContent();

          if(photo.IsMain) return BadRequest("huihui you can't delete your main Photo");

          if(photo.PublicId != null) {
            var deleteResult = await photoRepo.DeletePhotoAsync(photo.PublicId);
            if(deleteResult.Error !=null) return BadRequest("Problem deleting in Cloudinary");
          }

          user.Photos.Remove(photo);
          await userRepository.SaveAllAsync();

          

         return Ok();

        

        }


    }
}