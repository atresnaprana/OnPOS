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
    public class XstoreCommEditController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<XstoreEditController> _logger;
        public XstoreCommEditController(FormDBContext db, ILogger<XstoreCommEditController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "AdminFinance")]
        public IActionResult Index()
        {
            var data = new List<XstoreModel>();
            data = db.xstore_organization.Where(y => y.IS_DS == "Y" && y.inactive_flag == "0" && y.CLOSE_DATE == null).OrderBy(y => y.edp).ToList();
            return View(data);
        }
        [Authorize(Roles = "AdminFinance")]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            XstoreModel fld = db.xstore_organization.Where(y => y.edp == id).FirstOrDefault();
            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                if(fld.IS_PERC == "Y")
                {
                    fld.IS_PERC_BOOL = true;
                }
                else
                {
                    fld.IS_PERC_BOOL = false;

                }
                if (fld.IS_PERC_DEPT == "Y")
                {
                    fld.IS_PERC_DEPT_BOOL = true;
                }
                else
                {
                    fld.IS_PERC_DEPT_BOOL = false;
                }
                if (fld.RD_COMM_PERC != null)
                {
                    fld.RD_COMM_PERC_STRING = fld.RD_COMM_PERC.ToString();
                }
                if (fld.DEPT_COMM_PERC != null)
                {
                    fld.DEPT_COMM_PERC_STRING = fld.DEPT_COMM_PERC.ToString();
                }
            }

            return View(fld);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(string id, [Bind] XstoreModel fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var editFld = db.xstore_organization.Where(y => y.edp == id).FirstOrDefault();
                if (fld.IS_PERC_BOOL)
                {
                    editFld.IS_PERC = "Y";
                    editFld.RD_COMM = 0;
                    editFld.RD_COMM_PERC = Convert.ToDecimal(fld.RD_COMM_PERC_STRING);

                }
                else
                {
                    editFld.IS_PERC = "N";
                    editFld.RD_COMM_PERC = Convert.ToDecimal(0);
                    editFld.RD_COMM = fld.RD_COMM;
                }
                if (fld.IS_PERC_DEPT_BOOL)
                {
                    editFld.IS_PERC_DEPT = "Y";
                    editFld.DEPT_COMM_PERC = Convert.ToDecimal(fld.DEPT_COMM_PERC_STRING);
                    editFld.DEPT_COMM = 0;
                }
                else
                {
                    editFld.IS_PERC_DEPT = "N";
                    editFld.DEPT_COMM_PERC = Convert.ToDecimal(0);
                    editFld.DEPT_COMM = fld.DEPT_COMM;
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
