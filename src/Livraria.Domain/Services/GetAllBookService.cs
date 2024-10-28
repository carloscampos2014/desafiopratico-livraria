using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Responses;

namespace Livraria.Domain.Services;

public class GetAllBookService : IGetAllBookService
{
    private readonly IBookRepository _bookRepository;

    public GetAllBookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public BookResponse Execute()
    {
        try
        {
            return new BookResponse()
            {
                Models = _bookRepository.GetAll(),
            };
        }
        catch (Exception ex)
        {
            return new BookResponse()
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }
}
