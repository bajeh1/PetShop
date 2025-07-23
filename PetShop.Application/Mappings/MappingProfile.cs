using AutoMapper;
using PetShop.Application.Commands;
using PetShop.Application.Commands.Orders;
using PetShop.Application.Commands.Pets;
using PetShop.Application.Dtos;
using PetShop.Domain.Entities;

namespace PetShop.Application.Mappings ;

    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Customer Dtos
            CreateMap<CreateCustomerDto, CreateCustomerCommand>().ReverseMap();
            CreateMap<CreateCustomerCommand,Customer>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            
            
            //Pets Mapping
            CreateMap<CreatePetDto, CreatePetCommand>().ReverseMap();
            CreateMap<CreatePetCommand,Pet>().ReverseMap();
            CreateMap<Pet, PetDto>().ReverseMap();
            
            
            //Orders Mapping
            CreateMap<CreateOrderDto, CreateOrderCommand>().ReverseMap();
            CreateMap<UpdateOrderDto, UpdateOrderCommand>().ReverseMap();
            CreateMap<UpdateOrderCommand,Order>().ReverseMap();
            CreateMap<CreateOrderCommand, Order>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            
            
            //Order Items
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<CreateOrderItemDto, OrderItem>().ReverseMap();
            CreateMap<UpdateOrderItemDto, OrderItem>().ReverseMap();
            CreateMap<UpdateOrderItemsCommand, UpdateOrderItemsDto>().ReverseMap();


        }
        
    }