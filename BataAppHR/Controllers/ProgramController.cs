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
    public class ProgramController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        public const string SessionKeyName2 = "RDCode";

        private IHostingEnvironment Environment;
        private readonly ILogger<ProgramController> _logger;

        public ProgramController(FormDBContext db, ILogger<ProgramController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
           var data = db.programDb.Where(y => y.FLAG_AKTIF == "1").OrderBy(y => y.ProgramId).ToList();

            return View(data);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbProgram obj)
        {
            if (ModelState.IsValid)
            {
                obj.Entry_Date = DateTime.Now;
                obj.Update_Date = DateTime.Now;
                obj.Entry_User = User.Identity.Name;
                obj.Update_User = User.Identity.Name;
                obj.FLAG_AKTIF = "1";
                obj.ProgramId = "";
                try
                {

                    db.programDb.Add(obj);
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
            return View(obj);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string  id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dbProgram fld = db.programDb.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
         
            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(string id, [Bind] dbProgram fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var editFld = db.programDb.Find(id);
                editFld.ProgramName = fld.ProgramName;

                editFld.Update_Date = DateTime.Now;
                editFld.Update_User = User.Identity.Name;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dbProgram fld = db.programDb.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProgram(string id)
        {
            dbProgram fld = db.programDb.Find(id);
            fld.FLAG_AKTIF = "0";
            fld.Update_Date = DateTime.Now;
            fld.Update_User = User.Identity.Name;
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
    }
}
