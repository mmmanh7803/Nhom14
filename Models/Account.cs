using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLcaulacbosinhvien.Models
{
    public class Account
    {
        public int AccountID { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public int RoleID { get; set; }

        public int MemberID {get; set;}

        public Member? member {get; set;}

   
        
    }
}