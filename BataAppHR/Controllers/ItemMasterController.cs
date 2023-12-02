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

namespace OnPOS.Controllers
{
    public class ItemMasterController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<ItemMasterController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }


        public ItemMasterController(FormDBContext db, ILogger<ItemMasterController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;

        }
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Index()
        {
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            var getItemList = db.ItemMasterTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID).ToList();
            return View(getItemList);
        }
        [HttpGet]
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Create()
        {
            dbItemMaster fld = new dbItemMaster();
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();

            fld.ddcat = db.CategoryTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").ToList();
            fld.ddsubcat = db.SubCategoryTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").ToList();
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbItemMaster objTrainer)
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
                    db.ItemMasterTbl.Add(objTrainer);
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
            dbItemMaster fld = db.ItemMasterTbl.Find(ids);
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            fld.ddcat = db.CategoryTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").ToList();
            fld.ddsubcat = db.SubCategoryTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").ToList();
            if (fld == null)
            {
                return NotFound();
            }

            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind] dbItemMaster fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                int ids = Convert.ToInt32(id);

                var editFld = db.ItemMasterTbl.Find(ids);
                editFld.itemid = fld.itemid;
                editFld.color = fld.color;
                editFld.size = fld.size;
                editFld.category = fld.category;
                editFld.subcategory = fld.subcategory;
                editFld.itemdescription = fld.itemdescription;
                editFld.price1 = fld.price1;
                editFld.price2 = fld.price2;
                editFld.price3 = fld.price3;
                editFld.brand = fld.brand;

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

            dbItemMaster fld = db.ItemMasterTbl.Find(ids);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItems(string id)
        {
            int ids = Convert.ToInt32(id);

            dbItemMaster fld = db.ItemMasterTbl.Find(ids);

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