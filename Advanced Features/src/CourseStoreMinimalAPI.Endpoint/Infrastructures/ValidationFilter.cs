
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using FluentValidation;

namespace CourseStoreMinimalAPI.Endpoint.Infrastructures;

public class ValidationFilter<TModel> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
                                          EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<TModel>>();

        if (validator == null)
        {
            return await next(context);
        }

        var modelForValidate = context.Arguments.OfType<TModel>().FirstOrDefault();
        if (modelForValidate == null)
        {
            return Results.Problem("مدل ورودی قابل قبول نمی‌باشد");
        }

        var validationResult = await validator.ValidateAsync(modelForValidate);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await next(context);

        return result;
    }
}

public class LoggerFilter : IEndpointFilter
{
    private readonly ILogger logger;

    public LoggerFilter(ILoggerFactory loggerfactory)
    {
        this.logger = loggerfactory.CreateLogger<LoggerFilter>();
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
                                          EndpointFilterDelegate next)
    {
        string Path = context.HttpContext.Request.Path;

        logger.LogInformation("Start processing of {Path} at {time}", Path, DateTime.UtcNow);
        var result = await next(context);
        logger.LogInformation("Finish processing of {Path} at {time}", Path, DateTime.UtcNow);

        return result;
    }
}
