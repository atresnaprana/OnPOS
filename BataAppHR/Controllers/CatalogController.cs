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
    public class CatalogController : Controller
    {
        private readonly FormDBContext db;

        private IHostingEnvironment Environment;
        private readonly ILogger<SalesOrderController> _logger;
        public const string SessionKeyName = "FormList";
        public const string SessionKeyNameEdit = "FormListCart";
        public const string SessionKeyNameFilter = "TypeCode";
        private readonly IMailService mailService;

        public CatalogController(FormDBContext db, ILogger<CatalogController> logger, IHostingEnvironment _environment,
            IConfiguration configuration, IMailService mailService)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
            this.mailService = mailService;

        }
        public IConfiguration Configuration { get; }
        [Authorize(Roles = "CustomerIndustrial")]

        public IActionResult Index()
        {
            var fld = new dbSalesOrder();
            //List<Article> list = new List<Article>();

            //string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            //using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            //{
            //    conn.Open();
            //    MySqlCommand cmd = new MySqlCommand("select dz.dmram_article, dz.dmram_project_name from dm_zzram dz ", conn);

            //    using (var reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            list.Add(new Article()
            //            {
            //                articlecode = reader["dmram_article"].ToString(),
            //                articlename = reader["dmram_article"].ToString() + " - " + reader["dmram_project_name"].ToString()
            //            });
            //        }
            //    }
            //    conn.Close();
            //}
            var articleList = db.ArticleTbl.ToList().Select(y => new Article()
            {
                
                articlecode = y.Article,
                articlename = y.Article + " - " + getdataarticlename(y.Article)
               
            }).ToList();
            fld.articleList = articleList;
            return View(fld);
        }
        public IActionResult CatalogView()
        {
            string page = HttpContext.Request.Query["Category"].ToString();
            string page2 = HttpContext.Request.Query["Brand"].ToString();
            string article = HttpContext.Request.Query["article"].ToString();
            string isfirstopen = HttpContext.Request.Query["isfirstopen"].ToString();

            ViewBag.isfirstopen = isfirstopen;
            //article = "5246606";
            var salesorderfld = new dbSalesOrder();
            var dtrm55 = getdatarm55();
            var articleList = db.ArticleTbl.ToList().Select(y => new dbArticle()
            {
                Article = y.Article,
                pricestring = getdataprice(y.Article),
                isAvail = GetAvail(dtrm55, y.Article),
                qty = GetStock(dtrm55, y.Article),
                FILE_IMG = y.FILE_IMG,
                FILE_IMG_NAME = y.FILE_IMG_NAME,
                projectname = getdataarticlename(y.Article)
            }).ToList();
            if (!string.IsNullOrEmpty(article))
            {
                articleList = articleList.Where(y => y.Article == article).ToList();
            }
            salesorderfld.articleListTrans = articleList;
           
            return PartialView(salesorderfld);
        }
        public string GetAvail(List<RM55Model> dtrm55, string article)
        {
            string avail = "0";
            var fld = dtrm55.Where(y => y.Article == article).FirstOrDefault();
            if(fld != null)
            {
                var dctot = Convert.ToInt32(fld.DC_TOT);
                if(dctot > 0)
                {
                    avail = "1";
                }
            }
            return avail;
        }
        public string GetStock(List<RM55Model> dtrm55, string article)
        {
            string avail = "0";
            var fld = dtrm55.Where(y => y.Article == article).FirstOrDefault();
            if (fld != null)
            {
                var dctot = Convert.ToInt32(fld.DC_TOT);
                if (dctot > 0)
                {
                    avail = dctot.ToString();
                }
            }
            return avail;
        }

        public IActionResult getstockpersize(string article)
        {
            ArticleSize dtstock = new ArticleSize();
            var rm55dt = getdatarm55().Where(y => y.Article == article).FirstOrDefault();
            dtstock.size1 = rm55dt.S1;
            dtstock.size2 = rm55dt.S2;
            dtstock.size3 = rm55dt.S3;
            dtstock.size4 = rm55dt.S4;
            dtstock.size5 = rm55dt.S5;
            dtstock.size6 = rm55dt.S6;
            dtstock.size7 = rm55dt.S7;
            dtstock.size8 = rm55dt.S8;
            dtstock.size9 = rm55dt.S9;
            dtstock.size10 = rm55dt.S10;
            dtstock.size11 = rm55dt.S11;
            dtstock.size12 = rm55dt.S12;
            dtstock.size13 = rm55dt.S13;
            return Json(dtstock);
        }

        public string getdataprice(string article)
        {
            ArticleSize pricedt = new ArticleSize();

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
                        pricedt.articleprice = Convert.ToInt32(reader["price"]);

                    }
                }
                conn.Close();
            }
            return pricedt.articleprice.ToString();
        }

        public List<RM55Model> getdatarm55()
        {
            List<RM55Model> rm55dt = new List<RM55Model>();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
                                    Batch,
	                                Comp,
	                                Article,
	                                Cat,
	                                Seq,
	                                DC_TOT,
	                                S1,
	                                S2,
	                                S3,
	                                S4,
	                                S5,
	                                S6,
	                                S7,
	                                S8,
	                                S9,
	                                S10,
	                                S11,
	                                S12,
	                                S13,
	                                Rec,
	                                Price,
	                                Cost,
	                                Column22
                                from
                                    data_mart.dm_rm55
                where Comp = '650J'";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RM55Model fld = new RM55Model();
                        fld.Batch = reader["Batch"].ToString();
                        fld.Comp = reader["Comp"].ToString();
                        fld.Article = reader["Article"].ToString();
                        fld.Cat = reader["Cat"].ToString();
                        fld.Seq = Convert.ToInt32(reader["Seq"]);
                        fld.DC_TOT = reader["DC_TOT"].ToString();
                        fld.S1 = reader["S1"].ToString();
                        fld.S2 = reader["S2"].ToString();
                        fld.S3 = reader["S3"].ToString();
                        fld.S4 = reader["S4"].ToString();
                        fld.S5 = reader["S5"].ToString();
                        fld.S6 = reader["S6"].ToString();
                        fld.S7 = reader["S7"].ToString();
                        fld.S8 = reader["S8"].ToString();
                        fld.S9 = reader["S9"].ToString();
                        fld.S10 = reader["S10"].ToString();
                        fld.S11 = reader["S11"].ToString();
                        fld.S12 = reader["S12"].ToString();
                        fld.S13 = reader["S13"].ToString();
                        fld.Rec = reader["Rec"].ToString();
                        fld.Price = reader["Price"].ToString();
                        fld.Cost = reader["Cost"].ToString();
                        fld.Ext = reader["Column22"].ToString();
                        rm55dt.Add(fld);
                    }
                }
                conn.Close();
            }
            return rm55dt;
        }
        public string getdataarticlename(string article)
        {
            ArticleSize pricedt = new ArticleSize();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select dz.dmram_project_name as name from dm_zzram dz 
                                                    where  dz.dmram_article = '" + article + "'";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pricedt.projectname = reader["name"].ToString();

                    }
                }
                conn.Close();
            }
            return pricedt.projectname.ToString();
        }
        public IActionResult Catalog()
        {
            return View();
        }
        public IActionResult testCarousel()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([Bind] dbSalesOrder objDetail)
        {
            if (ModelState.IsValid)
            {
                //List<dbSalesOrderDtlTemp> orderdtltbl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtlTemp>>(HttpContext.Session, "FormList");
                //if (orderdtltbl == null)
                //{
                //orderdtltbl = new List<dbSalesOrderDtlTemp>();
                //SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderdtltbl);
                //}
               

                dbSalesOrderDtlTemp fld = new dbSalesOrderDtlTemp();
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
                var chkinv = db.SalesOrderTempTbl.Where(Y => Y.EMAIL == User.Identity.Name).FirstOrDefault();
                var newFld = new dbSalesOrderTemp();
                if(chkinv != null)
                {
                    newFld = chkinv;
                    var listcart = db.SalesOrderDtlTempTbl.Where(y => y.id_order == newFld.id).ToList();
                    var totalqtydb = listcart.Sum(y => y.qty);
                    var totalorderdb = listcart.Sum(y => y.final_price);
                    totalqtydb = fld.qty + totalqtydb;
                    totalorderdb = totalorderdb + totalorder;
                    newFld.TOTAL_QTY = totalqtydb;

                    var customerDt = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                    if (customerDt != null)
                    {
                        if (customerDt.discount_customer != null)
                        {
                            if (customerDt.discount_customer != 0)
                            {
                                var valdisc = (totalorderdb * customerDt.discount_customer) / 100;
                                newFld.TOTAL_ORDER = totalorderdb - valdisc;
                            }
                            else
                            {
                                newFld.TOTAL_ORDER = totalorderdb;
                            }
                        }
                        else
                        {
                            newFld.TOTAL_ORDER = totalorderdb;
                        }
                    }
                    else
                    {
                        newFld.TOTAL_ORDER = totalorderdb;
                    }
                    //newFld.TOTAL_ORDER = newFld.TOTAL_ORDER + totalorder;
                    //newFld.TOTAL_QTY = newFld.TOTAL_QTY + fld.qty;
                }
                else
                {
                    newFld.EMP_CODE = "5490009";
                    var customerDt = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                    if (customerDt != null)
                    {
                        newFld.id_customer = customerDt.id;
                    }
                    newFld.EMAIL = User.Identity.Name;
                    newFld.ORDER_DATE = DateTime.Now;
                    newFld.APPROVAL_1 = "0";
                    newFld.APPROVAL_2 = "0";

                    newFld.IS_DISC_PERC = "0";
                    newFld.INV_DISC_AMT = 0;
                    newFld.INV_DISC = 0;
                    if(customerDt != null)
                    {
                        if(customerDt.discount_customer != null)
                        {
                            if(customerDt.discount_customer != 0)
                            {
                                var valdisc = (totalorder * customerDt.discount_customer) / 100;
                                newFld.TOTAL_ORDER = totalorder - valdisc;
                            }
                            else
                            {
                                newFld.TOTAL_ORDER = totalorder;
                            }
                        }
                        else
                        {
                            newFld.TOTAL_ORDER = totalorder;
                        }
                    }
                    else
                    {
                        newFld.TOTAL_ORDER = totalorder;
                    }
                    newFld.TOTAL_QTY = fld.qty;
                    newFld.UPDATE_DATE = DateTime.Now;
                    newFld.UPDATE_USER = User.Identity.Name;
                    newFld.SHIPPING_ADDRESS = "";
                    db.SalesOrderTempTbl.Add(newFld);
                    db.SaveChanges();
                }
                var idhdr = newFld.id;
                fld.id_order = idhdr;
                try
                {
                    var validatedata = db.SalesOrderDtlTempTbl.Where(y => y.article == fld.article && y.id_order == idhdr).FirstOrDefault();
                    if(validatedata != null)
                    {
                        validatedata.qty = fld.qty;
                        validatedata.final_price = fld.final_price;
                        validatedata.Size_1 = fld.Size_1;
                        validatedata.Size_2 = fld.Size_2;
                        validatedata.Size_3 = fld.Size_3;
                        validatedata.Size_4 = fld.Size_4;
                        validatedata.Size_5 = fld.Size_5;
                        validatedata.Size_6 = fld.Size_6;
                        validatedata.Size_7 = fld.Size_7;
                        validatedata.Size_8 = fld.Size_8;
                        validatedata.Size_9 = fld.Size_9;
                        validatedata.Size_10 = fld.Size_10;
                        validatedata.Size_11 = fld.Size_11;
                        validatedata.Size_12 = fld.Size_12;
                        validatedata.Size_13 = fld.Size_13;

                    }
                    else
                    {
                        db.SalesOrderDtlTempTbl.Add(fld);
                    }
                    db.SaveChanges();
                    //orderdtltbl.Add(fld);

                    //objDetail.EMP_CODE = objDetail.EMP_CODETemp;
                    //objDetail.id_customer = objDetail.id_customerTemp;
                    //objDetail.ORDER_DATE = objDetail.ORDER_DATETemp;
                    //objDetail.APPROVAL_1 = objDetail.APPROVAL_1Temp;
                    //objDetail.APPROVAL_2 = objDetail.APPROVAL_2Temp;
                    //objDetail.TOTAL_ORDER = orderdtltbl.Sum(y => y.final_price);
                    //objDetail.TOTAL_QTY = orderdtltbl.Sum(y => y.qty);
                    //objDetail.INV_DISC = objDetail.INV_DISCTemp;
                    //objDetail.INV_DISC_AMT = objDetail.INV_DISC_AMTTemp;
                    //objDetail.IS_DISC_PERC = objDetail.IS_DISC_PERCTemp;
                    //objDetail.custdisc = objDetail.custdiscTemp;
                    //objDetail.SHIPPING_ADDRESS = objDetail.SHIPPING_ADDRESSTemp;
                    //objDetail.isPassed = true;
                    //SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", orderdtltbl);

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
                return RedirectToAction("Index", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Index", objDetail);
            }
        }

        
        public JsonResult getOrderDataEdit(string article)
        {
            var hdr = db.SalesOrderTempTbl.Where(y => y.EMAIL == User.Identity.Name).FirstOrDefault ();

            //Creating List    
            List<dbSalesOrderDtlTemp> salesorderDtl = db.SalesOrderDtlTempTbl.ToList();
            dbSalesOrderDtlTemp fld = new dbSalesOrderDtlTemp();

            if (hdr != null)
            {
                var data = salesorderDtl.Where(y => y.article == article && y.id_order == hdr.id).FirstOrDefault();
                if (data != null)
                {
                    fld = data;
                }

            }
            return Json(fld);
        }
        [Authorize(Roles = "CustomerIndustrial")]
        public IActionResult ViewItems(dbSalesOrderTemp objPassed)
        {
            dbSalesOrderTemp fld = db.SalesOrderTempTbl.Where(y => y.EMAIL == User.Identity.Name).FirstOrDefault();
            if(fld == null)
            {
                fld = new dbSalesOrderTemp();
            }
            else
            {
                if (objPassed.isPassed)
                {
                    fld.TOTAL_ORDER = objPassed.TOTAL_ORDER;
                    fld.TOTAL_QTY = objPassed.TOTAL_QTY;
                   
                }
               
            }
            decimal? custdisc = 0;
            var datacust = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            var datacredit = db.creditlogtbl.Where(y => y.edp == datacust.EDP && y.isused == "0").ToList();
            if(datacredit.Count() != 0)
            {
                fld.isHaveCredit = true;
                fld.creditnoteval = datacredit.Sum(y => y.valuecredit);
                if(fld.TOTAL_ORDER != 0 && fld.TOTAL_ORDER != null)
                {
                    fld.TOTAL_ORDER = fld.TOTAL_ORDER - fld.creditnoteval;
                }
            }
            else
            {
                fld.isHaveCredit = false;
                fld.creditnoteval = 0;
            }
            if (datacust != null)
            {
                if(datacust.discount_customer != null)
                {
                    custdisc = datacust.discount_customer;
                }
            }
            fld.custdisc = custdisc;
            List<dbSalesOrderDtlTemp> p = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtlTemp>>(HttpContext.Session, "FormListCart");
            p = db.SalesOrderDtlTempTbl.Where(y => y.id_order == fld.id).ToList();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListCart", p);
            //if (fld == null)
            ViewBag.countItem = p.Count();
            return View(fld);
        }
        public JsonResult getTblDtlEdit()
        {
            var datahdr = db.SalesOrderTempTbl.Where(y => y.EMAIL == User.Identity.Name).FirstOrDefault();
            //Creating List
            List<dbSalesOrderDtlTemp> ordertbl = new List<dbSalesOrderDtlTemp>();
            if (datahdr != null){
                ordertbl = db.SalesOrderDtlTempTbl.Where(y => y.id_order == datahdr.id).ToList();
            }
            return Json(ordertbl);
        }

        public JsonResult getOrderDataEdit2(string article)
        {
            //Creating List    
            List<dbSalesOrderDtlTemp> salesorderDtl = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtlTemp>>(HttpContext.Session, "FormListCart");
            dbSalesOrderDtlTemp fld = new dbSalesOrderDtlTemp();
            if (salesorderDtl != null)
            {
                fld = salesorderDtl.Where(y => y.article == article).FirstOrDefault();
            }
            return Json(fld);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCart([Bind] dbSalesOrderTemp objDetail)
        {
            if (ModelState.IsValid)
            {
                var hdr = db.SalesOrderTempTbl.Where(y => y.EMAIL == User.Identity.Name).FirstOrDefault();
                var id = 0;
                if(hdr != null)
                {
                    id = hdr.id;
                }
                List<dbSalesOrderDtlTemp> orderdata = db.SalesOrderDtlTempTbl.Where(y => y.id_order == id).ToList();
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
                            db.SaveChanges();
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
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListCart", orderdata);


                }
                else
                {
                    //List<dbSalesOrderDtl> orderdata = SessionHelper.GetObjectFromJson<List<dbSalesOrderDtl>>(HttpContext.Session, "FormListEdit");
                    var fld = orderdata.Where(y => y.article == objDetail.articleEdit).FirstOrDefault();
                    orderdata.Remove(fld);
                    db.SalesOrderDtlTempTbl.Remove(fld);
                    db.SaveChanges();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListCart", orderdata);

                }
                objDetail.TOTAL_ORDER = orderdata.Sum(y => y.final_price);
                objDetail.TOTAL_QTY = orderdata.Sum(y => y.qty);
                objDetail.custdisc = objDetail.custdiscTempEdit;
                objDetail.isPassed = true;
                return RedirectToAction("ViewItems", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("ViewItems", objDetail);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckoutItems([Bind] dbSalesOrderTemp fld)
        {

            if (ModelState.IsValid)
            {
                var editFld = db.SalesOrderTempTbl.Where(y => y.EMAIL == User.Identity.Name).FirstOrDefault();
                editFld.SHIPPING_ADDRESS = fld.SHIPPING_ADDRESS;
                //editFld.TOTAL_ORDER = fld.TOTAL_ORDER;
                //editFld.TOTAL_QTY = fld.TOTAL_QTY;
                dbSalesOrder newOrder = new dbSalesOrder();
                newOrder.id_customer = editFld.id_customer;
                newOrder.EMP_CODE = editFld.EMP_CODE;
                newOrder.TOTAL_QTY = editFld.TOTAL_QTY;
                newOrder.FLAG_AKTIF = "1";
                newOrder.ORDER_DATE = editFld.ORDER_DATE;
                newOrder.ENTRY_DATE = DateTime.Now;
                newOrder.UPDATE_DATE = DateTime.Now;
                newOrder.ENTRY_USER = User.Identity.Name;
                newOrder.UPDATE_USER = User.Identity.Name;
                newOrder.EMAIL = editFld.EMAIL;
                newOrder.SHIPPING_ADDRESS = editFld.SHIPPING_ADDRESS;
                newOrder.INV_DISC = 0;
                newOrder.IS_DISC_PERC = "0";
                newOrder.INV_DISC_AMT = 0;
                List<dbSalesOrderDtlTemp> ordertbl = db.SalesOrderDtlTempTbl.Where(y => y.id_order == editFld.id).ToList();
                newOrder.TOTAL_ORDER = ordertbl.Sum(y => y.final_price);
                db.SalesOrderTbl.Add(newOrder);
                db.SaveChanges();
                try
                {
                  foreach(var dts in ordertbl)
                    {
                        dbSalesOrderDtl formtrans = new dbSalesOrderDtl();
                        formtrans.id_order = newOrder.id;
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
                        var finalval = totalorder;
                        formtrans.final_price = Math.Round(Convert.ToDecimal(finalval), 2);
                        db.SalesOrderDtlTbl.Add(formtrans);
                        db.SalesOrderDtlTempTbl.Remove(dts);
                    }
                    db.SalesOrderTempTbl.Remove(editFld);
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
        [Authorize(Roles = "CustomerIndustrial")]
        public IActionResult PaymentPage()
        {
            SalesFront newfld = new SalesFront();
            var salesdata = db.SalesOrderTbl.Where(y => y.EMAIL == User.Identity.Name && y.APPROVAL_1 == "1" && y.APPROVAL_2 == "0" || y.APPROVAL_1 == "1" && string.IsNullOrEmpty(y.APPROVAL_2)).ToList().Select(y => new dbSalesOrder()
            {
                ORDER_DATE = y.ORDER_DATE,
                id = y.id,
                EMP_CODE = getEmpName(y.EMP_CODE).NM_EMP,
                TOTAL_ORDER = gettotalordernett(y.id),
                TOTAL_QTY = y.TOTAL_QTY,
                picking_no = y.picking_no,
                EMAIL = y.EMAIL
            }).ToList();
            newfld.salesOrdeData = salesdata;
            return View(newfld);
        }
        public dbEmployee getEmpName(string empcode)
        {
            string empname = "";
            var dbemp = db.EmpTbl.Where(y => y.EMP_CODE == empcode).FirstOrDefault();
            if (dbemp != null)
            {
                empname = dbemp.NM_EMP;
            }
            return dbemp;
        }

        public decimal? getcreditnotepdf(string edp)
        {
            dbCustomer custdt = new dbCustomer();

            string mySqlConnectionStr = Configuration.GetConnectionString("ConnDataMart");
            var dt = db.creditlogtbl.Where(y => y.edp == edp && y.isused == "0").ToList();
            var totalcredit = dt.Sum(y => y.valuecredit);
            return totalcredit;
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
            if (custdt != null)
            {
                if (custdt.discount_customer != null)
                {
                    discountcust = custdt.discount_customer;
                }
            }
            if (dbtotalorder != null)
            {
                totalorder = dbtotalorder;
                totalorder = totalorder - getcreditnote;
                var discountcustval = (totalorder * discountcust) / 100;
                totalorder = totalorder - discountcustval;
            }
            var isdiscperc = dbsalesorder.IS_DISC_PERC;
            decimal? invdisc = 0;
            if (isdiscperc == "1")
            {
                var invvaldb = dbsalesorder.INV_DISC;
                if (invvaldb != null)
                {
                    invdisc = (totalorder * invvaldb) / 100;
                }
            }
            else
            {
                var invvaldb = dbsalesorder.INV_DISC_AMT;
                if (invvaldb != null)
                {
                    invdisc = invvaldb;
                }
            }
            totalorder = totalorder - invdisc;
            return totalorder;
        }

        [Authorize(Roles = "CustomerIndustrial")]
        public IActionResult PaymentForm(int? id)
        {
            var fld = new dbPaymentList();
            fld.id_orderCust = Convert.ToInt32(id);
            var salesdata = db.SalesOrderTbl.Where(y => y.EMAIL == User.Identity.Name && y.APPROVAL_1 == "1" && y.APPROVAL_2 == "0" || y.APPROVAL_1 == "1" && string.IsNullOrEmpty(y.APPROVAL_2)).ToList().Select(y => new dbSalesOrder()
            {
                ORDER_DATE = y.ORDER_DATE,
                id = y.id,
                EMP_CODE = getEmpName(y.EMP_CODE).NM_EMP,
                TOTAL_ORDER = gettotalordernett(y.id),
                TOTAL_QTY = y.TOTAL_QTY,
                picking_no = y.picking_no,
                EMAIL = y.EMAIL
            }).Where(y => y.id == id).FirstOrDefault();
            fld.TOTAL_PAY = salesdata.TOTAL_ORDER;
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PaymentForm([Bind] dbPaymentList obj)
        {
            if (ModelState.IsValid)
            {
                obj.id = 0;
                obj.id_order = obj.id_orderCust;
                var datasales = db.SalesOrderTbl.Find(obj.id_orderCust);
                obj.id_customer = Convert.ToInt32(datasales.id_customer);
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
                return RedirectToAction("PaymentPage");
            }
            return View(obj);
        }
    }
}
