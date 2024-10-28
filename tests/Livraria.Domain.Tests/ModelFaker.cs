using Bogus;
using Livraria.Domain.Contracts.Enums;
using Livraria.Domain.Contracts.Models.Data;
using Livraria.Domain.Contracts.Models.Requests;

namespace Livraria.Domain.Tests;

public static class ModelFaker
{
    public static IEnumerable<BookRequest> ListBookRequest(int number)
    {
        var faker = new Faker<BookRequest>()
            .RuleFor(r => r.Title, f => f.Random.Words(5))
            .RuleFor(r => r.Author, f => f.Name.FullName())
            .RuleFor(r => r.Description, f => f.Random.Words(20))
            .RuleFor(r => r.PublicationYear, f => f.Random.Int(1930, DateTime.UtcNow.Year))
            .RuleFor(r => r.Genre, f => f.PickRandom<GenreBook>())
            .RuleFor(r => r.Price, f => f.Random.Decimal(0.01m, 199.99m))
            .RuleFor(r => r.Stock, f => f.Random.Int(0, 100));

        return faker.Generate(number);
    }

    public static IEnumerable<Book> ListBook(int number)
    {
        var faker = new Faker<Book>()
            .RuleFor(r => r.Id, f => Guid.NewGuid())
            .RuleFor(r => r.Title, f => f.Random.Words(5))
            .RuleFor(r => r.Author, f => f.Name.FullName())
            .RuleFor(r => r.Description, f => f.Random.Words(20))
            .RuleFor(r => r.PublicationYear, f => f.Random.Int(1930, DateTime.UtcNow.Year))
            .RuleFor(r => r.Price, f => f.Random.Decimal(0.01m, 199.99m))
            .RuleFor(r => r.Genre, f => f.PickRandom<GenreBook>())
            .RuleFor(r => r.Stock, f => f.Random.Int(0, 100))
            .RuleFor(r => r.Created, f => f.Date.Future())
            .RuleFor(r => r.Updated, f => f.Date.Future());

        return faker.Generate(number);
    }

    public static Book Book()
    {
        return ListBook(1).FirstOrDefault() ?? new Book();
    }
}
