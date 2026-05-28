using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Models
{
    [Table("danhgiasach")]
    public class DanhGiaSach
    {
        [Key]
        public int MaDanhGia { get; set; }
        public int MaDocGia { get; set; }
        public int MaSach { get; set; }
        public int SoSao { get; set; }
        public string BinhLuan { get; set; }
        public DateTime NgayDanhGia { get; set; }
        public TrangThaiHienThi TrangThai { get; set; }

        // Navigation properties
        public DocGia DocGia { get; set; }
        public Sach Sach { get; set; }
    }
}
