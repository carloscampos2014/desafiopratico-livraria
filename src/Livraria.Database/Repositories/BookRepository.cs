using Livraria.Database.Context;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Models.Data;

namespace Livraria.Database.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LivrariaDbContext _context;

    public BookRepository(LivrariaDbContext context)
    {
        _context = context;
    }

    public bool Add(Book model)
    {
        _context.Books.Add(model);
        return _context.SaveChanges() > 0;
    }

    public bool Delete(Guid id)
    {
        var model = GetById(id);
        _context.Books.Remove(model);
        return _context.SaveChanges() > 0;
    }

    public IEnumerable<Book> GetAll()
    {
        return _context.Books.ToList();
    }

    public Book GetById(Guid id)
    {
        return _context.Books.FirstOrDefault(f => f.Id == id);
    }

    public bool Update(Book model)
    {
        _context.Books.Update(model);
        return _context.SaveChanges() > 0;
    }
}
