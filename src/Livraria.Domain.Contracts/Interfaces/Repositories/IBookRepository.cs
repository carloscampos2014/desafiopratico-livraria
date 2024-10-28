using Livraria.Domain.Contracts.Models.Data;

namespace Livraria.Domain.Contracts.Interfaces.Repositories;

public interface IBookRepository
{
    bool Add(Book model);

    bool Delete(Guid id);

    IEnumerable<Book> GetAll();

    Book GetById(Guid id);

    bool Update(Book model);
}
