using System.ComponentModel.DataAnnotations;
using Bogus;
using FluentAssertions;
using FluentValidation.Results;
using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Validations;

namespace Livraria.Domain.Tests.Validations;
public class BookRequestValidationTest
{
    public static IEnumerable<object[]> ReturnInvalidValidation()
    {
        var titleEmpty = new FluentValidation.Results.ValidationFailure("Title", "O titulo do livro é obrigatório.");
        var titlelenght = new FluentValidation.Results.ValidationFailure("Title", "O titulo do livro deve ter entre 3 a 100 caracteres.");
        var authorEmpty = new FluentValidation.Results.ValidationFailure("Author", "O autor do livro é obrigatório.");
        var authorlenght = new FluentValidation.Results.ValidationFailure("Author", "O autor do livro deve ter entre 3 a 100 caracteres.");
        var descriptionEmpty = new FluentValidation.Results.ValidationFailure("Description", "A descrição do livro é obrigatória.");
        var descriptionlenght = new FluentValidation.Results.ValidationFailure("Description", "A descrição do livro deve ter entre 3 a 300 caracteres.");
        var publicationYearInvalid = new FluentValidation.Results.ValidationFailure("PublicationYear", "O ano de publicação do livro de ser maior ou igual a 1930.");
        var priceInvalid = new FluentValidation.Results.ValidationFailure("Price", "O preço inicial do livro de ser maior que R$ 0,00.");
        var stockInvalid = new FluentValidation.Results.ValidationFailure("Stock", "O estoque inicial do livro de ser maior ou igual a 0.");

        yield return new object[] { null, null, null, 0, 0, -1, new[]
            {
                titleEmpty, authorEmpty, descriptionEmpty, publicationYearInvalid, priceInvalid, stockInvalid,
            }
        };

        yield return new object[] { "a", "a", "a", DateTime.UtcNow.Year, 5, 1, new[]
            {
                titlelenght, authorlenght, descriptionlenght,
            }
        };
    }

    public static IEnumerable<object[]> ReturnModels()
    {
        var faker = new Faker<BookRequest>()
            .RuleFor(r => r.Title, f => f.Random.Words(5))
            .RuleFor(r => r.Author, f => f.Name.FullName())
            .RuleFor(r => r.Description, f => f.Random.Words(20))
            .RuleFor(r => r.PublicationYear, f => f.Random.Int(1930, DateTime.UtcNow.Year))
            .RuleFor(r => r.Price, f => f.Random.Decimal(0.01m, 199.99m))
            .RuleFor(r => r.Stock, f => f.Random.Int(0, 100));

        var models = faker.Generate(10);
        foreach (var model in models)
        {
            yield return new object[] { model };
        }
    }

    [Theory(DisplayName = "Deve Validar com Sucesso Quando Receber um Modelo Valido.")]
    [MemberData(nameof(ReturnModels))]
    public void Should_ValidateSuccessfully_WhenReceivingValidModel(BookRequest request)
    {
        // Arrange
        var validator = new BookRequestValidator();

        // Act 
        var actual = validator.Validate(request);

        // Assert
        actual.IsValid.Should().BeTrue();
        actual.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Deve Falhar a Validação Quando Receber um Modelo Nulo.")]
    public void Should_FailValidation_WhenReceivingNullModel()
    {
        // Arrange
        BookRequest request = null;
        var validator = new BookRequestValidator();
        var expected = new FluentValidation.Results.ValidationFailure(nameof(BookRequest), "A requisição não pode ser nula.");

        // Act 
        var actual = validator.Validate(request);

        // Assert
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should().NotBeEmpty();
        actual.Errors.FirstOrDefault().Should().BeEquivalentTo(expected);
    }

    [Theory(DisplayName = "Deve Falhar a Validação Quando Receber um Modelo Invalido.")]
    [MemberData(nameof(ReturnInvalidValidation))]
    public void Should_FailValidation_WhenReceivingInvalidModel(
        string title,
        string author,
        string description,
        int publicationYear,
        decimal price,
        int stock,
        IEnumerable<ValidationFailure> expecteds)
    {
        // Arrange
        var validator = new BookRequestValidator();
        BookRequest request = new() 
        { 
           Title = title,
           Author = author,
           Description = description,
           PublicationYear = publicationYear,
           Price = price,
           Stock = stock,
        };

        // Act 
        var actual = validator.Validate(request);

        // Assert
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should().NotBeEmpty();
        foreach (var expectedError in expecteds)
        {
            var matchingError = actual.Errors.SingleOrDefault(e => e.PropertyName == expectedError.PropertyName && e.ErrorMessage == expectedError.ErrorMessage);
            matchingError.Should().NotBeNull();
        }
    }
}
