using FluentValidation;
using FluentValidation.Results;
using Livraria.Domain.Contracts.Models.Requests;

namespace Livraria.Domain.Validations;
public class BookRequestValidator : AbstractValidator<BookRequest>
{
    public BookRequestValidator()
    {
        RuleFor(r => r.Title)
            .NotEmpty().WithMessage("O titulo do livro é obrigatório.")
            .Length(3, 100).WithMessage("O titulo do livro deve ter entre 3 a 100 caracteres.");

        RuleFor(r => r.Author)
            .NotEmpty().WithMessage("O autor do livro é obrigatório.")
            .Length(3, 100).WithMessage("O autor do livro deve ter entre 3 a 100 caracteres.");

        RuleFor(r => r.Description)
            .NotEmpty().WithMessage("A descrição do livro é obrigatória.")
            .Length(3, 300).WithMessage("A descrição do livro deve ter entre 3 a 300 caracteres.");

        RuleFor(r => r.PublicationYear)
            .GreaterThanOrEqualTo(1930).WithMessage("O ano de publicação do livro de ser maior ou igual a 1930.");

        RuleFor(r => r.Price)
            .GreaterThan(0).WithMessage("O preço inicial do livro de ser maior que R$ 0,00.");

        RuleFor(r => r.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("O estoque inicial do livro de ser maior ou igual a 0.");
    }

    public override ValidationResult Validate(ValidationContext<BookRequest> context)
    {
        if (context is null || context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure>() { new(nameof(BookRequest), "A requisição não pode ser nula.") });
        }

        return base.Validate(context);
    }
}
