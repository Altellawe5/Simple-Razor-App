using Menu.Domain.Models;
using Menu.Infrastructure.DTO;
using Menu.REST.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Services;

namespace RazorApp.Pages
{
    public class OrderPageModel : PageModel
    {
        
        public List<OrderDTO> UnpaidOrders { get; set; }
        public List<OrderDTO> PaidOrders { get; set; }

        private readonly CustomerService _customerService;
        private readonly DishService _dishService;
        private readonly OrderService _orderService;

        public OrderPageModel(CustomerService customerService, DishService dishService, OrderService orderService)
        {
            _customerService = customerService;
            _dishService = dishService;
            _orderService = orderService;
        }
        [BindProperty]
        public CustomerDTO Customer { get; set; }

        [BindProperty]
        public List<DishDTO> AllDishes { get; set; }
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
                return RedirectToPage("/MainPage");
            }

            var allOrders = await _orderService.GetOrdersAsync(Customer.Id);
            UnpaidOrders = allOrders.Where(o => o.PaymentDate == null).ToList();
            PaidOrders = allOrders.Where(o => o.PaymentDate != null).ToList();
            AllDishes = await _dishService.GetAllDishes();

            


            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _orderService.DeleteOrder(id);
            TempData["Message"] = "Order deleted successfully.";

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostPayAsync(Guid id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            order.PaymentDate = DateTime.UtcNow;  
            await _orderService.UpdateOrder(order);


            return RedirectToPage();  
        }
        public async Task<IActionResult> OnPostEditAsync(Guid id, Guid dishIdToAdd, int quantity)
        {
            var order = await _orderService.GetOrderById(id);
            var dish = await _dishService.GetDishById(dishIdToAdd);

            if (order == null || dish == null)
            {
                return NotFound();
            }

            order.OrderItems.Add(new OrderItemDTO { DishId = dish.Id, DishName = dish.Name,  Quantity = quantity });
            await _orderService.UpdateOrder(order);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostRemoveAsync(Guid id, Guid dishIdToRemove)
        {
            var order = await _orderService.GetOrderById(id);
            var itemToRemove = order.OrderItems.FirstOrDefault(oi => oi.DishId == dishIdToRemove);

            if (order == null || itemToRemove == null)
            {
                return NotFound();
            }

            order.OrderItems.Remove(itemToRemove);
            await _orderService.UpdateOrder(order);
            return RedirectToPage();
        }

    }
}
