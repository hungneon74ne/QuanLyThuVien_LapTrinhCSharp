using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Business;

namespace QuanLyThuVien.Controllers
{
    // [Authorize(Roles = "ThuThu")]
    public class ThuThuController : Controller
    {
        private readonly DocGiaBusiness _docGiaBusiness;
        private readonly SachBusiness _sachBusiness;
        private readonly PhieuMuonBusiness _phieuMuonBusiness;
        private readonly PhieuTraBusiness _phieuTraBusiness;
        private readonly ThongBaoBusiness _thongBaoBusiness;

        public ThuThuController(
            DocGiaBusiness docGiaBusiness,
            SachBusiness sachBusiness,
            PhieuMuonBusiness phieuMuonBusiness,
            PhieuTraBusiness phieuTraBusiness,
            ThongBaoBusiness thongBaoBusiness)
        {
            _docGiaBusiness = docGiaBusiness;
            _sachBusiness = sachBusiness;
            _phieuMuonBusiness = phieuMuonBusiness;
            _phieuTraBusiness = phieuTraBusiness;
            _thongBaoBusiness = thongBaoBusiness;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult QuanLyDocGia()
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

        public IActionResult TiepNhanYeuCauMuon()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult DuyetYeuCauMuon(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult TuChoiYeuCauMuon(int id)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult LapPhieuMuon()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult LapPhieuTra()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult TinhTienPhat(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public IActionResult CapNhatTinhTrangSach(int id, int tinhTrang)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult GuiThongBao()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult XemSachQuaHan()
        {
            throw new System.NotImplementedException();
        }
    }
}
