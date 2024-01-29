using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
                
        Task<IEnumerable<BlogPost>> GetAllBlogPosts();

        //usamos el "?" para decirle q si no encuentra datos, retorne un null back
        Task<BlogPost?> GetByIdAsync(Guid id);

        //creamos un nuevo get para pasarle al navegador el nombre del modulo y no el id
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
                
        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
