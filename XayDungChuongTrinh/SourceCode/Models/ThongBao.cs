using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("thongbao")]
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }
        public int MaNguoiDung { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public int DaDoc { get; set; }
        public DateTime NgayTao { get; set; }

        // Navigation properties
        public NguoiDung NguoiDung { get; set; }
    }
}
