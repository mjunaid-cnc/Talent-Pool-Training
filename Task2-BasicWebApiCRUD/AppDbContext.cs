using Microsoft.EntityFrameworkCore;
using Task2_BasicWebApiCRUD.Models;

namespace Task2_BasicWebApiCRUD
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ToDoModel> ToDos { get; set; }
    }
}
