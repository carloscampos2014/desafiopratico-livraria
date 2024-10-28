﻿using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Responses;

namespace Livraria.Domain.Services;

public class DeleteBookService : IDeleteBookService
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public BookResponse Execute(Guid id)
    {
        try
        {
            var model = _bookRepository.GetById(id);
            if (model is null)
            {
                throw new InvalidOperationException($"Nenhum livro encontrado com Id:{id}.");
            }

            return new BookResponse()
            {
                Success = _bookRepository.Delete(id),
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