namespace DevCreedMoviesApi.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAll(byte genreId = 0 ); // default value if no para
        Task<Movie> GetById( int id );
        Task<Movie> Add( Movie movie );
        Movie Update( Movie movie );
        Movie Delete( Movie movie );

    }
}
