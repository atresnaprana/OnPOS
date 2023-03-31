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
using OfficeOpenXml;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
namespace BataAppHR.Controllers
{
    public class PaymentController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<PaymentController> _logger;
        public IConfiguration Configuration { get; }

        public PaymentController(FormDBContext db, ILogger<PaymentController> logger, IHostingEnvironment _environment, IConfiguration configuration)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
        }
        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial")]
        public IActionResult Index()
        {
            var data = new List<dbPaymentList>();
            data = db.PaymentTbl.Where(y => y.FLAG_AKTIF == "1").OrderBy(y => y.id).ToList();
            return View(data);
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var fld = new dbPaymentList();
            List<dbSalesOrder> orderlist = new List<dbSalesOrder>();
            orderlist = db.SalesOrderTbl.ToList().Select(y => new dbSalesOrder()
            {
                id = y.id,
                ENTRY_USER = y.id + " - " + y.EMP_CODE
            }).ToList();
            List<dbCustomer> customerlist = new List<dbCustomer>();
            customerlist = db.CustomerTbl.ToList().Select(y => new dbCustomer()
            {
                id = y.id,
                CUST_NAME = y.EDP + " - " + y.CUST_NAME
            }).ToList();
            fld.ddorder = orderlist;
            fld.ddCustomer = customerlist;

            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbPaymentList obj)
        {
            if (ModelState.IsValid)
            {
                obj.ENTRY_DATE = DateTime.Now;
                obj.UPDATE_DATE = DateTime.Now;
                obj.ENTRY_USER = User.Identity.Name;
                obj.UPDATE_USER = User.Identity.Name;
                obj.FLAG_AKTIF = "1";
                if (obj.fileTransaction != null)
                {
                    
                    obj.FILE_PAYMENT_NAME = obj.fileTransaction.FileName;
                    using (var ms = new MemoryStream())
                    {
                        obj.fileTransaction.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        obj.FILE_PAYMENT = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                    }
                    //obj.FILE_PAYMENT = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }
                try
                {
                    db.PaymentTbl.Add(obj);
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
        public IActionResult getdatacustomer(int id_order)
        {
            List<ddCustomer> list = new List<ddCustomer>();

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
	                                d.id as id_order,
	                                d.id_customer,
	                                c.EDP,
	                                c.CUST_NAME
                                from
	                                dbsalesorder d
                                left join dbcustomer c on
	                                d.id_customer = c.id
                                where  d.id = '" + id_order + "'";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ddCustomer()
                        {
                            id_order = Convert.ToInt32(reader["id_order"]),
                            id_customer = Convert.ToInt32(reader["id_customer"]),
                            CUST_NAME = reader["EDP"].ToString() + " - " + reader["CUST_NAME"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return Json(list);
        }
        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dbPaymentList fld = db.PaymentTbl.Find(id);
            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                List<dbSalesOrder> orderlist = new List<dbSalesOrder>();
                orderlist = db.SalesOrderTbl.ToList().Select(y => new dbSalesOrder()
                {
                    id = y.id,
                    ENTRY_USER = y.id + " - " + y.EMP_CODE
                }).ToList();
                List<dbCustomer> customerlist = new List<dbCustomer>();
                customerlist = db.CustomerTbl.ToList().Select(y => new dbCustomer()
                {
                    id = y.id,
                    CUST_NAME = y.EDP + " - " + y.CUST_NAME
                }).ToList();
                fld.ddorder = orderlist;
                fld.ddCustomer = customerlist;
                if (fld.FILE_PAYMENT != null)
                {
                    string imageBase64Data = Convert.ToBase64String(fld.FILE_PAYMENT);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageData = imageDataURL;
                }
            }
            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int? id, [Bind] dbPaymentList fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var editFld = db.PaymentTbl.Find(id);
                editFld.id_order = fld.id_order;
                editFld.id_customer = fld.id_customer;
                editFld.BANK = fld.BANK;
                editFld.REF_ID = fld.REF_ID;

                if (fld.fileTransaction != null)
                {
                    editFld.FILE_PAYMENT_NAME = fld.fileTransaction.FileName;
                    using (var ms = new MemoryStream())
                    {
                        fld.fileTransaction.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        editFld.FILE_PAYMENT = fileBytes;
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                editFld.TOTAL_PAY = fld.TOTAL_PAY;
                editFld.PAYMENT_DATE = fld.PAYMENT_DATE;

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
        [Authorize(Roles = "ContentAdmin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            dbPaymentList fld = db.PaymentTbl.Find(id);
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
            dbPaymentList fld = db.PaymentTbl.Find(id);
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

    }
}
