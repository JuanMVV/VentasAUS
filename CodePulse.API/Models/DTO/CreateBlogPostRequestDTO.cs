namespace CodePulse.API.Models.DTO
{
    public class CreateBlogPostRequestDTO
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool IsVisible { get; set; }

        //cuando creamos un blogpost, tenemos que asignarle una o muchas categorias
        public Guid[] Categories { get; set; }

    } 
}
