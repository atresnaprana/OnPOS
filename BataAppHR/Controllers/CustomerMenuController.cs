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
using System.Data.SqlClient;
namespace BataAppHR.Controllers
{
    public class CustomerMenuController : Controller
    {
        private readonly FormDBContext db;

        private IHostingEnvironment Environment;
        private readonly ILogger<SalesOrderController> _logger;
        private readonly IMailService mailService;
        public IConfiguration Configuration { get; }
        
        public CustomerMenuController(FormDBContext db, ILogger<CustomerMenuController> logger, IHostingEnvironment _environment,
             IConfiguration configuration, IMailService mailService)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;
            this.mailService = mailService;

        }
        public List<dbSalesOrderDtl> testorderdata()
        {
            string mySqlConnectionStr = Configuration.GetConnectionString("DBGudang");
            var usermail = User.Identity.Name;
            var salesorder = db.SalesOrderTbl.Where(y => y.EMAIL ==usermail).ToList();
            List<int> listidorder = new List<int>();
            foreach(var fld in salesorder)
            {
                listidorder.Add(fld.id);
            }
            var salesdtlreal = db.SalesOrderDtlTbl.Where(y => listidorder.Contains(y.id_order)).OrderBy(y => y.id_order).ToList();
            List<dbSalesOrderDtl> orderdtldata = new List<dbSalesOrderDtl>();
            using (SqlConnection connection = new SqlConnection(mySqlConnectionStr))
            {

                connection.Open();

                String sql = @" select 
                                FORMAT(date_entry, 'dd/MM/yyyy ') as date_entry, 
                                picking_no as inv, 
                                store_id as EDP, 
                                article, 
                                sum(qty) as qty,
                                sum(case when size = 1 then qty else 0 end) as size1,
                                sum(case when size = 2 then qty else 0 end) as size2,
                                sum(case when size = 3 then qty else 0 end) as size3,
                                sum(case when size = 4 then qty else 0 end) as size4,
                                sum(case when size = 5 then qty else 0 end) as size5,
                                sum(case when size = 6 then qty else 0 end) as size6,
                                sum(case when size = 7 then qty else 0 end) as size7,
                                sum(case when size = 8 then qty else 0 end) as size8,
                                sum(case when size = 9 then qty else 0 end) as size9,
                                sum(case when size = 10 then qty else 0 end) as size10,
                                sum(case when size = 11 then qty else 0 end) as size11,
                                sum(case when size = 12 then qty else 0 end) as size12,
                                sum(case when size = 13 then qty else 0 end) as size13
                                from dbo.dwh_trn_final_invoice_detil where store_id = '56712'
	                            and picking_no = '13-196' 
	                            and yearclose = year(GETDATE()) 
	                            and wkclose is not null
	                            group by 
	                            picking_no,
	                            store_id,
	                            article,
	                            FORMAT (date_entry, 'dd/MM/yyyy ')
	                            order by article";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var date_entry = reader.GetString(0);
                            var invno = reader.GetString(1);
                            var edp = reader.GetString(2);
                            var article = reader.GetString(3);
                            var qty = reader.GetInt32(4);
                            var size1 = reader.GetInt32(5);
                            var size2 = reader.GetInt32(6);
                            var size3 = reader.GetInt32(7);
                            var size4 = reader.GetInt32(8);
                            var size5 = reader.GetInt32(9);
                            var size6 = reader.GetInt32(10);
                            var size7 = reader.GetInt32(11);
                            var size8 = reader.GetInt32(12);
                            var size9 = reader.GetInt32(13);
                            var size10 = reader.GetInt32(14);
                            var size11 = reader.GetInt32(15);
                            var size12 = reader.GetInt32(16);
                            var size13 = reader.GetInt32(17);
                            dbSalesOrderDtl flds = new dbSalesOrderDtl();
                            flds.article = article;
                            //flds.qty = Convert.ToInt32(qty);
                            flds.price = getdatapriceUpl(article);
                            flds.final_price = flds.price * flds.qty;
                            flds.Size_1 = Convert.ToInt32(size1);
                            flds.Size_2 = Convert.ToInt32(size2);
                            flds.Size_3 = Convert.ToInt32(size3)+3;
                            flds.Size_4 = Convert.ToInt32(size4)+2;
                            flds.Size_5 = Convert.ToInt32(size5);
                            flds.Size_6 = Convert.ToInt32(size6)+2;
                            flds.Size_7 = Convert.ToInt32(size7);
                            flds.Size_8 = Convert.ToInt32(size8);
                            flds.Size_9 = Convert.ToInt32(size9);
                            flds.Size_10 = Convert.ToInt32(size10);
                            flds.Size_11 = Convert.ToInt32(size11);
                            flds.Size_12 = Convert.ToInt32(size12);
                            flds.Size_13 = Convert.ToInt32(size13);
                            flds.picking_no = invno;
                            flds.qty = flds.Size_1 + flds.Size_2 + flds.Size_3 + flds.Size_4 + flds.Size_5 + flds.Size_6 + flds.Size_7 + flds.Size_8 + flds.Size_9 + flds.Size_10 + flds.Size_11 + flds.Size_12 + flds.Size_13;
                            orderdtldata.Add(flds);

                        }
                    }
                }
            }
            return orderdtldata;

        }
        public List<dbSalesOrderDtlCredit> testcreditdata()
        {
            string mySqlConnectionStr = Configuration.GetConnectionString("DBGudang");
            var usermail = User.Identity.Name;
            var salesorder = db.SalesOrderTbl.Where(y => y.EMAIL == usermail).ToList();
            List<int> listidorder = new List<int>();
            foreach (var fld in salesorder)
            {
                listidorder.Add(fld.id);
            }
            var salesdtlreal = db.SalesOrderDtlTbl.Where(y => listidorder.Contains(y.id_order)).OrderBy(y => y.id_order).ToList();
            List<dbSalesOrderDtlCredit> orderdtldata = new List<dbSalesOrderDtlCredit>();
            using (SqlConnection connection = new SqlConnection(mySqlConnectionStr))
            {

                connection.Open();

                String sql = @" select 
                                FORMAT(date_entry, 'dd/MM/yyyy ') as date_entry, 
                                picking_no as inv, 
                                store_id as EDP, 
                                article, 
                                sum(qty) as qty,
                                sum(case when size = 1 then qty else 0 end) as size1,
                                sum(case when size = 2 then qty else 0 end) as size2,
                                sum(case when size = 3 then qty else 0 end) as size3,
                                sum(case when size = 4 then qty else 0 end) as size4,
                                sum(case when size = 5 then qty else 0 end) as size5,
                                sum(case when size = 6 then qty else 0 end) as size6,
                                sum(case when size = 7 then qty else 0 end) as size7,
                                sum(case when size = 8 then qty else 0 end) as size8,
                                sum(case when size = 9 then qty else 0 end) as size9,
                                sum(case when size = 10 then qty else 0 end) as size10,
                                sum(case when size = 11 then qty else 0 end) as size11,
                                sum(case when size = 12 then qty else 0 end) as size12,
                                sum(case when size = 13 then qty else 0 end) as size13
                                from dbo.dwh_trn_final_invoice_detil where store_id = '56712'
	                            and picking_no = '13-196' 
	                            and yearclose = year(GETDATE()) 
	                            and wkclose is not null
	                            group by 
	                            picking_no,
	                            store_id,
	                            article,
	                            FORMAT (date_entry, 'dd/MM/yyyy ')
	                            order by article";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var date_entry = reader.GetString(0);
                            var invno = reader.GetString(1);
                            var edp = reader.GetString(2);
                            var article = reader.GetString(3);
                            var qty = reader.GetInt32(4);
                            var size1 = reader.GetInt32(5);
                            var size2 = reader.GetInt32(6);
                            var size3 = reader.GetInt32(7);
                            var size4 = reader.GetInt32(8);
                            var size5 = reader.GetInt32(9);
                            var size6 = reader.GetInt32(10);
                            var size7 = reader.GetInt32(11);
                            var size8 = reader.GetInt32(12);
                            var size9 = reader.GetInt32(13);
                            var size10 = reader.GetInt32(14);
                            var size11 = reader.GetInt32(15);
                            var size12 = reader.GetInt32(16);
                            var size13 = reader.GetInt32(17);
                            dbSalesOrderDtlCredit flds = new dbSalesOrderDtlCredit();
                            flds.article = article;
                            flds.qty = Convert.ToInt32(qty);
                            flds.price = getdatapriceUpl(article);
                            flds.final_price = flds.price * flds.qty;
                            flds.Size_1 = Convert.ToInt32(size1);
                            flds.Size_2 = Convert.ToInt32(size2);
                            flds.Size_3 = Convert.ToInt32(size3);
                            flds.Size_4 = Convert.ToInt32(size4);
                            flds.Size_5 = Convert.ToInt32(size5);
                            flds.Size_6 = Convert.ToInt32(size6);
                            flds.Size_7 = Convert.ToInt32(size7);
                            flds.Size_8 = Convert.ToInt32(size8);
                            flds.Size_9 = Convert.ToInt32(size9);
                            flds.Size_10 = Convert.ToInt32(size10);
                            flds.Size_11 = Convert.ToInt32(size11);
                            flds.Size_12 = Convert.ToInt32(size12);
                            flds.Size_13 = Convert.ToInt32(size13);
                            flds.picking_no = invno;
                            orderdtldata.Add(flds);

                        }
                    }
                }
            }
            return orderdtldata;

        }
        public List<dbSalesOrderDtlCredit> creditdata(string pickingno, string EDP)
        {
            string mySqlConnectionStr = Configuration.GetConnectionString("DBGudang");
            var usermail = User.Identity.Name;
            var salesorder = db.SalesOrderTbl.Where(y => y.EMAIL == usermail).ToList();
            List<int> listidorder = new List<int>();
            foreach (var fld in salesorder)
            {
                listidorder.Add(fld.id);
            }
            var salesdtlreal = db.SalesOrderDtlTbl.Where(y => listidorder.Contains(y.id_order)).OrderBy(y => y.id_order).ToList();
            List<dbSalesOrderDtlCredit> orderdtldata = new List<dbSalesOrderDtlCredit>();
            using (SqlConnection connection = new SqlConnection(mySqlConnectionStr))
            {

                connection.Open();

                String sql = @"  select
	                                picking_no as inv,
	                                article,
	                                sum(qty) as qty,
	                                sum(size1) as size1,
	                                sum(size2) as size2,
	                                sum(size3) as size3,
	                                sum(size4) as size4,
	                                sum(size5) as size5,
	                                sum(size6) as size6,
	                                sum(size7) as size7,
	                                sum(size8) as size8,
	                                sum(size9) as size9,
	                                sum(size10) as size10,
	                                sum(size11) as size11,
	                                sum(size12) as size12,
	                                sum(size13) as size13
                                    from dbo.dwh_trn_final_invoice_detil where store_id = '" + EDP + "'";
                             sql += @" and picking_no = '" + pickingno+"'";

                              sql += @" and yearclose = year(GETDATE()) 
	                            and wkclose is not null
	                            group by 
	                           picking_no,
	                            article

	                            order by article";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var invno = reader.GetString(0);
                            var article = reader.GetString(1);
                            var qty = reader.GetInt32(2);
                            var size1 = reader.GetInt32(3);
                            var size2 = reader.GetInt32(4);
                            var size3 = reader.GetInt32(5);
                            var size4 = reader.GetInt32(6);
                            var size5 = reader.GetInt32(7);
                            var size6 = reader.GetInt32(8);
                            var size7 = reader.GetInt32(9);
                            var size8 = reader.GetInt32(10);
                            var size9 = reader.GetInt32(11);
                            var size10 = reader.GetInt32(12);
                            var size11 = reader.GetInt32(13);
                            var size12 = reader.GetInt32(14);
                            var size13 = reader.GetInt32(15);
                            dbSalesOrderDtlCredit flds = new dbSalesOrderDtlCredit();
                            flds.article = article;
                            flds.qty = Convert.ToInt32(qty);
                            flds.price = getdatapriceUpl(article);
                            flds.final_price = flds.price * flds.qty;
                            flds.Size_1 = Convert.ToInt32(size1);
                            flds.Size_2 = Convert.ToInt32(size2);
                            flds.Size_3 = Convert.ToInt32(size3);
                            flds.Size_4 = Convert.ToInt32(size4);
                            flds.Size_5 = Convert.ToInt32(size5);
                            flds.Size_6 = Convert.ToInt32(size6);
                            flds.Size_7 = Convert.ToInt32(size7);
                            flds.Size_8 = Convert.ToInt32(size8);
                            flds.Size_9 = Convert.ToInt32(size9);
                            flds.Size_10 = Convert.ToInt32(size10);
                            flds.Size_11 = Convert.ToInt32(size11);
                            flds.Size_12 = Convert.ToInt32(size12);
                            flds.Size_13 = Convert.ToInt32(size13);
                            flds.picking_no = invno;
                            orderdtldata.Add(flds);

                        }
                    }
                }
            }
            return orderdtldata;

        }
        public List<trackingDtl> trackingdata(string pickingno, string EDP)
        {
            string mySqlConnectionStr = Configuration.GetConnectionString("DBGudang");
            var usermail = User.Identity.Name;
            //var salesorder = db.SalesOrderTbl.Where(y => y.EMAIL == usermail).ToList();
            //List<int> listidorder = new List<int>();
            //foreach (var fld in salesorder)
            //{
            //    listidorder.Add(fld.id);
            //}
            //var salesdtlreal = db.SalesOrderDtlTbl.Where(y => listidorder.Contains(y.id_order)).OrderBy(y => y.id_order).ToList();
            List<trackingDtl> trackingDtlData = new List<trackingDtl>();
            using (SqlConnection connection = new SqlConnection(mySqlConnectionStr))
            {

                connection.Open();

                String sql = @"  select
	                                invoice,
	                                store,
	                                dus,
	                                pairs,
	                                val_price,
	                                sik,
	                                trans_no,
	                                driver,
	                                date,
	                                time
                                    from dbo.dwh_gate_pass where store = '" + EDP + "'";
                sql += @" and invoice = '" + pickingno + "'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var invno = reader.GetString(0);
                            var store = reader.GetString(1);
                            var dus = reader.GetDecimal(2);
                            var pairs = reader.GetDecimal(3);
                            var val_price = reader.GetDecimal(4);
                            var sik = reader.GetString(5);
                            var trans_no = reader.GetString(6);
                            var driver = reader.GetString(7);
                            var date = reader.GetDateTime(8);
                            var time = reader.GetString(9);

                            trackingDtl flds = new trackingDtl();
                            flds.invoice = invno;
                            flds.edp = store;
                            flds.dus = dus;
                            flds.pairs = pairs;
                            flds.val_price = val_price;
                            flds.sik = sik;
                            flds.trans_no = trans_no;
                            flds.driver = driver;
                            flds.date = date;
                            flds.time = time;
                            trackingDtlData.Add(flds);

                        }
                    }
                }
            }
            return trackingDtlData;

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
        public dbSalesOrder gethdrinfo(int id)
        {
            var salesdata = db.SalesOrderTbl.Find(id);
            return salesdata;
        }
        [Authorize(Roles = "CustomerIndustrial")]
        public IActionResult Index()
        {
            var newfront = new CustomerMenuCls();
            var dataorderhdr = db.SalesOrderTbl.Where(y => y.EMAIL == User.Identity.Name).ToList();
            var datacust = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();
            //var dataorder = testorderdata();
            List<int> orderdatalist = new List<int>();
            var datacredit = new List<dbSalesOrderDtlCredit>();
            foreach(var fld in dataorderhdr)
            {
                orderdatalist.Add(fld.id);
                if (!string.IsNullOrEmpty(fld.picking_no))
                {
                    var datacreditdtl = creditdata(fld.picking_no, datacust.EDP);
                    foreach(var creditdata in datacreditdtl)
                    {
                        datacredit.Add(creditdata);
                    }

                }
            }
            var dataorder = db.SalesOrderDtlTbl.Where(y => orderdatalist.Contains(y.id_order)).ToList().Select(y => new dbSalesOrderDtl()
            {
                id_order = y.id_order,
                picking_no = gethdrinfo(y.id_order).picking_no,
                article = y.article,
                qty = y.qty,
                price = y.price,
                final_price = y.final_price,
                Size_1 = y.Size_1,
                Size_2 = y.Size_2,
                Size_3 = y.Size_3,
                Size_4 = y.Size_4,
                Size_5 = y.Size_5,
                Size_6 = y.Size_6,
                Size_7 = y.Size_7,
                Size_8 = y.Size_8,
                Size_9 = y.Size_9,
                Size_10 = y.Size_10,
                Size_11 = y.Size_11,
                Size_12 = y.Size_12,
                Size_13 = y.Size_13

            }).ToList();
            var disc = datacust.discount_customer;
            foreach(var dthdr in dataorderhdr)
            {
                var totalorder = dataorder.Where(y => y.id_order == dthdr.id).Sum(y => y.final_price);
                var dataorderdtl = dataorder.Where(y => y.id_order == dthdr.id).ToList();

                var disccust = ((totalorder * disc) / 100);
                var totalaftcustdisc = totalorder - disccust;
                var disctype = dthdr.IS_DISC_PERC;
                decimal? discount = 0;
                if(disctype == "1")
                {
                    discount = ((totalaftcustdisc * dthdr.INV_DISC) / 100);
                }
                else
                {
                    discount = dthdr.INV_DISC_AMT;
                }
                var totaldiscall = disccust + discount;
                var totalorderclean = totalorder - totaldiscall;
                foreach(var dtdtl in dataorderdtl)
                {
                    var subtotal = dtdtl.final_price;
                    var weighttage = (subtotal / totalorder) * 100;
                    dtdtl.weighttage = weighttage;
                    var discountperarticle = (totaldiscall * weighttage) / 100;
                    dtdtl.discountperarticle = discountperarticle;
                    var articleval = subtotal - discountperarticle;
                    var articleqtyval = articleval / dtdtl.qty;
                    dtdtl.articlevaltotal = articleval;
                    dtdtl.articlevalpiece = articleqtyval;
                }
                
            }

            //var datacredit = testcreditdata();
            List<dbSalesOrderDtlCredit> datacreditfront = new List<dbSalesOrderDtlCredit>();
            string stat1 = "0";
            string stat2 = "0";
            string stat3 = "0";
            string stat4 = "0";
            string stat5 = "0";
            string stat6 = "0";
            string stat7 = "0";
            string stat8 = "0";
            string stat9 = "0";
            string stat10 = "0";
            string stat11 = "0";
            string stat12 = "0";
            string stat13 = "0";

            foreach (var fld in dataorder)
            {
               foreach(var fldcredit in datacredit)
                {
                    string showcredit = "Y";
                    var validatedata = db.creditlogtbl.Where(y => y.edp == datacust.EDP && y.invno == fld.picking_no && y.year == DateTime.Now.Year).FirstOrDefault();
                    if(validatedata != null)
                    {
                       if(validatedata.isused == "1"){
                            showcredit = "N";
                        }
                    }
                    else
                    {
                        showcredit = "N";
                    }


                    if (fld.picking_no == fldcredit.picking_no && fld.article == fldcredit.article && showcredit == "Y")
                    {
                        dbSalesOrderDtlCredit creditfld = new dbSalesOrderDtlCredit();
                        creditfld.article = fld.article;
                        creditfld.picking_no = fld.picking_no;
                        int size1 = 0;
                        int size2 = 0;
                        int size3 = 0;
                        int size4 = 0;
                        int size5 = 0;
                        int size6 = 0;
                        int size7 = 0;
                        int size8 = 0;
                        int size9 = 0;
                        int size10 = 0;
                        int size11 = 0;
                        int size12 = 0;
                        int size13 = 0;

                        if (fld.Size_1 != fldcredit.Size_1)
                        {
                            size1 = fld.Size_1 - fldcredit.Size_1;
                            stat1 = "1";
                        }
                        if (fld.Size_2 != fldcredit.Size_2)
                        {
                            size2 = fld.Size_2 - fldcredit.Size_2;
                            stat2 = "1";

                        }
                        if (fld.Size_3 != fldcredit.Size_3)
                        {
                            size3 = fld.Size_3 - fldcredit.Size_3;
                            stat3 = "1";

                        }
                        if (fld.Size_4 != fldcredit.Size_4)
                        {
                            size4 = fld.Size_4 - fldcredit.Size_4;
                            stat4 = "1";

                        }
                        if (fld.Size_5 != fldcredit.Size_5)
                        {
                            size5 = fld.Size_5 - fldcredit.Size_5;
                            stat5 = "1";

                        }
                        if (fld.Size_6 != fldcredit.Size_6)
                        {
                            size6 = fld.Size_6 - fldcredit.Size_6;
                            stat6 = "1";

                        }
                        if (fld.Size_7 != fldcredit.Size_7)
                        {
                            size7 = fld.Size_7 - fldcredit.Size_7;
                            stat7 = "1";

                        }
                        if (fld.Size_8 != fldcredit.Size_8)
                        {
                            size8 = fld.Size_8 - fldcredit.Size_8;
                            stat8 = "1";

                        }
                        if (fld.Size_9 != fldcredit.Size_9)
                        {
                            size9 = fld.Size_9 - fldcredit.Size_9;
                            stat9 = "1";

                        }
                        if (fld.Size_10 != fldcredit.Size_10)
                        {
                            size10 = fld.Size_10 - fldcredit.Size_10;
                            stat10 = "1";

                        }
                        if (fld.Size_11 != fldcredit.Size_11)
                        {
                            size11 = fld.Size_11 - fldcredit.Size_11;
                            stat11 = "1";

                        }
                        if (fld.Size_12 != fldcredit.Size_12)
                        {
                            size12 = fld.Size_12 - fldcredit.Size_12;
                            stat12 = "1";

                        }
                        if (fld.Size_13 != fldcredit.Size_13)
                        {
                            size13 = fld.Size_13 - fldcredit.Size_13;
                            stat13 = "1";

                        }
                        creditfld.qty = size1 + size2 + size3 + size4 + size5 + size6 + size7 + size8 + size9+ size10 + size11 + size12 + size13;
                        creditfld.price = fld.price;
                        creditfld.final_price = creditfld.price * creditfld.qty;
                        creditfld.Size_1 = size1;
                        creditfld.Size_2 = size2;
                        creditfld.Size_3 = size3;
                        creditfld.Size_4 = size4;
                        creditfld.Size_5 = size5;
                        creditfld.Size_6 = size6;
                        creditfld.Size_7 = size7;
                        creditfld.Size_8 = size8;
                        creditfld.Size_9 = size9;
                        creditfld.Size_10 = size10;
                        creditfld.Size_11 = size11;
                        creditfld.Size_12 = size12;
                        creditfld.Size_13 = size13;
                        creditfld.creditval = (fld.articlevalpiece * creditfld.qty);

                        if (stat1 == "1" || stat2 == "1" || stat3 == "1" || stat4 == "1" || stat5 == "1" || stat6 == "1" || stat7 == "1"
                            || stat8 == "1" || stat9 == "1" || stat10 == "1" || stat11 == "1" || stat12 == "1" || stat13 == "1")
                        {
                            datacreditfront.Add(creditfld);
                        }
                    }
                }
            }
            newfront.orderList = datacreditfront;
            var creditnotetotal = datacreditfront.Sum(y => y.creditval);
            ViewBag.CreditVal = Math.Round(Convert.ToDecimal(creditnotetotal), 2);

            List<trackingModel> trackingData = new List<trackingModel>();
            foreach(var dthdr in dataorderhdr)
            {
                trackingModel flds = new trackingModel();
                flds.edp = datacust.EDP;
                if (string.IsNullOrEmpty(dthdr.picking_no))
                {
                    flds.status = "waiting for approval";
                    flds.invoice = dthdr.id.ToString();
                }
                else
                {
                    flds.invoice = dthdr.picking_no;
                    flds.status = "Order Processed";
                }
                var datacreditdtl = creditdata(dthdr.picking_no, datacust.EDP);
                if(datacreditdtl.Count() > 0)
                {
                    flds.status = "Packed";
                }
                var datatracking = trackingdata(dthdr.picking_no, datacust.EDP);
                if(datatracking.Count() > 0)
                {
                    flds.status = "On Delivery";
                }
               
                flds.update_date = DateTime.Now;
                trackingData.Add(flds);

            }
            newfront.trackingData = trackingData;
            return View(newfront);
        }
        public JsonResult getTblOrder(string inv_no)
        {
            var salesorder = db.SalesOrderTbl.Where(y => y.picking_no == inv_no).FirstOrDefault();
            var id = 0;
            if(salesorder != null)
            {
                id = salesorder.id;
            }
            //Creating List    
            List<dbSalesOrderDtl> ordertbl = db.SalesOrderDtlTbl.Where(y => y.id_order == id).ToList();
            return Json(ordertbl);
        }
        public JsonResult getTblOrderbyid(int id)
        {
            
            //Creating List    
            List<dbSalesOrderDtl> ordertbl = db.SalesOrderDtlTbl.Where(y => y.id_order == id).ToList();
            return Json(ordertbl);
        }
        public JsonResult getTblTracking(string inv_no)
        {
            var datacust = db.CustomerTbl.Where(y => y.Email == User.Identity.Name).FirstOrDefault();

           var trackingtbl = trackingdata(inv_no, datacust.EDP);
            return Json(trackingtbl);
        }
        public decimal getCreditNoteVal(string inv_no)
        {
            var dataorderhdr = db.SalesOrderTbl.Where(y => y.picking_no == inv_no).ToList();
            
            var datacust = db.CustomerTbl.Where(y => y.Email == dataorderhdr[0].EMAIL).FirstOrDefault();
            //var dataorder = testorderdata();
            List<int> orderdatalist = new List<int>();
            var datacredit = new List<dbSalesOrderDtlCredit>();
            foreach (var fld in dataorderhdr)
            {
                orderdatalist.Add(fld.id);
                if (!string.IsNullOrEmpty(fld.picking_no))
                {
                    var datacreditdtl = creditdata(fld.picking_no, datacust.EDP);
                    foreach (var creditdata in datacreditdtl)
                    {
                        datacredit.Add(creditdata);
                    }

                }
            }
            var dataorder = db.SalesOrderDtlTbl.Where(y => orderdatalist.Contains(y.id_order)).ToList().Select(y => new dbSalesOrderDtl()
            {
                id_order = y.id_order,
                picking_no = gethdrinfo(y.id_order).picking_no,
                article = y.article,
                qty = y.qty,
                price = y.price,
                final_price = y.final_price,
                Size_1 = y.Size_1,
                Size_2 = y.Size_2,
                Size_3 = y.Size_3,
                Size_4 = y.Size_4,
                Size_5 = y.Size_5,
                Size_6 = y.Size_6,
                Size_7 = y.Size_7,
                Size_8 = y.Size_8,
                Size_9 = y.Size_9,
                Size_10 = y.Size_10,
                Size_11 = y.Size_11,
                Size_12 = y.Size_12,
                Size_13 = y.Size_13

            }).ToList();
            var disc = datacust.discount_customer;
            foreach (var dthdr in dataorderhdr)
            {
                var totalorder = dataorder.Where(y => y.id_order == dthdr.id).Sum(y => y.final_price);
                var dataorderdtl = dataorder.Where(y => y.id_order == dthdr.id).ToList();

                var disccust = ((totalorder * disc) / 100);
                var totalaftcustdisc = totalorder - disccust;
                var disctype = dthdr.IS_DISC_PERC;
                decimal? discount = 0;
                if (disctype == "1")
                {
                    discount = ((totalaftcustdisc * dthdr.INV_DISC) / 100);
                }
                else
                {
                    discount = dthdr.INV_DISC_AMT;
                }
                var totaldiscall = disccust + discount;
                var totalorderclean = totalorder - totaldiscall;
                foreach (var dtdtl in dataorderdtl)
                {
                    var subtotal = dtdtl.final_price;
                    var weighttage = (subtotal / totalorder) * 100;
                    dtdtl.weighttage = weighttage;
                    var discountperarticle = (totaldiscall * weighttage) / 100;
                    dtdtl.discountperarticle = discountperarticle;
                    var articleval = subtotal - discountperarticle;
                    var articleqtyval = articleval / dtdtl.qty;
                    dtdtl.articlevaltotal = articleval;
                    dtdtl.articlevalpiece = articleqtyval;
                }

            }

            //var datacredit = testcreditdata();
            List<dbSalesOrderDtlCredit> datacreditfront = new List<dbSalesOrderDtlCredit>();
            string stat1 = "0";
            string stat2 = "0";
            string stat3 = "0";
            string stat4 = "0";
            string stat5 = "0";
            string stat6 = "0";
            string stat7 = "0";
            string stat8 = "0";
            string stat9 = "0";
            string stat10 = "0";
            string stat11 = "0";
            string stat12 = "0";
            string stat13 = "0";

            foreach (var fld in dataorder)
            {
                foreach (var fldcredit in datacredit)
                {
                    if (fld.picking_no == fldcredit.picking_no && fld.article == fldcredit.article)
                    {
                        dbSalesOrderDtlCredit creditfld = new dbSalesOrderDtlCredit();
                        creditfld.article = fld.article;
                        creditfld.picking_no = fld.picking_no;
                        int size1 = 0;
                        int size2 = 0;
                        int size3 = 0;
                        int size4 = 0;
                        int size5 = 0;
                        int size6 = 0;
                        int size7 = 0;
                        int size8 = 0;
                        int size9 = 0;
                        int size10 = 0;
                        int size11 = 0;
                        int size12 = 0;
                        int size13 = 0;

                        if (fld.Size_1 != fldcredit.Size_1)
                        {
                            size1 = fld.Size_1 - fldcredit.Size_1;
                            stat1 = "1";
                        }
                        if (fld.Size_2 != fldcredit.Size_2)
                        {
                            size2 = fld.Size_2 - fldcredit.Size_2;
                            stat2 = "1";

                        }
                        if (fld.Size_3 != fldcredit.Size_3)
                        {
                            size3 = fld.Size_3 - fldcredit.Size_3;
                            stat3 = "1";

                        }
                        if (fld.Size_4 != fldcredit.Size_4)
                        {
                            size4 = fld.Size_4 - fldcredit.Size_4;
                            stat4 = "1";

                        }
                        if (fld.Size_5 != fldcredit.Size_5)
                        {
                            size5 = fld.Size_5 - fldcredit.Size_5;
                            stat5 = "1";

                        }
                        if (fld.Size_6 != fldcredit.Size_6)
                        {
                            size6 = fld.Size_6 - fldcredit.Size_6;
                            stat6 = "1";

                        }
                        if (fld.Size_7 != fldcredit.Size_7)
                        {
                            size7 = fld.Size_7 - fldcredit.Size_7;
                            stat7 = "1";

                        }
                        if (fld.Size_8 != fldcredit.Size_8)
                        {
                            size8 = fld.Size_8 - fldcredit.Size_8;
                            stat8 = "1";

                        }
                        if (fld.Size_9 != fldcredit.Size_9)
                        {
                            size9 = fld.Size_9 - fldcredit.Size_9;
                            stat9 = "1";

                        }
                        if (fld.Size_10 != fldcredit.Size_10)
                        {
                            size10 = fld.Size_10 - fldcredit.Size_10;
                            stat10 = "1";

                        }
                        if (fld.Size_11 != fldcredit.Size_11)
                        {
                            size11 = fld.Size_11 - fldcredit.Size_11;
                            stat11 = "1";

                        }
                        if (fld.Size_12 != fldcredit.Size_12)
                        {
                            size12 = fld.Size_12 - fldcredit.Size_12;
                            stat12 = "1";

                        }
                        if (fld.Size_13 != fldcredit.Size_13)
                        {
                            size13 = fld.Size_13 - fldcredit.Size_13;
                            stat13 = "1";

                        }
                        creditfld.qty = size1 + size2 + size3 + size4 + size5 + size6 + size7 + size8 + size9 + size10 + size11 + size12 + size13;
                        creditfld.price = fld.price;
                        creditfld.final_price = creditfld.price * creditfld.qty;
                        creditfld.Size_1 = size1;
                        creditfld.Size_2 = size2;
                        creditfld.Size_3 = size3;
                        creditfld.Size_4 = size4;
                        creditfld.Size_5 = size5;
                        creditfld.Size_6 = size6;
                        creditfld.Size_7 = size7;
                        creditfld.Size_8 = size8;
                        creditfld.Size_9 = size9;
                        creditfld.Size_10 = size10;
                        creditfld.Size_11 = size11;
                        creditfld.Size_12 = size12;
                        creditfld.Size_13 = size13;
                        creditfld.creditval = (fld.articlevalpiece * creditfld.qty);

                        if (stat1 == "1" || stat2 == "1" || stat3 == "1" || stat4 == "1" || stat5 == "1" || stat6 == "1" || stat7 == "1"
                            || stat8 == "1" || stat9 == "1" || stat10 == "1" || stat11 == "1" || stat12 == "1" || stat13 == "1")
                        {
                            datacreditfront.Add(creditfld);
                        }
                    }
                }
            }
            var creditnotetotal = datacreditfront.Sum(y => y.creditval);
            var creditval = Math.Round(Convert.ToDecimal(creditnotetotal), 2);
            return creditval;
        }
        [HttpGet]
        public JsonResult GetCreditnotedata()
        {
            var dataorder = db.SalesOrderTbl.Where(y => !string.IsNullOrEmpty(y.picking_no)).ToList();
            List<dbsalescreditlog> creditlogdata = new List<dbsalescreditlog>();
            foreach(var fld in dataorder)
            {
                if(fld.picking_no == "30-003")
                {
                    dbsalescreditlog form = new dbsalescreditlog();
                    var getcredit = getCreditNoteVal(fld.picking_no);
                    var custdata = db.CustomerTbl.Where(y => y.Email == fld.EMAIL).FirstOrDefault();
                    if (getcredit != 0)
                    {
                        var validateisexist = db.creditlogtbl.Where(y => y.invno == fld.picking_no && y.edp == custdata.EDP).ToList();
                        if (validateisexist.Count() == 0)
                        {
                            form.invno = fld.picking_no;
                            form.isused = "0";
                            form.valuecredit = getcredit;
                            form.year = DateTime.Now.Year;
                            form.edp = custdata.EDP;
                            creditlogdata.Add(form);
                        }

                    }
                }
                

            }
            return Json(creditlogdata);
        }
    }
}
