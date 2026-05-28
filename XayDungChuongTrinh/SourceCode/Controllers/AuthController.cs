using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Business;

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
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Gọi _authBusiness để xử lý đăng nhập
            // Nếu thành công thì redirect dựa trên Quyen (Admin, ThuThu, DocGia)
            // Nếu thất bại trả về thông báo lỗi
            throw new System.NotImplementedException();
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            // Xóa session/cookie đăng nhập
            throw new System.NotImplementedException();
        }

        // GET: /Auth/AccessDenied
        public IActionResult AccessDenied()
        {
            // Hiển thị trang báo lỗi không có quyền truy cập
            return View();
        }
    }
}
