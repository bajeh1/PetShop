namespace PetShop.Application.Dtos ;

    public class CustomerDto
    {
        public Guid  Id { get; set; }
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double ActualPaymentDue { get; set; } 
        public double EstimatedPayment { get; set; } 
        
    }

    public class CreateCustomerDto
    {
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        
    }
    
    