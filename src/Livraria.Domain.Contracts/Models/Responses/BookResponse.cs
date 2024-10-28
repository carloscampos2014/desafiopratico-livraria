using Livraria.Domain.Contracts.Models.Data;

namespace Livraria.Domain.Contracts.Models.Responses;

public class BookResponse
{
    public bool Success { get; set; } = true;

    public string ErrorMessage { get; set; } = string.Empty;

    public Book? Model { get; set; }

    public IEnumerable<Book>? Models { get; set; }
}
