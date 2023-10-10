using FluentValidation;

namespace BackendAPI.Dtos;

public record UpdateObjectDto(string Description, string Image, double Grade);
public record ObjectDto(int Id, string Name, string Description, string Image, double Grade);
public record CreateObjectDto(string Name, string Description, string Image, double Grade);

public class CreateObjectDtoValidator : AbstractValidator<CreateObjectDto>
{
    public CreateObjectDtoValidator()
    {
        RuleFor(m => m.Name).NotEmpty().NotNull().MinimumLength(2);
        RuleFor(m => m.Description).NotEmpty().NotNull().MinimumLength(2);
        RuleFor(m => m.Image).NotEmpty().NotNull().MinimumLength(2);
        RuleFor(m => m.Grade).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
    }
}