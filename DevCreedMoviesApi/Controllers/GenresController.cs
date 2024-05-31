

using DevCreedMoviesApi.Dtos;
using DevCreedMoviesApi.Services;

namespace DevCreedMoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genreservice;

        // Dependency Injection , inject the service into the controller
        public GenresController( IGenresService genreservice )
        {
            _genreservice = genreservice;
        }



        #region EndPoints

        #region GetGenres
        [HttpGet] // api/Genres
        public async Task<IActionResult> GetAllSync()
        {
            var Genres = await _genreservice.GetAll();
            return Ok(Genres);
        }
        // we used async cuz we are using the database and it's an I/O operation
        // it helps to free the thread to do other things while waiting for the database to respond
        // if we not using async the thread will be blocked until the database responds
        //tolistasync() is used to convert the data to a list

        // another way to implement without using async :
        /*
        public IActionResult GetAllSync()
        {
            var Genres = _context.Genres.OrderBy(g=>g.Name).ToList();
            return Ok(Genres);
        }
        */
        #endregion

        #region AddGenre
        [HttpPost] // api/Genre
        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _genreservice.Add(genre);
            return Ok(genre); // return status code 200
        }

        //Dto is used to validate the data before sending it to the database
        // it's used to make sure that the data is valid before sending it to the database
        // we used it by add dtos folder and add a class inside it and add the properties that we want to validate
        // here we created CreateGenreDto and added the property that we want to validate
        // we used it in the CreateAsync method to validate the data before sending it to the database
        // we get from data with type CreateGenreDto and we validate it and then we send it to the database
        // we create genre object and we assign the name property from the dto object
        // if the data is valid we add it to the database with AddAsync method and then we save the changes
        #endregion

        #region UpdateGenre
        [HttpPut("{id}") ] // api/Genres/1 , here we used id to specify the genre that we want to update
        public async Task<IActionResult> UpdateAsync(byte id , [FromBody] CreateGenreDto dto)
        {
            var genre = await _genreservice.GetByID(id);
            if (genre == null) { return NotFound("No Genre Found"); }
            genre.Name = dto.Name;
            _genreservice.Update(genre);
            return Ok(genre);
        }
        // [from body ]we used it to get the data from the body of the request and we used it with the dto object to validate the data
        // singleordefaultasync is return the first element of the sequence or a default value if the sequence contains no elements while the default value is null
        // and we if we used async method so we have to use await keyword to wait for the database to respond


        #endregion

        #region DeleteGenre
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genreservice.GetByID(id);
            if (genre == null) { return NotFound("No Genre Found to Delete"); }

            _genreservice.Delete(genre);
            return Ok(genre);

        }

        // same as the update method but we used remove method to remove the genre from the database

        #endregion

        #endregion
    }
}
