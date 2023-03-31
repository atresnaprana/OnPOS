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
    public class ExportDataController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        private IHostingEnvironment Environment;
        private readonly ILogger<ExportDataController> _logger;

        public ExportDataController(FormDBContext db, ILogger<ExportDataController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            var data = new List<ReportModel>();
            var edpCode = HttpContext.Session.GetString(SessionKeyName);
            var xstoretbl = db.xstore_organization.ToList();
           
     

            if (User.IsInRole("Admin"))
            {
                ViewData["EdpCode"] = 0;
                data = db.SSTable.ToList().Select(y => new ReportModel()
                {
                    DISTRICT = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().district : "",
                    EDP_CODE = y.EDP_CODE,
                    STORE_LOCATION = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().store_location : "",
                    AREA = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().area : "",
                    SS_CODE = y.SS_CODE,
                    NAMA_SS = y.NAMA_SS,
                    POSITION = y.POSITION,
                    FLAG_AKTIF = y.FLAG_AKTIF,
                    SEX = y.SEX,
                    KTP = y.KTP,
                    HP_SS = y.HP_SS,
                    EMAIL_SS = y.EMAIL_SS,
                    SIZE_SERAGAM = y.SIZE_SERAGAM,
                    SIZE_SEPATU_UK = y.SIZE_SEPATU_UK,
                    JOIN_DATE = y.JOIN_DATE,
                    RESIGN_DATE = y.RESIGN_DATE,
                    RESIGN_TXT = y.RESIGN_TXT,
                    RESIGN_TYPE = y.RESIGN_TYPE,
                    RESIGN_TYPE2 = y.RESIGN_TYPE2,
                    LAMA_KERJA = getLength(y.JOIN_DATE, y.RESIGN_DATE),
                    VAKSIN1 = y.VAKSIN1,
                    VAKSIN2 = y.VAKSIN2,
                    DAYS_LENGTH = getDays(y.JOIN_DATE, y.RESIGN_DATE),
                    MONTH_LENGTH = getMonth(y.JOIN_DATE, y.RESIGN_DATE),
                    YEAR_LENGTH = getyr(y.JOIN_DATE, y.RESIGN_DATE),
                    STORE_UPDATE = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().UPDATE_DATE : null,
                    STORE_UPDATE_PERSON = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().UPDATE_USER : "",
                    UPDATE_DATE = y.UPDATE_DATE,
                    UPDATE_USER = y.UPDATE_USER,
                    ENTRY_DATE = y.ENTRY_DATE,
                    ENTRY_USER = y.ENTRY_USER
                   
                }).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(edpCode))
                {
                    data = db.SSTable.Where(y => y.EDP_CODE == edpCode).ToList().Select(y => new ReportModel()
                    {
                        DISTRICT = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().district : "",
                        EDP_CODE = y.EDP_CODE,
                        STORE_LOCATION = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().store_location : "",
                        AREA = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().area : "",
                        SS_CODE = y.SS_CODE,
                        NAMA_SS = y.NAMA_SS,
                        POSITION = y.POSITION,
                        FLAG_AKTIF = y.FLAG_AKTIF,
                        SEX = y.SEX,
                        KTP = y.KTP,
                        HP_SS = y.HP_SS,
                        EMAIL_SS = y.EMAIL_SS,
                        SIZE_SERAGAM = y.SIZE_SERAGAM,
                        SIZE_SEPATU_UK = y.SIZE_SEPATU_UK,
                        JOIN_DATE = y.JOIN_DATE,
                        RESIGN_DATE = y.RESIGN_DATE,
                        RESIGN_TXT = y.RESIGN_TXT,
                        RESIGN_TYPE = y.RESIGN_TYPE,
                        RESIGN_TYPE2 = y.RESIGN_TYPE2,
                        LAMA_KERJA = getLength(y.JOIN_DATE, y.RESIGN_DATE),
                        VAKSIN1 = y.VAKSIN1,
                        VAKSIN2 = y.VAKSIN2,
                        DAYS_LENGTH = getDays(y.JOIN_DATE, y.RESIGN_DATE),
                        MONTH_LENGTH = getMonth(y.JOIN_DATE, y.RESIGN_DATE),
                        YEAR_LENGTH = getyr(y.JOIN_DATE, y.RESIGN_DATE),
                        STORE_UPDATE = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().UPDATE_DATE : null,
                        STORE_UPDATE_PERSON = xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP_CODE).FirstOrDefault().UPDATE_USER : "",
                        UPDATE_DATE = y.UPDATE_DATE,
                        UPDATE_USER = y.UPDATE_USER,
                        ENTRY_DATE = y.ENTRY_DATE,
                        ENTRY_USER = y.ENTRY_USER
                    }).ToList(); ViewData["EdpCode"] = edpCode;
                }
            }
          
            return View(data);
        }
        private string getLength(DateTime? join, DateTime? resign)
        {
            string length = "";
            if (join != null)
            {
                DateTime dtResign = DateTime.Now;
                if (resign != null)
                {
                    dtResign = Convert.ToDateTime(resign.Value);
                }

                DateTime dtJoin = Convert.ToDateTime(join.Value);

                TimeSpan timeSpan = dtResign - dtJoin;
                DateTime age = DateTime.MinValue + timeSpan;
                int years = age.Year - 1;
                int months = age.Month - 1;
                int days = age.Day - 1;
                length = years + " Years " + months + " Months " + days + " days";
            }
            else
            {
                length = "0" + " Years " + "0" + " Months " + "0" + " days";

            }

            return length;
        }
        private int getyr(DateTime? join, DateTime? resign)
        {
            int years = 0;
            if (join != null)
            {
                DateTime dtResign = DateTime.Now;
                if (resign != null)
                {
                    dtResign = Convert.ToDateTime(resign.Value);
                }

                DateTime dtJoin = Convert.ToDateTime(join.Value);

                TimeSpan timeSpan = dtResign - dtJoin;
                DateTime age = DateTime.MinValue + timeSpan;
                years = age.Year - 1;
            }
            return years;
        }
        private int getMonth(DateTime? join, DateTime? resign)
        {
            int months = 0;

            if (join != null)
            {
                DateTime dtResign = DateTime.Now;
                if (resign != null)
                {
                    dtResign = Convert.ToDateTime(resign.Value);
                }

                DateTime dtJoin = Convert.ToDateTime(join.Value);

                TimeSpan timeSpan = dtResign - dtJoin;
                DateTime age = DateTime.MinValue + timeSpan;
                months = age.Month - 1;
            }            
            return months;
        }
        private int getDays(DateTime? join, DateTime? resign)
        {
            int days = 0;

            if (join != null)
            {
                DateTime dtResign = DateTime.Now;
                if (resign != null)
                {
                    dtResign = Convert.ToDateTime(resign.Value);
                }

                DateTime dtJoin = Convert.ToDateTime(join.Value);

                TimeSpan timeSpan = dtResign - dtJoin;
                DateTime age = DateTime.MinValue + timeSpan;
                days = age.Day - 1;
            }
            return days;
        }
    }
}
