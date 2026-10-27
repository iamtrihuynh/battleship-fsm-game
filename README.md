# 🚢 Battleship - Game Bắn Tàu Hải Chiến (WinForms)

Dự án game **Bắn Tàu (Battleship)** được xây dựng bằng ngôn ngữ **C#** trên nền tảng **.NET 8.0 Windows Forms**, phục vụ học phần **Lập trình trên Windows** (`2611COMP101901`).

Trò chơi áp dụng kiến trúc 3 lớp tiêu chuẩn kết hợp thuật toán máy trạng thái (FSM) cho Bot tự động, mang đến trải nghiệm đồ họa hải quân hiện đại và âm thanh chiến đấu sống động.

---

## 👥 THÔNG TIN DỰ ÁN & NHÓM THỰC HIỆN

- **Học phần:** Lập trình trên Windows (`2611COMP101901`)
- **Tên nhóm:** **Nhóm 10 — WINDOWN**
- **Khung giờ báo cáo:** 21:30 – 21:40 | Tối Thứ Năm, ngày 29/10/2026
- **Phân công nhiệm vụ các thành viên (WBS):**
  1. 👑 **Huỳnh Minh Trí** (`51.01.104.105`) - *Trưởng nhóm / Backend & Game Logic*: Thiết kế kiến trúc mô hình dữ liệu (Models) và bàn cờ 10x10; lập trình thuật toán Bot bắn tự động FSM (Hunt & Target Mode); xử lý kiểm tra va chạm xếp tàu và điều phối luồng ván đấu.
  2. 🎨 **Trần Trọng Hưng** (`51.01.104.132`) - *Frontend & UI/UX Lead*: Thiết kế toàn bộ giao diện WinForms phong cách hải quân; lập trình đồ họa bàn cờ, hiệu ứng đặt tàu và phát bắn; xây dựng màn hình Hướng dẫn và tối ưu trải nghiệm người dùng.
  3. 🔊 **Trần Vũ Hòa Phát** (`51.01.104.072`) - *System I/O, Audio & QA/Tester*: Tích hợp hệ thống âm thanh (nhạc nền và hiệu ứng chiến đấu); đọc/ghi dữ liệu bảng xếp hạng `thanhtichs.txt` qua `System.IO`; xây dựng cơ chế bẫy lỗi `try...catch` và thực hiện video demo.

🎥 **VIDEO DEMO SẢN PHẨM (3 – 5 PHÚT):**
> **[▶️ Xem Video Demo Thuyết Minh & Kiểm Thử Bẫy Lỗi (Google Drive / YouTube)](https://github.com/iamtrihuynh/battleship-fsm-game)** *(Cập nhật link video nộp bài tại đây)*

---

## 🎯 Các Chức Năng Chính Của Trò Chơi

### 1. Chế Độ Chơi Với Máy (PvE)
Người chơi đối đầu với hệ thống máy tự động trên 2 bàn cờ 10x10 riêng biệt:
- **Chế độ Dễ (Easy)**: Máy bắn ngẫu nhiên vào các ô chưa bắn trên bàn cờ.
- **Chế độ Khó (Hard - Thuật toán FSM)**:
  - **Giai đoạn Săn tìm (Hunt Mode)**: Máy bắn so le theo ô chẵn/lẻ (giống bàn cờ vua) giúp tìm tàu nhanh hơn 50%.
  - **Giai đoạn Triệt hạ (Target Mode)**: Khi bắn trúng tàu, máy tự động chuyển trạng thái để bắn các ô xung quanh; khi trúng từ 2 phát trở lên, máy khóa trục (ngang hoặc dọc) để bắn chìm hoàn toàn con tàu.

### 2. Bố Trí Hạm Đội (Ship Placement)
Hạm đội gồm 5 tàu chiến: Tàu tuần tra (2 ô), Tàu khu trục (3 ô), Tàu ngầm (3 ô), Thiết giáp hạm (4 ô), Tàu sân bay (5 ô).
- **Xem trước vị trí (Preview Shadow)**: Hiện màu xanh khi vị trí hợp lệ, hiện màu đỏ khi đè lên tàu khác hoặc vượt ra ngoài bàn cờ.
- **Xoay tàu nhanh**: Nhấn phím `Space` hoặc `R` để đổi hướng Dọc ↔ Ngang ngay tại con trỏ chuột.
- **Xếp tự động (Auto-place)**: 1 click để máy tự động xếp ngẫu nhiên 5 tàu vào vị trí hợp lệ.

### 3. Điều Khiển Trận Đấu & Đồng Hồ (Game Control)
- **Đầy đủ nút điều khiển**: Bắt đầu (`Start`), Tạm dừng (`Pause`), Tiếp tục (`Resume`), Chơi lại (`Restart`), Thoát game.
- **2 Đồng hồ độc lập**:
  - Đồng hồ đếm tổng thời gian ván đấu (`⏱ Trận: mm:ss`).
  - Đồng hồ đếm ngược từng lượt bắn (`⏳ Lượt: 30s` kèm thanh thời gian), tự động chuyển lượt khi người chơi hết giờ suy nghĩ.
- **Giao diện mượt mà**: Sử dụng `Timer` và xử lý bất đồng bộ, không dùng lệnh `Thread.Sleep` gây đơ giao diện.

### 4. Hệ Thống Âm Thanh (`bg/`)
- Đặt tại thư mục `bg/`, nạp qua thư viện `winmm.dll` (nhạc nền) và `SoundPlayer` (hiệu ứng).
- Đầy đủ âm thanh theo yêu cầu: `true.wav` (bắn trúng), `false.wav` (bắn trượt), `bgm_battle.wav` (nhạc nền), `shot.wav` (phát bắn), `sunk.wav` (tàu chìm), `victory.wav` (thắng), `defeat.wav` (thua).
- Nút Bật/Tắt âm thanh nhanh và thanh trượt điều chỉnh âm lượng từ 0% đến 100%.

### 5. Bảng Xếp Hạng & Lưu Trữ (`thanhtichs.txt`)
- Đọc và ghi dữ liệu lịch sử đấu ra file `thanhtichs.txt` bằng `System.IO`.
- **Nhập tên người chơi**: Khi thắng trận, xuất hiện popup để người chơi nhập tên chính thức lưu vào bảng thành tích.
- **Màn hình Bảng xếp hạng**: Hiển thị bằng `DataGridView`, hỗ trợ lọc theo chế độ chơi và sắp xếp theo Điểm số, Thời gian, Số lượt bắn; vinh danh huy chương 🥇, 🥈, 🥉.

### 6. Màn Hình Hướng Dẫn & Thông Tin (`frmAbout`)
- Gồm 4 mục trực quan:
  - **Luật chơi**: Hướng dẫn chi tiết cách chơi cho người mới.
  - **Đội tàu**: Thông số kích thước từng loại tàu chiến.
  - **Chiến thuật**: Gợi ý mẹo bắn so le và tập trung hỏa lực.
  - **Đội ngũ phát triển**: Hiển thị đầy đủ Họ tên, MSSV, Lớp và phân công nhiệm vụ của 3 thành viên nhóm WINDOWN.

### 7. Cơ Chế Bẫy Lỗi (Exception Handling - try...catch)
- Ứng dụng bọc bẫy lỗi ở tất cả các thao tác đọc/ghi file và nạp tài nguyên:
  - Khi thiếu thư mục âm thanh `bg/`: Hiện thông báo cảnh báo và tự chuyển sang âm thanh mặc định của hệ thống.
  - Khi thiếu thư mục ảnh `img/`: Hiện thông báo cảnh báo và tự vẽ logo dự phòng.
  - Khi thiếu file `thanhtichs.txt`: Hiện thông báo cảnh báo và tự động tái tạo file mẫu ban đầu.
- **Đảm bảo ứng dụng không bị văng (crash)** khi kiểm thử thực tế.

---

## 🏛️ Cấu Trúc Dự Án

```
battleship-fsm-game/
│
├── bg/                     # Thư mục chứa âm thanh (.wav)
│   ├── bgm_battle.wav      # Nhạc nền trận đấu
│   ├── true.wav            # Âm thanh bắn trúng (Hit)
│   ├── false.wav           # Âm thanh bắn trượt (Miss)
│   └── shot.wav, sunk.wav, victory.wav, defeat.wav, radar.wav, airstrike.wav
│
├── img/                    # Thư mục hình ảnh
│   └── logo.png            # Logo hải quân của nhóm
│
├── thanhtichs.txt          # File lưu thành tích người chơi
├── BattleshipGame.sln      # File Solution Visual Studio
├── docs/                   # Tài liệu đồ án
│
└── src/BattleshipGame/     # Mã nguồn C# WinForms
    ├── Properties/         # Cấu hình dự án (AssemblyInfo, Resources, Settings)
    ├── Models/             # Tầng dữ liệu: cO.cs, cTau.cs, ThanhTichItem.cs, Enums.cs
    ├── Logic/              # Tầng nghiệp vụ: cBanCo.cs, cMayBanAI.cs, cQuanLyVanDau.cs, cAmThanh.cs
    ├── DataAccess/         # Tầng lưu trữ: cThanhTichDAO.cs (đọc/ghi file System.IO)
    └── Views/              # Tầng giao diện: frmMenu, frmBanTau, frmThanhTich, frmAbout, frmKetQuaTranDau
```

---

## 💻 Hướng Dẫn Cài Đặt & Chạy Game

### Yêu Cầu
- Hệ điều hành: Windows 10 hoặc Windows 11.
- Môi trường: Đã cài sẵn [.NET 8.0 SDK / Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) hoặc Visual Studio 2022.

### Khởi Chạy
```bash
# Clone repository về máy
git clone https://github.com/iamtrihuynh/battleship-fsm-game.git
cd battleship-fsm-game

# Chạy game trực tiếp bằng lệnh
dotnet run --project src/BattleshipGame/BattleshipGame.csproj
```
*Hoặc mở file `BattleshipGame.sln` bằng Visual Studio 2022 và nhấn **F5**.*

---

## 🎮 Phím Tắt Tiện Ích Khi Báo Cáo

- **`Space` hoặc `R`**: Đổi chiều tàu (Dọc ↔ Ngang) khi đang đặt tàu.
- **`F8`**: Bật/Tắt chế độ soi vị trí tàu của máy (hỗ trợ kiểm tra máy xếp tàu).
- **`F12`**: **Kích hoạt Thắng nhanh Demo** — kết thúc ván thắng ngay lập tức để chuyển thẳng sang popup lưu tên và xem bảng xếp hạng trong 1 giây (tiết kiệm thời gian khi báo cáo 10 phút).
