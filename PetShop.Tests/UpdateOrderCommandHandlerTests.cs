using AutoMapper;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using PetShop.Application.Commands.Orders;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;
using PetShop.Domain.Enums;

namespace PetShop.Tests ;

    public class UpdateOrderCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOrderRepository> _orderRepoMock = new();
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<IPetRepository> _petRepoMock = new();
        private readonly UpdateOrderCommandHandler _handler;
        
        
        private readonly Guid _existingCustomerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        private readonly Guid _existingPetId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7");
        private readonly Guid _orderId = Guid.NewGuid();

        public UpdateOrderCommandHandlerTests()
        {
            _handler = new UpdateOrderCommandHandler(_mapperMock.Object, _orderRepoMock.Object);
            
            // Mock customer exists
            _customerRepoMock
                .Setup(repo => repo.GetByIdAsync(_existingCustomerId))
                .ReturnsAsync(new Customer
                {
                    Id = _existingCustomerId,
                    Email = "test@test.com",
                    FirstName = "Test",
                    LastName = "Test",
                    Address = "Test",
                    PhoneNumber = "07130843456",
                    
                });
           
            // Mock pet returned from DB
            _petRepoMock
                .Setup(repo => repo.GetByIdAsync(_existingPetId))
                .ReturnsAsync(new Pet
                {
                    id = _existingPetId,
                    Name = "Test Pet",
                    Price = 1000
                });
            
            // Mock existing order
            _orderRepoMock
                .Setup(repo => repo.GetByIdAsync(_orderId))
                .ReturnsAsync(new Order
                {
                    Id = _orderId,
                    CustomerId = _existingCustomerId,
                    PickupDate = DateTime.UtcNow.AddDays(2),
                    OrderStatus = EnumOrderStatus.Open, // Tests will fail if the Order status is changed
                    OrderItems = new List<OrderItem>
                    {
                        new() { PetId = _existingPetId, Name = "Test Pet", Price = 1000 }
                    }
                });
        }
        
        
        
        [Fact]
        public async Task Handle_Should_UpdateOrder_WhenValid()
        {
            var validator = new UpdateOrderCommandValidator(_orderRepoMock.Object, _customerRepoMock.Object);
            var command = new UpdateOrderCommand
            {
                Id = _orderId,
                CustomerId = _existingCustomerId ,
                PickupDate = DateTime.UtcNow.AddDays(3),
            };
            
            
            var result = await validator.TestValidateAsync(command);
            
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
        
        [Fact]
        public async Task Handle_Should_UpdateOrderItems_WhenValid()
        {
            var validator = new UpdateOrderItemsCommandValidator(_orderRepoMock.Object);
           
            var command = new UpdateOrderItemsCommand
            {
                OrderId = _orderId,
                OrderItems = new List<UpdateOrderItemDto>
                {
                    new() { Name = "Test Pet", Price = 2000,  PetId= _existingPetId } // Increase the Price
                }
            };
            
            
            var result = await validator.TestValidateAsync(command);
            
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }