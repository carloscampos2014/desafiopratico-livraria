using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Data;
using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Services;
using Moq;

namespace Livraria.Domain.Tests.Services;

public class UpdateBookServiceTest
{
    [Fact(DisplayName = "Não Deve Permitir Atualizar Quando Receber Dados Inválidos.")]
    public void Should_NotAllowUpdate_WhenReceivingInvalidData()
    {
        // Arrange
        Book book = ModelFaker.Book();
        BookRequest request = new BookRequest()
        {
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            PublicationYear = book.PublicationYear,
            Genre = book.Genre,
            Price = book.Price,
            Stock = book.Stock,
        };
        var validatorMock = new Mock<IValidator<BookRequest>>();
        ValidationResult resultValidation = new ValidationResult( new List<ValidationFailure>() { new("Title", "Titulo é Obrigatório.") });
        validatorMock.Setup(x => x.Validate(request)).Returns(resultValidation);
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetById(book.Id))
            .Returns(book);
        repositoryMock
            .Setup(x => x.Update(book))
            .Returns(true);
        string errorMessage = $"Dados Inválidos:{string.Join(";", resultValidation.Errors.Select(error => error.ErrorMessage))}";
        IUpdateBookService service = new UpdateBookService(validatorMock.Object, repositoryMock.Object);

        // Act
        var actual = service.Execute(book.Id, request);

        // Asserts
        actual.Success.Should().BeFalse();
        actual.ErrorMessage.Should().Be(errorMessage);
        validatorMock.Verify(v => v.Validate(request), Times.Exactly(1));
        repositoryMock.Verify(v => v.GetById(book.Id), Times.Never);
        repositoryMock.Verify(v => v.Update(book), Times.Never);
    }

    [Fact(DisplayName = "Não Deve Permitir Atualizar Quando Não Encontrar o Registro.")]
    public void Should_NotAllowUpdate_WhenRegisterNotFound()
    {
        // Arrange
        Book book = ModelFaker.Book();
        BookRequest request = new BookRequest()
        {
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            PublicationYear = book.PublicationYear,
            Genre = book.Genre,
            Price = book.Price,
            Stock = book.Stock,
        };
        Book nullBook = null;
        var validatorMock = new Mock<IValidator<BookRequest>>();
        ValidationResult resultValidation = new ValidationResult();
        validatorMock.Setup(x => x.Validate(request)).Returns(resultValidation);
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetById(book.Id))
            .Returns(nullBook);
        repositoryMock
            .Setup(x => x.Update(book))
            .Returns(true);
        string errorMessage = $"Nenhum livro encontrado com Id:{book.Id}.";
        IUpdateBookService service = new UpdateBookService(validatorMock.Object, repositoryMock.Object);

        // Act
        var actual = service.Execute(book.Id, request);

        // Asserts
        actual.Success.Should().BeFalse();
        actual.ErrorMessage.Should().Be(errorMessage);
        validatorMock.Verify(v => v.Validate(request), Times.Exactly(1));
        repositoryMock.Verify(v => v.GetById(book.Id), Times.Exactly(1));
        repositoryMock.Verify(v => v.Update(book), Times.Never);
    }

    [Fact(DisplayName = "Deve Permitir Atualizar Quando Receber Dados Válidos.")]
    public void Should_AllowUpdate_WhenReceivingValidData()
    {
        // Arrange
        Book book = ModelFaker.Book();
        BookRequest request = new BookRequest()
        {
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            PublicationYear = book.PublicationYear,
            Genre = book.Genre,
            Price = book.Price,
            Stock = book.Stock,
        };
        var validatorMock = new Mock<IValidator<BookRequest>>();
        ValidationResult resultValidation = new ValidationResult();
        validatorMock.Setup(x => x.Validate(request)).Returns(resultValidation);
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetById(book.Id))
            .Returns(book);
        repositoryMock
            .Setup(x => x.Update(book))
            .Returns(true);
        IUpdateBookService service = new UpdateBookService(validatorMock.Object, repositoryMock.Object);

        // Act
        var actual = service.Execute(book.Id, request);

        // Asserts
        actual.Success.Should().BeTrue();
        actual.Model.Should().BeEquivalentTo(book);
        validatorMock.Verify(v => v.Validate(request), Times.Exactly(1));
        repositoryMock.Verify(v => v.GetById(book.Id), Times.Exactly(1));
        repositoryMock.Verify(v => v.Update(book), Times.Exactly(1));
    }
}
