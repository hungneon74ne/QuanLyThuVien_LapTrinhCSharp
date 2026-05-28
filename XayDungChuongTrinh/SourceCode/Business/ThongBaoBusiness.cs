using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class ThongBaoBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public ThongBaoBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Gửi thông báo
        public void GuiThongBao(int nguoiDungId, string tieuDe, string noiDung)
        {
            throw new NotImplementedException();
        }

        // TODO: Xem thông báo
        public void GetThongBaoList(int nguoiDungId)
        {
            throw new NotImplementedException();
        }
    }
}
