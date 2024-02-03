using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    //hacer la referencia a entityframeworkCore
    public class ApplicationDbContext : DbContext
    {
        //cuando tenemos mas de 1 Dbcontext debemos indicar a cual corresponde
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //seteamos las clases, hacer la referencia al modelo
        //de aca se vincular cada modelo que replica en la db cuando actualizamos
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

    }
}
