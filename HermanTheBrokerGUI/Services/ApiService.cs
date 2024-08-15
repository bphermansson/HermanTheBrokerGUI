using HermanTheBrokerAPI.Classes;
using HermanTheBrokerGUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HermanTheBrokerGUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private Uri BaseAddress = new Uri("https://localhost:7015");

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        //public async Task<T> Get<T>(string endpoint)
        //{
        //    var response = await _httpClient.GetAsync(endpoint);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        return JsonSerializer.Deserialize<T>(content);

        //    }
        //    return default;
        //}
        //private static HttpClient sharedClient = new()
        //{
        //    BaseAddress = new Uri("https://localhost:7015"),
        //};
        public async Task<House> GetById(int houseId)
        {
            var response = await _httpClient.GetAsync("https://localhost:7015/api/Visitor/HouseById?id="+houseId.ToString());

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<House>(content);
            }
            return default;
        }
        public async Task<IEnumerable<House>> GetAllHouses()
        {
            var response = await _httpClient.GetAsync(BaseAddress + "api/Visitor/Houses");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<House>>(content);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<House>> SearchHouses(Searchobject searchobject)
        {
            var response = await _httpClient.GetAsync(BaseAddress + "/api/Visitor/"+searchobject.Minsize+"/"+searchobject.Maxsize+"/"+searchobject.City+"/"+searchobject.Category);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<House>>(content);
            }
            else
            {
                return null;
            }
        }
        public async Task<Boolean> Login<Bool>(string email, string password)
        {
            //var loggedIn = PostAsync(sharedClient, email, password);
            return true;
        }

        static async Task<Boolean> PostAsync(HttpClient httpClient, string email, string password)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    //email = "oatrik@paheco.nu",
                    //password = "String1234<",
                    email = email,
                    password = password
                }),
                Encoding.UTF8,
                "application/json");

                using HttpResponseMessage response = await httpClient.PostAsync(
                    "login",
                    jsonContent);
            {
                try
                {
                    response.EnsureSuccessStatusCode();
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
            var loggedIn = PostAsync(_httpClient, email, password).Result;
            return loggedIn;
        }
    }
}