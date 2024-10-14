using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLcaulacbosinhvien.Models
{
    public class Member
    {
        public int MemberID { get; set; }

        public string? MemberName { get; set; }

        public string? MemberEmail { get; set; }

        public string? MemberPhone { get; set; }

        public string? MemberMSV { get; set; }

        public string? Images { get; set; }

        public DateTime JoinDate { get; set; }

        public int Point { get; set; }

        public bool? Status { get; set; }

        public Account? account {get; set;}
    
        
    }
}