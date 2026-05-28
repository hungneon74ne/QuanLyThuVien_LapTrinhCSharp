using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Business;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyThuVienDbContext _context;
        private readonly SachBusiness _sachBusiness;

        public HomeController(QuanLyThuVienDbContext context, SachBusiness sachBusiness)
        {
            _context = context;
            _sachBusiness = sachBusiness;
        }

        public IActionResult Index()
        {
            // Lấy danh sách sách nổi bật (8 sách đầu tiên)
            var featuredBooks = _context.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Where(s => s.DaXoa == 0)
                .OrderByDescending(s => s.SoLuongHienCo)
                .Take(8)
                .ToList();

            // Lấy sách theo thể loại Văn học
            var vanHocBooks = _context.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Where(s => s.DaXoa == 0 && s.TheLoai.TenTheLoai.Contains("Văn"))
                .Take(4)
                .ToList();

            // Lấy sách theo thể loại Khoa học
            var khoaHocBooks = _context.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Where(s => s.DaXoa == 0 && (s.TheLoai.TenTheLoai.Contains("Khoa") || s.TheLoai.TenTheLoai.Contains("Kinh")))
                .Take(4)
                .ToList();

            ViewBag.FeaturedBooks = featuredBooks;
            ViewBag.VanHocBooks = vanHocBooks;
            ViewBag.KhoaHocBooks = khoaHocBooks;

            return View();
        }

        public IActionResult Books()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
