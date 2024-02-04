using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,
            //para poder hacer una busqueda de una categoria necesito injectarlo aqui
            ICategoryRepository categoryRepository
            )
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDTO request)
        {
            //convertimos el DTO en Domain
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,

                //ahora tambien le pasamos una lista de los ids para relacionarlos con las categoria, primero lo inicializamos.
                Categories = new List<Category>()
            };

            //ahora necesitamos iterar por cada id para ver si existen previamente en la base
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                //si no existe lo inserto
                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }

            }


            blogPost = await blogPostRepository.CreateAsync(blogPost);

            //convertimos el domain model en el dto
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto 
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);


        }


        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllBlogPosts();

            //convertimos el modelo al dto
            var response = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,

                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });               
            }

            return Ok(response);

        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id ) 
        {
            //get blog post por repository
            var blogPost = await blogPostRepository.GetByIdAsync(id);
            if (blogPost is null)
            {
                return NotFound();
            }

            //convertimos el modelo en dto para mostrar el resultado
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);

        }


        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle) 
        {
            // get blog post details from repository
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            if (blogPost is null)
            {
                return NotFound();
            }

            //convertimos el modelo en dto para mostrar el resultado
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);

        }



        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request) 
        {
            //convertimos el model por el dto
            var blogPost = new BlogPost
            {
                Id = id,               
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            //creamos el for each para cada uno que vega
            foreach (var categoryGuid in request.Categories)
            {
                var existingCateogry = await categoryRepository.GetById(categoryGuid);
                if (existingCateogry is not null)
                {
                    blogPost.Categories.Add(existingCateogry);
                }
            }

            //llamamos al repositorio donde tenemos el metodo
            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);
            if (updatedBlogPost == null)
            {
                return NotFound();
            }

            //convertimos el domain model en el dto
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);


        }



        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id) 
        {
            var deleteBlogPost = await blogPostRepository.DeleteAsync(id);
            if (deleteBlogPost == null)
            {
                return NotFound();
            }

            //si esta todo ok, debemos convertir el modelo
            var response = new BlogPostDto
            {
                Id = deleteBlogPost.Id,
                Author = deleteBlogPost.Author,
                Content = deleteBlogPost.Content,
                FeaturedImageUrl = deleteBlogPost.FeaturedImageUrl,
                IsVisible = deleteBlogPost.IsVisible,
                PublishedDate = deleteBlogPost.PublishedDate,
                ShortDescription = deleteBlogPost.ShortDescription,
                Title = deleteBlogPost.Title,
                UrlHandle = deleteBlogPost.UrlHandle
            };

            return Ok(response);
                 
        }




    }
}
