using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Application.Commands.Orders ;


    public class CreateOrderCommand : CreateOrderDto, IRequest<CreateOrderResponse>;
    
    public class CreateOrderCommandHandler(IMapper mapper, IOrderRepository repository):IRequestHandler<CreateOrderCommand,CreateOrderResponse>
    {
        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);
            await repository.AddAsync(order);
            return new CreateOrderResponse(true, "Order Successfully Added");

        }
    }