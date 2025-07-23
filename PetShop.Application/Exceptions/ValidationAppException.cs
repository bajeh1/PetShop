namespace PetShop.Application.Exceptions ;

    public class ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
        : Exception("One or more Validation errors occured")
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
    }