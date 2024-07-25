using HermanTheBrokerGUI.Models;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

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
        public class loginData()
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool TwoFactorCode { get; set; }
            public bool TwoFactorRecoveryCode { get; set; }
        }
        public class altLoginData()
        {
            public string username { get; set; }
            public string password { get; set; }
        }

            public record SampleValue(int Id, string Name);
        
  // "email": "a@b.com",
  // "password": "stringABC123<"

        public async Task<Boolean> Login<Bool>(string email, string password)
        {
            var sampleValue = new loginData();
            sampleValue.Email = "a@b.com";
            sampleValue.Password = "stringABC123<";
            sampleValue.TwoFactorCode = false;
            sampleValue.TwoFactorRecoveryCode = false;

            var lin = new altLoginData();
            lin.username = "a@b.com";
            lin.password = "stringABC123<";

            var sampleValueJson = JsonConvert.SerializeObject(lin);
            var stringData = new StringContent(sampleValueJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7015/login", stringData);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                 content = await response.Content.ReadAsStringAsync();
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}