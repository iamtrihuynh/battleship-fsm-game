# 🚢 Battleship Tactical War - Game Bắn Tàu Hải Chiến Hiện Đại

Một sản phẩm game máy tính (Desktop App) lấy đề tài hải chiến kinh điển **Battleship**, được xây dựng bằng ngôn ngữ **C#** trên nền tảng **.NET 8.0** và công nghệ **Windows Forms**. 

Dự án áp dụng kiến trúc 3 lớp (**3-Tier Architecture**) tiêu chuẩn công nghiệp kết hợp mô hình máy trạng thái hữu hạn (**FSM - Finite State Machine**) cho Bot AI thông minh, mang lại trải nghiệm thị giác quân sự hiện đại (Tactical Dark Theme) và hệ thống âm thanh đại bác, nhạc nền hoành tráng.

---

## 👥 THÔNG TIN DỰ ÁN & NHÓM THỰC HIỆN

- **Học phần:** Lập trình trên Windows (`2611COMP101901`)
- **Tên nhóm:** **Nhóm 10 — WINDOWN**
- **Khung giờ báo cáo:** 21:30 – 21:40 | Tối Thứ Năm, ngày 29/10/2026
- **Thành viên nhóm & Phân công công việc (WBS):**
  1. 👑 **Huỳnh Minh Trí** (`51.01.104.105`) - *Trưởng nhóm / Backend & AI Lead*: Thiết kế kiến trúc Models (`cO.cs`, `cTau.cs`, `cBanCo.cs`), thuật toán FSM Bot AI (Hunt Mode Parity & Target Mode bám đuổi), điều phối ván đấu `cQuanLyVanDau.cs`.
  2. 🎨 **Trần Trọng Hưng** (`51.01.104.132`) - *Frontend & UI/UX Lead*: Thiết kế toàn bộ giao diện WinForms chuẩn phong cách Hải quân, xử lý 2 bàn cờ 10x10, hiệu ứng bóng mờ (Preview Shadow), đồ họa phát bắn (`•`, `X`), Custom Controls GDI+ (`SonarScannerControl`, `TacticalButton`).
  3. 🔊 **Trần Vũ Hòa Phát** (`51.01.104.072`) - *System I/O, Audio & QA/Tester*: Tích hợp hệ thống âm thanh đa luồng (`bg/`), quản lý đọc/ghi file `thanhtichs.txt` qua `cThanhTichDAO.cs`, bọc bẫy lỗi `try...catch` toàn diện, soạn kịch bản kiểm thử và video demo.

🎥 **VIDEO DEMO SẢN PHẨM (3 – 5 PHÚT):**
> **[▶️ Xem Video Demo Thuyết Minh & Kiểm Thử Bẫy Lỗi (Google Drive / YouTube)](https://github.com/iamtrihuynh/battleship-fsm-game)** *(Cập nhật link video tại đây)*

---

## 🎯 Tính Năng Nổi Bật (Key Features)

### 1. Chế độ chơi đa dạng
- **Chơi với Máy (PvE - Dễ)**: AI bắn ngẫu nhiên thuần túy, thích hợp cho người mới làm quen hoặc chơi giải trí nhẹ nhàng.
- **Chơi với Máy (PvE - Khó)**: 
  - **Hunt Mode (Săn tìm)**: Áp dụng thuật toán bắn so le theo bàn cờ vua (*Parity Hunting*) giúp bao phủ bàn cờ và tìm ra tàu địch nhanh hơn 50%.
  - **Target Mode (Triệt hạ)**: Khi bắn trúng phát đầu, AI tự động thêm các ô lân cận vào hàng đợi. Khi trúng phát thứ hai, AI lập tức khóa trục (ngang/dọc) và ưu tiên bắn 2 đầu mút để bắn chìm hoàn toàn con tàu.
- **Hai Người Chơi (PvP Hotseat)**: Cho phép 2 người chơi thi đấu đối kháng trực tiếp trên cùng một máy tính, tích hợp màn che sương mù bàn cờ và đồng hồ đếm ngược 30 giây mỗi lượt bắn.

### 2. Giao diện Đồ họa Quân sự Hiện đại (Tactical Naval UI/UX)
- Thiết kế theo phong cách tác chiến hải quân hiện đại với tông màu tối (*Tactical Naval Dark Slate*), viền Neon phát sáng tương phản cao.
- **Màn hình chính (Menu)**: Tích hợp hiệu ứng Radar Scanner quét liên tục, logo hải quân (`img/logo.png`), hiển thị danh tính Đô đốc và thanh điều khiển âm lượng thời gian thực.
- **Giai đoạn Xếp tàu thông minh**:
  - Hỗ trợ xem trước vị trí đặt tàu qua bóng mờ (*Preview Shadow*): hiển thị màu Xanh lá khi hợp lệ và Đỏ báo động khi đè lên tàu khác hoặc tràn mép bàn cờ.
  - Phím tắt tiện lợi: Nhấn phím `Space` hoặc `R` để xoay đổi chiều Dọc ↔ Ngang tức thì ngay tại con trỏ chuột.
  - Tính năng **Xếp tự động (Auto-place)**: Tự động sắp xếp ngẫu nhiên 5 tàu vào vị trí hợp lệ chỉ với 1 click.
- **Giai đoạn Tác chiến thời gian thực**:
  - Ô nước biển chưa bắn được bảo vệ bởi màn che sương mù (*Fog of War*).
  - Phân định rõ ràng: Phát bắn Trúng hiển thị ký tự **`X`** rực đỏ, phát bắn Trượt hiển thị vòng sóng nước **`•`**, tàu chìm chuyển toàn bộ sang màu xám tro viền đỏ sẫm.
- Hộp thoại kết quả và thông báo độc quyền (`TacticalMessageBox`, `frmKetQuaTranDau`) thay thế toàn bộ MessageBox mặc định của Windows.

### 3. Hệ thống Âm thanh Quân sự Đa kênh (`bg/`)
- Đặt tại thư mục chuẩn **`bg/`** (và hỗ trợ `Assets/Audio/`), nạp trực tiếp qua thư viện Windows Media Control Interface (`winmm.dll`) và `SoundPlayer`.
- Đầy đủ âm thanh sự kiện: `true.wav` (phát bắn trúng), `false.wav` (phát bắn trượt), `bgm_battle.wav` (nhạc nền), `shot.wav`, `sunk.wav`, `victory.wav`, `defeat.wav`.
- Thanh trượt âm lượng đa tầng (`TacticalVolumeSlider`), có thể kéo chỉnh âm lượng từ 0% đến 100% hoặc click nút âm thanh để bật/tắt nhanh.

### 4. Bảng Xếp Hạng & Lưu Trữ Thành Tích (`thanhtichs.txt`)
- Quản lý và lưu trữ dữ liệu lịch sử đấu tự động vào file phẳng (`thanhtichs.txt`) qua `System.IO`.
- Tích hợp cơ chế tự phục hồi: Tự động tái tạo thư mục và sinh dữ liệu seed mẫu khi khởi động lần đầu, có fallback lưu vào `AppData` an toàn khi thư mục bị hạn chế quyền ghi.
- Tính năng lọc linh hoạt theo chế độ chơi (*Tất cả*, *Dễ*, *Khó*, *PvP*) và sắp xếp đa tiêu chí trên `DataGridView`.

---

## 🏛️ Kiến Trúc Hệ Thống (3-Tier Architecture)

Dự án được tổ chức chuẩn mực theo mô hình 3 lớp của Windows Forms:

```
battleship-fsm-game/        # THƯ MỤC GỐC REPOSITORY GITHUB
│
├── bg/                     # THƯ MỤC ÂM THANH & BACKGROUND (Chuẩn đề bài)
│   ├── bgm_battle.wav      # Nhạc nền trận chiến
│   ├── true.wav            # Âm thanh bắn trúng (Hit)
│   ├── false.wav           # Âm thanh bắn trượt (Miss)
│   └── shot.wav, sunk.wav, victory.wav, defeat.wav, radar.wav, airstrike.wav
│
├── img/                    # THƯ MỤC HÌNH ẢNH & LOGO (Chuẩn đề bài)
│   └── logo.png            # Logo huy hiệu hải quân (Tactical Naval Insignia)
│
├── thanhtichs.txt          # FILE LƯU TRỮ BẢNG XẾP HẠNG MẪU (Clone về có sẵn)
├── BattleshipGame.sln      # File Solution mở bằng Visual Studio 2022
├── docs/                   # Tài liệu báo cáo, đề cương môn học
│
└── src/BattleshipGame/     # MÃ NGUỒN DỰ ÁN WINFORMS (.NET 8.0)
    │
    ├── Properties/         # CẤU HÌNH DỰ ÁN WINFORMS (Chuẩn 5 files Visual Studio)
    │   ├── AssemblyInfo.cs     # Metadata, bản quyền Nhóm 10 WINDOWN, version 1.0.0.0
    │   ├── Resources.resx      # XML quản lý chuỗi tài nguyên
    │   ├── Resources.Designer.cs # Code-behind sinh tự động cho Resources
    │   ├── Settings.settings   # XML cấu hình thiết lập người dùng
    │   └── Settings.Designer.cs # Code-behind sinh tự động cho Settings
    │
    ├── Models/             # TẦNG DỮ LIỆU (Data Entities & Enums)
│   ├── Enums.cs            # Khai báo các enum: TrangThaiO, HuongTau, LoaiTau, KetQuaBan, CheDoChoi
│   ├── cO.cs               # Thực thể ô cờ (tọa độ X, Y, trạng thái, tàu chiếm giữ)
│   ├── cTau.cs             # Thực thể chiến hạm (kích thước, hướng, danh sách tọa độ, kiểm tra chìm)
│   └── ThanhTichItem.cs    # Đối tượng bản ghi thành tích lịch sử đấu
│
├── Logic/                  # TẦNG NGHIỆP VỤ (Business Logic & Core Engine)
│   ├── cBanCo.cs           # Quản lý không gian ma trận 10x10, thuật toán kiểm tra biên & va chạm
│   ├── cMayBanAI.cs        # Trí tuệ nhân tạo Bot AI (FSM: Hunt Parity & Target Lock Mode)
│   ├── cQuanLyVanDau.cs    # Trình quản lý luồng ván đấu, lượt bắn, điều kiện thắng/thua, tính điểm
│   └── cAmThanh.cs         # Quản lý phát nhạc nền BGM và các hiệu ứng âm thanh SFX
│
├── DataAccess/             # TẦNG LƯU TRỮ (Data Access Layer - DAO)
│   └── cThanhTichDAO.cs    # Đọc, ghi, tìm kiếm, lọc và sắp xếp dữ liệu file thanhtichs.txt
│
└── Views/                  # TẦNG GIAO DIỆN (Presentation & Custom UI Controls)
    ├── frmMenu.cs          # Màn hình chính (sảnh chờ, chọn chế độ chơi, cấu hình Đô đốc)
    ├── frmBanTau.cs        # Màn hình chiến đấu chính (2 bàn cờ 10x10, hạm đội, bảng điều khiển)
    ├── frmThanhTich.cs     # Màn hình Bảng xếp hạng và lịch sử vinh danh (DataGridView)
    ├── frmAbout.cs         # Màn hình hướng dẫn luật chơi chi tiết & thông tin nhóm
    ├── frmKetQuaTranDau.cs # Hộp thoại thông báo thắng / bại phong cách Hải quân
    ├── TacticalMessageBox.cs # Hộp thoại quân sự tùy biến thay thế MessageBox.Show
    ├── TacticalVolumeSlider.cs # Control tùy biến thanh kéo chỉnh âm lượng toàn cục
    ├── TacticalBoardControl.cs # Control bàn cờ đồ họa vector
    ├── SonarScannerControl.cs  # Control hiệu ứng radar xoay góc
    ├── TacticalButton.cs   # Nút bấm phong cách quân sự viền bo góc sắc nét
    └── UITheme.cs          # Bảng mã màu, kiểu chữ và tiện ích đồ họa dùng chung
```

---

## 💻 Yêu Cầu Hệ Thống & Hướng Dẫn Cài Đặt

### Yêu Cầu
- **Hệ điều hành**: Windows 10 / Windows 11.
- **Runtime**: [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0).
- **Môi trường phát triển**: Visual Studio 2022 (chọn workload **.NET Desktop Development**).

### Khởi Chạy Nhanh
```bash
# Clone repository
git clone https://github.com/iamtrihuynh/battleship-fsm-game.git
cd battleship-fsm-game

# Biên dịch và chạy
dotnet run --project src/BattleshipGame/BattleshipGame.csproj
```

---

## 🎮 Phím Tắt Tiện Ích Hỗ Trợ Báo Cáo & Live Demo

- **`Space` hoặc `R`**: Xoay hướng tàu (Ngang ↔ Dọc) khi rê chuột đặt tàu.
- **`F8`**: Bật/Tắt chế độ radar trinh sát (Soi vị trí tàu địch `⚓` phục vụ tester/giảng viên quan sát bot đặt tàu).
- **`F12`**: **Kích hoạt Thắng nhanh Demo** (`KichHoatThangNhanhDemo`) — kết thúc ván đấu ngay lập tức để chuyển thẳng sang màn hình Lưu thành tích và Bảng xếp hạng trong 1 giây (tránh mất thời gian trong 10 phút báo cáo).

---

## 📄 Bản Quyền & Tài Liệu Tham Khảo

- Sơ đồ kiến trúc: [`docs/system_structure.md`](docs/system_structure.md).
- Yêu cầu đề tài gốc: [`docs/requirements.docx`](docs/requirements.docx).
- Giấy phép phân phối: Dự án mã nguồn mở phục vụ học phần 2611COMP101901.
