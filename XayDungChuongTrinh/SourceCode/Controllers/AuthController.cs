using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Business;
using QuanLyThuVien.Models;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthBusiness _authBusiness;

        public AuthController(AuthBusiness authBusiness)
        {
            _authBusiness = authBusiness;
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            // Nếu đã đăng nhập thì redirect theo quyền
            if (HttpContext.Session.GetString("MaNguoiDung") != null)
            {
                var quyen = (QuyenNguoiDung)int.Parse(HttpContext.Session.GetString("Quyen"));
                return RedirectBasedOnRole(quyen);
            }
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _authBusiness.Login(username, password);

            if (user != null)
            {
                // Lưu session
                HttpContext.Session.SetString("MaNguoiDung", user.MaNguoiDung.ToString());
                HttpContext.Session.SetString("TenDangNhap", user.TenDangNhap);
                HttpContext.Session.SetString("HoTen", user.HoTen);
                HttpContext.Session.SetString("Quyen", ((int)user.Quyen).ToString());

                // Redirect theo quyền
                return RedirectBasedOnRole(user.Quyen);
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: /Auth/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectBasedOnRole(QuyenNguoiDung quyen)
        {
            switch (quyen)
            {
                case QuyenNguoiDung.Admin:
                    return RedirectToAction("Index", "Admin");
                case QuyenNguoiDung.ThuThu:
                    return RedirectToAction("Index", "ThuThu");
                case QuyenNguoiDung.DocGia:
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("AccessDenied");
            }
        }
    }
}
