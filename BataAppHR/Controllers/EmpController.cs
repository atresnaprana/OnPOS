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


namespace BataAppHR.Controllers
{
    public class EmpController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        public const string SessionKeyName2 = "RDCode";

        private IHostingEnvironment Environment;
        private readonly ILogger<EmpController> _logger;

        public EmpController(FormDBContext db, ILogger<EmpController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        public IActionResult Index()
        {
            var data = new List<dbRD>();

            return View();
        }
    }
}
