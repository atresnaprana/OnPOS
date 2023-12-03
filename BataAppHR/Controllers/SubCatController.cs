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
namespace OnPOS.Controllers
{
    public class SubCatController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<SubCatController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }


        public SubCatController(FormDBContext db, ILogger<SubCatController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;

        }
        public IActionResult Index()
        {
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            var getItemList = db.SubCategoryTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID).ToList();
            return View(getItemList);
        }
        [HttpGet]
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Create()
        {
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();

            dbSubCategory fld = new dbSubCategory();
            fld.ddcat = db.CategoryTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").ToList();
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbSubCategory objTrainer)
        {
            if (ModelState.IsValid)
            {
                var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                objTrainer.COMPANY_ID = data.COMPANY_ID;
                objTrainer.ENTRY_DATE = DateTime.Now;
                objTrainer.UPDATE_DATE = DateTime.Now;
                objTrainer.ENTRY_USER = User.Identity.Name;
                objTrainer.UPDATE_USER = User.Identity.Name;
                objTrainer.FLAG_AKTIF = "1";
                try
                {
                    db.SubCategoryTbl.Add(objTrainer);
                    db.SaveChanges();

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
                return RedirectToAction("Index");
            }
            return View(objTrainer);
        }

        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            int ids = Convert.ToInt32(id);
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();

            dbSubCategory fld = db.SubCategoryTbl.Find(ids);
            fld.ddcat = db.CategoryTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").ToList();

            if (fld == null)
            {
                return NotFound();
            }

            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind] dbSubCategory fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                int ids = Convert.ToInt32(id);

                var editFld = db.SubCategoryTbl.Find(ids);
                editFld.Category = fld.Category;
                editFld.SubCategory = fld.SubCategory;

                editFld.description = fld.description;
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
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            int ids = Convert.ToInt32(id);

            dbSubCategory fld = db.SubCategoryTbl.Find(ids);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCat(string id)
        {
            int ids = Convert.ToInt32(id);

            dbSubCategory fld = db.SubCategoryTbl.Find(ids);

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
    }
}
