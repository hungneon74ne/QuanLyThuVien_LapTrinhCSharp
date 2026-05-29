using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("vaitrodocgia")]
    public class VaiTroDocGia
    {
        [Key]
        public int MaVaiTro { get; set; }
        public string TenVaiTro { get; set; }
        public int DaXoa { get; set; }
        public DateTime? NgayXoa { get; set; }

        // Navigation properties
        public ICollection<DocGia> DocGias { get; set; }
    }
}
