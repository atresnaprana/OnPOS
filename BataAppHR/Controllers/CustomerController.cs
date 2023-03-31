﻿using System;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using MimeDetective.Definitions;
using MimeDetective.Diagnostics;
using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Net;
using BataAppHR.Services;

namespace BataAppHR.Controllers
{
    public class CustomerController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<CustomerController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }
        private readonly IMailService mailService;

        public string EmailConfirmationUrl { get; set; }

        public CustomerController(FormDBContext db, ILogger<CustomerController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
            this.mailService = mailService;

        }
        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial,LegalIndustrial")]
        public IActionResult Index()
        {
           
            var data = new List<dbCustomer>();
            data = db.CustomerTbl.Where(y => y.FLAG_AKTIF == "1").OrderBy(y => y.id).ToList();
            return View(data);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {

            return View();
        }
        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial,LegalIndustrial")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbCustomer objCust)
        {
            if (ModelState.IsValid)
            {
                int validate = 0;
                objCust.ENTRY_DATE = DateTime.Now;
                objCust.UPDATE_DATE = DateTime.Now;
                objCust.ENTRY_USER = User.Identity.Name;
                objCust.UPDATE_USER = User.Identity.Name;
                if (objCust.fileKtp != null)
                {
                    objCust.FILE_KTP_NAME = objCust.fileKtp.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileKtp.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_KTP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileAkta != null)
                {
                    objCust.FILE_AKTA_NAME = objCust.fileAkta.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileAkta.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_AKTA = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileRekening != null)
                {
                    objCust.FILE_REKENING_NAME = objCust.fileRekening.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileRekening.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_REKENING = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileNPWP != null)
                {
                    objCust.FILE_NPWP_NAME = objCust.fileNPWP.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileNPWP.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_NPWP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileTdp != null)
                {
                    objCust.FILE_TDP_NAME = objCust.fileTdp.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileTdp.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_TDP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileSIUP != null)
                {
                    objCust.FILE_SIUP_NAME = objCust.fileSIUP.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileSIUP.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_SIUP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileNIB != null)
                {
                    objCust.FILE_NIB_NAME = objCust.fileNIB.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileNIB.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_NIB = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileSPPKP != null)
                {
                    objCust.FILE_SPPKP_NAME = objCust.fileSPPKP.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileSPPKP.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_SPPKP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (objCust.fileSKT != null)
                {
                    objCust.FILE_SKT_NAME = objCust.fileSKT.FileName;
                    using (var ms = new MemoryStream())
                    {
                        objCust.fileSKT.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        objCust.FILE_SKT = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                objCust.FLAG_AKTIF = "1";


                try
                {
                    //var user = new IdentityUser { UserName = objCust.Email.Trim(), Email = objCust.Email.Trim(), EmailConfirmed = true };

                    var user = new IdentityUser { UserName = objCust.Email.Trim(), Email = objCust.Email.Trim() };
                    var validateisexist = _userManager.FindByEmailAsync(objCust.Email.Trim());

                    if (validateisexist.Result == null)
                    {
                        createuser(user, objCust.Password);
                        db.CustomerTbl.Add(objCust);
                        db.SaveChanges();
                        SendVerifyEmail(objCust.Email.Trim());
                        //if (result.Result )
                        //{
                        //    db.CustomerTbl.Add(objCust);
                        //    db.SaveChanges();

                        //}

                    }
                    else
                    {
                        validate = 1;
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
                if(validate == 1)
                {
                    objCust.errmsg = "This account is exist";
                    return View(objCust);
                }
                else
                {
                    return RedirectToAction("Index");

                }
            }
            return View(objCust);
        }
        public async Task<bool> createuser(IdentityUser user, string userpass)
        {
            var success = false;
            var result = await _userManager.CreateAsync(user, userpass);

            if (result.Succeeded)
            {
                success = true;
            }
            return success;
        }
        public async Task<bool> addrole(IdentityUser user)
        {
            var success = false;
            var result1 = await _userManager.AddToRoleAsync(user, "CustomerIndustrial");
            if (result1.Succeeded)
            {
                success = true;
            }
            return success;
        }
        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial,LegalIndustrial")]
        public IActionResult Edit(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}

            dbCustomer fld = db.CustomerTbl.Find(id);
            if(fld.BL_FLAG == "1")
            {
                fld.isBlackList = true;
            }
            else
            {
                fld.isBlackList = false;

            }
            if (fld.isApproved == "1")
            {
                fld.isApproveBool = true;
            }
            else
            {
                fld.isApproveBool = false;
            }
            if (fld.isApproved2 == "1")
            {
                fld.isApproveBool2 = true;
            }
            else
            {
                fld.isApproveBool2 = false;
            }
            if (fld == null)
            {
                return NotFound();
            }

            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind] dbCustomer fld)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                var editFld = db.CustomerTbl.Find(id);
                editFld.CUST_NAME = fld.CUST_NAME;
                editFld.COMPANY = fld.COMPANY;
                editFld.NPWP = fld.NPWP;
                editFld.address = fld.address;
                editFld.city = fld.city;
                editFld.province = fld.province;
                editFld.postal = fld.postal;
                editFld.Email = fld.Email;
                editFld.BANK_NAME = fld.BANK_NAME;
                editFld.BANK_NUMBER = fld.BANK_NUMBER;
                editFld.BANK_BRANCH = fld.BANK_BRANCH;
                editFld.BANK_COUNTRY = fld.BANK_COUNTRY;
                editFld.REG_DATE = fld.REG_DATE;
                editFld.Email = fld.Email;
                editFld.KTP = fld.KTP;
                editFld.PHONE1 = fld.PHONE1;
                editFld.PHONE2 = fld.PHONE2;
                editFld.VA1 = fld.VA1;
                editFld.VA2 = fld.VA2;
                editFld.VA1NOTE = fld.VA1NOTE;
                editFld.VA2NOTE = fld.VA2NOTE;
                editFld.discount_customer = fld.discount_customer;

                //editFld.FLAG_AKTIF = "0";
                if (fld.isBlackList == true)
                {
                    editFld.BL_FLAG = "1";
                }
                else
                {
                    editFld.BL_FLAG = "0";

                }

                if (fld.fileKtp != null)
                {
                    editFld.FILE_KTP_NAME = fld.fileKtp.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileKtp.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_KTP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileAkta != null)
                {
                    editFld.FILE_AKTA_NAME = fld.fileAkta.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileAkta.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_AKTA = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileRekening != null)
                {
                    editFld.FILE_REKENING_NAME = fld.fileRekening.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileRekening.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_REKENING = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileNPWP != null)
                {
                    editFld.FILE_NPWP_NAME = fld.fileNPWP.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileNPWP.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_NPWP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileTdp != null)
                {
                    editFld.FILE_TDP_NAME = fld.fileTdp.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileTdp.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_TDP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileSIUP != null)
                {
                    editFld.FILE_SIUP_NAME = fld.fileSIUP.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileSIUP.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_SIUP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileNIB != null)
                {
                    editFld.FILE_NIB_NAME = fld.fileNIB.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileNIB.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_NIB = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileSPPKP != null)
                {
                    editFld.FILE_SPPKP_NAME = fld.fileSPPKP.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileSPPKP.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_SPPKP = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.fileSKT != null)
                {
                    editFld.FILE_SKT_NAME = fld.fileSKT.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileSKT.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_SKT = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                if (fld.isApproveBool == true)
                {
                    editFld.isApproved = "1";
                }
                else
                {
                    editFld.isApproved = "0";
                }
                if (fld.isApproveBool2 == true)
                {
                    if (editFld.isApproved2 != "1")
                    {
                        editFld.isApproved2 = "1";

                        string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

                        using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
                        {
                            conn.Open();
                            string query = @"update aspnetusers set EmailConfirmed = '1' where Email  = '" + editFld.Email + "'";

                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    //pricedt.articleprice = Convert.ToInt32(reader["price"]);

                                }
                            }
                            conn.Close();
                        }
                        var getuser = _userManager.FindByEmailAsync(editFld.Email.Trim());
                        IdentityUser userdata = getuser.Result;
                        addrole(userdata);
                        PosttoRIMS(editFld.Email.Trim());
                        SendWelcomeMail(editFld.Email.Trim());
                    }
                }
                else
                {
                    editFld.isApproved2 = "0";

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
        public IActionResult DownloadKTP([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();           
            if(custdata != null)
            {
               
            }
            var file = custdata.FILE_KTP;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_KTP_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_KTP_NAME);
        }
        public IActionResult DownloadAkta([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_AKTA;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_AKTA_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_AKTA_NAME);
        }
        public IActionResult DownloadRekening([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_REKENING;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_REKENING_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_REKENING_NAME);
        }
        public IActionResult DownloadNPWP([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_NPWP;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_NPWP_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_NPWP_NAME);
        }
        public IActionResult DownloadTDP([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_TDP;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_TDP_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_TDP_NAME);
        }
        public IActionResult DownloadSIUP([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_SIUP;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_SIUP_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_SIUP_NAME);
        }
        public IActionResult DownloadNIB([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_NIB;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_NIB_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_NIB_NAME);
        }
        public IActionResult DownloadSPPKP([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_SPPKP;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_SPPKP_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_SPPKP_NAME);
        }
        public IActionResult DownloadSKT([FromQuery(Name = "iddata")] string id)
        {
            var custdata = db.CustomerTbl.Where(y => y.id == Convert.ToInt32(id)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_SKT;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_SKT_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_SKT_NAME);
        }
        private string GetContentType(string exts)
        {
            var types = GetMimeTypes();
            var ext = exts.ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {"txt", "text/plain"},
                {"pdf", "application/pdf"},
                {"doc", "application/vnd.ms-word"},
                {"docx", "application/vnd.ms-word"},
                {"xls", "application/vnd.ms-excel"},
                {"xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {"png", "image/png"},
                {"jpg", "image/jpeg"},
                {"jpeg", "image/jpeg"},
                {"gif", "image/gif"},
                {"csv", "text/csv"}
            };
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}
            dbCustomer fld = db.CustomerTbl.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCustomer(int id)
        {
            dbCustomer fld = db.CustomerTbl.Find(id);

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
                    fld.UPDATE_DATE = DateTime.Now;
                    fld.UPDATE_USER = User.Identity.Name;
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
        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            return Json("oke");
        }
        public string PosttoRIMS(string Email)
        {
            var dbCust = db.CustomerTbl.Where(y => y.Email == Email).FirstOrDefault();
            var newrmdt = new RIMSStore();
            int lengthname = 0;
            int miscelllength = 0;
            int addr1length = 0;
            int lengtthmgrname = 0;
            newrmdt.code = "RM07";
            newrmdt.batch = "";
            newrmdt.store = dbCust.EDP;
            newrmdt.seq = 1;
            newrmdt.storetype = "F";
            newrmdt.language = "1";
            newrmdt.storename = dbCust.address;
            newrmdt.manager = dbCust.CUST_NAME;
            newrmdt.addr1 = dbCust.address;
            newrmdt.addr2 = " ";
            newrmdt.miscell = " ";
            newrmdt.week = getwk();
            newrmdt.region = "1";
            newrmdt.dist = "50";
            string datetime = DateTime.Now.ToString("ddMMyyyy");
            newrmdt.opendate = datetime;
            string storename = newrmdt.storetype + newrmdt.language + newrmdt.storename;
            string mgr = newrmdt.manager;
            if (storename.Length > 17)
            {
                lengthname = 17;
            }
            else
            {
                lengthname = storename.Length;
            }
            lengtthmgrname = mgr.Length;
            addr1length = newrmdt.addr1.Length;
            int spare = 0;
            int spareaddr1 = 0;
            int sparemgr = 0;
            if (storename.Length < 17)
            {
                spare = 17 - lengthname;
            }
            else
            {
                if(newrmdt.storename.Length > 15)
                {
                    newrmdt.storename = newrmdt.storename.Substring(0, 15);
                }
                spare = 0;
            }
            lengtthmgrname = lengtthmgrname + spare;
            if (mgr.Length < 15)
            {
                sparemgr = 15 - mgr.Length;

            }
            else
            {
                mgr = mgr.Substring(0, 15);
                lengtthmgrname = mgr.Length+spare;
                //lengtthmgrname = mgr.Length;
                sparemgr = 0;
            }

            miscelllength = 1 + sparemgr;

            if (addr1length < 30)
            {
                spareaddr1 = 30 - addr1length;
            }
            else
            {
                addr1length = 30;
                spareaddr1 = 0;
            }
            addr1length = spareaddr1;
            newrmdt.closedate = "        ";
            string storedata = String.Format("{0,4}{1,5}{2,5}{3,1}{4," + lengthname + "}{5," + lengtthmgrname + "}{6, " + miscelllength + "}{7,2}{8,1}{9,2}{10,8}{11,8}",
                newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq, newrmdt.storetype + newrmdt.language + newrmdt.storename, mgr,
                newrmdt.miscell, newrmdt.week, newrmdt.region, newrmdt.dist, newrmdt.opendate, newrmdt.closedate);

            string storedata2 = String.Format("{0,4}{1,5}{2,5}{3,1}{4," + newrmdt.addr1.Length + "}{5," + addr1length + "}",
                newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq + 1, newrmdt.addr1, " ");
            string storedata3 = String.Format("{0,4}{1,5}{2,5}{3,1}{4,30}",
              newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq + 2, newrmdt.addr2);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CreateStore");
            var filenamerims = "RM_07_" + newrmdt.store + "_" + (DateTime.Now).ToString("ddMMyyyyHHmm") + ".dat";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, filenamerims)))
            {
                string storecompletedata = storedata + "\r\n" + storedata2 + "\r\n" + storedata3;
                outputFile.WriteLine(storecompletedata);
            }

            string link = Configuration.GetConnectionString("LinkFTP");
            string user = Configuration.GetConnectionString("UserFTP");
            string pass = Configuration.GetConnectionString("PassFTP");
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CreateStore");
            var files = Path.Combine(fileUrl, filenamerims);
            var linkftp = "ftp://" + link;
            string relativePath = "/data7/trdataw1";
            Uri serverUri = new Uri(linkftp);
            Uri relativeUri = new Uri(relativePath, UriKind.Relative);
            Uri fullUri = new Uri(serverUri, relativeUri);
            string PureFileName = new FileInfo(files).Name;

            String uploadUrl = String.Format("{0}/{1}/{2}", linkftp, "/data7/trdataw1", PureFileName);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential(user, pass);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Copy the contents of the file to the request stream.  
            StreamReader sourceStream = new StreamReader(files);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            return "Upload File Complete, status " + response.StatusCode;
        }
        public string CreateRimsTest()
        {
            string Email = "dwi.bayu@bata.com";
            var dbCust = db.CustomerTbl.Where(y => y.Email == Email).FirstOrDefault();
            var newrmdt = new RIMSStore();
            int lengthname = 0;
            int miscelllength = 0;
            int addr1length = 0;
            int lengtthmgrname = 0;
            newrmdt.code = "RM07";
            newrmdt.batch = "";
            newrmdt.store = dbCust.EDP;
            newrmdt.seq = 1;
            newrmdt.storetype = "F";
            newrmdt.language = "1";
            newrmdt.storename = dbCust.address;
            newrmdt.manager = dbCust.CUST_NAME;
            newrmdt.addr1 = dbCust.address;
            newrmdt.addr2 = " ";
            newrmdt.miscell = " ";
            newrmdt.week = getwk();
            newrmdt.region = "1";
            newrmdt.dist = "50";
            string datetime = DateTime.Now.ToString("ddMMyyyy");
            newrmdt.opendate = datetime;
            string storename = newrmdt.storetype + newrmdt.language + newrmdt.storename;
            string mgr = newrmdt.manager;
            if(storename.Length > 17)
            {
                lengthname = 17;
            }
            else
            {
                lengthname = storename.Length;
            }
            lengtthmgrname = mgr.Length;
            addr1length = newrmdt.addr1.Length;
            int spare = 0;
            int spareaddr1 = 0;
            int sparemgr = 0;
            if (storename.Length < 17)
            {
                spare = 17 - lengthname;
            }
            else
            {
                if (newrmdt.storename.Length > 15)
                {
                    newrmdt.storename = newrmdt.storename.Substring(0, 15);
                }
                spare = 0;
            }
            lengtthmgrname = lengtthmgrname + spare;
            if (mgr.Length < 15)
            {
                sparemgr = 15 - mgr.Length;

            }
            else
            {
                mgr = mgr.Substring(0, 15);
                lengtthmgrname = mgr.Length + spare;
                //lengtthmgrname = mgr.Length;
                sparemgr = 0;
            }

            miscelllength = 1 + sparemgr;

            if (addr1length < 30)
            {
                spareaddr1 = 30 - addr1length;
            }
            else
            {
                addr1length = 30;
                spareaddr1 = 0;
            }
            addr1length = spareaddr1;
            newrmdt.closedate = "        ";
            string storedata = String.Format("{0,4}{1,5}{2,5}{3,1}{4," + lengthname + "}{5," + lengtthmgrname + "}{6, " + miscelllength + "}{7,2}{8,1}{9,2}{10,8}{11,8}",
                newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq, newrmdt.storetype + newrmdt.language + newrmdt.storename, mgr,
                newrmdt.miscell, newrmdt.week, newrmdt.region, newrmdt.dist, newrmdt.opendate, newrmdt.closedate);

            string storedata2 = String.Format("{0,4}{1,5}{2,5}{3,1}{4," + newrmdt.addr1.Length + "}{5," + addr1length + "}",
                newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq + 1, newrmdt.addr1, " ");
            string storedata3 = String.Format("{0,4}{1,5}{2,5}{3,1}{4,30}",
              newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq + 2, newrmdt.addr2);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CreateStore");
            var filenamerims = "RM_07_" + newrmdt.store + "_" + (DateTime.Now).ToString("ddMMyyyyHHmm") + ".dat";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, filenamerims)))
            {
                string storecompletedata = storedata + "\r\n" + storedata2 + "\r\n" + storedata3;
                outputFile.WriteLine(storecompletedata);
            }

            string link = Configuration.GetConnectionString("LinkFTP");
            string user = Configuration.GetConnectionString("UserFTP");
            string pass = Configuration.GetConnectionString("PassFTP");
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CreateStore");
            var files = Path.Combine(fileUrl, filenamerims);
            var linkftp = "ftp://" + link;
            string relativePath = "/data7/trdataw1";
            Uri serverUri = new Uri(linkftp);
            Uri relativeUri = new Uri(relativePath, UriKind.Relative);
            Uri fullUri = new Uri(serverUri, relativeUri);
            string PureFileName = new FileInfo(files).Name;

            String uploadUrl = String.Format("{0}/{1}/{2}", linkftp, "/data7/trdataw1", PureFileName);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential(user, pass);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Copy the contents of the file to the request stream.  
            StreamReader sourceStream = new StreamReader(files);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            return "Upload File Complete, status " + response.StatusCode;

        }

        public string FtpUplTest()
        {
            string link = Configuration.GetConnectionString("LinkFTP");
            string user = Configuration.GetConnectionString("UserFTP");
            string pass = Configuration.GetConnectionString("PassFTP");
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CreateStore");
            var files = Path.Combine(fileUrl, "RM_07_71001_300520221611.dat");
            var linkftp = "ftp://" + link;
            string relativePath = "/data7/trdataj";
            Uri serverUri = new Uri(linkftp);
            Uri relativeUri = new Uri(relativePath, UriKind.Relative);
            Uri fullUri = new Uri(serverUri, relativeUri);
            string PureFileName = new FileInfo(files).Name;

            String uploadUrl = String.Format("{0}/{1}/{2}", linkftp, "/data7/trdataj", PureFileName);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential(user, pass);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Copy the contents of the file to the request stream.  
            StreamReader sourceStream = new StreamReader(files);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();
            request.ContentLength = fileContents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            return "Upload File Complete, status " + response.StatusDescription;
        }
        public async void SendWelcomeMail(string Email)
        {
            var request = new WelcomeRequest();
            request.UserName = Email;
            request.ToEmail = Email;
           
            //request.ToEmail = Input.Email;
            try
            {
                await mailService.SendWelcomeEmailAsync(request);
                //return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async void SendVerifyEmail(string Email)
        {
            //string Email = "aditya.tresnaprana@bata.com";
            var request = new WelcomeRequest();
            request.UserName = Email;
            request.ToEmail = Email;
            //request.ToEmail = Input.Email;
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot");
            var files = Path.Combine(fileUrl, "FileCodeofConduct.pdf");
            List<IFormFile> fileList = new List<IFormFile>();


            using (var stream = System.IO.File.OpenRead(files))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));


                fileList.Add(file);

                request.Attachments = fileList;
            }
            try
            {
                await mailService.SendVerifyEmailAsync(request);
                //return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int getwk()
        {
            int week = 0;
            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select week(now(), 5)+1 as wk";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        week = Convert.ToInt32(reader["wk"]);

                    }
                }
                conn.Close();
            }
            return week;
        }
        public async void TestSendWelcomeMail()
        {
            string Email = "customerbatatest5@mail.com";
            var request = new WelcomeRequest();
            request.UserName = Email;
            request.ToEmail = Email;
            //request.ToEmail = Input.Email;
            try
            {
                await mailService.SendWelcomeEmailAsync(request);
                //return Ok();
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
        }
        public async void TestSendVerifyEmail()
        {

            string Email = "customerbatatest8@mail.com";
            var request = new WelcomeRequest();
            request.UserName = Email;
            request.ToEmail = Email;
            //request.ToEmail = Input.Email;
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot");
            var files = Path.Combine(fileUrl, "FileCodeofConduct.pdf");
            List<IFormFile> fileList = new List<IFormFile>();


            using (var stream = System.IO.File.OpenRead(files))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));


                fileList.Add(file);

                request.Attachments = fileList;
            }
            try
            {
                await mailService.SendVerifyEmailAsync(request);
                //return Ok();
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
        }
    }
}
