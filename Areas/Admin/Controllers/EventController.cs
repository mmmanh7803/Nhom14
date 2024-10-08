using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace QLcaulacbosinhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        public IActionResult Read()
        {
            return View();
        }
    }
}