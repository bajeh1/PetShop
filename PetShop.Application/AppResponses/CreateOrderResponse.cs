using PetShop.Application.Dtos;

namespace PetShop.Application.AppResponses ;

    public class CreateOrderResponse(bool success, string message) :BaseResponse(success, message);
    public class UpdateOrderResponse(bool success, string message) :BaseResponse(success, message);



    public class GetAllOrdersResponse(bool success, string message, List<OrderDto> orders) 
        : BaseResponse(success, message)
    {
        public List<OrderDto> Data { get; set; } = orders;
    }

    public class GetSingleOrdersResponse(bool success, string message , OrderDto orderDto) : BaseResponse(success, message)
    {
        public OrderDto Data { get; set; } = orderDto;
    }