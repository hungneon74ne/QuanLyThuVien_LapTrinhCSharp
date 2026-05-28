using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Business;

namespace QuanLyThuVien.Controllers
{
    // [Authorize(Roles = "DocGia")]
    public class DocGiaController : Controller
    {
        private readonly SachBusiness _sachBusiness;
        private readonly PhieuMuonBusiness _phieuMuonBusiness;
        private readonly DanhGiaSachBusiness _danhGiaSachBusiness;
        private readonly SachYeuThichBusiness _sachYeuThichBusiness;
        private readonly ThongBaoBusiness _thongBaoBusiness;

        public DocGiaController(
            SachBusiness sachBusiness,
            PhieuMuonBusiness phieuMuonBusiness,
            DanhGiaSachBusiness danhGiaSachBusiness,
            SachYeuThichBusiness sachYeuThichBusiness,
            ThongBaoBusiness thongBaoBusiness)
        {
            _sachBusiness = sachBusiness;
            _phieuMuonBusiness = phieuMuonBusiness;
            _danhGiaSachBusiness = danhGiaSachBusiness;
            _sachYeuThichBusiness = sachYeuThichBusiness;
            _thongBaoBusiness = thongBaoBusiness;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult ThongTinCaNhan()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult CapNhatThongTinCaNhan()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult DoiMatKhau()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult DanhSachSach()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult TimKiemSach(string keyword)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult LocSach()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult ChiTietSach(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult GuiYeuCauMuonSach(int[] sachIds)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult HuyYeuCauMuonSach(int id)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult TheoDoiTrangThaiMuon()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult DanhSachDangMuon()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult LichSuMuon()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult XemTienPhat()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult NhanThongBao()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult LuuSachYeuThich(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult XoaSachYeuThich(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult DanhGiaSach(int sachId, int soSao, string nhanXet)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult XemQuyDinhThuVien()
        {
            throw new System.NotImplementedException();
        }
    }
}
