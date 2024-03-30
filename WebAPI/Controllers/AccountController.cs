using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Core.Service;
using Core.Repository;
using Core.Domian;
using System;
using Core.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ILoginServices _repository;

        public AccountController(ILoginServices repository)        
        {
            _repository = repository;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDetails loginDetails)
        {
            bool result = await _repository.Login(loginDetails);
            if (result)
            {
                return Ok("You are logged in successfully!");
            }
            return BadRequest("Login failed");
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserDetails userDetails)
        {
            var result =await _repository.Register(userDetails);
            if (result.Count() > 0)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = result });
            }
            return Ok("User Reigstration Successful");
            
        }

    }
}
