using PetShop.Application.Exceptions;

namespace PetShop.Application.Behaviors ;

using FluentValidation;
using MediatR;

    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators):IPipelineBehavior<TRequest, TResponse>
        where TRequest:IRequest<TResponse>
    {
    

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
                return await next();
        
            var context = new ValidationContext<TRequest>(request);
        
            var validationFailures = await Task.WhenAll(validators
                .Select(v => v.ValidateAsync(context, cancellationToken)));
        

            var failures = validationFailures
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .GroupBy(
                    x=>x.PropertyName.Substring(x.PropertyName.IndexOf('.') +1),
                    x=>x.ErrorMessage, (propertyName,errorMessages)=> new
                    {
                        Key =propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    } )
            
                .ToDictionary(x=> x.Key, x=> x.Values);

            if (failures.Count <= 0) return await next();
            {
                throw new ValidationAppException(failures);
            }

        }
    }