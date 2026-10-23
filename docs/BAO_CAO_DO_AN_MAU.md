# TRƯỜNG ĐẠI HỌC SƯ PHẠM THÀNH PHỐ HỒ CHÍ MINH
## KHOA CÔNG NGHỆ THÔNG TIN
### BỘ MÔN: LẬP TRÌNH WINDOWS / C# (.NET 8.0)

---

# BÁO CÁO ĐỒ ÁN MÔN HỌC
## ĐỀ TÀI: XÂY DỰNG TRÒ CHƠI BẮN TÀU (BATTLESHIP TACTICAL NAVAL WARFARE) TRÊN NỀN TẢNG WINDOWS FORMS VÀ THUẬT TOÁN FSM AI

---

### THÔNG TIN NHÓM THỰC HIỆN: NHÓM WINDOWN

| STT | Họ và Tên | Mã số sinh viên | Vai trò trong nhóm | Phân hệ phụ trách chính |
| :---: | :--- | :---: | :--- | :--- |
| **1** | **Huỳnh Gia Trí** | **51.01.104.148** | Trưởng nhóm / Backend & AI Lead | Kiến trúc Model, Ma trận bàn cờ, FSM Bot AI (Hunt & Target), Quản lý ván đấu `cQuanLyVanDau` |
| **2** | **Trần Trọng Hưng** | **51.01.104.132** | Frontend & UI/UX Lead | Giao diện WinForms, Bàn cờ động, Màn che PvP Hotseat, `TacticalMessageBox`, `TacticalVolumeSlider` |
| **3** | **Trần Vũ Hòa Phát** | **51.01.104.072** | System I/O, Audio & QA/Tester | Hệ thống Âm thanh đa luồng (MCI + SoundPlayer), DAO `cThanhTichDAO`, Bẫy lỗi toàn diện & Test Suite |

> **Giảng viên hướng dẫn:** ThS. Nguyễn Văn A  
> **Năm học:** 2026 – 2027  
> **Phiên bản mã nguồn:** 1.0.0 (Code Freeze - 82/82 Unit Tests Passed)

---

## MỤC LỤC TỔNG QUAN

1. **CHƯƠNG 1: TỔNG QUAN ĐỀ TÀI & PHÂN TÍCH YÊU CẦU**
   - 1.1. Giới thiệu trò chơi Bắn Tàu (Battleship)
   - 1.2. Mục tiêu đề tài và yêu cầu kỹ thuật
   - 1.3. Bảng phân chia công việc chi tiết theo tiến độ 7 ngày

2. **CHƯƠNG 2: KIẾN TRÚC HỆ THỐNG VÀ THIẾT KẾ CƠ SỞ DỮ LIỆU**
   - 2.1. Mô hình kiến trúc 3 tầng (3-Tier Layered Architecture)
   - 2.2. Thiết kế tầng Dữ liệu (Models Layer)
   - 2.3. Cấu trúc lưu trữ phẳng `thanhtichs.txt` và giải pháp tối ưu hóa I/O

3. **CHƯƠNG 3: CÁC THUẬT TOÁN CỐT LÕI VÀ TRÍ TUỆ NHÂN TẠO (AI)**
   - 3.1. Thuật toán kiểm tra va chạm và xếp tàu ngẫu nhiên
   - 3.2. Thuật toán Bot AI mô hình máy trạng thái hữu hạn (FSM: Hunt & Target Mode)
   - 3.3. Cơ chế tìm kiếm ô so le (Parity Search) và khóa trục tàu thẳng hàng

4. **CHƯƠNG 4: HỆ THỐNG ÂM THANH ĐA LUỒNG & BẪY LỖI TOÀN DIỆN (CHUYÊN ĐỀ TV3)**
   - 4.1. Kiến trúc âm thanh đa luồng không gián đoạn: Windows MCI (`winmm.dll`) kết hợp `SoundPlayer`
   - 4.2. Cơ chế bẫy lỗi Exception Handling chuẩn 1.0đ:
     - Bẫy lỗi mất file âm thanh: Tự động fallback sang `SystemSounds.Beep`
     - Bẫy lỗi tranh chấp file: Retry Pattern với Exponential Backoff
     - Bẫy lỗi quyền ghi đĩa: Cơ chế điều hướng tự động về `%LOCALAPPDATA%`
     - Bẫy lỗi thao tác người dùng: Khối `try...catch` an toàn tại toàn bộ Event Handlers và Timers
   - 4.3. Bảng xếp hạng kỷ lục: Thuật toán lọc đa tiêu chí và sắp xếp thứ hạng

5. **CHƯƠNG 5: THIẾT KẾ GIAO DIỆN VÀ TRẢI NGHIỆM NGƯỜI DÙNG (UI/UX)**
   - 5.1. Bộ giao diện hải quân hiện đại (Tactical Naval Dark Theme)
   - 5.2. Hộp thoại thông báo tùy biến `TacticalMessageBox` và thanh trượt `TacticalVolumeSlider`
   - 5.3. Cơ chế Màn che sương mù (Fog of War) trong chế độ 2 người chơi (PvP Hotseat)

6. **CHƯƠNG 6: KIỂM THỬ HỆ THỐNG (TESTING & QUALITY ASSURANCE)**
   - 6.1. Phương pháp kiểm thử tự động (Unit Test Harness)
   - 6.2. Ma trận kiểm thử 82 Test Cases (Test Case Matrix)
   - 6.3. Kiểm thử chéo (Cross-Testing) và các kịch bản phá mã (Adversarial Testing)

7. **CHƯƠNG 7: KẾT LUẬN VÀ HƯỚNG PHÁT TRIỂN**
   - 7.1. Đánh giá mức độ hoàn thành so với yêu cầu
   - 7.2. Hạn chế và hướng mở rộng trong tương lai

---

## NỘI DUNG TÓM TẮT CỦA CÁC PHÂN HỆ DO THÀNH VIÊN 3 PHỤ TRÁCH

### 1. Phân hệ Quản lý Dữ liệu Thành tích (`cThanhTichDAO.cs`)
- **Cấu trúc lưu trữ:** Tập tin văn bản UTF-8 không BOM định dạng phân cách pipe `|`.
  `YYYY-MM-DD HH:mm:ss|TênNgườiChơi|ĐiểmSố|SốPhátBắn|ThờiGianGiây|ChếĐộ|KếtQuả`
- **Các biện pháp an toàn dữ liệu:**
  * Khử ký tự phá vỡ định dạng (Sanitization): Tự động đổi ký tự `|` thành `-`, loại bỏ triệt để `\r`, `\n`.
  * Giới hạn kích thước file chống tràn bộ nhớ (Memory DOS Protection): Từ chối đọc nếu file `> 10MB`.
  * Cơ chế thử lại khi file bị khóa (Retry Pattern): Khi tiến trình khác đang ghi, thread sẽ chờ và thử lại tối đa 4 lần với khoảng trễ giãn cách cấp số nhân (50ms $\rightarrow$ 100ms $\rightarrow$ 200ms $\rightarrow$ 400ms).
  * Điều hướng thư mục lưu trữ (Storage Redirection): Tự động tạo thư mục con lồng nhau; nếu phân vùng ứng dụng bị cấm quyền ghi (ReadOnly), dữ liệu được tự động điều hướng sang thư mục an toàn của người dùng `%LOCALAPPDATA%/BattleshipGame/thanhtichs.txt`.

### 2. Phân hệ Âm thanh Đa luồng (`cAmThanh.cs`)
- **Vấn đề kỹ thuật truyền thống:** Lớp `SoundPlayer` của .NET chỉ phát được một âm thanh tại một thời điểm. Nếu phát tiếng đạn bắn hoặc trúng tàu, nhạc nền đang chạy sẽ lập tức bị ngắt.
- **Giải pháp tối ưu hóa:**
  * Nhạc nền chiến trận (BGM): Sử dụng Windows Multimedia API (`mciSendStringW` trong thư viện `winmm.dll`) để phát lặp vô tận trên kênh riêng biệt (`alias bgm`).
  * Hiệu ứng âm thanh tác chiến (SFX): Sử dụng bộ đệm `Dictionary<string, SoundPlayer>` lưu sẵn các sound effect (`shot.wav`, `hit.wav`, `miss.wav`, `sunk.wav`, `victory.wav`, `defeat.wav`).
  * Cơ chế Fallback an toàn: Khi môi trường không có file `.wav` hoặc thiếu card âm thanh phần cứng, hệ thống tự động gọi `SystemSounds.Beep.Play()`, tuyệt đối không phát sinh crash chương trình.

### 3. Đảm bảo Tiêu chí Exception Handling (1.0đ)
- Bọc toàn bộ các hàm tiếp nhận sự kiện (`Button_Click`, `Cell_Click`, `KeyDown`, `FormClosing`) trong các khối `try...catch`.
- Các luồng ngầm và sự kiện Timer (`tmrMatch_Tick`, `tmrTurnCountdown_Tick`) được cô lập biệt lập, bảo vệ vòng lặp chính của ứng dụng Windows Forms.
- Khi phát sinh tình huống bất thường, ứng dụng hiển thị cảnh báo chỉ huy quân sự thông qua `TacticalMessageBox` thay vì hiện cửa sổ crash hệ thống.

---

### KẾT QUẢ ĐÁNH GIÁ CUỐI CÙNG
- **Biên dịch:** `0 Errors, 0 Warnings`
- **Bộ test tự động:** `82/82 Test Cases PASSED (100%)`
- **Đóng băng mã nguồn (Code Freeze):** Sẵn sàng đóng gói và trình chiếu demo.
