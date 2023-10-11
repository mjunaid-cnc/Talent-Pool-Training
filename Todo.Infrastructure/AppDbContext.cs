using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Models;

namespace Todo.Infrastructure
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ToDoModel> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "ADMIN"},
                new IdentityRole { Name = "User", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "USER"}
                );
        }

        //private static void SeedAdmin(ModelBuilder builder)
        //{
        //    builder.Entity<IdentityUser>().HasData(
        //        new IdentityUser { UserName = "Admin12", PasswordHash = "AQAAAAEAACcQAAAAEDuRB+2iYtxnl1Q3u1RAi81ehiRbkan5mn2YtVYlzcq7qazoqX4ZDBtJa+IPr1q9vg==" }
        //        );
        //    RoleManager<IdentityRole> roleManager = new RoleManager();
            
        //        new IdentityUserRole { }
        //}
    }
}
