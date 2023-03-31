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
using System.Net;
using System.Text;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Drawing;
using BataAppHR.Services;

namespace BataAppHR.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly FormDBContext db;

        private IHostingEnvironment Environment;
        private readonly ILogger<SalesOrderController> _logger;
        public const string SessionKeyName = "FormList";
        public const string SessionKeyNameEdit = "FormListEdit";
        public const string SessionKeyNameFilter = "TypeCode";
        private readonly IMailService mailService;

        public SalesOrderController(FormDBContext db, ILogger<SalesOrderController> logger, IHostingEnvironment _environment,
            IConfiguration configuration, IMailService mailService)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
            this.mailService = mailService;

        }
        public IConfiguration Configuration { get; }

        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial")]
        public IActionResult Index(SalesFront objPassed)
        {
          
            var frontFld = new SalesFront();
            if (objPassed.isPassed == true)
            {
                if (!string.IsNullOrEmpty(objPassed.error))
                {
                    frontFld.error = objPassed.error;
                }
            }
            var data = db.SalesOrderTbl.Where(y => y.FLAG_AKTIF == "1").ToList().Select(y => new dbSalesOrder()
            {
                ORDER_DATE = y.ORDER_DATE,
                id = y.id,
                EMP_CODE = getEmpName(y.EMP_CODE).NM_EMP,
                TOTAL_ORDER = gettotalordernett(y.id),
                TOTAL_QTY = y.TOTAL_QTY,
                picking_no = y.picking_no,
                EMAIL = y.EMAIL
            }).ToList();
            var typeCode = HttpContext.Session.GetString(SessionKeyNameFilter);
            var dbprograms = db.programDb.ToList();
            frontFld.salesOrdeData = data;

            return View(frontFld);
        }
        public dbEmployee getEmpName(string empcode)
        {
            string empname = "";
            var dbemp = db.EmpTbl.Where(y => y.EMP_CODE == empcode).FirstOrDefault();
            if(dbemp != null)
            {
                empname = dbemp.NM_EMP;
            }
            return dbemp;
        }
        public decimal? gettotalordernett(int id_order)
        {
            decimal? totalorder = 0;
            var dbsalesorder = db.SalesOrderTbl.Find(id_order);
            var dbtotalorder = dbsalesorder.TOTAL_ORDER;
            var custdt = db.CustomerTbl.Find(dbsalesorder.id_customer);
            decimal? getcreditnote = 0;

            if (custdt != null)
            {
                getcreditnote = getcreditnotepdf(custdt.EDP);
            }

            decimal? discountcust = 0;
            if(custdt != null)
            {
                if(custdt.discount_customer != null)
                {
                    discountcust = custdt.discount_customer;
                }
            }
            if(dbtotalorder != null)
            {
                totalorder = dbtotalorder;
                totalorder = totalorder - getcreditnote;
                var discountcustval = (totalorder * discountcust) / 100;
                totalorder = totalorder - discountcustval;
            }
            var isdiscperc = dbsalesorder.IS_DISC_PERC;
            decimal? invdisc = 0;
            if(isdiscperc == "1")
            {
                var invvaldb = dbsalesorder.INV_DISC;
                if(invvaldb != null)
                {
                    invdisc = (totalorder * invvaldb) / 100;
                }
            }
            else
            {
                var invvaldb = dbsalesorder.INV_DISC_AMT;
                if(invvaldb != null)
                {
                    invdisc = invvaldb;
                }
            }
            totalorder = totalorder - invdisc;
            return totalorder;
        }
        public JsonResult getTblDtl()
        {

            //Creating List    
            List<dbSalesOrderDtl> orderDtlTbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
            return Json(orderDtlTbl);
        }
        public JsonResult getPaymentDetail(int idorder)
        {
            //Creating List    
            List<dbPaymentList> orderDtlTbl = db.PaymentTbl.Where(y => y.id_order == idorder).ToList();
            return Json(orderDtlTbl);
        }
        public IActionResult getdataprice(string article)
        {
            ArticleSize pricedt = new ArticleSize();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select dz.dmram_retail_price as price from dm_zzram dz 
                                                    where  dz.dmram_article = '" + article +"'"  ;
                
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pricedt.articleprice = Convert.ToInt32(reader["price"]);
                       
                    }
                }
                conn.Close();
            }
            return Json(pricedt);
        }
        public IActionResult getcustdisc(int custid)
        {
            dbCustomer custdt = new dbCustomer();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");
            var dt = db.CustomerTbl.Find(custid);
           
            return Json(dt);
        }
        public IActionResult getcustedp(int custid)
        {
            dbCustomer custdt = new dbCustomer();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");
            var dt = db.CustomerTbl.Find(custid).EDP;

            return Json(dt);
        }
        public IActionResult getcreditnote(string edp)
        {
            dbCustomer custdt = new dbCustomer();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");
            var dt = db.creditlogtbl.Where(y => y.edp == edp && y.isused == "0").ToList();
            var totalcredit = dt.Sum(y => y.valuecredit);
            return Json(totalcredit);
        }
        public decimal? getcreditnotepdf(string edp)
        {
            dbCustomer custdt = new dbCustomer();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");
            var dt = db.creditlogtbl.Where(y => y.edp == edp && y.isused == "0").ToList();
            var totalcredit = dt.Sum(y => y.valuecredit);
            return totalcredit;
        }
        public IActionResult getsizecode(string article)
        {
            ArticleSize pricedt = new ArticleSize();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
	                                a.dmram_article as article,
	                                a.dmram_size_code as size_code,
	                                dsc.dmsc_size_1 as size1,
	                                dsc.dmsc_size_2 as size2,
	                                dsc.dmsc_size_3 as size3,
	                                dsc.dmsc_size_4 as size4,
	                                dsc.dmsc_size_5 as size5,
	                                dsc.dmsc_size_6 as size6,
	                                dsc.dmsc_size_7 as size7,
	                                dsc.dmsc_size_8 as size8,
	                                dsc.dmsc_size_9 as size9,
	                                dsc.dmsc_size_10 as size10,
	                                dsc.dmsc_size_11 as size11,
	                                dsc.dmsc_size_12 as size12,
	                                dsc.dmsc_size_13 as size13

                                from
	                                dm_zzram a
                                left join dm_size_code dsc on a.dmram_size_code = dsc.dmsc_code 
                                where  a.dmram_article = '" + article + "'";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pricedt.size1 = reader["size1"].ToString();
                        pricedt.size2 = reader["size2"].ToString();
                        pricedt.size3 = reader["size3"].ToString();
                        pricedt.size4 = reader["size4"].ToString();
                        pricedt.size5 = reader["size5"].ToString();
                        pricedt.size6 = reader["size6"].ToString();
                        pricedt.size7 = reader["size7"].ToString();
                        pricedt.size8 = reader["size8"].ToString();
                        pricedt.size9 = reader["size9"].ToString();
                        pricedt.size10 = reader["size10"].ToString();
                        pricedt.size11 = reader["size11"].ToString();
                        pricedt.size12 = reader["size12"].ToString();
                        pricedt.size13 = reader["size13"].ToString();
                        pricedt.sizecode = reader["size_code"].ToString();

                    }
                }
                conn.Close();
            }
            return Json(pricedt);
        }
        public ArticleSize getsizecodePdf(string article)
        {
            ArticleSize pricedt = new ArticleSize();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
	                                a.dmram_article as article,
	                                a.dmram_size_code as size_code,
                                    a.dmram_project_name as project_name,
	                                dsc.dmsc_size_1 as size1,
	                                dsc.dmsc_size_2 as size2,
	                                dsc.dmsc_size_3 as size3,
	                                dsc.dmsc_size_4 as size4,
	                                dsc.dmsc_size_5 as size5,
	                                dsc.dmsc_size_6 as size6,
	                                dsc.dmsc_size_7 as size7,
	                                dsc.dmsc_size_8 as size8,
	                                dsc.dmsc_size_9 as size9,
	                                dsc.dmsc_size_10 as size10,
	                                dsc.dmsc_size_11 as size11,
	                                dsc.dmsc_size_12 as size12,
	                                dsc.dmsc_size_13 as size13

                                from
	                                dm_zzram a
                                left join dm_size_code dsc on a.dmram_size_code = dsc.dmsc_code 
                                where  a.dmram_article = '" + article + "'";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pricedt.size1 = reader["size1"].ToString();
                        pricedt.size2 = reader["size2"].ToString();
                        pricedt.size3 = reader["size3"].ToString();
                        pricedt.size4 = reader["size4"].ToString();
                        pricedt.size5 = reader["size5"].ToString();
                        pricedt.size6 = reader["size6"].ToString();
                        pricedt.size7 = reader["size7"].ToString();
                        pricedt.size8 = reader["size8"].ToString();
                        pricedt.size9 = reader["size9"].ToString();
                        pricedt.size10 = reader["size10"].ToString();
                        pricedt.size11 = reader["size11"].ToString();
                        pricedt.size12 = reader["size12"].ToString();
                        pricedt.size13 = reader["size13"].ToString();
                        pricedt.sizecode = reader["size_code"].ToString();
                        pricedt.projectname = reader["project_name"].ToString();
                    }
                }
                conn.Close();
            }
            return pricedt;
        }

        public IActionResult getdatatotalpaid(int orderID)
        {
            decimal? paid = Convert.ToDecimal(0);
            var dataPayment = db.PaymentTbl.Where(y => y.id_order == orderID).ToList();
            paid = dataPayment.Sum(y => y.TOTAL_PAY);
            return Json(paid);
        }
        public int getdatapriceUpl(string article)
        {
            ArticleSize pricedt = new ArticleSize();
            int price = 0; 
            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select dz.dmram_retail_price as price from dm_zzram dz 
                                                    where  dz.dmram_article = '" + article + "'";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        price = Convert.ToInt32(reader["price"]);

                    }
                }
                conn.Close();
            }
            return price;
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

        [HttpGet]
        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial")]
        public IActionResult Create(dbSalesOrder objPassed)
        {
          
            List<dbEmployee> empList = new List<dbEmployee>();
            empList = db.EmpTbl.ToList().Select(y => new dbEmployee()
            {
                EMP_CODE = y.EMP_CODE,
                NM_EMP = y.NM_EMP
            }).ToList();

            dbSalesOrder fld = new dbSalesOrder();
            fld.TOTAL_ORDER = objPassed.TOTAL_ORDER;
            fld.TOTAL_QTY = objPassed.TOTAL_QTY;
            fld.STATUS = objPassed.STATUS;
            fld.APPROVAL_1 = objPassed.APPROVAL_1;
            fld.APPROVAL_2 = objPassed.APPROVAL_2;
            fld.ORDER_DATE = objPassed.ORDER_DATE;
            if (fld.ORDER_DATE != null)
            {
                fld.passingDate = fld.ORDER_DATE.Value.ToString("MM/dd/yyyy HH:mm:ss");
            }
            fld.INV_DISC = objPassed.INV_DISC;
            fld.EMP_CODE = objPassed.EMP_CODE;
            fld.id_customer = objPassed.id_customer;
            fld.INV_DISC_AMT = objPassed.INV_DISC_AMT;
            fld.IS_DISC_PERC = objPassed.IS_DISC_PERC;
            fld.custdisc = objPassed.custdisc;
            fld.SHIPPING_ADDRESS = objPassed.SHIPPING_ADDRESS;
            fld.error = objPassed.error;
            if(fld.IS_DISC_PERC == "1")
            {
                fld.IS_DISC_PERCBoolHdr = true;
            }
            else
            {
                fld.IS_DISC_PERCBoolHdr = false;

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
            fld.articleList = list;
            fld.sizeList = new List<ArticleSize>();

            //if (fld.Date != null)
            //{
            //    fld.passingDate = fld.Date.Value.ToString("MM/dd/yyyy");
            //}

            List<dbSalesOrderDtl> p = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
            if (p == null || !objPassed.isPassed)
            {
                p = new List<dbSalesOrderDtl>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", p);
            }
            List<dbEmployee> EmpList = new List<dbEmployee>();
            EmpList = db.EmpTbl.Where(y => y.IS_SALES_WH == "1").ToList().Select(y => new dbEmployee()
            {
                EMP_CODE = y.EMP_CODE,
                NM_EMP = y.EMP_CODE + " - " + y.NM_EMP 
            }).ToList();
            fld.EmpDD = EmpList;

            List<dbCustomer> custList = new List<dbCustomer>();
            custList = db.CustomerTbl.ToList().Select(y => new dbCustomer()
            {
                id = y.id,
                CUST_NAME = y.EDP + " - " + y.CUST_NAME
            }).ToList();
            fld.custDD = custList;
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDetail([Bind] dbSalesOrder objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbSalesOrderDtl> orderdtltbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
                if (orderdtltbl == null)
                {
                    orderdtltbl = new List<dbSalesOrderDtl>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderdtltbl);
                }
                dbSalesOrderDtl fld = new dbSalesOrderDtl();
                fld.article = objDetail.article;
                fld.size = objDetail.size;
                fld.qty = objDetail.qty;
                fld.price = objDetail.price;
                
                fld.Size_1 = objDetail.Size_1;
                fld.Size_2 = objDetail.Size_2;
                fld.Size_3 = objDetail.Size_3;
                fld.Size_4 = objDetail.Size_4;
                fld.Size_5 = objDetail.Size_5;
                fld.Size_6 = objDetail.Size_6;
                fld.Size_7 = objDetail.Size_7;
                fld.Size_8 = objDetail.Size_8;
                fld.Size_9 = objDetail.Size_9;
                fld.Size_10 = objDetail.Size_10;
                fld.Size_11 = objDetail.Size_11;
                fld.Size_12 = objDetail.Size_12;
                fld.Size_13 = objDetail.Size_13;
                var totalorder = fld.price * fld.qty;
                var finalval = totalorder;
                fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);

                //if (objDetail.is_disc_percBool)
                //{
                //    fld.is_disc_perc = "1";
                //    fld.disc = objDetail.disc;
                //    fld.disc_amt = 0;
                //    var totalorder = fld.price * fld.qty;
                //    var valdisc = (totalorder * fld.disc) / 100;
                //    var finalval = totalorder - valdisc;
                //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                //}
                //else
                //{
                //    fld.is_disc_perc = "0";
                //    fld.disc = 0;
                //    fld.disc_amt = objDetail.disc_amt;
                //    var totalorder = fld.price * fld.qty;
                //    var valdisc = fld.disc_amt;
                //    var finalval = totalorder - valdisc;
                //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                //}

                try
                {
                    orderdtltbl.Add(fld);
                   
                    objDetail.EMP_CODE = objDetail.EMP_CODETemp;
                    objDetail.id_customer = objDetail.id_customerTemp;
                    objDetail.ORDER_DATE = objDetail.ORDER_DATETemp;
                    objDetail.APPROVAL_1 = objDetail.APPROVAL_1Temp;
                    objDetail.APPROVAL_2 = objDetail.APPROVAL_2Temp;
                    objDetail.TOTAL_ORDER = orderdtltbl.Sum(y => y.final_price);
                    objDetail.TOTAL_QTY = orderdtltbl.Sum(y => y.qty);
                    objDetail.INV_DISC = objDetail.INV_DISCTemp;
                    objDetail.INV_DISC_AMT = objDetail.INV_DISC_AMTTemp;
                    objDetail.IS_DISC_PERC = objDetail.IS_DISC_PERCTemp;
                    objDetail.custdisc = objDetail.custdiscTemp;
                    objDetail.SHIPPING_ADDRESS = objDetail.SHIPPING_ADDRESSTemp;
                    objDetail.isPassed = true;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderdtltbl);

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
                return RedirectToAction("Create", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Create", objDetail);
            }
        }
        public JsonResult getOrderDataEdit(string article)
        {

            //Creating List    
            List<dbSalesOrderDtl> salesorderDtl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
            dbSalesOrderDtl fld = new dbSalesOrderDtl();
            if (salesorderDtl != null)
            {
                fld = salesorderDtl.Where(y => y.article == article).FirstOrDefault();
            }
            return Json(fld);
        }
        public JsonResult validatearticle(string article,string btnid)
        {
            //Creating List    
            List<dbSalesOrderDtl> salesorderDtl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
            string stat = "pass";
            if (btnid != "addArticle")
            {
                ////nilaiSSTbl = nilaiSSTbl.Where(y => y.SS_CODE != sscode).ToList();
                //var fld = nilaiSSTbl.Where(y => y.SS_CODE == sscode).FirstOrDefault();
                //if(fld != null)
                //{

                //}
                //var validate = nilaiSSTbl.Where(y => y.SS_CODE == sscode).ToList();
                //if (validate.Count() > 0)
                //{
                //    stat = "nopass";
                //}
            }
            else
            {
                var validate = salesorderDtl.Where(y => y.article == article).ToList();
                if (validate.Count() > 0)
                {
                    stat = "nopass";
                }
            }

            return Json(stat);
        }
        public JsonResult validateqty(int qty)
        {
            //Creating List
            var datavalidate = db.OrderConfigTbl.OrderByDescending(y => y.id).FirstOrDefault();
            string stat = "pass";
            if (qty < datavalidate.pairsarticle)
            {
                stat = "nopass";
            }

            return Json(stat);
        }
        public JsonResult validateqtyShipping(int qty)
        {
            //Creating List
            var datavalidate = db.OrderConfigTbl.OrderByDescending(y => y.id).FirstOrDefault();
            string stat = "pass";
            if (qty < datavalidate.pairsorder)
            {
                stat = "nopass";
            }

            return Json(stat);
        }

        public string validateqtyupl(int qty)
        {
            //Creating List
            var datavalidate = db.OrderConfigTbl.OrderByDescending(y => y.id).FirstOrDefault();
            string stat = "pass";
            if (qty < datavalidate.pairsarticle)
            {
                stat = "nopass";
            }

            return stat;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDetail([Bind] dbSalesOrder objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbSalesOrderDtl> orderdata = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
                var splitdatamode = objDetail.datamode.Split("EditArticle");
                if (splitdatamode.Length > 1)
                {
                    foreach (var fld in orderdata)
                    {
                       
                        if (fld.article == objDetail.articleEdit)
                        {
                            //dbNilaiSSFixed fld = nilaiSSTbl.Where(y => y.SS_CODE == objDetail.SS_CODE_EDIT).FirstOrDefault();
                            fld.article = objDetail.articleEdit;
                            fld.size = objDetail.sizeEdit;
                            fld.qty = objDetail.qtyEdit;
                            fld.price = objDetail.priceEdit;
                            //fld.disc = objDetail.discEdit;
                            fld.Size_1 = objDetail.Size_1_Edit;
                            fld.Size_2 = objDetail.Size_2_Edit;
                            fld.Size_3 = objDetail.Size_3_Edit;
                            fld.Size_4 = objDetail.Size_4_Edit;
                            fld.Size_5 = objDetail.Size_5_Edit;
                            fld.Size_6 = objDetail.Size_6_Edit;
                            fld.Size_7 = objDetail.Size_7_Edit;
                            fld.Size_8 = objDetail.Size_8_Edit;
                            fld.Size_9 = objDetail.Size_9_Edit;
                            fld.Size_10 = objDetail.Size_10_Edit;
                            fld.Size_11 = objDetail.Size_11_Edit;
                            fld.Size_12 = objDetail.Size_12_Edit;
                            fld.Size_13 = objDetail.Size_13_Edit;
                            var totalorder = fld.price * fld.qty;
                            var finalval = totalorder;
                            fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);

                            //if (objDetail.is_disc_percBoolEdit)
                            //{
                            //    fld.is_disc_perc = "1";
                            //    fld.disc = objDetail.discEdit;
                            //    fld.disc_amt = 0;
                            //    var totalorder = fld.price * fld.qty;
                            //    var valdisc = (totalorder * fld.disc) / 100;
                            //    var finalval = totalorder - valdisc;
                            //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                            //}
                            //else
                            //{
                            //    fld.is_disc_perc = "0";
                            //    fld.disc = 0;
                            //    fld.disc_amt = objDetail.disc_amtEdit;
                            //    var totalorder = fld.price * fld.qty;
                            //    var valdisc = fld.disc_amt;
                            //    var finalval = totalorder - valdisc;
                            //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                            //}
                            //    var totalorder = fld.price * fld.qty;
                            //    var valdisc = (totalorder * fld.disc) / 100;
                            //    var finalval = totalorder - valdisc;
                            //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                        }

                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderdata);

                }
                else
                {
                    //List<dbSalesOrderDtl> orderdata = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
                    var fld = orderdata.Where(y => y.article == objDetail.articleEdit).FirstOrDefault();
                    orderdata.Remove(fld);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderdata);

                }

                objDetail.EMP_CODE = objDetail.EMP_CODETempEdit;
                objDetail.id_customer = objDetail.id_customerTempEdit;
                objDetail.ORDER_DATE = objDetail.ORDER_DATETempEdit;
                objDetail.APPROVAL_1 = objDetail.APPROVAL_1TempEdit;
                objDetail.APPROVAL_2 = objDetail.APPROVAL_2TempEdit;
                objDetail.TOTAL_ORDER = orderdata.Sum(y => y.final_price);
                objDetail.TOTAL_QTY = orderdata.Sum(y => y.qty);
                objDetail.INV_DISC = objDetail.INV_DISCTempEdit;
                objDetail.INV_DISC_AMT = objDetail.INV_DISC_AMTTempEdit;
                objDetail.IS_DISC_PERC = objDetail.IS_DISC_PERCTempEdit;
                objDetail.custdisc = objDetail.custdiscTempEdit;
                objDetail.SHIPPING_ADDRESS = objDetail.SHIPPING_ADDRESSTempEdit;
                objDetail.isPassed = true;
                return RedirectToAction("Create", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Create", objDetail);
            }
        }
        public IActionResult UploadArticle([Bind] dbSalesOrder objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbSalesOrderDtl> orderDtlTbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
                if (orderDtlTbl == null)
                {
                    orderDtlTbl = new List<dbSalesOrderDtl>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderDtlTbl);
                }
                var file = objDetail.fileuploadArticle;
                string error = "";

                try
                {
                    if (file == null || file.Length == 0)
                    {
                        error += "file not found " + "\n";
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
                                dbSalesOrderDtl field = new dbSalesOrderDtl();
                                object article = ws.Cells[row, 1].Value;
                                object size1 = ws.Cells[row, 2].Value;
                                object size2 = ws.Cells[row, 3].Value;
                                object size3 = ws.Cells[row, 4].Value;
                                object size4 = ws.Cells[row, 5].Value;
                                object size5 = ws.Cells[row, 6].Value;
                                object size6 = ws.Cells[row, 7].Value;
                                object size7 = ws.Cells[row, 8].Value;
                                object size8 = ws.Cells[row, 9].Value;
                                object size9 = ws.Cells[row, 10].Value;
                                object size10 = ws.Cells[row, 11].Value;
                                object size11 = ws.Cells[row, 12].Value;
                                object size12 = ws.Cells[row, 13].Value;
                                object size13 = ws.Cells[row, 14].Value;
                                //object disctype = ws.Cells[row, 15].Value;
                                //object discperc = ws.Cells[row, 16].Value;
                                //object discamt = ws.Cells[row, 17].Value;

                                var articlesizecode = new ArticleSize();
                                if (article != null)
                                {
                                    field.article = article.ToString();
                                    articlesizecode = getsizecodePdf(article.ToString());
                                }
                                if (size1 != null)
                                {
                                    field.Size_1 = Convert.ToInt32(size1);
                                    if (!string.IsNullOrEmpty(articlesizecode.size1))
                                    {
                                        var code = articlesizecode.size1.Substring(1);
                                        if(articlesizecode.size1.Length > 2)
                                        {
                                            code = articlesizecode.size1.Substring(2);
                                        }
                                        if(code == "5")
                                        {
                                            field.Size_1 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_1 = 0;
                                }
                                if (size2 != null)
                                {
                                    field.Size_2 = Convert.ToInt32(size2);
                                    if (!string.IsNullOrEmpty(articlesizecode.size2))
                                    {
                                        var code = articlesizecode.size2.Substring(1);
                                        if (articlesizecode.size2.Length > 2)
                                        {
                                            code = articlesizecode.size2.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_2 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_2 = 0;
                                }
                                if (size3 != null)
                                {
                                    field.Size_3 = Convert.ToInt32(size3);
                                    if (!string.IsNullOrEmpty(articlesizecode.size3))
                                    {
                                        var code = articlesizecode.size3.Substring(1);
                                        if (articlesizecode.size3.Length > 2)
                                        {
                                            code = articlesizecode.size3.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_3 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_3 = 0;
                                }
                                if (size4 != null)
                                {
                                    field.Size_4 = Convert.ToInt32(size4);
                                    if (!string.IsNullOrEmpty(articlesizecode.size4))
                                    {
                                        var code = articlesizecode.size4.Substring(1);
                                        if (articlesizecode.size4.Length > 2)
                                        {
                                            code = articlesizecode.size4.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_4 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_4 = 0;
                                }
                                if (size5 != null)
                                {
                                    field.Size_5 = Convert.ToInt32(size5);
                                    if (!string.IsNullOrEmpty(articlesizecode.size5))
                                    {
                                        var code = articlesizecode.size5.Substring(1);
                                        if (articlesizecode.size5.Length > 2)
                                        {
                                            code = articlesizecode.size5.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_5 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_5 = 0;
                                }
                                if (size6 != null)
                                {
                                    field.Size_6 = Convert.ToInt32(size6);
                                    if (!string.IsNullOrEmpty(articlesizecode.size6))
                                    {
                                        var code = articlesizecode.size6.Substring(1);
                                        if (articlesizecode.size6.Length > 2)
                                        {
                                            code = articlesizecode.size6.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_6 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_6 = 0;
                                }
                                if (size7 != null)
                                {
                                    field.Size_7 = Convert.ToInt32(size7);
                                    if (!string.IsNullOrEmpty(articlesizecode.size7))
                                    {
                                        var code = articlesizecode.size7.Substring(1);
                                        if (articlesizecode.size7.Length > 2)
                                        {
                                            code = articlesizecode.size7.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_7 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_7 = 0;
                                }
                                if (size8 != null)
                                {
                                    field.Size_8 = Convert.ToInt32(size8);
                                    if (!string.IsNullOrEmpty(articlesizecode.size8))
                                    {
                                        var code = articlesizecode.size8.Substring(1);
                                        if (articlesizecode.size8.Length > 2)
                                        {
                                            code = articlesizecode.size8.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_8 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_8 = 0;
                                }
                                if (size9 != null)
                                {
                                    field.Size_9 = Convert.ToInt32(size9);
                                    if (!string.IsNullOrEmpty(articlesizecode.size9))
                                    {
                                        var code = articlesizecode.size9.Substring(1);
                                        if (articlesizecode.size9.Length > 2)
                                        {
                                            code = articlesizecode.size9.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_9 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_9 = 0;
                                }
                                if (size10 != null)
                                {
                                    field.Size_10 = Convert.ToInt32(size10);
                                    if (!string.IsNullOrEmpty(articlesizecode.size10))
                                    {
                                        var code = articlesizecode.size10.Substring(1);
                                        if (articlesizecode.size10.Length > 2)
                                        {
                                            code = articlesizecode.size10.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_10 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_10 = 0;
                                }
                                if (size11 != null)
                                {
                                    field.Size_11 = Convert.ToInt32(size11);
                                    if (!string.IsNullOrEmpty(articlesizecode.size11))
                                    {
                                        var code = articlesizecode.size11.Substring(1);
                                        if (articlesizecode.size11.Length > 2)
                                        {
                                            code = articlesizecode.size11.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_11 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_11 = 0;
                                }
                                if (size12 != null)
                                {
                                    field.Size_12 = Convert.ToInt32(size12);
                                    if (!string.IsNullOrEmpty(articlesizecode.size12))
                                    {
                                        var code = articlesizecode.size12.Substring(1);
                                        if (articlesizecode.size12.Length > 2)
                                        {
                                            code = articlesizecode.size12.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_12 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_12 = 0;
                                }
                                if (size13 != null)
                                {
                                    field.Size_13 = Convert.ToInt32(size13);
                                    if (!string.IsNullOrEmpty(articlesizecode.size13))
                                    {
                                        var code = articlesizecode.size13.Substring(1);
                                        if (articlesizecode.size13.Length > 2)
                                        {
                                            code = articlesizecode.size13.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_13 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_13 = 0;
                                }
                               
                                field.qty = field.Size_1 + field.Size_2 + field.Size_3 + field.Size_4 + field.Size_5 + field.Size_6 + field.Size_7 + field.Size_8 + field.Size_9 + field.Size_10 + field.Size_11 + field.Size_12 + field.Size_13;
                                if (article != null)
                                {
                                    var price = getdatapriceUpl(field.article);
                                    field.price = price;
                                    var totalorder = Convert.ToDecimal(price * field.qty);
                                    field.final_price = Math.Round(totalorder, 2);
                                    //if(disctype != null)
                                    //{
                                    //    var disctypestr = disctype.ToString();
                                    //    if (!String.IsNullOrEmpty(disctypestr))
                                    //    {
                                    //        if(disctypestr == "1")
                                    //        {
                                    //            field.is_disc_perc = "1";
                                    //            field.disc_amt = 0;
                                    //            if (discperc != null)
                                    //            {
                                    //                field.disc = Convert.ToDecimal(discperc);
                                    //                var totaldisc = ((price * field.qty) * field.disc) / 100;
                                    //                totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //            }
                                    //            field.final_price = Math.Round(totalorder, 2);
                                    //        }
                                    //        else
                                    //        {
                                    //            field.is_disc_perc = "0";
                                    //            field.disc = 0;
                                    //            if (discamt != null)
                                    //            {
                                    //                field.disc_amt = Convert.ToDecimal(discamt);
                                    //                var totaldisc = field.disc_amt;
                                    //                totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //            }
                                    //            field.final_price = Math.Round(totalorder, 2);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        field.is_disc_perc = "0";
                                    //        field.disc = 0;
                                    //        if (discamt != null)
                                    //        {
                                    //            field.disc_amt = Convert.ToDecimal(discamt);
                                    //            var totaldisc = field.disc_amt;
                                    //            totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //        }
                                    //        field.final_price = Math.Round(totalorder, 2);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    field.is_disc_perc = "0";
                                    //    field.disc = 0;
                                    //    if (discamt != null)
                                    //    {
                                    //        field.disc_amt = Convert.ToDecimal(discamt);
                                    //        var totaldisc = field.disc_amt;
                                    //        totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //    }
                                    //    field.final_price = Math.Round(totalorder, 2);
                                    //}

                                    var validate = orderDtlTbl.Where(y => y.article == article.ToString()).ToList();
                                    if(validate.Count() == 0)
                                    {
                                        var validateqtydata = validateqtyupl(field.qty);
                                        if(validateqtydata == "pass")
                                        {
                                            orderDtlTbl.Add(field);
                                        }
                                        else
                                        {
                                            error += "article: " + article.ToString() + " Does not meet minimum qty " + "\n";
                                        }

                                    }
                                    else
                                    {
                                        foreach(var flds in validate)
                                        {
                                            flds.Size_1 = field.Size_1;
                                            flds.Size_2 = field.Size_2;
                                            flds.Size_3 = field.Size_3;
                                            flds.Size_4 = field.Size_4;
                                            flds.Size_5 = field.Size_5;
                                            flds.Size_6 = field.Size_6;
                                            flds.Size_7 = field.Size_7;
                                            flds.Size_8 = field.Size_8;
                                            flds.Size_9 = field.Size_9;
                                            flds.Size_10 = field.Size_10;
                                            flds.Size_11 = field.Size_11;
                                            flds.Size_12 = field.Size_12;
                                            flds.Size_13 = field.Size_13;
                                            flds.qty = field.qty;
                                            flds.price = field.price;
                                            flds.final_price = field.final_price;

                                        }
                                    }
                                }

                            }


                        }

                        FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
                        if (filedlt.Exists)//check file exsit or not  
                        {
                            filedlt.Delete();
                        }
                    }
                    objDetail.EMP_CODE = objDetail.EMP_CODETempUpl;
                    objDetail.id_customer = objDetail.id_customerTempUpl;
                    objDetail.ORDER_DATE = objDetail.ORDER_DATETempUpl;
                    objDetail.APPROVAL_1 = objDetail.APPROVAL_1TempUpl;
                    objDetail.APPROVAL_2 = objDetail.APPROVAL_2TempUpl;
                    objDetail.TOTAL_ORDER = objDetail.TOTAL_ORDERTempUpl;
                    objDetail.TOTAL_QTY = objDetail.TOTAL_QTYTempUpl;
                    objDetail.INV_DISC = objDetail.INV_DISCTempUpl;
                    objDetail.INV_DISC_AMT = objDetail.INV_DISC_AMTTempUpl;
                    objDetail.IS_DISC_PERC = objDetail.IS_DISC_PERCTempUpl;
                    objDetail.custdisc = objDetail.custdiscTempUpl;
                    objDetail.isPassed = true;
                    objDetail.SHIPPING_ADDRESS = objDetail.SHIPPING_ADDRESSTempUpl;
                    objDetail.error = error;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderDtlTbl);
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
                return RedirectToAction("Create", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Create", objDetail);
            }

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Creates([Bind] dbSalesOrder objSales)
        {
            if (ModelState.IsValid)
            {

                objSales.ENTRY_DATE = DateTime.Now;
                objSales.UPDATE_DATE = DateTime.Now;
                objSales.ENTRY_USER = User.Identity.Name;
                objSales.UPDATE_USER = User.Identity.Name;
                var dbcust = db.CustomerTbl.Find(objSales.id_customer);
                if(dbcust != null)
                {
                    objSales.EMAIL = dbcust.Email;
                }
                objSales.FLAG_AKTIF = "1";
                if (objSales.isApprove1)
                {
                    objSales.APPROVAL_1 = "1";
                }
                else
                {
                    objSales.APPROVAL_1 = "0";

                }
                if (objSales.isApprove2)
                {
                    objSales.APPROVAL_2 = "1";

                }
                else
                {
                    objSales.APPROVAL_2 = "0";

                }
                if (objSales.IS_DISC_PERCBoolHdr)
                {
                    objSales.IS_DISC_PERC = "1";
                }
                else
                {
                    objSales.IS_DISC_PERC = "0";

                }
                try
                {
                    using (var context = db)
                    {

                        context.SalesOrderTbl.Add(objSales);
                        context.SaveChanges();
                        int id = objSales.id; // Yes it's here
                    }
                    var ids = objSales.id;
                    //var trnid = db.rekapdbFixed.Find(ids);
                    List<dbSalesOrderDtl> orderTbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormList");
                    if (orderTbl != null)
                    {
                        
                        foreach (var fld in orderTbl)
                        {
                            dbSalesOrderDtl form = fld;
                            form.id_order = ids;
                         
                            db.SalesOrderDtlTbl.Add(form);
                        }
                      
                     
                        db.SaveChanges();
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
            return View(objSales);
        }
        [Authorize(Roles = "SalesIndustrial,FinanceIndustrial")]
        public IActionResult Edit(int id, dbSalesOrder objPassed)
        {

            List<dbEmployee> empList = new List<dbEmployee>();
            empList = db.EmpTbl.ToList().Select(y => new dbEmployee()
            {
                EMP_CODE = y.EMP_CODE,
                NM_EMP = y.NM_EMP
            }).ToList();

           
            if (objPassed.isPassed)
            {
                id = objPassed.id;
            }
            dbSalesOrder fld = db.SalesOrderTbl.Find(id);
            decimal? custdisc = 0;
            var datacust = db.CustomerTbl.Find(fld.id_customer);
            if(datacust != null)
            {
                custdisc = datacust.discount_customer;
            }
            fld.custdisc = custdisc;
            List<dbSalesOrderDtl> p = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
            if (p == null)
            {
                p = db.SalesOrderDtlTbl.Where(y => y.id_order == fld.id).ToList();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", p);
            }
            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                if (objPassed.isPassed)
                {
                    fld.TOTAL_ORDER = objPassed.TOTAL_ORDER;
                    fld.TOTAL_QTY = objPassed.TOTAL_QTY;
                    fld.STATUS = objPassed.STATUS;
                    fld.APPROVAL_1 = objPassed.APPROVAL_1;
                    fld.APPROVAL_2 = objPassed.APPROVAL_2;
                    fld.IS_DISC_PERC = objPassed.IS_DISC_PERC;

                    fld.ORDER_DATE = objPassed.ORDER_DATE;
                    fld.INV_DISC = objPassed.INV_DISC;
                    fld.custdisc = objPassed.custdisc;
                    if (fld.ORDER_DATE != null)
                    {
                        fld.passingDate = fld.ORDER_DATE.Value.ToString("MM/dd/yyyy HH:mm:ss");
                    }
                    fld.EMP_CODE = objPassed.EMP_CODE;
                    fld.SHIPPING_ADDRESS = objPassed.SHIPPING_ADDRESS;
                    fld.id_customer = objPassed.id_customer;
                    fld.error = objPassed.error;
                }
                else
                {
                    fld.error = ""; 
                    p = db.SalesOrderDtlTbl.Where(y => y.id_order == fld.id).ToList();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", p);
                }
                if (fld.ORDER_DATE != null)
                {
                    fld.passingDate = fld.ORDER_DATE.Value.ToString("MM/dd/yyyy HH:mm:ss");
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
                if (fld.APPROVAL_1 == "1")
                {
                    fld.isApprove1 = true;
                }
                else
                {
                    fld.isApprove1 = false;

                }
                if (fld.APPROVAL_2 == "1")
                {
                    fld.isApprove2 = true;

                }
                else
                {
                    fld.isApprove2 = false;

                }
                if(fld.IS_DISC_PERC == "1")
                {
                    fld.IS_DISC_PERCBoolHdr = true;
                }
                else
                {
                    fld.IS_DISC_PERCBoolHdr = false;
                }
                var test = list;
                fld.articleList = list;
                fld.sizeList = new List<ArticleSize>();
                List<dbEmployee> EmpList = new List<dbEmployee>();
                EmpList = db.EmpTbl.Where(y => y.IS_SALES_WH == "1").ToList().Select(y => new dbEmployee()
                {
                    EMP_CODE = y.EMP_CODE,
                    NM_EMP = y.EMP_CODE + " - " + y.NM_EMP
                }).ToList();
                fld.EmpDD = EmpList;

                List<dbCustomer> custList = new List<dbCustomer>();
                custList = db.CustomerTbl.ToList().Select(y => new dbCustomer()
                {
                    id = y.id,
                    CUST_NAME = y.EDP + " - " + y.CUST_NAME
                }).ToList();
                fld.custDD = custList;
            }
            return View(fld);
        }
        public JsonResult getTblDtlEdit()
        {

            //Creating List    
            List<dbSalesOrderDtl> ordertbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
            return Json(ordertbl);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDetailEdit([Bind] dbSalesOrder objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbSalesOrderDtl> orderdtltbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
                if (orderdtltbl == null)
                {
                    orderdtltbl = new List<dbSalesOrderDtl>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", orderdtltbl);
                }
                dbSalesOrderDtl fld = new dbSalesOrderDtl();
                fld.article = objDetail.article;
                fld.Size_1 = objDetail.Size_1;
                fld.Size_2 = objDetail.Size_2;
                fld.Size_3 = objDetail.Size_3;
                fld.Size_4 = objDetail.Size_4;
                fld.Size_5 = objDetail.Size_5;
                fld.Size_6 = objDetail.Size_6;
                fld.Size_7 = objDetail.Size_7;
                fld.Size_8 = objDetail.Size_8;
                fld.Size_9 = objDetail.Size_9;
                fld.Size_10 = objDetail.Size_10;
                fld.Size_11 = objDetail.Size_11;
                fld.Size_12 = objDetail.Size_12;
                fld.Size_13 = objDetail.Size_13;

                fld.qty = objDetail.qty;
                fld.price = objDetail.price;
                var totalorder = fld.price * fld.qty;
                var valdisc = (totalorder * fld.disc) / 100;
                var finalval = totalorder;
                fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                //if (objDetail.is_disc_percBool)
                //{
                //    fld.is_disc_perc = "1";
                //    fld.disc = objDetail.disc;
                //    fld.disc_amt = 0;
                //    var totalorder = fld.price * fld.qty;
                //    var valdisc = (totalorder * fld.disc) / 100;
                //    var finalval = totalorder - valdisc;
                //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                //}
                //else
                //{
                //    fld.is_disc_perc = "0";
                //    fld.disc = 0;
                //    fld.disc_amt = objDetail.disc_amt;
                //    var totalorder = fld.price * fld.qty;
                //    var valdisc = fld.disc_amt;
                //    var finalval = totalorder - valdisc;
                //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                //}


                try
                {
                    orderdtltbl.Add(fld);

                    objDetail.EMP_CODE = objDetail.EMP_CODETemp;
                    objDetail.id_customer = objDetail.id_customerTemp;
                    objDetail.ORDER_DATE = objDetail.ORDER_DATETemp;
                    objDetail.APPROVAL_1 = objDetail.APPROVAL_1Temp;
                    objDetail.APPROVAL_2 = objDetail.APPROVAL_2Temp;
                    objDetail.TOTAL_ORDER = orderdtltbl.Sum(y => y.final_price);
                    objDetail.TOTAL_QTY = orderdtltbl.Sum(y => y.qty);
                    objDetail.id = objDetail.idTemp;
                    objDetail.INV_DISC = objDetail.INV_DISCTemp;
                    objDetail.INV_DISC_AMT = objDetail.INV_DISC_AMTTemp;
                    objDetail.IS_DISC_PERC = objDetail.IS_DISC_PERCTemp;
                    objDetail.custdisc = objDetail.custdiscTemp;
                    objDetail.SHIPPING_ADDRESS = objDetail.SHIPPING_ADDRESSTemp;

                    objDetail.isPassed = true;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", orderdtltbl);

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
                return RedirectToAction("Edit", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Edit", objDetail);
            }
        }
        public JsonResult getOrderDataEdit2(string article, string size)
        {
            //Creating List    
            List<dbSalesOrderDtl> salesorderDtl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
            dbSalesOrderDtl fld = new dbSalesOrderDtl();
            if (salesorderDtl != null)
            {
                fld = salesorderDtl.Where(y => y.article == article && y.size == size).FirstOrDefault();
            }
            return Json(fld);
        }
        public JsonResult validatearticleEdit(string article, string btnid)
        {
            //Creating List    
            List<dbSalesOrderDtl> salesorderDtl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
            string stat = "pass";
            if (btnid != "addArticle")
            {
                ////nilaiSSTbl = nilaiSSTbl.Where(y => y.SS_CODE != sscode).ToList();
                //var fld = nilaiSSTbl.Where(y => y.SS_CODE == sscode).FirstOrDefault();
                //if(fld != null)
                //{

                //}
                //var validate = nilaiSSTbl.Where(y => y.SS_CODE == sscode).ToList();
                //if (validate.Count() > 0)
                //{
                //    stat = "nopass";
                //}
            }
            else
            {
                var validate = salesorderDtl.Where(y => y.article == article).ToList();
                if (validate.Count() > 0)
                {
                    stat = "nopass";
                }
            }

            return Json(stat);
        }
        public IActionResult UploadArticleEdit([Bind] dbSalesOrder objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbSalesOrderDtl> orderDtlTbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
                if (orderDtlTbl == null)
                {
                    orderDtlTbl = new List<dbSalesOrderDtl>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", orderDtlTbl);
                }
                var file = objDetail.fileuploadArticle;

                string error = "";
                try
                {
                    if (file == null || file.Length == 0)
                    {
                        error = "file not found" + "\n";
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
                                dbSalesOrderDtl field = new dbSalesOrderDtl();
                                object article = ws.Cells[row, 1].Value;
                                object size1 = ws.Cells[row, 2].Value;
                                object size2 = ws.Cells[row, 3].Value;
                                object size3 = ws.Cells[row, 4].Value;
                                object size4 = ws.Cells[row, 5].Value;
                                object size5 = ws.Cells[row, 6].Value;
                                object size6 = ws.Cells[row, 7].Value;
                                object size7 = ws.Cells[row, 8].Value;
                                object size8 = ws.Cells[row, 9].Value;
                                object size9 = ws.Cells[row, 10].Value;
                                object size10 = ws.Cells[row, 11].Value;
                                object size11 = ws.Cells[row, 12].Value;
                                object size12 = ws.Cells[row, 13].Value;
                                object size13 = ws.Cells[row, 14].Value;
                                //object disctype = ws.Cells[row, 15].Value;
                                //object discperc = ws.Cells[row, 16].Value;
                                //object discamt = ws.Cells[row, 17].Value;


                                var articlesizecode = new ArticleSize();
                                if (article != null)
                                {
                                    field.article = article.ToString();
                                    articlesizecode = getsizecodePdf(article.ToString());
                                }
                                if (size1 != null)
                                {
                                    field.Size_1 = Convert.ToInt32(size1);
                                    if (!string.IsNullOrEmpty(articlesizecode.size1))
                                    {
                                        var code = articlesizecode.size1.Substring(1);
                                        if (articlesizecode.size1.Length > 2)
                                        {
                                            code = articlesizecode.size1.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_1 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_1 = 0;
                                }
                                if (size2 != null)
                                {
                                    field.Size_2 = Convert.ToInt32(size2);
                                    if (!string.IsNullOrEmpty(articlesizecode.size2))
                                    {
                                        var code = articlesizecode.size2.Substring(1);
                                        if (articlesizecode.size2.Length > 2)
                                        {
                                            code = articlesizecode.size2.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_2 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_2 = 0;
                                }
                                if (size3 != null)
                                {
                                    field.Size_3 = Convert.ToInt32(size3);
                                    if (!string.IsNullOrEmpty(articlesizecode.size3))
                                    {
                                        var code = articlesizecode.size3.Substring(1);
                                        if (articlesizecode.size3.Length > 2)
                                        {
                                            code = articlesizecode.size3.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_3 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_3 = 0;
                                }
                                if (size4 != null)
                                {
                                    field.Size_4 = Convert.ToInt32(size4);
                                    if (!string.IsNullOrEmpty(articlesizecode.size4))
                                    {
                                        var code = articlesizecode.size4.Substring(1);
                                        if (articlesizecode.size4.Length > 2)
                                        {
                                            code = articlesizecode.size4.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_4 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_4 = 0;
                                }
                                if (size5 != null)
                                {
                                    field.Size_5 = Convert.ToInt32(size5);
                                    if (!string.IsNullOrEmpty(articlesizecode.size5))
                                    {
                                        var code = articlesizecode.size5.Substring(1);
                                        if (articlesizecode.size5.Length > 2)
                                        {
                                            code = articlesizecode.size5.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_5 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_5 = 0;
                                }
                                if (size6 != null)
                                {
                                    field.Size_6 = Convert.ToInt32(size6);
                                    if (!string.IsNullOrEmpty(articlesizecode.size6))
                                    {
                                        var code = articlesizecode.size6.Substring(1);
                                        if (articlesizecode.size6.Length > 2)
                                        {
                                            code = articlesizecode.size6.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_6 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_6 = 0;
                                }
                                if (size7 != null)
                                {
                                    field.Size_7 = Convert.ToInt32(size7);
                                    if (!string.IsNullOrEmpty(articlesizecode.size7))
                                    {
                                        var code = articlesizecode.size7.Substring(1);
                                        if (articlesizecode.size7.Length > 2)
                                        {
                                            code = articlesizecode.size7.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_7 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_7 = 0;
                                }
                                if (size8 != null)
                                {
                                    field.Size_8 = Convert.ToInt32(size8);
                                    if (!string.IsNullOrEmpty(articlesizecode.size8))
                                    {
                                        var code = articlesizecode.size8.Substring(1);
                                        if (articlesizecode.size8.Length > 2)
                                        {
                                            code = articlesizecode.size8.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_8 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_8 = 0;
                                }
                                if (size9 != null)
                                {
                                    field.Size_9 = Convert.ToInt32(size9);
                                    if (!string.IsNullOrEmpty(articlesizecode.size9))
                                    {
                                        var code = articlesizecode.size9.Substring(1);
                                        if (articlesizecode.size9.Length > 2)
                                        {
                                            code = articlesizecode.size9.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_9 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_9 = 0;
                                }
                                if (size10 != null)
                                {
                                    field.Size_10 = Convert.ToInt32(size10);
                                    if (!string.IsNullOrEmpty(articlesizecode.size10))
                                    {
                                        var code = articlesizecode.size10.Substring(1);
                                        if (articlesizecode.size10.Length > 2)
                                        {
                                            code = articlesizecode.size10.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_10 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_10 = 0;
                                }
                                if (size11 != null)
                                {
                                    field.Size_11 = Convert.ToInt32(size11);
                                    if (!string.IsNullOrEmpty(articlesizecode.size11))
                                    {
                                        var code = articlesizecode.size11.Substring(1);
                                        if (articlesizecode.size11.Length > 2)
                                        {
                                            code = articlesizecode.size11.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_11 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_11 = 0;
                                }
                                if (size12 != null)
                                {
                                    field.Size_12 = Convert.ToInt32(size12);
                                    if (!string.IsNullOrEmpty(articlesizecode.size12))
                                    {
                                        var code = articlesizecode.size12.Substring(1);
                                        if (articlesizecode.size12.Length > 2)
                                        {
                                            code = articlesizecode.size12.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_12 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_12 = 0;
                                }
                                if (size13 != null)
                                {
                                    field.Size_13 = Convert.ToInt32(size13);
                                    if (!string.IsNullOrEmpty(articlesizecode.size13))
                                    {
                                        var code = articlesizecode.size13.Substring(1);
                                        if (articlesizecode.size13.Length > 2)
                                        {
                                            code = articlesizecode.size13.Substring(2);
                                        }
                                        if (code == "5")
                                        {
                                            field.Size_13 = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    field.Size_13 = 0;
                                }

                                field.qty = field.Size_1 + field.Size_2 + field.Size_3 + field.Size_4 + field.Size_5 + field.Size_6 + field.Size_7 + field.Size_8 + field.Size_9 + field.Size_10 + field.Size_11 + field.Size_12 + field.Size_13;
                                if (article != null)
                                {
                                    var price = getdatapriceUpl(field.article);
                                    field.price = price;
                                    var totalorder = Convert.ToDecimal(price * field.qty);
                                    field.final_price = Math.Round(totalorder, 2);

                                    //if (disctype != null)
                                    //{
                                    //    var disctypestr = disctype.ToString();
                                    //    if (!String.IsNullOrEmpty(disctypestr))
                                    //    {
                                    //        if (disctypestr == "1")
                                    //        {
                                    //            field.is_disc_perc = "1";
                                    //            field.disc_amt = 0;
                                    //            if (discperc != null)
                                    //            {
                                    //                field.disc = Convert.ToDecimal(discperc);
                                    //                var totaldisc = ((price * field.qty) * field.disc) / 100;
                                    //                totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //            }
                                    //            field.final_price = Math.Round(totalorder, 2);
                                    //        }
                                    //        else
                                    //        {
                                    //            field.is_disc_perc = "0";
                                    //            field.disc = 0;
                                    //            if (discamt != null)
                                    //            {
                                    //                field.disc_amt = Convert.ToDecimal(discamt);
                                    //                var totaldisc = field.disc_amt;
                                    //                totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //            }
                                    //            field.final_price = Math.Round(totalorder, 2);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        field.is_disc_perc = "0";
                                    //        field.disc = 0;
                                    //        if (discamt != null)
                                    //        {
                                    //            field.disc_amt = Convert.ToDecimal(discamt);
                                    //            var totaldisc = field.disc_amt;
                                    //            totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //        }
                                    //        field.final_price = Math.Round(totalorder, 2);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    field.is_disc_perc = "0";
                                    //    field.disc = 0;
                                    //    if (discamt != null)
                                    //    {
                                    //        field.disc_amt = Convert.ToDecimal(discamt);
                                    //        var totaldisc = field.disc_amt;
                                    //        totalorder = (price * field.qty) - Math.Round(Convert.ToDecimal(totaldisc), 2);
                                    //    }
                                    //    field.final_price = Math.Round(totalorder, 2);
                                    //}

                                    var validate = orderDtlTbl.Where(y => y.article == article.ToString()).ToList();
                                    if (validate.Count() == 0)
                                    {
                                        var validateqtydata = validateqtyupl(field.qty);
                                        if (validateqtydata == "pass")
                                        {
                                            orderDtlTbl.Add(field);
                                        }
                                        else
                                        {
                                            error += "article: " + article.ToString() + " Does not meet minimum qty " + "\n";
                                        }
                                    }
                                    else
                                    {
                                        foreach (var flds in validate)
                                        {
                                            flds.Size_1 = field.Size_1;
                                            flds.Size_2 = field.Size_2;
                                            flds.Size_3 = field.Size_3;
                                            flds.Size_4 = field.Size_4;
                                            flds.Size_5 = field.Size_5;
                                            flds.Size_6 = field.Size_6;
                                            flds.Size_7 = field.Size_7;
                                            flds.Size_8 = field.Size_8;
                                            flds.Size_9 = field.Size_9;
                                            flds.Size_10 = field.Size_10;
                                            flds.Size_11 = field.Size_11;
                                            flds.Size_12 = field.Size_12;
                                            flds.Size_13 = field.Size_13;
                                            flds.qty = field.qty;
                                            flds.price = field.price;
                                            flds.final_price = field.final_price;

                                        }
                                    }
                                }

                            }


                        }

                        FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
                        if (filedlt.Exists)//check file exsit or not  
                        {
                            filedlt.Delete();
                        }
                    }
                    objDetail.EMP_CODE = objDetail.EMP_CODETempUpl;
                    objDetail.id_customer = objDetail.id_customerTempUpl;
                    objDetail.ORDER_DATE = objDetail.ORDER_DATETempUpl;
                    objDetail.APPROVAL_1 = objDetail.APPROVAL_1TempUpl;
                    objDetail.APPROVAL_2 = objDetail.APPROVAL_2TempUpl;
                    objDetail.TOTAL_ORDER = objDetail.TOTAL_ORDERTempUpl;
                    objDetail.TOTAL_QTY = objDetail.TOTAL_QTYTempUpl;
                    objDetail.id = objDetail.idTempUpl;
                    objDetail.INV_DISC = objDetail.INV_DISCTempUpl;
                    objDetail.INV_DISC_AMT = objDetail.INV_DISC_AMTTempUpl;
                    objDetail.IS_DISC_PERC = objDetail.IS_DISC_PERCTempUpl;
                    objDetail.custdisc = objDetail.custdiscTempUpl;
                    objDetail.SHIPPING_ADDRESS = objDetail.SHIPPING_ADDRESSTempUpl;
                    objDetail.error = error;
                    objDetail.isPassed = true;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", orderDtlTbl);
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
                return RedirectToAction("Edit", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Edit", objDetail);
            }

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDetailEdit([Bind] dbSalesOrder objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbSalesOrderDtl> orderdata = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
                var splitdatamode = objDetail.datamode.Split("EditArticle");
                if (splitdatamode.Length > 1)
                {
                    foreach (var fld in orderdata)
                    {

                        if (fld.article == objDetail.articleEdit)
                        {
                            //dbNilaiSSFixed fld = nilaiSSTbl.Where(y => y.SS_CODE == objDetail.SS_CODE_EDIT).FirstOrDefault();
                            fld.article = objDetail.articleEdit;
                            fld.size = objDetail.sizeEdit;
                            fld.qty = objDetail.qtyEdit;
                            fld.price = objDetail.priceEdit;
                            fld.Size_1 = objDetail.Size_1_Edit;
                            fld.Size_2 = objDetail.Size_2_Edit;
                            fld.Size_3 = objDetail.Size_3_Edit;
                            fld.Size_4 = objDetail.Size_4_Edit;
                            fld.Size_5 = objDetail.Size_5_Edit;
                            fld.Size_6 = objDetail.Size_6_Edit;
                            fld.Size_7 = objDetail.Size_7_Edit;
                            fld.Size_8 = objDetail.Size_8_Edit;
                            fld.Size_9 = objDetail.Size_9_Edit;
                            fld.Size_10 = objDetail.Size_10_Edit;
                            fld.Size_11 = objDetail.Size_11_Edit;
                            fld.Size_12 = objDetail.Size_12_Edit;
                            fld.Size_13 = objDetail.Size_13_Edit;
                            var totalorder = fld.price * fld.qty;
                            var finalval = totalorder;
                            fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                            //if (objDetail.is_disc_percBoolEdit)
                            //{
                            //    fld.is_disc_perc = "1";
                            //    fld.disc = objDetail.discEdit;
                            //    fld.disc_amt = 0;
                            //    var totalorder = fld.price * fld.qty;
                            //    var valdisc = (totalorder * fld.disc) / 100;
                            //    //var finalval = totalorder - valdisc;
                            //    var finalval = totalorder;

                            //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                            //}
                            //else
                            //{
                            //    fld.is_disc_perc = "0";
                            //    fld.disc = 0;
                            //    fld.disc_amt = objDetail.disc_amtEdit;
                            //    var totalorder = fld.price * fld.qty;
                            //    var valdisc = fld.disc_amt;
                            //    var finalval = totalorder - valdisc;
                            //    fld.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                            //}
                        }

                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", orderdata);

                }
                else
                {
                    //List<dbSalesOrderDtl> orderdata = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
                    var fld = orderdata.Where(y => y.article == objDetail.articleEdit).FirstOrDefault();
                    orderdata.Remove(fld);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", orderdata);

                }

                objDetail.EMP_CODE = objDetail.EMP_CODETempEdit;
                objDetail.id_customer = objDetail.id_customerTempEdit;
                objDetail.ORDER_DATE = objDetail.ORDER_DATETempEdit;
                objDetail.APPROVAL_1 = objDetail.APPROVAL_1TempEdit;
                objDetail.APPROVAL_2 = objDetail.APPROVAL_2TempEdit;
                objDetail.TOTAL_ORDER = orderdata.Sum(y => y.final_price);
                objDetail.TOTAL_QTY = orderdata.Sum(y => y.qty);
                objDetail.id = objDetail.idTempEdit;
                objDetail.INV_DISC = objDetail.INV_DISCTempEdit;
                objDetail.INV_DISC_AMT = objDetail.INV_DISC_AMTTempEdit;
                objDetail.IS_DISC_PERC = objDetail.IS_DISC_PERCTempEdit;
                objDetail.custdisc = objDetail.custdiscTempEdit;
                objDetail.SHIPPING_ADDRESS = objDetail.SHIPPING_ADDRESSTempEdit;

                objDetail.isPassed = true;
                return RedirectToAction("Edit", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Edit", objDetail);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edits(int id, [Bind] dbSalesOrder fld)
        {

            if (ModelState.IsValid)
            {
                var editFld = db.SalesOrderTbl.Find(fld.id);
                editFld.EMP_CODE = fld.EMP_CODE;
                editFld.id_customer = fld.id_customer;
                var dbcust = db.CustomerTbl.Find(fld.id_customer);
                if (dbcust != null)
                {
                    editFld.EMAIL = dbcust.Email;
                }
                editFld.ORDER_DATE = fld.ORDER_DATE;
               
                if (fld.IS_DISC_PERCBoolHdr)
                {
                    editFld.IS_DISC_PERC = "1";
                    editFld.INV_DISC = fld.INV_DISC;
                    editFld.INV_DISC_AMT = 0;
                }
                else
                {
                    editFld.IS_DISC_PERC = "0";
                    editFld.INV_DISC_AMT = fld.INV_DISC_AMT;
                    editFld.INV_DISC = 0;


                }

                editFld.TOTAL_ORDER = fld.TOTAL_ORDER;
                editFld.TOTAL_QTY = fld.TOTAL_QTY;
                editFld.UPDATE_DATE = DateTime.Now;
                editFld.UPDATE_USER = User.Identity.Name;
                editFld.SHIPPING_ADDRESS = fld.SHIPPING_ADDRESS;
                List<dbSalesOrderDtl> ordertbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
                try
                {
                    //Processing article

                    List<string> listtrans = new List<string>();
                    List<string> listExistingtrans = new List<string>();

                    var idToRemovetrans = new List<string>();
                    var idToAddtrans = new List<string>();

                    var ExistingTransDt = db.SalesOrderDtlTbl.Where(y => y.id_order == editFld.id).ToList();
                    for (int i = 0; i < ordertbl.Count(); i++)
                    {
                        var fldadd = ordertbl[i].article;
                        listtrans.Add(fldadd);
                        idToAddtrans.Add(fldadd);
                    }

                    foreach (var exist in ExistingTransDt)
                    {
                        var fldexist = exist.article;
                        listExistingtrans.Add(fldexist);
                        idToRemovetrans.Add(fldexist);

                    }

                    //removing logic 
                    for (int i = 0; i < listExistingtrans.Count(); i++)
                    {
                        var articleexist = listExistingtrans[i];
                        for (int y = 0; y < listtrans.Count(); y++)
                        {
                            var articlenew = listtrans[y];
                            if (articleexist == articlenew)
                            {
                                idToRemovetrans.Remove(listExistingtrans[i]);
                            }
                        }
                    }
                   

                    var empDt = ExistingTransDt.Where(y => idToRemovetrans.Contains(y.article)).ToList<dbSalesOrderDtl>();
                    foreach (var dtlemp in empDt)
                    {
                        foreach (var idtoremove in idToRemovetrans)
                        {
                            var formEmp = dtlemp;
                            db.SalesOrderDtlTbl.Remove(formEmp);
                        }
                    }
                    //adding logic
                    for (int i = 0; i < ordertbl.Count(); i++)
                    {
                        var dts = ordertbl[i];
                        var iddts = dts.id;
                        if (iddts != 0)
                        {
                            var formtrans = db.SalesOrderDtlTbl.Find(iddts);
                            formtrans.article = dts.article;
                            formtrans.article = dts.article;
                            formtrans.Size_1 = dts.Size_1;
                            formtrans.Size_2 = dts.Size_2;
                            formtrans.Size_3 = dts.Size_3;
                            formtrans.Size_4 = dts.Size_4;
                            formtrans.Size_5 = dts.Size_5;
                            formtrans.Size_6 = dts.Size_6;
                            formtrans.Size_7 = dts.Size_7;
                            formtrans.Size_8 = dts.Size_8;
                            formtrans.Size_9 = dts.Size_9;
                            formtrans.Size_10 = dts.Size_10;
                            formtrans.Size_11 = dts.Size_11;
                            formtrans.Size_12 = dts.Size_12;
                            formtrans.Size_13 = dts.Size_13;

                            formtrans.qty = dts.qty;
                            formtrans.price = dts.price;
                            formtrans.disc = dts.disc;
                            var totalorder = formtrans.price * formtrans.qty;
                            //var valdisc = (totalorder * formtrans.disc) / 100;
                            //var finalval = totalorder - valdisc;
                            var finalval = totalorder;
                            formtrans.final_price = Math.Round(Convert.ToDecimal(finalval), 2);                     
                            db.SalesOrderDtlTbl.Update(formtrans);
                        }
                        else
                        {
                            dbSalesOrderDtl form = dts;
                            form.id_order = editFld.id;
                            db.SalesOrderDtlTbl.Add(form);

                        }
                    }

                    db.SaveChanges();
                    //update approval status
                    var editfld2 = db.SalesOrderTbl.Find(fld.id);
                    if (fld.isApprove1)
                    {
                        if (editfld2.APPROVAL_1 != "1")
                        {
                            editfld2.APPROVAL_1 = "1";
                            SendOrderConfirm(editfld2.id);
                        }
                    }
                    else
                    {
                        editfld2.APPROVAL_1 = "0";
                    }
                    if (fld.isApprove2)
                    {
                        if (editfld2.APPROVAL_2 != "1")
                        {
                            editfld2.APPROVAL_2 = "1";

                            string invno = getinvno();
                            //string invno = "29-005";
                            editfld2.picking_no = invno;
                            PostPOtoRIMS(editfld2.id, invno);
                            SendInvoiceMail(editfld2.id, invno);
                            var creditnoteused = db.creditlogtbl.Where(y => y.edp == dbcust.EDP).ToList();
                            foreach (var datas in creditnoteused)
                            {
                                datas.isused = "1";
                                db.creditlogtbl.Update(datas);
                            }
                        }
                    }
                    else
                    {
                        editfld2.APPROVAL_2 = "0";
                    }
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
        [Authorize]
        public IActionResult Delete(int id)
        {

            dbSalesOrder fld = db.SalesOrderTbl.Find(id);

            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSalesOrder(int id)
        {
            dbSalesOrder fld = db.SalesOrderTbl.Find(id);

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
        public IActionResult DownloadFilePayment([FromQuery(Name = "iddata")] string id)
        {
            var ids = id.Replace("'", "");
            var custdata = db.PaymentTbl.Where(y => y.id == Convert.ToInt32(ids)).FirstOrDefault();
            if (custdata != null)
            {

            }
            var file = custdata.FILE_PAYMENT;
            var memory = new MemoryStream(file);
            string[] stringParts = custdata.FILE_PAYMENT_NAME.Split(new char[] { '.' });
            string strType = stringParts[1];
            memory.Position = 0;
            return File(memory, GetContentType(strType), custdata.FILE_PAYMENT_NAME);
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
        public string test()
        {
            var dbCust = db.CustomerTbl.Where(y => y.Email == "a_Tresnaprana@hotmail.com").FirstOrDefault();
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
            newrmdt.week = 22;
            newrmdt.region = "1";
            newrmdt.dist = "50";
            newrmdt.opendate = "25052022";
            string storename = newrmdt.storetype + newrmdt.language + newrmdt.storename;
            string mgr = newrmdt.manager;
            lengthname = storename.Length;
            lengtthmgrname = mgr.Length;
            addr1length = newrmdt.addr1.Length;
            int spare = 0;
            int spareaddr1 = 0;
            int sparemgr = 0;
            int totallength = 32;
            if (storename.Length < 17) 
            {
                spare = 17 - lengthname;
            }
            else
            {
               
                spare = 0;
            }
            totallength = totallength - spare - lengthname;
            if(mgr.Length < 15)
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
            string storedata = String.Format("{0,4}{1,5}{2,5}{3,1}{4,"+ lengthname + "}{5,"+ lengtthmgrname + "}{6, "+ miscelllength+"}{7,2}{8,1}{9,2}{10,8}{11,8}", 
                newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq, storename, mgr, 
                newrmdt.miscell, newrmdt.week, newrmdt.region, newrmdt.dist, newrmdt.opendate, newrmdt.closedate);
            
            string storedata2 = String.Format("{0,4}{1,5}{2,5}{3,1}{4," + newrmdt.addr1.Length + "}{5,"+ addr1length + "}",
                newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq+1, newrmdt.addr1, " ");
            string storedata3 = String.Format("{0,4}{1,5}{2,5}{3,1}{4,30}",
              newrmdt.code, newrmdt.batch, newrmdt.store, newrmdt.seq + 2, newrmdt.addr2);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "RM_07_" + newrmdt.store+"_" + (DateTime.Now).ToString("ddMMyyyyHHmm") + ".dat")))
            {
                string storecompletedata = storedata + "\r\n" + storedata2 + "\r\n" + storedata3;
                outputFile.WriteLine(storecompletedata);
            }
            //string b = String.Format("|{0,-5}|{1,-5}|{2,-5}", 1, 20, 300);
            return storedata;
        }
        public string testrm72()
        {
            var dbSalesHdr = db.SalesOrderTbl.Where(y => y.id == 3).FirstOrDefault();
            var dbCust = db.CustomerTbl.Where(y => y.id == dbSalesHdr.id_customer).FirstOrDefault();
            var dbSalesDtl = db.SalesOrderDtlTbl.Where(y => y.id_order == 3).ToList();
            string orderdata = "";
            foreach(var salesform in dbSalesDtl)
            {
                var newrm72dt = new RIMS72();
                newrm72dt.code = "RM72";
                newrm72dt.batch = "";
                newrm72dt.store = dbCust.EDP;
                newrm72dt.article = salesform.article;
                newrm72dt.space = "";
                newrm72dt.seq = "1";
                newrm72dt.total_pairs = dbSalesHdr.TOTAL_QTY;
                newrm72dt.Size_1 = salesform.Size_1;
                newrm72dt.Size_2 = salesform.Size_2;
                newrm72dt.Size_3 = salesform.Size_3;
                newrm72dt.Size_4 = salesform.Size_4;
                newrm72dt.Size_5 = salesform.Size_5;
                newrm72dt.Size_6 = salesform.Size_6;
                newrm72dt.Size_7 = salesform.Size_7;
                newrm72dt.Size_8 = salesform.Size_8;
                newrm72dt.Size_9 = salesform.Size_9;
                newrm72dt.Size_10 = salesform.Size_10;
                newrm72dt.Size_11 = salesform.Size_11;
                newrm72dt.Size_12 = salesform.Size_12;
                newrm72dt.Size_13 = salesform.Size_13;
                string pairsval = "";
                if(newrm72dt.total_pairs.ToString().Length == 1)
                {
                    pairsval = "0000" + newrm72dt.total_pairs.ToString();
                }
                else
                 if (newrm72dt.total_pairs.ToString().Length == 2)
                {
                    pairsval = "000" + newrm72dt.total_pairs.ToString();

                }
                else
                 if (newrm72dt.total_pairs.ToString().Length == 3)
                {
                    pairsval = "00" + newrm72dt.total_pairs.ToString();

                }
                else
                 if (newrm72dt.total_pairs.ToString().Length == 4)
                {
                    pairsval = "0" + newrm72dt.total_pairs.ToString();

                }
                else
                if (newrm72dt.total_pairs.ToString().Length == 5)
                {
                    pairsval =  newrm72dt.total_pairs.ToString();

                }
                else
                {
                    pairsval =  newrm72dt.total_pairs.ToString();

                }
                #region sizing process
                string size1val = "";
                if (newrm72dt.Size_1.ToString().Length == 0)
                {
                    size1val = "0000";
                }
                else
                if (newrm72dt.Size_1.ToString().Length == 1)
                {
                    size1val = "000" + newrm72dt.Size_1.ToString();
                }
                else
                 if (newrm72dt.Size_1.ToString().Length == 2)
                {
                    size1val = "00" + newrm72dt.Size_1.ToString();

                }
                else
                 if (newrm72dt.Size_1.ToString().Length == 3)
                {
                    size1val = "0" + newrm72dt.Size_1.ToString();

                }
                else
                 if (newrm72dt.Size_1.ToString().Length == 4)
                {
                    size1val = newrm72dt.Size_1.ToString();

                }
                else
                {
                    size1val = newrm72dt.Size_1.ToString();

                }
                string size2val = "";
                if (newrm72dt.Size_2.ToString().Length == 0)
                {
                    size2val = "0000";
                }
                else
                if (newrm72dt.Size_2.ToString().Length == 1)
                {
                    size2val = "000" + newrm72dt.Size_2.ToString();
                }
                else
                if (newrm72dt.Size_2.ToString().Length == 2)
                {
                    size2val = "00" + newrm72dt.Size_2.ToString();

                }
                else
                if (newrm72dt.Size_2.ToString().Length == 3)
                {
                    size2val = "0" + newrm72dt.Size_2.ToString();

                }
                else
                if (newrm72dt.Size_2.ToString().Length == 4)
                {
                    size2val = newrm72dt.Size_2.ToString();

                }
                else
                {
                    size2val = newrm72dt.Size_2.ToString();

                }
                string size3val = "";
                if (newrm72dt.Size_3.ToString().Length == 0)
                {
                    size3val = "0000";
                }
                else
                if (newrm72dt.Size_3.ToString().Length == 1)
                {
                    size3val = "000" + newrm72dt.Size_3.ToString();
                }
                else
                if (newrm72dt.Size_3.ToString().Length == 2)
                {
                    size3val = "00" + newrm72dt.Size_3.ToString();

                }
                else
                if (newrm72dt.Size_3.ToString().Length == 3)
                {
                    size3val = "0" + newrm72dt.Size_3.ToString();

                }
                else
                if (newrm72dt.Size_3.ToString().Length == 4)
                {
                    size3val = newrm72dt.Size_3.ToString();

                }
                else
                {
                    size3val = newrm72dt.Size_3.ToString();

                }
                string size4val = "";
                if (newrm72dt.Size_4.ToString().Length == 0)
                {
                    size4val = "0000";
                }
                else
                if (newrm72dt.Size_4.ToString().Length == 1)
                {
                    size4val = "000" + newrm72dt.Size_4.ToString();
                }
                else
                if (newrm72dt.Size_4.ToString().Length == 2)
                {
                    size4val = "00" + newrm72dt.Size_4.ToString();

                }
                else
                if (newrm72dt.Size_4.ToString().Length == 3)
                {
                    size4val = "0" + newrm72dt.Size_4.ToString();

                }
                else
                if (newrm72dt.Size_4.ToString().Length == 4)
                {
                    size4val = newrm72dt.Size_4.ToString();

                }
                else
                {
                    size4val = newrm72dt.Size_4.ToString();

                }
                string size5val = "";
                if (newrm72dt.Size_5.ToString().Length == 0)
                {
                    size5val = "0000";
                }
                else
                if (newrm72dt.Size_5.ToString().Length == 1)
                {
                    size5val = "000" + newrm72dt.Size_5.ToString();
                }
                else
                if (newrm72dt.Size_5.ToString().Length == 2)
                {
                    size5val = "00" + newrm72dt.Size_5.ToString();

                }
                else
                if (newrm72dt.Size_5.ToString().Length == 3)
                {
                    size5val = "0" + newrm72dt.Size_5.ToString();

                }
                else
                if (newrm72dt.Size_5.ToString().Length == 4)
                {
                    size5val = newrm72dt.Size_5.ToString();

                }
                else
                {
                    size5val = newrm72dt.Size_5.ToString();

                }
                string size6val = "";
                if (newrm72dt.Size_6.ToString().Length == 0)
                {
                    size6val = "0000";
                }
                else
                if (newrm72dt.Size_6.ToString().Length == 1)
                {
                    size6val = "000" + newrm72dt.Size_6.ToString();
                }
                else
                if (newrm72dt.Size_6.ToString().Length == 2)
                {
                    size6val = "00" + newrm72dt.Size_6.ToString();

                }
                else
                if (newrm72dt.Size_6.ToString().Length == 3)
                {
                    size6val = "0" + newrm72dt.Size_6.ToString();

                }
                else
                if (newrm72dt.Size_6.ToString().Length == 4)
                {
                    size6val = newrm72dt.Size_6.ToString();

                }
                else
                {
                    size6val = newrm72dt.Size_6.ToString();

                }
                string size7val = "";
                if (newrm72dt.Size_7.ToString().Length == 0)
                {
                    size7val = "0000";
                }
                else
                if (newrm72dt.Size_7.ToString().Length == 1)
                {
                    size7val = "000" + newrm72dt.Size_7.ToString();
                }
                else
                if (newrm72dt.Size_7.ToString().Length == 2)
                {
                    size7val = "00" + newrm72dt.Size_7.ToString();

                }
                else
                if (newrm72dt.Size_7.ToString().Length == 3)
                {
                    size7val = "0" + newrm72dt.Size_7.ToString();

                }
                else
                if (newrm72dt.Size_7.ToString().Length == 4)
                {
                    size7val = newrm72dt.Size_7.ToString();

                }
                else
                {
                    size7val = newrm72dt.Size_7.ToString();

                }
                string size8val = "";
                if (newrm72dt.Size_8.ToString().Length == 0)
                {
                    size8val = "0000";
                }
                else
                if (newrm72dt.Size_8.ToString().Length == 1)
                {
                    size8val = "000" + newrm72dt.Size_8.ToString();
                }
                else
                if (newrm72dt.Size_8.ToString().Length == 2)
                {
                    size8val = "00" + newrm72dt.Size_8.ToString();

                }
                else
                if (newrm72dt.Size_8.ToString().Length == 3)
                {
                    size8val = "0" + newrm72dt.Size_8.ToString();

                }
                else
                if (newrm72dt.Size_8.ToString().Length == 4)
                {
                    size8val = newrm72dt.Size_8.ToString();

                }
                else
                {
                    size8val = newrm72dt.Size_8.ToString();

                }
                string size9val = "";
                if (newrm72dt.Size_9.ToString().Length == 0)
                {
                    size9val = "0000";
                }
                else
                if (newrm72dt.Size_9.ToString().Length == 1)
                {
                    size9val = "000" + newrm72dt.Size_9.ToString();
                }
                else
                if (newrm72dt.Size_9.ToString().Length == 2)
                {
                    size9val = "00" + newrm72dt.Size_9.ToString();

                }
                else
                if (newrm72dt.Size_9.ToString().Length == 3)
                {
                    size9val = "0" + newrm72dt.Size_9.ToString();

                }
                else
                if (newrm72dt.Size_9.ToString().Length == 4)
                {
                    size9val = newrm72dt.Size_9.ToString();

                }
                else
                {
                    size9val = newrm72dt.Size_9.ToString();

                }

                string size10val = "";
                if (newrm72dt.Size_10.ToString().Length == 0)
                {
                    size10val = "0000";
                }
                else
                if (newrm72dt.Size_10.ToString().Length == 1)
                {
                    size10val = "000" + newrm72dt.Size_10.ToString();
                }
                else
                if (newrm72dt.Size_10.ToString().Length == 2)
                {
                    size10val = "00" + newrm72dt.Size_10.ToString();

                }
                else
                if (newrm72dt.Size_10.ToString().Length == 3)
                {
                    size10val = "0" + newrm72dt.Size_10.ToString();

                }
                else
                if (newrm72dt.Size_10.ToString().Length == 4)
                {
                    size10val = newrm72dt.Size_10.ToString();

                }
                else
                {
                    size10val = newrm72dt.Size_10.ToString();

                }
                string size11val = "";
                if (newrm72dt.Size_11.ToString().Length == 0)
                {
                    size11val = "0000";
                }
                else
                if (newrm72dt.Size_11.ToString().Length == 1)
                {
                    size11val = "000" + newrm72dt.Size_11.ToString();
                }
                else
                if (newrm72dt.Size_11.ToString().Length == 2)
                {
                    size11val = "00" + newrm72dt.Size_11.ToString();

                }
                else
                if (newrm72dt.Size_11.ToString().Length == 3)
                {
                    size11val = "0" + newrm72dt.Size_11.ToString();

                }
                else
                if (newrm72dt.Size_11.ToString().Length == 4)
                {
                    size11val = newrm72dt.Size_11.ToString();

                }
                else
                {
                    size11val = newrm72dt.Size_11.ToString();

                }

                string size12val = "";
                if (newrm72dt.Size_12.ToString().Length == 0)
                {
                    size12val = "0000";
                }
                else
                if (newrm72dt.Size_12.ToString().Length == 1)
                {
                    size12val = "000" + newrm72dt.Size_12.ToString();
                }
                else
                if (newrm72dt.Size_12.ToString().Length == 2)
                {
                    size12val = "00" + newrm72dt.Size_12.ToString();

                }
                else
                if (newrm72dt.Size_12.ToString().Length == 3)
                {
                    size12val = "0" + newrm72dt.Size_12.ToString();

                }
                else
                if (newrm72dt.Size_12.ToString().Length == 4)
                {
                    size12val = newrm72dt.Size_12.ToString();

                }
                else
                {
                    size12val = newrm72dt.Size_12.ToString();

                }

                string size13val = "";
                if (newrm72dt.Size_13.ToString().Length == 0)
                {
                    size13val = "000";
                }
                else
                if (newrm72dt.Size_13.ToString().Length == 1)
                {
                    size13val = "00" + newrm72dt.Size_13.ToString();
                }
                else
                if (newrm72dt.Size_13.ToString().Length == 2)
                {
                    size13val = "0" + newrm72dt.Size_13.ToString();

                }
                else
                if (newrm72dt.Size_13.ToString().Length == 3)
                {
                    size13val = newrm72dt.Size_13.ToString();

                }

                else
                {
                    size13val = newrm72dt.Size_13.ToString();

                }
                #endregion sizing process

                orderdata += String.Format("{0,4}{1,5}{2,5}{3,7}{4,2}{5,1}{6,5}{7,4}{8,4}{9,4}{10,4}{11,4}{12,4}{13,4}{14,4}{15,4}{16,4}{17,4}{18,4}{19,3}",
                                              newrm72dt.code, newrm72dt.batch, newrm72dt.store, newrm72dt.article, newrm72dt.space, newrm72dt.seq, pairsval, size1val,
                                              size2val, size3val, size4val, size5val, size6val, size7val, size8val, size9val, size10val, size11val, size12val, size13val) + "\r\n";
            }


            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "RM_72_" + dbSalesHdr.id + "_" + (DateTime.Now).ToString("ddMMyyyyHHmm") + ".dat")))
            {
                string orderdatas = orderdata;
                outputFile.WriteLine(orderdatas);
            }
            //string b = String.Format("|{0,-5}|{1,-5}|{2,-5}", 1, 20, 300);
            return orderdata;
        }
        public string FtpUplTest()
        {
            string link = Configuration.GetConnectionString("LinkFTP");
            string user = Configuration.GetConnectionString("UserFTP");
            string pass = Configuration.GetConnectionString("PassFTP");
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
            var files = Path.Combine(fileUrl, "RM_07_71023_080620220933.dat");
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
        public string PostPOtoRIMS(int orderid, string invno)
        {
            var dbSalesHdr = db.SalesOrderTbl.Where(y => y.id == orderid).FirstOrDefault();
            var dbCust = db.CustomerTbl.Where(y => y.id == dbSalesHdr.id_customer).FirstOrDefault();
            var dbSalesDtl = db.SalesOrderDtlTbl.Where(y => y.id_order == dbSalesHdr.id).ToList();
            string orderdata = "";
            foreach (var salesform in dbSalesDtl)
            {
                var newrm72dt = new RIMS72();
                newrm72dt.code = "RM72";
                newrm72dt.batch = "";
                newrm72dt.store = dbCust.EDP;
                newrm72dt.article = salesform.article;
                newrm72dt.space = "";
                newrm72dt.seq = "1";
                newrm72dt.total_pairs = salesform.qty;
                newrm72dt.Size_1 = salesform.Size_1;
                newrm72dt.Size_2 = salesform.Size_2;
                newrm72dt.Size_3 = salesform.Size_3;
                newrm72dt.Size_4 = salesform.Size_4;
                newrm72dt.Size_5 = salesform.Size_5;
                newrm72dt.Size_6 = salesform.Size_6;
                newrm72dt.Size_7 = salesform.Size_7;
                newrm72dt.Size_8 = salesform.Size_8;
                newrm72dt.Size_9 = salesform.Size_9;
                newrm72dt.Size_10 = salesform.Size_10;
                newrm72dt.Size_11 = salesform.Size_11;
                newrm72dt.Size_12 = salesform.Size_12;
                newrm72dt.Size_13 = salesform.Size_13;
                string pairsval = "";
                if (newrm72dt.total_pairs.ToString().Length == 1)
                {
                    pairsval = "0000" + newrm72dt.total_pairs.ToString();
                }
                else
                 if (newrm72dt.total_pairs.ToString().Length == 2)
                {
                    pairsval = "000" + newrm72dt.total_pairs.ToString();

                }
                else
                 if (newrm72dt.total_pairs.ToString().Length == 3)
                {
                    pairsval = "00" + newrm72dt.total_pairs.ToString();

                }
                else
                 if (newrm72dt.total_pairs.ToString().Length == 4)
                {
                    pairsval = "0" + newrm72dt.total_pairs.ToString();

                }
                else
                if (newrm72dt.total_pairs.ToString().Length == 5)
                {
                    pairsval = newrm72dt.total_pairs.ToString();

                }
                else
                {
                    pairsval = newrm72dt.total_pairs.ToString();

                }
                #region sizing process
                string size1val = "";
                if (newrm72dt.Size_1.ToString().Length == 0)
                {
                    size1val = "0000";
                }
                else
                if (newrm72dt.Size_1.ToString().Length == 1)
                {
                    size1val = "000" + newrm72dt.Size_1.ToString();
                }
                else
                 if (newrm72dt.Size_1.ToString().Length == 2)
                {
                    size1val = "00" + newrm72dt.Size_1.ToString();

                }
                else
                 if (newrm72dt.Size_1.ToString().Length == 3)
                {
                    size1val = "0" + newrm72dt.Size_1.ToString();

                }
                else
                 if (newrm72dt.Size_1.ToString().Length == 4)
                {
                    size1val = newrm72dt.Size_1.ToString();

                }
                else
                {
                    size1val = newrm72dt.Size_1.ToString();

                }
                string size2val = "";
                if (newrm72dt.Size_2.ToString().Length == 0)
                {
                    size2val = "0000";
                }
                else
                if (newrm72dt.Size_2.ToString().Length == 1)
                {
                    size2val = "000" + newrm72dt.Size_2.ToString();
                }
                else
                if (newrm72dt.Size_2.ToString().Length == 2)
                {
                    size2val = "00" + newrm72dt.Size_2.ToString();

                }
                else
                if (newrm72dt.Size_2.ToString().Length == 3)
                {
                    size2val = "0" + newrm72dt.Size_2.ToString();

                }
                else
                if (newrm72dt.Size_2.ToString().Length == 4)
                {
                    size2val = newrm72dt.Size_2.ToString();

                }
                else
                {
                    size2val = newrm72dt.Size_2.ToString();

                }
                string size3val = "";
                if (newrm72dt.Size_3.ToString().Length == 0)
                {
                    size3val = "0000";
                }
                else
                if (newrm72dt.Size_3.ToString().Length == 1)
                {
                    size3val = "000" + newrm72dt.Size_3.ToString();
                }
                else
                if (newrm72dt.Size_3.ToString().Length == 2)
                {
                    size3val = "00" + newrm72dt.Size_3.ToString();

                }
                else
                if (newrm72dt.Size_3.ToString().Length == 3)
                {
                    size3val = "0" + newrm72dt.Size_3.ToString();

                }
                else
                if (newrm72dt.Size_3.ToString().Length == 4)
                {
                    size3val = newrm72dt.Size_3.ToString();

                }
                else
                {
                    size3val = newrm72dt.Size_3.ToString();

                }
                string size4val = "";
                if (newrm72dt.Size_4.ToString().Length == 0)
                {
                    size4val = "0000";
                }
                else
                if (newrm72dt.Size_4.ToString().Length == 1)
                {
                    size4val = "000" + newrm72dt.Size_4.ToString();
                }
                else
                if (newrm72dt.Size_4.ToString().Length == 2)
                {
                    size4val = "00" + newrm72dt.Size_4.ToString();

                }
                else
                if (newrm72dt.Size_4.ToString().Length == 3)
                {
                    size4val = "0" + newrm72dt.Size_4.ToString();

                }
                else
                if (newrm72dt.Size_4.ToString().Length == 4)
                {
                    size4val = newrm72dt.Size_4.ToString();

                }
                else
                {
                    size4val = newrm72dt.Size_4.ToString();

                }
                string size5val = "";
                if (newrm72dt.Size_5.ToString().Length == 0)
                {
                    size5val = "0000";
                }
                else
                if (newrm72dt.Size_5.ToString().Length == 1)
                {
                    size5val = "000" + newrm72dt.Size_5.ToString();
                }
                else
                if (newrm72dt.Size_5.ToString().Length == 2)
                {
                    size5val = "00" + newrm72dt.Size_5.ToString();

                }
                else
                if (newrm72dt.Size_5.ToString().Length == 3)
                {
                    size5val = "0" + newrm72dt.Size_5.ToString();

                }
                else
                if (newrm72dt.Size_5.ToString().Length == 4)
                {
                    size5val = newrm72dt.Size_5.ToString();

                }
                else
                {
                    size5val = newrm72dt.Size_5.ToString();

                }
                string size6val = "";
                if (newrm72dt.Size_6.ToString().Length == 0)
                {
                    size6val = "0000";
                }
                else
                if (newrm72dt.Size_6.ToString().Length == 1)
                {
                    size6val = "000" + newrm72dt.Size_6.ToString();
                }
                else
                if (newrm72dt.Size_6.ToString().Length == 2)
                {
                    size6val = "00" + newrm72dt.Size_6.ToString();

                }
                else
                if (newrm72dt.Size_6.ToString().Length == 3)
                {
                    size6val = "0" + newrm72dt.Size_6.ToString();

                }
                else
                if (newrm72dt.Size_6.ToString().Length == 4)
                {
                    size6val = newrm72dt.Size_6.ToString();

                }
                else
                {
                    size6val = newrm72dt.Size_6.ToString();

                }
                string size7val = "";
                if (newrm72dt.Size_7.ToString().Length == 0)
                {
                    size7val = "0000";
                }
                else
                if (newrm72dt.Size_7.ToString().Length == 1)
                {
                    size7val = "000" + newrm72dt.Size_7.ToString();
                }
                else
                if (newrm72dt.Size_7.ToString().Length == 2)
                {
                    size7val = "00" + newrm72dt.Size_7.ToString();

                }
                else
                if (newrm72dt.Size_7.ToString().Length == 3)
                {
                    size7val = "0" + newrm72dt.Size_7.ToString();

                }
                else
                if (newrm72dt.Size_7.ToString().Length == 4)
                {
                    size7val = newrm72dt.Size_7.ToString();

                }
                else
                {
                    size7val = newrm72dt.Size_7.ToString();

                }
                string size8val = "";
                if (newrm72dt.Size_8.ToString().Length == 0)
                {
                    size8val = "0000";
                }
                else
                if (newrm72dt.Size_8.ToString().Length == 1)
                {
                    size8val = "000" + newrm72dt.Size_8.ToString();
                }
                else
                if (newrm72dt.Size_8.ToString().Length == 2)
                {
                    size8val = "00" + newrm72dt.Size_8.ToString();

                }
                else
                if (newrm72dt.Size_8.ToString().Length == 3)
                {
                    size8val = "0" + newrm72dt.Size_8.ToString();

                }
                else
                if (newrm72dt.Size_8.ToString().Length == 4)
                {
                    size8val = newrm72dt.Size_8.ToString();

                }
                else
                {
                    size8val = newrm72dt.Size_8.ToString();

                }
                string size9val = "";
                if (newrm72dt.Size_9.ToString().Length == 0)
                {
                    size9val = "0000";
                }
                else
                if (newrm72dt.Size_9.ToString().Length == 1)
                {
                    size9val = "000" + newrm72dt.Size_9.ToString();
                }
                else
                if (newrm72dt.Size_9.ToString().Length == 2)
                {
                    size9val = "00" + newrm72dt.Size_9.ToString();

                }
                else
                if (newrm72dt.Size_9.ToString().Length == 3)
                {
                    size9val = "0" + newrm72dt.Size_9.ToString();

                }
                else
                if (newrm72dt.Size_9.ToString().Length == 4)
                {
                    size9val = newrm72dt.Size_9.ToString();

                }
                else
                {
                    size9val = newrm72dt.Size_9.ToString();

                }

                string size10val = "";
                if (newrm72dt.Size_10.ToString().Length == 0)
                {
                    size10val = "0000";
                }
                else
                if (newrm72dt.Size_10.ToString().Length == 1)
                {
                    size10val = "000" + newrm72dt.Size_10.ToString();
                }
                else
                if (newrm72dt.Size_10.ToString().Length == 2)
                {
                    size10val = "00" + newrm72dt.Size_10.ToString();

                }
                else
                if (newrm72dt.Size_10.ToString().Length == 3)
                {
                    size10val = "0" + newrm72dt.Size_10.ToString();

                }
                else
                if (newrm72dt.Size_10.ToString().Length == 4)
                {
                    size10val = newrm72dt.Size_10.ToString();

                }
                else
                {
                    size10val = newrm72dt.Size_10.ToString();

                }
                string size11val = "";
                if (newrm72dt.Size_11.ToString().Length == 0)
                {
                    size11val = "0000";
                }
                else
                if (newrm72dt.Size_11.ToString().Length == 1)
                {
                    size11val = "000" + newrm72dt.Size_11.ToString();
                }
                else
                if (newrm72dt.Size_11.ToString().Length == 2)
                {
                    size11val = "00" + newrm72dt.Size_11.ToString();

                }
                else
                if (newrm72dt.Size_11.ToString().Length == 3)
                {
                    size11val = "0" + newrm72dt.Size_11.ToString();

                }
                else
                if (newrm72dt.Size_11.ToString().Length == 4)
                {
                    size11val = newrm72dt.Size_11.ToString();

                }
                else
                {
                    size11val = newrm72dt.Size_11.ToString();

                }

                string size12val = "";
                if (newrm72dt.Size_12.ToString().Length == 0)
                {
                    size12val = "0000";
                }
                else
                if (newrm72dt.Size_12.ToString().Length == 1)
                {
                    size12val = "000" + newrm72dt.Size_12.ToString();
                }
                else
                if (newrm72dt.Size_12.ToString().Length == 2)
                {
                    size12val = "00" + newrm72dt.Size_12.ToString();

                }
                else
                if (newrm72dt.Size_12.ToString().Length == 3)
                {
                    size12val = "0" + newrm72dt.Size_12.ToString();

                }
                else
                if (newrm72dt.Size_12.ToString().Length == 4)
                {
                    size12val = newrm72dt.Size_12.ToString();

                }
                else
                {
                    size12val = newrm72dt.Size_12.ToString();

                }

                string size13val = "";
                if (newrm72dt.Size_13.ToString().Length == 0)
                {
                    size13val = "000";
                }
                else
                if (newrm72dt.Size_13.ToString().Length == 1)
                {
                    size13val = "00" + newrm72dt.Size_13.ToString();
                }
                else
                if (newrm72dt.Size_13.ToString().Length == 2)
                {
                    size13val = "0" + newrm72dt.Size_13.ToString();

                }
                else
                if (newrm72dt.Size_13.ToString().Length == 3)
                {
                    size13val = newrm72dt.Size_13.ToString();

                }

                else
                {
                    size13val = newrm72dt.Size_13.ToString();

                }
                #endregion sizing process

                orderdata += String.Format("{0,4}{1,5}{2,5}{3,7}{4,2}{5,1}{6,5}{7,4}{8,4}{9,4}{10,4}{11,4}{12,4}{13,4}{14,4}{15,4}{16,4}{17,4}{18,4}{19,3}",
                                              newrm72dt.code, newrm72dt.batch, newrm72dt.store, newrm72dt.article, newrm72dt.space, newrm72dt.seq, pairsval, size1val,
                                              size2val, size3val, size4val, size5val, size6val, size7val, size8val, size9val, size10val, size11val, size12val, size13val) + "\r\n";
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CreatePO");
            //var filenamerims = "RM_72_" + dbSalesHdr.id + "_" + (DateTime.Now).ToString("ddMMyyyyHHmm") + ".dat";
            var splitinvno = invno.Split("-");
            var datenow = (DateTime.Now).ToString("dd") + (DateTime.Now).ToString("MMM");
            var hrnow = (DateTime.Now).ToString("HH") + (DateTime.Now).ToString("mm");
            var filenamerims =  "72W-"+ splitinvno[0] + splitinvno[1] +"-" + datenow + "-" + hrnow + ".dat";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, filenamerims)))
            {
                string orderdatas = orderdata;
                outputFile.WriteLine(orderdatas);
            }

            //string b = String.Format("|{0,-5}|{1,-5}|{2,-5}", 1, 20, 300);
            string link = Configuration.GetConnectionString("LinkFTP");
            string user = Configuration.GetConnectionString("UserFTP");
            string pass = Configuration.GetConnectionString("PassFTP");
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CreatePO");
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
        public JsonResult validateorder(int orderid)
        {
            //Creating List    
            List<dbPaymentList> paymentData = db.PaymentTbl.Where(y => y.id_order == orderid).ToList();
            dbSalesOrder orderdata = db.SalesOrderTbl.Find(orderid);
            dbCustomer custdata = db.CustomerTbl.Where(y => y.Email == orderdata.EMAIL).FirstOrDefault();
            decimal? custdisc = 0;
            if(custdata != null)
            {
                custdisc = custdata.discount_customer;
            }
            var totalorder = orderdata.TOTAL_ORDER;
            var creditnotedata = getcreditnotepdf(custdata.EDP);
            totalorder = totalorder - creditnotedata;
            var discountcustval = (totalorder * custdisc) / 100;
            if(discountcustval != null)
            {
                totalorder = totalorder - discountcustval;
            }
            var disc = orderdata.INV_DISC;
            var sumpaid = paymentData.Where(y => y.id_order == orderid).Sum(y => y.TOTAL_PAY);
            string stat = "pass";
           if(orderdata.IS_DISC_PERC == "1")
            {
                
                var totalorderdisc = (totalorder * disc) / 100;
                var totalorderfinal = totalorder - totalorderdisc;
                if(sumpaid >= totalorderfinal)
                {
                    stat = "pass";
                }
                else
                {
                    stat = "nopass";

                }
            }
            else
            {
                var discs = orderdata.INV_DISC_AMT;
                if(discs == null)
                {
                    discs = 0;
                }
                var totalorderfinal = totalorder - discs;
                if (sumpaid >= totalorderfinal)
                {
                    stat = "pass";
                }
                else
                {
                    stat = "nopass";

                }

            }

            return Json(stat);
        }
        public void testpdf()
        {
            //Create a new PDF document.

            PdfDocument document = new PdfDocument();

            // Set the page size.

            document.PageSettings.Size = PdfPageSize.A4;

            //Add a page to the document.

            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page.

            PdfGraphics graphics = page.Graphics;

            //Set the font.

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text.

            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));
            
            //Save the document.

            FileStream fileStream = new FileStream("Sample.pdf", FileMode.Create, FileAccess.ReadWrite);

            //Save and close the PDF document
            document.Save(fileStream);
            document.Close(true);
        }
        public string CreatePDFProformInv(int id, string invno)
        {
            string filename = "";
            var datasourcehdr = db.SalesOrderTbl.Find(id);
            var datasourcedtl = db.SalesOrderDtlTbl.Where(y => y.id_order == datasourcehdr.id).ToList();
            var datacust = db.CustomerTbl.Find(datasourcehdr.id_customer);
            List<InvoiceDtl> invoicedtl = new List<InvoiceDtl>();
            foreach(var fld in datasourcedtl)
            {
                var row1 = new InvoiceDtl();
                row1.article = fld.article;
                var sizecodedt = getsizecodePdf(fld.article);
                row1.projectname = sizecodedt.projectname;
                row1.price = fld.price;
                row1.qty = fld.qty;
                row1.S1 = fld.Size_1.ToString();
                row1.S2 = fld.Size_2.ToString();
                row1.S3 = fld.Size_3.ToString();
                row1.S4 = fld.Size_4.ToString();
                row1.S5 = fld.Size_5.ToString();
                row1.S6 = fld.Size_6.ToString();
                row1.S7 = fld.Size_7.ToString();
                row1.S8 = fld.Size_8.ToString();
                row1.S9 = fld.Size_9.ToString();
                row1.S10 = fld.Size_10.ToString();
                row1.S11 = fld.Size_11.ToString();
                row1.S12 = fld.Size_12.ToString();
                row1.S13 = fld.Size_13.ToString();

                string disctype = "";
                if(fld.is_disc_perc == "1")
                {
                    disctype = "percentage";
                    row1.disc = fld.disc;
                }
                else
                {
                    disctype = "amount";
                    row1.disc = fld.disc_amt;

                }
                row1.disctype = disctype;
                row1.final = fld.final_price;
               
                invoicedtl.Add(row1);

            }
            //Creates a new PDF document
            PdfDocument document = new PdfDocument();
            //Adds page settings
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            document.PageSettings.Margins.All = 25;
            document.PageSettings.Size = PdfPageSize.A2;
            //Adds a page to the document
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            
            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");
            
        var files = Path.Combine(fileUrl, "Bata_red_small.png");
            FileStream imageStream = new FileStream(files, FileMode.Open, FileAccess.Read);
            Syncfusion.Drawing.RectangleF bounds = new Syncfusion.Drawing.RectangleF(700, 0,250, 60);
            PdfImage image = PdfImage.FromStream(imageStream);
            PdfFont fonts = new PdfStandardFont(PdfFontFamily.TimesRoman, 15);

            //Draw the text.

            //Draws the image to the PDF page
            page.Graphics.DrawImage(image, bounds);
            graphics.DrawString("Size Guide:", fonts, PdfBrushes.Black, new Syncfusion.Drawing.PointF(220, 280));

            var fileUrlSize = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");
            var filesize = Path.Combine(fileUrl, "sizeguide.png");
            FileStream imageStream2 = new FileStream(filesize, FileMode.Open, FileAccess.Read);
            Syncfusion.Drawing.RectangleF bounds2 = new Syncfusion.Drawing.RectangleF(300,280,1000, 160);
            PdfImage image2 = PdfImage.FromStream(imageStream2);
            //Draws the image to the PDF page
            page.Graphics.DrawImage(image2, bounds2);

            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(232, 4, 20));
            bounds = new Syncfusion.Drawing.RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number
            PdfTextElement element = new PdfTextElement("INVOICE: " + invno, subHeadingFont);
            element.Brush = PdfBrushes.White;
            //Draws the heading on the page
            PdfLayoutResult result = element.Draw(page, new Syncfusion.Drawing.PointF(10, bounds.Top + 8));

            string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            //Measures the width of the text to place it in the correct location
            Syncfusion.Drawing.SizeF textSize = subHeadingFont.MeasureString(currentDate);
            Syncfusion.Drawing.PointF textPosition = new Syncfusion.Drawing.PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //Draws the date by using DrawString method
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
            //Creates text elements to add the address and draw it to the page.
            string billto = "Bill To: " + "\r\n";
            billto += datacust.CUST_NAME + "\r\n";
            billto += datacust.address + "\r\n";
            billto += datacust.PHONE1 + "\r\n";
            element = new PdfTextElement(billto, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(0, 0, 0));
            result = element.Draw(page, new Syncfusion.Drawing.PointF(10, result.Bounds.Bottom + 25));
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            Syncfusion.Drawing.PointF startPoint = new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + 3);
            Syncfusion.Drawing.PointF endPoint = new Syncfusion.Drawing.PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);


            var invoiceDetails = invoicedtl;
            //Creates a PDF grid
            PdfGrid grid = new PdfGrid();
            //Adds the data source
            grid.DataSource = invoiceDetails;
            //Creates the grid cell styles
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(232, 4, 20));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);
            PdfGridCellStyle rowstyle = new PdfGridCellStyle();
            rowstyle.TextBrush = PdfBrushes.Black;
            rowstyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
            var test = grid.Rows.Count();
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                var cellcount = grid.Rows[i].Cells.Count;
                for (int x = 0; x < cellcount; x++)
                {
                    grid.Rows[i].Cells[x].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                    grid.Rows[i].Cells[x].Style = rowstyle;
                }
            }

            //Adds cell customizations
            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            }

            //Applies the header style
            header.ApplyStyle(headerStyle);
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            //Creates the layout format for grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            // Creates layout format settings to allow the table pagination
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new Syncfusion.Drawing.RectangleF(new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + 200), new Syncfusion.Drawing.SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            //Creates text elements to add the address and draw it to the page.
            string footer = " " + "\r\n";
            footer += "Total Qty: " + datasourcehdr.TOTAL_QTY + "\r\n";

            decimal? totalorders = datasourcehdr.TOTAL_ORDER;
            var edpcust = datacust.EDP;
            var getcreditnote = getcreditnotepdf(edpcust);
            totalorders = totalorders - getcreditnote;
            string totalorder = "";
            string discounts = "";
            string custdiscstr = "0";
            var datacustdisc = datacust.discount_customer;
            if (datacustdisc != null && datacustdisc != 0)
            {
                var totalorderdiscs = (totalorders * datacustdisc) / 100;
                custdiscstr = totalorderdiscs.ToString();
                totalorders = totalorders - totalorderdiscs;
            }
            if (datasourcehdr.IS_DISC_PERC == "1")
            {
                decimal? disc = 0;

                if (datasourcehdr.INV_DISC != null)
                {
                    disc = datasourcehdr.INV_DISC;

                }
                decimal? totaldisc = (totalorders * disc) / 100;
                totalorders = totalorders - totaldisc;
                totalorder = totalorders.ToString();
                discounts = totaldisc.ToString();
            }
            else
            {
                decimal? disc = 0;
                if (datasourcehdr.INV_DISC_AMT != null)
                {
                    disc = datasourcehdr.INV_DISC_AMT;

                }
                totalorders = totalorders - disc;

                totalorder = totalorders.ToString();
                discounts = disc.ToString();
            }
            footer += "Subtotal: " + datasourcehdr.TOTAL_ORDER.ToString() + "\r\n";
            
            footer += "Customer Discount: " + custdiscstr + "\r\n";

            footer += "Discount: " + discounts + "\r\n";

            footer += "Total Order: " + totalorder;


            PdfFont fontFoot = new PdfStandardFont(PdfFontFamily.TimesRoman, 15);

            //Draw the text.

            graphics.DrawString(footer, fontFoot, PdfBrushes.Black, new Syncfusion.Drawing.PointF(1450, 170));


            var fileUrlInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Invoice");
            filename = "Invoice_"+ datasourcehdr.id.ToString()+ "_" + (DateTime.Now).ToString("ddMMyyyyHHmm") + ".pdf";
            var filesInv = Path.Combine(fileUrlInv, filename);
            var filePathInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Invoice");
            if (!Directory.Exists(filePathInv))
            {
                Directory.CreateDirectory(filePathInv);
            }
            FileStream fileStream = new FileStream(filesInv, FileMode.CreateNew, FileAccess.ReadWrite);
            //Save and close the PDF document 
            document.Save(fileStream);
            document.Close(true);
            fileStream.Dispose();
            return filename;
        }
        public string CreatePDFOrderConfirm(int id)
        {
            string filename = "";
            var datasourcehdr = db.SalesOrderTbl.Find(id);
            var datasourcedtl = db.SalesOrderDtlTbl.Where(y => y.id_order == datasourcehdr.id).ToList();
            var datacust = db.CustomerTbl.Find(datasourcehdr.id_customer);
            List<InvoiceDtl> invoicedtl = new List<InvoiceDtl>();
            foreach (var fld in datasourcedtl)
            {
                var row1 = new InvoiceDtl();
                row1.article = fld.article;
                var sizecodedt = getsizecodePdf(fld.article);
                row1.projectname = sizecodedt.projectname;
                row1.price = fld.price;
                row1.qty = fld.qty;
                row1.S1 = fld.Size_1.ToString();
                row1.S2 = fld.Size_2.ToString();
                row1.S3 = fld.Size_3.ToString();
                row1.S4 = fld.Size_4.ToString();
                row1.S5 = fld.Size_5.ToString();
                row1.S6 = fld.Size_6.ToString();
                row1.S7 = fld.Size_7.ToString();
                row1.S8 = fld.Size_8.ToString();
                row1.S9 = fld.Size_9.ToString();
                row1.S10 = fld.Size_10.ToString();
                row1.S11 = fld.Size_11.ToString();
                row1.S12 = fld.Size_12.ToString();
                row1.S13 = fld.Size_13.ToString();

                string disctype = "";
                if (fld.is_disc_perc == "1")
                {
                    disctype = "percentage";
                    row1.disc = fld.disc;
                }
                else
                {
                    disctype = "amount";
                    row1.disc = fld.disc_amt;

                }
                row1.disctype = disctype;
                row1.final = fld.final_price;

                invoicedtl.Add(row1);

            }
            //Creates a new PDF document
            PdfDocument document = new PdfDocument();
            //Adds page settings
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            document.PageSettings.Margins.All = 25;
            document.PageSettings.Size = PdfPageSize.A2;
            //Adds a page to the document
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");

            var files = Path.Combine(fileUrl, "Bata_red_small.png");
            FileStream imageStream = new FileStream(files, FileMode.Open, FileAccess.Read);
            Syncfusion.Drawing.RectangleF bounds = new Syncfusion.Drawing.RectangleF(700, 0, 250, 60);
            PdfImage image = PdfImage.FromStream(imageStream);
            PdfFont fonts = new PdfStandardFont(PdfFontFamily.TimesRoman, 15);

            //Draw the text.

            //Draws the image to the PDF page
            page.Graphics.DrawImage(image, bounds);
            graphics.DrawString("Size Guide:", fonts, PdfBrushes.Black, new Syncfusion.Drawing.PointF(220, 280));

            var fileUrlSize = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");
            var filesize = Path.Combine(fileUrl, "sizeguide.png");
            FileStream imageStream2 = new FileStream(filesize, FileMode.Open, FileAccess.Read);
            Syncfusion.Drawing.RectangleF bounds2 = new Syncfusion.Drawing.RectangleF(300, 280, 1000, 160);
            PdfImage image2 = PdfImage.FromStream(imageStream2);
            //Draws the image to the PDF page
            page.Graphics.DrawImage(image2, bounds2);

            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(232, 4, 20));
            bounds = new Syncfusion.Drawing.RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number
            PdfTextElement element = new PdfTextElement("Order No: " + id, subHeadingFont);
            element.Brush = PdfBrushes.White;
            //Draws the heading on the page
            PdfLayoutResult result = element.Draw(page, new Syncfusion.Drawing.PointF(10, bounds.Top + 8));

            string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            //Measures the width of the text to place it in the correct location
            Syncfusion.Drawing.SizeF textSize = subHeadingFont.MeasureString(currentDate);
            Syncfusion.Drawing.PointF textPosition = new Syncfusion.Drawing.PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //Draws the date by using DrawString method
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
            //Creates text elements to add the address and draw it to the page.
            string billto = "Bill To: " + "\r\n";
            billto += datacust.CUST_NAME + "\r\n";
            billto += datacust.address + "\r\n";
            billto += datacust.PHONE1 + "\r\n";
            element = new PdfTextElement(billto, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(0, 0, 0));
            result = element.Draw(page, new Syncfusion.Drawing.PointF(10, result.Bounds.Bottom + 25));
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            Syncfusion.Drawing.PointF startPoint = new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + 3);
            Syncfusion.Drawing.PointF endPoint = new Syncfusion.Drawing.PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);


            var invoiceDetails = invoicedtl;
            //Creates a PDF grid
            PdfGrid grid = new PdfGrid();
            //Adds the data source
            grid.DataSource = invoiceDetails;
            //Creates the grid cell styles
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(232, 4, 20));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);
            PdfGridCellStyle rowstyle = new PdfGridCellStyle();
            rowstyle.TextBrush = PdfBrushes.Black;
            rowstyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
            var test = grid.Rows.Count();
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                var cellcount = grid.Rows[i].Cells.Count;
                for (int x = 0; x < cellcount; x++)
                {
                    grid.Rows[i].Cells[x].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                    grid.Rows[i].Cells[x].Style = rowstyle;
                }
            }

            //Adds cell customizations
            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            }

            //Applies the header style
            header.ApplyStyle(headerStyle);
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            //Creates the layout format for grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            // Creates layout format settings to allow the table pagination
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new Syncfusion.Drawing.RectangleF(new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + 200), new Syncfusion.Drawing.SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            //Creates text elements to add the address and draw it to the page.
            string footer = " " + "\r\n";
            footer += "Total Qty: " + datasourcehdr.TOTAL_QTY + "\r\n";
            decimal? totalorders = datasourcehdr.TOTAL_ORDER;
            var edpcust = datacust.EDP;
            var getcreditnote = getcreditnotepdf(edpcust);
            totalorders = totalorders - getcreditnote;
            string totalorder = "";
            string discounts = "";
            var datacustdisc = datacust.discount_customer;
            var totalcustdiscstr = "0";
            if(datacustdisc != null && datacustdisc != 0)
            {
                var totalorderdiscs = (totalorders * datacustdisc) / 100;
                totalcustdiscstr = totalorderdiscs.ToString();
                totalorders = totalorders - totalorderdiscs;
            }
            if (datasourcehdr.IS_DISC_PERC == "1")
            {
                decimal? disc = 0;

                if (datasourcehdr.INV_DISC != null)
                {
                    disc = datasourcehdr.INV_DISC;

                }
                decimal? totaldisc = (totalorders * disc) / 100;
                totalorders = totalorders - totaldisc;
                totalorder = totalorders.ToString();
                discounts = totaldisc.ToString();
            }
            else
            {
                decimal? disc = 0;
                if (datasourcehdr.INV_DISC_AMT != null)
                {
                    disc = datasourcehdr.INV_DISC_AMT;

                }
                totalorders = totalorders - disc;

                totalorder = totalorders.ToString();
                discounts = disc.ToString();
            }
            footer += "Subtotal: " + datasourcehdr.TOTAL_ORDER.ToString() + "\r\n";
            footer += "Customer Discount: " + totalcustdiscstr + "\r\n";
            footer += "Discount: " + discounts + "\r\n";
            footer += "Total Order to pay: " + totalorder;


            PdfFont fontFoot = new PdfStandardFont(PdfFontFamily.TimesRoman, 15);

            //Draw the text.

            graphics.DrawString(footer, fontFoot, PdfBrushes.Black, new Syncfusion.Drawing.PointF(1450, 170));


            var fileUrlInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\OrderConfirm");
            filename = "Order_" + datasourcehdr.id.ToString() + "_" + (DateTime.Now).ToString("ddMMyyyyHHmm") + ".pdf";
            var filesInv = Path.Combine(fileUrlInv, filename);
            var filePathInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\OrderConfirm");
            if (!Directory.Exists(filePathInv))
            {
                Directory.CreateDirectory(filePathInv);
            }
            FileStream fileStream = new FileStream(filesInv, FileMode.CreateNew, FileAccess.ReadWrite);
            //Save and close the PDF document 
            document.Save(fileStream);
            document.Close(true);
            fileStream.Dispose();
            return filename;
        }

        public async void SendInvoiceMail(int orderid, string invno)
        {
            var datasourcehdr = db.SalesOrderTbl.Find(orderid);
            var datasourcedtl = db.SalesOrderDtlTbl.Where(y => y.id_order == datasourcehdr.id).ToList();
            var datacust = db.CustomerTbl.Find(datasourcehdr.id_customer);
            dbSalesOrder orderFld = new dbSalesOrder();
            orderFld = datasourcehdr;
            orderFld.salesOrderDtl = datasourcedtl;
            orderFld.picking_no = invno;
            string Email = datacust.Email.Trim();
            var getcreditnote = getcreditnotepdf(datacust.EDP);
            var totalorderaftercredit = datasourcehdr.TOTAL_ORDER - getcreditnote;
            decimal? custdiscval = 0;
            if (datacust.discount_customer != null)
            {
                custdiscval = datacust.discount_customer;
            }
            var custdiscs = (totalorderaftercredit * custdiscval) / 100;
            var request = new WelcomeRequest();
            request.UserName = Email;
            request.ToEmail = Email;
            request.salesorderTbl = orderFld;
            request.creditnoteval = getcreditnote;
            request.custdisc = custdiscs;
            var inv = CreatePDFProformInv(datasourcehdr.id, invno);
            request.fileinvoice = inv;
            //request.ToEmail = Input.Email;
           
            try
            {
                await mailService.SendInvoiceEmailAsync(request);
                //return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async void TestOrderConfirm()
        {
            int orderid = 24;
            var datasourcehdr = db.SalesOrderTbl.Find(orderid);
            var datasourcedtl = db.SalesOrderDtlTbl.Where(y => y.id_order == datasourcehdr.id).ToList();
            var datacust = db.CustomerTbl.Find(datasourcehdr.id_customer);
            dbSalesOrder orderFld = new dbSalesOrder();
            orderFld = datasourcehdr;
            orderFld.salesOrderDtl = datasourcedtl;
            orderFld.id = orderid;
            string Email = datacust.Email.Trim();
            var request = new WelcomeRequest();
            var getcreditnote = getcreditnotepdf(datacust.EDP);
            var totalorderaftercredit = datasourcehdr.TOTAL_ORDER - getcreditnote;
            decimal? custdiscval = 0;
            if (datacust.discount_customer != null)
            {
                custdiscval = datacust.discount_customer;
            }
            var custdiscs = (totalorderaftercredit * custdiscval) / 100;

            request.UserName = Email;
            request.ToEmail = Email;
            request.salesorderTbl = orderFld;
            request.creditnoteval = getcreditnote;
            request.custdisc = custdiscs;
            var inv = CreatePDFOrderConfirm(datasourcehdr.id);
            request.fileinvoice = inv;
            //request.ToEmail = Input.Email;

            try
            {
                await mailService.SendOrderConfirm(request);
                //return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async void SendOrderConfirm(int orderid)
        {
            var datasourcehdr = db.SalesOrderTbl.Find(orderid);
            var datasourcedtl = db.SalesOrderDtlTbl.Where(y => y.id_order == datasourcehdr.id).ToList();
            var datacust = db.CustomerTbl.Find(datasourcehdr.id_customer);
            dbSalesOrder orderFld = new dbSalesOrder();
            orderFld = datasourcehdr;
            orderFld.salesOrderDtl = datasourcedtl;
            orderFld.id = orderid;
            string Email = datacust.Email.Trim();
            var request = new WelcomeRequest();
            var getcreditnote = getcreditnotepdf(datacust.EDP);
            var totalorderaftercredit = datasourcehdr.TOTAL_ORDER - getcreditnote;
            decimal? custdiscval = 0;
            if(datacust.discount_customer != null)
            {
                custdiscval = datacust.discount_customer;
            }
            var custdiscs = (totalorderaftercredit * custdiscval) / 100;

            request.UserName = Email;
            request.ToEmail = Email;
            request.salesorderTbl = orderFld;
            request.creditnoteval = getcreditnote;
            request.custdisc = custdiscs;
            var inv = CreatePDFOrderConfirm(datasourcehdr.id);
            request.fileinvoice = inv;
            //request.ToEmail = Input.Email;

            try
            {
                await mailService.SendOrderConfirm(request);
                //return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async void InvoiceManual()
        {
            var datasourcehdr = db.SalesOrderTbl.Find(6);
            var datasourcedtl = db.SalesOrderDtlTbl.Where(y => y.id_order == datasourcehdr.id).ToList();
            var datacust = db.CustomerTbl.Find(datasourcehdr.id_customer);
            dbSalesOrder orderFld = new dbSalesOrder();
            orderFld = datasourcehdr;
            orderFld.salesOrderDtl = datasourcedtl;

            string Email = "customerbatatest@mail.com";
            var request = new WelcomeRequest();
            request.UserName = Email;
            request.ToEmail = Email;
            request.salesorderTbl = orderFld;
            var inv = CreatePDFProformInv(datasourcehdr.id, "");
            request.fileinvoice = inv;
            //request.ToEmail = Input.Email;

            try
            {
                await mailService.SendInvoiceEmailAsync(request);
                //return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string getinvno()
        {
            string invno = "";
            string ordernostr = "";
            int wk = getwk();
            int wkfromtbl = getmaxvalpickingwk();
            int wkassign = 0;
            int getpickingdefault = getmaxvalpickingorder(wkfromtbl);
            if (getwk() > getmaxvalpickingwk())
            {
                wkassign = wk;
                truncatepicking();
            }
            else
             if (getwk() == getmaxvalpickingwk() && getpickingdefault == 999)
            {
                truncatepicking();
                wkassign = wk + 1;
            }
            else
            if (wk < wkfromtbl)
            {
                wkassign = wk + 1;
            }
            else
            {
                wkassign = wk;
            }
            insertpicking();
            int getpickingafter = getpicking();

            if (getpickingafter.ToString().Length == 1)
            {
                ordernostr = "00" + getpickingafter.ToString();
            }else
            if (getpickingafter.ToString().Length == 2)
            {
                ordernostr = "0" + getpickingafter.ToString();

            }
            else
            {
                ordernostr = getpickingafter.ToString();
            }
            invno = wkassign + "-" + ordernostr;
            return invno;
        }
        public int getmaxvalpickingwk()
        {
            int week = 0;
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select max(left(picking_no,2)) as maxwk from dbsalesorder d where picking_no is not null";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        week = Convert.ToInt32(reader["maxwk"]);

                    }
                }
                conn.Close();
            }
            return week;
        }
        public int getmaxvalpickingorder(int wk)
        {
            int orderno = 0;
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
	                                max(right(picking_no, 3)) as maxorder
                                from
	                                dbsalesorder d
                                where
	                                 left(picking_no, 2) = "+wk+"";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderno = Convert.ToInt32(reader["maxorder"]);

                    }
                }
                conn.Close();
            }
            return orderno;
        }

        public void truncatepicking()
        {
            int week = 0;
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"truncate table dbsalesorderpicking";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void insertpicking()
        {
            int week = 0;
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"insert into dbsalesorderpicking (id) values (null);";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public int getpicking()
        {
            int id = 0;
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"SELECT id FROM batahrdb.dbsalesorderpicking order by id desc limit 1";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["id"]);

                    }
                }
                conn.Close();
            }
            return id;
        }


    }
}
