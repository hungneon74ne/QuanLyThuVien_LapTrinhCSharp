using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Business;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Controllers
{
    // [Authorize(Roles = "ThuThu")]
    public class ThuThuController : Controller
    {
        private readonly QuanLyThuVienDbContext _context;
        private readonly DocGiaBusiness _docGiaBusiness;
        private readonly SachBusiness _sachBusiness;
        private readonly PhieuMuonBusiness _phieuMuonBusiness;
        private readonly PhieuTraBusiness _phieuTraBusiness;
        private readonly ThongBaoBusiness _thongBaoBusiness;

        public ThuThuController(
            QuanLyThuVienDbContext context,
            DocGiaBusiness docGiaBusiness,
            SachBusiness sachBusiness,
            PhieuMuonBusiness phieuMuonBusiness,
            PhieuTraBusiness phieuTraBusiness,
            ThongBaoBusiness thongBaoBusiness)
        {
            _context = context;
            _docGiaBusiness = docGiaBusiness;
            _sachBusiness = sachBusiness;
            _phieuMuonBusiness = phieuMuonBusiness;
            _phieuTraBusiness = phieuTraBusiness;
            _thongBaoBusiness = thongBaoBusiness;
        }

        private int? GetCurrentThuThuId()
        {
            var maNguoiDungStr = HttpContext.Session.GetString("MaNguoiDung");
            if (int.TryParse(maNguoiDungStr, out int maNguoiDung))
            {
                var nguoiDung = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == maNguoiDung);
                if (nguoiDung != null && nguoiDung.Quyen == QuyenNguoiDung.ThuThu)
                {
                    return maNguoiDung;
                }
            }
            return null;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            var maNguoiDung = GetCurrentThuThuId();
            if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var thuThu = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == maNguoiDung.Value);

            // Thống kê
            ViewBag.ThuThu = thuThu;
            ViewBag.TongDocGia = _context.DocGias.Count();
            ViewBag.TongSach = _context.Sachs.Count(s => s.DaXoa == 0);
            ViewBag.YeuCauChoDuyet = _context.PhieuMuons.Count(p => p.TrangThai == TrangThaiPhieuMuon.ChoDuyet);
            ViewBag.SachDangMuon = _context.ChiTietPhieuMuons.Count(ct => ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon);
            ViewBag.SachQuaHan = _context.ChiTietPhieuMuons
                .Count(ct => ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon && ct.PhieuMuon.HanTra < DateTime.Now);

            // Yêu cầu mượn gần đây
            ViewBag.RecentRequests = _context.PhieuMuons
                .Include(p => p.DocGia)
                    .ThenInclude(d => d.NguoiDung)
                .Where(p => p.TrangThai == TrangThaiPhieuMuon.ChoDuyet)
                .OrderByDescending(p => p.NgayMuon)
                .Take(5)
                .ToList();

            // Sách quá hạn
            ViewBag.OverdueBooks = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                    .ThenInclude(p => p.DocGia)
                        .ThenInclude(d => d.NguoiDung)
                .Include(ct => ct.Sach)
                .Where(ct => ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon && ct.PhieuMuon.HanTra < DateTime.Now)
                .OrderBy(ct => ct.PhieuMuon.HanTra)
                .Take(5)
                .ToList();

            return View();
        }

        public IActionResult QuanLyDocGia()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var docGias = _context.DocGias
                .Include(d => d.NguoiDung)
                .Include(d => d.VaiTroDocGia)
                .ToList();
            return View(docGias);
        }

        public IActionResult QuanLySach()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var sachs = _context.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Include(s => s.NhaXuatBan)
                .Where(s => s.DaXoa == 0)
                .ToList();
            ViewBag.TheLoais = _context.TheLoais.ToList();
            return View(sachs);
        }

        public IActionResult QuanLyTacGia()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var tacGias = _context.TacGias.ToList();
            return View(tacGias);
        }

        public IActionResult QuanLyTheLoai()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var theLoais = _context.TheLoais.ToList();
            return View(theLoais);
        }

        public IActionResult QuanLyNhaXuatBan()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var nhaXuatBans = _context.NhaXuatBans.ToList();
            return View(nhaXuatBans);
        }

        public IActionResult QuanLyVaiTro()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var vaiTros = _context.VaiTroDocGias.ToList();
            return View(vaiTros);
        }

        public IActionResult YeuCauMuon()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var yeuCau = _context.PhieuMuons
                .Include(p => p.DocGia)
                    .ThenInclude(d => d.NguoiDung)
                .Include(p => p.ChiTietPhieuMuons)
                    .ThenInclude(ct => ct.Sach)
                .Where(p => p.TrangThai == TrangThaiPhieuMuon.ChoDuyet)
                .OrderByDescending(p => p.NgayMuon)
                .ToList();
            return View(yeuCau);
        }

        public IActionResult PhieuDangMuon()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var dangMuon = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                    .ThenInclude(p => p.DocGia)
                        .ThenInclude(d => d.NguoiDung)
                .Include(ct => ct.Sach)
                .Where(ct => ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon)
                .ToList();
            return View(dangMuon);
        }

        public IActionResult TraSach()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var dangMuon = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                    .ThenInclude(p => p.DocGia)
                        .ThenInclude(d => d.NguoiDung)
                .Include(ct => ct.Sach)
                .Where(ct => ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon)
                .ToList();
            return View(dangMuon);
        }

        public IActionResult QuaHan()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var quaHan = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                    .ThenInclude(p => p.DocGia)
                        .ThenInclude(d => d.NguoiDung)
                .Include(ct => ct.Sach)
                .Where(ct => ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon && ct.PhieuMuon.HanTra < DateTime.Now)
                .OrderBy(ct => ct.PhieuMuon.HanTra)
                .ToList();
            return View(quaHan);
        }

        public IActionResult LichSuMuonTra()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var lichSu = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                    .ThenInclude(p => p.DocGia)
                        .ThenInclude(d => d.NguoiDung)
                .Include(ct => ct.Sach)
                .Include(ct => ct.PhieuTra)
                .Where(ct => ct.TrangThaiTra == 1)
                .OrderByDescending(ct => ct.PhieuMuon.NgayMuon)
                .ToList();
            return View(lichSu);
        }

        public IActionResult ThongBao()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var thongBaos = _context.ThongBaos
                .Include(t => t.NguoiDung)
                .OrderByDescending(t => t.NgayTao)
                .ToList();
            return View(thongBaos);
        }

        public IActionResult QuyDinhThuVien()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var quyDinhs = _context.QuyDinhs
                .Where(q => q.TrangThai == 1)
                .OrderByDescending(q => q.NgayTao)
                .ToList();
            return View(quyDinhs);
        }

        public IActionResult ThongKe()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            // Thống kê
            ViewBag.TongSach = _context.Sachs.Count(s => s.DaXoa == 0);
            ViewBag.TongDocGia = _context.DocGias.Count();
            ViewBag.TongMuon = _context.PhieuMuons.Count(p => p.TrangThai == TrangThaiPhieuMuon.DangMuon);
            ViewBag.TongTra = _context.ChiTietPhieuMuons.Count(ct => ct.TrangThaiTra == 1);

            // Top sách được mượn nhiều
            ViewBag.TopSach = _context.ChiTietPhieuMuons
                .Include(ct => ct.Sach)
                .GroupBy(ct => ct.MaSach)
                .Select(g => new { Sach = g.First().Sach, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(10)
                .ToList();

            // Top độc giả mượn nhiều
            ViewBag.TopDocGia = _context.PhieuMuons
                .Include(p => p.DocGia)
                    .ThenInclude(d => d.NguoiDung)
                .GroupBy(p => p.MaDocGia)
                .Select(g => new { DocGia = g.First().DocGia, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(10)
                .ToList();

            return View();
        }

        public IActionResult DanhSachSach()
        {
            return RedirectToAction("QuanLySach");
        }

        public IActionResult TimKiemSach(string keyword)
        {
            return RedirectToAction("QuanLySach");
        }

        public IActionResult TiepNhanYeuCauMuon()
        {
            return RedirectToAction("YeuCauMuon");
        }

        public IActionResult LapPhieuMuon()
        {
            return RedirectToAction("YeuCauMuon");
        }

        public IActionResult LapPhieuTra()
        {
            return RedirectToAction("TraSach");
        }

        public IActionResult TinhTienPhat(int id)
        {
            return RedirectToAction("TraSach");
        }

        public IActionResult XemSachQuaHan()
        {
            return RedirectToAction("QuaHan");
        }

        public IActionResult GuiThongBao()
        {
            return RedirectToAction("ThongBao");
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "Không có file được chọn" });
            }

            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "books", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageUrl = $"/images/books/{fileName}";
                return Json(new { success = true, imageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
