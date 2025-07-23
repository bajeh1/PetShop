using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Queries.Orders ;

    public class GetOrderByIdQuery : IRequest<GetSingleOrdersResponse>
    {
        public Guid Id { get; set; }
    }
    public class GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository repository):IRequestHandler<GetOrderByIdQuery,GetSingleOrdersResponse>
    {
        public async Task<GetSingleOrdersResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await repository.GetByIdAsync(request.Id);
            var mappedOrder = mapper.Map<OrderDto>(response);
            return new GetSingleOrdersResponse(true, "Operation Succeeded",  mappedOrder);
        }
    }