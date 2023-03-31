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
    public class SalesWHController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<SalesWHController> _logger;

        public SalesWHController(FormDBContext db, ILogger<SalesWHController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "AdminFinance")]
        public IActionResult Index()
        {
            var data = new List<dbEmployee>();
            data = db.EmpTbl.Where(y => y.FLAG_AKTIF == 1 && y.IS_SALES_WH == "1").OrderBy(y => y.id).ToList();
            return View(data);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var fld = new dbEmployee();
            var empList = db.EmpTbl.Where(y => y.FLAG_AKTIF == 1 && string.IsNullOrEmpty(y.IS_SALES_WH) || y.IS_SALES_WH == "0").ToList();
            fld.ddEmp = empList;
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbEmployee objEmp)
        {
            if (ModelState.IsValid)
            {
                var editFld = db.EmpTbl.Where(y => y.EMP_CODE == objEmp.EMP_CODE).FirstOrDefault();

                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name;
                if (objEmp.isSalesWH == true)
                {
                    editFld.IS_SALES_WH = "1";
                }
                else
                {
                    editFld.IS_SALES_WH = "0";

                }

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
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                //apprDal.AddApproval(objApproval);
                return RedirectToAction("Index");
            }
            return View(objEmp);
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}

            dbEmployee fld = db.EmpTbl.Find(id);
            if (fld.IS_SALES_WH == "1")
            {
                fld.isSalesWH = true;
            }
            else
            {
                fld.isSalesWH = false;

            }
            if (fld == null)
            {
                return NotFound();
            }

            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] dbEmployee fld)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                var editFld = db.EmpTbl.Where(y => y.EMP_CODE == fld.EMP_CODE).FirstOrDefault();

                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name;
                if (fld.isSalesWH == true)
                {
                    editFld.IS_SALES_WH = "1";
                }
                else
                {
                    editFld.IS_SALES_WH = "0";

                }

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
    }
}
