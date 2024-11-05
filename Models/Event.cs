using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QLcaulacbosinhvien.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public string? EventName { get; set; }

        public string? EventDescription { get; set; }

        public string? Images { get; set; }

        public DateTime? EventDate { get; set; }

        public DateTime? EndEvent { get; set; }

        public string? EventLocation { get; set; }

        public int Point { get; set; }

        public bool? Status {get; set;} 

        public int AccountID {get; set;}

        public int MemberJoin {get; set;}
         public bool IsActive => DateTime.Now >= EventDate && DateTime.Now <= EndEvent;

          public Account Account { get; set; }

         public ICollection<EventMember> EventMembers { get; set; }
    }
}