using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICMR.Models
{
    public class CartViewModel
    {

        public int Id { get; set; }
        public string casetitle { get; set; }
        public string Type { get; set; }
        public Nullable<double> pricers { get; set; }
        public List<AvaCas> AvaCases { get; set; }
        public List<avashortcas> AvaShortCases { get; set; }
        
    }
}