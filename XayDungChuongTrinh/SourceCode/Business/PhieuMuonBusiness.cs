using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class PhieuMuonBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public PhieuMuonBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Gửi yêu cầu mượn
        public void GuiYeuCauMuon()
        {
            throw new NotImplementedException();
        }

        // TODO: Hủy yêu cầu
        public void HuyYeuCau(int phieuMuonId)
        {
            throw new NotImplementedException();
        }

        // TODO: Duyệt yêu cầu
        public void DuyetYeuCau(int phieuMuonId)
        {
            throw new NotImplementedException();
        }

        // TODO: Từ chối yêu cầu
        public void TuChoiYeuCau(int phieuMuonId)
        {
            throw new NotImplementedException();
        }
    }
}
