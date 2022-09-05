using Microsoft.AspNetCore.Mvc;

namespace Practice_Core6Book.Controllers
{
    public class ChattingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
