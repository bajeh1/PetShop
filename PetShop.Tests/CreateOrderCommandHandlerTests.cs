using AutoMapper;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using PetShop.Application.Commands.Orders;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Domain.Entities;

namespace PetShop.Tests ;

    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOrderRepository> _orderRepoMock = new();
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<IPetRepository> _petRepoMock = new();
        private readonly CreateOrderCommandHandler _handler;
        
        
        private readonly Guid _existingCustomerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        private readonly Guid _existingPetId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7");

        public CreateOrderCommandHandlerTests()
        {
            _handler = new CreateOrderCommandHandler(_mapperMock.Object, _orderRepoMock.Object);
            
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

          
        }
        
        
        
        [Fact]
        public async Task Handle_Should_CreateOrder_WhenValid()
        {
            var validator = new CreateOrderCommandValidator(_customerRepoMock.Object, _petRepoMock.Object);
            var command = new CreateOrderCommand
            {
                CustomerId = _existingCustomerId ,
                PickupDate = DateTime.UtcNow.AddDays(3),
                OrderItems = new List<CreateOrderItemDto>
                {
                    new() { Name = "Test Pet", Price = 1000,  PetId= _existingPetId }
                }
            };
            
            
            var result = await validator.TestValidateAsync(command);
            
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
            
            var addResponse = await _handler.Handle(command, default);
            
            addResponse.Success.Should().BeTrue(); // Assert that Order was added successfuly
        }
    }