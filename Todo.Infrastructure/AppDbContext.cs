using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Todo.Domain.Entities;

namespace Todo.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<UserRole> UserRoles { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            builder.Entity<UserRole>()
                   .HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId)
                   .IsRequired();

            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<TodoList>()
                .HasOne(x => x.User)
                .WithMany(u => u.TodoLists)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid(), Name = "Admin", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "ADMIN" },
                new Role { Id = Guid.NewGuid(), Name = "User", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "USER" }
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
