﻿using HermanTheBrokerAPI.Classes;
using HermanTheBrokerGUI.Classes;
using HermanTheBrokerGUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", Config.AuthToken);
            var response = await _httpClient.GetAsync(BaseAddress + "api/House/Houses");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<House>>(content);
            }
            else
            {
                List<House> ErrorList = new List<House>()
                {
                    new House()
                    {
                       Status = "Ett fel uppstod.",
                       Error = true
                    }
                };
                IEnumerable<House> en = ErrorList;
                return en;
             }
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

        public async Task<String> Test()
        {
            var email = "a@a.com";
            var password = "123qweE_";
            HttpResponseMessage loginResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "login", new { email, password });

            var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
            var accessToken = loginContent.GetProperty("accessToken").GetString();

           //_httpClient.DefaultRequestHeaders.Authorization = new("Bearer", accessToken);

            var response = await _httpClient.GetAsync(BaseAddress + "api/Visitor/Houses");

            var resString = await _httpClient.GetStringAsync(BaseAddress + "requires-auth");
            return resString;
        }

        public async Task<bool> Login(loginObject lin)
        {
            //email = "a@a.com";
            //password = "123qweE_";
            HttpResponseMessage loginResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "login", new { lin.Email, lin.Password });
            //HttpResponseMessage loginResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "login?cookieMode=true", new { lin.Email, lin.Password });

            if (loginResponse.IsSuccessStatusCode)
            {
                var loginContent = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
                Config.AuthToken = loginContent.GetProperty("accessToken").GetString();
                Config.RefreshToken = loginContent.GetProperty("refreshToken").GetString();
                return true; 
            }
            return false;
        }
        public async Task<bool> Register(loginObject lin)
        {
            HttpResponseMessage regResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "register", new { lin.Email, lin.Password });
            //var lc = await regResponse.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (regResponse.IsSuccessStatusCode)
            {
                //var loginContent = await regResponse.Content.ReadFromJsonAsync<JsonElement>();
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
            HttpResponseMessage addResponse = await _httpClient.PostAsJsonAsync(BaseAddress + "api/House/EditHouse", house);
            //var lc = await addResponse.Content.ReadFromJsonAsync<JsonElement>();    // Can be used to check for errors
            if (addResponse.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}