using FluentValidation;

namespace BackendAPI.Dtos;

public record UserDto(int Id, string Login, string Email, string Password, string Name, DateTime LastLogin);

public record CreateUserDto(string Login, string Email, string Password, string Name);

public record UpdateUserDto(string Password, string Name);

public record UserAdvertisementDto(string Email);

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Login).NotEmpty().Length(min: 3, max: 50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
