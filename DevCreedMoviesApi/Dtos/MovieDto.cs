
namespace DevCreedMoviesApi.Dtos
{
    // we copy all movie model prop except the id and navigation property
   // and we add the poster property as IFormFile cuz it's media file or image
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string StoreLine { get; set; }
        public IFormFile? Poster { get; set; } // to store Media
        public byte GenreId { get; set; }
    }
}
