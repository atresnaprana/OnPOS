using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BataAppHR.Data;
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
using OnPOS.Models;
using static System.Net.WebRequestMethods;
using OfficeOpenXml;
using BataAppHR.Models;
using MimeDetective.Storage.Xml.v2;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;
using System.ComponentModel.DataAnnotations;
using Syncfusion.Pdf.Security;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace OnPOS.Controllers
{
    public class POSController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<POSController> _logger;
        public const string SessionKeyName = "FormList";
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }


        public POSController(FormDBContext db, ILogger<POSController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;

        }
        public IActionResult Index()
        {
            dbSalesHdr fld = new dbSalesHdr();
            dbStoreList storedt = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();
            List<dbStoreStockModel> stocktbl = db.StockTbl.Where(y => y.storeid == storedt.id).ToList();
            List<dbSalesStaff> stafftbl = db.SalesStaffTbl.Where(y=> y.STORE_ID == storedt.id).ToList();
            var existingsales = db.saleshdrtbl.Where(y => y.Store_id == storedt.id).ToList();
            //&& y.invoice.Substring(0, 6) == DateTime.Now.ToString("ddMMyy")
            var number = "0001";
            var invdata = existingsales.Select(y => new dbSalesHdr()
            {
              invshort = y.invoice.Substring(0, 6),
              lastinvoice = Convert.ToInt32(y.invoice.Substring(6, 4))


            }).ToList();
            var checkinvoicecurrent = invdata.Where(y => y.invshort == DateTime.Now.ToString("ddMMyy")).ToList();

            if (checkinvoicecurrent.Count > 0)
            {
                var chkinvnum = checkinvoicecurrent.Max(y => y.lastinvoice) + 1;
                if (chkinvnum.ToString().Length == 1)
                {
                    number = "000" + chkinvnum.ToString();
                }
                else if (chkinvnum.ToString().Length == 2)
                {
                    number = "00" + chkinvnum.ToString();
                }
                else if (chkinvnum.ToString().Length == 3)
                {
                    number = "0" + chkinvnum.ToString();
                }
                else if (chkinvnum.ToString().Length == 4)
                {
                    number = chkinvnum.ToString();
                }
            }
            fld.username = storedt.STORE_MANAGER_NAME;
            fld.invoice = DateTime.Now.ToString("ddMMyy") + number;
            var dddata = stafftbl.Select(y => new DropDownModel()
            {
                id = y.id.ToString(),
                name = y.id.ToString() + " - " + y.SALES_NAME,
                

            }).ToList();
            fld.ddStaff = dddata;
            fld.scanqty = 1;
            fld.Store_id = storedt.id;
            fld.storedata = storedt;
            List<dbSalesDtl> salesdtl = SessionHelper.GetObjectFromJson<List<dbSalesDtl>>(HttpContext.Session, "FormList");
            if (salesdtl == null)
            {
                salesdtl = new List<dbSalesDtl>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", salesdtl);
            }
            return View(fld);
        }
        public JsonResult getstockinfo(string scanitem, string Store_id)
        {
            string scanitemshort = scanitem.Substring(0, 14);

            var getstockdata = db.StockTbl.Where(y => y.storeid == Convert.ToInt32(Store_id) && y.itemid == scanitemshort && y.bucket_id == "ON_HAND").FirstOrDefault();
            return Json(getstockdata);
        }
        public JsonResult validateitems(string scanitem)
        {
            string scanitemshort = scanitem.Substring(0, 14);
            var getdiscount = db.DiscTbl.Where(y => y.article == scanitemshort && y.status == "1" && DateTime.Now >= y.validfrom && DateTime.Now < y.validto).FirstOrDefault();

            var getstockdata = db.ItemMasterTbl.Where(y => y.itemid == scanitemshort).FirstOrDefault();
            if(getdiscount != null)
            {
                if(getdiscount.isallstore == "Y")
                {
                    getstockdata.disctype = getdiscount.type;
                    getstockdata.discamt = getdiscount.amount;
                    getstockdata.discperc = getdiscount.percentage;
                }
                else
                {
                    dbStoreList storedt = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();

                    var checkstoreislist = db.StoreDiscTbl.Where(y => y.storeid == storedt.id && y.COMPANY_ID == storedt.COMPANY_ID && y.promoid == getdiscount.id).FirstOrDefault();
                    if(checkstoreislist != null)
                    {
                        getstockdata.disctype = getdiscount.type;
                        getstockdata.discamt = getdiscount.amount;
                        getstockdata.discperc = getdiscount.percentage;
                    }
                }
               
            }
            

            return Json(getstockdata);
        }
        public JsonResult SaveTemp(string salesid, string item, string total, string discount, string subtotal)
        {

            string stat = "ok";
            string chkval = salesid + "_" + item +"_" + total + "_" + discount + "_" + subtotal;
            string article = item.Substring(0, 14);
            string size = item.Substring(14, 2);
            
            List<dbSalesDtl> salesdtl = SessionHelper.GetObjectFromJson<List<dbSalesDtl>>(HttpContext.Session, "FormList");
            dbStoreList storedt = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();
            var getstockdata = db.ItemMasterTbl.Where(y => y.itemid == article).FirstOrDefault();
            var validate = salesdtl.Where(y => y.article == article && y.size == size).ToList().Count();
            if(validate == 0)
            {
                dbSalesDtl fld = new dbSalesDtl();
                fld.store_id = storedt.id;
                fld.staff_id = Convert.ToInt32(salesid);
                fld.article = article;
                fld.size = size;
                fld.qty = Convert.ToInt32(total);
                fld.cat = getstockdata.category;
                fld.subcat = getstockdata.subcategory;
                var getdiscount = db.DiscTbl.Where(y => y.article == article && y.status == "1" && DateTime.Now >= y.validfrom && DateTime.Now < y.validto).FirstOrDefault();
                if (getdiscount != null)
                {
                    if (getdiscount.isallstore == "Y")
                    {
                        fld.discountcode = getdiscount.id;
                        fld.disc_amount = getdiscount.amount;
                        fld.disc_prc = getdiscount.percentage;

                    }
                    else
                    {

                        var checkstoreislist = db.StoreDiscTbl.Where(y => y.storeid == storedt.id && y.COMPANY_ID == storedt.COMPANY_ID && y.promoid == getdiscount.id).FirstOrDefault();
                        if (checkstoreislist != null)
                        {
                            fld.discountcode = getdiscount.id;
                            fld.disc_amount = getdiscount.amount;
                            fld.disc_prc = getdiscount.percentage;

                        }
                    }

                }
                fld.price = Convert.ToInt32(subtotal);
                if (size == "33")
                {
                    fld.s33 = Convert.ToInt32(total);
                }
                else
                if (size == "34")
                {
                    fld.s34 = Convert.ToInt32(total);
                }
                else
                if (size == "35")
                {
                    fld.s35 = Convert.ToInt32(total);
                }
                else
                if (size == "36")
                {
                    fld.s36 = Convert.ToInt32(total);
                }
                else
                if (size == "37")
                {
                    fld.s37 = Convert.ToInt32(total);
                }
                else
                if (size == "38")
                {
                    fld.s38 = Convert.ToInt32(total);
                }
                else
                if (size == "39")
                {
                    fld.s39 = Convert.ToInt32(total);

                }
                else
                if (size == "40")
                {
                    fld.s40 = Convert.ToInt32(total);

                }
                else
                if (size == "41")
                {
                    fld.s41 = Convert.ToInt32(total);

                }
                else
                if (size == "42")
                {
                    fld.s42 = Convert.ToInt32(total);

                }
                else
                if (size == "43")
                {
                    fld.s43 = Convert.ToInt32(total);

                }
                else
                if (size == "44")
                {
                    fld.s44 = Convert.ToInt32(total);

                }
                else
                if (size == "45")
                {
                    fld.s45 = Convert.ToInt32(total);

                }
                else
                if (size == "46")
                {
                    fld.s46 = Convert.ToInt32(total);

                }
                salesdtl.Add(fld);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", salesdtl);

            }
            else
            {
                dbSalesDtl fld = salesdtl.Where(y => y.article == article && y.size == size).FirstOrDefault();
                fld.store_id = storedt.id;
                fld.staff_id = Convert.ToInt32(salesid);
                fld.article = article;
                fld.size = size;
                fld.qty = Convert.ToInt32(total);
                fld.cat = getstockdata.category;
                fld.subcat = getstockdata.subcategory;
                var getdiscount = db.DiscTbl.Where(y => y.article == article && y.status == "1" && DateTime.Now >= y.validfrom && DateTime.Now < y.validto).FirstOrDefault();
                if (getdiscount != null)
                {
                    if (getdiscount.isallstore == "Y")
                    {
                        fld.discountcode = getdiscount.id;
                        fld.disc_amount = getdiscount.amount;
                        fld.disc_prc = getdiscount.percentage;

                    }
                    else
                    {

                        var checkstoreislist = db.StoreDiscTbl.Where(y => y.storeid == storedt.id && y.COMPANY_ID == storedt.COMPANY_ID && y.promoid == getdiscount.id).FirstOrDefault();
                        if (checkstoreislist != null)
                        {
                            fld.discountcode = getdiscount.id;
                            fld.disc_amount = getdiscount.amount;
                            fld.disc_prc = getdiscount.percentage;

                        }
                    }

                }
                fld.price = Convert.ToInt32(subtotal);
                if (size == "33")
                {
                    fld.s33 = Convert.ToInt32(total);
                }
                else
                if (size == "34")
                {
                    fld.s34 = Convert.ToInt32(total);
                }
                else
                if (size == "35")
                {
                    fld.s35 = Convert.ToInt32(total);
                }
                else
                if (size == "36")
                {
                    fld.s36 = Convert.ToInt32(total);
                }
                else
                if (size == "37")
                {
                    fld.s37 = Convert.ToInt32(total);
                }
                else
                if (size == "38")
                {
                    fld.s38 = Convert.ToInt32(total);
                }
                else
                if (size == "39")
                {
                    fld.s39 = Convert.ToInt32(total);

                }
                else
                if (size == "40")
                {
                    fld.s40 = Convert.ToInt32(total);

                }
                else
                if (size == "41")
                {
                    fld.s41 = Convert.ToInt32(total);

                }
                else
                if (size == "42")
                {
                    fld.s42 = Convert.ToInt32(total);

                }
                else
                if (size == "43")
                {
                    fld.s43 = Convert.ToInt32(total);

                }
                else
                if (size == "44")
                {
                    fld.s44 = Convert.ToInt32(total);

                }
                else
                if (size == "45")
                {
                    fld.s45 = Convert.ToInt32(total);

                }
                else
                if (size == "46")
                {
                    fld.s46 = Convert.ToInt32(total);

                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", salesdtl);

            }

            return Json(chkval);
        }

        public JsonResult RemoveTemp(string item)
        {
            string stat = "ok";
            string article = item.Substring(0, 14);
            string size = item.Substring(14, 2);
            List<dbSalesDtl> salesdtl = SessionHelper.GetObjectFromJson<List<dbSalesDtl>>(HttpContext.Session, "FormList");
            dbSalesDtl fld = salesdtl.Where(y => y.article == article && y.size == size).FirstOrDefault();
            salesdtl.Remove(fld);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", salesdtl);
            return Json(stat);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePOS([Bind] dbSalesHdr objSales)
        {
            string err = "";
            if (ModelState.IsValid)
            {
                var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                dbStoreList storedt = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();
                objSales.Store_id = storedt.id;
                objSales.transdate = DateTime.Now;
                List<dbSalesDtl> salesdtl = SessionHelper.GetObjectFromJson<List<dbSalesDtl>>(HttpContext.Session, "FormList");
                objSales.trans_qty = salesdtl.Sum(y=> y.qty);
                objSales.update_date = DateTime.Now;
                objSales.update_user = User.Identity.Name;
                objSales.transtype = objSales.transtypehidden;
                objSales.cardnum = objSales.cardnumhidden;
                objSales.approval_code = objSales.approvalhidden;
                foreach(var flds in salesdtl)
                {
                    flds.invoice = objSales.invoice;
                    flds.update_date = DateTime.Now;
                    flds.update_user = User.Identity.Name;
                }
                try
                {
                    db.saleshdrtbl.Add(objSales);
                    foreach(var dtl in salesdtl)
                    {
                        var dtstock = db.StockTbl.Where(y => y.itemid == dtl.article && y.storeid == storedt.id).FirstOrDefault();
                        var currentstock = dtstock.Current_stock;
                        dtstock.Current_stock = currentstock - dtl.qty;
                        var s33total = dtstock.s33;
                        dtstock.s33 = s33total - dtl.s33;
                        
                        var s34total = dtstock.s34;
                        dtstock.s34 = s34total - dtl.s34;
                        
                        var s35total = dtstock.s35;
                        dtstock.s35 = s35total - dtl.s35;
                        
                        var s36total = dtstock.s36;
                        dtstock.s36 = s36total - dtl.s36;
                        
                        var s37total = dtstock.s37;
                        dtstock.s37 = s37total - dtl.s37;
                        
                        var s38total = dtstock.s38;
                        dtstock.s38 = s38total - dtl.s38;
                        
                        var s39total = dtstock.s39;
                        dtstock.s39 = s39total - dtl.s39;
                        
                        var s40total = dtstock.s40;
                        dtstock.s40 = s40total - dtl.s40;
                        
                        var s41total = dtstock.s41;
                        dtstock.s41 = s41total - dtl.s41;
                        
                        var s42total = dtstock.s42;
                        dtstock.s42 = s42total - dtl.s42;
                        
                        var s43total = dtstock.s43;
                        dtstock.s43 = s43total - dtl.s43;
                        
                        var s44total = dtstock.s44;
                        dtstock.s44 = s44total - dtl.s44;
                        
                        var s45total = dtstock.s45;
                        dtstock.s45 = s45total - dtl.s45;
                        
                        var s46total = dtstock.s46;
                        dtstock.s46 = s46total - dtl.s46;
                        db.StockTbl.Update(dtstock);
                        db.salesdtltbl.Add(dtl);
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
    }
}
