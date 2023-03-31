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
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using OfficeOpenXml;

namespace BataAppHR.Controllers
{
    public class ArticleController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<ArticleController> _logger;
        public IConfiguration Configuration { get; }

        public ArticleController(FormDBContext db, ILogger<ArticleController> logger, IHostingEnvironment _environment, IConfiguration configuration)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;

        }
        [Authorize(Roles = "MerchandiserIndustrial")]
        public IActionResult Index()
        {
            var fieldfront = new articlefront();
            var data = new List<dbArticle>();
            data = db.ArticleTbl.Where(y => y.FLAG_AKTIF == "1").OrderBy(y => y.Article).ToList();
            fieldfront.dataarticle = data;
            return View(fieldfront);
        }
        [HttpGet]
        [Authorize(Roles = "MerchandiserIndustrial")]
        public IActionResult Create()
        {
            var artField = new dbArticle();
            List<Article> list = new List<Article>();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select dz.dmram_article, dz.dmram_project_name from dm_zzram dz ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Article()
                        {
                            articlecode = reader["dmram_article"].ToString(),
                            articlename = reader["dmram_article"].ToString() + " - " + reader["dmram_project_name"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            var test = list;
            artField.listArticle = list;
            return View(artField);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbArticle obj)
        {
            if (ModelState.IsValid)
            {
                obj.ENTRY_DATE = DateTime.Now;
                obj.UPDATE_DATE = DateTime.Now;
                obj.ENTRY_USER = User.Identity.Name;
                obj.UPDATE_USER = User.Identity.Name;
                obj.FLAG_AKTIF = "1";
                try
                {
                    if (obj.fileImg != null)
                    {
                        obj.FILE_IMG_NAME = obj.fileImg.FileName;
                        using (var ms = new MemoryStream())
                        {
                            obj.fileImg.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            obj.FILE_IMG = fileBytes;
                            string s = Convert.ToBase64String(fileBytes);
                            // act on the Base64 data
                        }
                    }
                    db.ArticleTbl.Add(obj);
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
        [Authorize(Roles = "MerchandiserIndustrial")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dbArticle fld = db.ArticleTbl.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                if (fld.FILE_IMG_NAME != null)
                {
                    string imageBase64Data = Convert.ToBase64String(fld.FILE_IMG);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageData = imageDataURL;
                }
                List<Article> list = new List<Article>();

                string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

                using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("select dz.dmram_article, dz.dmram_project_name from dm_zzram dz ", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Article()
                            {
                                articlecode = reader["dmram_article"].ToString(),
                                articlename = reader["dmram_article"].ToString() + " - " + reader["dmram_project_name"].ToString()
                            });
                        }
                    }
                    conn.Close();
                }
                var test = list;
                fld.listArticle = list;
            }
            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int? id, [Bind] dbArticle fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var editFld = db.ArticleTbl.Find(id);
                editFld.Article = fld.Article;
                if (fld.fileImg != null)
                {
                    editFld.FILE_IMG_NAME = fld.fileImg.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileImg.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_IMG = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name;
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


        [Authorize(Roles = "MerchandiserIndustrial")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dbArticle fld = db.ArticleTbl.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteImg(int? id)
        {
            dbArticle fld = db.ArticleTbl.Find(id);
            fld.FLAG_AKTIF = "0";
            fld.UPDATE_DATE = DateTime.Now;
            fld.UPDATE_USER = User.Identity.Name;
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
        public IActionResult UploadArticleAll([Bind] articlefront objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbArticle> articleTbl = new List<dbArticle>();

                var file = objDetail.fileUploadArticle;


                try
                {
                    if (file == null || file.Length == 0)
                    {
                        objDetail.error = "file not found";
                    }
                    else
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsXls");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        var filePath = Path.Combine(path2, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
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
                                dbArticle field = new dbArticle();
                                object article = ws.Cells[row, 1].Value;
                               
                                if (article != null)
                                {
                                    var validatefield = db.ArticleTbl.Where(y => y.Article == article.ToString()).ToList();
                                    if (validatefield.Count() == 0)
                                    {
                                        field.Article = article.ToString();
                                        field.UPDATE_DATE = DateTime.Now;
                                        field.UPDATE_USER = User.Identity.Name;
                                        field.ENTRY_USER = User.Identity.Name;
                                        field.ENTRY_DATE = DateTime.Now;
                                        field.FLAG_AKTIF = "1";
                                        db.ArticleTbl.Add(field);
                                    }
                                    else
                                    {
                                        objDetail.error += "Article Exist:  " + article.ToString() + "";
                                    }

                                    //nilaiSSTbl.Add(field);
                                }
                                else
                                {
                                    //objDetail.error += "missing form id: " + field.SS_CODE + " ";
                                }

                            }


                        }

                        FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
                        if (filedlt.Exists)//check file exsit or not  
                        {
                            filedlt.Delete();
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    objDetail.error = ex.InnerException.ToString();
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
                objDetail.isPassed = true;
                return RedirectToAction("Index", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                objDetail.isPassed = true;
                objDetail.error = "error";
                return RedirectToAction("Index", objDetail);
            }

        }

    }
}
