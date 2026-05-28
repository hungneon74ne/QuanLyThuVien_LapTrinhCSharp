using QuanLyThuVien.Models;

namespace QuanLyThuVien.Validation
{
    public class SachValidation
    {
        public bool Validate(Sach sach, out string errorMessage)
        {
            if (sach.SoLuongTong < 0 || sach.SoLuongHienCo < 0)
            {
                errorMessage = "Số lượng sách không được âm.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
