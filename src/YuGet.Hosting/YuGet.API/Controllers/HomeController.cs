using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YuGet.API.Controllers
{
	
	public class HomeController : Controller
	{
		[HttpGet("Home/Index")]
		public IActionResult Index()
		{
			return Content(DateTime.Now.ToLongTimeString());
		}
	}
}
