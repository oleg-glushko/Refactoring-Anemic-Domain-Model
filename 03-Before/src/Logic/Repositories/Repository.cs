using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories;

public abstract class Repository<T> where T : Entity
{
    protected readonly AppDbContext _context;

    protected Repository(AppDbContext context)
    {
        _context = context;
    }

    public T? GetById(long id)
    {
        return _context.Find<T>(id);
    }

    public void Add(T entity)
    {
        _context.Add(entity);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}

