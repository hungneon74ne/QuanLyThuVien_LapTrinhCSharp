using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace QuanLyThuVien
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Cấu hình Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Đăng ký DbContext
            builder.Services.AddDbContext<QuanLyThuVien.Data.QuanLyThuVienDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 9)));
            });

            // Đăng ký các lớp Business (Dependency Injection)
            builder.Services.AddScoped<QuanLyThuVien.Business.AuthBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.NguoiDungBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.DocGiaBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.SachBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.PhieuMuonBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.PhieuTraBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.ThongBaoBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.DanhGiaSachBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.SachYeuThichBusiness>();
            builder.Services.AddScoped<QuanLyThuVien.Business.QuyDinhBusiness>();
            builder.Services.AddHostedService<QuanLyThuVien.Services.CleanupBackgroundService>();
            var app = builder.Build();

            // Tạo DB và seed dữ liệu mẫu khi khởi động
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<QuanLyThuVien.Data.QuanLyThuVienDbContext>();
                db.Database.EnsureCreated();
                QuanLyThuVien.Data.DbSeeder.Seed(db);
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
