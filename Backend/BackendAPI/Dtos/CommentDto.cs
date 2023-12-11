using FluentValidation;

namespace BackendAPI.Dtos;

public record CommentDto(int Id, string Content);
public record CreateCommentDto(string Content);

public class CommentDtoValidator : AbstractValidator<CommentDto>
{
    public CommentDtoValidator()
    {
        RuleFor(m => m.Content).NotEmpty().NotNull().MinimumLength(1);
    }
}