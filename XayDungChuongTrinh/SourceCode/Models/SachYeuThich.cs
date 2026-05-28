using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("sachyeuthich")]
    public class SachYeuThich
    {
        [Key]
        public int MaYeuThich { get; set; }
        public int MaDocGia { get; set; }
        public int MaSach { get; set; }
        public DateTime NgayThem { get; set; }

        // Navigation properties
        public DocGia DocGia { get; set; }
        public Sach Sach { get; set; }
    }
}
