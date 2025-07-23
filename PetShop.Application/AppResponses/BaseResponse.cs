namespace PetShop.Application.AppResponses ;

    public class BaseResponse(bool success, string message)
    {
        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
    }