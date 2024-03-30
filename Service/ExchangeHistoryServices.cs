using Core.Domian;
using Core.Service;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Service
{
    public class ExchangeHistoryServices : IExchangHistoryServices
    {
        private readonly IRepository<ExchageHistory> _repository;
        public ExchangeHistoryServices(IRepository<ExchageHistory> repository)
        {
            _repository = repository;
        }
        public async Task<bool> AddExchange(CurrencyVM currency, int id)
        {

            try
            {
                ExchageHistory ex = new ExchageHistory
                {
                    CurId = id,
                    Rate = currency.Rate,
                    ExchangeDate = DateTime.Now
                };
                var result = await _repository.Add(ex);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CurrencyVM>> GetHighestNCurrencies(int n)
        {
            var result = _repository.GetAllHistories().Result.Where(x=>x.Currency.IsActive == true).OrderByDescending(x => x.ExchangeDate).DistinctBy(x=>x.CurId).OrderByDescending(x => x.Rate).Take(n);
           
            List<CurrencyVM> currencies = new List<CurrencyVM>();
            foreach (var item in result)
            {
                CurrencyVM obj = new CurrencyVM
                {
                    Name = item.Currency.Name,
                    Sign = item.Currency.Sign,
                    Rate = item.Rate
                };
                currencies.Add(obj);
            }
            return currencies;
        }

        public async Task<List<CurrencyVM>> GetLowestNCurrencies(int n)
        {

            var result = _repository.GetAllHistories().Result.Where(x => x.Currency.IsActive == true).OrderByDescending(x => x.ExchangeDate).DistinctBy(x => x.CurId).OrderBy(x => x.Rate).Take(n);
            List<CurrencyVM> currencies = new List<CurrencyVM>();
            foreach (var item in result)
            {
                CurrencyVM obj = new CurrencyVM
                {
                    Name = item.Currency.Name,
                    Sign = item.Currency.Sign,
                    Rate = item.Rate
                };
                currencies.Add(obj);
            }
            return currencies;
        }

        public async Task<List<CurrencyVM>> GetMostNImprovedCurrenciesByDate(DateTime from , DateTime to , int n)
        {
            var obj1 =_repository.GetAllHistories().Result.Where(x => x.Currency.IsActive == true && x.ExchangeDate >= from && x.ExchangeDate <= to).OrderBy(x => x.ExchangeDate).DistinctBy(x => x.CurId).ToList();
            var obj2 = _repository.GetAllHistories().Result.Where(x => x.Currency.IsActive == true && x.ExchangeDate >= from && x.ExchangeDate <= to).OrderByDescending(x => x.ExchangeDate).DistinctBy(x => x.CurId).ToList();

            List<CurrencyVM> currencies = new List<CurrencyVM>();
            foreach (var item1 in obj2)
            {
                foreach (var item2 in obj1)
                {
                    if (item1.CurId == item2.CurId)
                    {
                        CurrencyVM currency = new CurrencyVM
                        {
                            Name = item1.Currency.Name,
                            Sign = item1.Currency.Sign,
                            Rate = item1.Rate - item2.Rate
                        };
                        currencies.Add(currency);
                    }
                }
            }
            return currencies.Where(x=>x.Rate > 0).OrderByDescending(x=>x.Rate).Take(n).ToList();
        }

        public async Task<List<CurrencyVM>> GetMostNDecrementalCurrenciesByDate(DateTime from, DateTime to, int n)
        {
            var obj1 = _repository.GetAllHistories().Result.Where(x => x.Currency.IsActive == true && x.ExchangeDate >= from && x.ExchangeDate <= to).OrderBy(x => x.ExchangeDate).DistinctBy(x => x.CurId).ToList();
            var obj2 = _repository.GetAllHistories().Result.Where(x => x.Currency.IsActive == true && x.ExchangeDate >= from && x.ExchangeDate <= to).OrderByDescending(x => x.ExchangeDate).DistinctBy(x => x.CurId).ToList();

            List<CurrencyVM> currencies = new List<CurrencyVM>();
            foreach (var item1 in obj2)
            {
                foreach (var item2 in obj1)
                {
                    if (item1.CurId == item2.CurId)
                    {
                        CurrencyVM currency = new CurrencyVM
                        {
                            Name = item1.Currency.Name,
                            Sign = item1.Currency.Sign,
                            Rate = item1.Rate - item2.Rate
                        };
                        currencies.Add(currency);
                    }
                }
            }
            return currencies.Where(x => x.Rate < 0).OrderBy(x => x.Rate).Take(n).ToList();
        }

        public async Task<decimal> ConvertCurrency(decimal amount , string from , string to)
        {
            var objFrom = _repository.GetAllHistories().Result.OrderByDescending(x => x.ExchangeDate).FirstOrDefault(x => x.Currency.Sign == from.ToUpper() || x.Currency.Name == from.ToUpper());
            var objTo = _repository.GetAllHistories().Result.OrderByDescending(x => x.ExchangeDate).FirstOrDefault(x => x.Currency.Sign == to.ToUpper() || x.Currency.Name == to.ToUpper());
            if (objFrom == null || objTo == null)
            {
                return 0;
            }
            return ((amount / objTo.Rate) * objFrom.Rate);
        }

    }
}
