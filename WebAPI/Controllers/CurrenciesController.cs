using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Core.Service;
using Core.Repository;
using Core.Domian;
using System;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyServices _currency;
        private readonly IExchangHistoryServices _exchange;
        public CurrenciesController(ICurrencyServices currency , IExchangHistoryServices exchange)
        {
            _currency = currency;
            _exchange = exchange;
        }

        [HttpPost("AddCurrency")]
        public async Task<IActionResult> AddCurrency(CurrencyVM currency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (currency.Rate <= 0)
                    {
                        return BadRequest("Invalid Rate");
                    }

                    // cheack if there are a currency with the same name
                    if (_currency.GetCurrencyByName(currency.Name).Result != null)
                    {
                        return BadRequest("There are a currency with the same name!");
                    }

                    var result = await _currency.AddCurrency(currency);
                    if (result != null)
                    {
                        // add data to Exchange History Table
                        if (AddExchange(currency, result.Id).Result != true)
                        {
                            return BadRequest("Something went wrong");
                        }
                        return Ok("Currency Added Successfully!");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost("UpdateCurrency")]
        public async Task<IActionResult> UpdateCurrency(CurrencyVM currency , int id)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if(id <= 0)
                    {
                        return BadRequest("Invalid ID");
                    }

                    if (currency.Rate <= 0)
                    {
                        return BadRequest("Invalid Rate");
                    }

                    // cheack if there are a currency with the same name
                    if (_currency.IsExist(currency , id).Result != null)
                    {
                        return BadRequest("There are a currency with the same name!");
                    }

                    var result = await _currency.UpdateCurrency(currency , id);
                    if (result)
                    {
                        if (AddExchange(currency, id).Result != true)
                        {
                            return BadRequest("Something went wrong");
                        }
                        return Ok("Currency Updated successfully!");
                    }
                    return NotFound("Invalid ID");
                }
                catch (Exception)
                {

                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpPost("DeleteCurrency")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {


            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            try
            {
                if (_currency.GetCurrencyById(id).Result == null)
                {
                    return NotFound("Invalid ID");
                }

                bool result = await _currency.DeleteCurrency(id);
                if (result == false)
                {
                    return NotFound();
                }
                return Ok("Currency Deleted Successfully!");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("GetAllCurrencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            try
            {
                var Currencies =await _currency.GetAllCurrencies();
                if (Currencies.Count() == 0)
                {
                    return NotFound("There are no data available!");
                }

                return Ok(Currencies);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("GetHighestNCurrencies")]
        public async Task<IActionResult> GetHighestNCurrencies(int n)
        {
            try
            {
                if (n <= 0)
                {
                    return BadRequest("Invalid Number!");
                }
                var Currencies =await _exchange.GetHighestNCurrencies(n);
                if (Currencies.Count() == 0)
                {
                    return NotFound("There are no data available!");
                }
                return Ok(Currencies);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("GetLowestNCurrencies")]
        public async Task<IActionResult> GetLowestNCurrencies(int n)
        {
            try
            {
                if (n <= 0)
                {
                    return BadRequest("Invalid Number!");
                }
                var Currencies =await _exchange.GetLowestNCurrencies(n);
                if (Currencies.Count() == 0)
                {
                    return NotFound("There are no data available!");
                }
                return Ok(Currencies);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("GetCurrencyByName")]
        public async Task<IActionResult> GetCurrencyByName(string name)
        {
            try
            {
                var Currencies = await _currency.GetCurrencyByName(name);
                if (Currencies == null)
                {
                    return NotFound("There are no data available!");
                }

                return Ok(Currencies);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("GetMostNImprovedCurrenciesByDate")]
        public async Task<IActionResult> GetMostNImprovedCurrenciesByDate(DateTime dateFrom, DateTime dateTo, int n)
        {
            try
            {
                if (dateFrom > dateTo)
                {
                    return NotFound("There are no data available!");
                }
                var Currencies = await _exchange.GetMostNImprovedCurrenciesByDate(dateFrom, dateTo,n);
                if (Currencies.Count() == 0)
                {
                    return NotFound("There are no data available!");
                }

                return Ok(Currencies);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetMostNDecrementalCurrenciesByDate")]
        public async Task<IActionResult> GetMostNDecrementalCurrenciesByDate(DateTime dateFrom, DateTime dateTo, int n)
        {
            try
            {
                if (dateFrom > dateTo)
                {
                    return NotFound("There are no data available!");
                }
                var Currencies = await _exchange.GetMostNDecrementalCurrenciesByDate(dateFrom, dateTo, n);
                if (Currencies.Count() == 0)
                {
                    return NotFound("There are no data available!");
                }

                return Ok(Currencies);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("ConvertCurrency")]
        public async Task<ActionResult<decimal>> ConvertCurrency(decimal amount ,string CurrencyFrom, string CurrencyTo)
        {
            try
            {
                if (amount <= 0)
                {
                    return BadRequest("Invalid Amount");
                }
                if (String.IsNullOrEmpty(CurrencyFrom) || String.IsNullOrEmpty(CurrencyTo))
                {
                    return BadRequest("Invalid Currency");
                }
                var result = await _exchange.ConvertCurrency(amount, CurrencyFrom, CurrencyTo);

                if (result == 0)
                {
                    return BadRequest("Invalid Currency");
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [NonAction]
        public async Task<bool> AddExchange(CurrencyVM currency ,int id)
        {
    
                try
                {

                    bool Added = await _exchange.AddExchange(currency , id);
                    if (Added)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                
                }
                catch (Exception)
                {

                    return false;
                }

           
        }
    }
}
