using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Queries.Orders ;

    public class GetAllOrdersQuery : IRequest<GetAllOrdersResponse>;

    public class GetAllOrdersQueryHandler(IMapper mapper, IOrderRepository repository):IRequestHandler<GetAllOrdersQuery,GetAllOrdersResponse>
    {
        public async Task<GetAllOrdersResponse> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var response = await repository.GetAllAsync();
            var mappedResponse = mapper.Map<List<OrderDto>>(response);
            return new GetAllOrdersResponse(true, "Operation Successful", mappedResponse);

        }
    }