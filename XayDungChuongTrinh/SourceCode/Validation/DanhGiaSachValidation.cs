using QuanLyThuVien.Models;

namespace QuanLyThuVien.Validation
{
    public class DanhGiaSachValidation
    {
        public bool Validate(DanhGiaSach danhGia, out string errorMessage)
        {
            if (danhGia.SoSao < 1 || danhGia.SoSao > 5)
            {
                errorMessage = "Số sao đánh giá phải từ 1 đến 5.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
