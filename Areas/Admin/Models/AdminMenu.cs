using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLcaulacbosinhvien.Areas.Admin.Models
{
    public class AdminMenu
    {
        public int AdminMenuID { get; set; }

       public string? ItemName { get; set; }

       public int ItemLevel { get; set; }

       public int ParentLevel { get; set; }

       public int ItemOrder { get; set; }

       public bool? IsActive { get; set; }

       public string? AreaName { get; set; }

       public string? ControllerName { get; set; }

       public string? ActionName { get; set; }

       public string? Icon { get; set; }
    }
}