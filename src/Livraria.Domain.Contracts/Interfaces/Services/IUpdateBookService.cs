using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Contracts.Models.Responses;

namespace Livraria.Domain.Contracts.Interfaces.Services;

public interface IUpdateBookService
{
    BookResponse Execute(Guid id, BookRequest request);
}
