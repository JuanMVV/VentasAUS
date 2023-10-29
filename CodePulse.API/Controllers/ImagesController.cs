using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //llamamos al repositorio para llamar a todas las imgs
            var images = await imageRepository.GetAll();

            // convertimos el dominio al dto
            var response = new List<BlogImageDTO>();
            foreach (var image in images) 
            {
                response.Add(new BlogImageDTO
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreate = image.DateCreate,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });
            }

            return Ok(response);
            
        }



        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title) 
        {
            ValidateFileUpload(file);
            
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreate = DateTime.Now
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                //convertimos el modelo a dto
                var response = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreate = blogImage.DateCreate,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };

                return Ok(response);
            }

            return BadRequest(ModelState);

           
        }

        private void ValidateFileUpload(IFormFile file)
        { 
            
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".pnp" };//estas validaciones se deberian poner en el appsetting como buena practica

                if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    ModelState.AddModelError("file", "Unsupported file format");
                }

                if (file.Length > 10485760) 
                {
                    ModelState.AddModelError("file", "File size cannot be more than 10MB");
                }

        }



    }
}
