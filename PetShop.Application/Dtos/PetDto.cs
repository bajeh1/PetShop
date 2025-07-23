namespace PetShop.Application.Dtos ;

    public class PetDto
    {
        public Guid id { get; set; }
        public string Name { get; set; } // e.g "Catlina"
        public string PetKind { get; set; } // e.g Cat, Dog, Fish
        public string Breed { get; set; } // German Shepherd, Gold Fish, 
        public string Description { get; set; } //  Any Description of the Pet
        public int Price { get; set; }
        public string Color { get; set; }
    }
    
    public class CreatePetDto
    {
        public string Name { get; set; } 
        public string PetKind { get; set; }
        public string Breed { get; set; }  
        public string Description { get; set; } 
        public int Price { get; set; }
        public string Color { get; set; }
    }