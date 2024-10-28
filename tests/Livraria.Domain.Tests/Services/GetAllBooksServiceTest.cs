using FluentAssertions;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Responses;
using Livraria.Domain.Services;
using Moq;

namespace Livraria.Domain.Tests.Services;

public class GetAllBooksServiceTest
{
    [Fact(DisplayName = "Deve Retornar uma Resposta com Falha Quando Ocorrer um Erro no Repositório.")]
    public void Should_ReturnFailedResponse_WhenErrorOccursRepository()
    {
        // Arrange
        var expected = new BookResponse()
        {
            Success = false,
            ErrorMessage = "A operação não é válida para o estado atual do repositório.",
        };
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetAll())
            .Throws(new InvalidOperationException(expected.ErrorMessage));
        IGetAllBookService service = new GetAllBookService(repositoryMock.Object);

        // Act
        var actual = service.Execute();

        // Asset
        actual.Success.Should().BeFalse();
        actual.ErrorMessage.Should().Be(expected.ErrorMessage);
        repositoryMock.Verify(v => v.GetAll(), Times.Exactly(1));
    }

    [Fact(DisplayName = "Deve Retornar uma Resposta com Lista Vazia Quando Não Encontrar Registros.")]
    public void Should_ReturnEmptyListResponse_WhenNoRecordsAreFound()
    {
        // Arrange
        var expected = new BookResponse()
        {
            Success = true,
        };
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetAll())
            .Returns(expected.Models);
        IGetAllBookService service = new GetAllBookService(repositoryMock.Object);

        // Act
        var actual = service.Execute();

        // Asset
        actual.Success.Should().BeTrue();
        actual.Models.Should().BeEquivalentTo(expected.Models);
        repositoryMock.Verify(v => v.GetAll(), Times.Exactly(1));
    }

    [Fact(DisplayName = "Deve Retornar uma Resposta com Lista Preenchida Quando Encontrar Registros.")]
    public void Should_ReturnPopuletedListResponse_WhenRecordsAreFound()
    {
        // Arrange
        var expected = new BookResponse()
        {
            Success = true,
            Models = ModelFaker.ListBook(10),
        };
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetAll())
            .Returns(expected.Models);
        IGetAllBookService service = new GetAllBookService(repositoryMock.Object);

        // Act
        var actual = service.Execute();

        // Asset
        actual.Success.Should().BeTrue();
        actual.Models.Should().BeEquivalentTo(expected.Models);
        repositoryMock.Verify(v => v.GetAll(), Times.Exactly(1));
    }
}
