namespace Livraria.Domain.Contracts.Models.Requests;

public class BookRequest
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int PublicationYear { get; set; } = DateTime.UtcNow.Year;

    public decimal Price { get; set; }

    public int Stock { get; set; }
}
