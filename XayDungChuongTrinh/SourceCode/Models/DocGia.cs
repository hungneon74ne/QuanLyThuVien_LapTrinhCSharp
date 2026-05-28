using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("docgia")]
    public class DocGia
    {
        [Key]
        public int MaDocGia { get; set; }
        public int MaNguoiDung { get; set; }
        public string TenDocGia { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string SoCCCD { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int? MaVaiTro { get; set; }
        public int DaXoa { get; set; }
        
        // Navigation properties
        public NguoiDung NguoiDung { get; set; }
        public VaiTroDocGia VaiTroDocGia { get; set; }
        public ICollection<PhieuMuon> PhieuMuons { get; set; }
        public ICollection<SachYeuThich> SachYeuThichs { get; set; }
        public ICollection<DanhGiaSach> DanhGiaSachs { get; set; }
    }
}
