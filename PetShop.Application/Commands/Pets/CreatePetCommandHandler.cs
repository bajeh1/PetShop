using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Application.Commands.Pets ;

    public class CreatePetCommand : IRequest<CreatePetResponse>
    {
        public string Name { get; set; } 
        public string PetKind { get; set; } 
        public string Breed { get; set; } 
        public string Description { get; set; } 
        public int Price { get; set; }
        public string Color { get; set; }
    } 

    public class CreatePetCommandHandler(IMapper mapper, IPetRepository repository):IRequestHandler<CreatePetCommand,CreatePetResponse>
    {
        public async Task<CreatePetResponse> Handle(CreatePetCommand request, CancellationToken cancellationToken)
        {
            var pet = mapper.Map<Pet>(request);
            await repository.AddAsync(pet);
            return new CreatePetResponse(true, "Pet Successfully Added");
        }
        
    }