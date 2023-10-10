using FluentValidation;

namespace BackendAPI.Dtos;

public record CommentDto(string Content);

public class CommentDtoValidator : AbstractValidator<CommentDto>
{
    public CommentDtoValidator()
    {
        RuleFor(m => m.Content).NotEmpty().NotNull().MinimumLength(1);
    }
}