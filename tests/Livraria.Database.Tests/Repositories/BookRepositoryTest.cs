using FluentAssertions;
using Livraria.Database.Repositories;
using Livraria.Database.Tests.Fixtures;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Models.Data;

namespace Livraria.Database.Tests.Repositories;

[Collection("Database")]
public class BookRepositoryTest
{
    private readonly DatabaseFixture _fixture;

    public BookRepositoryTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Deve Incluir Livro Quando Receber Dados Validos")]
    public void Should_Add_WhenReceivingValidData()
    {
        // Arrange
        _fixture.Clear();
        Book model = _fixture.Book();
        IBookRepository repository = new BookRepository(_fixture.Contexto);

        // Act
        var actual = repository.Add(model);
        var expected = repository.GetById(model.Id);

        // Asserts
        actual.Should().BeTrue();
        expected.Should().BeEquivalentTo(model);
    }

    [Fact(DisplayName = "Deve Apagar Livro Quando Receber Dados Validos")]
    public void Should_Delete_WhenReceivingValidData()
    {
        // Arrange
        _fixture.Clear();
        Book model = _fixture.Book();
        _fixture.Add(model);
        IBookRepository repository = new BookRepository(_fixture.Contexto);

        // Act
        var actual = repository.Delete(model.Id);
        var expected = repository.GetById(model.Id);

        // Asserts
        actual.Should().BeTrue();
        expected.Should().BeNull();
    }

    [Fact(DisplayName = "Deve Retornar Todos Livro")]
    public void Should_GetAll()
    {
        // Arrange
        _fixture.Clear();
        Book model = _fixture.Book();
        _fixture.Add(model);
        IBookRepository repository = new BookRepository(_fixture.Contexto);

        // Act
        var actual = repository.GetAll();

        // Asserts
        actual.Any().Should().BeTrue();
        actual.Count().Should().Be(1);
        actual.FirstOrDefault().Should().BeEquivalentTo(model);
    }

    [Fact(DisplayName = "Deve Retornar Livro por Id")]
    public void Should_GetById()
    {
        // Arrange
        _fixture.Clear();
        Book model = _fixture.Book();
        _fixture.Add(model);
        IBookRepository repository = new BookRepository(_fixture.Contexto);

        // Act
        var actual = repository.GetById(model.Id);

        // Asserts
        actual.Should().BeEquivalentTo(model);
    }

    [Fact(DisplayName = "Deve Atualizar Livro Quando Receber Dados Validos")]
    public void Should_Update_WhenReceivingValidData()
    {
        // Arrange
        _fixture.Clear();
        Book model = _fixture.Book();
        Book newData = _fixture.Book();
        _fixture.Add(model);
        IBookRepository repository = new BookRepository(_fixture.Contexto);
        model.Title = newData.Title;
        model.Author = newData.Author;
        model.Description = newData.Description;

        // Act
        var actual = repository.Update(model);
        var expected = repository.GetById(model.Id);

        // Asserts
        actual.Should().BeTrue();
        expected.Should().BeEquivalentTo(model);
    }
}
