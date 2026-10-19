using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BattleshipGame.Models;

namespace BattleshipGame.DataAccess
{
    public enum TieuChiSapXep
    {
        DiemCaoNhat,
        ThoiGianNganNhat,
        SoLuotItNhat
    }

    /// <summary>
    /// Data Access Object (DAO) quản lý việc Đọc / Ghi file thanhtichs.txt
    /// - Retry Pattern chống mất dữ liệu khi file bị khóa tức thời (Transient Lockout).
    /// - Sử dụng UTF8Encoding(false) không chèn BOM gây lỗi ngày tháng.
    /// - Lọc và sắp xếp chuẩn hóa không bị sai lệch dữ liệu.
    /// </summary>
    public class cThanhTichDAO
    {
        private static readonly object _fileLock = new object();
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);
        private static string _filePath = null;

        public static string LoiGanNhat { get; private set; } = null;

        public static string DuongDanFile
        {
            get
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string primaryPath = Path.Combine(baseDir, "Assets", "Data", "thanhtichs.txt");
                    string fallbackPath = Path.Combine(baseDir, "thanhtichs.txt");

                    if (File.Exists(primaryPath))
                    {
                        _filePath = primaryPath;
                    }
                    else if (File.Exists(fallbackPath))
                    {
                        _filePath = fallbackPath;
                    }
                    else
                    {
                        try
                        {
                            string dir = Path.Combine(baseDir, "Assets", "Data");
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            _filePath = primaryPath;
                            KhoiTaoFileMauNeuChuaCo(_filePath);
                        }
                        catch
                        {
                            _filePath = fallbackPath;
                            KhoiTaoFileMauNeuChuaCo(_filePath);
                        }
                    }
                }
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }

        private static void KhoiTaoFileMauNeuChuaCo(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(path, false, Utf8NoBom))
                    {
                        sw.WriteLine("2026-10-18 14:20:15|DoDocHaiQuan|1850|24|115|PvE - Nâng cao|Thắng");
                        sw.WriteLine("2026-10-18 16:05:40|ThuyenTruongTri|1620|28|145|PvE - Dễ|Thắng");
                        sw.WriteLine("2026-10-19 09:30:10|HaiTacHungCute|1480|32|170|PvP Hotseat|Thắng");
                        sw.WriteLine("2026-10-19 11:15:22|ChienBinhHoaPho|1350|36|190|Custom|Thắng");
                        sw.WriteLine("2026-10-19 15:45:00|TanBinhBien|820|48|260|PvE - Nâng cao|Thua");
                    }
                }
            }
            catch (Exception ex)
            {
                LoiGanNhat = ex.Message;
                System.Diagnostics.Debug.WriteLine("Lỗi khởi tạo file mẫu: " + ex.Message);
            }
        }

        /// <summary>
        /// Đọc toàn bộ danh sách thành tích từ file thanhtichs.txt
        /// </summary>
        public static List<ThanhTichItem> DocDanhSach()
        {
            List<ThanhTichItem> danhSach = new List<ThanhTichItem>();
            LoiGanNhat = null;

            lock (_fileLock)
            {
                try
                {
                    string path = DuongDanFile;
                    if (!File.Exists(path))
                    {
                        KhoiTaoFileMauNeuChuaCo(path);
                    }

                    if (File.Exists(path))
                    {
                        FileInfo fi = new FileInfo(path);
                        if (fi.Length > 10 * 1024 * 1024)
                        {
                            LoiGanNhat = "Kích thước tập tin thành tích vượt quá giới hạn an toàn (10MB).";
                            return danhSach;
                        }

                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (StreamReader sr = new StreamReader(fs, Utf8NoBom))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                try
                                {
                                    ThanhTichItem item = new ThanhTichItem(line);
                                    danhSach.Add(item);
                                }
                                catch (Exception exParse)
                                {
                                    System.Diagnostics.Debug.WriteLine("Bỏ qua dòng lỗi định dạng: " + exParse.Message);
                                }
                            }
                        }
                    }
                }
                catch (IOException ioEx)
                {
                    LoiGanNhat = "Tập tin đang bị khóa bởi tiến trình khác: " + ioEx.Message;
                    System.Diagnostics.Debug.WriteLine(LoiGanNhat);
                }
                catch (Exception ex)
                {
                    LoiGanNhat = "Lỗi khi đọc tập tin thành tích: " + ex.Message;
                    System.Diagnostics.Debug.WriteLine(LoiGanNhat);
                }
            }

            return SapXep(danhSach, TieuChiSapXep.DiemCaoNhat);
        }

        /// <summary>
        /// Ghi thêm 1 bản ghi mới với cơ chế Retry Pattern chống mất mát dữ liệu do xung đột khóa file
        /// </summary>
        public static bool LuuThanhTich(ThanhTichItem item)
        {
            if (item == null) return false;
            LoiGanNhat = null;

            lock (_fileLock)
            {
                int maxRetries = 4;
                int delayMs = 50;

                while (maxRetries > 0)
                {
                    try
                    {
                        string path = DuongDanFile;
                        string dir = Path.GetDirectoryName(path);
                        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                        using (StreamWriter sw = new StreamWriter(fs, Utf8NoBom))
                        {
                            sw.WriteLine(item.ToString());
                        }
                        return true;
                    }
                    catch (IOException ioEx)
                    {
                        maxRetries--;
                        if (maxRetries <= 0)
                        {
                            LoiGanNhat = "Tập tin thành tích bị khóa sau nhiều lần thử: " + ioEx.Message;
                            System.Diagnostics.Debug.WriteLine(LoiGanNhat);
                            return false;
                        }
                        Thread.Sleep(delayMs);
                        delayMs *= 2; // Exponential Backoff: 50ms, 100ms, 200ms...
                    }
                    catch (Exception ex)
                    {
                        LoiGanNhat = "Lỗi không xác định khi lưu thành tích: " + ex.Message;
                        System.Diagnostics.Debug.WriteLine(LoiGanNhat);
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sắp xếp danh sách thành tích (Ưu tiên các trận Thắng lên đầu)
        /// </summary>
        public static List<ThanhTichItem> SapXep(List<ThanhTichItem> danhSach, TieuChiSapXep tieuChi)
        {
            if (danhSach == null) return new List<ThanhTichItem>();

            switch (tieuChi)
            {
                case TieuChiSapXep.DiemCaoNhat:
                    return danhSach
                        .OrderByDescending(t => t.DiemSo)
                        .ThenBy(t => t.ThoiGianGiay)
                        .ThenBy(t => t.SoLuotBan)
                        .ToList();

                case TieuChiSapXep.ThoiGianNganNhat:
                    return danhSach
                        .OrderByDescending(t => (t.KetQua != null && t.KetQua.IndexOf("Thắng", StringComparison.OrdinalIgnoreCase) >= 0) ? 1 : 0)
                        .ThenBy(t => t.ThoiGianGiay)
                        .ThenByDescending(t => t.DiemSo)
                        .ThenBy(t => t.SoLuotBan)
                        .ToList();

                case TieuChiSapXep.SoLuotItNhat:
                    return danhSach
                        .OrderByDescending(t => (t.KetQua != null && t.KetQua.IndexOf("Thắng", StringComparison.OrdinalIgnoreCase) >= 0) ? 1 : 0)
                        .ThenBy(t => t.SoLuotBan)
                        .ThenByDescending(t => t.DiemSo)
                        .ThenBy(t => t.ThoiGianGiay)
                        .ToList();

                default:
                    return danhSach;
            }
        }

        /// <summary>
        /// Lọc danh sách theo chế độ chơi với so khớp tiền tố/chính xác, tránh nhầm lẫn chuỗi con
        /// </summary>
        public static List<ThanhTichItem> LocTheoCheDo(List<ThanhTichItem> danhSach, string cheDo)
        {
            if (danhSach == null) return new List<ThanhTichItem>();
            if (string.IsNullOrWhiteSpace(cheDo) || cheDo.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
                return danhSach;

            return danhSach
                .Where(t => t != null && !string.IsNullOrEmpty(t.CheDo) &&
                            (t.CheDo.Equals(cheDo, StringComparison.OrdinalIgnoreCase) ||
                             t.CheDo.StartsWith(cheDo + " -", StringComparison.OrdinalIgnoreCase) ||
                             (cheDo.Equals("PvE", StringComparison.OrdinalIgnoreCase) && t.CheDo.StartsWith("PvE", StringComparison.OrdinalIgnoreCase))))
                .ToList();
        }
    }
}
