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
    public class MasterTrainerController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<MasterTrainerController> _logger;

        public MasterTrainerController(FormDBContext db, ILogger<MasterTrainerController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            var data = new List<dbTrainer>();
            data = db.trainerDb.Where(y => y.FLAG_AKTIF != "0").OrderBy(y => y.idTrainer).ToList();
            return View(data);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
           
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbTrainer objTrainer)
        {
            if (ModelState.IsValid)
            {

                objTrainer.Entry_Date = DateTime.Now;
                objTrainer.Update_Date = DateTime.Now;
                objTrainer.Entry_User = User.Identity.Name;
                objTrainer.Update_User = User.Identity.Name;
                objTrainer.FLAG_AKTIF = "1";
                objTrainer.idTrainer = "";
                try
                {
                    db.trainerDb.Add(objTrainer);
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
        [Authorize]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
          
            dbTrainer fld = db.trainerDb.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
         
            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind] dbTrainer fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
               

                var editFld = db.trainerDb.Find(id);
                editFld.NmTrainer = fld.NmTrainer;
                editFld.NmShort = fld.NmShort;
                editFld.HP = fld.HP;
                editFld.Email = fld.Email;
                
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
        [Authorize]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            dbTrainer fld = db.trainerDb.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTrainer(string id)
        {
            dbTrainer fld = db.trainerDb.Find(id);
            
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
                    fld.Update_Date = DateTime.Now;
                    fld.Update_User = User.Identity.Name;
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
