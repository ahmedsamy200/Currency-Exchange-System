using Core.Model;
using Core.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Service
{
    public class LoginServices : ILoginServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<bool> Login(LoginDetails loginDetails)
        {
            try
            {
                if (loginDetails == null)
                {
                    return false;
                }

                var identityUser = await _userManager.FindByEmailAsync(loginDetails.Email);
                if (identityUser == null)
                {
                    return false;
                }

                var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, loginDetails.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<ModelStateDictionary> Register(UserDetails userDetails)
        {
            try
            {
                var dictionary = new ModelStateDictionary();

                if (userDetails == null)
                {
                    return dictionary;
                }

                var identityUser = new ApplicationUser() { UserName = userDetails.UserName, Email = userDetails.Email };
                var result = await _userManager.CreateAsync(identityUser, userDetails.Password);
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        dictionary.AddModelError(error.Code, error.Description);
                    }
                    return dictionary;
                }

                return dictionary;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
