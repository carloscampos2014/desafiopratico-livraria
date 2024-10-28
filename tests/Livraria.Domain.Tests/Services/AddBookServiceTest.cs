using FluentValidation.Results;
using FluentValidation;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Data;
using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Services;
using Moq;
using FluentAssertions;

namespace Livraria.Domain.Tests.Services;

public class AddBookServiceTest
{
    [Fact(DisplayName = "Não Deve Permitir Incluir Quando Receber Dados Inválidos.")]
    public void Should_NotAllowAdd_WhenReceivingInvalidData()
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
        ValidationResult resultValidation = new ValidationResult(new List<ValidationFailure>() { new("Title", "Titulo é Obrigatório.") });
        validatorMock.Setup(x => x.Validate(request)).Returns(resultValidation);
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.Add(book))
            .Returns(true);
        string errorMessage = $"Dados Inválidos:{Environment.NewLine}{string.Join(Environment.NewLine, resultValidation.Errors.Select(error => error.ErrorMessage))}";
        IAddBookService service = new AddBookService(validatorMock.Object, repositoryMock.Object);

        // Act
        var actual = service.Execute(request);

        // Asserts
        actual.Success.Should().BeFalse();
        actual.ErrorMessage.Should().Be(errorMessage);
        validatorMock.Verify(v => v.Validate(request), Times.Exactly(1));
        repositoryMock.Verify(v => v.Add(book), Times.Never);
    }

    [Fact(DisplayName = "Deve Permitir Incluir Quando Receber Dados Válidos.")]
    public void Should_AllowAdd_WhenReceivingValidData()
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
            .Setup(x => x.Add(It.IsAny<Book>()))
            .Returns(true);
        IAddBookService service = new AddBookService(validatorMock.Object, repositoryMock.Object);

        // Act
        var actual = service.Execute(request);

        // Asserts
        actual.Success.Should().BeTrue();
        actual.Model.Title.Should().BeEquivalentTo(book.Title);
        actual.Model.Author.Should().BeEquivalentTo(book.Author);
        actual.Model.Description.Should().BeEquivalentTo(book.Description);
        validatorMock.Verify(v => v.Validate(request), Times.Exactly(1));
        repositoryMock.Verify(v => v.Add(It.IsAny<Book>()), Times.Exactly(1));
    }
}
