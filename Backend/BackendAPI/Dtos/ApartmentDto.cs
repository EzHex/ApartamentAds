using FluentValidation;

namespace BackendAPI.Dtos;

public record ApartmentDto(int Id, string Address, int Floor, int Number, double Area, double Rating);
public record CreateApartmentDto(string Address, int Floor, int Number, double Area, double Rating);
public record UpdateApartmentDto(double Rating);

public class CreateApartmentDtoValidator : AbstractValidator<CreateApartmentDto>
{
    public CreateApartmentDtoValidator()
    {
        RuleFor(m => m.Address).NotEmpty().NotNull().MinimumLength(2);
        RuleFor(m => m.Floor).NotEmpty().NotNull().GreaterThan(-1);
        RuleFor(m => m.Number).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(m => m.Area).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(m => m.Rating).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
    }
}

public class UpdateApartmentDtoValidator : AbstractValidator<UpdateApartmentDto>
{
    public UpdateApartmentDtoValidator()
    {
        RuleFor(m => m.Rating).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
    }
}