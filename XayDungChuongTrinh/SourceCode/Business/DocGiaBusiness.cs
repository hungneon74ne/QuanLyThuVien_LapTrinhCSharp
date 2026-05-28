using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class DocGiaBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public DocGiaBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Quản lý độc giả
        public void GetDocGiaList()
        {
            throw new NotImplementedException();
        }
    }
}
