using Craft.Domain.Exceptions;
using Craft.Security.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Craft.Infrastructure.ApiControllers;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ModelValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(AlreadyExistsException), HandleAlreadyExistsException },
                { typeof(InvalidCredentialsException), HandleInvalidCredentialsException },
            };
    }

    private static void HandleInvalidModelStateException(ExceptionContext context)
    {
        ValidationProblemDetails details = new(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        ProblemDetails details = new()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private void HandleAlreadyExistsException(ExceptionContext context)
    {
        AlreadyExistsException exception = context.Exception as AlreadyExistsException;

        ProblemDetails details = new()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "The specified resource already exists.",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleException(ExceptionContext context)
    {
        Log.Error(context.Exception, "Handling exception:");

        Type type = context.Exception.GetType();

        if (_exceptionHandlers.TryGetValue(type, out Action<ExceptionContext> value))
        {
            value.Invoke(context);
            return;
        }

        // Model state represents errors that come from two subsystems: model binding and model validation.
        // Errors that originate from model binding are generally data conversion errors.
        // Model validation occurs after model binding and reports errors where data doesn't conform to business rules.
        // Both model binding and model validation occur before the execution of a controller action handler method.
        // Web API controllers don't have to check ModelState.IsValid if they have the [ApiController] attribute.
        // In that case, an automatic HTTP 400 response containing error details is returned when model state is invalid.
        // This is still added here in case we no longer clear the default MVC model validators.
        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleInvalidCredentialsException(ExceptionContext context)
    {
        var exception = context.Exception as InvalidCredentialsException;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Title = "Invalid Username and/or Password.",
            Detail = exception.Message
        };

        context.Result = new UnauthorizedObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        NotFoundException exception = context.Exception as NotFoundException;

        ProblemDetails details = new()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        // MS reference: https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-5.0#automatic-http-400-responses
        ModelValidationException exception = context.Exception as ModelValidationException;

        ValidationProblemDetails details = new(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }
}
