using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Application.Commands ;

    public class CreateCustomerCommand : IRequest<CreateCustomerResponse>
    {
        public  string FirstName { get; set; } = string.Empty;
        public  string LastName { get; set; }  = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }


    public class CreateCustomerCommandHandler(IMapper mapper, ICustomerRepository repository):IRequestHandler<CreateCustomerCommand,CreateCustomerResponse >
    {
        public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = mapper.Map<Customer>(request);
            await repository.AddAsync(customer);
            return new CreateCustomerResponse(true, "Customer Successfully Added");
        }
    }