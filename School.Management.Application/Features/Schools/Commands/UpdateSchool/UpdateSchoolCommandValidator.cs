using FluentValidation;

namespace SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommandValidator : AbstractValidator<UpdateSchoolCommand>
    {
        public UpdateSchoolCommandValidator()
        {
            RuleFor(p => p.School.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.School.Adress)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.School.Town)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
            RuleFor(p => p.School.Description)
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}
