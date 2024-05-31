
namespace DevCreedMoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // dependency injection , inject the 2 services
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;
        private readonly IMapper _mapper;

        public MoviesController( IMoviesService moviesService, IGenresService genresService, IMapper mapper )
        {
            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }

        // to allow only some exetesnions and size only for the image or media file
        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1024 * 1024; // 1MB

        #region Endpoints

        #region GetMovies
        [HttpGet] // api/Movies
        public async Task<IActionResult> GetAllMoviesAsync()
        {
            var movies = await _moviesService.GetAll();
            // map using automapper
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            return Ok(data);
        }
        #endregion

        #region GetMovieById
        [HttpGet("id")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            // here we cannot use findasync with include , then we will use firstordefaultasync
            var Movie = await _moviesService.GetById(id);

            if (Movie == null)
                return NotFound();
            var dto = _mapper.Map<MovieDetailsDto>(Movie);

            return Ok(dto);
        }

        #endregion
   
        #region GetMovieByGenreID
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync( byte genreId )
        {
            var Movies = await _moviesService.GetAll(genreId);
            // map movies to dto
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(Movies);

            return Ok(Movies);
        }


        #endregion
        
        #region AddMovie and handling IFormFile ( image or media )
        [HttpPost] // api/Movies    
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            #region Some Validation
            // validate if poster not sent
            if (dto.Poster == null) return BadRequest("Poster is Required");

            // to validate the image extesnion
            if ( !_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Invalid file extension");

            // to validate the image size
            if(dto.Poster.Length > _maxAllowedPosterSize) return BadRequest("Invalid file size");

            // to validate the entered genre id
            var isValiedGenre = await _genresService.IsValidGenre(dto.GenreId);
            if (!isValiedGenre ) return BadRequest("Invalid genre id");

            #endregion

            // to store the image in the database we need to convert it to byte array
            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);


            var Movie = _mapper.Map<Movie>(dto);
            Movie.Poster = dataStream.ToArray();

            await _moviesService.Add(Movie);
            return Ok(Movie);
        }


        // this api to allow user to add a new movie so ,
        // we need to add new dto to validate the data before adding it to the database
        // but the problem is that poster property is of type IFormFile cuz it's media file or image 
        // so we need to convert it to byte array to store it in the database
        // then we need to use [fromform] to make the api accept the data in form format so i can upload the image


        #endregion

        #region UpdateMovie
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync( int id, [FromForm] MovieDto dto )
        {
            var movie = await _moviesService.GetById(id);
            if ( movie == null ) { return NotFound("No Genre Found"); }

            #region Some Validation

            // to validate the entered genre id
            var isValiedGenre = await _genresService.IsValidGenre(dto.GenreId);
            if ( !isValiedGenre )
                return BadRequest("Invalid genre id");

            // to update the poster optioanlly
            if ( dto.Poster != null )
            {
                // to validate the image extesnion
                if ( !_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()) )
                    return BadRequest("Invalid file extension");

                // to validate the image size
                if ( dto.Poster.Length > _maxAllowedPosterSize )
                    return BadRequest("Invalid file size");

                // to store the image in the database we need to convert it to byte array
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }


            #endregion

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Rate = dto.Rate;
            movie.StoreLine = dto.StoreLine;
            movie.Year = dto.Year;
            // optional to update the poster 

            _moviesService.Update(movie);
            return Ok(movie);
        }


        #endregion

        #region DeleteMovie
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync( int id )
        {
            var movie = await _moviesService.GetById(id);
            if ( movie == null ) { return NotFound("No Movie Found to Delete"); }

            _moviesService.Delete(movie);
            return Ok(movie);

        }

        #endregion


        #endregion






    }
}
