using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Business;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly QuanLyThuVienDbContext _context;
        private readonly NguoiDungBusiness _nguoiDungBusiness;
        private readonly QuyDinhBusiness _quyDinhBusiness;
        private readonly SachBusiness _sachBusiness;
        private readonly PhieuMuonBusiness _phieuMuonBusiness;

        public AdminController(
            QuanLyThuVienDbContext context,
            NguoiDungBusiness nguoiDungBusiness,
            QuyDinhBusiness quyDinhBusiness,
            SachBusiness sachBusiness,
            PhieuMuonBusiness phieuMuonBusiness)
        {
            _context = context;
            _nguoiDungBusiness = nguoiDungBusiness;
            _quyDinhBusiness = quyDinhBusiness;
            _sachBusiness = sachBusiness;
            _phieuMuonBusiness = phieuMuonBusiness;
        }

        private int? GetCurrentAdminId()
        {
            var maNguoiDungStr = HttpContext.Session.GetString("MaNguoiDung");
            if (int.TryParse(maNguoiDungStr, out int maNguoiDung))
            {
                var nguoiDung = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == maNguoiDung);
                if (nguoiDung != null && nguoiDung.Quyen == QuyenNguoiDung.Admin)
                {
                    return maNguoiDung;
                }
            }
            return null;
        }

        public IActionResult Index()
        {
            if (GetCurrentAdminId() == null) return RedirectToAction("Login", "Auth");

            // Thống kê
            ViewBag.TotalSach = _context.Sachs.Where(s => s.DaXoa == 0).Count();
            ViewBag.TotalDocGia = _context.DocGias.Where(d => d.DaXoa == 0).Count();
            ViewBag.TotalThuThu = _context.NguoiDungs.Where(n => n.Quyen == QuyenNguoiDung.ThuThu && n.TrangThai == TrangThaiTaiKhoan.HoatDong).Count();
            ViewBag.TotalPhieuMuon = _context.PhieuMuons.Where(p => p.TrangThai == TrangThaiPhieuMuon.DangMuon).Count();
            ViewBag.TotalPhieuQuaHan = _context.ChiTietPhieuMuons
                .Include(c => c.PhieuMuon)
                .Where(c => c.PhieuMuon.HanTra < DateTime.Now && c.TrangThaiTra == 0 && c.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon)
                .Count();

            // Top sách được mượn nhiều nhất
            ViewBag.TopSach = _context.ChiTietPhieuMuons
                .Include(c => c.Sach)
                .GroupBy(c => c.MaSach)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new { Sach = g.First().Sach, Count = g.Count() })
                .ToList();

            // Top độc giả mượn nhiều nhất
            ViewBag.TopDocGia = _context.PhieuMuons
                .Include(p => p.DocGia)
                .ThenInclude(d => d.NguoiDung)
                .GroupBy(p => p.MaDocGia)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new { DocGia = g.First().DocGia, Count = g.Count() })
                .ToList();

            // Thống kê mượn trả theo tháng
            var currentYear = DateTime.Now.Year;
            ViewBag.ThongKeThang = _context.PhieuMuons
                .Where(p => p.NgayMuon.Year == currentYear)
                .GroupBy(p => p.NgayMuon.Month)
                .OrderBy(g => g.Key)
                .Select(g => new { Thang = g.Key, Count = g.Count() })
                .ToList();

            return View();
        }

        public IActionResult QuanLyNguoiDung()
        {
            if (GetCurrentAdminId() == null) return RedirectToAction("Login", "Auth");

            var users = _context.NguoiDungs
                .Where(n => n.Quyen == QuyenNguoiDung.ThuThu || n.Quyen == QuyenNguoiDung.Admin)
                .ToList();

            return View(users);
        }

        public IActionResult ToggleUserStatus(int id)
        {
            if (GetCurrentAdminId() == null) return RedirectToAction("Login", "Auth");

            var user = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == id);
            if (user != null)
            {
                user.TrangThai = user.TrangThai == TrangThaiTaiKhoan.HoatDong ? TrangThaiTaiKhoan.Khoa : TrangThaiTaiKhoan.HoatDong;
                _context.SaveChanges();
            }

            return RedirectToAction("QuanLyNguoiDung");
        }

        public IActionResult QuyDinhThuVien()
        {
            if (GetCurrentAdminId() == null) return RedirectToAction("Login", "Auth");

            var quyDinhs = _context.QuyDinhs
                .OrderByDescending(q => q.NgayTao)
                .ToList();
            return View(quyDinhs);
        }

        [HttpGet]
        public IActionResult GetQuyDinh(int id)
        {
            if (GetCurrentAdminId() == null) return Unauthorized();

            var qd = _context.QuyDinhs.FirstOrDefault(q => q.MaQuyDinh == id);
            if (qd == null) return NotFound();

            return Json(new { success = true, data = qd });
        }

        [HttpPost]
        public IActionResult SaveQuyDinh(int? maQuyDinh, string tieuDe, string noiDung, int trangThai)
        {
            if (GetCurrentAdminId() == null) return RedirectToAction("Login", "Auth");

            if (string.IsNullOrEmpty(tieuDe) || string.IsNullOrEmpty(noiDung))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin bắt buộc.";
                return RedirectToAction("QuyDinhThuVien");
            }

            if (maQuyDinh.HasValue && maQuyDinh.Value > 0)
            {
                // Update
                var qd = _context.QuyDinhs.FirstOrDefault(q => q.MaQuyDinh == maQuyDinh.Value);
                if (qd != null)
                {
                    qd.TieuDe = tieuDe;
                    qd.NoiDung = noiDung;
                    qd.TrangThai = trangThai;
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật quy định thành công!";
                }
            }
            else
            {
                // Insert
                var qd = new QuyDinh
                {
                    TieuDe = tieuDe,
                    NoiDung = noiDung,
                    TrangThai = trangThai,
                    NgayTao = DateTime.Now
                };
                _context.QuyDinhs.Add(qd);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Thêm mới quy định thành công!";
            }

            return RedirectToAction("QuyDinhThuVien");
        }

        [HttpPost]
        public IActionResult DeleteQuyDinh(int id)
        {
            if (GetCurrentAdminId() == null) return Unauthorized();

            var qd = _context.QuyDinhs.FirstOrDefault(q => q.MaQuyDinh == id);
            if (qd != null)
            {
                _context.QuyDinhs.Remove(qd);
                _context.SaveChanges();
                return Json(new { success = true, message = "Xóa quy định thành công!" });
            }
            return Json(new { success = false, message = "Không tìm thấy quy định." });
        }
    }
}
