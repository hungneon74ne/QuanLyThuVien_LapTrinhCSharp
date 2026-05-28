using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    public class QuanLyThuVienDbContext : DbContext
    {
        public QuanLyThuVienDbContext(DbContextOptions<QuanLyThuVienDbContext> options) : base(options)
        {
        }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DocGia> DocGias { get; set; }
        public DbSet<VaiTroDocGia> VaiTroDocGias { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public DbSet<Sach> Sachs { get; set; }
        public DbSet<PhieuMuon> PhieuMuons { get; set; }
        public DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }
        public DbSet<PhieuTra> PhieuTras { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<SachYeuThich> SachYeuThichs { get; set; }
        public DbSet<DanhGiaSach> DanhGiaSachs { get; set; }
        public DbSet<QuyDinh> QuyDinhs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập các khoá ngoại
            modelBuilder.Entity<DocGia>()
                .HasOne(dg => dg.NguoiDung)
                .WithOne(nd => nd.DocGia)
                .HasForeignKey<DocGia>(dg => dg.MaNguoiDung);

            modelBuilder.Entity<DocGia>()
                .HasOne(dg => dg.VaiTroDocGia)
                .WithMany(vt => vt.DocGias)
                .HasForeignKey(dg => dg.MaVaiTro);

            modelBuilder.Entity<Sach>()
                .HasOne(s => s.TacGia)
                .WithMany(tg => tg.Sachs)
                .HasForeignKey(s => s.MaTacGia);

            modelBuilder.Entity<Sach>()
                .HasOne(s => s.TheLoai)
                .WithMany(tl => tl.Sachs)
                .HasForeignKey(s => s.MaTheLoai);

            modelBuilder.Entity<Sach>()
                .HasOne(s => s.NhaXuatBan)
                .WithMany(nxb => nxb.Sachs)
                .HasForeignKey(s => s.MaNhaXuatBan);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(pm => pm.DocGia)
                .WithMany(dg => dg.PhieuMuons)
                .HasForeignKey(pm => pm.MaDocGia);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(pm => pm.NhanVienDuyet)
                .WithMany(nd => nd.PhieuMuons)
                .HasForeignKey(pm => pm.MaNhanVien);

            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasOne(ct => ct.PhieuMuon)
                .WithMany(pm => pm.ChiTietPhieuMuons)
                .HasForeignKey(ct => ct.MaPhieuMuon);

            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasOne(ct => ct.Sach)
                .WithMany(s => s.ChiTietPhieuMuons)
                .HasForeignKey(ct => ct.MaSach);

            modelBuilder.Entity<PhieuTra>()
                .HasOne(pt => pt.ChiTietPhieuMuon)
                .WithOne(ct => ct.PhieuTra)
                .HasForeignKey<PhieuTra>(pt => pt.MaChiTiet);

            modelBuilder.Entity<PhieuTra>()
                .HasOne(pt => pt.NhanVienNhan)
                .WithMany(nd => nd.PhieuTras)
                .HasForeignKey(pt => pt.MaNhanVien);

            modelBuilder.Entity<ThongBao>()
                .HasOne(tb => tb.NguoiDung)
                .WithMany(nd => nd.ThongBaos)
                .HasForeignKey(tb => tb.MaNguoiDung);

            modelBuilder.Entity<SachYeuThich>()
                .HasOne(sy => sy.DocGia)
                .WithMany(dg => dg.SachYeuThichs)
                .HasForeignKey(sy => sy.MaDocGia);

            modelBuilder.Entity<SachYeuThich>()
                .HasOne(sy => sy.Sach)
                .WithMany(s => s.SachYeuThichs)
                .HasForeignKey(sy => sy.MaSach);

            modelBuilder.Entity<DanhGiaSach>()
                .HasOne(dg => dg.DocGia)
                .WithMany(d => d.DanhGiaSachs)
                .HasForeignKey(dg => dg.MaDocGia);

            modelBuilder.Entity<DanhGiaSach>()
                .HasOne(dg => dg.Sach)
                .WithMany(s => s.DanhGiaSachs)
                .HasForeignKey(dg => dg.MaSach);
        }
    }
}
