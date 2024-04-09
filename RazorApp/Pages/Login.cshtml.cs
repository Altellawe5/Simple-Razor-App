using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Services;

namespace RazorApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly CustomerService _customerService;
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }

        public LoginModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                
                ModelState.AddModelError("", "Both first and last name must be provided.");
                return Page();
            }

            var customer = await _customerService.GetCustomerByName(FirstName, LastName);

            if (customer != null && customer.IsActive)
            {
                HttpContext.Session.SetString("CustomerId", customer.Id.ToString());

                return RedirectToPage("/MainPage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                ModelState.AddModelError(string.Empty, "Customer not found");
                return Page();
            }
        }
    }
}
