using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BataAppHR.Controllers
{
    public class AssignRoleController : Controller
    {
       
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //var test = IdentityRoleClaim();
            return View();
        }
    }
}
