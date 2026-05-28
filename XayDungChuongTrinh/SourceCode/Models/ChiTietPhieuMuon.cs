using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("chitietphieumuon")]
    public class ChiTietPhieuMuon
    {
        [Key]
        public int MaChiTiet { get; set; }
        public int MaPhieuMuon { get; set; }
        public int MaSach { get; set; }
        public int TrangThaiTra { get; set; }

        // Navigation properties
        public PhieuMuon PhieuMuon { get; set; }
        public Sach Sach { get; set; }
        public PhieuTra PhieuTra { get; set; }
    }
}
