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

            var tacGias = _context.TacGias.Where(t => t.DaXoa == 0).ToList();
            return View(tacGias);
        }

        public IActionResult QuanLyTheLoai()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var theLoais = _context.TheLoais.Where(t => t.DaXoa == 0).ToList();
            return View(theLoais);
        }

        public IActionResult QuanLyNhaXuatBan()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var nhaXuatBans = _context.NhaXuatBans.Where(t => t.DaXoa == 0).ToList();
            return View(nhaXuatBans);
        }

        public IActionResult QuanLyVaiTro()
        {
            // Tạm thời bỏ qua kiểm tra quyền để test
            // var maNguoiDung = GetCurrentThuThuId();
            // if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var vaiTros = _context.VaiTroDocGias.Where(t => t.DaXoa == 0).ToList();
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

        public IActionResult LichSuMuonTra(string keyword = null, string tuNgay = null, string denNgay = null)
        {
            var query = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon).ThenInclude(p => p.DocGia).ThenInclude(d => d.NguoiDung)
                .Include(ct => ct.Sach).Include(ct => ct.PhieuTra)
                .Where(ct => ct.TrangThaiTra == 1)
                .AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(ct => ct.Sach.TenSach.Contains(keyword) || ct.PhieuMuon.DocGia.NguoiDung.HoTen.Contains(keyword));
            if (DateTime.TryParse(tuNgay, out var ngayBd)) query = query.Where(ct => ct.PhieuMuon.NgayMuon >= ngayBd);
            if (DateTime.TryParse(denNgay, out var ngayKt)) query = query.Where(ct => ct.PhieuMuon.NgayMuon <= ngayKt);
            return View(query.OrderByDescending(ct => ct.PhieuMuon.NgayMuon).ToList());
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

            // Biểu đồ thể loại
            var theLoaiChart = _context.Sachs
                .Where(s => s.DaXoa == 0 && s.TheLoai != null)
                .Include(s => s.TheLoai)
                .AsEnumerable()
                .GroupBy(s => s.TheLoai.TenTheLoai)
                .Select(g => new { ten = g.Key, soLuong = g.Count() })
                .ToList();
            ViewBag.TheLoaiChartJson = System.Text.Json.JsonSerializer.Serialize(theLoaiChart);

            // Biểu đồ mượn theo tháng (năm hiện tại)
            var namNay = DateTime.Now.Year;
            var muonThang = _context.PhieuMuons
                .Where(p => p.NgayMuon.Year == namNay)
                .AsEnumerable()
                .GroupBy(p => p.NgayMuon.Month)
                .Select(g => new { Thang = g.Key, SoLuong = g.Count() })
                .ToList();
            var muonThangArr = Enumerable.Range(1, 12)
                .Select(m => muonThang.FirstOrDefault(x => x.Thang == m)?.SoLuong ?? 0)
                .ToArray();
            ViewBag.MuonThangChartJson = System.Text.Json.JsonSerializer.Serialize(muonThangArr);

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
                return Json(new { success = false, message = "Không có file được chọn" });
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "books", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);
                return Json(new { success = true, imageUrl = $"/images/books/{fileName}" });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ===== DROPDOWNS =====
        [HttpGet]
        public IActionResult GetDropdowns()
        {
            var tacGias = _context.TacGias.Where(t => t.DaXoa == 0).Select(t => new { t.MaTacGia, t.TenTacGia }).ToList();
            var theLoais = _context.TheLoais.Where(t => t.DaXoa == 0).Select(t => new { t.MaTheLoai, t.TenTheLoai }).ToList();
            var nhaXuatBans = _context.NhaXuatBans.Where(t => t.DaXoa == 0).Select(t => new { t.MaNhaXuatBan, t.TenNhaXuatBan }).ToList();
            return Json(new { tacGias, theLoais, nhaXuatBans });
        }

        // ===== SÁCH =====
        [HttpGet]
        public IActionResult GetSachById(int id)
        {
            var s = _context.Sachs.FirstOrDefault(x => x.MaSach == id);
            if (s == null) return Json(new { success = false });
            return Json(new { success = true, data = new { s.MaSach, s.ISBN, s.TenSach, s.MaTacGia, s.MaTheLoai, s.MaNhaXuatBan, s.NamXuatBan, s.SoLuongTong, s.SoLuongHienCo, s.SoTrang, s.GiaTien, s.HinhAnh, s.MoTa } });
        }

        [HttpPost]
        public IActionResult SaveSach([FromBody] SachRequest req)
        {
            try
            {
                if (req.MaSach == 0)
                {
                    _context.Sachs.Add(new Sach { ISBN = req.ISBN, TenSach = req.TenSach, MaTacGia = req.MaTacGia, MaTheLoai = req.MaTheLoai, MaNhaXuatBan = req.MaNhaXuatBan, NamXuatBan = req.NamXuatBan, SoLuongTong = req.SoLuongTong, SoLuongHienCo = req.SoLuongHienCo, SoTrang = req.SoTrang, GiaTien = req.GiaTien, HinhAnh = req.HinhAnh, MoTa = req.MoTa, NgayTao = DateTime.Now, DaXoa = 0 });
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Thêm sách thành công!" });
                }
                else
                {
                    var s = _context.Sachs.FirstOrDefault(x => x.MaSach == req.MaSach);
                    if (s == null) return Json(new { success = false, message = "Không tìm thấy sách!" });
                    s.ISBN = req.ISBN; s.TenSach = req.TenSach; s.MaTacGia = req.MaTacGia; s.MaTheLoai = req.MaTheLoai; s.MaNhaXuatBan = req.MaNhaXuatBan;
                    s.NamXuatBan = req.NamXuatBan; s.SoLuongTong = req.SoLuongTong; s.SoLuongHienCo = req.SoLuongHienCo; s.SoTrang = req.SoTrang; s.GiaTien = req.GiaTien; s.MoTa = req.MoTa;
                    if (!string.IsNullOrEmpty(req.HinhAnh)) s.HinhAnh = req.HinhAnh;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật sách thành công!" });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult DeleteSach([FromBody] IdRequest req)
        {
            var s = _context.Sachs.FirstOrDefault(x => x.MaSach == req.Id);
            if (s == null) return Json(new { success = false, message = "Không tìm thấy sách!" });
            s.DaXoa = 1; s.NgayXoa = DateTime.Now; _context.SaveChanges();
            return Json(new { success = true, message = "Xóa sách thành công!" });
        }

        // ===== TÁC GIẢ =====
        [HttpGet]
        public IActionResult GetTacGiaById(int id)
        {
            var t = _context.TacGias.FirstOrDefault(x => x.MaTacGia == id);
            if (t == null) return Json(new { success = false });
            return Json(new { success = true, data = new { t.MaTacGia, t.TenTacGia, t.TieuSu } });
        }

        [HttpPost]
        public IActionResult SaveTacGia([FromBody] TacGiaRequest req)
        {
            try
            {
                if (req.MaTacGia == 0)
                {
                    _context.TacGias.Add(new TacGia { TenTacGia = req.TenTacGia, TieuSu = req.TieuSu, DaXoa = 0 });
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Thêm tác giả thành công!" });
                }
                else
                {
                    var t = _context.TacGias.FirstOrDefault(x => x.MaTacGia == req.MaTacGia);
                    if (t == null) return Json(new { success = false, message = "Không tìm thấy tác giả!" });
                    t.TenTacGia = req.TenTacGia; t.TieuSu = req.TieuSu;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật tác giả thành công!" });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult DeleteTacGia([FromBody] IdRequest req)
        {
            var t = _context.TacGias.FirstOrDefault(x => x.MaTacGia == req.Id);
            if (t == null) return Json(new { success = false, message = "Không tìm thấy tác giả!" });

            var hasBooks = _context.Sachs.Any(s => s.MaTacGia == req.Id && s.DaXoa == 0);
            if (hasBooks)
            {
                return Json(new { success = false, message = "Không thể xóa tác giả này vì đang có sách thuộc tác giả này trong thư viện!" });
            }

            t.DaXoa = 1; t.NgayXoa = DateTime.Now; _context.SaveChanges();
            return Json(new { success = true, message = "Xóa tác giả thành công!" });
        }

        // ===== THỂ LOẠI =====
        [HttpGet]
        public IActionResult GetTheLoaiById(int id)
        {
            var t = _context.TheLoais.FirstOrDefault(x => x.MaTheLoai == id);
            if (t == null) return Json(new { success = false });
            return Json(new { success = true, data = new { t.MaTheLoai, t.TenTheLoai } });
        }

        [HttpPost]
        public IActionResult SaveTheLoai([FromBody] TheLoaiRequest req)
        {
            try
            {
                if (req.MaTheLoai == 0)
                {
                    _context.TheLoais.Add(new TheLoai { TenTheLoai = req.TenTheLoai, DaXoa = 0 });
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Thêm thể loại thành công!" });
                }
                else
                {
                    var t = _context.TheLoais.FirstOrDefault(x => x.MaTheLoai == req.MaTheLoai);
                    if (t == null) return Json(new { success = false, message = "Không tìm thấy thể loại!" });
                    t.TenTheLoai = req.TenTheLoai; _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật thể loại thành công!" });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult DeleteTheLoai([FromBody] IdRequest req)
        {
            var t = _context.TheLoais.FirstOrDefault(x => x.MaTheLoai == req.Id);
            if (t == null) return Json(new { success = false, message = "Không tìm thấy thể loại!" });

            var hasBooks = _context.Sachs.Any(s => s.MaTheLoai == req.Id && s.DaXoa == 0);
            if (hasBooks)
            {
                return Json(new { success = false, message = "Không thể xóa thể loại này vì đang có sách thuộc thể loại này trong thư viện!" });
            }

            t.DaXoa = 1; t.NgayXoa = DateTime.Now; _context.SaveChanges();
            return Json(new { success = true, message = "Xóa thể loại thành công!" });
        }

        // ===== NHÀ XUẤT BẢN =====
        [HttpGet]
        public IActionResult GetNhaXuatBanById(int id)
        {
            var n = _context.NhaXuatBans.FirstOrDefault(x => x.MaNhaXuatBan == id);
            if (n == null) return Json(new { success = false });
            return Json(new { success = true, data = new { n.MaNhaXuatBan, n.TenNhaXuatBan, n.DiaChi, n.SoDienThoai } });
        }

        [HttpPost]
        public IActionResult SaveNhaXuatBan([FromBody] NhaXuatBanRequest req)
        {
            try
            {
                if (req.MaNhaXuatBan == 0)
                {
                    _context.NhaXuatBans.Add(new NhaXuatBan { TenNhaXuatBan = req.TenNhaXuatBan, DiaChi = req.DiaChi, SoDienThoai = req.SoDienThoai, DaXoa = 0 });
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Thêm nhà xuất bản thành công!" });
                }
                else
                {
                    var n = _context.NhaXuatBans.FirstOrDefault(x => x.MaNhaXuatBan == req.MaNhaXuatBan);
                    if (n == null) return Json(new { success = false, message = "Không tìm thấy NXB!" });
                    n.TenNhaXuatBan = req.TenNhaXuatBan; n.DiaChi = req.DiaChi; n.SoDienThoai = req.SoDienThoai;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật NXB thành công!" });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult DeleteNhaXuatBan([FromBody] IdRequest req)
        {
            var n = _context.NhaXuatBans.FirstOrDefault(x => x.MaNhaXuatBan == req.Id);
            if (n == null) return Json(new { success = false, message = "Không tìm thấy NXB!" });

            var hasBooks = _context.Sachs.Any(s => s.MaNhaXuatBan == req.Id && s.DaXoa == 0);
            if (hasBooks)
            {
                return Json(new { success = false, message = "Không thể xóa nhà xuất bản này vì đang có sách thuộc nhà xuất bản này trong thư viện!" });
            }

            n.DaXoa = 1; n.NgayXoa = DateTime.Now; _context.SaveChanges();
            return Json(new { success = true, message = "Xóa NXB thành công!" });
        }

        // ===== VAI TRÒ =====
        [HttpGet]
        public IActionResult GetVaiTroById(int id)
        {
            var v = _context.VaiTroDocGias.FirstOrDefault(x => x.MaVaiTro == id);
            if (v == null) return Json(new { success = false });
            return Json(new { success = true, data = new { v.MaVaiTro, v.TenVaiTro } });
        }

        [HttpPost]
        public IActionResult SaveVaiTro([FromBody] VaiTroRequest req)
        {
            try
            {
                if (req.MaVaiTro == 0)
                {
                    _context.VaiTroDocGias.Add(new VaiTroDocGia { TenVaiTro = req.TenVaiTro, DaXoa = 0 });
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Thêm vai trò thành công!" });
                }
                else
                {
                    var v = _context.VaiTroDocGias.FirstOrDefault(x => x.MaVaiTro == req.MaVaiTro);
                    if (v == null) return Json(new { success = false, message = "Không tìm thấy vai trò!" });
                    v.TenVaiTro = req.TenVaiTro; _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật vai trò thành công!" });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult DeleteVaiTro([FromBody] IdRequest req)
        {
            var v = _context.VaiTroDocGias.FirstOrDefault(x => x.MaVaiTro == req.Id);
            if (v == null) return Json(new { success = false, message = "Không tìm thấy vai trò!" });
            v.DaXoa = 1; v.NgayXoa = DateTime.Now; _context.SaveChanges();
            return Json(new { success = true, message = "Xóa vai trò thành công!" });
        }

        // ===== ĐỘC GIẢ =====
        [HttpGet]
        public IActionResult GetVaiTros()
        {
            var vts = _context.VaiTroDocGias.Where(v => v.DaXoa == 0).Select(v => new { v.MaVaiTro, v.TenVaiTro }).ToList();
            return Json(new { success = true, data = vts });
        }

        [HttpGet]
        public IActionResult GetDocGiaById(int id)
        {
            var dg = _context.DocGias.Include(d => d.NguoiDung).FirstOrDefault(x => x.MaDocGia == id);
            if (dg == null) return Json(new { success = false });
            return Json(new { success = true, data = new {
                dg.MaDocGia, dg.MaNguoiDung, tenDocGia = dg.TenDocGia, dg.GioiTinh,
                ngaySinh = dg.NgaySinh?.ToString("yyyy-MM-dd"),
                dg.SoDienThoai, dg.Email, dg.DiaChi, dg.SoCCCD,
                ngayBatDau = dg.NgayBatDau.ToString("yyyy-MM-dd"),
                ngayKetThuc = dg.NgayKetThuc?.ToString("yyyy-MM-dd"),
                dg.MaVaiTro,
                tenDangNhap = dg.NguoiDung?.TenDangNhap,
                trangThai = (int)(dg.NguoiDung?.TrangThai ?? TrangThaiTaiKhoan.HoatDong)
            }});
        }

        [HttpPost]
        public IActionResult SaveDocGia([FromBody] DocGiaRequest req)
        {
            try
            {
                if (req.MaDocGia == 0)
                {
                    var nd = new NguoiDung { HoTen = req.HoTen, TenDangNhap = req.TenDangNhap, MatKhau = req.MatKhau ?? "123456",
                        SoDienThoai = req.SoDienThoai, Email = req.Email, Quyen = QuyenNguoiDung.DocGia, TrangThai = TrangThaiTaiKhoan.HoatDong };
                    _context.NguoiDungs.Add(nd); _context.SaveChanges();
                    var dg = new DocGia { MaNguoiDung = nd.MaNguoiDung, TenDocGia = req.HoTen, GioiTinh = req.GioiTinh,
                        NgaySinh = req.NgaySinh, SoDienThoai = req.SoDienThoai, Email = req.Email, DiaChi = req.DiaChi,
                        SoCCCD = req.SoCCCD, NgayBatDau = req.NgayBatDau ?? DateTime.Today, NgayKetThuc = req.NgayKetThuc,
                        MaVaiTro = req.MaVaiTro, DaXoa = 0 };
                    _context.DocGias.Add(dg); _context.SaveChanges();
                    return Json(new { success = true, message = "Thêm độc giả thành công!" });
                }
                else
                {
                    var dg = _context.DocGias.Include(d => d.NguoiDung).FirstOrDefault(x => x.MaDocGia == req.MaDocGia);
                    if (dg == null) return Json(new { success = false, message = "Không tìm thấy độc giả!" });
                    dg.TenDocGia = req.HoTen; dg.GioiTinh = req.GioiTinh; dg.NgaySinh = req.NgaySinh;
                    dg.SoDienThoai = req.SoDienThoai; dg.Email = req.Email; dg.DiaChi = req.DiaChi;
                    dg.SoCCCD = req.SoCCCD; if (req.NgayBatDau.HasValue) dg.NgayBatDau = req.NgayBatDau.Value;
                    dg.NgayKetThuc = req.NgayKetThuc; dg.MaVaiTro = req.MaVaiTro;
                    if (dg.NguoiDung != null)
                    {
                        dg.NguoiDung.HoTen = req.HoTen; dg.NguoiDung.SoDienThoai = req.SoDienThoai; dg.NguoiDung.Email = req.Email;
                        dg.NguoiDung.TrangThai = req.TrangThai == 1 ? TrangThaiTaiKhoan.HoatDong : TrangThaiTaiKhoan.Khoa;
                        if (!string.IsNullOrEmpty(req.MatKhau)) dg.NguoiDung.MatKhau = req.MatKhau;
                    }
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật độc giả thành công!" });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult DeleteDocGia([FromBody] IdRequest req)
        {
            var dg = _context.DocGias.Include(d => d.NguoiDung).FirstOrDefault(x => x.MaDocGia == req.Id);
            if (dg == null) return Json(new { success = false, message = "Không tìm thấy độc giả!" });
            dg.DaXoa = 1;
            if (dg.NguoiDung != null) dg.NguoiDung.TrangThai = TrangThaiTaiKhoan.Khoa;
            _context.SaveChanges();
            return Json(new { success = true, message = "Xóa độc giả thành công!" });
        }

        // ===== TRẢ SÁCH =====
        [HttpPost]
        public IActionResult XacNhanTraSach([FromBody] TraSachRequest req)
        {
            try
            {
                var ct = _context.ChiTietPhieuMuons.Include(c => c.Sach).Include(c => c.PhieuMuon)
                    .FirstOrDefault(x => x.MaChiTiet == req.ChiTietId);
                if (ct == null) return Json(new { success = false, message = "Không tìm thấy chi tiết phiếu mượn!" });
                int.TryParse(HttpContext.Session.GetString("MaNguoiDung"), out int maNV);
                _context.PhieuTras.Add(new PhieuTra { MaChiTiet = req.ChiTietId, NgayTra = DateTime.Now,
                    TinhTrangSach = req.TinhTrang, TienPhat = req.TienPhat, GhiChu = req.GhiChu, MaNhanVien = maNV > 0 ? maNV : 1 });
                ct.TrangThaiTra = 1;
                if (ct.Sach != null) ct.Sach.SoLuongHienCo++;
                var tatCa = _context.ChiTietPhieuMuons.Where(c => c.MaPhieuMuon == ct.MaPhieuMuon).ToList();
                bool tatCaDaTra = tatCa.All(c => c.MaChiTiet == req.ChiTietId || c.TrangThaiTra == 1);
                if (tatCaDaTra && ct.PhieuMuon != null) ct.PhieuMuon.TrangThai = TrangThaiPhieuMuon.DaTra;
                _context.SaveChanges();
                return Json(new { success = true, message = "Xác nhận trả sách thành công!" });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        // ===== DUYỆT / TỪ CHỐI PHIẾU MƯỢN =====
        [HttpPost]
        public IActionResult DuyetYeuCauMuon([FromBody] IdRequest req)
        {
            var phieu = _context.PhieuMuons.FirstOrDefault(p => p.MaPhieuMuon == req.Id);
            if (phieu == null) return Json(new { success = false, message = "Không tìm thấy phiếu mượn!" });
            var chiTiets = _context.ChiTietPhieuMuons.Include(ct => ct.Sach).Where(ct => ct.MaPhieuMuon == req.Id).ToList();
            foreach (var ct in chiTiets)
            {
                if (ct.Sach == null || ct.Sach.SoLuongHienCo <= 0)
                    return Json(new { success = false, message = $"Sách '{ct.Sach?.TenSach}' đã hết!" });
                ct.Sach.SoLuongHienCo--;
            }
            int.TryParse(HttpContext.Session.GetString("MaNguoiDung"), out int maNV);
            phieu.TrangThai = TrangThaiPhieuMuon.DangMuon;
            phieu.MaNhanVien = maNV > 0 ? maNV : (int?)null;
            phieu.NgayCapNhat = DateTime.Now;
            _context.SaveChanges();
            return Json(new { success = true, message = "Duyệt phiếu mượn thành công!" });
        }

        [HttpPost]
        public IActionResult TuChoiYeuCauMuon([FromBody] RejectRequest req)
        {
            var phieu = _context.PhieuMuons.FirstOrDefault(p => p.MaPhieuMuon == req.Id);
            if (phieu == null) return Json(new { success = false, message = "Không tìm thấy phiếu mượn!" });
            phieu.TrangThai = TrangThaiPhieuMuon.TuChoi;
            phieu.LyDoTuChoi = req.Reason;
            phieu.NgayCapNhat = DateTime.Now;
            _context.SaveChanges();
            return Json(new { success = true, message = "Đã từ chối phiếu mượn!" });
        }

        [HttpGet]
        public IActionResult GetDocGiaList()
        {
            var list = _context.DocGias.Where(d => d.DaXoa == 0)
                .Select(d => new { d.MaDocGia, d.TenDocGia, d.SoDienThoai }).ToList();
            return Json(list);
        }

        // ===== THÔNG BÁO =====
        [HttpPost]
        public IActionResult GuiThongBao([FromBody] ThongBaoRequest req)
        {
            try
            {
                if (req.GuiDen == "all")
                {
                    var nguoiDungs = _context.DocGias.Where(d => d.DaXoa == 0).Select(d => d.MaNguoiDung).ToList();
                    foreach (var maNd in nguoiDungs)
                        _context.ThongBaos.Add(new ThongBao { MaNguoiDung = maNd, TieuDe = req.TieuDe, NoiDung = req.NoiDung, DaDoc = 0, NgayTao = DateTime.Now });
                }
                else if (req.MaDocGia > 0)
                {
                    var dg = _context.DocGias.FirstOrDefault(d => d.MaDocGia == req.MaDocGia);
                    if (dg != null)
                        _context.ThongBaos.Add(new ThongBao { MaNguoiDung = dg.MaNguoiDung, TieuDe = req.TieuDe, NoiDung = req.NoiDung, DaDoc = 0, NgayTao = DateTime.Now });
                }
                _context.SaveChanges();
                return Json(new { success = true, message = "Gửi thông báo thành công!" });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult XoaThongBao([FromBody] IdRequest req)
        {
            var tb = _context.ThongBaos.FirstOrDefault(x => x.MaThongBao == req.Id);
            if (tb == null) return Json(new { success = false, message = "Không tìm thấy thông báo!" });
            _context.ThongBaos.Remove(tb); _context.SaveChanges();
            return Json(new { success = true, message = "Xóa thông báo thành công!" });
        }

        // ===== QUY ĐỊNH =====
        [HttpGet]
        public IActionResult GetQuyDinhById(int id)
        {
            var q = _context.QuyDinhs.FirstOrDefault(x => x.MaQuyDinh == id);
            if (q == null) return Json(new { success = false });
            return Json(new { success = true, data = new { q.MaQuyDinh, q.TieuDe, q.NoiDung, q.TrangThai } });
        }

        [HttpPost]
        public IActionResult SaveQuyDinh([FromBody] QuyDinhRequest req)
        {
            try
            {
                if (req.MaQuyDinh == 0)
                {
                    _context.QuyDinhs.Add(new QuyDinh { TieuDe = req.TieuDe, NoiDung = req.NoiDung, TrangThai = req.TrangThai, NgayTao = DateTime.Now });
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Thêm quy định thành công!" });
                }
                else
                {
                    var q = _context.QuyDinhs.FirstOrDefault(x => x.MaQuyDinh == req.MaQuyDinh);
                    if (q == null) return Json(new { success = false, message = "Không tìm thấy quy định!" });
                    q.TieuDe = req.TieuDe; q.NoiDung = req.NoiDung; q.TrangThai = req.TrangThai;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật quy định thành công!" });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult DeleteQuyDinh([FromBody] IdRequest req)
        {
            var q = _context.QuyDinhs.FirstOrDefault(x => x.MaQuyDinh == req.Id);
            if (q == null) return Json(new { success = false, message = "Không tìm thấy quy định!" });
            _context.QuyDinhs.Remove(q); _context.SaveChanges();
            return Json(new { success = true, message = "Xóa quy định thành công!" });
        }
    }

    // ===== DTO CLASSES =====
    public class IdRequest { public int Id { get; set; } }
    public class RejectRequest { public int Id { get; set; } public string Reason { get; set; } }
    public class SachRequest { public int MaSach { get; set; } public string ISBN { get; set; } public string TenSach { get; set; } public int? MaTacGia { get; set; } public int? MaTheLoai { get; set; } public int? MaNhaXuatBan { get; set; } public int? NamXuatBan { get; set; } public int SoLuongTong { get; set; } public int SoLuongHienCo { get; set; } public int? SoTrang { get; set; } public decimal? GiaTien { get; set; } public string HinhAnh { get; set; } public string MoTa { get; set; } }
    public class TacGiaRequest { public int MaTacGia { get; set; } public string TenTacGia { get; set; } public string TieuSu { get; set; } }
    public class TheLoaiRequest { public int MaTheLoai { get; set; } public string TenTheLoai { get; set; } }
    public class NhaXuatBanRequest { public int MaNhaXuatBan { get; set; } public string TenNhaXuatBan { get; set; } public string DiaChi { get; set; } public string SoDienThoai { get; set; } }
    public class VaiTroRequest { public int MaVaiTro { get; set; } public string TenVaiTro { get; set; } }
    public class DocGiaRequest { public int MaDocGia { get; set; } public string HoTen { get; set; } public string GioiTinh { get; set; } public DateTime? NgaySinh { get; set; } public string SoDienThoai { get; set; } public string Email { get; set; } public string DiaChi { get; set; } public string SoCCCD { get; set; } public DateTime? NgayBatDau { get; set; } public DateTime? NgayKetThuc { get; set; } public int? MaVaiTro { get; set; } public string TenDangNhap { get; set; } public string MatKhau { get; set; } public int TrangThai { get; set; } = 1; }
    public class TraSachRequest { public int ChiTietId { get; set; } public int TinhTrang { get; set; } public decimal TienPhat { get; set; } public string GhiChu { get; set; } }
    public class ThongBaoRequest { public string TieuDe { get; set; } public string NoiDung { get; set; } public string GuiDen { get; set; } public int MaDocGia { get; set; } }
    public class QuyDinhRequest { public int MaQuyDinh { get; set; } public string TieuDe { get; set; } public string NoiDung { get; set; } public int TrangThai { get; set; } }
}
