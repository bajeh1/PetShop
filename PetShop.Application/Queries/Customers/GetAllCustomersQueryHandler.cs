using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Application.Queries.Customers ;

    public class GetAllCustomersQuery : IRequest<GetAllCustomersResponse>;

    public class GetAllCustomersQueryHandler(IMapper mapper, ICustomerRepository customerRepository):IRequestHandler<GetAllCustomersQuery, GetAllCustomersResponse>
    {
        public async Task<GetAllCustomersResponse> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var response = await customerRepository.GetAllAsync();
            var mappedResponse = mapper.Map<List<CustomerDto>>(response);
            return new GetAllCustomersResponse(true, "Operation Successful", mappedResponse);
        }
    }