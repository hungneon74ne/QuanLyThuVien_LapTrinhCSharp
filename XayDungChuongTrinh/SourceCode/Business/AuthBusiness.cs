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
            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password && u.TrangThai == TrangThaiTaiKhoan.HoatDong);

            return user;
        }

        // Lấy thông tin độc giả theo MaNguoiDung
        public DocGia GetDocGiaByMaNguoiDung(int maNguoiDung)
        {
            return _context.DocGias.FirstOrDefault(dg => dg.MaNguoiDung == maNguoiDung);
        }
    }
}
