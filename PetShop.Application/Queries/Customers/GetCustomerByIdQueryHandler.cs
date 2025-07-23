using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;
using PetShop.Domain.Enums;

namespace PetShop.Application.Queries.Customers ;

    public class GetCustomerByIdQuery : IRequest<GetAllCustomersResponse>
    {
        public Guid Id { get; init; }
    }


public class GetCustomerByIdQueryHandler(IMapper mapper, ICustomerRepository customerRepository): IRequestHandler<GetCustomerByIdQuery, GetAllCustomersResponse>
{
    public async Task<GetAllCustomersResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
       var response = await customerRepository.GetByIdAsync(request.Id);
        if (response == null)
        {
             return new GetAllCustomersResponse(true, "Customer not found", []);
        }
        
        var actualPaymentDue = response.Orders.Where(o => o.OrderStatus == EnumOrderStatus.Delivered)
            .Sum(o => o.ActualCost);

        var estimatedPayment = response.Orders.Where(o => o.OrderStatus != EnumOrderStatus.Delivered)
            .Sum(o => o.OrderItems.Sum(oi => oi.Price));
        
        
        
        var mappedCustomers = mapper.Map<CustomerDto>(response);
        mappedCustomers.ActualPaymentDue = actualPaymentDue;
        mappedCustomers.EstimatedPayment = estimatedPayment;
        
        return new GetAllCustomersResponse(true, "Operation Succeeded",  new List<CustomerDto>{mappedCustomers});
    }
}