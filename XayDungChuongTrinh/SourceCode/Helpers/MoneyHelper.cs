using System.Globalization;

namespace QuanLyThuVien.Helpers
{
    public static class MoneyHelper
    {
        public static string FormatVND(decimal amount)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            return amount.ToString("#,###", cul.NumberFormat) + " VNĐ";
        }
    }
}
