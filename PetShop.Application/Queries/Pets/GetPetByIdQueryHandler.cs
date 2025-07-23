using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Application.Queries.Pets ;

    public class GetPetByIdQuery : IRequest<GetAllPetsResponse>
    {
        public Guid Id { get; init; }
    }


public class GetPetByIdQueryHandler(IMapper mapper, IPetRepository repository): IRequestHandler<GetPetByIdQuery, GetAllPetsResponse>
{
    public async Task<GetAllPetsResponse> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
    {
       var response = await repository.GetByIdAsync(request.Id);
        if (response == null)
        {
             return new GetAllPetsResponse(true, "Pet not found", []);
        }
        
        var mappedResponse = mapper.Map<PetDto>(response);
        return new GetAllPetsResponse(true, "Operation Succeeded", [mappedResponse]);
    }
}