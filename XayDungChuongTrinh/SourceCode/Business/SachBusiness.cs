using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class SachBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public SachBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Quản lý sách
        public void GetSachList()
        {
            throw new NotImplementedException();
        }

        // TODO: Tìm kiếm sách
        public void SearchSach(string keyword)
        {
            throw new NotImplementedException();
        }

        // TODO: Kiểm tra số lượng sách
        public bool KiemTraSoLuongSach(int sachId)
        {
            throw new NotImplementedException();
        }
    }
}
