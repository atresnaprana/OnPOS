using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BataAppHR.Data;
using BataAppHR.Models;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using MimeDetective.Definitions;
using MimeDetective.Diagnostics;
using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Net;
using BataAppHR.Services;
using OnPOS.Models;
using static System.Net.WebRequestMethods;

namespace OnPOS.Controllers
{
    public class SalesStaffController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<SalesStaffController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }
        private readonly IMailService mailService;

        public string EmailConfirmationUrl { get; set; }

        public SalesStaffController(FormDBContext db, ILogger<SalesStaffController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
            this.mailService = mailService;

        }

        [Authorize(Roles = "CustomerOnPos,StoreOnPos")]
        public IActionResult Index()
        {
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            if (User.IsInRole("CustomerOnPos"))
            {
                var getSalesStaff = db.StoreListTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID).ToList();
                return View();
            } else if (User.IsInRole("StoreOnPos"))
            {
                var getStorelist = db.StoreListTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID).ToList();
                return View();
            }
            else
            {
                return NotFound();
            }
           
        }
    }
}
