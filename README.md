# 🚢 Battleship Tactical War - Game Bắn Tàu Hải Chiến Hiện Đại

Một sản phẩm game máy tính (Desktop App) lấy đề tài hải chiến kinh điển **Battleship**, được xây dựng bằng ngôn ngữ **C#** trên nền tảng **.NET 8.0** và công nghệ **Windows Forms**. 

Dự án áp dụng kiến trúc 3 lớp (**3-Tier Architecture**) tiêu chuẩn công nghiệp kết hợp mô hình máy trạng thái hữu hạn (**FSM - Finite State Machine**) cho Bot AI thông minh, mang lại trải nghiệm thị giác quân sự hiện đại (Tactical Dark Theme) và hệ thống âm thanh đại bác, nhạc nền hoành tráng.

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
- **Màn hình chính (Menu)**: Tích hợp hiệu ứng Radar Scanner quét liên tục, hiển thị danh tính Đô đốc và thanh điều khiển âm lượng thời gian thực.
- **Giai đoạn Xếp tàu thông minh**:
  - Hỗ trợ xem trước vị trí đặt tàu qua bóng mờ (*Preview Shadow*): hiển thị màu Xanh lá khi hợp lệ và Đỏ báo động khi đè lên tàu khác hoặc tràn mép bàn cờ.
  - Phím tắt tiện lợi: Nhấn phím `Space` hoặc `R` để xoay đổi chiều Dọc ↔ Ngang tức thì ngay tại con trỏ chuột.
  - Tính năng **Xếp tự động (Auto-place)**: Tự động sắp xếp ngẫu nhiên 5 tàu vào vị trí hợp lệ chỉ với 1 click.
- **Giai đoạn Tác chiến thời gian thực**:
  - Ô nước biển chưa bắn được bảo vệ bởi màn che sương mù (*Fog of War*).
  - Phân định rõ ràng: Phát bắn Trúng hiển thị ký tự **`X`** rực đỏ, phát bắn Trượt hiển thị vòng sóng nước **`•`**, tàu chìm chuyển toàn bộ sang màu xám tro viền đỏ sẫm.
- Hộp thoại kết quả và thông báo độc quyền (`TacticalMessageBox`, `frmKetQuaTranDau`) thay thế toàn bộ MessageBox mặc định của Windows.

### 3. Hệ thống Âm thanh Quân sự Đa kênh (Audio System)
- Sử dụng thư viện Windows Media Control Interface (`winmm.dll`) để phát nhạc nền trận chiến (*BGM*) lặp vô tận mượt mà.
- Hiệu ứng âm thanh đa kênh (*SFX*) chân thực: tiếng nổ đại bác khai hỏa, đạn rơi tõm vào nước biển khi trượt, tiếng còi báo động chìm tàu, âm hưởng chiến thắng và thất bại.
- Âm thanh SFX phát độc lập không làm ngắt quãng nhạc nền.
- Thanh trượt âm lượng đa tầng (`TacticalVolumeSlider`), có thể kéo chỉnh âm lượng từ 0% đến 100% hoặc click icon loa để tắt/bật nhanh.

### 4. Bảng Xếp Hạng & Lưu Trữ Thành Tích (Leaderboard DAO)
- Quản lý và lưu trữ dữ liệu lịch sử đấu tự động vào file phẳng (`thanhtichs.txt`).
- Tích hợp cơ chế tự phục hồi: Tự động tái tạo thư mục và sinh dữ liệu seed mẫu khi khởi động lần đầu, có fallback lưu vào `AppData` an toàn khi thư mục bị hạn chế quyền ghi.
- Tính năng lọc linh hoạt theo chế độ chơi (*Tất cả*, *Dễ*, *Khó*, *PvP*) và sắp xếp đa tiêu chí (*Điểm cao nhất*, *Thời gian ngắn nhất*, *Số lượt bắn ít nhất*).
- Công thức tính điểm cân bằng (*Game Balance*): Thưởng điểm theo số tàu còn sống sót, trừ điểm theo số phát bắn và thời gian tác chiến.

---

## 🏛️ Kiến Trúc Hệ Thống (3-Tier Architecture)

Dự án được phân tầng chuẩn hóa theo mô hình kiến trúc 3 lớp:

```
src/BattleshipGame/
│
├── Models/                 # TẦNG DỮ LIỆU (Data Entities & Enums)
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
    ├── frmThanhTich.cs     # Màn hình Bảng xếp hạng và lịch sử vinh danh
    ├── frmAbout.cs         # Màn hình hướng dẫn luật chơi chi tiết & thông tin dự án
    ├── frmKetQuaTranDau.cs # Hộp thoại thông báo thắng / bại phong cách Hải quân
    ├── TacticalMessageBox.cs # Hộp thoại quân sự tùy biến thay thế MessageBox.Show
    ├── TacticalVolumeSlider.cs # Control tùy biến thanh kéo chỉnh âm lượng toàn cục
    ├── TacticalBoardControl.cs # Control bàn cờ đồ họa vector
    ├── SonarScannerControl.cs  # Control hiệu ứng radar xoay góc
    ├── TacticalButton.cs   # Nút bấm phong cách quân sự viền bo góc sắc nét
    └── UITheme.cs          # Bảng mã màu, kiểu chữ và tiện ích đồ họa dùng chung
```

---

## 💻 Yêu Cầu Hệ Thống (Prerequisites)

- **Hệ điều hành**: Windows 10 / Windows 11 (khuyến nghị để hiển thị trọn vẹn Windows Forms & WinMM Audio).
- **Runtime**: [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0).
- **Môi trường phát triển (tùy chọn)**:
  - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (chọn workload **.NET Desktop Development**).
  - Hoặc Visual Studio Code / JetBrains Rider có cài đặt .NET 8.0 SDK.

---

## 🚀 Hướng Dẫn Cài Đặt & Sử Dụng (Getting Started)

### Bước 1: Clone Repository về máy
Mở Terminal hoặc Command Prompt / Git Bash và chạy lệnh:
```bash
git clone https://github.com/iamtrihuynh/battleship-fsm-game.git
cd battleship-fsm-game
```

### Bước 2: Mở và Chạy ứng dụng

#### Cách 1: Sử dụng Visual Studio 2022 (Khuyên dùng)
1. Nhấp đúp vào file solution **`BattleshipGame.sln`** ở thư mục gốc để mở dự án bằng Visual Studio.
2. Tại thanh công cụ trên cùng, chọn cấu hình build là **`Debug`** hoặc **`Release`** (với nền tảng `Any CPU` hoặc `x64`).
3. Nhấn phím **`F5`** (hoặc bấm nút **Start / Local Windows Debugger**) để tự động restore dependencies, biên dịch và khởi chạy game.

#### Cách 2: Sử dụng .NET CLI (Command Line)
Nếu đã cài đặt .NET 8.0 SDK trên Windows, bạn có thể khởi chạy trực tiếp qua lệnh:
```bash
dotnet run --project src/BattleshipGame/BattleshipGame.csproj
```

---

## 📖 Hướng Dẫn Trải Nghiệm Game (Gameplay Rules)

1. **Giai đoạn Đặt tên Đô đốc**:
   - Tại Menu chính, bạn có thể nhấp vào nút **`ĐỔI TÊN`** ở góc trên bên trái để thiết lập tên hiển thị trên Bảng xếp hạng.
2. **Giai đoạn Bố trí Hạm đội**:
   - Mỗi bên sở hữu 5 chiến hạm tiêu chuẩn quốc tế:
     - ✈️ **Tàu sân bay** (*Aircraft Carrier*): 5 ô.
     - 🛡️ **Thiết giáp hạm** (*Battleship*): 4 ô.
     - ⚓ **Tàu khu trục** (*Destroyer*): 3 ô.
     - 🌊 **Tàu ngầm** (*Submarine*): 3 ô.
     - 🚤 **Tàu tuần tra** (*Patrol Boat*): 2 ô.
   - Chọn tàu từ danh sách bên trái, rê chuột lên Bàn cờ Đội nhà.
   - Nhấn **`Space`** hoặc **`R`** để đổi hướng tàu (Ngang hoặc Dọc).
   - Nhấp chuột trái để đặt tàu khi bóng mờ hiển thị màu **Xanh lá**.
   - Hoặc nhấp nút **`XẾP TỰ ĐỘNG`** để máy tự sinh vị trí ngẫu nhiên hợp lệ cho 5 tàu trong tích tắc.
3. **Giai đoạn Khai hỏa (Chiến đấu)**:
   - Nhấp vào các ô nước trên **Bàn cờ Tác chiến (Radar)** bên phải để bắn phá vị trí nghi ngờ có tàu đối thủ.
   - **Quy tắc lượt chơi**:
     - Bắn **TRÚNG** (`Hit` / `Sunk`): Bạn được thưởng thêm 1 phát bắn tiếp theo ngay lập tức.
     - Bắn **TRƯỢT** (`Miss`): Lượt bắn lập tức chuyển giao sang đối thủ / Máy.
   - Người chơi nào tiêu diệt toàn bộ 17 ô tàu của đối thủ trước sẽ giành **Chiến thắng chung cuộc**.

---

## 👥 Nhóm Tác Giả & Phân Công Nhiệm Vụ

Dự án được hoàn thiện qua quá trình pair programming và phối hợp nhóm chặt chẽ:
- **Thành viên 1**: Kiến trúc hệ thống, Mô hình dữ liệu (*Models*), Thuật toán bàn cờ ma trận & Bot AI máy trạng thái (*Logic*).
- **Thành viên 2**: Thiết kế giao diện đồ họa chiến thuật, tương tác người dùng, xử lý hiệu ứng bóng mờ & hộp thoại quân sự (*UI/UX Lead & Views*).
- **Thành viên 3**: Tích hợp âm thanh đa kênh WinMM, thiết kế tầng lưu trữ file phẳng (*DataAccess DAO*), tính điểm cân bằng & tối ưu hóa hệ thống.

---

## 📄 Bản Quyền & Tài Liệu Tham Khảo

- Xem thêm tài liệu kiến trúc chi tiết tại: [`docs/system_structure.md`](docs/system_structure.md).
- Yêu cầu đề tài gốc tại: [`docs/requirements.docx`](docs/requirements.docx).
- Giấy phép phân phối: Dự án mã nguồn mở dành cho mục đích học tập và nghiên cứu.
