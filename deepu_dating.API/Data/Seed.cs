using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using deepu_dating.API.Models;
using Microsoft.EntityFrameworkCore;

namespace deepu_dating.API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context){
               if(await context.userdata.AnyAsync()) return ;

               var userData =await System.IO.File.ReadAllTextAsync("Data/SeedUser.json");

               var data = JsonSerializer.Deserialize<List<userData>>(userData);

               foreach(var user in data){

                   using  var hmac = new HMACSHA512();
                          user.UserName= user.UserName.ToLower();
                          user.Salt = hmac.Key;
                          user.Hashcode = hmac.ComputeHash(Encoding.UTF8.GetBytes("Vesper@31298"));
                          context.userdata.Add(user);


                   
               }
               await context.SaveChangesAsync();

        }
    }
}