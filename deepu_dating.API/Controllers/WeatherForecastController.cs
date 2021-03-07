using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using deepu_dating.API.Models;
using Microsoft.EntityFrameworkCore;
using deepu_dating.API.Repo;

using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

using deepu_dating.API.DTO;


using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace deepu_dating.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IUserRepository userRepository;
        private readonly IAuthRepository Repo;

        public WeatherForecastController(IUserRepository userRepository, IAuthRepository Repo)
        {
            this.userRepository = userRepository;
            this.Repo = Repo;
           


        }
        



        /* private static readonly string[] Summaries = new[]
         {
             "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
         };

         private readonly ILogger<WeatherForecastController> _logger;

         public WeatherForecastController(ILogger<WeatherForecastController> logger)
         {
             _logger = logger;
         } */

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetNames()
        {
            


            var names = await userRepository.GetUserAsync();

            return Ok(names);


        }
        [HttpGet]
        public async Task<IActionResult> GetUserById(int id){

            return Ok(await userRepository.GetUserByIdAsync(id));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username){

            return Ok(await userRepository.GetUserByUsernameAsync(username));
        }




        [Route("DeepuFriend")]

        [HttpGet]

        public async Task<IActionResult> DeepuFriend()
        {


            var names = await _context.Names.ToListAsync();

            return Ok(names);


        }



    }
}
