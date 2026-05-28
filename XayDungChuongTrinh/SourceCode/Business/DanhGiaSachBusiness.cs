using System;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Business
{
    public class DanhGiaSachBusiness
    {
        private readonly QuanLyThuVienDbContext _context;

        public DanhGiaSachBusiness(QuanLyThuVienDbContext context)
        {
            _context = context;
        }

        // TODO: Thêm đánh giá
        public void ThemDanhGia(int docGiaId, int sachId, int soSao, string nhanXet)
        {
            throw new NotImplementedException();
        }

        // TODO: Sửa đánh giá
        public void SuaDanhGia(int danhGiaId, int soSao, string nhanXet)
        {
            throw new NotImplementedException();
        }

        // TODO: Ẩn đánh giá
        public void AnDanhGia(int danhGiaId)
        {
            throw new NotImplementedException();
        }
    }
}
