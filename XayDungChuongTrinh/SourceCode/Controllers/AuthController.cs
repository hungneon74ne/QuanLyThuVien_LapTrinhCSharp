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

        // GET: /Auth/Register
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("MaNguoiDung") != null)
            {
                var quyen = (QuyenNguoiDung)int.Parse(HttpContext.Session.GetString("Quyen"));
                return RedirectBasedOnRole(quyen);
            }
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public IActionResult Register(string username, string password, string hoten, string email, string sodienthoai, string gioitinh, DateTime? ngaysinh, string diachi, string socccd)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hoten))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin bắt buộc (Tên đăng nhập, Mật khẩu, Họ tên)!";
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin bắt buộc!";
                return View();
            }

            var success = _authBusiness.RegisterDocGia(username, password, hoten, email, sodienthoai, gioitinh, ngaysinh, diachi, socccd);
            if (success)
            {
                TempData["SuccessMessage"] = "Đăng ký tài khoản độc giả thành công! Hãy đăng nhập để bắt đầu.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại trong hệ thống!";
                TempData["ErrorMessage"] = "Tên đăng nhập đã tồn tại!";
                return View();
            }
        }

        // GET: /Auth/TestUsers
        public IActionResult TestUsers()
        {
            try
            {
                var users = _authBusiness.Login("admin", "123456"); // This triggers the logging logic we added
                return Json(new { message = "Logged to console. Check the server logs." });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
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

                // Lưu thông báo thành công dạng Toast
                TempData["SuccessMessage"] = $"Chào mừng {user.HoTen} đã quay trở lại!";

                // Redirect theo quyền
                return RedirectBasedOnRole(user.Quyen);
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Bạn đã đăng xuất tài khoản thành công!";
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
