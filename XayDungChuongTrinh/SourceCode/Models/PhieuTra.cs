using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("phieutra")]
    public class PhieuTra
    {
        [Key]
        public int MaPhieuTra { get; set; }
        public int MaChiTiet { get; set; }
        public DateTime NgayTra { get; set; }
        public int? TinhTrangSach { get; set; }
        public decimal TienPhat { get; set; }
        public string GhiChu { get; set; }
        public int MaNhanVien { get; set; }

        // Navigation properties
        public ChiTietPhieuMuon ChiTietPhieuMuon { get; set; }
        public NguoiDung NhanVienNhan { get; set; }
    }
}
