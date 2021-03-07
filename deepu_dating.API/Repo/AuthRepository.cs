using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using deepu_dating.API.Models;

namespace deepu_dating.API.Repo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context)
        {
            this.context = context;

        }

        public async Task<userData> Login(string user, string password)
        {
           var userData= await context.userdata.Include(c=>c.Photos).FirstOrDefaultAsync(c=>c.UserName==user);

            if(userData==null){
                return null;
            }

            if(!verifyPassword(password,userData.Salt,userData.Hashcode)){
                return null;
            }

            return userData;


        }

        private bool verifyPassword(string password, byte[] salt, byte[] hashcode)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
               
               
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                 for(int i=0;i<computedHash.Length;i++){

                     if(computedHash[i]!=hashcode[i]){
                         return false;
                     }

                 }

                 return true;

            }
            
        }

        public async Task<userData> Register(userData user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreateHash(password, out passwordHash, out passwordSalt);
                                                                    
            user.Hashcode = passwordHash;

            user.Salt = passwordSalt;

            await context.userdata.AddAsync(user);
            await context.SaveChangesAsync();


            return user;





        }

        private void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;

                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


            }
        }

        public async Task<bool> UserExists(string user)
        {
           if(await context.userdata.AnyAsync(c=>c.UserName==user)){
               return true;
           }
           else{
               return false;
           }
           
            
        }
    }
}