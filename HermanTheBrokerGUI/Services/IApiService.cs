using HermanTheBrokerAPI.Classes;
using HermanTheBrokerGUI.Models;

namespace HermanTheBrokerGUI.Services
{
    public interface IApiService
    {
        Task<House> GetHouseAsync(int id);
        public Task<IEnumerable<House>> GetAllHouses();

        Task<IEnumerable<House>> SearchHouses<House>(Searchobject searchobject);
    }
}
