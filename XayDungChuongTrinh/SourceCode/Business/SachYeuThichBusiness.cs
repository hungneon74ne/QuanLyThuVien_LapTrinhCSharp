using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class SachYeuThichBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public SachYeuThichBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Thêm sách yêu thích
        public void ThemSachYeuThich(int docGiaId, int sachId)
        {
            throw new NotImplementedException();
        }

        // TODO: Xóa sách yêu thích
        public void XoaSachYeuThich(int docGiaId, int sachId)
        {
            throw new NotImplementedException();
        }
    }
}
