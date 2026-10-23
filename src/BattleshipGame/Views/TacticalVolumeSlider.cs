#pragma warning disable CS8618, CS8625
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BattleshipGame.Views
{
    /// <summary>
    /// [NGÀY 4 - THÀNH VIÊN 2: TRẦN TRỌNG HƯNG - FRONTEND & UI/UX LEAD]
    /// Thanh điều khiển âm lượng phong cách Hải quân hiện đại (Tactical Naval Dark Theme):
    /// - Cho phép kéo thả (drag & drop) thanh trượt âm lượng mượt mà từ 0% đến 100%.
    /// - Nhấp vào icon loa, nhãn phần trăm hoặc nút âm thanh để Bật/Tắt tức thì trong 1 lần nhấn.
    /// - Kéo về 0%: Tự động chuyển sang trạng thái TẮT.
    /// - Kéo lớn hơn 0%: Tự động chuyển sang trạng thái BẬT.
    /// - Tự động ghi nhớ mức âm lượng trước đó khi Bật lại từ trạng thái Tắt.
    /// - Quản lý trạng thái âm lượng toàn cục (Global Volume) đồng bộ tuyệt đối giữa Menu và Trận đấu.
    /// - Điều chỉnh trực tiếp âm lượng nhạc nền Windows Multimedia (mciSendString) không can thiệp logic backend.
    /// </summary>
    public class TacticalVolumeSlider : Control
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringW", CharSet = CharSet.Unicode)]
        private static extern int mciSendString(string cmd, string ret, int len, IntPtr hwnd);

        public static void CapNhatAmThanhMci(int val, bool muted)
        {
            try
            {
                int mciVol = muted ? 0 : Math.Clamp(val * 10, 0, 1000);
                mciSendString($"setaudio bgm volume to {mciVol}", null, 0, IntPtr.Zero);
            }
            catch { }
        }

        #region TRẠNG THÁI TOÀN CỤC (GLOBAL SYNC)
        private static int _globalVolume = 80;
        private static bool _globalMuted = false;
        private static int _globalSavedVolume = 80;

        public static int GlobalVolume => _globalVolume;
        public static bool GlobalMuted => _globalMuted;
        public static bool IsSoundActive => !_globalMuted && _globalVolume > 0;

        public static event EventHandler GlobalVolumeChanged;

        public static void SetGlobalState(int volume, bool muted, object origin)
        {
            int clamped = Math.Clamp(volume, 0, 100);
            if (clamped == 0)
            {
                muted = true;
            }
            else if (!muted)
            {
                _globalSavedVolume = clamped;
            }

            bool changed = (_globalVolume != clamped) || (_globalMuted != muted);
            _globalVolume = clamped;
            _globalMuted = muted;

            CapNhatAmThanhMci(_globalVolume, _globalMuted);
            BattleshipGame.Logic.cAmThanh.DaBatAmThanh = !_globalMuted && (_globalVolume > 0);

            if (!BattleshipGame.Logic.cAmThanh.DaBatAmThanh)
            {
                BattleshipGame.Logic.cAmThanh.DungNhacNen(donSachTrack: false);
            }
            else if (!string.IsNullOrEmpty(BattleshipGame.Logic.cAmThanh.CurrentBgm) && !BattleshipGame.Logic.cAmThanh.DangPhatNhacNen)
            {
                BattleshipGame.Logic.cAmThanh.PhatNhacNen(BattleshipGame.Logic.cAmThanh.CurrentBgm);
            }

            if (changed)
            {
                GlobalVolumeChanged?.Invoke(origin, EventArgs.Empty);
            }
        }

        public static bool ToggleGlobalMute(object origin = null)
        {
            bool willBeMuted = IsSoundActive;
            int vol = willBeMuted ? 0 : (_globalSavedVolume > 0 ? _globalSavedVolume : 80);
            SetGlobalState(vol, willBeMuted, origin);
            return IsSoundActive;
        }
        #endregion

        #region PROPERTIES
        private int _value = 80;
        private bool _isMuted = false;
        private int _savedVolume = 80;
        private bool _isDragging = false;
        private bool _isHovered = false;
        private bool _isSpeakerHovered = false;
        private bool _isPercentHovered = false;
        private bool _isUpdating = false;

        [Category("Tactical Volume")]
        [Description("Mức âm lượng hiện tại từ 0 đến 100")]
        public int Value
        {
            get => _value;
            set
            {
                int clamped = Math.Clamp(value, 0, 100);
                bool newMuted = (clamped == 0);
                if (clamped > 0)
                {
                    _savedVolume = clamped;
                }

                if (_value != clamped || _isMuted != newMuted)
                {
                    _value = clamped;
                    _isMuted = newMuted;
                    DongBoToanCuc();
                    OnValueChanged();
                    Invalidate();
                }
            }
        }

        [Category("Tactical Volume")]
        [Description("Trạng thái tắt tiếng")]
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                if (_isMuted != value)
                {
                    _isMuted = value;
                    if (_isMuted)
                    {
                        if (_value > 0) _savedVolume = _value;
                        _value = 0;
                    }
                    else
                    {
                        _value = _savedVolume > 0 ? _savedVolume : 80;
                    }

                    DongBoToanCuc();
                    OnValueChanged();
                    Invalidate();
                }
            }
        }

        [Category("Tactical Volume")]
        public Color TrackBackgroundColor { get; set; } = Color.FromArgb(24, 34, 52);

        [Category("Tactical Volume")]
        public Color TrackBorderColor { get; set; } = UITheme.BorderDefault;

        [Category("Tactical Volume")]
        public Color TrackFillColorStart { get; set; } = UITheme.PrimaryCyan;

        [Category("Tactical Volume")]
        public Color TrackFillColorEnd { get; set; } = UITheme.AccentNeon;

        [Category("Tactical Volume")]
        public Color ThumbColor { get; set; } = UITheme.AccentNeon;

        [Category("Tactical Volume")]
        public Color ThumbGlowColor { get; set; } = Color.FromArgb(90, 100, 255, 218);

        public event EventHandler ValueChanged;
        #endregion

        public TacticalVolumeSlider()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.SupportsTransparentBackColor,
                true
            );

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            ForeColor = UITheme.TextPrimary;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            Size = new Size(230, 36);
            Cursor = Cursors.Default;

            // Đồng bộ trạng thái khởi tạo
            _value = GlobalVolume;
            _isMuted = GlobalMuted;
            _savedVolume = _globalSavedVolume > 0 ? _globalSavedVolume : 80;

            // Lắng nghe sự kiện đồng bộ từ các form khác (bỏ qua nếu chính instance này kích hoạt)
            GlobalVolumeChanged += (origin, e) =>
            {
                if (ReferenceEquals(origin, this)) return;
                if (IsDisposed) return;

                if (_value != GlobalVolume || _isMuted != GlobalMuted)
                {
                    _value = GlobalVolume;
                    _isMuted = GlobalMuted;
                    if (_value > 0 && !_isMuted) _savedVolume = _value;
                    OnValueChanged();
                    Invalidate();
                }
            };
        }

        private void DongBoToanCuc()
        {
            if (_isUpdating) return;
            _isUpdating = true;
            try
            {
                SetGlobalState(_value, _isMuted, this);
            }
            finally
            {
                _isUpdating = false;
            }
        }

        /// <summary>
        /// Chuyển đổi tức thì giữa trạng thái Bật và Tắt âm lượng chỉ với 1 lần nhấp duy nhất.
        /// - Nếu đang TẮT: Bật lại và phục hồi mức âm lượng trước đó.
        /// - Nếu đang BẬT: Lưu mức âm lượng hiện tại và tắt về 0.
        /// </summary>
        public void ToggleMute()
        {
            if (_isMuted || _value == 0)
            {
                // Đang TẮT -> BẬT
                _isMuted = false;
                _value = _savedVolume > 0 ? _savedVolume : 80;
            }
            else
            {
                // Đang BẬT -> TẮT
                _savedVolume = _value;
                _value = 0;
                _isMuted = true;
            }

            DongBoToanCuc();
            OnValueChanged();
            Invalidate();
        }

        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        #region TÍNH TOÁN VÙNG TƯƠNG TÁC (LAYOUT GEOMETRY)
        private Rectangle SpeakerRect => new Rectangle(0, 0, 34, Height);
        private int TrackLeft => 38;
        private int TrackRight => Width - 52;
        private int TrackWidth => Math.Max(20, TrackRight - TrackLeft);
        private int TrackY => (Height - 8) / 2;
        private int TrackHeight => 8;
        private Rectangle PercentRect => new Rectangle(Width - 48, 0, 48, Height);

        private float ThumbX
        {
            get
            {
                float ratio = (float)_value / 100f;
                return TrackLeft + ratio * TrackWidth;
            }
        }

        private RectangleF ThumbRect
        {
            get
            {
                float x = ThumbX;
                float y = Height / 2f;
                float r = _isDragging ? 9.5f : (_isHovered ? 8.5f : 7.5f);
                return new RectangleF(x - r, y - r, r * 2f, r * 2f);
            }
        }
        #endregion

        #region XỬ LÝ SỰ KIỆN CHUỘT & BÀN PHÍM
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();

            if (e.Button == MouseButtons.Left)
            {
                // Nhấp vào icon loa hoặc nhãn phần trăm để chuyển Bật / Tắt tức thì
                if (SpeakerRect.Contains(e.Location) || PercentRect.Contains(e.Location))
                {
                    ToggleMute();
                    return;
                }

                // Nhấp vào vùng thanh trượt hoặc con trỏ để điều chỉnh âm lượng
                if (e.X >= TrackLeft - 6 && e.X <= TrackRight + 6)
                {
                    _isDragging = true;
                    CapNhatGiaTriTheoToaDoX(e.X);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool wasSpeakerHovered = _isSpeakerHovered;
            bool wasPercentHovered = _isPercentHovered;
            bool wasHovered = _isHovered;

            _isSpeakerHovered = SpeakerRect.Contains(e.Location);
            _isPercentHovered = PercentRect.Contains(e.Location);
            _isHovered = ThumbRect.Contains(e.Location) || (e.X >= TrackLeft && e.X <= TrackRight && Math.Abs(e.Y - Height / 2) <= 12);

            if (_isSpeakerHovered || _isPercentHovered || _isHovered || _isDragging)
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Default;
            }

            if (_isDragging)
            {
                CapNhatGiaTriTheoToaDoX(e.X);
            }
            else if (wasSpeakerHovered != _isSpeakerHovered || wasPercentHovered != _isPercentHovered || wasHovered != _isHovered)
            {
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_isDragging)
            {
                _isDragging = false;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            _isSpeakerHovered = false;
            _isPercentHovered = false;
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int delta = e.Delta > 0 ? 5 : -5;
            Value = Math.Clamp(Value + delta, 0, 100);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Down)
            {
                Value = Math.Max(0, Value - 5);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Up)
            {
                Value = Math.Min(100, Value + 5);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.M || e.KeyCode == Keys.Space)
            {
                ToggleMute();
                e.Handled = true;
            }
        }

        private void CapNhatGiaTriTheoToaDoX(int mouseX)
        {
            float relativeX = mouseX - TrackLeft;
            float ratio = Math.Clamp(relativeX / (float)TrackWidth, 0f, 1f);
            int newValue = (int)Math.Round(ratio * 100);
            Value = newValue;
        }
        #endregion

        #region VẼ ĐỒ HỌA GDI+ CHUẨN TACTICAL NAVAL DARK THEME
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            UITheme.SetHighQuality(g);

            // 1. NÚT LOA & ICON ÂM LƯỢNG (BÊN TRÁI)
            VeNutLoa(g);

            // 2. THANH RÃNH TRƯỢT (TRACK BACKGROUND)
            Rectangle trackBounds = new Rectangle(TrackLeft, TrackY, TrackWidth, TrackHeight);
            using (GraphicsPath trackPath = UITheme.CreateRoundedPath(trackBounds, TrackHeight / 2))
            {
                using (SolidBrush bgBrush = new SolidBrush(TrackBackgroundColor))
                {
                    g.FillPath(bgBrush, trackPath);
                }
                using (Pen borderPen = new Pen(TrackBorderColor, 1f))
                {
                    g.DrawPath(borderPen, trackPath);
                }
            }

            // 3. THANH ĐẦY HOẠT ĐỘNG (ACTIVE FILL TRACK VỚI GRADIENT NEON)
            float currentThumbX = ThumbX;
            int fillWidth = (int)(currentThumbX - TrackLeft);
            if (fillWidth > 2 && !_isMuted && _value > 0)
            {
                Rectangle fillBounds = new Rectangle(TrackLeft, TrackY, fillWidth, TrackHeight);
                using (GraphicsPath fillPath = UITheme.CreateRoundedPath(fillBounds, TrackHeight / 2))
                {
                    using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                        fillBounds,
                        TrackFillColorStart,
                        TrackFillColorEnd,
                        LinearGradientMode.Horizontal))
                    {
                        g.FillPath(fillBrush, fillPath);
                    }
                }
            }

            // 4. CON TRỎ KÉO (THUMB / KNOB PHÁT SÁNG NEON)
            VeThumbPhatSang(g, currentThumbX);

            // 5. CHỮ PHẦN TRĂM ÂM LƯỢNG (BÊN PHẢI)
            VeChuPhanTram(g);
        }

        private void VeNutLoa(Graphics g)
        {
            Rectangle spk = new Rectangle(2, (Height - 26) / 2, 30, 26);

            if (_isSpeakerHovered)
            {
                using (GraphicsPath hoverBg = UITheme.CreateRoundedPath(spk, 4))
                {
                    using (SolidBrush hb = new SolidBrush(Color.FromArgb(50, 40, 60, 90))) g.FillPath(hb, hoverBg);
                    using (Pen hp = new Pen(UITheme.AccentNeon, 1f)) g.DrawPath(hp, hoverBg);
                }
            }

            // Chọn icon theo mức âm lượng
            string icon;
            Color iconColor;
            if (_isMuted || _value == 0)
            {
                icon = "\U0001F507"; // 🔇 Mute
                iconColor = UITheme.DangerRed;
            }
            else if (_value < 35)
            {
                icon = "\U0001F508"; // 🔈 Low
                iconColor = UITheme.PrimaryCyan;
            }
            else if (_value < 70)
            {
                icon = "\U0001F509"; // 🔉 Medium
                iconColor = UITheme.PrimaryCyan;
            }
            else
            {
                icon = "\U0001F50A"; // 🔊 High
                iconColor = UITheme.AccentNeon;
            }

            using (Font iconFont = new Font("Segoe UI", 12f))
            {
                TextRenderer.DrawText(
                    g,
                    icon,
                    iconFont,
                    spk,
                    iconColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix
                );
            }
        }

        private void VeThumbPhatSang(Graphics g, float cx)
        {
            float cy = Height / 2f;
            float radius = _isDragging ? 9f : (_isHovered ? 8f : 7f);

            // Quầng sáng mờ Neon (Aura Glow)
            if (_isHovered || _isDragging)
            {
                float glowR = radius + 5f;
                RectangleF glowRect = new RectangleF(cx - glowR, cy - glowR, glowR * 2, glowR * 2);
                using (GraphicsPath glowPath = new GraphicsPath())
                {
                    glowPath.AddEllipse(glowRect);
                    using (PathGradientBrush pgb = new PathGradientBrush(glowPath))
                    {
                        pgb.CenterColor = ThumbGlowColor;
                        pgb.SurroundColors = new Color[] { Color.FromArgb(0, 100, 255, 218) };
                        g.FillPath(pgb, glowPath);
                    }
                }
            }

            // Thân thumb hình tròn
            RectangleF thumbRect = new RectangleF(cx - radius, cy - radius, radius * 2, radius * 2);
            Color thumbFill = (_isMuted || _value == 0) ? Color.FromArgb(100, 116, 139) : (_isDragging ? Color.White : ThumbColor);

            using (SolidBrush tb = new SolidBrush(thumbFill))
            {
                g.FillEllipse(tb, thumbRect);
            }

            // Viền ngoài của thumb
            using (Pen tp = new Pen(Color.FromArgb(15, 23, 42), 1.5f))
            {
                g.DrawEllipse(tp, thumbRect);
            }

            // Điểm tâm nhỏ bên trong
            if (!_isMuted && _value > 0 && radius >= 7f)
            {
                float centerR = 2.5f;
                RectangleF cRect = new RectangleF(cx - centerR, cy - centerR, centerR * 2, centerR * 2);
                using (SolidBrush cb = new SolidBrush(Color.FromArgb(15, 23, 42)))
                {
                    g.FillEllipse(cb, cRect);
                }
            }
        }

        private void VeChuPhanTram(Graphics g)
        {
            Rectangle pr = PercentRect;

            if (_isPercentHovered)
            {
                using (GraphicsPath hoverBg = UITheme.CreateRoundedPath(pr, 4))
                {
                    using (SolidBrush hb = new SolidBrush(Color.FromArgb(40, 40, 60, 90))) g.FillPath(hb, hoverBg);
                }
            }

            string text = (_isMuted || _value == 0) ? "T\u1EAET" : $"{_value}%";
            Color textColor = (_isMuted || _value == 0) ? UITheme.DangerRed : (_value > 0 ? UITheme.AccentNeon : UITheme.TextMuted);

            using (Font f = new Font("Segoe UI", 9f, FontStyle.Bold))
            {
                TextRenderer.DrawText(
                    g,
                    text,
                    f,
                    pr,
                    textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix
                );
            }
        }
        #endregion
    }
}
