using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLcaulacbosinhvien.Models;

namespace QLcaulacbosinhvien.Components
{
    [ViewComponent(Name = "MenuView")]  
    public class MenuViewComponents : ViewComponent
    {
        private readonly DataContext _context;
        public MenuViewComponents(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = (from m in _context.Menus
                        where (m.IsActive == true) && (m.Position == 1)
                        select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", list));
        }
        
        
    }
}