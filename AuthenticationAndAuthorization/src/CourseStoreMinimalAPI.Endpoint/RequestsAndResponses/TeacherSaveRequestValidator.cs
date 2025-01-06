using FluentValidation;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;

public class TeacherSaveRequestValidator : AbstractValidator<TeacherSaveRequest>
{
    public TeacherSaveRequestValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage(ValidationMessages.REQUIRED).WithName(PropertyName.FirstName)
            .MaximumLength(50).WithMessage(ValidationMessages.MAX_LENGHT)
            .MinimumLength(2).WithMessage(ValidationMessages.MIN_LENGHT);
        RuleFor(c => c.LastName).NotEmpty().WithMessage(ValidationMessages.REQUIRED).WithName(PropertyName.LastName)
            .MaximumLength(50).WithMessage(ValidationMessages.MAX_LENGHT)
            .MinimumLength(2).WithMessage(ValidationMessages.MIN_LENGHT);
    }
}
