using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataAppHR.Data;
using BataAppHR.Models;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Drawing;

namespace BataAppHR.Controllers
{
    public class XstoreAddController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<XstoreAddController> _logger;
        public XstoreAddController(FormDBContext db, ILogger<XstoreAddController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "AdminRealEstate")]
        public IActionResult Index()
        {
            var data = new List<XstoreModel>();
            data = db.xstore_organization.Where(y => y.FLAG_APPROVAL != 4).OrderBy(y => y.edp).ToList();
            return View(data);
        }
        [HttpGet]
        [Authorize(Roles = "AdminRealEstate")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
