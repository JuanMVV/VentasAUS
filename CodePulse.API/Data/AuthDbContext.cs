using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // creamos dos id ramdom, para esto podemos usar una ventana de C# interativa Guid.NewGuid(), view->Other Windows->C# interactive
            var readerRoleId = "ca1e693e-959e-49da-a239-a90521dc13dc";
            var writerRoleId = "3788892a-6f6c-4b4c-b2e8-7dcb24f0b13d";

            //creamos dos roles de leedor y escritor
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp=readerRoleId
                },
                 new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };


            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);


            //creamos admin user
            var adminUserId = "fb4b482b-fdb9-40ab-b724-91ef9e353b8b";

            var admin = new IdentityUser()
            {
                Id=adminUserId,
                UserName="admin@codepulse.com",
                Email= "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            //give roles to admin (all de roles)
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);








        }


    }
}
