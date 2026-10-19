using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BattleshipGame.Logic
{
    /// <summary>
    /// Lớp quản lý toàn bộ hệ thống âm thanh của trò chơi
    /// - Nhạc nền (BGM): Chạy trên luồng Task nền bất đồng bộ (Non-blocking UI), sử dụng mciSendStringW (Unicode).
    /// - Hiệu ứng âm thanh (SFX): Sử dụng Dictionary Caching có giới hạn số lượng phần tử chống rò rỉ RAM.
    /// - Thread-safe & Exception Handling toàn diện.
    /// </summary>
    public static class cAmThanh
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringW", CharSet = CharSet.Unicode)]
        private static extern int mciSendString(string strCommand, string strReturn, int iReturnLength, IntPtr hwndCallback);

        private static readonly object _audioLock = new object();
        private static readonly Dictionary<string, SoundPlayer> _soundCache = new Dictionary<string, SoundPlayer>(StringComparer.OrdinalIgnoreCase);
        private const int MaxCacheEntries = 30;

        public static bool DaBatAmThanh { get; set; } = true;

        private static string _audioDir = null;

        public static string ThuMucAudio
        {
            get
            {
                if (string.IsNullOrEmpty(_audioDir))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string primaryDir = Path.Combine(baseDir, "Assets", "Audio");
                    string fallbackDir = Path.Combine(baseDir, "bg");

                    if (Directory.Exists(primaryDir))
                    {
                        _audioDir = primaryDir;
                    }
                    else if (Directory.Exists(fallbackDir))
                    {
                        _audioDir = fallbackDir;
                    }
                    else
                    {
                        _audioDir = primaryDir;
                    }
                }
                return _audioDir;
            }
            set
            {
                _audioDir = value;
            }
        }

        #region 1. QUẢN LÝ NHẠC NỀN (BGM) - NON-BLOCKING TASK

        private static string _currentBgmFile = null;

        /// <summary>
        /// Phát nhạc nền bất đồng bộ trên luồng Task nền, không gây đứng hình giao diện
        /// </summary>
        public static void PhatNhacNen(string fileName = "bgm_battle.wav")
        {
            if (!DaBatAmThanh) return;

            Task.Run(() =>
            {
                lock (_audioLock)
                {
                    try
                    {
                        string cleanFileName = (fileName ?? "bgm_battle.wav").Replace("\"", "").Trim();
                        string filePath = Path.Combine(ThuMucAudio, cleanFileName);
                        if (!File.Exists(filePath))
                        {
                            System.Diagnostics.Debug.WriteLine("Không tìm thấy file nhạc nền: " + filePath);
                            return;
                        }

                        _currentBgmFile = cleanFileName;
                        mciSendString("close bgm", null, 0, IntPtr.Zero);

                        string openCmd = string.Format("open \"{0}\" type mpegvideo alias bgm", filePath);
                        int errOpen = mciSendString(openCmd, null, 0, IntPtr.Zero);
                        if (errOpen == 0)
                        {
                            mciSendString("play bgm repeat", null, 0, IntPtr.Zero);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Lỗi phát nhạc nền: " + ex.Message);
                    }
                }
            });
        }

        /// <summary>
        /// Dừng phát nhạc nền trên luồng Task nền
        /// </summary>
        public static void DungNhacNen()
        {
            Task.Run(() =>
            {
                lock (_audioLock)
                {
                    try
                    {
                        mciSendString("stop bgm", null, 0, IntPtr.Zero);
                        mciSendString("close bgm", null, 0, IntPtr.Zero);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Lỗi dừng nhạc nền: " + ex.Message);
                    }
                }
            });
        }

        #endregion

        #region 2. QUẢN LÝ HIỆU ỨNG ÂM THANH (SFX - CACHE CÓ GIỚI HẠN)

        private static SoundPlayer LayHoacTaoPlayer(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            string cleanFileName = fileName.Replace("\"", "").Trim();

            lock (_soundCache)
            {
                if (_soundCache.TryGetValue(cleanFileName, out SoundPlayer cachedPlayer))
                {
                    return cachedPlayer;
                }

                // Bảo vệ bộ nhớ: Nếu cache vượt quá số lượng tối đa, giải phóng và xóa bớt
                if (_soundCache.Count >= MaxCacheEntries)
                {
                    foreach (var p in _soundCache.Values)
                    {
                        try { p?.Dispose(); } catch { }
                    }
                    _soundCache.Clear();
                }

                string filePath = Path.Combine(ThuMucAudio, cleanFileName);
                if (File.Exists(filePath))
                {
                    try
                    {
                        SoundPlayer newPlayer = new SoundPlayer(filePath);
                        newPlayer.LoadAsync();
                        _soundCache[cleanFileName] = newPlayer;
                        return newPlayer;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Lỗi khởi tạo SoundPlayer cho " + cleanFileName + ": " + ex.Message);
                    }
                }
            }
            return null;
        }

        public static void PhatSFX(string fileName)
        {
            if (!DaBatAmThanh) return;

            try
            {
                SoundPlayer player = LayHoacTaoPlayer(fileName);
                if (player != null)
                {
                    player.Play();
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi phát SFX: " + ex.Message);
                try { SystemSounds.Beep.Play(); } catch { }
            }
        }

        public static void PhatShot() => PhatSFX("shot.wav");
        public static void PhatHit() => PhatSFX("hit.wav");
        public static void PhatMiss() => PhatSFX("miss.wav");
        public static void PhatSunk() => PhatSFX("sunk.wav");
        public static void PhatRadar() => PhatSFX("radar.wav");
        public static void PhatAirstrike() => PhatSFX("airstrike.wav");

        public static void PhatVictory()
        {
            DungNhacNen();
            PhatSFX("victory.wav");
        }

        public static void PhatDefeat()
        {
            DungNhacNen();
            PhatSFX("defeat.wav");
        }

        #endregion

        #region 3. ĐIỀU KHIỂN BẬT / TẮT ÂM THANH

        public static bool BatTatAmThanh()
        {
            DaBatAmThanh = !DaBatAmThanh;
            if (!DaBatAmThanh)
            {
                DungNhacNen();
            }
            else
            {
                PhatNhacNen(_currentBgmFile ?? "bgm_battle.wav");
            }
            return DaBatAmThanh;
        }

        /// <summary>
        /// Giải phóng toàn bộ tài nguyên âm thanh khi đóng Form hoặc thoát trò chơi
        /// </summary>
        public static void GiaiPhong()
        {
            try
            {
                DungNhacNen();
            }
            catch { }

            lock (_soundCache)
            {
                foreach (var player in _soundCache.Values)
                {
                    try { player?.Dispose(); } catch { }
                }
                _soundCache.Clear();
            }
        }

        #endregion
    }
}
