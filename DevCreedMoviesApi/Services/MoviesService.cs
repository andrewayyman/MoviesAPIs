
namespace DevCreedMoviesApi.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly AppDbContext _context;

        public MoviesService( AppDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            return await _context.Movies
                .Where(m => m.GenreId == genreId || genreId == 0 )
                .Include(m => m.Genre)
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        public async Task<Movie> GetById( int id )
        {
            return await _context.Movies
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie> Add( Movie movie )
        {
             await _context.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Update( Movie movie )
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete( Movie movie )
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie;
        }


    }
}
