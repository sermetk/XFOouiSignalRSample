using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ooui.AspNetCore;
using OouiSignalRSample.Core.Hubs;
using OouiSignalRSample.Models;
using OouiSignalRSample.Modules;
using OouiSignalRSample.Modules.Support;
using System.Diagnostics;
using Xamarin.Forms;

namespace OouiSignalRSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<ChatHub, IChatHub> hubContext;
        public HomeController(IHubContext<ChatHub, IChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Route("/Home")]
        public IActionResult Index()
        {
            var chatPage = new ChatPage {
                BindingContext = new ChatPageViewModel(hubContext)
            };            
            var page = new NavigationPage(chatPage);
            var element = page.GetOouiElement();
            return new ElementResult(element, "Support");
        }

        [HttpGet]
        [Route("/Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
