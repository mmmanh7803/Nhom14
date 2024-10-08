using Microsoft.AspNetCore.Mvc;
using System.Linq;
using QLcaulacbosinhvien.Models;

namespace QLcaulacbosinhvien.Areas.Admin.Components
{
		[ViewComponent(Name ="AdminMenu")]
	public class AdminMenuComponents : ViewComponent
	{
		private DataContext _context;

		public AdminMenuComponents(DataContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{ 
			 var listofMenu = (from m in _context.AdminMenus
							   where (m.IsActive == true)
							   select m).ToList();
			return await Task.FromResult((IViewComponentResult)View("Default",listofMenu));
		}
	}
}
