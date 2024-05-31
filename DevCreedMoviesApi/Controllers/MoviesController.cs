using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace DevCreedMoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // dependency injection


        private readonly AppDbContext _context;
        public MoviesController( AppDbContext context )
        {
            _context = context;
        }
        // to allow only some exetesnions and size only for the image or media file
        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1024 * 1024; // 1MB

        #region Endpoints

        #region GetMovies
        [HttpGet] // api/Movies
        public async Task<IActionResult> GetAllMoviesAsync()
        {
            var Movies = await _context.Movies
                .Include(g => g.Genre)
                .OrderByDescending(r => r.Rate)
                .Select( m => new MovieDetailsDto
                {
                    Id = m.Id,
                    GenreId = m.GenreId,
                    GenreName = m.Genre.Name, // to get data from complex object
                    Poster = m.Poster,
                    Rate = m.Rate,
                    Storeline = m.StoreLine,
                    Title = m.Title,
                    Year = m.Year
                })
                .ToListAsync();

            return Ok(Movies);
        }
        #endregion

        #region GetMovieById
        [HttpGet("id")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            // here we cannot use findasync with include , then we will use firstordefaultasync
            var Movie = await _context.Movies
                .Include(g => g.Genre)
                .FirstOrDefaultAsync( m => m.Id == id ) ;

            if(Movie == null)
                return NotFound();
            var dto = new MovieDetailsDto
            {
                Id = Movie.Id,
                GenreId = Movie.GenreId,
                GenreName = Movie.Genre?.Name, // to accept null value
                Poster = Movie.Poster,
                Rate = Movie.Rate,
                Storeline = Movie.StoreLine,
                Title = Movie.Title,
                Year = Movie.Year
            };

            return Ok(dto);
        }

        #endregion

        #region AddMovie and handling IFormFile ( image or media )
        [HttpPost] // api/Movies    
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            #region Some Validation
            // to validate the image extesnion
            if ( !_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Invalid file extension");

            // to validate the image size
            if(dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Invalid file size");

            // to validate the entered genre id
            var isValiedGenre = await _context.Genres.AnyAsync(d=>d.Id == dto.GenreId);
            if(!isValiedGenre )
                return BadRequest("Invalid genre id");
            #endregion

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

        #endregion






    }
}
