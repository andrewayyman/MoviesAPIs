
namespace MoviesApi.Helper
{
    public class Mappingprofile : Profile
    {
        public Mappingprofile()
        {
            // source -> destination
            CreateMap<Movie, MovieDetailsDto>(); 
            CreateMap<MovieDto, Movie>()
                 .ForMember(src => src.Poster, opt => opt.Ignore()) ; 
        }

    }
}
