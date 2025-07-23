using FluentValidation;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Commands.Orders ;

    public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPetRepository _petRepository;
        public CreateOrderCommandValidator( ICustomerRepository customerRepository, IPetRepository petRepository )
        {
            _customerRepository = customerRepository;
            _petRepository = petRepository;
            
            RuleFor(x=>x.CustomerId).NotEmpty().WithMessage("Customer Id is Required");

            // Ensure Valid Customer
            RuleFor(x => x).CustomAsync(async (dto, context, ct) =>
            {
                var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
                if (customer == null)
                {
                    context.AddFailure("CustomerId", "Provide a valid Customer");
                    return;
                }
            });
            
            RuleFor(x => x.PickupDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Pickup date must be in the future");
            
            RuleFor(x => x.OrderItems)
                .NotNull().WithMessage("Order must contain at least one pet")
                .Must(items => items.Count > 0).WithMessage("At least one pet is required");
            
            RuleForEach(x => x.OrderItems)
                .SetValidator(new CreateOrderItemDtoValidator(_petRepository));
            
        }
    }
    
    
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        private readonly IPetRepository _petRepository;
        public CreateOrderItemDtoValidator(IPetRepository petRepository)
        {
            _petRepository = petRepository;
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Pet name is required");
            
            RuleFor(x => x.PetId)
                .NotEmpty().WithMessage("Pet Id is required");

            // Ensure Valid Pet
            RuleFor(x => x).CustomAsync(async (dto, context, ct) =>
            {
                var customer = await _petRepository.GetByIdAsync(dto.PetId);
                if (customer == null)
                {
                    context.AddFailure("PetId", "Provide a valid Pet");
                    return;
                }
            });
            
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Pet price must be greater than 0");
            
        }
    }