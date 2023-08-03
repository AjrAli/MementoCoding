using FluentValidation;

namespace ManagementProject.Application.Models.Account.Command
{
    public class CreateSimpleUserCommandValidator : AbstractValidator<CreateSimpleUserCommand>
    {
        public CreateSimpleUserCommandValidator()
        {
            RuleFor(p => p.Account.Username)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Account.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MinimumLength(4).WithMessage("{PropertyName} must have at least 4 characters/digits.");
            RuleFor(p => p.Account.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Account.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Account.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress().WithMessage("{PropertyName} doesn't have a correct email address.");
        }
    }
}
