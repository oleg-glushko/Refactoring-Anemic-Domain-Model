using Logic.Common;
using Logic.Utils;

namespace Logic.Movies;

public class MovieRepository : Repository<Movie>
{
    public MovieRepository(AppDbContext context) : base(context)
    {
    }

    public IReadOnlyList<Movie> GetList()
    {
        return _context.Set<Movie>().ToList();
    }
}
