using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Menu.REST.DTO;

namespace Razor.Services
{
    public class DishService 
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public DishService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7020/api/"),
            };
        }

        public async Task<List<DishDTO>> GetAllDishes()
        {
            var response = await _httpClient.GetAsync("Dish");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DishDTO>>(content, _jsonOptions);
        }

        public async Task<DishDTO> GetDishById(Guid id)
        {
            var response = await _httpClient.GetAsync($"Dish/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DishDTO>(content, _jsonOptions);
        }
    }
}
