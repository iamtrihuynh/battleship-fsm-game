using System;
using System.Globalization;

namespace BattleshipGame.Models
{
    public class ThanhTichItem
    {
        private string _ten = "VoDanh";
        private string _cheDo = "PvE";
        private string _ketQua = "Thắng";

        public DateTime NgayChoi { get; set; }

        public string TenNguoiChoi
        {
            get => _ten;
            set => _ten = ChuanHoa(value, "VoDanh", 40);
        }

        public int DiemSo { get; set; }
        public int SoLuotBan { get; set; }
        public int ThoiGianGiay { get; set; }

        public string CheDo
        {
            get => _cheDo;
            set => _cheDo = ChuanHoa(value, "PvE", 30);
        }

        public string KetQua
        {
            get => _ketQua;
            set => _ketQua = ChuanHoa(value, "Thắng", 20);
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

        public ThanhTichItem(DateTime ngay, string ten, int diem, int luot, int giay, string cheDo, string ketQua)
        {
            NgayChoi = ngay;
            TenNguoiChoi = ten;
            DiemSo = Math.Max(0, diem);
            SoLuotBan = Math.Max(0, luot);
            ThoiGianGiay = Math.Max(0, giay);
            CheDo = cheDo;
            KetQua = ketQua;
        }

        public ThanhTichItem(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Du lieu rong");

            string[] arr = line.Split('|');
            if (arr.Length < 7)
                throw new FormatException("Khong du 7 truong du lieu");

            string dStr = arr[0].Trim().Trim('\uFEFF');
            if (DateTime.TryParseExact(dStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                NgayChoi = dt;
            else if (DateTime.TryParse(dStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                NgayChoi = dt;
            else
                NgayChoi = DateTime.Now;

            TenNguoiChoi = arr[1];

            DiemSo = int.TryParse(arr[2].Trim(), out int d) ? Math.Max(0, d) : 0;
            SoLuotBan = int.TryParse(arr[3].Trim(), out int l) ? Math.Max(0, l) : 0;
            ThoiGianGiay = int.TryParse(arr[4].Trim(), out int g) ? Math.Max(0, g) : 0;

            CheDo = arr[5];
            KetQua = arr[6];
        }

        private static string ChuanHoa(string s, string def, int maxLen = 40)
        {
            if (string.IsNullOrWhiteSpace(s)) return def;
            string res = s.Replace("|", "-").Replace("\r", "").Replace("\n", " ").Trim();
            if (string.IsNullOrWhiteSpace(res)) return def;
            if (res.Length > maxLen) res = res.Substring(0, maxLen).TrimEnd();
            return string.IsNullOrWhiteSpace(res) ? def : res;
        }

        public string ThoiGianHienThi
        {
            get
            {
                int s = Math.Max(0, ThoiGianGiay);
                TimeSpan ts = TimeSpan.FromSeconds(s);
                return string.Format("{0:D2}:{1:D2}", (int)ts.TotalMinutes, ts.Seconds);
            }
        }

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}|{1}|{2}|{3}|{4}|{5}|{6}",
                NgayChoi, TenNguoiChoi, DiemSo, SoLuotBan, ThoiGianGiay, CheDo, KetQua);
        }
    }
}
