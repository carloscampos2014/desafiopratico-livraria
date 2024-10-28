using FluentValidation;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Contracts.Models.Responses;

namespace Livraria.Domain.Services;

public class UpdateBookService : IUpdateBookService
{
    private readonly IValidator<BookRequest> _validator;
    private readonly IBookRepository _bookRepository;

    public UpdateBookService(IValidator<BookRequest> validator, IBookRepository bookRepository)
    {
        _validator = validator;
        _bookRepository = bookRepository;
    }

    public BookResponse Execute(Guid id, BookRequest request)
    {
        try
        {
            var resultValidation = _validator.Validate(request);
            if (!resultValidation.IsValid)
            {
                throw new InvalidOperationException($"Dados Inválidos:{string.Join(";", resultValidation.Errors.Select(error => error.ErrorMessage))}");
            }

            var model = _bookRepository.GetById(id);
            if (model is null)
            {
                throw new InvalidOperationException($"Nenhum livro encontrado com Id:{id}.");
            }

            model.Title = request.Title;
            model.Author = request.Author;
            model.Description = request.Description;
            model.PublicationYear = request.PublicationYear;
            model.Genre = request.Genre;
            model.Price = request.Price;
            model.Stock = request.Stock;
            model.Updated = DateTime.UtcNow;

            return new BookResponse()
            {
                Success = _bookRepository.Update(model),
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
