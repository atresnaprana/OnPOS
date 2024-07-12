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
using MimeDetective.Storage.Xml.v2;

namespace OnPOS.Controllers
{
    public class PromotionController : Controller
    {
        private readonly FormDBContext db;
        private IHostingEnvironment Environment;
        private readonly ILogger<PromotionController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public IConfiguration Configuration { get; }


        public PromotionController(FormDBContext db, ILogger<PromotionController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;

        }
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Index()
        {
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            var getPromoList = db.DiscTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID).ToList();

            return View(getPromoList);
        }
        [HttpGet]
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Create()
        {
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();

            dbDiscount fld = new dbDiscount();
            fld.liststore = db.StoreListTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
            {
                id = y.id.ToString(),
                name = y.id.ToString() + " - " + y.STORE_NAME,


            }).ToList();
            fld.listitems = db.ItemMasterTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
            {
                id = y.itemid.ToString(),
                name = y.itemid.ToString() + " - " + y.itemdescription,


            }).ToList();
            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] dbDiscount objTrainer)
        {
            if (ModelState.IsValid)
            {
                var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
                objTrainer.COMPANY_ID = data.COMPANY_ID;
                objTrainer.entry_date = DateTime.Now;
                objTrainer.update_date = DateTime.Now;
                objTrainer.entry_user = User.Identity.Name;
                objTrainer.update_user = User.Identity.Name;
                if (objTrainer.isactive)
                {
                    objTrainer.status = "1";

                }
                else
                {
                    objTrainer.status = "0";

                }

                if (objTrainer.isallstorebool)
                {
                    objTrainer.isallstore = "Y";

                }
                else
                {
                    objTrainer.status = "N";
                    
                }
                try
                {

                    var validate = checkvalidate(objTrainer.article, objTrainer.validfrom, objTrainer.validto);
                    if (validate)
                    {
                        db.DiscTbl.Add(objTrainer);
                        db.SaveChanges();
                        if (!objTrainer.isallstorebool)
                        {
                            for (int i = 0; i < objTrainer.storeidlist.Count(); i++)
                            {
                                dbDiscountStoreList flds = new dbDiscountStoreList();
                                var storeid = objTrainer.storeidlist[i];
                                flds.storeid = storeid;
                                flds.promoid = objTrainer.id;
                                flds.COMPANY_ID = data.COMPANY_ID;
                                db.StoreDiscTbl.Add(flds);

                            }

                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        objTrainer.errormessage = "This article's promo is exist during this period";
                        dbDiscount fld = new dbDiscount();
                        objTrainer.liststore = db.StoreListTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
                        {
                            id = y.id.ToString(),
                            name = y.id.ToString() + " - " + y.STORE_NAME,


                        }).ToList();
                        objTrainer.listitems = db.ItemMasterTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
                        {
                            id = y.itemid.ToString(),
                            name = y.itemid.ToString() + " - " + y.itemdescription,


                        }).ToList();
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
            }
            return View(objTrainer);
        }

        public bool checkvalidate(string article, DateTime from, DateTime to)
        {
            bool isok = true;
            var chkdbdiscount = db.DiscTbl.Where(y => y.article == article && y.status == "1" && from < y.validto && y.validfrom < to).FirstOrDefault();
            if(chkdbdiscount != null)
            {
                isok = false;
            }

            return isok;
        }

        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            int ids = Convert.ToInt32(id);
            var data = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();

            dbDiscount fld = db.DiscTbl.Find(ids);
            fld.liststore = db.StoreListTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
            {
                id = y.id.ToString(),
                name = y.id.ToString() + " - " + y.STORE_NAME,


            }).ToList();
            fld.listitems = db.ItemMasterTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
            {
                id = y.itemid.ToString(),
                name = y.itemid.ToString() + " - " + y.itemdescription,


            }).ToList();
            if(fld.isallstore == "Y")
            {
                fld.isallstorebool = true;
            }
            else
            {
                fld.isallstorebool = false;
                var liststore = db.StoreDiscTbl.Where(y => y.COMPANY_ID == data.COMPANY_ID && y.promoid == fld.id).ToList();
                List<int> storeintlist = new List<int>();
                foreach(var lst in liststore)
                {
                    storeintlist.Add(lst.storeid);
                }
                fld.storeidlist = storeintlist;
            }   
            if (fld == null)
            {
                return NotFound();
            }

            return View(fld);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind] dbDiscount fld)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                int ids = Convert.ToInt32(id);

                var editFld = db.DiscTbl.Find(ids);
                editFld.article = fld.article;
                editFld.type = fld.type;
                editFld.percentage = fld.percentage;
                editFld.amount = fld.amount;
                editFld.promo_name = fld.promo_name;

                editFld.update_date = DateTime.Now;
                editFld.update_user = User.Identity.Name;
                editFld.validfrom = fld.validfrom;
                editFld.validto = fld.validto;

                try
                {
                    var validate = checkvalidate(editFld.article, editFld.validfrom, editFld.validto);
                    if (validate)
                    {
                        List<dbDiscountStoreList> newtbl = new List<dbDiscountStoreList>();
                        foreach (var flds in fld.storeidlist)
                        {
                            dbDiscountStoreList newdt = new dbDiscountStoreList();
                            newdt.promoid = editFld.id;
                            newdt.storeid = flds;
                            newdt.COMPANY_ID = editFld.COMPANY_ID;
                            newtbl.Add(newdt);
                        }
                        //Processing UserData

                        List<int> listtrans = new List<int>();
                        List<int> listExistingtrans = new List<int>();

                        var idToRemovetrans = new List<int>();
                        var idToAddtrans = new List<int>();

                        var ExistingTransDt = db.StoreDiscTbl.Where(y => y.COMPANY_ID == editFld.COMPANY_ID && y.promoid == editFld.id).ToList();
                        for (int i = 0; i < newtbl.Count(); i++)
                        {
                            var idDtl = newtbl[i].storeid;
                            listtrans.Add(idDtl);
                            idToAddtrans.Add(idDtl);
                        }

                        foreach (var exist in ExistingTransDt)
                        {
                            var transExist = exist.storeid;
                            listExistingtrans.Add(transExist);
                            idToRemovetrans.Add(transExist);

                        }

                        //removing logic 
                        for (int i = 0; i < listExistingtrans.Count(); i++)
                        {
                            var nopekExist = listExistingtrans[i];
                            for (int y = 0; y < listtrans.Count(); y++)
                            {
                                var nopekNew = listtrans[y];
                                if (nopekExist == nopekNew)
                                {
                                    idToRemovetrans.Remove(nopekExist);
                                }
                            }
                        }

                        var empDt = ExistingTransDt.Where(y => idToRemovetrans.Contains(y.storeid)).ToList<dbDiscountStoreList>();
                        foreach (var dtlemp in empDt)
                        {
                            var formEmp = dtlemp;

                            db.StoreDiscTbl.Remove(formEmp);
                            //foreach (var idtoremove in idToRemovetrans)
                            //{

                            //}
                        }
                        //adding logic
                        for (int i = 0; i < newtbl.Count(); i++)
                        {
                            var dts = newtbl[i];

                            db.StoreDiscTbl.Add(dts);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        fld.errormessage = "This article's promo is exist during this period";
                        fld.liststore = db.StoreListTbl.Where(y => y.COMPANY_ID == editFld.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
                        {
                            id = y.id.ToString(),
                            name = y.id.ToString() + " - " + y.STORE_NAME,


                        }).ToList();
                        fld.listitems = db.ItemMasterTbl.Where(y => y.COMPANY_ID == editFld.COMPANY_ID && y.FLAG_AKTIF == "1").Select(y => new DropDownModel()
                        {
                            id = y.itemid.ToString(),
                            name = y.itemid.ToString() + " - " + y.itemdescription,


                        }).ToList();
                    }
                  

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
        [Authorize(Roles = "CustomerOnPos")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            int ids = Convert.ToInt32(id);

            dbDiscount fld = db.DiscTbl.Find(ids);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Deletedisc(string id)
        {
            int ids = Convert.ToInt32(id);

            dbDiscount fld = db.DiscTbl.Find(ids);

            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    //db.trainerDb.Remove(fld);
                    fld.status = "0";
                    fld.update_date = DateTime.Now;
                    fld.update_user = User.Identity.Name;
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

        public IActionResult Reactivate(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            int ids = Convert.ToInt32(id);

            dbDiscount fld = db.DiscTbl.Find(ids);
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Reactivate")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ReactivateDisc(string id)
        {
            int ids = Convert.ToInt32(id);

            dbDiscount fld = db.DiscTbl.Find(ids);

            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    //db.trainerDb.Remove(fld);
                    fld.status = "1";
                    fld.update_date = DateTime.Now;
                    fld.update_user = User.Identity.Name;
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
    }
}
