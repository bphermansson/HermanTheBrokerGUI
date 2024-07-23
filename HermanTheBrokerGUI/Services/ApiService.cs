using HermanTheBrokerGUI.Models;
using Newtonsoft.Json;

namespace HermanTheBrokerGUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return default;
        }
        public async Task<IEnumerable<T>> GetById<T>(int houseId)
        {
            var response = await _httpClient.GetAsync("https://localhost:7015/api/Visitor/HouseById?id="+houseId.ToString());

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(content);


            }
            return default;
        }
        public async Task<IEnumerable<T>> GetAllHouses<T>()
        {

            var response = await _httpClient.GetAsync("https://localhost:7015/api/Visitor/Houses");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
            }
            else
            {
                return null;
            }
        }
    }
}