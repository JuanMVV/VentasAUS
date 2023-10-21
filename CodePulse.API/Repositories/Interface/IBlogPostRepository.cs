using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
                
        Task<IEnumerable<BlogPost>> GetAllBlogPosts();

        //usamos el "?" para decirle q si no encuentra datos, retorne un null back
        Task<BlogPost?> GetByIdAsync(Guid id);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
                
        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
