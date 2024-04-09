using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Menu.REST.DTO;
using Menu.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Menu.Infrastructure.DTO;

namespace Razor.Services
{
    public class CustomerService 
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public CustomerService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7020/api/"),
            };
        }

        public async Task<List<CustomerDTO>> GetAllCustomers()
        {
            var response = await _httpClient.GetAsync("Customers");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CustomerDTO>>(content, _jsonOptions);
        }

        public async Task<CustomerDTO> GetCustomerById(Guid id)
        {
            var response = await _httpClient.GetAsync($"Customers/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CustomerDTO>(content, _jsonOptions);
        }

        public async Task CreateCustomer(CustomerDTO customerDto)
        {
            
            customerDto.Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password);
            customerDto.Id = Guid.NewGuid();
            var response = await _httpClient.PostAsJsonAsync("Customers", customerDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task<CustomerDTO> GetCustomerByName(string firstName, string lastName)
        {
            var response = await _httpClient.GetAsync($"Customers/ByName/{firstName}/{lastName}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CustomerDTO>(content, _jsonOptions);
        }

        public async Task DeleteCustomer(Guid customerId)
        {
            var response = await _httpClient.DeleteAsync($"Customers/{customerId}");
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateCustomer(CustomerDTO updatedCustomer)
        {
            updatedCustomer.Password = BCrypt.Net.BCrypt.HashPassword(updatedCustomer.Password);
            var response = await _httpClient.PutAsJsonAsync($"Customers/{updatedCustomer.Id}", updatedCustomer);
            response.EnsureSuccessStatusCode();
        }
    }
}
