using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICMR.Models
{
   
    public class TextBookCaseViewModel
    {
        public object CaseId { get; set; }
        public object CaseTitle { get; set; }
        public object SubjectCategory { get; set; }
        public object Title { get; set; }
        public object Volume { get; set; }
        public object Category { get; set; }
        public int Pages { get; set; }
        public decimal Prise { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public object CoverImg { get; set; }
        public object EbookPdf { get; set; }



        public int TextBookId { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string LocalIpAddress { get; set; }
        public string IpAddress { get; set; }
        public Nullable<int> TextBookPages { get; set; }
        public Nullable<int> WorkBookPages { get; set; }
        public Nullable<decimal> TextBookprise { get; set; }
        public Nullable<decimal> WorkBookprise { get; set; }
        public string TextBookPdf { get; set; }
        public string Overview { get; set; }



        public int Id { get; set; }
        public string CoverImage { get; set; }
        public virtual tblTextBook tblTextBook { get; set; }


        public Nullable<int> ChapterNo { get; set; }
        public string ChapterTitle { get; set; }
        public string Keywords { get; set; }

        public string WorkBookPdf { get; set; }
       

        public List<tblTextBookImg> tblTextBookImgs { get; set; } = new List<tblTextBookImg>();
        public List<tblChapter> tblChapters { get; set; } = new List<tblChapter>();
        public List<tblTextBookImg> CoverImages { get; internal set; }
        public List<tblChapter> Chapters { get; internal set; }
    }
}
