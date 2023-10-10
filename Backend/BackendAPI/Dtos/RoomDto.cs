using FluentValidation;

namespace BackendAPI.Dtos;

public record RoomDto(int Id, string Name, double Grade);
public record CreateRoomDto(string Name, double Grade);

public class CreateRoomDtoValidator : AbstractValidator<RoomDto>
{
    public CreateRoomDtoValidator()
    {
        RuleFor(m => m.Name).NotEmpty().NotNull().MinimumLength(2);
        RuleFor(m => m.Grade).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
    }
}

