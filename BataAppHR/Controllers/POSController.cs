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

namespace OnPOS.Controllers
{
    public class POSController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<POSController> _logger;
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
            fld.username = storedt.STORE_MANAGER_NAME;
            fld.invoice = DateTime.Now.ToString("ddMMyy") + "0001";
            var dddata = stafftbl.Select(y => new DropDownModel()
            {
                id = y.id.ToString(),
                name = y.id.ToString() + " - " + y.SALES_NAME,
                

            }).ToList();
            fld.ddStaff = dddata;
            fld.scanqty = 1;
            fld.Store_id = storedt.id;
            fld.storedata = storedt;
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
            var getdiscount = db.DiscTbl.Where(y => y.article == scanitemshort).FirstOrDefault();

            var getstockdata = db.ItemMasterTbl.Where(y => y.itemid == scanitemshort).FirstOrDefault();
            if(getdiscount != null)
            {
                getstockdata.disctype = getdiscount.type;
                getstockdata.discamt = getdiscount.amount;
                getstockdata.discperc = getdiscount.percentage;
            }
            

            return Json(getstockdata);
        }
        
    }
}
