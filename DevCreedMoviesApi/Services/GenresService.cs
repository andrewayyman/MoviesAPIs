

namespace DevCreedMoviesApi.Services
{
    public class GenresService : IGenresService
    {
        private readonly AppDbContext _context;

        public GenresService( AppDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre> GetByID(byte id )
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Genre> Add( Genre genre )
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Update( Genre genre )
        {
            _context.Update(genre);
            _context.SaveChanges();

            return genre;

        }
        public Genre Delete( Genre genre )
        {
            _context.Remove(genre);
            _context.SaveChanges();

            return genre;
        }


    }
}
