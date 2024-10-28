using FluentAssertions;
using Livraria.Domain.Contracts.Interfaces.Repositories;
using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Data;
using Livraria.Domain.Contracts.Models.Responses;
using Livraria.Domain.Services;
using Moq;

namespace Livraria.Domain.Tests.Services;

public class DeleteBookServiceTest
{
    [Fact(DisplayName = "Deve Retornar uma Resposta com Falha Quando Ocorrer um Erro no Repositório.")]
    public void Should_ReturnFailedResponse_WhenErrorOccursRepository()
    {
        // Arrange
        Book model = ModelFaker.Book();
        var expected = new BookResponse()
        {
            Success = false,
            ErrorMessage = "A operação não é válida para o estado atual do repositório.",
        };
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetById(model.Id))
            .Throws(new InvalidOperationException(expected.ErrorMessage));
        repositoryMock
            .Setup(x => x.Delete(model.Id))
            .Returns(true);
        IDeleteBookService service = new DeleteBookService(repositoryMock.Object);

        // Act
        var actual = service.Execute(model.Id);

        // Asset
        actual.Success.Should().BeFalse();
        actual.ErrorMessage.Should().Be(expected.ErrorMessage);
        repositoryMock.Verify(v => v.GetById(model.Id), Times.Exactly(1));
        repositoryMock.Verify(v => v.Delete(model.Id), Times.Never);
    }

    [Fact(DisplayName = "Deve Retornar uma Resposta com Falha Quando Não Encontrar Registro.")]
    public void Should_ReturnFailedResponse_WhenErrorNotFoundRegistre()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        Book model = null;
        var expected = new BookResponse()
        {
            Success = false,
            ErrorMessage = $"Nenhum livro encontrado com Id:{id}.",
        };
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetById(id))
            .Returns(model);
        repositoryMock
            .Setup(x => x.Delete(id))
            .Returns(true);
        IDeleteBookService service = new DeleteBookService(repositoryMock.Object);

        // Act
        var actual = service.Execute(id);

        // Asset
        actual.Success.Should().BeFalse();
        actual.ErrorMessage.Should().Be(expected.ErrorMessage);
        repositoryMock.Verify(v => v.GetById(id), Times.Exactly(1));
        repositoryMock.Verify(v => v.Delete(id), Times.Never);
    }

    [Fact(DisplayName = "Deve Retornar uma Resposta com Sucesso Quando Encontrar o Registro.")]
    public void Should_ReturnSucessResponse_WhenFoundRegister()
    {
        // Arrange
        Book model = ModelFaker.Book();
        var expected = new BookResponse()
        {
            Model = model,
        };
        var repositoryMock = new Mock<IBookRepository>();
        repositoryMock
            .Setup(x => x.GetById(model.Id))
            .Returns(model);
        repositoryMock
            .Setup(x => x.Delete(model.Id))
            .Returns(true);
        IDeleteBookService service = new DeleteBookService(repositoryMock.Object);

        // Act
        var actual = service.Execute(model.Id);

        // Asset
        actual.Success.Should().BeTrue();
        actual.Model.Should().Be(expected.Model);
        repositoryMock.Verify(v => v.GetById(model.Id), Times.Exactly(1));
        repositoryMock.Verify(v => v.Delete(model.Id), Times.Exactly(1));
    }
}
