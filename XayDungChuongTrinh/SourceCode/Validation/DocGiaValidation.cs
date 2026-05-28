using QuanLyThuVien.Models;

namespace QuanLyThuVien.Validation
{
    public class DocGiaValidation
    {
        public bool Validate(DocGia docGia, out string errorMessage)
        {
            // Thêm các logic kiểm tra thông tin độc giả riêng nếu cần
            
            errorMessage = string.Empty;
            return true;
        }
    }
}
