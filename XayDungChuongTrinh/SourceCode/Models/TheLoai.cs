using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("theloai")]
    public class TheLoai
    {
        [Key]
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public int DaXoa { get; set; }

        // Navigation properties
        public ICollection<Sach> Sachs { get; set; }
    }
}
