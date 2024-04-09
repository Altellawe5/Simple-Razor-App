namespace Menu.Domain.Models
{
    // Geen EF code: nergens bijvoorbeeld [Key]!
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string BusNumber { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
    }
}