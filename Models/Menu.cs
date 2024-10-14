using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLcaulacbosinhvien.Models
{
    public class Menu
    {
      	public int MenuID {  get; set; }

		public string? MenuName {  get; set; }

		public string? ControllerName { get; set; }

		public string? ActionName { get; set;}

		public int Levels { get; set;}

		public int ParentID { get; set; }

		public string? Link { get; set;}

		public int MenuOrder { get; set; }

		public bool? IsShow { get; set; }

		public int Position { get; set; }

    }
}