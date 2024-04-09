// ProfilePageModel.cshtml.cs

using Menu.REST.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Services;

namespace RazorApp.Pages
{
    public class ProfilePageModel : PageModel
    {
        private readonly CustomerService _customerService;

        public ProfilePageModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public CustomerDTO Customer { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerId) || !Guid.TryParse(customerId, out var parsedCustomerId))
            {
                return RedirectToPage("/Login");
            }

            Customer = await _customerService.GetCustomerById(parsedCustomerId);
            if (Customer == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            Guid.TryParse(customerId, out var parsedId);
            Customer.Id = parsedId;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var updatedCustomer = await _customerService.GetCustomerById(Customer.Id);
            if (updatedCustomer == null)
            {
                return NotFound();
            }
            

            updatedCustomer.FirstName = Customer.FirstName;
            updatedCustomer.LastName = Customer.LastName;
            updatedCustomer.Email = Customer.Email;
            updatedCustomer.PhoneNumber = Customer.PhoneNumber;
            updatedCustomer.Password = Customer.Password;
            updatedCustomer.Street = Customer.Street;
            updatedCustomer.HouseNumber = Customer.HouseNumber;
            updatedCustomer.BusNumber = Customer.BusNumber;
            updatedCustomer.ZipCode = Customer.ZipCode;
            updatedCustomer.City = Customer.City;
            updatedCustomer.Country = Customer.Country;

            await _customerService.UpdateCustomer(updatedCustomer);
            return RedirectToPage("/MainPage");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToPage("/Login");
            }
            Guid.TryParse(customerId, out var parsedId);

            await _customerService.DeleteCustomer(parsedId);

            // Log out the customer
            HttpContext.Session.Clear();

            return RedirectToPage("/Index");
        }
    }
}
