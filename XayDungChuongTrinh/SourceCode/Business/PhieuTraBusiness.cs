using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class PhieuTraBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public PhieuTraBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Lập phiếu trả
        public void LapPhieuTra(int chiTietPhieuMuonId)
        {
            throw new NotImplementedException();
        }

        // TODO: Tính tiền phạt
        public decimal TinhTienPhat(int chiTietPhieuMuonId)
        {
            throw new NotImplementedException();
        }

        // TODO: Cập nhật trạng thái trả sách
        public void CapNhatTrangThaiTraSach(int chiTietPhieuMuonId)
        {
            throw new NotImplementedException();
        }
    }
}
