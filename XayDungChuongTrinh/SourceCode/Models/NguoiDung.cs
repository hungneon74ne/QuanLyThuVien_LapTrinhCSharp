using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Models
{
    [Table("nguoidung")]
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public QuyenNguoiDung Quyen { get; set; }
        public TrangThaiTaiKhoan TrangThai { get; set; }

        // Navigation properties
        public DocGia DocGia { get; set; }
        public ICollection<PhieuMuon> PhieuMuons { get; set; }
        public ICollection<PhieuTra> PhieuTras { get; set; }
        public ICollection<ThongBao> ThongBaos { get; set; }
    }
}
