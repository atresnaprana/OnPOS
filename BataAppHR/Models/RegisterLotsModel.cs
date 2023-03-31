using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace BataAppHR.Models
{
    public class RegisterLotsModel
    {
        public List<UserToRegister> ApplicationUsers { get; set; }
    }
    public class UserToRegister
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
