using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Enums;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace QuanLyThuVien.Controllers
{
    public class DocGiaController : Controller
    {
        private readonly QuanLyThuVienDbContext _context;

        public DocGiaController(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var userId = GetCurrentUserId();
            if (userId.HasValue)
            {
                ViewBag.UnreadCount = _context.ThongBaos.Count(t => t.MaNguoiDung == userId.Value && t.DaDoc == 0);
            }
            else
            {
                ViewBag.UnreadCount = 0;
            }
        }

        private int? GetCurrentDocGiaId()
        {
            var maNguoiDung = HttpContext.Session.GetString("MaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDung)) return null;

            var ndId = int.Parse(maNguoiDung);
            var docGia = _context.DocGias.FirstOrDefault(d => d.MaNguoiDung == ndId);
            return docGia?.MaDocGia;
        }

        private int? GetCurrentUserId()
        {
            var maNguoiDung = HttpContext.Session.GetString("MaNguoiDung");
            if (string.IsNullOrEmpty(maNguoiDung)) return null;
            return int.Parse(maNguoiDung);
        }

        public IActionResult Dashboard()
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var docGia = _context.DocGias
                .Include(d => d.VaiTroDocGia)
                .FirstOrDefault(d => d.MaDocGia == maDocGia.Value);

            ViewBag.DocGia = docGia;

            ViewBag.SoSachDangMuon = _context.ChiTietPhieuMuons
                .Count(ct => ct.PhieuMuon.MaDocGia == maDocGia.Value && ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon);
            
            ViewBag.SoSachYeuThich = _context.SachYeuThichs.Count(s => s.MaDocGia == maDocGia.Value);
            
            ViewBag.SoLichSuMuon = _context.PhieuMuons.Count(p => p.MaDocGia == maDocGia.Value);

            ViewBag.ThongBaos = _context.ThongBaos
                .Where(t => t.MaNguoiDung == docGia.MaNguoiDung)
                .OrderByDescending(t => t.NgayTao)
                .Take(5)
                .ToList();

            ViewBag.RecentLoans = _context.PhieuMuons
                .Where(p => p.MaDocGia == maDocGia.Value)
                .OrderByDescending(p => p.NgayMuon)
                .Take(5)
                .ToList();

            ViewBag.NewBooks = _context.Sachs
                .Include(s => s.TacGia)
                .Where(s => s.DaXoa == 0)
                .OrderByDescending(s => s.NgayTao)
                .Take(4)
                .ToList();

            return View();
        }

        public IActionResult ThongTinCaNhan()
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var docGia = _context.DocGias
                .Include(d => d.VaiTroDocGia)
                .FirstOrDefault(d => d.MaDocGia == maDocGia.Value);

            ViewBag.Roles = _context.VaiTroDocGias.Where(r => r.DaXoa == 0).ToList();

            return View(docGia);
        }

        [HttpPost]
        public IActionResult CapNhatThongTinCaNhan(string tenDocGia, string gioiTinh, DateTime? ngaySinh, string soDienThoai, string email, string diaChi, string soCCCD, int maVaiTro)
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var docGia = _context.DocGias.FirstOrDefault(d => d.MaDocGia == maDocGia.Value);
            if (docGia != null)
            {
                docGia.TenDocGia = tenDocGia;
                docGia.GioiTinh = gioiTinh;
                docGia.NgaySinh = ngaySinh;
                docGia.SoDienThoai = soDienThoai;
                docGia.Email = email;
                docGia.DiaChi = diaChi;
                docGia.SoCCCD = soCCCD;
                docGia.MaVaiTro = maVaiTro;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            }
            return RedirectToAction("ThongTinCaNhan");
        }

        [HttpPost]
        public IActionResult DoiMatKhau(string matKhauCu, string matKhauMoi, string xacNhanMatKhauMoi)
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            if (matKhauMoi != xacNhanMatKhauMoi)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp!";
                return RedirectToAction("ThongTinCaNhan");
            }

            var nd = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == maNguoiDung.Value);
            if (nd != null && nd.MatKhau == matKhauCu)
            {
                nd.MatKhau = matKhauMoi;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Mật khẩu cũ không chính xác!";
            }
            return RedirectToAction("ThongTinCaNhan");
        }

        public IActionResult ChiTietSach(int id)
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var sach = _context.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Include(s => s.NhaXuatBan)
                .Include(s => s.DanhGiaSachs)
                    .ThenInclude(d => d.DocGia)
                .FirstOrDefault(s => s.MaSach == id && s.DaXoa == 0);

            if (sach == null) return NotFound();

            ViewBag.IsFavorited = _context.SachYeuThichs.Any(s => s.MaDocGia == maDocGia.Value && s.MaSach == id);

            return View(sach);
        }

        [HttpPost]
        public IActionResult GuiYeuCauMuonSach([FromBody] Dictionary<string, int[]> data)
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return Json(new { success = false, message = "Chưa đăng nhập!" });

            if (data == null || !data.ContainsKey("sachIds") || data["sachIds"].Length == 0)
                return Json(new { success = false, message = "Không có sách nào được chọn!" });

            var sachIds = data["sachIds"];

            // Kiểm tra tính hợp lệ của từng sách trước khi tạo phiếu mượn
            foreach (var sId in sachIds)
            {
                var sach = _context.Sachs.FirstOrDefault(s => s.MaSach == sId && s.DaXoa == 0);
                if (sach == null)
                    return Json(new { success = false, message = "Sách không tồn tại!" });

                if (sach.SoLuongHienCo <= 0)
                    return Json(new { success = false, message = $"Sách '{sach.TenSach}' đã hết bản in trong kho, không thể mượn!" });

                // Kiểm tra xem độc giả đã mượn hoặc đang yêu cầu mượn cuốn này chưa
                var daDangKy = _context.ChiTietPhieuMuons
                    .Any(ct => ct.MaSach == sId && 
                               ct.PhieuMuon.MaDocGia == maDocGia.Value && 
                               ct.TrangThaiTra == 0 && 
                               (ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.ChoDuyet || 
                                ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon));
                
                if (daDangKy)
                    return Json(new { success = false, message = $"Bạn đang mượn hoặc đã gửi yêu cầu mượn sách '{sach.TenSach}' trước đó!" });
            }

            var phieuMuon = new PhieuMuon
            {
                MaDocGia = maDocGia.Value,
                NgayMuon = DateTime.Now,
                HanTra = DateTime.Now.AddDays(14),
                TrangThai = TrangThaiPhieuMuon.ChoDuyet
            };
            _context.PhieuMuons.Add(phieuMuon);
            _context.SaveChanges();

            foreach (var sId in sachIds)
            {
                var ct = new ChiTietPhieuMuon
                {
                    MaPhieuMuon = phieuMuon.MaPhieuMuon,
                    MaSach = sId,
                    TrangThaiTra = 0
                };
                _context.ChiTietPhieuMuons.Add(ct);
            }
            _context.SaveChanges();

            return Json(new { success = true, message = "Đã gửi yêu cầu mượn sách thành công!" });
        }

        [HttpPost]
        public IActionResult HuyYeuCauMuonSach([FromBody] Dictionary<string, int> data)
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return Json(new { success = false, message = "Chưa đăng nhập!" });

            if (data == null || !data.ContainsKey("id")) return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            
            var id = data["id"];
            var phieu = _context.PhieuMuons.FirstOrDefault(p => p.MaPhieuMuon == id && p.MaDocGia == maDocGia.Value && p.TrangThai == TrangThaiPhieuMuon.ChoDuyet);
            if (phieu != null)
            {
                phieu.TrangThai = TrangThaiPhieuMuon.DaHuy;
                _context.SaveChanges();
                return Json(new { success = true, message = "Đã hủy yêu cầu mượn sách!" });
            }
            return Json(new { success = false, message = "Không thể hủy yêu cầu này!" });
        }

        public IActionResult TheoDoiTrangThaiMuon()
        {
            return RedirectToAction("Dashboard");
        }

        public IActionResult YeuCauMuon()
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var list = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                .Include(ct => ct.Sach)
                    .ThenInclude(s => s.TacGia)
                .Where(ct => ct.PhieuMuon.MaDocGia == maDocGia.Value && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.ChoDuyet)
                .OrderByDescending(ct => ct.PhieuMuon.NgayMuon)
                .ToList();

            return View(list);
        }

        public IActionResult DanhSachDangMuon()
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var list = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                .Include(ct => ct.Sach)
                    .ThenInclude(s => s.TacGia)
                .Where(ct => ct.PhieuMuon.MaDocGia == maDocGia.Value && ct.TrangThaiTra == 0 && ct.PhieuMuon.TrangThai == TrangThaiPhieuMuon.DangMuon)
                .ToList();

            ViewBag.OverdueBooksCount = list.Count(ct => ct.PhieuMuon.HanTra.HasValue && ct.PhieuMuon.HanTra.Value < DateTime.Now);

            return View(list);
        }

        public IActionResult LichSuMuon()
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var list = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon)
                .Include(ct => ct.Sach)
                .Include(ct => ct.PhieuTra)
                .Where(ct => ct.PhieuMuon.MaDocGia == maDocGia.Value && ct.PhieuMuon.TrangThai != TrangThaiPhieuMuon.ChoDuyet)
                .OrderByDescending(ct => ct.PhieuMuon.NgayMuon)
                .ToList();

            ViewBag.TongSoLanMuon = list.Count;
            ViewBag.DaTraDungHan = list.Count(ct => ct.TrangThaiTra == (int)TrangThaiTraSach.DaTra && ct.PhieuTra != null && ct.PhieuTra.TienPhat == 0);
            ViewBag.TongTienPhat = list.Where(ct => ct.PhieuTra != null).Sum(ct => ct.PhieuTra.TienPhat);

            return View(list);
        }

        public IActionResult NhanThongBao()
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue) return RedirectToAction("Login", "Auth");

            var list = _context.ThongBaos
                .Where(t => t.MaNguoiDung == maNguoiDung.Value)
                .OrderByDescending(t => t.NgayTao)
                .ToList();

            return View(list);
        }

        [HttpPost]
        public IActionResult DanhDauDaDocTatCa()
        {
            var maNguoiDung = GetCurrentUserId();
            if (!maNguoiDung.HasValue) return Json(new { success = false, message = "Chưa đăng nhập!" });

            var unread = _context.ThongBaos.Where(t => t.MaNguoiDung == maNguoiDung.Value && t.DaDoc == 0).ToList();
            foreach (var t in unread)
            {
                t.DaDoc = 1;
            }
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetNotificationsJson()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Json(new { success = false, message = "Chưa đăng nhập!" });

            var thongBaos = _context.ThongBaos
                .Where(t => t.MaNguoiDung == userId.Value)
                .OrderByDescending(t => t.NgayTao)
                .Take(15)
                .Select(t => new {
                    t.MaThongBao,
                    t.TieuDe,
                    t.NoiDung,
                    t.DaDoc,
                    NgayTao = t.NgayTao.ToString("yyyy-MM-dd HH:mm:ss"),
                    ThoiGianText = GetRelativeTime(t.NgayTao)
                })
                .ToList();

            return Json(new { success = true, data = thongBaos });
        }

        [HttpPost]
        public IActionResult DanhDauDaDoc([FromBody] Dictionary<string, int> data)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Json(new { success = false, message = "Chưa đăng nhập!" });

            if (data == null || !data.ContainsKey("id")) return Json(new { success = false, message = "Lỗi!" });
            var id = data["id"];

            var t = _context.ThongBaos.FirstOrDefault(x => x.MaThongBao == id && x.MaNguoiDung == userId.Value);
            if (t != null)
            {
                t.DaDoc = 1;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Không tìm thấy thông báo!" });
        }

        private static string GetRelativeTime(DateTime dt)
        {
            var span = DateTime.Now - dt;
            if (span.TotalDays > 30) return dt.ToString("dd/MM/yyyy");
            if (span.TotalDays >= 1) return $"{(int)span.TotalDays} ngày trước";
            if (span.TotalHours >= 1) return $"{(int)span.TotalHours} giờ trước";
            if (span.TotalMinutes >= 1) return $"{(int)span.TotalMinutes} phút trước";
            return "Vừa xong";
        }

        [HttpPost]
        public IActionResult LuuSachYeuThich([FromBody] Dictionary<string, int> data)
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return Json(new { success = false, message = "Chưa đăng nhập!" });

            if (data == null || !data.ContainsKey("id")) return Json(new { success = false, message = "Lỗi!" });
            var sachId = data["id"];

            var exists = _context.SachYeuThichs.Any(s => s.MaDocGia == maDocGia.Value && s.MaSach == sachId);
            if (!exists)
            {
                _context.SachYeuThichs.Add(new SachYeuThich 
                { 
                    MaDocGia = maDocGia.Value, 
                    MaSach = sachId,
                    NgayThem = DateTime.Now
                });
                _context.SaveChanges();
            }
            return Json(new { success = true, message = "Đã thêm vào yêu thích!" });
        }

        [HttpPost]
        public IActionResult XoaSachYeuThich([FromBody] Dictionary<string, int> data)
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return Json(new { success = false, message = "Chưa đăng nhập!" });

            if (data == null || !data.ContainsKey("id")) return Json(new { success = false, message = "Lỗi!" });
            var sachId = data["id"];

            var item = _context.SachYeuThichs.FirstOrDefault(s => s.MaDocGia == maDocGia.Value && s.MaSach == sachId);
            if (item != null)
            {
                _context.SachYeuThichs.Remove(item);
                _context.SaveChanges();
            }
            return Json(new { success = true, message = "Đã xóa khỏi yêu thích!" });
        }

        public IActionResult SachYeuThich()
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var favoriteBooks = _context.SachYeuThichs
                .Include(s => s.Sach)
                    .ThenInclude(s => s.TacGia)
                .Include(s => s.Sach)
                    .ThenInclude(s => s.TheLoai)
                .Where(s => s.MaDocGia == maDocGia.Value)
                .Select(s => s.Sach)
                .ToList();

            return View(favoriteBooks);
        }

        [HttpPost]
        public IActionResult DanhGiaSach(int sachId, int soSao, string binhLuan)
        {
            var maDocGia = GetCurrentDocGiaId();
            if (!maDocGia.HasValue) return RedirectToAction("Login", "Auth");

            var dg = _context.DanhGiaSachs.FirstOrDefault(d => d.MaDocGia == maDocGia.Value && d.MaSach == sachId);
            if (dg != null)
            {
                dg.SoSao = soSao;
                dg.BinhLuan = binhLuan;
                dg.NgayDanhGia = DateTime.Now;
            }
            else
            {
                _context.DanhGiaSachs.Add(new DanhGiaSach
                {
                    MaDocGia = maDocGia.Value,
                    MaSach = sachId,
                    SoSao = soSao,
                    BinhLuan = binhLuan,
                    NgayDanhGia = DateTime.Now,
                    TrangThai = (TrangThaiHienThi)1
                });
            }
            _context.SaveChanges();
            return RedirectToAction("ChiTietSach", new { id = sachId });
        }

        public IActionResult XemQuyDinhThuVien()
        {
            var list = _context.QuyDinhs.Where(q => (int)q.TrangThai == 1).OrderBy(q => q.MaQuyDinh).ToList();
            return View(list);
        }
    }
}
