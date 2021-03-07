using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using deepu_dating.API.DTO;
using deepu_dating.API.Helper;
using deepu_dating.API.Models;
using Microsoft.EntityFrameworkCore;

namespace deepu_dating.API.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;

        private readonly IMapper mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;

            this.context = context;

        }
        public async Task<IEnumerable<MemberDto>> GetUserAsync()
        {
            var user = await context.userdata.Include(c=>c.Photos).ToListAsync();

            var userToReturn = mapper.Map<IEnumerable<MemberDto>>(user);

            return  userToReturn ;
        }

        public async Task<userData> GetUserByIdAsync(int id)
        {
            return await context.userdata.FindAsync(id);
        }

        public async Task<MemberDto> GetUserByUsernameAsync(string username)
        {
            var user = await context.userdata.Include(c=>c.Photos).SingleOrDefaultAsync(c => c.UserName == username);

            var returnData = mapper.Map<MemberDto>(user);
            return returnData ;
        }

         public async Task<userData> GetUserByUsernameNondtoAsync(string username)
        {
            var user = await context.userdata.Include(c=>c.Photos).SingleOrDefaultAsync(c => c.UserName == username);

           
            return user ;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(userData user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}