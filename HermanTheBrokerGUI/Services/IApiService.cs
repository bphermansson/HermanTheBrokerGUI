using HermanTheBrokerGUI.Models;
using HermanTheBrokerGUI.Classes;
namespace HermanTheBrokerGUI.Services
{
    public interface IApiService
    {
        Task<House> GetHouseAsync(int id);
        public Task<IEnumerable<House>> GetAllHouses();
        public Task<IEnumerable<Broker>> GetAllBrokers();

        Task<IEnumerable<House>> SearchHouses<House>(Searchobject searchobject);
    }
}
