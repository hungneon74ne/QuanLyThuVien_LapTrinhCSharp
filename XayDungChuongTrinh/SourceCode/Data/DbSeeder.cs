using QuanLyThuVien.Models;
using QuanLyThuVien.Enums;

namespace QuanLyThuVien.Data
{
    public static class DbSeeder
    {
        public static void Seed(QuanLyThuVienDbContext db)
        {
            if (db.NguoiDungs.Any()) return;

            var nd1 = new NguoiDung { TenDangNhap = "admin", MatKhau = "123456", HoTen = "Quản trị viên", SoDienThoai = "0900000001", Email = "admin@gmail.com", Quyen = QuyenNguoiDung.Admin, TrangThai = TrangThaiTaiKhoan.HoatDong };
            var nd2 = new NguoiDung { TenDangNhap = "thuthu01", MatKhau = "123456", HoTen = "Nguyễn Văn Thủ Thư", SoDienThoai = "0900000002", Email = "thuthu@gmail.com", Quyen = QuyenNguoiDung.ThuThu, TrangThai = TrangThaiTaiKhoan.HoatDong };
            var nd3 = new NguoiDung { TenDangNhap = "docgia01", MatKhau = "123456", HoTen = "Trần Hồng Việt", SoDienThoai = "0912345678", Email = "tranhongviet@gmail.com", Quyen = QuyenNguoiDung.DocGia, TrangThai = TrangThaiTaiKhoan.HoatDong };
            var nd4 = new NguoiDung { TenDangNhap = "docgia02", MatKhau = "123456", HoTen = "Lê Thị B", SoDienThoai = "0987654321", Email = "lethib@gmail.com", Quyen = QuyenNguoiDung.DocGia, TrangThai = TrangThaiTaiKhoan.HoatDong };
            db.NguoiDungs.AddRange(nd1, nd2, nd3, nd4);
            db.SaveChanges();

            var tg1 = new TacGia { TenTacGia = "Nguyễn Nhật Ánh", TieuSu = "Nhà văn Việt Nam nổi tiếng với các tác phẩm dành cho tuổi học trò.", DaXoa = 0 };
            var tg2 = new TacGia { TenTacGia = "Nam Cao", TieuSu = "Nhà văn hiện thực phê phán Việt Nam.", DaXoa = 0 };
            var tg3 = new TacGia { TenTacGia = "J.K. Rowling", TieuSu = "Tác giả bộ truyện Harry Potter.", DaXoa = 0 };
            db.TacGias.AddRange(tg1, tg2, tg3);
            db.SaveChanges();

            var tl1 = new TheLoai { TenTheLoai = "Văn học Việt Nam", DaXoa = 0 };
            var tl2 = new TheLoai { TenTheLoai = "Truyện thiếu nhi", DaXoa = 0 };
            var tl3 = new TheLoai { TenTheLoai = "Tiểu thuyết", DaXoa = 0 };
            var tl4 = new TheLoai { TenTheLoai = "Khoa học", DaXoa = 0 };
            db.TheLoais.AddRange(tl1, tl2, tl3, tl4);
            db.SaveChanges();

            var nxb1 = new NhaXuatBan { TenNhaXuatBan = "NXB Trẻ", DiaChi = "TP. Hồ Chí Minh", SoDienThoai = "0281111111", DaXoa = 0 };
            var nxb2 = new NhaXuatBan { TenNhaXuatBan = "NXB Kim Đồng", DiaChi = "Hà Nội", SoDienThoai = "0242222222", DaXoa = 0 };
            var nxb3 = new NhaXuatBan { TenNhaXuatBan = "NXB Giáo dục", DiaChi = "Hà Nội", SoDienThoai = "0243333333", DaXoa = 0 };
            db.NhaXuatBans.AddRange(nxb1, nxb2, nxb3);
            db.SaveChanges();

            var vt1 = new VaiTroDocGia { TenVaiTro = "Sinh viên", DaXoa = 0 };
            var vt2 = new VaiTroDocGia { TenVaiTro = "Giảng viên", DaXoa = 0 };
            var vt3 = new VaiTroDocGia { TenVaiTro = "Khách ngoài", DaXoa = 0 };
            db.VaiTroDocGias.AddRange(vt1, vt2, vt3);
            db.SaveChanges();

            var dg1 = new DocGia { MaNguoiDung = nd3.MaNguoiDung, TenDocGia = "Trần Hồng Việt", GioiTinh = "Nam", NgaySinh = new DateTime(2005, 8, 6), SoDienThoai = "0912345678", Email = "tranhongviet@gmail.com", DiaChi = "Đà Nẵng", SoCCCD = "123456789012", MaVaiTro = vt1.MaVaiTro, DaXoa = 0 };
            var dg2 = new DocGia { MaNguoiDung = nd4.MaNguoiDung, TenDocGia = "Lê Thị B", GioiTinh = "Nữ", NgaySinh = new DateTime(2004, 5, 10), SoDienThoai = "0987654321", Email = "lethib@gmail.com", DiaChi = "Quảng Nam", SoCCCD = "987654321012", MaVaiTro = vt1.MaVaiTro, DaXoa = 0 };
            db.DocGias.AddRange(dg1, dg2);
            db.SaveChanges();

            var sachs = new[]
            {
                new Sach { ISBN = "978604100001", TenSach = "Cho tôi xin một vé đi tuổi thơ", HinhAnh = "/images/books/02d05cc9-7554-471e-82b1-2fc9f6d25c59.webp", MaTacGia = tg1.MaTacGia, MaTheLoai = tl2.MaTheLoai, MaNhaXuatBan = nxb1.MaNhaXuatBan, GiaTien = 85000, NamXuatBan = 2008, SoTrang = 220, SoLuongTong = 10, SoLuongHienCo = 10, MoTa = "Tác phẩm nổi tiếng của Nguyễn Nhật Ánh.", DaXoa = 0 },
                new Sach { ISBN = "978604100002", TenSach = "Chí Phèo", HinhAnh = "/images/books/0862d15c-ffa1-4c0a-a34f-ef423541eb19.webp", MaTacGia = tg2.MaTacGia, MaTheLoai = tl1.MaTheLoai, MaNhaXuatBan = nxb3.MaNhaXuatBan, GiaTien = 50000, NamXuatBan = 1941, SoTrang = 150, SoLuongTong = 8, SoLuongHienCo = 8, MoTa = "Tác phẩm văn học hiện thực của Nam Cao.", DaXoa = 0 },
                new Sach { ISBN = "978604100003", TenSach = "Harry Potter và Hòn đá Phù thủy", HinhAnh = "/images/books/191bef80-3ac8-47e8-bf5b-17cfff1c72fc.webp", MaTacGia = tg3.MaTacGia, MaTheLoai = tl3.MaTheLoai, MaNhaXuatBan = nxb2.MaNhaXuatBan, GiaTien = 120000, NamXuatBan = 1997, SoTrang = 350, SoLuongTong = 5, SoLuongHienCo = 5, MoTa = "Tiểu thuyết giả tưởng nổi tiếng.", DaXoa = 0 },
                new Sach { ISBN = "978604100004", TenSach = "Đắc Nhân Tâm", HinhAnh = "/images/books/2c6108a8-5947-40c8-ad71-2e759983ce7e.webp", MaTacGia = tg1.MaTacGia, MaTheLoai = tl2.MaTheLoai, MaNhaXuatBan = nxb1.MaNhaXuatBan, GiaTien = 95000, NamXuatBan = 1936, SoTrang = 280, SoLuongTong = 15, SoLuongHienCo = 12, MoTa = "Cuốn sách kinh điển về nghệ thuật giao tiếp.", DaXoa = 0 },
                new Sach { ISBN = "978604100005", TenSach = "Nhà Giả Kim", HinhAnh = "/images/books/384e8ba1-c860-4980-949a-58b9bca02b43.webp", MaTacGia = tg1.MaTacGia, MaTheLoai = tl2.MaTheLoai, MaNhaXuatBan = nxb2.MaNhaXuatBan, GiaTien = 75000, NamXuatBan = 1988, SoTrang = 200, SoLuongTong = 20, SoLuongHienCo = 18, MoTa = "Tiểu thuyết truyền cảm hứng.", DaXoa = 0 },
                new Sach { ISBN = "978604100006", TenSach = "Sapiens: Lược sử loài người", HinhAnh = "/images/books/886cf06b-76cd-4d81-9cef-5f680703fa14.webp", MaTacGia = tg2.MaTacGia, MaTheLoai = tl1.MaTheLoai, MaNhaXuatBan = nxb3.MaNhaXuatBan, GiaTien = 150000, NamXuatBan = 2011, SoTrang = 450, SoLuongTong = 10, SoLuongHienCo = 8, MoTa = "Lịch sử loài người.", DaXoa = 0 },
                new Sach { ISBN = "978604100007", TenSach = "Tư duy nhanh và chậm", HinhAnh = "/images/books/bd792243-45e1-4f95-824f-12e15175955b.webp", MaTacGia = tg2.MaTacGia, MaTheLoai = tl1.MaTheLoai, MaNhaXuatBan = nxb1.MaNhaXuatBan, GiaTien = 180000, NamXuatBan = 2011, SoTrang = 400, SoLuongTong = 8, SoLuongHienCo = 6, MoTa = "Cuốn sách về tâm lý học.", DaXoa = 0 },
                new Sach { ISBN = "978604100008", TenSach = "Lược sử thời gian", HinhAnh = "/images/books/d8d8fa10-6858-4af4-b51e-1117e269fa72.webp", MaTacGia = tg3.MaTacGia, MaTheLoai = tl3.MaTheLoai, MaNhaXuatBan = nxb2.MaNhaXuatBan, GiaTien = 110000, NamXuatBan = 1988, SoTrang = 250, SoLuongTong = 12, SoLuongHienCo = 10, MoTa = "Cuốn sách về vũ trụ.", DaXoa = 0 },
                new Sach { ISBN = "978604100009", TenSach = "Tôi thấy hoa vàng trên cỏ xanh", HinhAnh = "/images/books/ecb09d4a-4f17-4a30-84d6-b5ade578fddd.webp", MaTacGia = tg1.MaTacGia, MaTheLoai = tl2.MaTheLoai, MaNhaXuatBan = nxb1.MaNhaXuatBan, GiaTien = 90000, NamXuatBan = 2010, SoTrang = 300, SoLuongTong = 15, SoLuongHienCo = 13, MoTa = "Tác phẩm mới của Nguyễn Nhật Ánh.", DaXoa = 0 },
                new Sach { ISBN = "978604100010", TenSach = "Gen: Một lịch sử di truyền", HinhAnh = "/images/books/f45c2537-bf8e-4b1d-b9f8-14d8ff87ad71.webp", MaTacGia = tg2.MaTacGia, MaTheLoai = tl1.MaTheLoai, MaNhaXuatBan = nxb3.MaNhaXuatBan, GiaTien = 200000, NamXuatBan = 2016, SoTrang = 600, SoLuongTong = 6, SoLuongHienCo = 5, MoTa = "Lịch sử của gen.", DaXoa = 0 },
                new Sach { ISBN = "978604100011", TenSach = "Nơi đây núi điện đại", HinhAnh = "/images/books/noi-day-nui-dien-dai-manga-tap-2-bia.webp", MaTacGia = tg1.MaTacGia, MaTheLoai = tl2.MaTheLoai, MaNhaXuatBan = nxb2.MaNhaXuatBan, GiaTien = 70000, NamXuatBan = 2015, SoTrang = 180, SoLuongTong = 25, SoLuongHienCo = 20, MoTa = "Truyện tranh nổi tiếng.", DaXoa = 0 },
                new Sach { ISBN = "978604100012", TenSach = "Câu hỏi lớn", HinhAnh = "/images/books/tranh-bien-nho-ve-cau-hoi-lon-dieu-gi-khien-cuoc-song-co-y-nghia-bia.webp", MaTacGia = tg1.MaTacGia, MaTheLoai = tl2.MaTheLoai, MaNhaXuatBan = nxb1.MaNhaXuatBan, GiaTien = 85000, NamXuatBan = 2014, SoTrang = 220, SoLuongTong = 18, SoLuongHienCo = 15, MoTa = "Cuốn sách về ý nghĩa cuộc sống.", DaXoa = 0 },
            };
            db.Sachs.AddRange(sachs);
            db.SaveChanges();

            var pm1 = new PhieuMuon { MaDocGia = dg1.MaDocGia, HanTra = new DateTime(2026, 6, 10, 17, 0, 0), TienCoc = 50000, TrangThai = TrangThaiPhieuMuon.DangMuon, MaNhanVien = nd2.MaNguoiDung, DaXoa = 0 };
            var pm2 = new PhieuMuon { MaDocGia = dg2.MaDocGia, HanTra = new DateTime(2026, 6, 12, 17, 0, 0), TienCoc = 50000, TrangThai = TrangThaiPhieuMuon.ChoDuyet, DaXoa = 0 };
            db.PhieuMuons.AddRange(pm1, pm2);
            db.SaveChanges();

            var ct1 = new ChiTietPhieuMuon { MaPhieuMuon = pm1.MaPhieuMuon, MaSach = sachs[0].MaSach, TrangThaiTra = 1 };
            var ct2 = new ChiTietPhieuMuon { MaPhieuMuon = pm1.MaPhieuMuon, MaSach = sachs[1].MaSach, TrangThaiTra = 0 };
            var ct3 = new ChiTietPhieuMuon { MaPhieuMuon = pm2.MaPhieuMuon, MaSach = sachs[2].MaSach, TrangThaiTra = 0 };
            db.ChiTietPhieuMuons.AddRange(ct1, ct2, ct3);
            db.SaveChanges();

            db.PhieuTras.Add(new PhieuTra { MaChiTiet = ct1.MaChiTiet, TinhTrangSach = 1, TienPhat = 0, GhiChu = "Sách còn tốt", MaNhanVien = nd2.MaNguoiDung });
            db.SaveChanges();

            db.ThongBaos.AddRange(
                new ThongBao { MaNguoiDung = nd3.MaNguoiDung, TieuDe = "Yêu cầu mượn đã được duyệt", NoiDung = "Phiếu mượn của bạn đã được thủ thư duyệt.", DaDoc = 0 },
                new ThongBao { MaNguoiDung = nd3.MaNguoiDung, TieuDe = "Nhắc hạn trả sách", NoiDung = "Bạn có sách sắp đến hạn trả, vui lòng kiểm tra lịch sử mượn.", DaDoc = 0 }
            );
            db.SaveChanges();

            db.SachYeuThichs.AddRange(
                new SachYeuThich { MaDocGia = dg1.MaDocGia, MaSach = sachs[2].MaSach },
                new SachYeuThich { MaDocGia = dg2.MaDocGia, MaSach = sachs[0].MaSach }
            );
            db.SaveChanges();

            db.DanhGiaSachs.AddRange(
                new DanhGiaSach { MaDocGia = dg1.MaDocGia, MaSach = sachs[0].MaSach, SoSao = 5, BinhLuan = "Sách rất hay và dễ đọc.", TrangThai = TrangThaiHienThi.Hien },
                new DanhGiaSach { MaDocGia = dg2.MaDocGia, MaSach = sachs[2].MaSach, SoSao = 4, BinhLuan = "Nội dung hấp dẫn.", TrangThai = TrangThaiHienThi.Hien }
            );
            db.SaveChanges();

            db.QuyDinhs.AddRange(
                new QuyDinh { TieuDe = "Quy định mượn sách", NoiDung = "Mỗi độc giả được mượn tối đa 3 cuốn sách trong một lần.", TrangThai = 1, NgayTao = DateTime.Now },
                new QuyDinh { TieuDe = "Quy định trả sách", NoiDung = "Độc giả cần trả sách đúng hạn. Nếu quá hạn hoặc làm hư hỏng sách sẽ bị tính tiền phạt.", TrangThai = 1, NgayTao = DateTime.Now }
            );
            db.SaveChanges();
        }
    }
}
