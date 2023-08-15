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
    public class StoreListController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<StoreListController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }
        private readonly IMailService mailService;

        public string EmailConfirmationUrl { get; set; }

        public StoreListController(FormDBContext db, ILogger<StoreListController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
            this.mailService = mailService;

        }
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Index()
        {
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            var getStorelist = db.StoreListTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID).ToList();
            return View(getStorelist);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {

            return View();
        }
        [Authorize(Roles = "CustomerOnPos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] dbStoreList objCust)
        {
            if (ModelState.IsValid)
            {
                var custdb = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                int validate = 0;
                objCust.ENTRY_DATE = DateTime.Now;
                objCust.UPDATE_DATE = DateTime.Now;
                objCust.ENTRY_USER = User.Identity.Name;
                objCust.UPDATE_USER = User.Identity.Name;
                objCust.COMPANY_ID = custdb.COMPANY_ID;
                objCust.FLAG_AKTIF = "1";


                try
                {
                    //var user = new IdentityUser { UserName = objCust.Email.Trim(), Email = objCust.Email.Trim(), EmailConfirmed = true };

                    var user = new IdentityUser { UserName = objCust.STORE_EMAIL.Trim(), Email = objCust.STORE_EMAIL.Trim() };
                    var validateisexist = _userManager.FindByEmailAsync(objCust.STORE_EMAIL.Trim());

                    if (validateisexist.Result == null)
                    {
                        user.EmailConfirmed = true;
                        var result = await _userManager.CreateAsync(user, objCust.Password);

                        if (result.Succeeded)
                        {
                            var isinRole = _userManager.IsInRoleAsync(user, "StoreOnPos");

                            if (!isinRole.Result)
                            {
                                var result1 = await _userManager.AddToRoleAsync(user, "StoreOnPos");
                              
                            }
                           
                        }
                        db.StoreListTbl.Add(objCust);
                        db.SaveChanges();
                        //SendVerifyEmail(objCust.Email.Trim());
                        //if (result.Result )
                        //{
                        //    db.CustomerTbl.Add(objCust);
                        //    db.SaveChanges();

                        //}

                    }
                    else
                    {
                        validate = 1;
                    }


                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                //apprDal.AddApproval(objApproval);
                if (validate == 1)
                {
                    objCust.errmsg = "This account is exist";
                    return View(objCust);
                }
                else
                {
                    return RedirectToAction("Index");

                }
            }
            return View(objCust);
        }
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Edit(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}

            dbStoreList fld = db.StoreListTbl.Find(id);
            if (fld.STORE_BL_FLAG == "1")
            {
                fld.isBlackList = true;
            }
            else
            {
                fld.isBlackList = false;

            }
            if (fld == null)
            {
                return NotFound();
            }

            return View(fld);
        }
    }
}