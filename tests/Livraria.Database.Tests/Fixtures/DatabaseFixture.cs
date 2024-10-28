using Bogus;
using Livraria.Database.Context;
using Livraria.Domain.Contracts.Enums;
using Livraria.Domain.Contracts.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Database.Tests.Fixtures;

public class DatabaseFixture : IDisposable
{
    private readonly DbContextOptions<LivrariaDbContext> _options;
    private readonly LivrariaDbContext _context;

    public DatabaseFixture()
    {
        // Configura o banco de dados em memória
        _options = new DbContextOptionsBuilder<LivrariaDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        _context = new LivrariaDbContext(_options);
        _context.Database.OpenConnection(); // Abre a conexão com o banco de dados em memória
        _context.Database.Migrate(); // Aplica as migrações
    }

    public LivrariaDbContext Contexto => _context;

    public Book Book()
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

        return faker.Generate();
    }

    public void Add(Book model)
    {
        _context.Books.Add(model);
        _context.SaveChanges();
    }

    public void Clear()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM Books");
    }

    public void Dispose()
    {
        _context?.Database.CloseConnection();
        _context?.Dispose();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // Essa classe não contém código, serve apenas como ponto de definição.
}

