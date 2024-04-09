using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Menu.REST.DTO;

namespace Razor.Services
{
    public class OrderService 
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public OrderService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7020/api/"),
            };
        }
        public async Task<OrderDTO> GetOrderById(Guid id)
        {
           // return JsonSerializer.Deserialize<CustomerDTO>(content, _jsonOptions);


            var response = await _httpClient.GetAsync($"Order/GetOrder/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OrderDTO>(content, _jsonOptions);
            
        }
        public async Task CreateOrder(OrderDTO order)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Order/Create", order);
                response.EnsureSuccessStatusCode();
            }                        
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception("", ex);
            }
        }

        public async Task UpdateOrder(OrderDTO order)
        {
            var response = await _httpClient.PutAsJsonAsync($"Order/{order.Id}", order);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(Guid customerId)
        {
            var response = await _httpClient.GetAsync($"Order/{customerId}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OrderDTO>>(content, _jsonOptions);
        }
        public async Task DeleteOrder(Guid orderId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Order/{orderId}");
                response.EnsureSuccessStatusCode();
            }
            catch(Exception ex)
            {
                throw new Exception("", ex);
            }
        }
    }
}
