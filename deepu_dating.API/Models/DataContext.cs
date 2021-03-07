using System.ComponentModel.DataAnnotations;

using System.Reflection.Emit;
using  Microsoft.EntityFrameworkCore;
namespace deepu_dating.API.Models
{
    public class DataContext:DbContext
    {

        public DataContext(DbContextOptions<DataContext> options): base(options){}

         public DbSet<Value> values {get; set;}

         public DbSet<names> Names {get; set;}

         public DbSet<userData> userdata { get; set; }

         public DbSet<UserLike> userlike {get; set;}

         public DbSet<Message> Messages {get; set;}

         protected override void OnModelCreating(ModelBuilder builder){
              base.OnModelCreating(builder);

              builder.Entity<UserLike>()
              .HasKey(k =>new {k.sourceUserId,k.likedUserId});

              builder.Entity<UserLike>()
              .HasOne(s => s.sourceUser)
              .WithMany(l=>l.LikedUsers)
              .HasForeignKey(l=>l.sourceUserId)
              .OnDelete(DeleteBehavior.Cascade);

              builder.Entity<UserLike>()
              .HasOne(s => s.likedUser)
              .WithMany(l=>l.LikedByUsers)
              .HasForeignKey(l=>l.likedUserId)
              .OnDelete(DeleteBehavior.Cascade);

              builder.Entity<Message>()
              .HasOne(s => s.Sender)
              .WithMany(l => l.MessagesSend)
              .OnDelete(DeleteBehavior.Restrict);

              builder.Entity<Message>()
              .HasOne(r => r.Recipient)
              .WithMany(r => r.MessagesReceived)
              .OnDelete(DeleteBehavior.Restrict);

              
         }

        
    }
}