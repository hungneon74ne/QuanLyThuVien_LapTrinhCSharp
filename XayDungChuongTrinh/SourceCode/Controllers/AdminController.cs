using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Business;

namespace QuanLyThuVien.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly NguoiDungBusiness _nguoiDungBusiness;
        private readonly QuyDinhBusiness _quyDinhBusiness;

        public AdminController(NguoiDungBusiness nguoiDungBusiness, QuyDinhBusiness quyDinhBusiness)
        {
            _nguoiDungBusiness = nguoiDungBusiness;
            _quyDinhBusiness = quyDinhBusiness;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult QuanLyTaiKhoan()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLyPhanQuyen()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLySach()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLyTacGia()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLyTheLoai()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLyNhaXuatBan()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLyDocGia()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLyQuyDinh()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult QuanLyDanhGia()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult XemThongKe()
        {
            throw new System.NotImplementedException();
        }
    }
}
