using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BattleshipGame.Logic
{
    // quan ly nhac nen, am thanh
    public static class cAmThanh
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringW", CharSet = CharSet.Unicode)]
        private static extern int mciSendString(string cmd, string ret, int len, IntPtr hwnd);

        private static readonly object _lock = new object();
        private static readonly Dictionary<string, SoundPlayer> _cache = new Dictionary<string, SoundPlayer>(StringComparer.OrdinalIgnoreCase);

        public static bool DaBatAmThanh { get; set; } = true;
        public static bool DangPhatNhacNen { get; private set; } = false;
        public static string AmThanhVuaPhat { get; private set; } = null;
        public static int SoLanDungNhacNen { get; private set; } = 0;
        private static string _audioDir = null;
        private static string _currentBgm = null;

        public static string ThuMucAudio
        {
            get
            {
                if (string.IsNullOrEmpty(_audioDir))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string path1 = Path.Combine(baseDir, "Assets", "Audio");
                    string path2 = Path.Combine(baseDir, "bg");

                    if (Directory.Exists(path1)) _audioDir = path1;
                    else if (Directory.Exists(path2)) _audioDir = path2;
                    else
                    {
                        string p = baseDir;
                        bool timThay = false;
                        for (int i = 0; i < 6; i++)
                        {
                            string candidate = Path.Combine(p, "src", "BattleshipGame", "Assets", "Audio");
                            if (Directory.Exists(candidate))
                            {
                                _audioDir = Path.GetFullPath(candidate);
                                timThay = true;
                                break;
                            }
                            string parent = Path.GetDirectoryName(p);
                            if (string.IsNullOrEmpty(parent) || parent == p) break;
                            p = parent;
                        }

                        if (!timThay) _audioDir = path1;
                    }
                }
                return _audioDir;
            }
            set => _audioDir = value;
        }

        public static void PhatNhacNen(string file = "bgm_battle.wav")
        {
            if (!DaBatAmThanh) return;

            string ten = (file ?? "bgm_battle.wav").Replace("\"", "").Trim();
            string path = Path.Combine(ThuMucAudio, ten);
            if (!File.Exists(path))
            {
                DangPhatNhacNen = false;
                return;
            }

            DangPhatNhacNen = true;

            Task.Run(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        if (!DangPhatNhacNen) return;
                        _currentBgm = ten;
                        mciSendString("close bgm", null, 0, IntPtr.Zero);

                        if (mciSendString(string.Format("open \"{0}\" type mpegvideo alias bgm", path), null, 0, IntPtr.Zero) == 0)
                        {
                            if (DangPhatNhacNen)
                                mciSendString("play bgm repeat", null, 0, IntPtr.Zero);
                            else
                                mciSendString("close bgm", null, 0, IntPtr.Zero);
                        }
                        else
                        {
                            DangPhatNhacNen = false;
                        }
                    }
                    catch { }
                }
            });
        }

        public static void DungNhacNen()
        {
            DangPhatNhacNen = false;
            _currentBgm = null;
            SoLanDungNhacNen++;

            Task.Run(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        mciSendString("stop bgm", null, 0, IntPtr.Zero);
                        mciSendString("close bgm", null, 0, IntPtr.Zero);
                    }
                    catch { }
                }
            });
        }

        // Lay player tu cache hoac tao moi neu chua co
        private static SoundPlayer LayPlayer(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return null;
            string ten = file.Replace("\"", "").Trim();

            lock (_cache)
            {
                if (_cache.TryGetValue(ten, out SoundPlayer p))
                    return p;

                if (_cache.Count >= 30)
                {
                    foreach (var item in _cache.Values)
                    {
                        try { item?.Dispose(); } catch { }
                    }
                    _cache.Clear();
                }

                string path = Path.Combine(ThuMucAudio, ten);
                if (File.Exists(path))
                {
                    try
                    {
                        SoundPlayer sp = new SoundPlayer(path);
                        sp.Load();
                        _cache[ten] = sp;
                        return sp;
                    }
                    catch { }
                }
            }
            return null;
        }

        // Phat hieu ung am thanh
        public static void PhatSFX(string file)
        {
            if (!DaBatAmThanh) return;
            AmThanhVuaPhat = file;

            try
            {
                SoundPlayer p = LayPlayer(file);
                if (p != null) p.Play();
                else SystemSounds.Beep.Play();
            }
            catch
            {
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

        public static bool BatTatAmThanh()
        {
            DaBatAmThanh = !DaBatAmThanh;
            if (!DaBatAmThanh)
                DungNhacNen();
            else if (!string.IsNullOrEmpty(_currentBgm))
                PhatNhacNen(_currentBgm);
            return DaBatAmThanh;
        }

        // Giai phong khi tat game
        public static void GiaiPhong()
        {
            try { DungNhacNen(); } catch { }

            lock (_cache)
            {
                foreach (var p in _cache.Values)
                {
                    try { p?.Dispose(); } catch { }
                }
                _cache.Clear();
            }
        }
    }
}
