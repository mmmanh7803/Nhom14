using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLcaulacbosinhvien.Models;


namespace QLcaulacbosinhvien.ViewModels
{
    public class EventViewModel
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



    }
}
