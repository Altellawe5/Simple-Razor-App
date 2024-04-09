using Menu.REST.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Services;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace RazorApp.Pages
{
    
    public class MainPageModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly DishService _dishService;
        private readonly OrderService _orderService;

        public MainPageModel(CustomerService customerService, DishService dishService, OrderService orderService)
        {
            _customerService = customerService;
            _dishService = dishService;
            _orderService = orderService;
        }
        [BindProperty]
        public CustomerDTO Customer { get; set; }

        [BindProperty]
        public List<DishDTO> Dishes { get; set; }

        public List<OrderItemDTO> SelectedItems { get; set; } = new List<OrderItemDTO>();

        
        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve customer information
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (!string.IsNullOrEmpty(customerId))
            {
                Customer = await _customerService.GetCustomerById(Guid.Parse(customerId));
            }
            else
            {
                // Redirect to login if customer is not logged in
                return RedirectToPage("/Login");
            }
            // items 
            var selectedItemsJson = HttpContext.Session.GetString("SelectedItems");
            if (!string.IsNullOrEmpty(selectedItemsJson))
            {
                SelectedItems = JsonConvert.DeserializeObject<List<OrderItemDTO>>(selectedItemsJson);
            }
            // Get Dishes
            Dishes = await _dishService.GetAllDishes();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid dishId, int quantity)
        {
            var selectedItemsJson = HttpContext.Session.GetString("SelectedItems");
            if (!string.IsNullOrEmpty(selectedItemsJson))
            {
                SelectedItems = JsonConvert.DeserializeObject<List<OrderItemDTO>>(selectedItemsJson);
            }


            var dish = await _dishService.GetDishById(dishId);
            if (dish != null)
            {
                var orderItem = new OrderItemDTO
                {
                    DishId = dish.Id,
                    Quantity = quantity,
                    Id = Guid.NewGuid()                    
                };

                SelectedItems.Add(orderItem);

                HttpContext.Session.SetString("SelectedItems", JsonConvert.SerializeObject(SelectedItems));

            }
            // Refresh the Dishes
            Dishes = await _dishService.GetAllDishes();


            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromCartAsync(Guid dishId)
        {
            var selectedItemsJson = HttpContext.Session.GetString("SelectedItems");
            if (!string.IsNullOrEmpty(selectedItemsJson))
            {
                SelectedItems = JsonConvert.DeserializeObject<List<OrderItemDTO>>(selectedItemsJson);
            }


            SelectedItems.RemoveAll(item => item.DishId == dishId);
            HttpContext.Session.SetString("SelectedItems", JsonConvert.SerializeObject(SelectedItems));

            Dishes = await _dishService.GetAllDishes();

            return Page();
        }

        public async Task<IActionResult> OnPostClearCartAsync()
        {
            var selectedItemsJson = HttpContext.Session.GetString("SelectedItems");
            if (!string.IsNullOrEmpty(selectedItemsJson))
            {
                SelectedItems = JsonConvert.DeserializeObject<List<OrderItemDTO>>(selectedItemsJson);
            }


            SelectedItems.Clear();
            HttpContext.Session.Remove("SelectedItems");

            Dishes = await _dishService.GetAllDishes();

            return Page();
        }
        public async Task<IActionResult> OnPostPlaceOrderAsync()
        {
            var customerId = HttpContext.Session.GetString("CustomerId");
            if (!string.IsNullOrEmpty(customerId))
            {
                Customer = await _customerService.GetCustomerById(Guid.Parse(customerId));
            }

            var selectedItemsJson = HttpContext.Session.GetString("SelectedItems");
            if (!string.IsNullOrEmpty(selectedItemsJson))
            {
                SelectedItems = JsonConvert.DeserializeObject<List<OrderItemDTO>>(selectedItemsJson);
            }


            var orderDTO = new OrderDTO
            {
                Id = Guid.NewGuid(),
                OrderItems = SelectedItems,
                CreatedAt = DateTime.Now,
                CustomerId = Customer.Id,
                PaymentDate = null
            };

            await _orderService.CreateOrder(orderDTO);
            SelectedItems.Clear();
            HttpContext.Session.Remove("SelectedItems");

            Dishes = await _dishService.GetAllDishes();


            return Page();
        }
    }
}
