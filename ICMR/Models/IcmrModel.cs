using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICMR.Models
{
    public class IcmrModel
    {

        public decimal TotalAmount { get; set; }  // ✅ correct
        public List<AvaCas> Items { get; internal set; }
        public string Price { get; internal set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public Nullable<decimal> mobile_phone { get; set; }
        public List<avashortcas> ShortItems { get; set; }

      
    }
}