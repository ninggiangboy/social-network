using Application.DTOs.Replies;
using FluentValidation;

namespace Application.Validators;

public class ReplyCreateValidator : AbstractValidator<ReplyCreateRequest>
{
    public ReplyCreateValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required")
            .MaximumLength(500)
            .WithMessage("Content must not exceed 500 characters");
    }
}