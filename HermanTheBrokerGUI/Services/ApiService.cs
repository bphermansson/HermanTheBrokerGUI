using HermanTheBrokerGUI.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using HermanTheBrokerGUI.Services;
using System.Net.Http;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text.Json.Serialization;
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

        public async Task<IEnumerable<T>> Get<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
            }

            return null;
        }
    }
}
//    public class ApiService : IApiService
//    {
//        private readonly HttpClient httpClient;

//        public ApiService(HttpClient httpClient)
//        {
//            this.httpClient = httpClient;
//        }

//        public async Task<House> GetHouseAsync(int id)
//        {
//            //return await httpClient.GetJsonAsync<House>("api/employees");
//            //HttpResponseMessage message = httpClient.GetAsync("api/Visitor/HouseById/" + id).Result;
//            //string returnText = message.Content.ReadAsStringAsync().Result;
//            //return JsonSerializer.Deserialize<List<House>>(returnText);
//            return await httpClient.GetFromJsonAsync<House>("api/Visitor/HouseById?id=2");

//        }

//        //private readonly HttpClient _httpClient;

//        //public ApiService(HttpClient httpClient)
//        //{
//        //    _httpClient = httpClient;
//        //}

//        //    public async Task<House> GetHouseAsync(int id)
//        //    {
//        //        // From Swagger: https://localhost:7015/api/Visitor/HouseById?id=2
//        //        var response = await _httpClient.GetAsync($"api/Visitor/HouseById/{id}");
//        //        response.EnsureSuccessStatusCode();
//        //        var responseBody = await response.Content.ReadAsStringAsync();
//        //        return JsonSerializer.Deserialize<House>(responseBody);
//        //    }
//        //}
//    }
//}
