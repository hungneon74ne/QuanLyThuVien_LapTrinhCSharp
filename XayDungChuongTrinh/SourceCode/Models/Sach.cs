using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("sach")]
    public class Sach
    {
        [Key]
        public int MaSach { get; set; }
        public string ISBN { get; set; }
        public string TenSach { get; set; }
        public string HinhAnh { get; set; }
        public int? MaTacGia { get; set; }
        public int? MaTheLoai { get; set; }
        public int? MaNhaXuatBan { get; set; }
        public decimal? GiaTien { get; set; }
        public int? NamXuatBan { get; set; }
        public int? SoTrang { get; set; }
        public int SoLuongTong { get; set; }
        public int SoLuongHienCo { get; set; }
        public string MoTa { get; set; }
        public DateTime NgayTao { get; set; }
        public int DaXoa { get; set; }
        public DateTime? NgayXoa { get; set; }

        // Navigation properties
        public TacGia TacGia { get; set; }
        public TheLoai TheLoai { get; set; }
        public NhaXuatBan NhaXuatBan { get; set; }
        public ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }
        public ICollection<SachYeuThich> SachYeuThichs { get; set; }
        public ICollection<DanhGiaSach> DanhGiaSachs { get; set; }
    }
}
