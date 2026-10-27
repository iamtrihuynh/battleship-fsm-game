#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipGame.Logic
{
    public static class cAmThanh
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringW", CharSet = CharSet.Unicode)]
        private static extern int mciSendString(string cmd, StringBuilder? ret, int len, IntPtr hwnd);

        [DllImport("winmm.dll")]
        private static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private static readonly object _lock = new object();
        private static readonly Dictionary<string, SoundPlayer> _cache = new Dictionary<string, SoundPlayer>(StringComparer.OrdinalIgnoreCase);

        public static bool DaBatAmThanh { get; set; } = true;
        public static bool DangPhatNhacNen { get; private set; } = false;
        public static string? AmThanhVuaPhat { get; private set; } = null;
        public static int SoLanDungNhacNen { get; private set; } = 0;
        private static string? _audioDir = null;
        private static string? _currentBgm = null;
        public static string? CurrentBgm => _currentBgm;

        private static System.Threading.Timer? _bgmLoopTimer = null;

        static cAmThanh()
        {
            _bgmLoopTimer = new System.Threading.Timer(KiemTraVaLapNhacNen, null, 1500, 1500);
        }

        public static string ThuMucAudio
        {
            get
            {
                if (string.IsNullOrEmpty(_audioDir) || !Directory.Exists(_audioDir))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string pathBg = Path.Combine(baseDir, "bg");

                    if (Directory.Exists(pathBg))
                    {
                        _audioDir = pathBg;
                    }
                    else
                    {
                        string p = baseDir;
                        bool timThay = false;
                        for (int i = 0; i < 6; i++)
                        {
                            string candidate = Path.Combine(p, "bg");
                            if (Directory.Exists(candidate))
                            {
                                _audioDir = Path.GetFullPath(candidate);
                                timThay = true;
                                break;
                            }
                            string? parent = Path.GetDirectoryName(p);
                            if (string.IsNullOrEmpty(parent) || parent == p) break;
                            p = parent;
                        }

                        if (!timThay) _audioDir = pathBg;
                    }
                }
                return _audioDir ?? "";
            }
            set => _audioDir = value;
        }

        public static void PhatNhacNen(string? file = "bgm_battle.wav")
        {
            if (!DaBatAmThanh) return;

            string ten = (file ?? "bgm_battle.wav").Replace("\"", "").Trim();
            string path = Path.Combine(ThuMucAudio, ten);
                if (!File.Exists(path))
                {
                    if (ten.Equals("hit.wav", StringComparison.OrdinalIgnoreCase))
                    {
                        string alt = Path.Combine(ThuMucAudio, "true.wav");
                        if (File.Exists(alt)) path = alt;
                    }
                    else if (ten.Equals("true.wav", StringComparison.OrdinalIgnoreCase))
                    {
                        string alt = Path.Combine(ThuMucAudio, "hit.wav");
                        if (File.Exists(alt)) path = alt;
                    }
                    else if (ten.Equals("miss.wav", StringComparison.OrdinalIgnoreCase))
                    {
                        string alt = Path.Combine(ThuMucAudio, "false.wav");
                        if (File.Exists(alt)) path = alt;
                    }
                    else if (ten.Equals("false.wav", StringComparison.OrdinalIgnoreCase))
                    {
                        string alt = Path.Combine(ThuMucAudio, "miss.wav");
                        if (File.Exists(alt)) path = alt;
                    }
                }
            if (!File.Exists(path))
            {
                if (ten.Equals("bgm_battle.wav", StringComparison.OrdinalIgnoreCase))
                {
                    string alt = Path.Combine(ThuMucAudio, "bmg_battle.wav");
                    if (File.Exists(alt)) { path = alt; ten = "bmg_battle.wav"; }
                }
                else if (ten.Equals("bmg_battle.wav", StringComparison.OrdinalIgnoreCase))
                {
                    string alt = Path.Combine(ThuMucAudio, "bgm_battle.wav");
                    if (File.Exists(alt)) { path = alt; ten = "bgm_battle.wav"; }
                }
            }

            if (!File.Exists(path))
            {
                DangPhatNhacNen = false;
                return;
            }

            // kiem tra mci dang phat
            if (DangPhatNhacNen && string.Equals(_currentBgm, ten, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    StringBuilder modeBuf = new StringBuilder(64);
                    mciSendString("status bgm mode", modeBuf, 64, IntPtr.Zero);
                    string mode = modeBuf.ToString().Trim().ToLowerInvariant();
                    if (mode == "playing") return;

                    int retryErr = mciSendString("play bgm repeat", null, 0, IntPtr.Zero);
                    if (retryErr != 0) mciSendString("play bgm from 0", null, 0, IntPtr.Zero);
                }
                catch { }
                return;
            }

            DangPhatNhacNen = true;
            _currentBgm = ten;

            Task.Run(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        if (!DangPhatNhacNen) return;
                        mciSendString("close bgm", null, 0, IntPtr.Zero);

                        int err = mciSendString(string.Format("open \"{0}\" type mpegvideo alias bgm", path), null, 0, IntPtr.Zero);
                        if (err != 0)
                        {
                            err = mciSendString(string.Format("open \"{0}\" type waveaudio alias bgm", path), null, 0, IntPtr.Zero);
                        }
                        if (err != 0)
                        {
                            err = mciSendString(string.Format("open \"{0}\" alias bgm", path), null, 0, IntPtr.Zero);
                        }

                        if (err == 0 || err == 266) 
                        {
                            if (DangPhatNhacNen && err == 0)
                            {
                                int playErr = mciSendString("play bgm repeat", null, 0, IntPtr.Zero);
                                if (playErr != 0)
                                {
                                    mciSendString("play bgm from 0", null, 0, IntPtr.Zero);
                                }
                                mciSendString("setaudio bgm volume to 800", null, 0, IntPtr.Zero);
                            }
                            else if (!DangPhatNhacNen)
                            {
                                mciSendString("close bgm", null, 0, IntPtr.Zero);
                            }
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

        private static void KiemTraVaLapNhacNen(object? state)
        {
            if (!DaBatAmThanh || !DangPhatNhacNen) return;
            try
            {
                StringBuilder buf = new StringBuilder(64);
                int err = mciSendString("status bgm mode", buf, 64, IntPtr.Zero);
                if (err == 0)
                {
                    string mode = buf.ToString().Trim().ToLowerInvariant();
                    if (mode == "stopped")
                    {
                        mciSendString("play bgm from 0", null, 0, IntPtr.Zero);
                    }
                }
            }
            catch { }
        }

        public static void DungNhacNen(bool donSachTrack = true)
        {
            DangPhatNhacNen = false;
            if (donSachTrack) _currentBgm = null;
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

        public static void DungSFX()
        {
            try
            {
                lock (_cache)
                {
                    foreach (var p in _cache.Values)
                    {
                        try { p?.Stop(); } catch { }
                    }
                }
            }
            catch { }
        }

        public static void DungTatCa()
        {
            DungNhacNen();
            DungSFX();
        }

        private static SoundPlayer? LayPlayer(string? file)
        {
            if (string.IsNullOrWhiteSpace(file)) return null;
            string ten = file.Replace("\"", "").Trim();

            lock (_cache)
            {
                if (_cache.TryGetValue(ten, out SoundPlayer? p))
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

        public static bool DaDungFallbackSystemSound { get; set; } = false;

        public static void PhatSFX(string? file)
        {
            if (!DaBatAmThanh) return;
            AmThanhVuaPhat = file;

            try
            {
                SoundPlayer? p = LayPlayer(file);
                if (p != null)
                {
                    p.Play();
                }
                else
                {
                    DaDungFallbackSystemSound = true;
                    try { SystemSounds.Beep.Play(); } catch { }
                }
            }
            catch
            {
                DaDungFallbackSystemSound = true;
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
            {
                DungNhacNen(donSachTrack: false);
                DungSFX();
            }
            else if (!string.IsNullOrEmpty(_currentBgm))
            {
                PhatNhacNen(_currentBgm);
            }
            return DaBatAmThanh;
        }

        public static void GiaiPhong()
        {
            try
            {
                _bgmLoopTimer?.Dispose();
                _bgmLoopTimer = null;
            }
            catch { }

            try { DungTatCa(); } catch { }

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
