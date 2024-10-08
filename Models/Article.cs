using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLcaulacbosinhvien.Models
{
    public class Article
    {
       
        public int ArticleID { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? Image { get; set; }

        public DateTime Date { get; set; }

        public int View { get; set; }

        public bool Status { get; set; }

        public int MemberID { get; set; }
        
    }
}