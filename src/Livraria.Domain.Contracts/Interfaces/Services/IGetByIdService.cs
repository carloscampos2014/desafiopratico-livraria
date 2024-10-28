using Livraria.Domain.Contracts.Models.Responses;

namespace Livraria.Domain.Contracts.Interfaces.Services;

public interface IGetByIdService
{
    BookResponse Execute(Guid id);
}
