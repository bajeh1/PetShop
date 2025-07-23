using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Application.Queries.Pets ;

    public class GetAllPetsQuery : IRequest<GetAllPetsResponse>;

    public class GetAllPetsQueryHandler(IMapper mapper, IPetRepository repository):IRequestHandler<GetAllPetsQuery, GetAllPetsResponse>
    {
        public async Task<GetAllPetsResponse> Handle(GetAllPetsQuery request, CancellationToken cancellationToken)
        {
            var response = await repository.GetAllAsync();
            var mappedResponse = mapper.Map<List<PetDto>>(response);
            return new GetAllPetsResponse(true, "Operation Successful", mappedResponse);
        }
    }