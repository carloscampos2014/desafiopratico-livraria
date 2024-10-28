using Livraria.Domain.Contracts.Models.Responses;

namespace Livraria.Domain.Contracts.Interfaces.Services;

public interface IGetAllBookService
{
    BookResponse Execute();
}
