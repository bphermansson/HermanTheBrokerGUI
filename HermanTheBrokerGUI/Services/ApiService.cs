using System.Text;
using System.Text.Json;

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
                return JsonSerializer.Deserialize<T>(content);

            }
            return default;
        }
        public async Task<IEnumerable<T>> GetById<T>(int houseId)
        {
            var response = await _httpClient.GetAsync("https://localhost:7015/api/Visitor/HouseById?id="+houseId.ToString());

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize < IEnumerable<T>>(content);

            }
            return default;
        }
        public async Task<IEnumerable<T>> GetAllHouses<T>()
        {
            var response = await _httpClient.GetAsync("https://localhost:7015/api/Visitor/Houses");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<T>>(content);
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

        // HttpClient lifecycle management best practices:
        // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://localhost:7015"),
        };

        public async Task<Boolean> Login<Bool>(string email, string password)
        {
            PostAsync(sharedClient);
            return true;

        }

        static async Task PostAsync(HttpClient httpClient)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    email = "oatrik@paheco.nu",
                    password = "String1234<",
                }),
                Encoding.UTF8,
                "application/json");

                using HttpResponseMessage response = await httpClient.PostAsync(
                    "login",
                    jsonContent);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{jsonResponse}\n");
        }
    }
}