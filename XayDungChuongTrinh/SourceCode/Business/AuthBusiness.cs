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

        // Lấy thông tin độc giả theo MaNguoiDung
        public DocGia GetDocGiaByMaNguoiDung(int maNguoiDung)
        {
            return _context.DocGias.FirstOrDefault(dg => dg.MaNguoiDung == maNguoiDung);
        }
    }
}
