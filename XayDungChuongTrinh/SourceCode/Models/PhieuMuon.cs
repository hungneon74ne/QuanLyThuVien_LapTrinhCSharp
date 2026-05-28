using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Models
{
    [Table("phieumuon")]
    public class PhieuMuon
    {
        [Key]
        public int MaPhieuMuon { get; set; }
        public int MaDocGia { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime? HanTra { get; set; }
        public decimal TienCoc { get; set; }
        public TrangThaiPhieuMuon TrangThai { get; set; }
        public int? MaNhanVien { get; set; }
        public string LyDoTuChoi { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int DaXoa { get; set; }

        // Navigation properties
        public DocGia DocGia { get; set; }
        public NguoiDung NhanVienDuyet { get; set; }
        public ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }
    }
}
