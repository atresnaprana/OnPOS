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
    public class FormDataRDController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        public const string SessionKeyName2 = "RDCode";

        private IHostingEnvironment Environment;
        private readonly ILogger<FormDataRDController> _logger;

        public FormDataRDController(FormDBContext db, ILogger<FormDataRDController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            var data = new List<dbRD>();
            var edpCode = HttpContext.Session.GetString(SessionKeyName);
            //var RDCode = HttpContext.Session.GetString(SessionKeyName2);

            //if (!string.IsNullOrEmpty(edpCode) && !string.IsNullOrEmpty(RDCode))
            //{
            //    data = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == edpCode && y.RD_CODE == RDCode).ToList();
            //    ViewData["EdpCode"] = edpCode;
            //    ViewData["RDCode"] = RDCode;

            //}
            //else
            //if (!string.IsNullOrEmpty(edpCode) && string.IsNullOrEmpty(RDCode))
            //{
            //    data = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == edpCode).ToList();
            //    ViewData["EdpCode"] = edpCode;
            //    ViewData["RDCode"] = 0;
            //}
            //else
            //{
            //    ViewData["EdpCode"] = 0;
            //    ViewData["RDCode"] = 0;
            //}
            if (!string.IsNullOrEmpty(edpCode))
            {
                var edpcoderegistered = db.xstore_organization.Where(y => y.edp == edpCode).ToList();
                List<string> rdcodelist = new List<string>();
                foreach(var flds in edpcoderegistered)
                {
                    rdcodelist.Add(flds.RD_CODE);
                }
                ViewData["EdpCode"] = edpCode;
                data = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && rdcodelist.Contains(y.RD_CODE)).OrderBy(y => y.RD_CODE).ToList();
            }
            else
            {
                ViewData["EdpCode"] = 0;
                data = db.RDTbl.Where(y => y.FLAG_AKTIF == 1).OrderBy(y => y.RD_CODE).ToList();

            }
            return View(data);
        }
        public IActionResult getdataRD(string edp)
        {
            List<dbRD> RDList = new List<dbRD>();
            if (!string.IsNullOrEmpty(edp))
            {
                RDList = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == edp).ToList().Select(y => new dbRD()
                {
                    RD_CODE = y.RD_CODE,
                    NM_RD = y.EDP_CODE + " - " + y.RD_CODE + " - " + y.NM_RD
                }).ToList();
            }
            else
            {
                RDList = db.RDTbl.Where(y => y.FLAG_AKTIF == 1).ToList().Select(y => new dbRD()
                {
                    RD_CODE = y.RD_CODE,
                    NM_RD = y.EDP_CODE + " - " + y.RD_CODE + " - " + y.NM_RD
                }).ToList();
            }

            return Json(RDList);
        }
        public IActionResult getdata()
        {
            List<XstoreModel> edplist = new List<XstoreModel>();
            edplist = db.xstore_organization.ToList();
            return Json(edplist);
        }
        public JsonResult getTbl(string id, string rdcode)
        {
            //HttpContext.Session.SetString(SessionKeyName, id);

            //Creating List    
            List<dbRD> TblDt = new List<dbRD>();
            //if (!string.IsNullOrEmpty(rdcode))
            //{
            //    HttpContext.Session.SetString(SessionKeyName2, rdcode);
            //    TblDt = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == id && y.RD_CODE == rdcode).ToList();
            //}
            //else
            //{
            //    HttpContext.Session.SetString(SessionKeyName2, "");
            //    TblDt = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == id).ToList();

            //}
            TblDt = db.RDTbl.Where(y => y.FLAG_AKTIF == 1).ToList();

            return Json(TblDt);
        }
        public JsonResult getTblEmpty()
        {
            HttpContext.Session.SetString(SessionKeyName, "");
            HttpContext.Session.SetString(SessionKeyName2, "");
            //Creating List    
            List<dbRD> TblDt = new List<dbRD>();
            return Json(TblDt);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            List<XstoreModel> edplist = new List<XstoreModel>();
            edplist = db.xstore_organization.Where(y => string.IsNullOrEmpty(y.RD_CODE)).ToList().Select(y => new XstoreModel()
            {
                edp = y.edp,
                store_location = y.edp + " - " + y.store_location
            }).ToList();
            dbRD fld = new dbRD();
            
            fld.ddEdp = edplist.OrderBy(y => y.store_location).ToList();
            var edpCode = HttpContext.Session.GetString(SessionKeyName);
            if (!string.IsNullOrEmpty(edpCode))
            {
                fld.EDP_CODE = edpCode;
                ViewBag.edp = edpCode;
            }
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbRD objVaksin)
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
                objVaksin.RD_CODE = "";
                if (objVaksin.fileVaksin1 != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin1.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin1.FileName);

                    objVaksin.FILE_SERTIFIKAT1 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (objVaksin.fileVaksin2 != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin2.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin2.FileName);
                    objVaksin.FILE_SERTIFIKAT2 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (objVaksin.filePhoto != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.filePhoto.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.filePhoto.FileName);
                    objVaksin.RD_PHOTO = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (objVaksin.fileMedic != null)
                {
                    string ext = System.IO.Path.GetExtension(objVaksin.fileMedic.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileMedic.FileName);
                    objVaksin.FILE_MEDIC = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {

                    db.RDTbl.Add(objVaksin);
                    
                    db.SaveChanges();
                    var rdcode = objVaksin.RD_CODE;
                    var dbEDP = db.xstore_organization.Where(y => string.IsNullOrEmpty(y.RD_CODE)).ToList();
                    var edpCode = HttpContext.Session.GetString(SessionKeyName);
                    if (!string.IsNullOrEmpty(edpCode))
                    {
                        var edpFld = dbEDP.Where(y => y.edp == edpCode).FirstOrDefault();
                        XstoreModel edpDt = db.xstore_organization.Where(y => y.edp == edpCode).FirstOrDefault();
                        if (edpDt != null)
                        {
                            edpDt.UPDATE_DATE = DateTime.Now;
                            edpDt.UPDATE_USER = User.Identity.Name;
                        }
                        if (edpFld != null)
                        {
                            edpFld.RD_CODE = rdcode;
                            db.xstore_organization.Update(edpFld);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < objVaksin.EDP_CODE_LIST.Count(); i++)
                        {
                            var edp = objVaksin.EDP_CODE_LIST[i];
                            var edpFld = dbEDP.Where(y => y.edp == edp).FirstOrDefault();
                            XstoreModel edpDt = db.xstore_organization.Where(y => y.edp == edp).FirstOrDefault();
                            if (edpDt != null)
                            {
                                edpDt.UPDATE_DATE = DateTime.Now;
                                edpDt.UPDATE_USER = User.Identity.Name;
                            }
                            if (edpFld != null)
                            {
                                edpFld.RD_CODE = rdcode;
                                db.xstore_organization.Update(edpFld);
                            }
                        }
                    }
                    
                    db.SaveChanges();
                  
                    var fileVaksin1 = objVaksin.fileVaksin1;
                    if (fileVaksin1 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1RD");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin1.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin1.FileName);
                        string fname1 = objVaksin.FILE_SERTIFIKAT1;

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

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2RD");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objVaksin.fileVaksin2.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.fileVaksin2.FileName);
                        string fname2 = objVaksin.FILE_SERTIFIKAT2;
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

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhotoRD");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objVaksin.filePhoto.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objVaksin.filePhoto.FileName);
                        string fname2 = objVaksin.RD_PHOTO;
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

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedicRD");
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
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
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
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1RD");
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
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2RD");
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
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhotoRD");
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
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedicRD");
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
           
            dbRD fld = db.RDTbl.Find(id);
            edplist = db.xstore_organization.Where(y=> string.IsNullOrEmpty(y.RD_CODE) || y.RD_CODE == fld.RD_CODE).ToList().Select(y => new XstoreModel()
            {
                edp = y.edp,
                store_location = y.edp + " - " + y.store_location
            }).ToList();
            var edpcoderegistered = db.xstore_organization.Where(y => y.RD_CODE == fld.RD_CODE).ToList();
            List<string> edpcodelist = new List<string>();
            foreach(var field in edpcoderegistered)
            {
                edpcodelist.Add(field.edp);
            }
            fld.EDP_CODE_LIST = edpcodelist;
            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                var edpCode = HttpContext.Session.GetString(SessionKeyName);
                if (!string.IsNullOrEmpty(edpCode))
                {
                    fld.EDP_CODE = edpCode;
                    ViewBag.edp = edpCode;
                }
                fld.ddEdp = edplist.OrderBy(y => y.store_location).ToList();
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
        public JsonResult getMultiValue(string rdcode)
        {
            //HttpContext.Session.SetString(SessionKeyName, id);

            //Creating List    
            List<string> TblDt = new List<string>();
            //if (!string.IsNullOrEmpty(rdcode))
            //{
            //    HttpContext.Session.SetString(SessionKeyName2, rdcode);
            //    TblDt = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == id && y.RD_CODE == rdcode).ToList();
            //}
            //else
            //{
            //    HttpContext.Session.SetString(SessionKeyName2, "");
            //    TblDt = db.RDTbl.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == id).ToList();

            //}
            var xstores = db.xstore_organization.Where(y => y.RD_CODE == rdcode).ToList();
            foreach(var fld in xstores)
            {
                TblDt.Add(fld.edp);
            }
            return Json(TblDt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int? id, [Bind] dbRD fld)
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

                var editFld = db.RDTbl.Find(id);
                editFld.EDP_CODE = fld.EDP_CODE;
                editFld.NM_RD = fld.NM_RD;
                editFld.SEX = fld.SEX;
                editFld.No_KTP = fld.No_KTP;
                editFld.RD_HP = fld.RD_HP;
                editFld.RD_EMAIL = fld.RD_EMAIL;
                editFld.RD_SERAGAM_SIZE = fld.RD_SERAGAM_SIZE;
                editFld.RD_SEPATU_SIZEUK = fld.RD_SEPATU_SIZEUK;
                editFld.RESIGN_DATE = fld.RESIGN_DATE;
                editFld.JOIN_DATE = fld.JOIN_DATE;
                editFld.RESIGN_TXT = fld.RESIGN_TXT;
                editFld.LAMA_KERJA = fld.LAMA_KERJA;
                editFld.VAKSIN1 = fld.VAKSIN1;
                editFld.VAKSIN2 = fld.VAKSIN2;
                editFld.EMERGENCY_ADDRESS = fld.EMERGENCY_ADDRESS;
                editFld.EMERGENCY_NAME = fld.EMERGENCY_NAME;
                editFld.EMERGENCY_PHONE = fld.EMERGENCY_PHONE;
                editFld.RESIGN_TYPE = fld.RESIGN_TYPE;
                editFld.RESIGN_TYPE2 = fld.RESIGN_TYPE2;
                editFld.YEAR_LENGTH = fld.YEAR_LENGTH;
                editFld.MONTH_LENGTH = fld.MONTH_LENGTH;
                editFld.DAYS_LENGTH = fld.DAYS_LENGTH;
                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name;
               
                if (fld.fileVaksin1 != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.fileVaksin1.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin1.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1RD");
                    if (!string.IsNullOrEmpty(editFld.FILE_SERTIFIKAT1))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.FILE_SERTIFIKAT1));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    editFld.FILE_SERTIFIKAT1 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (fld.fileVaksin2 != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.fileVaksin2.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin2.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2RD");
                    if (!string.IsNullOrEmpty(editFld.FILE_SERTIFIKAT2))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.FILE_SERTIFIKAT2));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }

                    editFld.FILE_SERTIFIKAT2 = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (fld.fileMedic != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.fileMedic.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileMedic.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedicRD");
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
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhotoRD");
                    if (!string.IsNullOrEmpty(editFld.RD_PHOTO))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.RD_PHOTO));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    editFld.RD_PHOTO = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    
                    if (fld.EDP_CODE_LIST != null)
                    {
                        var edplist = fld.EDP_CODE_LIST;
                        //Processing UserData
                        List<string> listtrans = new List<string>();
                        List<string> listExistingtrans = new List<string>();

                        var idToRemovetrans = new List<string>();
                        var idToAddtrans = new List<string>();

                        var ExistingTransDt = db.xstore_organization.Where(y => y.RD_CODE == editFld.RD_CODE).ToList();
                        for (int i = 0; i < edplist.Count(); i++)
                        {
                            var idDtl = edplist[i];
                            listtrans.Add(idDtl);
                            idToAddtrans.Add(idDtl);
                        }

                        foreach (var exist in ExistingTransDt)
                        {
                            var transExist = exist.edp;
                            listExistingtrans.Add(transExist);
                            idToRemovetrans.Add(transExist);

                        }

                        //removing logic 
                        for (int i = 0; i < listExistingtrans.Count(); i++)
                        {
                            var nopekExist = listExistingtrans[i];
                            for (int y = 0; y < listtrans.Count(); y++)
                            {
                                var nopekNew = listtrans[y];
                                if (nopekExist == nopekNew)
                                {
                                    idToRemovetrans.Remove(nopekExist);
                                }
                            }
                        }
                        var empDt = ExistingTransDt.Where(y => idToRemovetrans.Contains(y.edp)).ToList<XstoreModel>();
                        foreach (var dtlemp in empDt)
                        {
                            var formEmp = dtlemp;
                            formEmp.RD_CODE = "";
                            XstoreModel edpDt = db.xstore_organization.Where(y => y.edp == dtlemp.edp).FirstOrDefault();
                            if (edpDt != null)
                            {
                                edpDt.UPDATE_DATE = DateTime.Now;
                                edpDt.UPDATE_USER = User.Identity.Name;
                            }
                            db.xstore_organization.Update(formEmp);
                        }
                        //adding logic
                        foreach (var dts in edplist)
                        {
                            var flds = db.xstore_organization.Where(y => y.edp == dts).FirstOrDefault();
                            flds.RD_CODE = editFld.RD_CODE;
                            XstoreModel edpDt = db.xstore_organization.Where(y => y.edp == dts).FirstOrDefault();
                            if (edpDt != null)
                            {
                                edpDt.UPDATE_DATE = DateTime.Now;
                                edpDt.UPDATE_USER = User.Identity.Name;
                            }
                            db.xstore_organization.Update(flds);
                        }
                    }
                  
                    db.SaveChanges();
                    var fileVaksin1 = fld.fileVaksin1;
                    if (fileVaksin1 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin1RD");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileVaksin1.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin1.FileName);
                        string fname1 = editFld.FILE_SERTIFIKAT1;

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

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsVaksin2RD");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileVaksin2.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileVaksin2.FileName);
                        string fname2 = editFld.FILE_SERTIFIKAT2;
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

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsMedicRD");
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

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsPhotoRD");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.filePhoto.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.filePhoto.FileName);
                        string fname2 = editFld.RD_PHOTO;
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
            dbRD fld = db.RDTbl.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRD(int? id)
        {
            dbRD fld = db.RDTbl.Find(id);
            fld.FLAG_AKTIF = 0;
            fld.UPDATE_DATE = DateTime.Now;
            fld.UPDATE_USER = User.Identity.Name;
            var empDt = db.xstore_organization.Where(y => y.RD_CODE == fld.RD_CODE).ToList<XstoreModel>();
            foreach (var dtlemp in empDt)
            {
                var formEmp = dtlemp;
                formEmp.RD_CODE = "";
                db.xstore_organization.Update(formEmp);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
