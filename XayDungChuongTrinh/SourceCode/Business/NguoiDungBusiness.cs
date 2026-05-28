using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class NguoiDungBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public NguoiDungBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Quản lý tài khoản
        public void GetAll()
        {
            throw new NotImplementedException();
        }

        // TODO: Khóa/Mở khóa tài khoản
        public void ToggleStatus(int id)
        {
            throw new NotImplementedException();
        }
    }
}
