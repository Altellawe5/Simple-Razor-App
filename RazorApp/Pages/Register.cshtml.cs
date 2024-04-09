using Menu.REST.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Services;

namespace RazorApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly CustomerService _customerService;

        public RegisterModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Street { get; set; }

        [BindProperty]
        public string HouseNumber { get; set; }

        [BindProperty]
        public string BusNumber { get; set; }

        [BindProperty]
        public string ZipCode { get; set; }

        [BindProperty]
        public string City { get; set; }

        [BindProperty]
        public string Country { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var customerDto = new CustomerDTO
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Password = Password,
                Street = Street,
                HouseNumber = HouseNumber,
                BusNumber = BusNumber,
                ZipCode = ZipCode,
                City = City,
                Country = Country
            };
            await _customerService.CreateCustomer(customerDto);
            return RedirectToPage("/Login");
        }
    }


}
