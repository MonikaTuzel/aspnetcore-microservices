using SpendingsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarApi.Models;

namespace SpendingsApi.IServices
{
    public interface ISpendingsService
    {
        Task<IList<Spendings>> GetSpendings();
        Task<List<Spendings>> GetSpendingsByIdAsync(string id);
        Task<List<Car>> GetUserCars(string id);
        Task<string> AddSpendings(Spendings spendings);
        Spendings UpdateSpendings(Spendings spendings);
        Task<string> DeleteSpendings(int id);
        Task<List<Log>> GetLogsByIdAsync(string id);
        Task<Tuple<Task<string>, Task<string>>> SetNames(int idCar, int idCost);




    }
}
