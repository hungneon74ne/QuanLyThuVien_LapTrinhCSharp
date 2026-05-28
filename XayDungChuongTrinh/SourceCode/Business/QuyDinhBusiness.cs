using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class QuyDinhBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public QuyDinhBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Quản lý quy định thư viện
        public void GetAllQuyDinh()
        {
            throw new NotImplementedException();
        }
    }
}
