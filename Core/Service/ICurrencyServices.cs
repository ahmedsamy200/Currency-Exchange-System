using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Domian;


namespace Core.Service
{
    public interface ICurrencyServices
    {
        Task<Currency> AddCurrency(CurrencyVM currency);
        Task<bool> UpdateCurrency(CurrencyVM currency , int id);
        Task<bool> DeleteCurrency(int id);
        Task<Currency> GetCurrencyByName(string name);
        Task<Currency> IsExist(CurrencyVM currency , int id);
        Task<Currency> GetCurrencyById(int id);
        Task<IEnumerable<Currency>> GetAllCurrencies();
    }
}
