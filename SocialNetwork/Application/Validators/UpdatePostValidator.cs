using Application.Constants;
using Application.DTOs.Posts;
using FluentValidation;

namespace Application.Validators;

public class UpdatePostValidator : AbstractValidator<PostUpdateRequest>
{
    public UpdatePostValidator()
    {
        RuleFor(p => p.Content)
            .NotNull().When(p => p.ImageUrls == null)
            .NotEmpty().When(p => p.Content != null)
            .WithMessage("Content is required when there are no images");
        RuleFor(p => p.ImageUrls)
            .NotNull().When(p => p.Content == null)
            .Must(images => images == null || images.Any(string.IsNullOrEmpty))
            .WithMessage("At least one image must be provided if content is null");
        RuleFor(p => p.ImageUrls)
            .Must(images => images is not { Length: > ConstantValue.MaxImagesAllow })
            .WithMessage($"A maximum of {ConstantValue.MaxImagesAllow}images are allowed");
    }
}