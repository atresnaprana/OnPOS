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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] dbStoreList fld)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                var editFld = db.StoreListTbl.Find(id);
                editFld.STORE_NAME = fld.STORE_NAME;
                editFld.STORE_ADDRESS = fld.STORE_ADDRESS;
                editFld.STORE_CITY = fld.STORE_CITY;
                editFld.STORE_PROVINCE = fld.STORE_PROVINCE;
                editFld.STORE_POSTAL = fld.STORE_POSTAL;
                editFld.STORE_REG_DATE = fld.STORE_REG_DATE;
                editFld.STORE_BANK_NAME = fld.STORE_BANK_NAME;
                editFld.STORE_BANK_NUMBER = fld.STORE_BANK_NUMBER;
                editFld.STORE_BANK_BRANCH = fld.STORE_BANK_BRANCH;
                editFld.STORE_BANK_COUNTRY = fld.STORE_BANK_COUNTRY;
                editFld.STORE_MANAGER_KTP = fld.STORE_MANAGER_KTP;
                editFld.STORE_MANAGER_NAME = fld.STORE_MANAGER_NAME;
                editFld.STORE_MANAGER_PHONE = fld.STORE_MANAGER_PHONE;
                editFld.STORE_MANAGER_EMAIL = fld.STORE_MANAGER_EMAIL;
                editFld.STORE_EMAIL = fld.STORE_EMAIL;
               

                //editFld.FLAG_AKTIF = "0";
                if (fld.isBlackList == true)
                {
                    editFld.STORE_BL_FLAG = "1";
                }
                else
                {
                    editFld.STORE_BL_FLAG = "0";

                }

                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name;

                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgEdit" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                return RedirectToAction("Index");
            }
            return View(fld);
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}
            dbStoreList fld = db.StoreListTbl.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeactivateStore(int id)
        {
            dbStoreList fld = db.StoreListTbl.Find(id);

            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    //db.trainerDb.Remove(fld);
                    fld.FLAG_AKTIF = "0";
                    fld.UPDATE_DATE = DateTime.Now;
                    fld.UPDATE_USER = User.Identity.Name;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgEdit" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Reactivate(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}
            dbStoreList fld = db.StoreListTbl.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Reactivate")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ReactivateStore(int id)
        {
            dbStoreList fld = db.StoreListTbl.Find(id);

            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    //db.trainerDb.Remove(fld);
                    fld.FLAG_AKTIF = "1";
                    fld.UPDATE_DATE = DateTime.Now;
                    fld.UPDATE_USER = User.Identity.Name;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgEdit" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}