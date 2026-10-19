using System;
using System.Globalization;

namespace BattleshipGame.Models
{
    /// <summary>
    /// Thực thể đại diện cho một bản ghi thành tích của người chơi
    /// Định dạng file: {NgayChoi}|{TenNguoiChoi}|{DiemSo}|{SoLuotBan}|{ThoiGianGiay}|{CheDo}|{KetQua}
    /// Tích hợp cơ chế bảo vệ:
    /// - Chống Delimiter Injection (loại bỏ ký tự '|')
    /// - Chống CRLF / Newline Injection (loại bỏ ký tự '\r', '\n' ngăn chặn chèn bản ghi giả mạo)
    /// - Xử lý bóc tách ký tự BOM ('\uFEFF') và Culture-invariant ngày tháng
    /// </summary>
    public class ThanhTichItem
    {
        private string _tenNguoiChoi = "VoDanh";
        private string _cheDo = "PvE";
        private string _ketQua = "Thắng";

        public DateTime NgayChoi { get; set; }

        public string TenNguoiChoi
        {
            get => _tenNguoiChoi;
            set => _tenNguoiChoi = SanitizeInput(value, "VoDanh");
        }

        public int DiemSo { get; set; }
        public int SoLuotBan { get; set; }
        public int ThoiGianGiay { get; set; }

        public string CheDo
        {
            get => _cheDo;
            set => _cheDo = SanitizeInput(value, "PvE");
        }

        public string KetQua
        {
            get => _ketQua;
            set => _ketQua = SanitizeInput(value, "Thắng");
        }

        public ThanhTichItem()
        {
            NgayChoi = DateTime.Now;
            TenNguoiChoi = "VoDanh";
            DiemSo = 0;
            SoLuotBan = 0;
            ThoiGianGiay = 0;
            CheDo = "PvE";
            KetQua = "Thắng";
        }

        public ThanhTichItem(DateTime ngayChoi, string tenNguoiChoi, int diemSo, int soLuotBan, int thoiGianGiay, string cheDo, string ketQua)
        {
            NgayChoi = ngayChoi;
            TenNguoiChoi = tenNguoiChoi;
            DiemSo = Math.Max(0, diemSo);
            SoLuotBan = Math.Max(0, soLuotBan);
            ThoiGianGiay = Math.Max(0, thoiGianGiay);
            CheDo = cheDo;
            KetQua = ketQua;
        }

        /// <summary>
        /// Khởi tạo đối tượng từ 1 dòng chuỗi phân tách bằng ký tự '|'
        /// </summary>
        public ThanhTichItem(string rawLine)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
                throw new ArgumentException("Dòng dữ liệu không được để trống.");

            string[] parts = rawLine.Split('|');
            if (parts.Length < 7)
                throw new FormatException(string.Format("Dữ liệu dòng thành tích không đúng định dạng (có {0}/7 trường).", parts.Length));

            // Bóc tách ký tự BOM '\uFEFF' (nếu có) trước khi parse ngày tháng
            string dateStr = parts[0].Trim().Trim('\uFEFF');
            DateTime dt;
            if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                NgayChoi = dt;
            }
            else if (DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                NgayChoi = dt;
            }
            else
            {
                NgayChoi = DateTime.Now;
            }

            TenNguoiChoi = parts[1];

            int diem;
            DiemSo = int.TryParse(parts[2].Trim(), out diem) ? Math.Max(0, diem) : 0;

            int luot;
            SoLuotBan = int.TryParse(parts[3].Trim(), out luot) ? Math.Max(0, luot) : 0;

            int tg;
            ThoiGianGiay = int.TryParse(parts[4].Trim(), out tg) ? Math.Max(0, tg) : 0;

            CheDo = parts[5];
            KetQua = parts[6];
        }

        /// <summary>
        /// Làm sạch chuỗi đầu vào: loại bỏ dấu '|' và các ký tự ngắt dòng '\r', '\n'
        /// </summary>
        private static string SanitizeInput(string input, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            // Xóa ký tự phân tách và ký tự ngắt dòng
            string cleaned = input.Replace("|", "-")
                                  .Replace("\r", "")
                                  .Replace("\n", " ")
                                  .Trim();

            return string.IsNullOrWhiteSpace(cleaned) ? defaultValue : cleaned;
        }

        /// <summary>
        /// Định dạng thời gian theo kiểu mm:ss để hiển thị lên bảng xếp hạng
        /// </summary>
        public string ThoiGianHienThi
        {
            get
            {
                int safeSeconds = Math.Max(0, ThoiGianGiay);
                TimeSpan ts = TimeSpan.FromSeconds(safeSeconds);
                return string.Format("{0:D2}:{1:D2}", (int)ts.TotalMinutes, ts.Seconds);
            }
        }

        /// <summary>
        /// Xuất định dạng chuỗi chuẩn để lưu vào tập tin thanhtichs.txt
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}|{1}|{2}|{3}|{4}|{5}|{6}",
                NgayChoi, TenNguoiChoi, DiemSo, SoLuotBan, ThoiGianGiay, CheDo, KetQua);
        }
    }
}
