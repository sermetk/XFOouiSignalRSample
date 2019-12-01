using Microsoft.AspNetCore.Mvc;
using Ooui.AspNetCore;
using OouiSignalRSample.Models;
using OouiSignalRSample.Modules;
using System.Diagnostics;
using Xamarin.Forms;

namespace OouiSignalRSample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var page = new ChatPage();
            var element = page.GetOouiElement();
            return new ElementResult(element, "Support");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
