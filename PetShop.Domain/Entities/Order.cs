using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetShop.Domain.Enums;

namespace PetShop.Domain.Entities ;

    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PickupDate { get; set; }
        public EnumOrderStatus OrderStatus { get; set; } = EnumOrderStatus.Open;
        public double Discount { get; set; }
        public double ActualCost { get; set; }
        public double Shipping { get; set; }
        
       [Required] public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }

    }
