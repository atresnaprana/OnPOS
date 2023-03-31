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
    public class XstoreEditController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<XstoreEditController> _logger;
        public XstoreEditController(FormDBContext db, ILogger<XstoreEditController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "Real Estate Role")]
        public IActionResult Index()
        {
            var data = new List<XstoreModel>();
            data = db.xstore_organization.Where(y => y.FLAG_APPROVAL == 4 && y.inactive_flag == "0" && y.CLOSE_DATE == null).OrderBy(y => y.edp).ToList();
            return View(data);
        }
        //public IActionResult GetImage(int id)
        //{
        //    // determine file path from id and then
        //    var field = db.xstore_organization.Where(y => y.id == id).FirstOrDefault();

        //    return File(path, "image/jpeg");
        //}
        [HttpGet]
        [Authorize(Roles = "AdminRealEstate")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create([Bind] XstoreModel fld)
        {
          
            if (ModelState.IsValid)
            {
                if (fld.inactive_flagBool)
                {
                    fld.inactive_flag = "1";
                }
                else
                {
                    fld.inactive_flag = "0";
                }
                if (fld.genesis_FlagBool)
                {
                    fld.genesis_Flag = "Genesis";
                }
                else
                {
                    fld.genesis_Flag = "Non Genesis";
                }
                if (fld.IS_DS_BOOL)
                {
                    fld.IS_DS = "Y";
                }
                else
                {
                    fld.IS_DS = "N";
                }
                if (!string.IsNullOrEmpty(fld.SELLING_AREA_STRING))
                {
                    fld.SELLING_AREA = Convert.ToDecimal(fld.SELLING_AREA_STRING);
                }
                if (!string.IsNullOrEmpty(fld.STOCK_AREA_STRING))
                {
                    fld.STOCK_AREA = Convert.ToDecimal(fld.STOCK_AREA_STRING);
                }
                if (!string.IsNullOrEmpty(fld.TOTAL_AREA_STRING))
                {
                    fld.TOTAL_AREA = Convert.ToDecimal(fld.TOTAL_AREA_STRING);
                }
                if (!string.IsNullOrEmpty(fld.GROSS_VAL_STRING))
                {
                    fld.GROSS_VAL = Convert.ToDecimal(fld.GROSS_VAL_STRING);
                }
                if (!string.IsNullOrEmpty(fld.STORAGE_VAL_STRING))
                {
                    fld.STORAGE_VAL = Convert.ToDecimal(fld.STORAGE_VAL_STRING);
                }
                if (!string.IsNullOrEmpty(fld.SELLING_VAL_STRING))
                {
                    fld.SELLING_VAL = Convert.ToDecimal(fld.SELLING_VAL_STRING);
                }
               
                if (fld.LeaseFile != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.LeaseFile.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.LeaseFile.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsLease");
                
                    fld.FILE_LEASE = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (fld.FileOthers != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.FileOthers.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.FileOthers.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsREOthers");
                
                    fld.FILE_OTHERS = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }

                if (fld.fileToko != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        fld.fileToko.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        fld.STORE_IMG_BLOB = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);

                        // act on the Base64 data
                    }

                    string ext = System.IO.Path.GetExtension(fld.fileToko.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileToko.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsStoreImage");
                  

                    fld.STORE_IMAGE = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    fld.ENTRY_DATE = DateTime.Now;
                    fld.UPDATE_DATE = DateTime.Now;
                    fld.ENTRY_USER = User.Identity.Name;
                    fld.UPDATE_USER = User.Identity.Name;
                    fld.FLAG_APPROVAL = 4;
                    db.xstore_organization.Add(fld);
                    db.SaveChanges();
                    var fileToko = fld.fileToko;
                    if (fileToko != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsStoreImage");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileToko.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileToko.FileName);
                        string fname1 = fld.STORE_IMAGE;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileToko.CopyTo(stream);
                        }
                    }
                    var fileLease = fld.LeaseFile;
                    if (fileLease != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsLease");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.LeaseFile.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.LeaseFile.FileName);
                        string fname1 = fld.FILE_LEASE;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileLease.CopyTo(stream);
                        }
                    }
                    var fileOthers = fld.FileOthers;
                    if (fileOthers != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsREOthers");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.FileOthers.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.FileOthers.FileName);
                        string fname1 = fld.FILE_OTHERS;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileOthers.CopyTo(stream);
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
        public JsonResult validateedp(string edp)
        {
            string message = "ok";
            //Creating List    
            var xstorevalidate = db.xstore_organization.Where(y => y.edp == edp && y.inactive_flag == "0" && y.CLOSE_DATE == null).ToList();
            if(xstorevalidate.Count() > 0)
            {
                message = "Duplicate EDP";
            }
            return Json(message);
        }

        [Authorize(Roles = "Real Estate Role")]
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
                if (fld.STORE_IMG_BLOB != null)
                {
                    string imageBase64Data = Convert.ToBase64String(fld.STORE_IMG_BLOB);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageData = imageDataURL;
                }
                if (fld.SELLING_AREA != null)
                {
                    fld.SELLING_AREA_STRING = fld.SELLING_AREA.ToString();
                }
                if (fld.STOCK_AREA != null)
                {
                    fld.STOCK_AREA_STRING = fld.STOCK_AREA.ToString();
                }
                if (fld.TOTAL_AREA != null)
                {
                    fld.TOTAL_AREA_STRING = fld.TOTAL_AREA.ToString();
                }
                if (fld.GROSS_VAL != null)
                {
                    fld.GROSS_VAL_STRING = fld.GROSS_VAL.ToString();
                }
                if (fld.STORAGE_VAL != null)
                {
                    fld.STORAGE_VAL_STRING = fld.STORAGE_VAL.ToString();
                }
                if (fld.SELLING_VAL != null)
                {
                    fld.SELLING_VAL_STRING = fld.SELLING_VAL.ToString();
                }
                if (!string.IsNullOrEmpty(fld.genesis_Flag))
                {
                    if (fld.genesis_Flag.Trim() == "Genesis")
                    {
                        fld.genesis_FlagBool = true;
                    }
                    else
                    {
                        fld.genesis_FlagBool = false;

                    }
                }
                else
                {
                    fld.genesis_FlagBool = false;
                }
                if (!string.IsNullOrEmpty(fld.inactive_flag))
                {
                    if (fld.inactive_flag.Trim() == "1")
                    {
                        fld.inactive_flagBool = true;
                    }
                    else
                    {
                        fld.inactive_flagBool = false;
                    }
                }
                else
                {
                    fld.inactive_flagBool = false;

                }
                if (!string.IsNullOrEmpty(fld.IS_DS))
                {
                    if (fld.IS_DS.Trim() == "Y")
                    {
                        fld.IS_DS_BOOL = true;
                    }
                    else
                    {
                        fld.IS_DS_BOOL = false;
                    }
                }
                else
                {
                    fld.IS_DS_BOOL = false;

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
                if (fld.inactive_flagBool)
                {
                    fld.inactive_flag = "1";
                }
                else
                {
                    fld.inactive_flag = "0";
                }
                if (fld.IS_DS_BOOL)
                {
                    fld.IS_DS = "Y";
                }
                else
                {
                    fld.IS_DS = "N";
                }
                if (fld.genesis_FlagBool)
                {
                    fld.genesis_Flag = "Genesis";
                }
                else
                {
                    fld.genesis_Flag = "Non Genesis";
                }
                if (!string.IsNullOrEmpty(fld.SELLING_AREA_STRING))
                {
                    fld.SELLING_AREA = Convert.ToDecimal(fld.SELLING_AREA_STRING);
                }
                if (!string.IsNullOrEmpty(fld.STOCK_AREA_STRING))
                {
                    fld.STOCK_AREA = Convert.ToDecimal(fld.STOCK_AREA_STRING);
                }
                if (!string.IsNullOrEmpty(fld.TOTAL_AREA_STRING))
                {
                    fld.TOTAL_AREA = Convert.ToDecimal(fld.TOTAL_AREA_STRING);
                }
                if (!string.IsNullOrEmpty(fld.GROSS_VAL_STRING))
                {
                    fld.GROSS_VAL = Convert.ToDecimal(fld.GROSS_VAL_STRING);
                }
                if (!string.IsNullOrEmpty(fld.STORAGE_VAL_STRING))
                {
                    fld.STORAGE_VAL = Convert.ToDecimal(fld.STORAGE_VAL_STRING);
                }
                if (!string.IsNullOrEmpty(fld.SELLING_VAL_STRING))
                {
                    fld.SELLING_VAL = Convert.ToDecimal(fld.SELLING_VAL_STRING);
                }
                var editFld = db.xstore_organization.Where(y => y.edp == id).FirstOrDefault();
                editFld.district = fld.district;
                editFld.area = fld.area;
                editFld.store_location = fld.store_location;
                editFld.inactive_flag = fld.inactive_flag;
                editFld.genesis_Flag = fld.genesis_Flag;
              
                editFld.STORE_CONCEPT = fld.STORE_CONCEPT;
                editFld.SELLING_AREA = fld.SELLING_AREA;
                editFld.STOCK_AREA = fld.STOCK_AREA;
                editFld.TOTAL_AREA = fld.TOTAL_AREA;
                editFld.ADDRESS = fld.ADDRESS;
                editFld.OPENING_DATE = fld.OPENING_DATE;
                editFld.LEASE_START = fld.LEASE_START;
                editFld.LEASE_EXPIRED = fld.LEASE_EXPIRED;
                editFld.LAST_RENOVATION_DATE = fld.LAST_RENOVATION_DATE;
                editFld.GROSS_VAL = fld.GROSS_VAL;
                editFld.SELLING_VAL = fld.SELLING_VAL;
                editFld.STORAGE_VAL = fld.STORAGE_VAL;

                editFld.IS_DS = fld.IS_DS;
                editFld.CLOSE_DATE = fld.CLOSE_DATE;
                if (fld.LeaseFile != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.LeaseFile.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.LeaseFile.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsLease");
                    if (!string.IsNullOrEmpty(editFld.FILE_LEASE))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.FILE_LEASE));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    editFld.FILE_LEASE = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                if (fld.FileOthers != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.FileOthers.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.FileOthers.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsREOthers");
                    if (!string.IsNullOrEmpty(editFld.FILE_OTHERS))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.FILE_OTHERS));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    editFld.FILE_OTHERS = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
            
                if (fld.fileToko != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        fld.fileToko.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.STORE_IMG_BLOB = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);

                        // act on the Base64 data
                    }

                    string ext = System.IO.Path.GetExtension(fld.fileToko.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileToko.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsStoreImage");
                    if (!string.IsNullOrEmpty(editFld.STORE_IMAGE))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, editFld.STORE_IMAGE));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                   
                    editFld.STORE_IMAGE = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    editFld.UPDATE_DATE = DateTime.Now;
                    editFld.UPDATE_USER = User.Identity.Name;
                    db.SaveChanges();
                    var fileToko = fld.fileToko;
                    if (fileToko != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsStoreImage");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileToko.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileToko.FileName);
                        string fname1 = editFld.STORE_IMAGE;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileToko.CopyTo(stream);
                        }
                    }
                    var fileLease = fld.LeaseFile;
                    if (fileLease != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsLease");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.LeaseFile.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.LeaseFile.FileName);
                        string fname1 = editFld.FILE_LEASE;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileLease.CopyTo(stream);
                        }
                    }
                    var fileOthers = fld.FileOthers;
                    if (fileOthers != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsREOthers");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.FileOthers.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.FileOthers.FileName);
                        string fname1 = editFld.FILE_OTHERS;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileOthers.CopyTo(stream);
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

        public async Task<IActionResult> DownloadLease([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsLease");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        public async Task<IActionResult> DownloadOthers([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsREOthers");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        public async Task<IActionResult> DownloadStoreImg([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsStoreImage");
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
    }
}
