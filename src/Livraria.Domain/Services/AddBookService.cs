using FluentValidation;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Data;
using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Contracts.Models.Responses;

namespace Livraria.Domain.Services;

public class AddBookService : IAddBookService
{
    private readonly IValidator<BookRequest> _validator;
    private readonly IBookRepository _bookRepository;

    public AddBookService(IValidator<BookRequest> validator, IBookRepository bookRepository)
    {
        _validator = validator;
        _bookRepository = bookRepository;
    }

    public BookResponse Execute(BookRequest request)
    {
        try
        {
            var resultValidation = _validator.Validate(request);
            if (!resultValidation.IsValid)
            {
                throw new InvalidOperationException($"Dados Inválidos:{string.Join(";", resultValidation.Errors.Select(error => error.ErrorMessage))}");
            }

            var model = new Book()
            {
                Title = request.Title,
                Author = request.Author,
                Description = request.Description,
                PublicationYear = request.PublicationYear,
                Genre = request.Genre,
                Price = request.Price,
                Stock = request.Stock,
            };

            return new BookResponse()
            {
                Success = _bookRepository.Add(model),
                Model = model,
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
