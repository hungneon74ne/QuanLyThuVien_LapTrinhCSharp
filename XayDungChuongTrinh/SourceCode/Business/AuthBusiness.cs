using System;
using System.Linq;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Business
{
    public class AuthBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public AuthBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // Xử lý đăng nhập
        public NguoiDung Login(string username, string password)
        {
            Console.WriteLine($"--- LOGIN ATTEMPT: username='{username}', password='{password}' ---");
            try
            {
                var allUsers = _context.NguoiDungs.ToList();
                Console.WriteLine($"Total users in DB: {allUsers.Count}");
                foreach (var u in allUsers)
                {
                    Console.WriteLine($"- User: ID={u.MaNguoiDung}, Username='{u.TenDangNhap}', Password='{u.MatKhau}', Quyen={u.Quyen}, TrangThai={u.TrangThai}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
            }

            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password && u.TrangThai == TrangThaiTaiKhoan.HoatDong);

            return user;
        }

        // Xử lý đăng ký độc giả mới
        public bool RegisterDocGia(string username, string password, string hoten, string email, string sodienthoai, string gioitinh, DateTime? ngaysinh, string diachi, string socccd)
        {
            var exists = _context.NguoiDungs.Any(u => u.TenDangNhap == username);
            if (exists) return false;

            var nd = new NguoiDung
            {
                TenDangNhap = username,
                MatKhau = password,
                HoTen = hoten,
                Email = email,
                SoDienThoai = sodienthoai,
                Quyen = QuyenNguoiDung.DocGia,
                TrangThai = TrangThaiTaiKhoan.HoatDong
            };
            _context.NguoiDungs.Add(nd);
            _context.SaveChanges();

            var dg = new DocGia
            {
                MaNguoiDung = nd.MaNguoiDung,
                TenDocGia = hoten,
                GioiTinh = gioitinh,
                NgaySinh = ngaysinh,
                SoDienThoai = sodienthoai,
                Email = email,
                DiaChi = diachi,
                SoCCCD = socccd,
                NgayBatDau = DateTime.Now,
                DaXoa = 0
            };
            _context.DocGias.Add(dg);
            _context.SaveChanges();

            return true;
        }

        // Lấy thông tin độc giả theo MaNguoiDung
        public DocGia GetDocGiaByMaNguoiDung(int maNguoiDung)
        {
            return _context.DocGias.FirstOrDefault(dg => dg.MaNguoiDung == maNguoiDung);
        }
    }
}
