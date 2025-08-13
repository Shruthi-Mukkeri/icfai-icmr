using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICMR.Models
{
    public class CartItem
    {
        public string userid { get; set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public string casetitle { get; set; }
        public DateTime? sdate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string amount { get; set; }
        public Nullable<decimal> mobile_phone { get; set; }

    }

}