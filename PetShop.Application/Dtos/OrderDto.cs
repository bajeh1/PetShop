using System.ComponentModel.DataAnnotations;
using PetShop.Domain.Entities;
using PetShop.Domain.Enums;

namespace PetShop.Application.Dtos ;

    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PickupDate { get; set; }
        public EnumOrderStatus OrderStatus { get; set; } = EnumOrderStatus.Open;
        public double Discount { get; set; }
        public double ActualCost { get; set; }
        
        public string CustomerId { get; set; }
        
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
    
    
    public class OrderItemDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Guid PetId { get; set; }
        
        
    }
    
    
    public class CreateOrderDto
    {
        public DateTime PickupDate { get; set; }
        public Guid CustomerId { get; set; }
        
        public ICollection<CreateOrderItemDto> OrderItems { get; set; }
    }
    
    
    public class CreateOrderItemDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Guid PetId { get; set; }
        
    }


    public class UpdateOrderDto
    {
        public Guid Id { get; set; }
        public EnumOrderStatus? OrderStatus { get; set; }
        public DateTime? PickupDate { get; set; }
        public Guid? CustomerId { get; set; }
        
    }

    public class UpdateOrderItemsDto
    {
        public Guid OrderId { get; set; }
        public ICollection<UpdateOrderItemDto>? OrderItems { get; set; }
    }
    public class UpdateOrderItemDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Guid PetId { get; set; }
        
    }