using System.ComponentModel.DataAnnotations;

namespace PetShop.Domain.Entities ;

    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }

       [Required] public string Name { get; set; }
        public int Quantity { get; set; } = 1;
        public double Price { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        
        
        // Reference to Pets Table
        public Guid PetId { get; set; }
        public Pet Pet { get; set; }
    }