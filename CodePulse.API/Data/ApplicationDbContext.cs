using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    //hacer la referencia a entityframeworkCore
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //seteamos las clases, hacer la referencia al modelo
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
