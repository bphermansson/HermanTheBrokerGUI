using HermanTheBrokerGUI.Models;

namespace HermanTheBrokerGUI.Services
{
    public interface IApiService
    {
        Task<House> GetHouseAsync(int id);
    }
}
