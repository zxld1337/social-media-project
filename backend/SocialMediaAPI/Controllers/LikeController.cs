using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SocialMediaAPI.Controllers
{
    public class LikeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
