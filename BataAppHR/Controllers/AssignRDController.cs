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
using System.Security.Claims;

namespace BataAppHR.Controllers
{
    public class AssignRDController : Controller
    {
        private readonly FormDBContext db;

        public const string SessionKeyName = "EDPCode";
        private IHostingEnvironment Environment;
        private readonly ILogger<AssignRDController> _logger;

        public AssignRDController(FormDBContext db, ILogger<AssignRDController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<dbRD> rdList = new List<dbRD>();
            rdList = db.RDTbl.ToList().Select(y => new dbRD()
            {
                RD_CODE = y.RD_CODE,
                NM_RD = y.NM_RD
            }).ToList();
            XstoreModel fld = new XstoreModel();
            if (!User.IsInRole("Admin"))
            {
                var edpCode = HttpContext.Session.GetString(SessionKeyName);
                var fldxstore = db.xstore_organization.Where(y => y.edp == edpCode).FirstOrDefault();
                fld.edp = edpCode;
                if(fldxstore != null)
                {
                    fld.store_location = fldxstore.store_location;
                    fld.district = fldxstore.district;
                    fld.area = fldxstore.area;
                    fld.RD_CODE = fldxstore.RD_CODE;
                }

            }
            else
            {
                List<XstoreModel> edplist = new List<XstoreModel>();
                edplist = db.xstore_organization.Where(y => y.inactive_flag == "0").ToList().Select(y => new XstoreModel()
                {
                    edp = y.edp,
                    store_location = y.edp + " - " + y.store_location
                }).ToList();
                fld.ddEdp = edplist;
            }
            fld.rdlist = rdList.OrderBy(y => y.NM_RD).ToList();
            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateData(int? id, [Bind] XstoreModel fld)
        {
            if (string.IsNullOrEmpty(fld.edp))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
               
                var editFld = db.xstore_organization.Where(y => y.edp == fld.edp).FirstOrDefault();
                editFld.RD_CODE = fld.RD_CODE;
                
                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name ;
                //editFld.YEAR_LENGTH = fld.YEAR_LENGTH;
                //editFld.MONTH_LENGTH = fld.MONTH_LENGTH;
                //editFld.DAYS_LENGTH = fld.DAYS_LENGTH;
               
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
        public JsonResult getXstore([FromQuery(Name = "edp")] string edp)
        {
            //Creating List    
            List<XstoreModel> TblDt = new List<XstoreModel>();
            TblDt = db.xstore_organization.Where(y => y.edp == edp).ToList();
            return Json(TblDt);
        }

    }
}
