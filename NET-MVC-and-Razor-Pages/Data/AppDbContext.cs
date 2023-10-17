using Microsoft.EntityFrameworkCore;
using NET_MVC_and_Razor_Pages.Models;

namespace NET_MVC_and_Razor_Pages.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
    }

    
}
