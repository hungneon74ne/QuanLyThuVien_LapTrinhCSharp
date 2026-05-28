using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("tacgia")]
    public class TacGia
    {
        [Key]
        public int MaTacGia { get; set; }
        public string TenTacGia { get; set; }
        public string TieuSu { get; set; }
        public int DaXoa { get; set; }
        public DateTime? NgayXoa { get; set; }

        // Navigation properties
        public ICollection<Sach> Sachs { get; set; }
    }
}
