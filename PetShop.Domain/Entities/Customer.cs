using System.ComponentModel.DataAnnotations;

namespace PetShop.Domain.Entities ;

    public class Customer
    {
        [Key]
        public Guid  Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public required string FirstName { get; set; }
        
        [Required(ErrorMessage = "First Name is required")] public required string LastName { get; init; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
        public ICollection<Order> Orders { get; set; } // A customer can have multiple Orders
        
    }