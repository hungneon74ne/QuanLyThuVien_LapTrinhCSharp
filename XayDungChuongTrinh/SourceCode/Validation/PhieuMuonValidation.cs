using QuanLyThuVien.Models;
using QuanLyThuVien.Enums;
using System.Linq;

namespace QuanLyThuVien.Validation
{
    public class PhieuMuonValidation
    {
        public bool KiemTraDieuKienMuon(Sach sach, out string errorMessage)
        {
            if (sach.SoLuongHienCo <= 0)
            {
                errorMessage = "Sách hiện tại đã hết, không thể mượn.";
                return false;
            }
            
            errorMessage = string.Empty;
            return true;
        }

        public bool KiemTraDieuKienHuyYeuCau(PhieuMuon phieuMuon, out string errorMessage)
        {
            if (phieuMuon.TrangThai != TrangThaiPhieuMuon.ChoDuyet)
            {
                errorMessage = "Chỉ được hủy yêu cầu mượn khi trạng thái là Chờ duyệt.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
