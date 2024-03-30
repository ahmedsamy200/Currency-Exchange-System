using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Domian;

namespace Core.Service
{
    public interface IExchangHistoryServices
    {
        Task<bool> AddExchange(CurrencyVM currency, int id);
        Task<List<CurrencyVM>> GetHighestNCurrencies(int n);
        Task<List<CurrencyVM>> GetLowestNCurrencies(int n);
        Task<List<CurrencyVM>> GetMostNImprovedCurrenciesByDate(DateTime from , DateTime to , int n);
        Task<List<CurrencyVM>> GetMostNDecrementalCurrenciesByDate(DateTime from, DateTime to, int n);
        Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string Tocurrency);
    }
}
