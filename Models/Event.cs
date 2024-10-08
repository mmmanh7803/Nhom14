using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLcaulacbosinhvien.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public string? EventName { get; set; }

        public string? EventDescription { get; set; }

        public string? Images { get; set; }

        public DateTime EventDate { get; set; }

        public string? EndEvent { get; set; }

        public string? EventLocation { get; set; }

        public string? OrganizerId { get; set; }

        public int Point { get; set; }
        
    }
}