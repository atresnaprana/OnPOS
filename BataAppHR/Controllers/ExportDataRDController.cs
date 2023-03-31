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
    public class ExportDataRDController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        private IHostingEnvironment Environment;
        private readonly ILogger<ExportDataRDController> _logger;

        public ExportDataRDController(FormDBContext db, ILogger<ExportDataRDController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var data = new List<dbRD>();
            var edpCode = HttpContext.Session.GetString(SessionKeyName);
            var xstoretbl = db.xstore_organization.ToList();
            if (User.IsInRole("Admin"))
            {
                data = db.RDTbl.ToList().Select(y => new dbRD()
                {
                    RD_CODE = y.RD_CODE,
                    EDP_CODE = getEdpConcat(y.RD_CODE),
                    NM_RD = y.NM_RD,
                    FLAG_AKTIF = y.FLAG_AKTIF,
                    RESIGN_DATE = y.RESIGN_DATE,
                    RESIGN_TXT = y.RESIGN_TXT,
                    SEX = y.SEX,
                    RD_HP = y.RD_HP,
                    RD_EMAIL = y.RD_EMAIL,
                    JOIN_DATE = y.JOIN_DATE,
                    RD_SERAGAM_SIZE = y.RD_SERAGAM_SIZE,
                    RD_SEPATU_SIZEUK = y.RD_SEPATU_SIZEUK,
                    No_KTP = y.No_KTP,
                    VAKSIN1 = y.VAKSIN1,
                    VAKSIN2 = y.VAKSIN2,
                    STORE_NAME = getStoreLocConcat(y.RD_CODE),
                    //STORE_UPDATE = xstoretbl.Where(x => x.edp == getEdpConcat(y.RD_CODE)).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == getEdpConcat(y.RD_CODE)).FirstOrDefault().UPDATE_DATE : null,
                    //STORE_UPDATE_PERSON = xstoretbl.Where(x => x.edp == getEdpConcat(y.RD_CODE)).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == getEdpConcat(y.RD_CODE)).FirstOrDefault().UPDATE_USER : "",
                    UPDATE_DATE = y.UPDATE_DATE,
                    UPDATE_USER = y.UPDATE_USER,
                    ENTRY_DATE = y.ENTRY_DATE,
                    ENTRY_USER = y.ENTRY_USER,
                    STORE_UPDATE = getStoreUpdateConcat(y.RD_CODE),
                    STORE_UPDATE_PERSON = getUpdateUsrConcat(y.RD_CODE)
                    //RD_CODE = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().store_location : "",

                }).ToList();
            }
            var test = data.Where(y => !string.IsNullOrEmpty(y.STORE_UPDATE)).ToList().Count();
            return View(data);
        }
        public string getEdpConcat(string rdcode)
        {
            string edp = "";
            var xstoredt = db.xstore_organization.Where(y => y.RD_CODE == rdcode).ToList();
            if(xstoredt.Count() > 0)
            {
                if(xstoredt.Count() == 1)
                {
                    edp = xstoredt[0].edp;
                }
                else
                {
                    foreach(var fld in xstoredt)
                    {
                        edp += fld.edp;
                        edp += ";";
                    }
                }
            }
            return edp;
        }
        public string getStoreLocConcat(string rdcode)
        {
            string toko = "";
            var xstoredt = db.xstore_organization.Where(y => y.RD_CODE == rdcode).ToList();
            if (xstoredt.Count() > 0)
            {
                if (xstoredt.Count() == 1)
                {
                    toko = xstoredt[0].store_location;
                }
                else
                {
                    foreach (var fld in xstoredt)
                    {
                        toko += fld.store_location;
                        toko += ";";
                    }
                }
            }
            return toko;
        }
        public string getStoreUpdateConcat(string rdcode)
        {
            string toko = "";
            string updateDate = "";
            var xstoredt = db.xstore_organization.Where(y => y.RD_CODE == rdcode).ToList();
            if (xstoredt.Count() > 0)
            {
                if (xstoredt.Count() == 1)
                {
                    if(xstoredt[0].UPDATE_DATE != null)
                    {
                        updateDate = xstoredt[0].UPDATE_DATE.ToString();
                    }
                    else
                    {
                        updateDate = "N/A";
                    }
                }
                else
                {
                    foreach (var fld in xstoredt)
                    {
                        if(fld.UPDATE_DATE != null)
                        {
                            updateDate += fld.edp + ":" + fld.UPDATE_DATE.ToString();
                            updateDate += ";";
                        }
                        else
                        {
                            updateDate += fld.edp + ":N/A" ;
                            updateDate += ";";
                        }
                    }
                }
            }
            return updateDate;
        }
        public string getUpdateUsrConcat(string rdcode)
        {
            string edp = "";
            var xstoredt = db.xstore_organization.Where(y => y.RD_CODE == rdcode).ToList();
            if (xstoredt.Count() > 0)
            {
                if (xstoredt.Count() == 1)
                {
                    edp = xstoredt[0].UPDATE_USER;
                }
                else
                {
                    foreach (var fld in xstoredt)
                    {
                       
                        if (!string.IsNullOrEmpty(fld.UPDATE_USER))
                        {
                            edp += fld.edp + ":" + fld.UPDATE_USER;
                            edp += ";";
                        }
                        else
                        {
                            edp += fld.edp + ":N/A";
                            edp += ";";

                        }
                       
                    }
                }
            }
            return edp;
        }
    }
}
