using FluentValidation;

namespace BackendAPI.Dtos;

public record AdvertisementDto(int Id, string Title, string Description, double Price, DateTime Date);

public record AdvertisementWithOwnerDataDto(string Title, string Description, double Price, DateTime Date);

public record CreateAdvertisementDto(string Title, string Description, double Price, int ApartmentId);

public class CreateAdvertisementDtoValidator : AbstractValidator<CreateAdvertisementDto>
{
    public CreateAdvertisementDtoValidator()
    {
        RuleFor(m => m.Title).NotEmpty().NotNull().MinimumLength(2);
        RuleFor(m => m.Description).NotEmpty().NotNull().MinimumLength(2);
        RuleFor(m => m.ApartmentId).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(m => m.Price).NotEmpty().NotNull().GreaterThan(0);
    }
}