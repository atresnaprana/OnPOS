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
    public class FormVaksinController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        public const string SessionKeyName2 = "SSCode";

        private IHostingEnvironment Environment;
        private readonly ILogger<FormVaksinController> _logger;
        
        public FormVaksinController(FormDBContext db, ILogger<FormVaksinController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            var data = new List<VaksinModel>();
            var edpCode = HttpContext.Session.GetString(SessionKeyName);
            var SSCode = HttpContext.Session.GetString(SessionKeyName2);

            if (!string.IsNullOrEmpty(edpCode) && !string.IsNullOrEmpty(SSCode))
            {
                data = db.SSTable.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == edpCode && y.SS_CODE == SSCode).ToList();
                ViewData["EdpCode"] = edpCode;
                ViewData["SSCode"] = SSCode;

            }
            else
            if (!string.IsNullOrEmpty(edpCode) && string.IsNullOrEmpty(SSCode))
            {
                data = db.SSTable.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == edpCode).ToList();
                ViewData["EdpCode"] = edpCode;
                ViewData["SSCode"] = 0;
            }
            else
            {
                ViewData["EdpCode"] = 0;
                ViewData["SSCode"] = 0;
            }
            return View(data);
        }
        
        public JsonResult getTbl(string id, string sscode)
        {
            HttpContext.Session.SetString(SessionKeyName, id);

            //Creating List    
            List<VaksinModel> TblDt = new List<VaksinModel>();
            if (!string.IsNullOrEmpty(sscode))
            {
                HttpContext.Session.SetString(SessionKeyName2, sscode);
                TblDt = db.SSTable.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == id && y.SS_CODE == sscode).ToList();
            }
            else
            {
                HttpContext.Session.SetString(SessionKeyName2, "");
                TblDt = db.SSTable.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == id).ToList();

            }
            return Json(TblDt);
        }
        public JsonResult getTblEmpty()
        {
            HttpContext.Session.SetString(SessionKeyName, "");
            HttpContext.Session.SetString(SessionKeyName2, "");

            //Creating List    
            List<VaksinModel> TblDt = new List<VaksinModel>();
          
            return Json(TblDt);
        }
        public JsonResult getXstore([FromQuery(Name = "edp")] string edp)
        {
            //Creating List    
            List<XstoreModel> TblDt = new List<XstoreModel>();
            TblDt = db.xstore_organization.Where(y => y.edp == edp).ToList();
            return Json(TblDt);
        }
        //public JsonResult getdata()
        //{
        //    List<string> edplist = new List<string>();
        //    edplist = db.SSTable.Select(y => y.EDP_CODE).ToList();

        //    return Json(new { data = edplist });
        //}
        public IActionResult getdata()
        {
            List<XstoreModel> edplist = new List<XstoreModel>();
            edplist = db.xstore_organization.ToList(); 
            return Json(edplist);
        }
        public IActionResult getdataSS(string edp)
        {
            List<VaksinModel> SSList = new List<VaksinModel>();
            if (!string.IsNullOrEmpty(edp))
            {
                SSList = db.SSTable.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == edp).ToList().Select(y => new VaksinModel()
                {
                    SS_CODE = y.SS_CODE,
                    NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
                }).ToList();
            }
            else
            {
                SSList = db.SSTable.Where(y => y.FLAG_AKTIF == 1).ToList().Select(y => new VaksinModel()
                {
                    SS_CODE = y.SS_CODE,
                    NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
                }).ToList();
            }
           
            return Json(SSList);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            List<XstoreModel> edplist = new List<XstoreModel>();
            edplist = db.xstore_organization.ToList().Select(y => new XstoreModel() { 
                edp = y.edp,
                store_location = y.edp + " - " + y.store_location
            }).ToList();
            VaksinModel fld = new VaksinModel();
            if (!User.IsInRole("Admin"))
            {
                var edpCode = HttpContext.Session.GetString(SessionKeyName);
                fld.EDP_CODE = edpCode;
            }
            fld.ddEdp = edplist.OrderBy(y => y.store_location).ToList();
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] VaksinModel objVaksin)
        {
            if (ModelState.IsValid)
            {
                if (objVaksin.VAKSIN1Bool)
                {
                    objVaksin.VAKSIN1 = "1";
                }
                else
                {
                    objVaksin.VAKSIN1 = "0";
                }
                if (objVaksin.VAKSIN2Bool)
                {
                    objVaksin.VAKSIN2 = "1";
                }
                else
                {
                    objVaksin.VAKSIN2 = "0";
                }
                objVaksin.ENTRY_DATE = DateTime.Now;
                objVaksin.UPDATE_DATE = DateTime.Now;
                objVaksin.ENTRY_USER = User.Identity.Name;
                objVaksin.UPDATE_USER = User.Identity.Name;
              
                objVaksin.FLAG_AKTIF = 1;
                if (objVaksin.JOIN_DATE != null)
                {
                    DateTime dtResign = DateTime.Now;
                    if (objVaksin.RESIGN_DATE != null)
                    {
                        dtResign = Convert.ToDateTime(objVaksin.RESIGN_DATE.Value);
                    }

                    DateTime dtJoin = Convert.ToDateTime(objVaksin.JOIN_DATE.Value);

                    TimeSpan timeSpan = dtResign - dtJoin;
                    DateTime age = DateTime.MinValue + timeSpan;
                    int years = age.Year - 1;
                    int months = age.Month - 1;
                    int days = age.Day - 1;
                    objVaksin.LAMA_KERJA = years + " Years " + months + " Months " + days + " days";
                    objVaksin.YEAR_LENGTH = years;
                    objVaksin.MONTH_LENGTH = months;
                    objVaksin.DAYS_LENGTH = days;
                }
                else
                {
                    objVaksin.LAMA_KERJA = "0" + " Years " + "0" + " Months " + "0" + " days";
                    objVaksin.YEAR_LENGTH = 0;
                    objVaksin.MONTH_LENGTH = 0;
                    objVaksin.DAYS_LENGTH = 0;
                }
               
                if (objVaksin.fileVaksin1 != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin1.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin1.FileName);

                    objVaksin.FOTOSERTIFIKAT1 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (objVaksin.fileVaksin2 != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin2.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin2.FileName);
                    objVaksin.FOTOSERTIFIKAT2 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (objVaksin.filePhoto != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.filePhoto.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.filePhoto.FileName);
                    objVaksin.STAFF_PHOTO = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (objVaksin.fileMedic != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.fileMedic.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileMedic.FileName);
                    objVaksin.FILE_MEDIC = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    db.SSTable.Add(objVaksin);
                    XstoreModel edpDt = db.xstore_organization.Where(y => y.edp == objVaksin.EDP_CODE).FirstOrDefault();
                    if (edpDt != null)
                    {
                        edpDt.UPDATE_DATE = DateTime.Now;
                        edpDt.UPDATE_USER = User.Identity.Name;
                    }
                    db.SaveChanges();
                    //var SSCodeFormat = "SS";
                    //var SSCodeCombined = SSCodeFormat + objVaksin.ID.ToString("0000");
                    //var sstblObj = db.SSTable.Find(objVaksin.ID);
                    //sstblObj.SS_CODE = SSCodeCombined;
                    //db.SaveChanges();
                    var fileVaksin1 = objVaksin.fileVaksin1;
                    if (fileVaksin1 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin1.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin1.FileName);
                        string fname1 = objVaksin.FOTOSERTIFIKAT1;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileVaksin1.CopyTo(stream);
                        }
                    }
                    var fileVaksin2 = objVaksin.fileVaksin2;
                    if (fileVaksin2 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin2.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin2.FileName);
                        string fname2 = objVaksin.FOTOSERTIFIKAT2;
                        var filePath = Path.Combine(path2, fname2);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileVaksin2.CopyTo(stream);
                        }
                    }
                  
                    var filePhoto = objVaksin.filePhoto;
                    if (filePhoto != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhoto");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objVaksin.filePhoto.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.filePhoto.FileName);
                        string fname2 = objVaksin.STAFF_PHOTO;
                        var filePath = Path.Combine(path2, fname2);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            filePhoto.CopyTo(stream);
                        }
                    }
                    var fileMedic = objVaksin.fileMedic;
                    if (fileMedic != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedic");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objVaksin.fileMedic.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileMedic.FileName);
                        string fname2 = objVaksin.FILE_MEDIC;
                        var filePath = Path.Combine(path2, fname2);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileMedic.CopyTo(stream);
                        }
                    }

                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd"+ (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                //apprDal.AddApproval(objApproval);
                return RedirectToAction("Index");
            }
            return View(objVaksin);
        }
        public async Task<IActionResult> DownloadVaksin1([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        public async Task<IActionResult> DownloadVaksin2([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        public async Task<IActionResult> DownloadPhoto([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhoto");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        public async Task<IActionResult> DownloadMedic([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedic");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<XstoreModel> edplist = new List<XstoreModel>();
            edplist = db.xstore_organization.ToList().Select(y => new XstoreModel()
            {
                edp = y.edp,
                store_location = y.edp + " - " + y.store_location
            }).ToList();
            VaksinModel fld = db.SSTable.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                
                fld.ddEdp = edplist.OrderBy(y => y.store_location).ToList();
                if (fld.JOIN_DATE != null)
                {
                    DateTime dtResign = DateTime.Now;
                    if (fld.RESIGN_DATE != null)
                    {
                        dtResign = fld.RESIGN_DATE.Value;
                    }

                    DateTime dtJoin = fld.JOIN_DATE.Value;

                    TimeSpan timeSpan = dtResign - dtJoin;
                    DateTime age = DateTime.MinValue + timeSpan;
                    int years = age.Year - 1;
                    int months = age.Month - 1;
                    int days = age.Day - 1;
                    fld.YEAR_LENGTH = years;
                    fld.MONTH_LENGTH = months;
                    fld.DAYS_LENGTH = days;
                }
                if (fld.VAKSIN1.Trim() == "1")
                {
                    fld.VAKSIN1Bool = true;
                }
                else
                {
                    fld.VAKSIN1Bool = false;

                }
                if (fld.VAKSIN2.Trim() == "1")
                {
                    fld.VAKSIN2Bool = true;
                }
                else
                {
                    fld.VAKSIN2Bool = false;
                }
            }
            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int? id, [Bind] VaksinModel fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (fld.VAKSIN1Bool)
                {
                    fld.VAKSIN1 = "1";
                }
                else
                {
                    fld.VAKSIN1 = "0";
                }
                if (fld.VAKSIN2Bool)
                {
                    fld.VAKSIN2 = "1";
                }
                else
                {
                    fld.VAKSIN2 = "0";
                }
               
                var editFld = db.SSTable.Find(id);
                editFld.EDP_CODE = fld.EDP_CODE;
                editFld.NAMA_SS = fld.NAMA_SS;
                editFld.SEX = fld.SEX;
                editFld.KTP = fld.KTP;
                editFld.HP_SS = fld.HP_SS;
                editFld.EMAIL_SS = fld.EMAIL_SS;
                editFld.SIZE_SERAGAM = fld.SIZE_SERAGAM;
                editFld.SIZE_SEPATU_UK = fld.SIZE_SEPATU_UK;
                editFld.RESIGN_DATE = fld.RESIGN_DATE;
                editFld.JOIN_DATE = fld.JOIN_DATE;
                editFld.RESIGN_TXT = fld.RESIGN_TXT;
                //editFld.LAMA_KERJA = fld.LAMA_KERJA;
                editFld.VAKSIN1 = fld.VAKSIN1;
                editFld.VAKSIN2 = fld.VAKSIN2;
                editFld.EMERGENCY_ADDRESS = fld.EMERGENCY_ADDRESS;
                editFld.EMERGENCY_NAME = fld.EMERGENCY_NAME;
                editFld.EMERGENCY_PHONE = fld.EMERGENCY_PHONE;
                editFld.RESIGN_TYPE = fld.RESIGN_TYPE;
                editFld.RESIGN_TYPE2 = fld.RESIGN_TYPE2;
                editFld.POSITION = fld.POSITION;
                //editFld.YEAR_LENGTH = fld.YEAR_LENGTH;
                //editFld.MONTH_LENGTH = fld.MONTH_LENGTH;
                //editFld.DAYS_LENGTH = fld.DAYS_LENGTH;
                if (fld.JOIN_DATE != null)
                {
                    DateTime dtResign = DateTime.Now;
                    if (fld.RESIGN_DATE != null)
                    {
                        dtResign = Convert.ToDateTime(fld.RESIGN_DATE.Value);
                    }

                    DateTime dtJoin = Convert.ToDateTime(fld.JOIN_DATE.Value);

                    TimeSpan timeSpan = dtResign - dtJoin;
                    DateTime age = DateTime.MinValue + timeSpan;
                    int years = age.Year - 1;
                    int months = age.Month - 1;
                    int days = age.Day - 1;
                    editFld.LAMA_KERJA = years + " Years " + months + " Months " + days + " days";
                    editFld.YEAR_LENGTH = years;
                    editFld.MONTH_LENGTH = months;
                    editFld.DAYS_LENGTH = days;
                }
                else
                {
                    editFld.LAMA_KERJA = "0" + " Years " + "0" + " Months " + "0" + " days";
                    editFld.YEAR_LENGTH = 0;
                    editFld.MONTH_LENGTH = 0;
                    editFld.DAYS_LENGTH = 0;
                }
                
                editFld.XSTORE_LOGIN = fld.XSTORE_LOGIN;
                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name;

                if (fld.fileVaksin1 != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.fileVaksin1.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin1.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1");
                    if (!string.IsNullOrEmpty(editFld.FOTOSERTIFIKAT1))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.FOTOSERTIFIKAT1));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    editFld.FOTOSERTIFIKAT1 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (fld.fileVaksin2 != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.fileVaksin2.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin2.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2");
                    if (!string.IsNullOrEmpty(editFld.FOTOSERTIFIKAT2))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.FOTOSERTIFIKAT2));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                   
                    editFld.FOTOSERTIFIKAT2 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (fld.fileMedic != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.fileMedic.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileMedic.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedic");
                    if (!string.IsNullOrEmpty(editFld.FILE_MEDIC))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.FILE_MEDIC));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    editFld.FILE_MEDIC = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (fld.filePhoto != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.filePhoto.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.filePhoto.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhoto");
                    if (!string.IsNullOrEmpty(editFld.STAFF_PHOTO))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.STAFF_PHOTO));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    editFld.STAFF_PHOTO = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    XstoreModel edpDt = db.xstore_organization.Where(y => y.edp == editFld.EDP_CODE).FirstOrDefault();
                    if (edpDt != null)
                    {
                        edpDt.UPDATE_DATE = DateTime.Now;
                        edpDt.UPDATE_USER = User.Identity.Name;
                    }
                    db.SaveChanges();
                    var fileVaksin1 = fld.fileVaksin1;
                    if (fileVaksin1 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileVaksin1.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin1.FileName);
                        string fname1 = editFld.FOTOSERTIFIKAT1;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileVaksin1.CopyTo(stream);
                        }
                    }
                    var fileVaksin2 = fld.fileVaksin2;
                    if (fileVaksin2 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileVaksin2.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin2.FileName);
                        string fname2 = editFld.FOTOSERTIFIKAT2;
                        var filePath = Path.Combine(path2, fname2);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileVaksin2.CopyTo(stream);
                        }
                    }
                    var fileMedic2 = fld.fileMedic;
                    if (fileMedic2 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedic");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileMedic.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileMedic.FileName);
                        string fname2 = editFld.FILE_MEDIC;
                        var filePath = Path.Combine(path2, fname2);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileMedic2.CopyTo(stream);
                        }
                    }
                    var filePhoto2 = fld.filePhoto;
                    if (filePhoto2 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhoto");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.filePhoto.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.filePhoto.FileName);
                        string fname2 = editFld.STAFF_PHOTO;
                        var filePath = Path.Combine(path2, fname2);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            filePhoto2.CopyTo(stream);
                        }
                    }
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VaksinModel fld = db.SSTable.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteVaksin(int? id)
        {
            VaksinModel fld = db.SSTable.Find(id);
            fld.FLAG_AKTIF = 0;
            fld.UPDATE_DATE = DateTime.Now;
            fld.UPDATE_USER = User.Identity.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult OnGetVaksin1()
        {
            var provider = new PhysicalFileProvider(Environment.WebRootPath);
            var contents = provider.GetDirectoryContents(Path.Combine("UploadsVaksin1"));
            var objFiles = contents.OrderBy(m => m.LastModified);
            return new JsonResult(objFiles);
        }
        public IActionResult OnGetVaksin2()
        {
            var provider = new PhysicalFileProvider(Environment.WebRootPath);
            var contents = provider.GetDirectoryContents(Path.Combine("UploadsVaksin2"));
            var objFiles = contents.OrderBy(m => m.LastModified);
            return new JsonResult(objFiles);
        }
    }
}