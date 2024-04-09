using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.REST.DTO
{
    public class CustomerDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; } // should be hashed
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string BusNumber { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public bool IsActive { get; set; }
    }
}
