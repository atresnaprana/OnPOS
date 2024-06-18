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
using OnPOS.Models;
using static System.Net.WebRequestMethods;
using static MimeDetective.Definitions.Default.FileTypes;

namespace OnPOS.Controllers
{
    public class SalesStaffController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<SalesStaffController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }
        private readonly IMailService mailService;

        public string EmailConfirmationUrl { get; set; }

        public SalesStaffController(FormDBContext db, ILogger<SalesStaffController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
            this.mailService = mailService;

        }

        [Authorize(Roles = "CustomerOnPos,StoreOnPos,SuperAdmin")]
        public IActionResult Index()
        {
            if (User.IsInRole("CustomerOnPos"))
            {
                var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                var datastorelist = db.StoreListTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID).ToList();
                List<int> storeid = new List<int>();
                foreach(var fld in datastorelist)
                {
                    storeid.Add(fld.id);
                }
                var datass = db.SalesStaffTbl.Where(y => storeid.Contains(y.STORE_ID)).ToList();
                return View(datass);
            } else if (User.IsInRole("StoreOnPos"))
            {
                var getStorelist = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();
                var datass = db.SalesStaffTbl.Where(y => y.STORE_ID == getStorelist.id).ToList();

                return View(datass);
            }
            else
            {
                return NotFound();
            }
           
        }
        [Authorize(Roles = "CustomerOnPos,StoreOnPos,SuperAdmin")]
        [Authorize]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] dbSalesStaff objCust)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("CustomerOnPos"))
                {
                    var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                    objCust.COMPANY_ID = data.COMPANY_ID;                    
                }
                else if (User.IsInRole("StoreOnPos"))
                {
                    var getStorelist = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();
                    objCust.COMPANY_ID = getStorelist.COMPANY_ID;
                    objCust.STORE_ID = getStorelist.id;
                }
                int validate = 0;
                objCust.ENTRY_DATE = DateTime.Now;
                objCust.UPDATE_DATE = DateTime.Now;
                objCust.ENTRY_USER = User.Identity.Name;
                objCust.UPDATE_USER = User.Identity.Name;
              
                objCust.FLAG_AKTIF = "1";


                try
                {
                    //var user = new IdentityUser { UserName = objCust.Email.Trim(), Email = objCust.Email.Trim(), EmailConfirmed = true };

                    var user = new IdentityUser { UserName = objCust.SALES_EMAIL.Trim(), Email = objCust.SALES_EMAIL.Trim() };
                    var validateisexist = _userManager.FindByEmailAsync(objCust.SALES_EMAIL.Trim());

                    if (validateisexist.Result == null)
                    {
                        await createuser(user, objCust.Password, objCust.SALES_EMAIL.Trim());
                        string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

                        using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
                        {
                            conn.Open();
                            string query = @"update aspnetusers set EmailConfirmed = '1' where Email  = '" + objCust.SALES_EMAIL.Trim() + "'";

                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }

                        db.SalesStaffTbl.Add(objCust);
                        db.SaveChanges();
                        
                        //SendVerifyEmail(objCust.SALES_EMAIL.Trim());
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
                if (validate == 1)
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
        public async Task<bool> createuser(IdentityUser user, string userpass, string mail)
        {
            var success = false;
            var result = await _userManager.CreateAsync(user, userpass);

            if (result.Succeeded)
            {
                success = true;
               
                var getuser = _userManager.FindByEmailAsync(mail);
                IdentityUser userdata = getuser.Result;
                addrole(userdata);
                SendWelcomeMail(mail);
            }
            
           
            return success;
        }
        
        public async Task<bool> addrole(IdentityUser user)
        {
            var success = false;
            var result1 = await _userManager.AddToRoleAsync(user, "SalesStaffOnPOS");
            if (result1.Succeeded)
            {
                success = true;
            }
            return success;
        }
        [Authorize(Roles = "CustomerOnPos,StoreOnPos,SuperAdmin")]
        public IActionResult Edit(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}

            dbSalesStaff fld = db.SalesStaffTbl.Find(id);
            if (fld.SALES_BLACKLIST_FLAG == "1")
            {
                fld.isBlackList = true;
            }
            else
            {
                fld.isBlackList = false;

            }
           
            if (fld == null)
            {
                return NotFound();
            }

            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] dbSalesStaff fld)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                var editFld = db.SalesStaffTbl.Find(id);
                editFld.SALES_NAME = fld.SALES_NAME;
                editFld.STORE_ID = fld.STORE_ID;
              
                editFld.SALES_EMAIL = fld.SALES_EMAIL;
                
                editFld.SALES_REG_DATE = fld.SALES_REG_DATE;
                editFld.SALES_KTP = fld.SALES_KTP;
                editFld.SALES_PHONE = fld.SALES_PHONE;
              

                //editFld.FLAG_AKTIF = "0";
                if (fld.isBlackList == true)
                {
                    editFld.SALES_BLACKLIST_FLAG = "1";
                }
                else
                {
                    editFld.SALES_BLACKLIST_FLAG = "0";

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
            if (custdata != null)
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

        [Authorize(Roles = "CustomerOnPos,StoreOnPos,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            //if (string.IsNullOrEmpty(id))
            //{
            //    return NotFound();
            //}
            dbSalesStaff fld = db.SalesStaffTbl.Find(id);
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
            dbSalesStaff fld = db.SalesStaffTbl.Find(id);

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
