using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;

namespace QuanLyThuVien.Services
{
    public class CleanupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CleanupBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromHours(24); // Chạy mỗi 24 giờ

        public CleanupBackgroundService(IServiceProvider serviceProvider, ILogger<CleanupBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CleanupBackgroundService đã khởi động.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, stoppingToken);
                await RunCleanupAsync();
            }
        }

        private async Task RunCleanupAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<QuanLyThuVienDbContext>();
                var ngayGioiHan = DateTime.Now.AddDays(-30);

                // Xóa cứng Sách đã xóa mềm quá 30 ngày
                var sachs = await db.Sachs
                    .Where(x => x.DaXoa == 1 && x.NgayXoa != null && x.NgayXoa < ngayGioiHan)
                    .ToListAsync();
                if (sachs.Any())
                {
                    db.Sachs.RemoveRange(sachs);
                    _logger.LogInformation($"[Cleanup] Xóa cứng {sachs.Count} sách.");
                }

                // Xóa cứng Tác giả
                var tacGias = await db.TacGias
                    .Where(x => x.DaXoa == 1 && x.NgayXoa != null && x.NgayXoa < ngayGioiHan)
                    .ToListAsync();
                if (tacGias.Any())
                {
                    db.TacGias.RemoveRange(tacGias);
                    _logger.LogInformation($"[Cleanup] Xóa cứng {tacGias.Count} tác giả.");
                }

                // Xóa cứng Thể loại
                var theLoais = await db.TheLoais
                    .Where(x => x.DaXoa == 1 && x.NgayXoa != null && x.NgayXoa < ngayGioiHan)
                    .ToListAsync();
                if (theLoais.Any())
                {
                    db.TheLoais.RemoveRange(theLoais);
                    _logger.LogInformation($"[Cleanup] Xóa cứng {theLoais.Count} thể loại.");
                }

                // Xóa cứng Nhà xuất bản
                var nxbs = await db.NhaXuatBans
                    .Where(x => x.DaXoa == 1 && x.NgayXoa != null && x.NgayXoa < ngayGioiHan)
                    .ToListAsync();
                if (nxbs.Any())
                {
                    db.NhaXuatBans.RemoveRange(nxbs);
                    _logger.LogInformation($"[Cleanup] Xóa cứng {nxbs.Count} nhà xuất bản.");
                }

                // Xóa cứng Vai trò độc giả
                var vaiTros = await db.VaiTroDocGias
                    .Where(x => x.DaXoa == 1 && x.NgayXoa != null && x.NgayXoa < ngayGioiHan)
                    .ToListAsync();
                if (vaiTros.Any())
                {
                    db.VaiTroDocGias.RemoveRange(vaiTros);
                    _logger.LogInformation($"[Cleanup] Xóa cứng {vaiTros.Count} vai trò.");
                }

                await db.SaveChangesAsync();
                _logger.LogInformation($"[Cleanup] Hoàn thành lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Cleanup] Lỗi khi chạy cleanup.");
            }
        }
    }
}
