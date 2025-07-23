using AutoMapper;
using MediatR;
using PetShop.Application.AppResponses;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Application.Commands.Orders ;


    public class UpdateOrderCommand : UpdateOrderDto, IRequest<UpdateOrderResponse>;
    
    public class UpdateOrderCommandHandler(IMapper mapper, IOrderRepository repository):IRequestHandler<UpdateOrderCommand,UpdateOrderResponse>
    {
        public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            await repository.UpdateAsync(request);
            return new UpdateOrderResponse(true, "Order Successfully Updated");
        }
    }
    
    
    public class UpdateOrderItemsCommand : UpdateOrderItemsDto, IRequest<UpdateOrderResponse>;
    
    public class UpdateOrderItemsCommandHandler(IOrderRepository repository):IRequestHandler<UpdateOrderItemsCommand,UpdateOrderResponse>
    {
        public async Task<UpdateOrderResponse> Handle(UpdateOrderItemsCommand request, CancellationToken cancellationToken)
        {
            await repository.UpdateOrderItemsAsync(request);
            return new UpdateOrderResponse(true, "Order Items Successfully Updated");
        }
    }