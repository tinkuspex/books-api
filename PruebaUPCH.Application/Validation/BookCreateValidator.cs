using FluentValidation;
using PruebaUPCH.Application.DTOs;

namespace PruebaUPCH.Application.Validation
{
    public class BookCreateValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().MaximumLength(200);

            RuleFor(x => x.Author)
                .NotEmpty().MaximumLength(150);

            RuleFor(x => x.PublicationYear)
                .InclusiveBetween(1000, DateTime.UtcNow.Year);

            RuleFor(x => x.PageCount)
                .GreaterThan(0);
        }
    }
}
