-- =========================
-- TẠO DATABASE
-- =========================
DROP DATABASE IF EXISTS QuanLyThuVien;

CREATE DATABASE QuanLyThuVien
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE QuanLyThuVien;

-- =========================
-- BẢNG NGƯỜI DÙNG
-- Quyen:
-- 1 = Admin
-- 2 = Thủ thư
-- 3 = Độc giả
-- =========================
CREATE TABLE nguoidung (
    MaNguoiDung INT AUTO_INCREMENT PRIMARY KEY,
    TenDangNhap VARCHAR(250) NOT NULL UNIQUE,
    MatKhau VARCHAR(250) NOT NULL,
    HoTen VARCHAR(255) NOT NULL,
    SoDienThoai VARCHAR(20),
    Email VARCHAR(150),
    Quyen INT NOT NULL,
    TrangThai INT DEFAULT 1,

    CONSTRAINT chk_nguoidung_quyen
        CHECK (Quyen IN (1, 2, 3)),

    CONSTRAINT chk_nguoidung_trangthai
        CHECK (TrangThai IN (0, 1))
);

CREATE TABLE tacgia (
    MaTacGia INT AUTO_INCREMENT PRIMARY KEY,
    TenTacGia VARCHAR(255) NOT NULL,
    TieuSu TEXT,
    DaXoa INT DEFAULT 0,
    NgayXoa DATETIME NULL
);

CREATE TABLE theloai (
    MaTheLoai INT AUTO_INCREMENT PRIMARY KEY,
    TenTheLoai VARCHAR(255) NOT NULL,
    DaXoa INT DEFAULT 0,
    NgayXoa DATETIME NULL
);

CREATE TABLE nhaxuatban (
    MaNhaXuatBan INT AUTO_INCREMENT PRIMARY KEY,
    TenNhaXuatBan VARCHAR(255) NOT NULL,
    DiaChi VARCHAR(255),
    SoDienThoai VARCHAR(20),
    DaXoa INT DEFAULT 0,
    NgayXoa DATETIME NULL
);

CREATE TABLE vaitrodocgia (
    MaVaiTro INT AUTO_INCREMENT PRIMARY KEY,
    TenVaiTro VARCHAR(150) NOT NULL,
    DaXoa INT DEFAULT 0,
    NgayXoa DATETIME NULL
);

-- =========================
-- BẢNG ĐỘC GIẢ
-- MaNguoiDung bắt buộc có vì độc giả phải đăng nhập được
-- =========================
CREATE TABLE docgia (
    MaDocGia INT AUTO_INCREMENT PRIMARY KEY,
    MaNguoiDung INT NOT NULL UNIQUE,
    TenDocGia VARCHAR(250) NOT NULL,
    GioiTinh VARCHAR(10),
    NgaySinh DATE,
    SoDienThoai VARCHAR(15),
    Email VARCHAR(150),
    DiaChi TEXT,
    SoCCCD VARCHAR(15),
    NgayBatDau TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    NgayKetThuc TIMESTAMP NULL,
    MaVaiTro INT,
    DaXoa INT DEFAULT 0,

    CONSTRAINT fk_docgia_nguoidung
        FOREIGN KEY (MaNguoiDung)
        REFERENCES nguoidung(MaNguoiDung)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT fk_docgia_vaitrodocgia
        FOREIGN KEY (MaVaiTro)
        REFERENCES vaitrodocgia(MaVaiTro)
        ON DELETE SET NULL
        ON UPDATE CASCADE
);

CREATE TABLE sach (
    MaSach INT AUTO_INCREMENT PRIMARY KEY,
    ISBN VARCHAR(20),
    TenSach VARCHAR(255) NOT NULL,
    HinhAnh VARCHAR(250),
    MaTacGia INT,
    MaTheLoai INT,
    MaNhaXuatBan INT,
    GiaTien DECIMAL(15,2),
    NamXuatBan INT,
    SoTrang INT,
    SoLuongTong INT DEFAULT 0,
    SoLuongHienCo INT DEFAULT 0,
    MoTa TEXT,
    NgayTao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    DaXoa INT DEFAULT 0,
    NgayXoa DATETIME NULL,

    CONSTRAINT fk_sach_tacgia
        FOREIGN KEY (MaTacGia)
        REFERENCES tacgia(MaTacGia)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT fk_sach_theloai
        FOREIGN KEY (MaTheLoai)
        REFERENCES theloai(MaTheLoai)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT fk_sach_nhaxuatban
        FOREIGN KEY (MaNhaXuatBan)
        REFERENCES nhaxuatban(MaNhaXuatBan)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

-- =========================
-- BẢNG PHIẾU MƯỢN
-- TrangThai:
-- 0 = Chờ duyệt
-- 1 = Đã duyệt / Đang mượn
-- 2 = Đã trả
-- 3 = Từ chối
-- 4 = Đã hủy
-- =========================
CREATE TABLE phieumuon (
    MaPhieuMuon INT AUTO_INCREMENT PRIMARY KEY,
    MaDocGia INT NOT NULL,
    NgayMuon TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    HanTra DATETIME,
    TienCoc DECIMAL(15,2) DEFAULT 0,
    TrangThai INT DEFAULT 0,
    MaNhanVien INT NULL,
    LyDoTuChoi TEXT,
    NgayCapNhat TIMESTAMP NULL,
    DaXoa INT DEFAULT 0,

    CONSTRAINT chk_phieumuon_trangthai
        CHECK (TrangThai IN (0, 1, 2, 3, 4)),

    CONSTRAINT fk_phieumuon_docgia
        FOREIGN KEY (MaDocGia)
        REFERENCES docgia(MaDocGia)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT fk_phieumuon_nguoidung
        FOREIGN KEY (MaNhanVien)
        REFERENCES nguoidung(MaNguoiDung)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

CREATE TABLE chitietphieumuon (
    MaChiTiet INT AUTO_INCREMENT PRIMARY KEY,
    MaPhieuMuon INT NOT NULL,
    MaSach INT NOT NULL,
    TrangThaiTra INT DEFAULT 0,

    CONSTRAINT chk_chitietphieumuon_trangthaitra
        CHECK (TrangThaiTra IN (0, 1)),

    CONSTRAINT fk_chitietphieumuon_phieumuon
        FOREIGN KEY (MaPhieuMuon)
        REFERENCES phieumuon(MaPhieuMuon)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_chitietphieumuon_sach
        FOREIGN KEY (MaSach)
        REFERENCES sach(MaSach)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT uq_chitietphieumuon_phieumuon_sach
        UNIQUE (MaPhieuMuon, MaSach)
);

CREATE TABLE phieutra (
    MaPhieuTra INT AUTO_INCREMENT PRIMARY KEY,
    MaChiTiet INT NOT NULL UNIQUE,
    NgayTra TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    TinhTrangSach INT,
    TienPhat DECIMAL(15,2) DEFAULT 0,
    GhiChu TEXT,
    MaNhanVien INT NOT NULL,

    CONSTRAINT fk_phieutra_chitietphieumuon
        FOREIGN KEY (MaChiTiet)
        REFERENCES chitietphieumuon(MaChiTiet)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT fk_phieutra_nguoidung
        FOREIGN KEY (MaNhanVien)
        REFERENCES nguoidung(MaNguoiDung)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

CREATE TABLE thongbao (
    MaThongBao INT AUTO_INCREMENT PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    TieuDe VARCHAR(255) NOT NULL,
    NoiDung TEXT NOT NULL,
    DaDoc INT DEFAULT 0,
    NgayTao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT chk_thongbao_dadoc
        CHECK (DaDoc IN (0, 1)),

    CONSTRAINT fk_thongbao_nguoidung
        FOREIGN KEY (MaNguoiDung)
        REFERENCES nguoidung(MaNguoiDung)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

CREATE TABLE sachyeuthich (
    MaYeuThich INT AUTO_INCREMENT PRIMARY KEY,
    MaDocGia INT NOT NULL,
    MaSach INT NOT NULL,
    NgayThem TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_sachyeuthich_docgia
        FOREIGN KEY (MaDocGia)
        REFERENCES docgia(MaDocGia)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_sachyeuthich_sach
        FOREIGN KEY (MaSach)
        REFERENCES sach(MaSach)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT uq_sachyeuthich_docgia_sach
        UNIQUE (MaDocGia, MaSach)
);

CREATE TABLE danhgiasach (
    MaDanhGia INT AUTO_INCREMENT PRIMARY KEY,
    MaDocGia INT NOT NULL,
    MaSach INT NOT NULL,
    SoSao INT NOT NULL,
    BinhLuan TEXT,
    NgayDanhGia TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    TrangThai INT DEFAULT 1,

    CONSTRAINT chk_danhgiasach_sosao
        CHECK (SoSao BETWEEN 1 AND 5),

    CONSTRAINT chk_danhgiasach_trangthai
        CHECK (TrangThai IN (0, 1)),

    CONSTRAINT fk_danhgiasach_docgia
        FOREIGN KEY (MaDocGia)
        REFERENCES docgia(MaDocGia)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_danhgiasach_sach
        FOREIGN KEY (MaSach)
        REFERENCES sach(MaSach)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,

    CONSTRAINT uq_danhgiasach_docgia_sach
        UNIQUE (MaDocGia, MaSach)
);

CREATE TABLE quydinh (
    MaQuyDinh INT AUTO_INCREMENT PRIMARY KEY,
    TieuDe VARCHAR(255) NOT NULL,
    NoiDung TEXT NOT NULL,
    NgayTao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    TrangThai INT DEFAULT 1,

    CONSTRAINT chk_quydinh_trangthai
        CHECK (TrangThai IN (0, 1))
);

-- =========================
-- DỮ LIỆU MẪU
-- =========================

INSERT INTO nguoidung 
(TenDangNhap, MatKhau, HoTen, SoDienThoai, Email, Quyen, TrangThai)
VALUES
('admin', '123456', 'Quản trị viên', '0900000001', 'admin@gmail.com', 1, 1),
('thuthu01', '123456', 'Nguyễn Văn Thủ Thư', '0900000002', 'thuthu@gmail.com', 2, 1),
('docgia01', '123456', 'Trần Hồng Việt', '0912345678', 'tranhongviet@gmail.com', 3, 1),
('docgia02', '123456', 'Lê Thị B', '0987654321', 'lethib@gmail.com', 3, 1);

INSERT INTO tacgia 
(TenTacGia, TieuSu, DaXoa)
VALUES
('Nguyễn Nhật Ánh', 'Nhà văn Việt Nam nổi tiếng với các tác phẩm dành cho tuổi học trò.', 0),
('Nam Cao', 'Nhà văn hiện thực phê phán Việt Nam.', 0),
('J.K. Rowling', 'Tác giả bộ truyện Harry Potter.', 0);

INSERT INTO theloai 
(TenTheLoai, DaXoa)
VALUES
('Văn học Việt Nam', 0),
('Truyện thiếu nhi', 0),
('Tiểu thuyết', 0),
('Khoa học', 0);

INSERT INTO nhaxuatban 
(TenNhaXuatBan, DiaChi, SoDienThoai, DaXoa)
VALUES
('NXB Trẻ', 'TP. Hồ Chí Minh', '0281111111', 0),
('NXB Kim Đồng', 'Hà Nội', '0242222222', 0),
('NXB Giáo dục', 'Hà Nội', '0243333333', 0);

INSERT INTO vaitrodocgia 
(TenVaiTro, DaXoa)
VALUES
('Sinh viên', 0),
('Giảng viên', 0),
('Khách ngoài', 0);

INSERT INTO docgia 
(MaNguoiDung, TenDocGia, GioiTinh, NgaySinh, SoDienThoai, Email, DiaChi, SoCCCD, MaVaiTro, DaXoa)
VALUES
(3, 'Trần Hồng Việt', 'Nam', '2005-08-06', '0912345678', 'tranhongviet@gmail.com', 'Đà Nẵng', '123456789012', 1, 0),
(4, 'Lê Thị B', 'Nữ', '2004-05-10', '0987654321', 'lethib@gmail.com', 'Quảng Nam', '987654321012', 1, 0);

INSERT INTO sach 
(ISBN, TenSach, HinhAnh, MaTacGia, MaTheLoai, MaNhaXuatBan, GiaTien, NamXuatBan, SoTrang, SoLuongTong, SoLuongHienCo, MoTa, DaXoa)
VALUES
('978604100001', 'Cho tôi xin một vé đi tuổi thơ', '/images/books/02d05cc9-7554-471e-82b1-2fc9f6d25c59.webp', 1, 2, 1, 85000, 2008, 220, 10, 10, 'Tác phẩm nổi tiếng của Nguyễn Nhật Ánh.', 0),
('978604100002', 'Chí Phèo', '/images/books/0862d15c-ffa1-4c0a-a34f-ef423541eb19.webp', 2, 1, 3, 50000, 1941, 150, 8, 8, 'Tác phẩm văn học hiện thực của Nam Cao.', 0),
('978604100003', 'Harry Potter và Hòn đá Phù thủy', '/images/books/191bef80-3ac8-47e8-bf5b-17cfff1c72fc.webp', 3, 3, 2, 120000, 1997, 350, 5, 5, 'Tiểu thuyết giả tưởng nổi tiếng.', 0),
('978604100004', 'Đắc Nhân Tâm', '/images/books/2c6108a8-5947-40c8-ad71-2e759983ce7e.webp', 1, 2, 1, 95000, 1936, 280, 15, 12, 'Cuốn sách kinh điển về nghệ thuật giao tiếp.', 0),
('978604100005', 'Nhà Giả Kim', '/images/books/384e8ba1-c860-4980-949a-58b9bca02b43.webp', 1, 2, 2, 75000, 1988, 200, 20, 18, 'Tiểu thuyết truyền cảm hứng.', 0),
('978604100006', 'Sapiens: Lược sử loài người', '/images/books/886cf06b-76cd-4d81-9cef-5f680703fa14.webp', 2, 1, 3, 150000, 2011, 450, 10, 8, 'Lịch sử loài người.', 0),
('978604100007', 'Tư duy nhanh và chậm', '/images/books/bd792243-45e1-4f95-824f-12e15175955b.webp', 2, 1, 1, 180000, 2011, 400, 8, 6, 'Cuốn sách về tâm lý học.', 0),
('978604100008', 'Lược sử thời gian', '/images/books/d8d8fa10-6858-4af4-b51e-1117e269fa72.webp', 3, 3, 2, 110000, 1988, 250, 12, 10, 'Cuốn sách về vũ trụ.', 0),
('978604100009', 'Tôi thấy hoa vàng trên cỏ xanh', '/images/books/ecb09d4a-4f17-4a30-84d6-b5ade578fddd.webp', 1, 2, 1, 90000, 2010, 300, 15, 13, 'Tác phẩm mới của Nguyễn Nhật Ánh.', 0),
('978604100010', 'Gen: Một lịch sử di truyền', '/images/books/f45c2537-bf8e-4b1d-b9f8-14d8ff87ad71.webp', 2, 1, 3, 200000, 2016, 600, 6, 5, 'Lịch sử của gen.', 0),
('978604100011', 'Nơi đây núi điện đại', '/images/books/noi-day-nui-dien-dai-manga-tap-2-bia.webp', 1, 2, 2, 70000, 2015, 180, 25, 20, 'Truyện tranh nổi tiếng.', 0),
('978604100012', 'Câu hỏi lớn', '/images/books/tranh-bien-nho-ve-cau-hoi-lon-dieu-gi-khien-cuoc-song-co-y-nghia-bia.webp', 1, 2, 1, 85000, 2014, 220, 18, 15, 'Cuốn sách về ý nghĩa cuộc sống.', 0);

INSERT INTO phieumuon 
(MaDocGia, HanTra, TienCoc, TrangThai, MaNhanVien, DaXoa)
VALUES
(1, '2026-06-10 17:00:00', 50000, 1, 2, 0),
(2, '2026-06-12 17:00:00', 50000, 0, NULL, 0);

INSERT INTO chitietphieumuon 
(MaPhieuMuon, MaSach, TrangThaiTra)
VALUES
(1, 1, 1),
(1, 2, 0),
(2, 3, 0);

INSERT INTO phieutra 
(MaChiTiet, TinhTrangSach, TienPhat, GhiChu, MaNhanVien)
VALUES
(1, 1, 0, 'Sách còn tốt', 2);

INSERT INTO thongbao
(MaNguoiDung, TieuDe, NoiDung, DaDoc)
VALUES
(3, 'Yêu cầu mượn đã được duyệt', 'Phiếu mượn của bạn đã được thủ thư duyệt.', 0),
(3, 'Nhắc hạn trả sách', 'Bạn có sách sắp đến hạn trả, vui lòng kiểm tra lịch sử mượn.', 0);

INSERT INTO sachyeuthich
(MaDocGia, MaSach)
VALUES
(1, 3),
(2, 1);

INSERT INTO danhgiasach
(MaDocGia, MaSach, SoSao, BinhLuan, TrangThai)
VALUES
(1, 1, 5, 'Sách rất hay và dễ đọc.', 1),
(2, 3, 4, 'Nội dung hấp dẫn.', 1);

INSERT INTO quydinh
(TieuDe, NoiDung, TrangThai)
VALUES
('Quy định mượn sách', 'Mỗi độc giả được mượn tối đa 3 cuốn sách trong một lần.', 1),
('Quy định trả sách', 'Độc giả cần trả sách đúng hạn. Nếu quá hạn hoặc làm hư hỏng sách sẽ bị tính tiền phạt.', 1);

-- =========================
-- TEST ĐĂNG NHẬP 3 ACTOR
-- =========================

-- Admin
SELECT MaNguoiDung, TenDangNhap, HoTen, Quyen, TrangThai
FROM nguoidung
WHERE TenDangNhap = 'admin'
  AND MatKhau = '123456'
  AND TrangThai = 1;

-- Thủ thư
SELECT MaNguoiDung, TenDangNhap, HoTen, Quyen, TrangThai
FROM nguoidung
WHERE TenDangNhap = 'thuthu01'
  AND MatKhau = '123456'
  AND TrangThai = 1;

-- Độc giả
SELECT 
    nd.MaNguoiDung,
    nd.TenDangNhap,
    nd.HoTen,
    nd.Quyen,
    dg.MaDocGia,
    dg.TenDocGia
FROM nguoidung nd
JOIN docgia dg ON nd.MaNguoiDung = dg.MaNguoiDung
WHERE nd.TenDangNhap = 'docgia01'
  AND nd.MatKhau = '123456'
  AND nd.TrangThai = 1;

-- =========================
-- KIỂM TRA QUAN HỆ
-- =========================

SELECT 
    dg.MaDocGia,
    dg.TenDocGia,
    nd.TenDangNhap,
    nd.Quyen,
    vt.TenVaiTro
FROM docgia dg
JOIN nguoidung nd ON dg.MaNguoiDung = nd.MaNguoiDung
LEFT JOIN vaitrodocgia vt ON dg.MaVaiTro = vt.MaVaiTro;

SELECT 
    s.MaSach,
    s.TenSach,
    tg.TenTacGia,
    tl.TenTheLoai,
    nxb.TenNhaXuatBan
FROM sach s
JOIN tacgia tg ON s.MaTacGia = tg.MaTacGia
JOIN theloai tl ON s.MaTheLoai = tl.MaTheLoai
JOIN nhaxuatban nxb ON s.MaNhaXuatBan = nxb.MaNhaXuatBan;

SELECT 
    pm.MaPhieuMuon,
    dg.TenDocGia,
    s.TenSach,
    pm.TrangThai AS TrangThaiPhieuMuon,
    ct.TrangThaiTra,
    pt.NgayTra,
    pt.TienPhat
FROM phieumuon pm
JOIN docgia dg ON pm.MaDocGia = dg.MaDocGia
JOIN chitietphieumuon ct ON pm.MaPhieuMuon = ct.MaPhieuMuon
JOIN sach s ON ct.MaSach = s.MaSach
LEFT JOIN phieutra pt ON ct.MaChiTiet = pt.MaChiTiet;