using CourseStoreMinimalAPI.ApplicationServices;
using FluentValidation;

namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator(CategoryService categoryService,IHttpContextAccessor httpContextAccessor)
    {
        int id = 0;
        if(httpContextAccessor?.HttpContext?.Request.RouteValues.ContainsKey("id") == true)
        {
            var routId = httpContextAccessor.HttpContext.Request.RouteValues["id"];
            id = int.Parse(routId.ToString());
        }

        RuleFor(c => c.Name).NotEmpty().WithMessage(ValidationMessages.REQUIRED).WithName(PropertyName.Name)
            .MaximumLength(50).WithMessage(ValidationMessages.MAX_LENGHT)
            .MinimumLength(2).WithMessage(ValidationMessages.MIN_LENGHT)
            .MustAsync(async (c,CancellationToken) =>
            {
                return !(await categoryService.IsRepeated(id, c));
            }).WithMessage(ValidationMessages.REPEATED_VALUE);
    }
}
