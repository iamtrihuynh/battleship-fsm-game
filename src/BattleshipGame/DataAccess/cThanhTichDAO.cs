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

    public class cThanhTichDAO
    {
        private static readonly object _lock = new object();
        private static readonly UTF8Encoding _utf8 = new UTF8Encoding(false);
        private static string _filePath = null;

        public static string LoiGanNhat { get; private set; } = null;

        public static string DuongDanFile
        {
            get
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string path1 = Path.Combine(baseDir, "Assets", "Data", "thanhtichs.txt");
                    string path2 = Path.Combine(baseDir, "thanhtichs.txt");

                    if (File.Exists(path1))
                    {
                        _filePath = path1;
                    }
                    else if (File.Exists(path2))
                    {
                        _filePath = path2;
                    }
                    else
                    {
                        try
                        {
                            string dir = Path.Combine(baseDir, "Assets", "Data");
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);

                            _filePath = path1;
                            TaoFileMau(_filePath);
                        }
                        catch
                        {
                            try
                            {
                                _filePath = path2;
                                TaoFileMau(_filePath);
                            }
                            catch
                            {
                                try
                                {
                                    string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BattleshipGame");
                                    if (!Directory.Exists(appData))
                                        Directory.CreateDirectory(appData);
                                    _filePath = Path.Combine(appData, "thanhtichs.txt");
                                    TaoFileMau(_filePath);
                                }
                                catch
                                {
                                    _filePath = path2;
                                }
                            }
                        }
                    }
                }
                return _filePath;
            }
            set => _filePath = value;
        }

        // tao file mau neu chua co
        private static void TaoFileMau(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    string dir = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (StreamWriter sw = new StreamWriter(path, false, _utf8))
                    {
                        sw.WriteLine("2026-10-18 14:20:15|DoDocHaiQuan|1850|24|115|PvE - Khó|Thắng");
                        sw.WriteLine("2026-10-18 16:05:40|ThuyenTruongTri|1620|28|145|PvE - Dễ|Thắng");
                        sw.WriteLine("2026-10-19 09:30:10|HaiTacHungCute|1550|30|130|PvE - Khó|Thắng");
                        sw.WriteLine("2026-10-19 11:15:22|ChienBinhHoaPho|1420|32|150|PvE - Dễ|Thắng");
                        sw.WriteLine("2026-10-19 14:00:00|ThuyenPhoPhat|1380|35|160|PvE - Dễ|Thắng");
                    }
                }
            }
            catch (Exception ex)
            {
                LoiGanNhat = ex.Message;
            }
        }

        public static List<ThanhTichItem> DocThanhTich()
        {
            return DocDanhSach();
        }

        public static List<ThanhTichItem> DocDanhSach()
        {
            List<ThanhTichItem> list = new List<ThanhTichItem>();
            LoiGanNhat = null;

            lock (_lock)
            {
                try
                {
                    string path = DuongDanFile;
                    if (!File.Exists(path))
                        TaoFileMau(path);

                    if (File.Exists(path))
                    {
                        if (new FileInfo(path).Length > 10 * 1024 * 1024)
                        {
                            LoiGanNhat = "File vuot qua 10MB";
                            return list;
                        }

                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (StreamReader sr = new StreamReader(fs, _utf8))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                try
                                {
                                    list.Add(new ThanhTichItem(line));
                                }
                                catch { }
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    LoiGanNhat = "File dang ban: " + ex.Message;
                }
                catch (Exception ex)
                {
                    LoiGanNhat = "Loi doc file: " + ex.Message;
                }
            }

            return SapXep(list, TieuChiSapXep.DiemCaoNhat);
        }

        public static bool LuuThanhTich(string ten, int diem, int luot, int giay, string cheDo, string ketQua)
        {
            return LuuThanhTich(new ThanhTichItem(DateTime.Now, ten, diem, luot, giay, cheDo, ketQua));
        }

        public static bool LuuThanhTich(ThanhTichItem item)
        {
            if (item == null) return false;
            LoiGanNhat = null;

            lock (_lock)
            {
                int retries = 4;
                int delay = 50;

                while (retries > 0)
                {
                    try
                    {
                        string path = DuongDanFile;
                        string dir = Path.GetDirectoryName(path);
                        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        bool canXuongDong = false;
                        if (File.Exists(path))
                        {
                            var fi = new FileInfo(path);
                            if (fi.Length > 0)
                            {
                                using (var readFs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    readFs.Seek(-1, SeekOrigin.End);
                                    int lastByte = readFs.ReadByte();
                                    if (lastByte != '\n') canXuongDong = true;
                                }
                            }
                        }

                        using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                        using (StreamWriter sw = new StreamWriter(fs, _utf8))
                        {
                            if (canXuongDong) sw.WriteLine();
                            sw.WriteLine(item.ToString());
                        }
                        return true;
                    }
                    catch (IOException)
                    {
                        retries--;
                        if (retries <= 0)
                        {
                            LoiGanNhat = "File dang bi khoa boi app khac";
                            return false;
                        }
                        Thread.Sleep(delay);
                        delay *= 2;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        LoiGanNhat = "Lỗi quyền truy cập file: " + ex.Message;
                        try
                        {
                            string appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BattleshipGame");
                            if (!Directory.Exists(appDataDir)) Directory.CreateDirectory(appDataDir);
                            string fallbackPath = Path.Combine(appDataDir, "thanhtichs.txt");
                            _filePath = fallbackPath;
                            using (FileStream fs = new FileStream(fallbackPath, FileMode.Append, FileAccess.Write, FileShare.Read))
                            using (StreamWriter sw = new StreamWriter(fs, _utf8))
                            {
                                sw.WriteLine(item.ToString());
                            }
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoiGanNhat = ex.Message;
                        return false;
                    }
                }
            }
            return false;
        }

        public static List<ThanhTichItem> SapXep(List<ThanhTichItem> list, TieuChiSapXep tieuChi)
        {
            if (list == null) return new List<ThanhTichItem>();
            var valid = list.Where(t => t != null);

            switch (tieuChi)
            {
                case TieuChiSapXep.DiemCaoNhat:
                    return valid
                        .OrderByDescending(t => t.DiemSo)
                        .ThenBy(t => t.ThoiGianGiay)
                        .ThenBy(t => t.SoLuotBan)
                        .ToList();

                case TieuChiSapXep.ThoiGianNganNhat:
                    return valid
                        .OrderByDescending(t => (t.KetQua != null && t.KetQua.IndexOf("Thắng", StringComparison.OrdinalIgnoreCase) >= 0) ? 1 : 0)
                        .ThenBy(t => t.ThoiGianGiay)
                        .ThenByDescending(t => t.DiemSo)
                        .ThenBy(t => t.SoLuotBan)
                        .ToList();

                case TieuChiSapXep.SoLuotItNhat:
                    return valid
                        .OrderByDescending(t => (t.KetQua != null && t.KetQua.IndexOf("Thắng", StringComparison.OrdinalIgnoreCase) >= 0) ? 1 : 0)
                        .ThenBy(t => t.SoLuotBan)
                        .ThenByDescending(t => t.DiemSo)
                        .ThenBy(t => t.ThoiGianGiay)
                        .ToList();

                default:
                    return valid.ToList();
            }
        }

        public static List<ThanhTichItem> LocTheoCheDo(List<ThanhTichItem> list, string cheDo)
        {
            if (list == null) return new List<ThanhTichItem>();
            if (string.IsNullOrWhiteSpace(cheDo) || cheDo.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
                return list.Where(t => t != null).ToList();

            string cd = cheDo.Trim();
            bool isGenericPve = cd.Equals("PvE", StringComparison.OrdinalIgnoreCase);
            bool isPvpFilter = cd.StartsWith("PvP", StringComparison.OrdinalIgnoreCase);
            bool isDeFilter = cd.Equals("Dễ", StringComparison.OrdinalIgnoreCase);
            bool isKhoFilter = cd.Equals("Khó", StringComparison.OrdinalIgnoreCase);

            return list
                .Where(t => t != null && !string.IsNullOrEmpty(t.CheDo) &&
                            (t.CheDo.Equals(cd, StringComparison.OrdinalIgnoreCase) ||
                             t.CheDo.StartsWith(cd + " -", StringComparison.OrdinalIgnoreCase) ||
                             (isGenericPve && t.CheDo.StartsWith("PvE", StringComparison.OrdinalIgnoreCase)) ||
                             (isPvpFilter && t.CheDo.StartsWith("PvP", StringComparison.OrdinalIgnoreCase)) ||
                             (isDeFilter && t.CheDo.IndexOf("Dễ", StringComparison.OrdinalIgnoreCase) >= 0) ||
                             (isKhoFilter && (t.CheDo.IndexOf("Khó", StringComparison.OrdinalIgnoreCase) >= 0 || t.CheDo.IndexOf("Nâng cao", StringComparison.OrdinalIgnoreCase) >= 0))))
                .ToList();
        }

        public static int TinhDiem(bool thang, int soTauConSong, int soPhatBan, int thoiGianGiay)
        {
            if (!thang) return 0;
            long soTau = Math.Max(0, Math.Min(5, soTauConSong));
            long phatBan = Math.Max(0, soPhatBan);
            long giay = Math.Max(0, thoiGianGiay);

            long diem = 1000L + (soTau * 200L) - (phatBan * 15L) - (giay * 2L);
            return (int)Math.Max(0L, Math.Min((long)int.MaxValue, diem));
        }
    }
}
