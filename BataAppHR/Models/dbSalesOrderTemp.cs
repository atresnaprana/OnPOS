using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BataAppHR.Models
{
    public class dbSalesOrderTemp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_customer { get; set; }

        public string EMP_CODE { get; set; }
        public decimal? TOTAL_ORDER { get; set; }
        public int TOTAL_QTY { get; set; }
        public string STATUS { get; set; }
        public string APPROVAL_1 { get; set; }
        public string APPROVAL_2 { get; set; }

        public string FLAG_AKTIF { get; set; }
        public DateTime? ORDER_DATE { get; set; }

        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string EMAIL { get; set; }

        [Range(0, 100)]
        public decimal? INV_DISC { get; set; }
        public string FLAG_SENT { get; set; }
        public string IS_DISC_PERC { get; set; }
        public decimal? INV_DISC_AMT { get; set; }
        public string SHIPPING_ADDRESS { get; set; }
        public string APPROVAL_AR { get; set; }
        [NotMapped]
        public bool isHaveCredit { get; set; }
        [NotMapped]
        public decimal? creditnoteval { get; set; }
        [NotMapped]
        public List<dbSalesOrderDtlTemp> dtltempdata { get; set; }
        [NotMapped]
        public decimal? custdisc { get; set; }
        [NotMapped]
        public decimal? custdiscTemp { get; set; }
        [NotMapped]
        public decimal? custdiscTempEdit { get; set; }
        [NotMapped]
        public decimal? custdiscTempUpl { get; set; }
        [NotMapped]
        public bool IS_DISC_PERCBoolHdr { get; set; }

        [NotMapped]
        public string datamode { get; set; }
        [NotMapped]
        public List<dbSalesOrderDtl> salesOrderDtl { get; set; }
        [NotMapped]
        public string PaidAmt { get; set; }

        [NotMapped]
        public bool isPassed { get; set; }
        [NotMapped]
        public bool isApprove1 { get; set; }
        [NotMapped]
        public bool isApprove2 { get; set; }
        [NotMapped]
        public List<dbEmployee> EmpDD { get; set; }
        [NotMapped]
        public List<dbCustomer> custDD { get; set; }
        [NotMapped]
        public string EMP_CODETemp { get; set; }
        [NotMapped]
        public int idTemp { get; set; }

        [NotMapped]
        public decimal? TOTAL_ORDERTemp { get; set; }
        [NotMapped]
        public int? id_customerTemp { get; set; }

        [NotMapped]
        public int TOTAL_QTYTemp { get; set; }

        [NotMapped]
        public string STATUSTemp { get; set; }
        [NotMapped]
        public string error { get; set; }

        [NotMapped]
        public string APPROVAL_1Temp { get; set; }

        [NotMapped]
        public string APPROVAL_2Temp { get; set; }

        [NotMapped]
        public DateTime? ORDER_DATETemp { get; set; }
        [NotMapped]
        public decimal? INV_DISCTemp { get; set; }
        [NotMapped]
        public decimal? INV_DISC_AMTTemp { get; set; }
        [NotMapped]
        public string IS_DISC_PERCTemp { get; set; }

        [NotMapped]
        public string SHIPPING_ADDRESSTemp { get; set; }

        [NotMapped]
        public string EMP_CODETempEdit { get; set; }
        [NotMapped]
        public int? id_customerTempEdit { get; set; }
        [NotMapped]
        public decimal? TOTAL_ORDERTempEdit { get; set; }

        [NotMapped]
        public int TOTAL_QTYTempEdit { get; set; }

        [NotMapped]
        public string STATUSTempEdit { get; set; }
        [NotMapped]
        public int idTempEdit { get; set; }
        [NotMapped]
        public string APPROVAL_1TempEdit { get; set; }

        [NotMapped]
        public string APPROVAL_2TempEdit { get; set; }

        [NotMapped]
        public DateTime? ORDER_DATETempEdit { get; set; }
        [NotMapped]
        public decimal? INV_DISCTempEdit { get; set; }
        [NotMapped]
        public decimal? INV_DISC_AMTTempEdit { get; set; }
        [NotMapped]
        public string IS_DISC_PERCTempEdit { get; set; }

        [NotMapped]
        public string SHIPPING_ADDRESSTempEdit { get; set; }
        [NotMapped]
        public string EMP_CODETempUpl { get; set; }

        [NotMapped]
        public decimal? TOTAL_ORDERTempUpl { get; set; }

        [NotMapped]
        public int TOTAL_QTYTempUpl { get; set; }
        [NotMapped]
        public int idTempUpl { get; set; }
        [NotMapped]
        public string STATUSTempUpl { get; set; }

        [NotMapped]
        public string APPROVAL_1TempUpl { get; set; }

        [NotMapped]
        public string APPROVAL_2TempUpl { get; set; }
        [NotMapped]
        public int? id_customerTempUpl { get; set; }
        [NotMapped]
        public string passingDate { get; set; }

        [NotMapped]
        public DateTime? ORDER_DATETempUpl { get; set; }
        [NotMapped]
        public decimal? INV_DISCTempUpl { get; set; }
        [NotMapped]
        public decimal? INV_DISC_AMTTempUpl { get; set; }
        [NotMapped]
        public string IS_DISC_PERCTempUpl { get; set; }

        [NotMapped]
        public string SHIPPING_ADDRESSTempUpl { get; set; }
        [NotMapped]
        public string article { get; set; }
        [NotMapped]
        public string size { get; set; }
        [NotMapped]
        public int qty { get; set; }
        [NotMapped]
        public decimal? price { get; set; }
        [NotMapped]
        [Range(0, 100)]
        public decimal? disc { get; set; }
        [NotMapped]
        public string is_disc_percdtl { get; set; }
        [NotMapped]
        public bool is_disc_percBool { get; set; }
        [NotMapped]
        public decimal? disc_amt { get; set; }
        [NotMapped]
        public decimal? final_price { get; set; }
        [NotMapped]
        public int Size_1 { get; set; }
        [NotMapped]
        public int Size_2 { get; set; }
        [NotMapped]
        public int Size_3 { get; set; }
        [NotMapped]
        public int Size_4 { get; set; }
        [NotMapped]
        public int Size_5 { get; set; }
        [NotMapped]
        public int Size_6 { get; set; }
        [NotMapped]
        public int Size_7 { get; set; }
        [NotMapped]
        public int Size_8 { get; set; }
        [NotMapped]
        public int Size_9 { get; set; }
        [NotMapped]
        public int Size_10 { get; set; }
        [NotMapped]
        public int Size_11 { get; set; }
        [NotMapped]
        public int Size_12 { get; set; }
        [NotMapped]
        public int Size_13 { get; set; }

        [NotMapped]
        public string articleTemp { get; set; }
        [NotMapped]
        public string sizeTemp { get; set; }
        [NotMapped]
        public int qtyTemp { get; set; }
        [NotMapped]
        public decimal? priceTemp { get; set; }
        [NotMapped]
        public decimal? discTemp { get; set; }
        [NotMapped]
        public decimal? final_priceTemp { get; set; }

        [NotMapped]
        public string articleEdit { get; set; }
        [NotMapped]
        public string sizeEdit { get; set; }
        [NotMapped]
        public int qtyEdit { get; set; }
        [NotMapped]
        public decimal? priceEdit { get; set; }
        [NotMapped]
        public string is_disc_percEdit { get; set; }
        [NotMapped]
        public bool is_disc_percBoolEdit { get; set; }
        [NotMapped]
        public decimal? disc_amtEdit { get; set; }
        [NotMapped]
        [Range(0, 100)]
        public decimal? discEdit { get; set; }
        [NotMapped]
        public decimal? final_priceEdit { get; set; }
        [NotMapped]
        public int Size_1_Edit { get; set; }
        [NotMapped]
        public int Size_2_Edit { get; set; }
        [NotMapped]
        public int Size_3_Edit { get; set; }
        [NotMapped]
        public int Size_4_Edit { get; set; }
        [NotMapped]
        public int Size_5_Edit { get; set; }
        [NotMapped]
        public int Size_6_Edit { get; set; }
        [NotMapped]
        public int Size_7_Edit { get; set; }
        [NotMapped]
        public int Size_8_Edit { get; set; }
        [NotMapped]
        public int Size_9_Edit { get; set; }
        [NotMapped]
        public int Size_10_Edit { get; set; }
        [NotMapped]
        public int Size_11_Edit { get; set; }
        [NotMapped]
        public int Size_12_Edit { get; set; }
        [NotMapped]
        public int Size_13_Edit { get; set; }
        [NotMapped]
        public string articleTempEdit { get; set; }
        [NotMapped]
        public string sizeTempEdit { get; set; }
        [NotMapped]
        public int qtyTempEdit { get; set; }
        [NotMapped]
        public decimal? priceTempEdit { get; set; }
        [NotMapped]
        public decimal? discTempEdit { get; set; }
        [NotMapped]
        public decimal? final_priceTempEdit { get; set; }
        [NotMapped]
        public List<Article> articleList { get; set; }
        [NotMapped]
        public List<dbArticle> articleListTrans { get; set; }
        [NotMapped]
        public List<ArticleSize> sizeList { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileuploadArticle { get; set; }
    }
}
