using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BataAppHR.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using BataAppHR.Data;
using Microsoft.AspNetCore.Authorization;

namespace BataAppHR.Controllers
{
    public class UploadVaksinController : Controller
    {
        private readonly ILogger<UploadVaksinController> _logger;
        private IHostingEnvironment Environment;
        const string SessionName = "_Name";
        private readonly FormDBContext db;


        public UploadVaksinController(ILogger<UploadVaksinController> logger, IHostingEnvironment _environment, FormDBContext db)
        {
            _logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVaksin(IFormFile file)
        {
            List<VaksinModel> vaksinList = new List<VaksinModel>();
            var objVaksin = new VaksinModel();
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsXls");
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
            }
            else
            {
               
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                var filePath = Path.Combine(path2, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                bool hasHeader = true;
                using (var excelPack = new ExcelPackage())
                {
                    FileInfo fi = new FileInfo(filePath);

                    //Load excel stream
                    using (var stream = fi.OpenRead())
                    {
                        excelPack.Load(stream);
                    }

                    //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                    var ws = excelPack.Workbook.Worksheets[0];

                    //Get all details as DataTable -because Datatable make life easy :)
                    //DataTable excelasTable = new DataTable();
                    //var excelasTable = model.ApplicationUsers;

                    var start = ws.Dimension.Start;
                    var end = ws.Dimension.End;
                    var startrow = start.Row + 1;
                    for (int row = startrow; row <= end.Row; row++)
                    { // Row by row...
                      //UserToRegister field = new UserToRegister();
                        var fld = new VaksinModel();
                        object SSCode = ws.Cells[row, 1].Value;
                        object EDPCode = ws.Cells[row, 2].Value;
                        object NamaSS = ws.Cells[row, 3].Value;
                        object flagAktif = ws.Cells[row, 4].Value;
                        object Sex = ws.Cells[row, 5].Value;
                        object KTP = ws.Cells[row, 6].Value;
                        object HPSS = ws.Cells[row, 7].Value;
                        object EmailSS = ws.Cells[row, 8].Value;
                        object SizeSeragam = ws.Cells[row, 9].Value;
                        object SizeSepatuUK = ws.Cells[row, 10].Value;
                        object ResignDate = ws.Cells[row, 11].Value;
                        object JoinDate = ws.Cells[row, 12].Value;
                        object ResignTxt = ws.Cells[row, 13].Value;
                        object LamaKerja = ws.Cells[row, 14].Value;
                        object Vaksin1 = ws.Cells[row, 15].Value;
                        object Vaksin2 = ws.Cells[row, 16].Value;
                        fld.SS_CODE = SSCode.ToString();
                        if(EDPCode != null)
                        {
                            fld.EDP_CODE = EDPCode.ToString();
                        }
                        if(NamaSS != null)
                        {
                            fld.NAMA_SS = NamaSS.ToString();
                        }
                        if(flagAktif != null)
                        {
                            fld.FLAG_AKTIF = Convert.ToInt32(flagAktif);
                        }
                        if (Sex != null)
                        {
                            fld.SEX = Sex.ToString();
                        }
                        if (KTP != null)
                        {
                            fld.KTP = KTP.ToString();
                        }
                        if (HPSS != null)
                        {
                            fld.HP_SS = HPSS.ToString();
                        }
                        if (EmailSS != null)
                        {
                            fld.EMAIL_SS = EmailSS.ToString();
                        }
                        if (SizeSeragam != null)
                        {
                            fld.SIZE_SERAGAM = SizeSeragam.ToString();
                        }
                        if (SizeSepatuUK != null)
                        {
                            fld.SIZE_SEPATU_UK = SizeSepatuUK.ToString();
                        }
                        if(ResignDate != null)
                        {
                            if (!string.IsNullOrEmpty(ResignDate.ToString()))
                            {

                                DateTime temp;
                                if (DateTime.TryParse(ResignDate.ToString(), out temp))
                                {
                                    fld.RESIGN_DATE = Convert.ToDateTime(ResignDate);
                                }
                                else
                                {

                                    try
                                    {

                                        var month = Convert.ToInt32(ResignDate.ToString().Split("/")[0]);
                                        var date = Convert.ToInt32(ResignDate.ToString().Split("/")[1]);
                                        var year = Convert.ToInt32(ResignDate.ToString().Split("/")[2]);
                                        var newDate = new DateTime(year, month, date);
                                        fld.RESIGN_DATE = newDate;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                        if(JoinDate != null)
                        {
                            if (!string.IsNullOrEmpty(JoinDate.ToString()))
                            {

                                DateTime temp;

                                if (DateTime.TryParse(JoinDate.ToString(), out temp))
                                {
                                    fld.JOIN_DATE = Convert.ToDateTime(JoinDate);
                                }
                                else
                                {
                                    try
                                    {

                                        var month = Convert.ToInt32(JoinDate.ToString().Split("/")[0]);
                                        var date = Convert.ToInt32(JoinDate.ToString().Split("/")[1]);
                                        var year = Convert.ToInt32(JoinDate.ToString().Split("/")[2]);
                                        var newDate = new DateTime(year, month, date);
                                        fld.JOIN_DATE = newDate;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }

                                }
                            }

                        }
                        if (ResignTxt != null)
                        {
                            fld.RESIGN_TXT = ResignTxt.ToString();
                        }
                        if (LamaKerja != null)
                        {
                            fld.LAMA_KERJA = LamaKerja.ToString();
                        }
                        if (Vaksin1.ToString() == "TRUE")
                        {
                            fld.VAKSIN1 = "Y";
                        }
                        else
                        {
                            fld.VAKSIN1 = "N";
                        }
                        if (Vaksin2.ToString() == "TRUE")
                        {
                            fld.VAKSIN2 = "Y";
                        }
                        else
                        {
                            fld.VAKSIN2 = "N";
                        }
                        vaksinList.Add(fld);
                    }
                }
            }
            var successful = new List<string>();
            var failed = new List<string>();
            var sscode = "";
            foreach (var fld in vaksinList.OrderBy(y => y.SS_CODE).ToList())
            {
                sscode = fld.SS_CODE;
                fld.ENTRY_DATE = DateTime.Now;
                fld.UPDATE_DATE = DateTime.Now;
                fld.ENTRY_USER = User.Identity.Name;
                fld.UPDATE_USER = User.Identity.Name;
                var dbEdp = db.xstore_organization.Where(y => y.edp == fld.EDP_CODE).FirstOrDefault();
                if(dbEdp == null)
                {
                    XstoreModel fieldXstore = new XstoreModel();
                    fieldXstore.edp = fld.EDP_CODE;
                    fieldXstore.district = "500";
                    fieldXstore.store_location = "External Add";
                    fieldXstore.area = "External Add";
                    fieldXstore.inactive_flag = "0";
                    db.xstore_organization.Add(fieldXstore);
                    db.SaveChanges();
                }
                var dbs = db.SSTable.Where(y => y.SS_CODE == fld.SS_CODE).FirstOrDefault();
                if (dbs == null)
                {
                    try
                    {
                        db.SSTable.Add(fld);
                        db.SaveChanges();
                        successful.Add(fld.SS_CODE);
                    }
                    catch (Exception ex)
                    {
                        failed.Add(sscode);
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
                    
                }

            }
           
            FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
            if (filedlt.Exists)//check file exsit or not  
            {
                filedlt.Delete();
            }
            return Json(new { SuccessfullyRegistered = successful, FailedToRegister = failed });
        }
    }
}
