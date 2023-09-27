using System.Reflection.Metadata.Ecma335;

namespace CodePulse.API.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }

        //creamos una nueva propiedad que representa la relacion muchos a muchos
        //una categoria puede tener muchos blogpost
        public ICollection<BlogPost> blogPosts { get; set; }
    }
}
