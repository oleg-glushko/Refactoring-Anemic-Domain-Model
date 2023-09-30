using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories;

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
