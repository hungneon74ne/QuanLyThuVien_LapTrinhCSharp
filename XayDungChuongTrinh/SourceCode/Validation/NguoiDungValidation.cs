using System.Text.RegularExpressions;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Validation
{
    public class NguoiDungValidation
    {
        public bool Validate(NguoiDung nguoiDung, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(nguoiDung.TenDangNhap))
            {
                errorMessage = "Tên đăng nhập không được rỗng.";
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(nguoiDung.MatKhau))
            {
                errorMessage = "Mật khẩu không được rỗng.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(nguoiDung.Email))
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(nguoiDung.Email))
                {
                    errorMessage = "Email không đúng định dạng.";
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(nguoiDung.SoDienThoai))
            {
                var phoneRegex = new Regex(@"^(0[3|5|7|8|9])+([0-9]{8})$");
                if (!phoneRegex.IsMatch(nguoiDung.SoDienThoai))
                {
                    errorMessage = "Số điện thoại không đúng định dạng.";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
