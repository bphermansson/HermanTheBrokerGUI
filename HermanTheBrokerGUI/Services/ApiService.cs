﻿using HermanTheBrokerGUI.Classes;
using HermanTheBrokerGUI.Models;
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
                return JsonSerializer.Deserialize<House>(content);

            }
            return default;
        }
        public async Task<IEnumerable<House>> GetAllHouses()
        {
           
            var response = await _httpClient.GetAsync(BaseAddress + "api/House/Houses");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<House>>(content);
            }
            return default;
        }
        public async Task<IEnumerable<House>> GetHousesByBrokerEmail(string brokerEmail)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            var response = await _httpClient.GetAsync(BaseAddress + "api/Broker/GetHousesByBrokerEmail?brokerEmail=" + brokerEmail);

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

        public async Task<string> Login(LoginObject lin)
        {
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
        public async Task<string> Register(LoginObject lin)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseAddress + "identity/register", new { lin.Email, lin.Password });
            {
                // Get & decode response.
                try
                {
                    response.EnsureSuccessStatusCode();
                    return "Kontot registrerat.";
                }
                catch (HttpRequestException e)
                {
                    // TODO: Better error reporting, what went wrong?
                    //using var contentStream = await response.Content.ReadAsStreamAsync();
                    //var root = await JsonSerializer.DeserializeAsync<Root>(contentStream);
                    //var err = root.errors;
                    //var s = err.GetType();
                    //System.Reflection.PropertyInfo pi = err.GetType().GetProperty("name"); // get the property info of "name"
                    //string name = (string)(pi.GetValue(err, null));
                    return "Något gick fel.";
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
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseAddress + "api/House/NewHouse", house);
            //var lc = await response.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Edithouse(House house)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseAddress + "api/House/EditHouse", house);
            //var lc = await response.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Removehouse(House house)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            HttpResponseMessage response = await _httpClient.DeleteAsync(BaseAddress + "api/House/RemoveHouse/" + house.HouseId);
            //var lc = await response.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Editbroker(Broker uid)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseAddress + "api/Broker/EditBroker", uid);
            //var lc = await response.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<Broker> GetBrokerByEmail(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress + "api/Broker/GetBrokerByEmail?brokerEmail=" + email);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>();
                return responseContent[0].Deserialize<Broker>();
            }
            Broker error = new();
            error.Email = "Error";
            return error;
            
        }
        public async Task<List<Broker>> GetAllBrokers()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress + "api/Broker/GetAllBrokers");
            var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>();
            return JsonSerializer.Deserialize<List<Broker>>(responseContent);
        }
    }
}