# BẢNG THEO DÕI TIẾN ĐỘ DỰ ÁN BẮN TÀU (BATTLESHIP FSM GAME)

> **Cập nhật lần cuối:** 23/10/2026  
> **Ngôn ngữ & Nền tảng:** C# (.NET 8.0 - Windows Forms)  
> **Kiến trúc:** 3-Tier Layered Architecture (Models - Logic/DataAccess - Views)  
> **Trạng thái biên dịch:** `Build Succeeded - 0 Error, 0 Warning`  
> **Kiểm thử tự động:** `82/82 Test Cases PASSED (100%)`

---

## BẢNG TỔNG QUAN TIẾN ĐỘ THEO NGÀY

| Ngày | Nội dung trọng tâm | Trạng thái |
| :---: | :--- | :---: |
| **Ngày 1** | Thiết lập Solution, Mô hình Dữ liệu (Models), Khung Form & Tài nguyên | **DONE** |
| **Ngày 2** | Thuật toán Xếp tàu, Ma trận 100 Button động & Hệ thống Âm thanh | **DONE** |
| **Ngày 3** | Trí tuệ Nhân tạo Bot AI (FSM Hunt & Target), Đồ họa Phát bắn & DAO | **DONE** |
| **Ngày 4** | Quản lý ván đấu, Đếm ngược 30s, Thưởng lượt & Màn che PvP Hotseat | **DONE** (Cả 3 thành viên hoàn thành) |
| **Ngày 5** | Bẫy lỗi toàn diện (Exception Handling), Kỹ năng đặc biệt & Tối ưu UX | **PARTIAL** (Hưng & Phát đã xong; Trí chưa làm) |
| **Ngày 6** | Kiểm thử chéo (Cross-Testing), Ma trận Test Case & Viết Báo cáo Word | **PARTIAL** (Hưng & Phát đã xong; Trí chưa viết báo cáo AI) |
| **Ngày 7** | Build Release, Thiết kế Slide báo cáo, Quay Video Demo & Đóng gói | **IN PROGRESS** |

---

## CHI TIẾT CÔNG VIỆC TỪNG NGÀY & PHÂN CÔNG THÀNH VIÊN

### 📅 NGÀY 1 (Thứ Hai): Khung kiến trúc, Mô hình dữ liệu & Giao diện tĩnh

| Thành viên | Nhiệm vụ chi tiết | File liên quan | Trạng thái |
| :--- | :--- | :--- | :---: |
| **Trí (TV1)** | Tạo Git repo, cấu hình `.gitignore` chuẩn Visual Studio | `.gitignore` | **DONE** |
| **Trí (TV1)** | Lập trình các thực thể dữ liệu: `Enums.cs`, `cO.cs`, `cTau.cs` | `Models/` | **DONE** |
| **Trí (TV1)** | Dựng khung lớp ma trận bàn cờ $10 \times 10$ (`cBanCo.cs`) | `Logic/cBanCo.cs` | **DONE** |
| **Hưng (TV2)** | Khởi tạo Solution WinForms và cấu hình 3 tầng | `BattleshipGame.sln` | **DONE** |
| **Hưng (TV2)** | Thiết kế Menu chính (`frmMenu`) và hiệu ứng Sonar Radar | `Views/frmMenu.cs` | **DONE** |
| **Hưng (TV2)** | Phác thảo kích thước tĩnh cho màn hình trận đấu `frmBanTau` | `Views/frmBanTau.cs` | **DONE** |
| **Phát (TV3)** | Thu thập 9 file âm thanh `.wav` (súng đại bác, trúng, trượt, chìm, bgm) | `Assets/Audio/` | **DONE** |
| **Phát (TV3)** | Thiết kế cấu trúc `thanhtichs.txt` và thực thể `ThanhTichItem.cs` | `Models/ThanhTichItem.cs` | **DONE** |

---

### 📅 NGÀY 2 (Thứ Ba): Thuật toán Xếp tàu, Bàn cờ Động & Âm thanh

| Thành viên | Nhiệm vụ chi tiết | File liên quan | Trạng thái |
| :--- | :--- | :--- | :---: |
| **Trí (TV1)** | Thuật toán chặn va chạm và tràn biên (`KiemTraDatTauHopLe`) | `Logic/cBanCo.cs` | **DONE** |
| **Trí (TV1)** | Thuật toán tự động sinh ngẫu nhiên 5 tàu (`TuDongSinhBanCoNgauNhien`) | `Logic/cBanCo.cs` | **DONE** |
| **Trí (TV1)** | Logic đặt tàu thủ công (`DatTau`) và xóa bàn cờ (`XoaTatCaTau`) | `Logic/cBanCo.cs` | **DONE** |
| **Hưng (TV2)** | Sinh động $10 \times 10 = 100$ Button cho 2 bàn cờ lúc Form_Load | `Views/frmBanTau.cs` | **DONE** |
| **Hưng (TV2)** | Hiệu ứng bóng mờ xem trước (Xanh lá hợp lệ, Đỏ đè/tràn biên) | `Views/frmBanTau.cs` | **DONE** |
| **Hưng (TV2)** | Phím tắt xoay tàu `Space` / `R` đổi chiều Ngang ↔ Dọc | `Views/frmBanTau.cs` | **DONE** |
| **Phát (TV3)** | Module âm thanh `cAmThanh.cs` (mciSendString BGM + SoundPlayer SFX) | `Logic/cAmThanh.cs` | **DONE** |
| **Phát (TV3)** | Giao diện Bảng xếp hạng kỷ lục (`frmThanhTich.cs`, DataGridView) | `Views/frmThanhTich.cs` | **DONE** |

---

### 📅 NGÀY 3 (Thứ Tư): Trí tuệ Nhân tạo Bot AI, Đồ họa Bắn & DAO

| Thành viên | Nhiệm vụ chi tiết | File liên quan | Trạng thái |
| :--- | :--- | :--- | :---: |
| **Trí (TV1)** | Lập trình Bot AI Hunt Mode: Săn tìm theo ô so le Parity $(X+Y) \pmod 2 == 0$ | `Logic/cMayBanAI.cs` | **DONE** |
| **Trí (TV1)** | Lập trình Bot AI Target Mode: Bám đuổi 4 hướng loang BFS & khóa trục tàu | `Logic/cMayBanAI.cs` | **DONE** |
| **Trí (TV1)** | Tiếp nhận phát bắn `TiepNhanPhatBan` trả về `Miss`, `Hit`, `Sunk` | `Logic/cBanCo.cs` | **DONE** |
| **Hưng (TV2)** | Đồ họa phát bắn trực quan: Trượt (`•`), Trúng (`✕` Đỏ), Chìm (`💀` Đỏ thẫm) | `Views/frmBanTau.cs` | **DONE** |
| **Hưng (TV2)** | Thiết kế Panel Màn che sương mù (`pnlFogOverlay`) cho PvP Hotseat | `Views/frmBanTau.cs` | **DONE** |
| **Phát (TV3)** | Lớp `cThanhTichDAO.cs`: `LuuThanhTich()` và `DocDanhSach()` | `DataAccess/` | **DONE** |
| **Phát (TV3)** | Viết bộ kiểm thử Unit Test âm thanh combat không ngắt BGM (TC-28 $\rightarrow$ TC-35) | `tests/` | **DONE** |

---

### 📅 NGÀY 4 (Thứ Năm): Quản lý Ván đấu, Countdown 30s & PvP Hotseat

| Thành viên | Nhiệm vụ chi tiết | File liên quan | Trạng thái |
| :--- | :--- | :--- | :---: |
| **Trí (TV1)** | Cơ chế chuyển lượt: Trượt $\rightarrow$ Đổi lượt; Trúng/Chìm $\rightarrow$ Thưởng lượt bắn tiếp | `Views/frmBanTau.cs` | **DONE** |
| **Trí (TV1)** | Điều kiện kết thúc trận khi chìm toàn bộ 5 tàu (17 ô) | `Views/frmBanTau.cs` | **DONE** |
| **Trí (TV1)** | Bộ đếm giờ ván đấu (`tmrMatch`) và đếm ngược 30s mỗi lượt (`tmrTurnCountdown`) | `Views/frmBanTau.cs` | **DONE** |
| **Trí (TV1)** | Tách lớp quản lý điều phối ván đấu độc lập `cQuanLyVanDau.cs` (FSM + TinhDiem) | `Logic/cQuanLyVanDau.cs` | **DONE** |
| **Hưng (TV2)** | Hoàn thiện luồng đổi lượt PvP Hotseat (đổi giao diện bàn cờ 2 người chơi) | `Views/frmBanTau.cs` | **DONE** |
| **Hưng (TV2)** | Form Hướng dẫn & Giới thiệu thành viên nhóm (`frmAbout.cs`) | `Views/frmAbout.cs` | **DONE** |
| **Phát (TV3)** | Viết công thức tính điểm chuẩn kết thúc trận đấu (`cThanhTichDAO.TinhDiem`) | `DataAccess/cThanhTichDAO.cs` | **DONE** |
| **Phát (TV3)** | Hộp thoại nhập tên ghi danh (`HienThiHopThoaiNhapTen`) & tự động lưu `thanhtichs.txt` | `Views/frmBanTau.cs` | **DONE** |

---

### 📅 NGÀY 5 (Thứ Sáu): Xử lý Ngoại lệ (Exception Handling) & Tinh chỉnh UX

| Thành viên | Nhiệm vụ chi tiết | File liên quan | Trạng thái |
| :--- | :--- | :--- | :---: |
| **Trí (TV1)** | Rà soát AI không bắn trùng ô, không lặp vô tận khi tàu chìm | `Logic/cMayBanAI.cs` | **NOT DONE** (Dừng ở Ngày 4) |
| **Trí (TV1)** | Kỹ năng đặc biệt: Radar trinh sát $3 \times 3$ và Pháo kích diện rộng dấu `+` | `Views/frmBanTau.cs` | **NOT DONE** (Dừng ở Ngày 4) |
| **Hưng (TV2)** | Nút bật/tắt âm thanh (Mute/Unmute) trên thanh công cụ kết hợp slider | `Views/frmMenu.cs` | **DONE** |
| **Hưng (TV2)** | Khóa toàn bộ các nút không hợp lệ khi đang trong trận đấu | `Views/frmBanTau.cs` | **DONE** |
| **Hưng (TV2)** | Chống chớp giật giao diện (DoubleBuffered) và căn chỉnh giao diện tự co giãn | `Views/` | **DONE** |
| **Phát (TV3)** | Bẫy lỗi thiếu file âm thanh/ảnh: Tự động fallback sang `SystemSounds.Beep` | `Logic/cAmThanh.cs` | **DONE** |
| **Phát (TV3)** | Bẫy lỗi mất file `thanhtichs.txt`: Tự động khởi tạo file mẫu mới | `DataAccess/` | **DONE** |
| **Phát (TV3)** | Cơ chế Retry Pattern với Exponential Backoff chống lỗi khóa file & AppData fallback | `DataAccess/` | **DONE** |
| **Phát (TV3)** | Bọc khối `try...catch` an toàn tại toàn bộ sự kiện click chuột và luồng Timer | `Views/` | **DONE** |

---

### 📅 NGÀY 6 (Thứ Bảy): Kiểm thử chéo & Soạn Báo cáo

| Thành viên | Nhiệm vụ chi tiết | Trạng thái |
| :--- | :--- | :---: |
| **Cả nhóm** | Chạy kiểm thử tự động toàn diện và test tay trên Windows 10/11 | **DONE** (87 Test Cases PASSED 100%) |
| **Trí (TV1)** | Viết phần báo cáo: Mô hình lớp (Class Diagram), FSM & Thuật toán AI | **NOT DONE** (Chưa viết) |
| **Hưng (TV2)** | Tinh chỉnh hộp thoại `TacticalMessageBox` (Chiến thắng, Thất bại, Đầu hàng) & Báo cáo UI | **DONE** |
| **Phát (TV3)** | Tổng hợp bảng Ma trận kiểm thử (Test Case Matrix 87 Kịch bản Pass 100%) | **DONE** (`docs/TEST_CASE_MATRIX.md`) |
| **Phát (TV3)** | Soạn thảo khung Báo cáo đồ án Word theo mẫu chuẩn khoa CNTT | **DONE** (`docs/BAO_CAO_DO_AN_MAU.md`) |
| **Phát (TV3)** | Đóng băng mã nguồn (Code Freeze), chuẩn bị bàn giao và xuất bản Release | **DONE** |

---

### 📅 NGÀY 7 (Chủ Nhật): Đóng gói Release & Trình chiếu Demo

| Thành viên | Nhiệm vụ chi tiết | Trạng thái |
| :--- | :--- | :---: |
| **Cả nhóm** | Xuất bản file chạy độc lập (`Release/publish`) không cần cài Visual Studio | **NOT DONE** |
| **Trí (TV1)** | Chuẩn bị slide trình bày phần Backend, Logic bàn cờ & Bot AI | **NOT DONE** |
| **Hưng (TV2)** | Chuẩn bị kịch bản demo trực tiếp các tính năng trên lớp | **NOT DONE** |
| **Phát (TV3)** | Tổng hợp nộp file ZIP (.sln + Báo cáo Word + Slide PPT) cho giảng viên | **NOT DONE** |
