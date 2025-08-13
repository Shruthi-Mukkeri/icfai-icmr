using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICMR.Models
{
    public class EbookCaseViewModel
    {
        // From tblEbookCases
        public int CaseId { get; set; }
        public string CaseTitle { get; set; }
        public string SubjectCategory { get; set; }
        public string casecode { get; set; }

        // From tblEbooks
        public string Title { get; set; }
        public string Volume { get; set; }
        public string Category { get; set; }
        public int Pages { get; set; }
        public decimal Prise { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string CoverImg { get; set; }
        public string EbookPdf { get; set; }
        
    }
}