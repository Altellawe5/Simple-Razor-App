using Menu.REST.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

using System.Threading.Tasks;
using System.Windows;

namespace DishManegment.Services
{
    public class DishService
    {
        private HttpClient _client;

        public DishService()
        {
            _client = new HttpClient { 
                BaseAddress = new Uri("https://localhost:7020/api/"),
            };
        }


        public async Task<IEnumerable<DishDTO>> GetAllDishes()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("Dish");
                if (response.IsSuccessStatusCode)
                {
                    var dishes = await response.Content.ReadFromJsonAsync<IEnumerable<DishDTO>>();
                    return dishes;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request exception: {e.Message}");
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
            }
            return null;
        }

        public async Task<DishDTO> GetDish(Guid id)
        {
            HttpResponseMessage response = await _client.GetAsync($"Dish/{id}");
            if (response.IsSuccessStatusCode)
            {
                var dish = await response.Content.ReadFromJsonAsync<DishDTO>();
                return dish;
            }
            return null;
        }


        public async Task AddDish(DishDTO dish)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("Dish", dish);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateDish(Guid id, DishDTO dish)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"Dish/{id}", dish);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDish(Guid id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"Dish/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}
