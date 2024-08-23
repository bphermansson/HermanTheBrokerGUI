﻿using HermanTheBrokerAPI.Areas.Identity.Data;
using HermanTheBrokerAPI.Classes;
using HermanTheBrokerAPI.Models;
using HermanTheBrokerGUI.Classes;
using HermanTheBrokerGUI.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
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

        public async Task<House> GetById(int houseId)
        {
            var response = await _httpClient.GetAsync("https://localhost:7015/api/House/HouseById?id=" + houseId.ToString());

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                //IEnumerable<House> house = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<House>(content);

            }
            return default;
        }
        public async Task<IEnumerable<House>> GetAllHouses()
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            var response = await _httpClient.GetAsync(BaseAddress + "api/House/Houses");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<House>>(content);
            }
            return default;
        }
        public async Task<IEnumerable<House>> GetHousesByBrokerEmail(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            var response = await _httpClient.GetAsync(BaseAddress + "api/Broker/GetHousesByBrokerEmail?email=" + email);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<House>>(content);
            }
            return default;
        }
        public async Task<IEnumerable<House>> SearchHouses(Searchobject searchobject)
        {
            if (searchobject.Minsize == null)
                searchobject.Minsize = 0;
            if (searchobject.Maxsize == null)
                searchobject.Maxsize = 0;
            if (searchobject.City == null)
                searchobject.City = "*";
            if (searchobject.Noofrooms == null)
                searchobject.Noofrooms = 0;

            var response = await _httpClient.GetAsync(BaseAddress + "api/House/Search/" + searchobject.Minsize+"/"+searchobject.Maxsize+"/"+searchobject.City+"/"+searchobject.Noofrooms);
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

        public async Task<string> Login(loginObject lin)
        {
            //email = "a@a.com";
            //password = "123qweE_";
            HttpResponseMessage loginResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "identity/login", new { lin.Email, lin.Password });

            if (loginResponse.IsSuccessStatusCode)
            {
                var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
                Config.AuthToken = loginContent.GetProperty("accessToken").GetString();
                Config.RefreshToken = loginContent.GetProperty("refreshToken").GetString();
                Config.CurrentEmail = lin.Email;
                return "Inloggad"; 
            }
            else
            {
                return "Fel användarnamn eller lösenord.";
            }
        }
        public async Task<string> Register(loginObject lin)
        {
            HttpResponseMessage regResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "identity/register", new { lin.Email, lin.Password });


            if (regResponse.IsSuccessStatusCode)
            {
                return "Kontot registrerat.";
            }
            else
            {
                // Get & decode response.
                using var contentStream = await regResponse.Content.ReadAsStreamAsync();
                var root = await JsonSerializer.DeserializeAsync<ResponseRoot>(contentStream);
                if (root.errors != null) 
                {
                    return root.errors.DuplicateUserName[0].ToString();
                }
                if(regResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return "Serverfel.";
                }
            }
            return "Okänt fel.";
        }
        public bool LoginCheck()
        {
            if (Config.AuthToken != null)
            {
                return true;
            }
            return false;
        }
        static async Task<Boolean> PostAsync(HttpClient httpClient, string email, string password)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
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
        }
        public async Task<bool> Newhouse(House house)
        {
            HttpResponseMessage addResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "api/House/NewHouse", house);
            //var lc = await addResponse.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (addResponse.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Edithouse(House house)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseAddress + "api/House/EditHouse", house);
            //var lc = await addResponse.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Removehouse(House house)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseAddress + "api/House/RemoveHouse", house);
            //var lc = await addResponse.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Editbroker(IdentityUser uid)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseAddress + "api/Broker/EditBroker", uid);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<Broker> GetBrokerByEmail(string email)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress + "api/Broker/" + email);
            var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            return responseContent.Deserialize<Broker>();
        }
    }
}