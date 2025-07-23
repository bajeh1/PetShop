using System.ComponentModel.DataAnnotations;

namespace PetShop.Domain.Entities ;

    public class Pet
    {
        [Key]
        public Guid id { get; set; }
        [Required] public string Name { get; set; } // e.g "Catlina"
        [Required] public string PetKind { get; set; } // e.g Cat, Dog, Fish
        public string Breed { get; set; } // German Shepherd, Gold Fish, 
        public string Description { get; set; } //  Any Description of the Pet
        [Required] public int Price { get; set; }
        public string Color { get; set; }
        
    }