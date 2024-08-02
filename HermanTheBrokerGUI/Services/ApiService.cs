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
        //public record SampleValue(int Id, string Name);

        // HttpClient lifecycle management best practices:
        // https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://localhost:7015"),
        };

        public async Task<Boolean> Login<Bool>(string email, string password)
        {
            var loggedIn = PostAsync(sharedClient, email, password);
            return true;
        }

        static async Task<Boolean> PostAsync(HttpClient httpClient, string email, string password)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    email = "oatrik@paheco.nu",
                    password = "String1234<",
                    //email = email,
                    //password = password
                }),
                Encoding.UTF8,
                "application/json");

                using HttpResponseMessage response = await httpClient.PostAsync(
                    "login",
                    jsonContent);
            {
                try
                {
                    // Handle success
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"{jsonResponse}\n");
                    return true;
                }
                catch (HttpRequestException)
                {
                    // Handle failure
                    return false;
                }
            }
            //response.EnsureSuccessStatusCode();



        }
        public async Task<Boolean> Register<Bool>(string email, string password)
        {
            var loggedIn = PostAsync(sharedClient, email, password).Result;
            return loggedIn;
        }
    }
}