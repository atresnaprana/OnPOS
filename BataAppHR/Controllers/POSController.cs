using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BataAppHR.Data;
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
using OnPOS.Models;
using static System.Net.WebRequestMethods;
using OfficeOpenXml;
using BataAppHR.Models;
using MimeDetective.Storage.Xml.v2;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;
using System.ComponentModel.DataAnnotations;

namespace OnPOS.Controllers
{
    public class POSController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<POSController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }


        public POSController(FormDBContext db, ILogger<POSController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;

        }
        public IActionResult Index()
        {
            dbSalesHdr fld = new dbSalesHdr();
            fld.trans_amount = Convert.ToDecimal(149999);
            fld.s33 = 100;
            fld.s34 = 100;
            fld.s35 = 100;
            fld.s36 = 100;
            fld.s37 = 100;
            fld.s38 = 100;
            fld.s39 = 100;
            fld.s40 = 100;
            fld.s41 = 100;
            fld.s42 = 100;
            fld.s43 = 100;
            fld.s44 = 100;
            fld.s45 = 100;
            fld.s46 = 100;
            fld.totalstock = 100;
            fld.username = "salesuser1";

            return View(fld);
        }
    }
}
