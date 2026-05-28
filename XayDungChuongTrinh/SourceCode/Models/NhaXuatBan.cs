using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    [Table("nhaxuatban")]
    public class NhaXuatBan
    {
        [Key]
        public int MaNhaXuatBan { get; set; }
        public string TenNhaXuatBan { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public int DaXoa { get; set; }

        // Navigation properties
        public ICollection<Sach> Sachs { get; set; }
    }
}
