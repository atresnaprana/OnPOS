using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataAppHR.Models
{
    public class CustomerMenuCls
    {
        public List<dbSalesOrderDtlCredit> orderList { get; set; }
        public List<trackingModel> trackingData { get; set; }
    }
}
