using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Business;

namespace QuanLyThuVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly SachBusiness _sachBusiness;

        public HomeController(SachBusiness sachBusiness)
        {
            _sachBusiness = sachBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Books()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
