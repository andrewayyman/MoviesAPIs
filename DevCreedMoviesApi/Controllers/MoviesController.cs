using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevCreedMoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MoviesController( AppDbContext context )
        {
            _context = context;
        }

        // Endpoints //


        #region AddMovie and handling IFormFile ( image or media )
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            // to store the image in the database we need to convert it to byte array
            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);


            var Movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                StoreLine = dto.StoreLine,
                Poster = dataStream.ToArray(), // not dto.Poster
                GenreId = dto.GenreId
            };
            await _context.AddAsync(Movie);
            _context.SaveChanges();
            return Ok(Movie);
        }
        // this api to allow user to add a new movie so ,
        // we need to add new dto to validate the data before adding it to the database
        // but the problem is that poster property is of type IFormFile cuz it's media file or image 
        // so we need to convert it to byte array to store it in the database
        // then we need to use [fromform] to make the api accept the data in form format so i can upload the image


        #endregion








    }
}
