using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Core.Service
{
    public interface ILoginServices
    {
        Task<bool> Login(LoginDetails loginDetails);
        Task<ModelStateDictionary> Register(UserDetails userDetails);
    }
}
