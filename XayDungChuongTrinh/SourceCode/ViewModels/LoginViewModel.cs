using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được rỗng.")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được rỗng.")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
    }
}
