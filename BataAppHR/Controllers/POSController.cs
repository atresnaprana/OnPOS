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
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

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
        public IActionResult Index(int storeid, string invoice)
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
            if(storeid != 0 && !String.IsNullOrEmpty(invoice))
            {
                fld.storeidpass = storeid;
                fld.invoiceidpass = invoice;
            }
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
        public JsonResult SaveTemp(string salesid, string item, string total, string discount, string subtotal, string price)
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
                fld.price = Convert.ToInt32(price);
                fld.finalprice = Convert.ToInt32(subtotal);
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
                fld.price = Convert.ToInt32(price);
                fld.finalprice = Convert.ToInt32(subtotal);
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
                    flds.transdate = objSales.transdate;
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
                    var newsalesdtl = new List<dbSalesDtl>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", newsalesdtl);

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
                return RedirectToAction("Index", "POS", new
                {
                    storeid = objSales.Store_id,
                    invoice = objSales.invoice,
                });
                //return RedirectToAction("Index");

            }
            return View(objSales);
        }

        public void CreateInvoice()
        {
            var fileUrlInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\InvoiceData");
            string pdfPath = "POSReceipt.pdf";
            var filesInv = Path.Combine(fileUrlInv, pdfPath);

            CreateReceipt(filesInv);
            Console.WriteLine($"Receipt created at: {filesInv}");
        }
        static void CreateReceipt(string pdfPath)
        {
            //// Define custom page size (e.g., 80mm x 200mm)
            //float width = 80 * 0.352778f;
            //// Convert mm to points (1 mm = 0.352778 points)
            //float height = 220 * 0.352778f;
            pdfPath = "POSReceipt.pdf";


            float width = 320;
            float height = 880;

            var pageSize = new iText.Kernel.Geom.PageSize(width, height);
            using (PdfWriter writer = new PdfWriter(pdfPath))
            using (iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            {

                Document document = new Document(pdf, pageSize);

                // Add receipt header
                document.Add(new Paragraph("Store Name").SetBold().SetFontSize(20));
                document.Add(new Paragraph("Store Address").SetFontSize(12));
                document.Add(new Paragraph("Phone: (123) 456-7890").SetFontSize(12));
                document.Add(new Paragraph("Date: " + DateTime.Now.ToString("MM/dd/yyyy")).SetFontSize(12));
                document.Add(new Paragraph("Time: " + DateTime.Now.ToString("HH:mm:ss")).SetFontSize(12));
                document.Add(new Paragraph("=================================").SetFontSize(12));

                // Add receipt items
                AddReceiptItem(document, "Item 1", 2, 10.00);
                AddReceiptItem(document, "Item 2", 1, 15.50);
                AddReceiptItem(document, "Item 3", 3, 7.99);

                // Add total
                document.Add(new Paragraph("=================================").SetFontSize(12));
                document.Add(new Paragraph("Total: $37.49").SetBold().SetFontSize(14));
                document.Add(new Paragraph("Thank you for shopping!").SetFontSize(12));

                document.Close();
            }
           
        }
        public async Task<IActionResult> DownloadReceipt(string invoiceno, int storeno)
        {
           
            var storedt = db.StoreListTbl.Where(y => y.id == storeno).FirstOrDefault();
            
            var datainvoice = getinvoiceval(invoiceno, storeno);
            var datahdr = db.saleshdrtbl.Where(y=> y.Store_id == storeno && y.invoice == invoiceno).FirstOrDefault();
            string filename = storeno.ToString() +"_" + invoiceno + "_Sales.pdf";
            float width = 320;
            float height = 880;
            var fileUrlInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/InvoiceData");
            var filesInv = Path.Combine(fileUrlInv, filename);
            string pdfPath = filesInv;
            var pageSize = new iText.Kernel.Geom.PageSize(width, height);
            using (PdfWriter writer = new PdfWriter(pdfPath))
            using (iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            {

                Document document = new Document(pdf, pageSize);    

                // Add receipt header
                document.Add(new Paragraph(storedt.STORE_NAME).SetBold().SetFontSize(20));
                document.Add(new Paragraph(storedt.STORE_ADDRESS).SetFontSize(11));
                document.Add(new Paragraph("Phone: " + storedt.STORE_MANAGER_PHONE).SetFontSize(11));
                document.Add(new Paragraph("Date: " + datahdr.transdate.ToString("dd/MM/yyyy")).SetFontSize(11));
                document.Add(new Paragraph("Time: " + datahdr.transdate.ToString("HH:mm:ss")).SetFontSize(11));
                document.Add(new Paragraph("=================================").SetFontSize(12));

                // Add receipt items

                //AddReceiptItem(document, "Item id", "qty", "amt");
                foreach(var fld in datainvoice)
                {
                    AddReceiptItem(document, fld.article, fld.qty, fld.amount);
                }
                

                // Add total
                document.Add(new Paragraph("=================================").SetFontSize(12));
                document.Add(new Paragraph("Total: IDR " + String.Format("{0:0,0}", datainvoice.Sum(y => y.amount))).SetBold().SetFontSize(14));
                document.Add(new Paragraph("Thank you for shopping!").SetFontSize(12));
                
                document.Close();
            }


            if (filename == null)
                return Content("filename not present");

           
            string path2 = Path.Combine(this.Environment.WebRootPath, "InvoiceData");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        static void AddReceiptItem(Document document, string itemName, int quantity, double price)
        {
            
            document.Add(new Paragraph($"{itemName} x{quantity} - Rp {String.Format("{0:0,0}", price)}").SetFontSize(11));
        }
        #region syncfusion
        //public string PrintInvoice(List<dbSalesHdr> data, int storeid, string invoice)
        //{
        //    string filename = "";


        //    //Creates a new PDF document
        //    PdfDocument document = new PdfDocument();
        //    //Adds page settings
        //    document.PageSettings.Orientation = PdfPageOrientation.Landscape;
        //    document.PageSettings.Margins.All = 25;
        //    document.PageSettings.Size = new Syncfusion.Drawing.SizeF(58,40);
        //    //Adds a page to the document
        //    PdfPage page = document.Pages.Add();
        //    PdfGraphics graphics = page.Graphics;

        //    var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");

        //    var files = Path.Combine(fileUrl, "Bata_red_small.png");
        //    FileStream imageStream = new FileStream(files, FileMode.Open, FileAccess.Read);
        //    Syncfusion.Drawing.RectangleF bounds = new Syncfusion.Drawing.RectangleF(400, 0, 250, 60);
        //    PdfImage image = PdfImage.FromStream(imageStream);
        //    PdfFont fonts = new PdfStandardFont(PdfFontFamily.TimesRoman, 15);

        //    //Draw the text.

        //    //Draws the image to the PDF page
        //    page.Graphics.DrawImage(image, bounds);

        //    PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(232, 4, 20));
        //    bounds = new Syncfusion.Drawing.RectangleF(0, bounds.Bottom + 10, graphics.ClientSize.Width, 30);
        //    //Draws a rectangle to place the heading in that region.
        //    graphics.DrawRectangle(solidBrush, bounds);
        //    //Creates a font for adding the heading in the page
        //    PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
        //    //Creates a text element to add the invoice number

        //    PdfTextElement element = new PdfTextElement("StoreID: " + data[0].Store_id, subHeadingFont);
        //    PdfTextElement element2 = new PdfTextElement("Invoice: " + data[0].invoice, subHeadingFont);

        //    element.Brush = PdfBrushes.White;
        //    element2.Brush = PdfBrushes.Black;

        //    //Draws the heading on the page
        //    PdfLayoutResult result = element.Draw(page, new Syncfusion.Drawing.PointF(10, bounds.Top + 8));

        //    string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
        //    //Measures the width of the text to place it in the correct location
        //    Syncfusion.Drawing.SizeF textSize = subHeadingFont.MeasureString(currentDate);
        //    Syncfusion.Drawing.PointF textPosition = new Syncfusion.Drawing.PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
        //    //Draws the date by using DrawString method
        //    graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);

        //    PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
        //    //Creates text elements to add the address and draw it to the page.
        //    element.Brush = new PdfSolidBrush(new PdfColor(0, 0, 0));
        //    result = element2.Draw(page, new Syncfusion.Drawing.PointF(10, result.Bounds.Bottom + 20));
        //    PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
        //    Syncfusion.Drawing.PointF startPoint = new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + 3);
        //    Syncfusion.Drawing.PointF endPoint = new Syncfusion.Drawing.PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
        //    //Draws a line at the bottom of the address
        //    graphics.DrawLine(linePen, startPoint, endPoint);


        //    ////Creates a PDF grid
        //    //PdfGrid grid = new PdfGrid();
        //    ////Adds the data source
        //    //grid.DataSource = data.Select(y => new LedgerTbl()
        //    //{
        //    //    id = y.id,
        //    //    docnum = y.docnum,
        //    //    periode = y.periode,
        //    //    open_balance = y.open_balance,
        //    //    debit = y.debit,
        //    //    credit = y.credit,
        //    //    close_balance = y.close_balance,
        //    //    deskripsi = y.deskripsi,
        //    //    tgl_doc = y.tgl_doc,
        //    //    reference = y.reference
        //    //}).ToList();
        //    ////Creates the grid cell styles
        //    //PdfGridCellStyle cellStyle = new PdfGridCellStyle();
        //    //cellStyle.Borders.All = PdfPens.White;
        //    //PdfGridRow header = grid.Headers[0];
        //    ////Creates the header style
        //    //PdfGridCellStyle headerStyle = new PdfGridCellStyle();
        //    //headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
        //    //headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(232, 4, 20));
        //    //headerStyle.TextBrush = PdfBrushes.White;
        //    //headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);
        //    //PdfGridCellStyle rowstyle = new PdfGridCellStyle();
        //    //rowstyle.TextBrush = PdfBrushes.Black;
        //    //rowstyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
        //    //var test = grid.Rows.Count();
        //    //int maxrow = 0;
        //    //for (int i = 0; i < grid.Rows.Count; i++)
        //    //{
        //    //    var cellcount = grid.Rows[i].Cells.Count;
        //    //    for (int x = 0; x < cellcount; x++)
        //    //    {

        //    //        grid.Rows[i].Cells[x].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //    //        grid.Rows[i].Cells[x].Style = rowstyle;
        //    //    }
        //    //    maxrow++;
        //    //}
        //    //var lastrow = grid.Rows[maxrow - 1];
        //    //PdfGridCellStyle rowstyletotal = new PdfGridCellStyle();
        //    //rowstyletotal.TextBrush = PdfBrushes.Black;
        //    //rowstyletotal.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Bold);
        //    //var cellcounttotal = lastrow.Cells.Count;
        //    //for (int x = 0; x < cellcounttotal; x++)
        //    //{

        //    //    lastrow.Cells[x].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //    //    lastrow.Cells[x].Style = rowstyletotal;
        //    //}

        //    ////Adds cell customizations
        //    //for (int i = 0; i < header.Cells.Count; i++)
        //    //{
        //    //    if (i == 0 || i == 1)
        //    //        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //    //    else
        //    //        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //    //}

        //    ////Applies the header style
        //    //header.ApplyStyle(headerStyle);
        //    //cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
        //    //cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
        //    //cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
        //    ////Creates the layout format for grid
        //    //PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
        //    //// Creates layout format settings to allow the table pagination
        //    //layoutFormat.Layout = PdfLayoutType.Paginate;
        //    ////Draws the grid to the PDF page.
        //    //PdfGridLayoutResult gridResult = grid.Draw(page, new Syncfusion.Drawing.RectangleF(new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + 20), new Syncfusion.Drawing.SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

        //    ////Creates text elements to add the address and draw it to the page.
        //    //string footer = " " + "\r\n";



        //    //PdfFont fontFoot = new PdfStandardFont(PdfFontFamily.TimesRoman, 15);

        //    ////Draw the text.

        //    //graphics.DrawString(footer, fontFoot, PdfBrushes.Black, new Syncfusion.Drawing.PointF(1450, 170));


        //    var fileUrlInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\InvoiceData");
        //    filename = "Invoice_" + storeid + "_" + invoice + ".pdf";
        //    var filesInv = Path.Combine(fileUrlInv, filename);

        //    var filePathInv = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\InvoiceData");
        //    if (!Directory.Exists(filePathInv))
        //    {
        //        Directory.CreateDirectory(filePathInv);
        //    }
        //    FileStream fileStream = new FileStream(filesInv, FileMode.CreateNew, FileAccess.ReadWrite);
        //    //Save and close the PDF document 
        //    document.Save(fileStream);
        //    document.Close(true);
        //    fileStream.Dispose();
        //    return filename;
        //}
        #endregion syncfusion
        public JsonResult getRecap()
        {
            dbStoreList storedt = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();
            List<SalesRecap> salesrecap = new List<SalesRecap>();

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
	                d2.codedivisi,
	                d3.DivisiName,
	                 sum(d.qty) as qty,
                   sum(d.finalprice) as price
                from
	                onposdb.dbSalesDtl d
	                left join dbStoreList db on d.store_id = db.id 
	                left join onposdb.dbItemMaster d2 on d.article = d2.itemid and d2.COMPANY_ID = db.COMPANY_ID 
	                left join onposdb.dbDepartment d3 on d3.CodeDivisi = d2.codedivisi and  d3.COMPANY_ID = db.COMPANY_ID
                where date_format(d.transdate, '%Y-%m-%d') = date_format(now(), '%Y-%m-%d')
                and d.Store_id = " + storedt.id + "";
                
                query+= @" group by d2.codedivisi,
	                d3.DivisiName";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SalesRecap fld = new SalesRecap();
                        fld.department = reader["DivisiName"].ToString();
                        fld.qty = Convert.ToInt32(reader["qty"].ToString());
                        fld.amount = Convert.ToInt32(reader["price"].ToString());
                        
                       

                        salesrecap.Add(fld);
                    }
                }
                conn.Close();
            }

            return Json(salesrecap);
        }
        public JsonResult getrecapdtl()
        {
            dbStoreList storedt = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();
            List<SalesRecap> salesrecap = new List<SalesRecap>();

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
	                            d.store_id,
                                d.invoice,
	                            date_format(d.transdate, '%d/%m/%Y') as TransDate,
	                            d.article,
	                            d2.itemdescription,
	                            d2.category ,
	                            d4.`type` as discounttype,
	                            case when d4.`type` = 'percentage' then d.disc_prc else sum(d.disc_amount) end as Discount,
	                            d.price,
                                 case when sum(d.s33) <> 0 then 
	                            '33'
	                           
	                            when sum(d.s34) <> 0 then 
	                            '34'
	                            when sum(d.s35) <> 0 then 
	                            '35'
	                            when sum(d.s36) <> 0 then 
	                            '36'
	                            when sum(d.s37) <> 0 then 
	                            '37'
	                            when sum(d.s38) <> 0 then 
	                            '38'
	                            when sum(d.s39) <> 0 then 
	                            '39'
	                            when sum(d.s40) <> 0 then 
	                            '40'
	                            when sum(d.s41) <> 0 then 
	                            '41'
	                            when sum(d.s42) <> 0 then 
	                            '42'
	                            when sum(d.s43) <> 0 then 
	                            '43'
	                            when sum(d.s44) <> 0 then 
	                            '44'
	                            when sum(d.s45) <> 0 then 
	                            '45'
	                            when sum(d.s46) <> 0 then 
	                            '46'
	                            end as sz,
	                            sum(d.qty) as qty,
	                            sum(d.finalprice) as amount  
                            from
	                            onposdb.dbSalesDtl d
	                            left join dbStoreList db on d.store_id = db.id 
	                            left join onposdb.dbItemMaster d2 on d.article = d2.itemid and d2.COMPANY_ID = db.COMPANY_ID 
	                            left join onposdb.dbDepartment d3 on d3.CodeDivisi = d2.codedivisi and  d3.COMPANY_ID = db.COMPANY_ID
	                            left join onposdb.dbDiscount d4 on d4.id = d.discountcode and d4.article = d.article
                            where date_format(d.transdate, '%Y-%m-%d') = date_format(now(), '%Y-%m-%d')
                            and d.Store_id = " + storedt.id + "";

                            query += @" group by d.store_id,
	                                    date_format(d.transdate, '%d/%m/%Y'),
	                                    d.article,
	                                    d2.itemdescription,
	                                    d2.category,
	                                    d4.`type` ,
	                                    d.price,
                                        d.invoice";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SalesRecap fld = new SalesRecap();
                        fld.store_id = Convert.ToInt32(reader["store_id"].ToString());
                        fld.transdate = reader["TransDate"].ToString();
                        fld.article = reader["article"].ToString();
                        fld.itemdescription = reader["itemdescription"].ToString();
                        fld.category = reader["category"].ToString();
                        fld.invoice = reader["invoice"].ToString();
                        fld.discounttype = reader["discounttype"].ToString();
                        fld.discount = Convert.ToInt32(reader["Discount"].ToString());
                        fld.articleprice = Convert.ToInt32(reader["price"].ToString());
                        fld.qty = Convert.ToInt32(reader["qty"].ToString());
                        fld.amount = Convert.ToInt32(reader["amount"].ToString());
                        fld.sz = reader["sz"].ToString();



                        salesrecap.Add(fld);
                    }
                }
                conn.Close();
            }

            return Json(salesrecap);
        }
        public List<SalesRecap> getinvoiceval(string invoiceno, int storeid)
        {
            List<SalesRecap> salesrecap = new List<SalesRecap>();

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select
	                            d.store_id,
                                d.invoice,
	                            date_format(d.transdate, '%d/%m/%Y') as TransDate,
	                            d.article,
	                            d2.itemdescription,
	                            d2.category ,
	                            d4.`type` as discounttype,
	                            case when d4.`type` = 'percentage' then d.disc_prc else sum(d.disc_amount) end as Discount,
	                            d.price,
                                 case when sum(d.s33) <> 0 then 
	                            '33'
	                           
	                            when sum(d.s34) <> 0 then 
	                            '34'
	                            when sum(d.s35) <> 0 then 
	                            '35'
	                            when sum(d.s36) <> 0 then 
	                            '36'
	                            when sum(d.s37) <> 0 then 
	                            '37'
	                            when sum(d.s38) <> 0 then 
	                            '38'
	                            when sum(d.s39) <> 0 then 
	                            '39'
	                            when sum(d.s40) <> 0 then 
	                            '40'
	                            when sum(d.s41) <> 0 then 
	                            '41'
	                            when sum(d.s42) <> 0 then 
	                            '42'
	                            when sum(d.s43) <> 0 then 
	                            '43'
	                            when sum(d.s44) <> 0 then 
	                            '44'
	                            when sum(d.s45) <> 0 then 
	                            '45'
	                            when sum(d.s46) <> 0 then 
	                            '46'
	                            end as sz,
	                            sum(d.qty) as qty,
	                            sum(d.finalprice) as amount  
                            from
	                            onposdb.dbSalesDtl d
	                            left join dbStoreList db on d.store_id = db.id 
	                            left join onposdb.dbItemMaster d2 on d.article = d2.itemid and d2.COMPANY_ID = db.COMPANY_ID 
	                            left join onposdb.dbDepartment d3 on d3.CodeDivisi = d2.codedivisi and  d3.COMPANY_ID = db.COMPANY_ID
	                            left join onposdb.dbDiscount d4 on d4.id = d.discountcode and d4.article = d.article
                            where 
                             d.Store_id = " + storeid + " and d.invoice ="+invoiceno+"";

                query += @" group by d.store_id,
	                                    date_format(d.transdate, '%d/%m/%Y'),
	                                    d.article,
	                                    d2.itemdescription,
	                                    d2.category,
	                                    d4.`type` ,
	                                    d.price,
                                        d.invoice";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SalesRecap fld = new SalesRecap();
                        fld.store_id = Convert.ToInt32(reader["store_id"].ToString());
                        fld.transdate = reader["TransDate"].ToString();
                        fld.article = reader["article"].ToString();
                        fld.itemdescription = reader["itemdescription"].ToString();
                        fld.category = reader["category"].ToString();
                        fld.invoice = reader["invoice"].ToString();
                        fld.discounttype = reader["discounttype"].ToString();
                        fld.discount = Convert.ToInt32(reader["Discount"].ToString());
                        fld.articleprice = Convert.ToInt32(reader["price"].ToString());
                        fld.qty = Convert.ToInt32(reader["qty"].ToString());
                        fld.amount = Convert.ToInt32(reader["amount"].ToString());
                        fld.sz = reader["sz"].ToString();



                        salesrecap.Add(fld);
                    }
                }
                conn.Close();
            }

            return salesrecap;
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

    }
}
