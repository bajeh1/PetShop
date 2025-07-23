
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using PetShop.Application.Exceptions;


namespace PetShop.Application.Behaviors ;

    public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService):IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            httpContext.Response.ContentType = "application/json";
            var execDetails = exception switch
            {
                ValidationAppException=> (Detail: exception.Message,StatusCode: StatusCodes.Status422UnprocessableEntity),
                _ => (Detail: exception.Message, StatusCode: StatusCodes.Status500InternalServerError)
                };

            httpContext.Response.StatusCode = execDetails.StatusCode;

            if (exception is ValidationAppException validationAppException)
            {
                await httpContext.Response.WriteAsJsonAsync(new { validationAppException.Errors }, cancellationToken);
                return true;
            }
        
            var problem = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Title = "An Error Occurred",
                    Type = exception.GetType().Name,
                    Status = execDetails.StatusCode
                },
                // Exception = exception
            });
            return problem;
        }
    }