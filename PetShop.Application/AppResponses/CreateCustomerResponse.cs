using PetShop.Application.Dtos;
using PetShop.Domain.Entities;

namespace PetShop.Application.AppResponses ;

    public class CreateCustomerResponse(bool success, string message) :BaseResponse(success, message);

    public class GetAllCustomersResponse(bool success, string message, List<CustomerDto> customers ) : BaseResponse(success, message)
    {
        public List<CustomerDto> Data { get; set; } = customers;
    }