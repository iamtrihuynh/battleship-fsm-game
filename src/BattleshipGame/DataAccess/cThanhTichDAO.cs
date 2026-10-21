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

    // doc ghi thanhtichs.txt
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

        // Tao san vai dong mau neu chua co file
        private static void TaoFileMau(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(path, false, _utf8))
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
            }
        }

        // Doc danh sach thanh tich tu file
        public static List<ThanhTichItem> DocThanhTich()
        {
            return DocDanhSach();
        }

        // Doc danh sach tu file
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

        // Ghi them 1 tran moi truyen truc tiep thong so
        public static bool LuuThanhTich(string ten, int diem, int luot, int giay, string cheDo, string ketQua)
        {
            return LuuThanhTich(new ThanhTichItem(DateTime.Now, ten, diem, luot, giay, cheDo, ketQua));
        }

        // Ghi them 1 tran moi vao cuoi file
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
                    catch (Exception ex)
                    {
                        LoiGanNhat = ex.Message;
                        return false;
                    }
                }
            }
            return false;
        }

        // Sap xep danh sach
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

        // Loc theo che do choi
        public static List<ThanhTichItem> LocTheoCheDo(List<ThanhTichItem> list, string cheDo)
        {
            if (list == null) return new List<ThanhTichItem>();
            if (string.IsNullOrWhiteSpace(cheDo) || cheDo.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
                return list.Where(t => t != null).ToList();

            bool isGenericPve = cheDo.Equals("PvE", StringComparison.OrdinalIgnoreCase);
            bool isPvpFilter = cheDo.StartsWith("PvP", StringComparison.OrdinalIgnoreCase);

            return list
                .Where(t => t != null && !string.IsNullOrEmpty(t.CheDo) &&
                            (t.CheDo.Equals(cheDo, StringComparison.OrdinalIgnoreCase) ||
                             t.CheDo.StartsWith(cheDo + " -", StringComparison.OrdinalIgnoreCase) ||
                             (isGenericPve && t.CheDo.StartsWith("PvE", StringComparison.OrdinalIgnoreCase)) ||
                             (isPvpFilter && t.CheDo.StartsWith("PvP", StringComparison.OrdinalIgnoreCase))))
                .ToList();
        }

        /// <summary>
        /// [NGÀY 4 - TV3] Hàm tính điểm tự động theo công thức Game Balance chuẩn:
        /// Điểm = 1000 (Thắng) + (Số tàu còn sống * 200) - (Số phát bắn * 15) - (Thời gian giây * 2).
        /// Kẹp biên an toàn trong khoảng [0, int.MaxValue] chống tràn số.
        /// Thua trận mặc định 0 điểm.
        /// </summary>
        public static int TinhDiem(bool thang, int soTauConSong, int soPhatBan, int thoiGianGiay)
        {
            if (!thang) return 0;
            long soTau = Math.Max(0, soTauConSong);
            long phatBan = Math.Max(0, soPhatBan);
            long giay = Math.Max(0, thoiGianGiay);

            long diem = 1000L + (soTau * 200L) - (phatBan * 15L) - (giay * 2L);
            return (int)Math.Max(0L, Math.Min((long)int.MaxValue, diem));
        }
    }
}
