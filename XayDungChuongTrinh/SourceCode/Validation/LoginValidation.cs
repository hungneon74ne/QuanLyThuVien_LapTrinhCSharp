using System.ComponentModel.DataAnnotations;
using QuanLyThuVien.ViewModels;

namespace QuanLyThuVien.Validation
{
    public class LoginValidation
    {
        public bool Validate(LoginViewModel model, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(model.TenDangNhap))
            {
                errorMessage = "Tên đăng nhập không được rỗng.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.MatKhau))
            {
                errorMessage = "Mật khẩu không được rỗng.";
                return false;
            }
            
            errorMessage = string.Empty;
            return true;
        }
    }
}
