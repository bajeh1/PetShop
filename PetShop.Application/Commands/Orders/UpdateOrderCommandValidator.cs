using FluentValidation;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Enums;

namespace PetShop.Application.Commands.Orders ;

    public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        
        public UpdateOrderCommandValidator(IOrderRepository repository, ICustomerRepository customerRepository)
        {
            _orderRepository = repository;
            
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Order Id  is required");

            RuleFor(x => x).CustomAsync(async (dto, context, ct) =>
            {
                var order = await _orderRepository.GetByIdAsync(dto.Id);
                if (order == null)
                {
                    context.AddFailure("OrderId", "Order not found");
                    return;
                }
                if (order.OrderStatus == EnumOrderStatus.Delivered)
                {
                    context.AddFailure("OrderId", "Order is already delivered and cannot be updated");
                    return;
                }

                if (dto.PickupDate.HasValue && dto.PickupDate < DateTime.Now)
                {
                    context.AddFailure("PickupDate", "Pickup date cannot be in the past");
                    return;
                }
              
                if (order.OrderStatus == EnumOrderStatus.Processing && dto.CustomerId.HasValue)
                {
                    context.AddFailure("OrderStatus", "Only Pickup Date can be changed when order is in 'Processing' status");
                }

            });
            

        }
        
    }

    public class UpdateOrderItemsCommandValidator : AbstractValidator<UpdateOrderItemsCommand>
    {
        private readonly IOrderRepository _orderRepository;
        
        public UpdateOrderItemsCommandValidator(IOrderRepository repository)
        {
            _orderRepository = repository;
            RuleFor(x => x).CustomAsync(async (dto, context, ct) =>
            {
                var order = await _orderRepository.GetByIdAsync(dto.OrderId);
                if (order == null)
                {
                    context.AddFailure("OrderId", "Order not found");
                    return;
                }
                if (order.OrderStatus != EnumOrderStatus.Open)
                {
                    context.AddFailure("OrderId", "Order Items  cannot be updated when order is in this status");
                    return;
                }
                

            });
            
            RuleForEach(x => x.OrderItems)
                .SetValidator(new UpdateOrderItemDtoValidator());
        }
       
    }
    
    public class UpdateOrderItemDtoValidator : AbstractValidator<UpdateOrderItemDto>
    {
        public UpdateOrderItemDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Pet name is required");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Pet price must be greater than 0");
            
        }
    }