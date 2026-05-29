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
    public class QuyDinhRequest { public int MaQuyDinh { get; set; } public string TieuDe { get; set; } public string NoiDung { get; set; } public int TrangThai { get; set; } }
}
