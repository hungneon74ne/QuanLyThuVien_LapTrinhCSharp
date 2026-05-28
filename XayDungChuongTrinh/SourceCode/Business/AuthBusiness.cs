using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class AuthBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public AuthBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Xử lý đăng nhập
        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        // TODO: Xử lý phân quyền
        public void CheckRole()
        {
            throw new NotImplementedException();
        }
    }
}
