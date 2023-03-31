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
    public class ScoreDBController : Controller
    {
        private readonly FormDBContext db;

        private IHostingEnvironment Environment;
        private readonly ILogger<ScoreDBController> _logger;

        public ScoreDBController(FormDBContext db, ILogger<ScoreDBController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
           
            //var data = db.NilaissTblFixed.ToList();
            var tblRekap = db.rekapDb.ToList();
            var tblStaff = db.SSTable.ToList();
            var data = db.NilaissTblFixed.ToList().Select(y => new dbNilaiSSFixed()
            {
                ScoreId = y.ScoreId,
                SS_CODE = y.SS_CODE,
                NAMA_SS = tblStaff.Where(x => x.SS_CODE == y.SS_CODE).FirstOrDefault() != null ? tblStaff.Where(x => x.SS_CODE == y.SS_CODE).FirstOrDefault().NAMA_SS : "",
                TRN_ID = y.TRN_ID,
                NILAI = y.NILAI,
                SERTIFIKAT = y.SERTIFIKAT,
                NoSertifikat = y.NoSertifikat,
                FILE_SERTIFIKAT = y.FILE_SERTIFIKAT,
                Type = tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Type : "",
                Program = tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Program : "",
                ProgramTxt = tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Program : "",
                EDP = tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().EDP : "",
                Periode = tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Periode : "",
                Week = tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Week : "",
                Trainer = tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? tblRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Trainer : "",
            }).ToList();
            return View(data);
        }
        public JsonResult getDataRekap([FromQuery(Name = "TRN_ID")] string id)
        {
            //Creating List    
            List<dbRekapTraining> TblDt = new List<dbRekapTraining>();
            TblDt = db.rekapDb.Where(y => y.TRN_ID == id).ToList();
            return Json(TblDt);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            List<dbRekapTraining> rekapDt = new List<dbRekapTraining>();
            rekapDt = db.rekapDb.ToList().Select(y => new dbRekapTraining()
            {
                TRN_ID = y.TRN_ID,
                Program = y.TRN_ID + " - " + y.Program + " - " + y.ProgramTxt
            }).ToList();
            List<VaksinModel> SSList = new List<VaksinModel>();
            SSList = db.SSTable.ToList().Select(y => new VaksinModel()
            {
                SS_CODE = y.SS_CODE,
                NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
            }).ToList();
            dbNilaiSSFixed fld = new dbNilaiSSFixed();
            fld.rekapDD = rekapDt;
            fld.SSDD = SSList;
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbNilaiSSFixed objNilai)
        {
            if (ModelState.IsValid)
            {
                if (objNilai.isCertified)
                {
                    objNilai.SERTIFIKAT = 1;
                }
                else
                {
                    objNilai.SERTIFIKAT = 0;
                }
                objNilai.Entry_Date = DateTime.Now;
                objNilai.Update_Date = DateTime.Now;
                objNilai.Entry_User = User.Identity.Name;
                objNilai.Update_User = User.Identity.Name;
                objNilai.FLAG_AKTIF = "1";
                if (objNilai.fileSertifikat != null)
                {
                    string ext = System.IO.Path.GetExtension(objNilai.fileSertifikat.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objNilai.fileSertifikat.FileName);

                    objNilai.FILE_SERTIFIKAT = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    db.NilaissTblFixed.Add(objNilai);
                    db.SaveChanges();
                    var fileSertifikat = objNilai.fileSertifikat;
                    if (fileSertifikat != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objNilai.fileSertifikat.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objNilai.fileSertifikat.FileName);
                        string fname1 = objNilai.FILE_SERTIFIKAT;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileSertifikat.CopyTo(stream);
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
            return View(objNilai);
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<dbRekapTraining> rekapDt = new List<dbRekapTraining>();
            rekapDt = db.rekapDb.ToList().Select(y => new dbRekapTraining()
            {
                TRN_ID = y.TRN_ID,
                Program = y.TRN_ID + " - " + y.Program + " - " + y.ProgramTxt
            }).ToList();
            List<VaksinModel> SSList = new List<VaksinModel>();
            SSList = db.SSTable.ToList().Select(y => new VaksinModel()
            {
                SS_CODE = y.SS_CODE,
                NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
            }).ToList();
            dbNilaiSSFixed fld = db.NilaissTblFixed.Find(id);

            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                fld.rekapDD = rekapDt;
                fld.SSDD = SSList;
                if (fld.SERTIFIKAT == 1)
                {
                    fld.isCertified = true;
                }
                else
                {
                    fld.isCertified = false;

                }
            }

            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, [Bind] dbNilaiSSFixed fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (fld.isCertified)
                {
                    fld.SERTIFIKAT = 1;
                }
                else
                {
                    fld.SERTIFIKAT = 0;
                }
                var editFld = db.NilaissTblFixed.Find(id);

                editFld.SS_CODE = fld.SS_CODE;
                editFld.TRN_ID = fld.TRN_ID;
                editFld.ProgramTxt = fld.ProgramTxt;
                editFld.NILAI = fld.NILAI;
                editFld.SERTIFIKAT = fld.SERTIFIKAT;
                editFld.NoSertifikat = fld.NoSertifikat;

                editFld.Update_Date = DateTime.Now;
                editFld.Update_User = User.Identity.Name;

                if (fld.fileSertifikat != null)
                {
                    string ext = System.IO.Path.GetExtension(fld.fileSertifikat.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileSertifikat.FileName);
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");

                    FileInfo file = new FileInfo(Path.Combine(path2, editFld.FILE_SERTIFIKAT));
                    if (file.Exists)//check file exsit or not  
                    {
                        file.Delete();
                    }
                    editFld.FILE_SERTIFIKAT = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    db.SaveChanges();
                    var fileSertifikat2 = fld.fileSertifikat;
                    if (fileSertifikat2 != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(fld.fileSertifikat.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(fld.fileSertifikat.FileName);
                        string fname1 = editFld.FILE_SERTIFIKAT;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileSertifikat2.CopyTo(stream);
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
        public async Task<IActionResult> DownloadSertifikat([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dbNilaiSSFixed fld = db.NilaissTblFixed.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteNilai(int? id)
        {
            dbNilaiSSFixed fld = db.NilaissTblFixed.Find(id);

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
