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
    public class CurrencyServices : ICurrencyServices
    {
        private readonly IRepository<Currency> _repository;
        public CurrencyServices(IRepository<Currency> repository)
        {
            _repository = repository;
        }
        public async Task<Currency> AddCurrency(CurrencyVM currency)
        {
            try
            {
                Currency obj = new Currency
                {
                    IsActive = true,
                    Name = currency.Name.ToUpper(),
                    Sign = currency.Sign
                };
                var result = await _repository.Add(obj);
                return obj;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> DeleteCurrency(int id)
        {
            try
            {
                var currency =await _repository.GetById(id);
                currency.IsActive = false;
                _repository.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            var result = await _repository.GetAll();
            return result.Where(x=>x.IsActive == true);
        }

        public async Task<Currency> GetCurrencyByName(string name)
        {
            try
            {
                var currencies = await _repository.GetAll();
                var result = currencies.Where(x => x.Name.Contains(name.ToUpper()) && x.IsActive == true).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<Currency> GetCurrencyById(int id)
        {
            try
            {
                var currencies = await _repository.GetAll();
                var result = currencies.Where(x => x.Id == id && x.IsActive == true).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> UpdateCurrency(CurrencyVM Currency , int id)
        {
            try
            {
                Currency obj = new Currency
                {
                    Id = id,
                    Name = Currency.Name,
                    Sign = Currency.Sign,
                    IsActive = true
                };

                if (GetCurrencyById(id).Result == null)
                {
                    return false;
                }
                obj.Name = obj.Name.ToUpper();
                await _repository.Update(obj);
                 return true;
               
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Currency> IsExist(CurrencyVM currency , int id)
        {
            try
            {
                var currencies = await _repository.GetAll();
                var result = currencies.Where(x => x.Name == currency.Name.ToUpper() && x.Id != id && x.IsActive == true).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }




    }
}
