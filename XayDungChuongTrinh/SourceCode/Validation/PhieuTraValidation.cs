using QuanLyThuVien.Models;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Validation
{
    public class PhieuTraValidation
    {
        public bool KiemTraChiTietChuaTra(ChiTietPhieuMuon chiTiet, out string errorMessage)
        {
            if (chiTiet.TrangThaiTra == 1) // 1 = Đã trả
            {
                errorMessage = "Sách này đã được trả trước đó.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
