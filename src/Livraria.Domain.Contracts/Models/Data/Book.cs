using Livraria.Domain.Contracts.Models.Requests;

namespace Livraria.Domain.Contracts.Models.Data;

public class Book : BookRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime Updated { get; set; } = DateTime.UtcNow;
}
