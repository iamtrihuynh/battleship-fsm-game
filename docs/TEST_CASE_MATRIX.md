# BẢNG MA TRẬN KỊCH BẢN KIỂM THỬ (TEST CASE MATRIX)
## DỰ ÁN: GAME BẮN TÀU (BATTLESHIP FSM GAME - WINFORMS C#)

> **Người thực hiện:** Trần Vũ Hòa Phát - 51.01.104.072 (Thành viên 3 - System I/O, Audio & QA/Tester)  
> **Nhiệm vụ Ngày 6:** Tổng hợp bảng kết quả kiểm thử (Test Case Matrix) đạt kết quả Pass 100%.  
> **Kết quả tổng quan:** `82/82 Test Cases PASSED (100%)`  
> **Framework:** .NET 8.0 Automated Test Harness (`tests/BattleshipGame.Tests/Program.cs`)

---

### BẢNG TỔNG HỢP THEO PHÂN HỆ KIỂM THỬ

| Nhóm kiểm thử | Phân hệ phụ trách | Số lượng Test Case | Trạng thái | Tỷ lệ Pass |
| :--- | :--- | :---: | :---: | :---: |
| **Phần 1: System I/O & File DAO** | TV3 (Phát) | 15 (TC-01 $\rightarrow$ TC-15) | **PASSED** | 100% |
| **Phần 2: Models, Board Logic & Combat** | TV1 (Trí) | 12 (TC-16 $\rightarrow$ TC-27) | **PASSED** | 100% |
| **Phần 3: Audio Engine & Non-interrupt BGM** | TV3 (Phát) | 8 (TC-28 $\rightarrow$ TC-35) | **PASSED** | 100% |
| **Phần 4: Bug Regression & Boundary Verification** | TV1 & TV3 | 9 (TC-36 $\rightarrow$ TC-44) | **PASSED** | 100% |
| **Phần 5: Bot AI FSM (Hunt Parity & Target)** | TV1 (Trí) | 4 (TC-45 $\rightarrow$ TC-48) | **PASSED** | 100% |
| **Phần 6: Game Manager, Scoring & PvP Hotseat** | TV1 & TV3 | 28 (TC-49 $\rightarrow$ TC-76) | **PASSED** | 100% |
| **Phần 7: Exception Handling & System Fallback** | TV3 (Phát) | 6 (TC-77 $\rightarrow$ TC-82) | **PASSED** | 100% |
| **TỔNG CỘNG** | **Toàn bộ hệ thống** | **82 Test Cases** | **PASSED** | **100%** |

---

### CHI TIẾT CÁC TEST CASES ĐIỂN HÌNH CỦA THÀNH VIÊN 3 (SYSTEM I/O, AUDIO & QA)

| Mã TC | Tên kịch bản kiểm thử | Dữ liệu đầu vào (Input) | Kết quả kỳ vọng (Expected) | Kết quả thực tế | Đánh giá |
| :---: | :--- | :--- | :--- | :--- | :---: |
| **TC-01** | Định dạng chuỗi bản ghi thành tích | `ThanhTichItem` hợp lệ | Định dạng chuẩn: `Date\|Name\|Score\|Shots\|Time\|Mode\|Result` | Đúng định dạng phân cách pipe `\|` | **PASS** |
| **TC-02** | Chống Delimiter Injection | Tên người chơi chứa ký tự `\|` | Tự động thay `\|` bằng `-` để không làm vỡ cấu trúc file | Ký tự pipe bị thay thế an toàn | **PASS** |
| **TC-03** | Chống CRLF Injection | Tên người chơi chứa `\r` hoặc `\n` | Tự động loại bỏ ký tự xuống dòng, chống phá hoại file | Chuỗi được làm sạch hoàn toàn | **PASS** |
| **TC-04** | Xử lý ký tự lạ BOM UTF-8 | Chuỗi ngày giờ có tiền tố BOM | Cắt bỏ ký tự BOM, parse ngày giờ chuẩn xác không throw | Parse `DateTime` thành công | **PASS** |
| **TC-05** | Kẹp biên giá trị số nguyên âm | Điểm số, phát bắn truyền số âm `< 0` | Tự động clamp về `0` bảo vệ toàn vẹn dữ liệu | Không xuất hiện giá trị âm | **PASS** |
| **TC-07** | Đọc danh sách kỷ lục từ file | Đọc file `thanhtichs.txt` | Trả về danh sách thực thể `ThanhTichItem` đúng dữ liệu | Đọc đủ và đúng các bản ghi | **PASS** |
| **TC-08** | Sắp xếp kỷ lục Điểm cao nhất | Danh sách nhiều người chơi | Bản ghi điểm cao xếp lên đầu bảng (Rank #1) | Thứ tự giảm dần theo Điểm số | **PASS** |
| **TC-10** | Lọc dữ liệu theo Chế độ chơi | Lọc "PvE" | Khớp đúng các phân hệ PvE Dễ và PvE Nâng cao | Danh sách lọc chính xác | **PASS** |
| **TC-14** | Khôi phục Retry Pattern khi file bị khóa | File bị tiến trình khác lock tạm thời | Tự động thử lại 4 lần với Exponential Backoff | Ghi file thành công sau khi nhả lock | **PASS** |
| **TC-15** | Chống tràn bộ nhớ file quá lớn | File có kích thước `> 10MB` | Từ chối đọc, trả về danh sách rỗng, không gây OOM | Chặn an toàn, bảo vệ RAM | **PASS** |
| **TC-28** | Tích hợp LuuThanhTich và DocThanhTich | Ghi bản ghi mới và đọc lại ngay | Bản ghi mới xuất hiện tức thì trong danh sách | Dữ liệu đồng bộ tức thời | **PASS** |
| **TC-29** | Khởi động nhạc nền trận chiến BGM | Gọi `cAmThanh.PhatNhacNen()` | Cờ `DangPhatNhacNen == true`, mở kênh MCI | Kênh BGM phát lặp vô tận | **PASS** |
| **TC-30** | SFX bắn súng không làm ngắt BGM | Gọi `PhatShot()` khi BGM đang chạy | Tiếng đại bác nổ qua SoundPlayer, BGM không ngắt | Âm thanh đa luồng song song | **PASS** |
| **TC-31** | Hiệu ứng âm thanh Trượt (Miss) | Gọi `PhatMiss()` | Phát `miss.wav` chuẩn xác | Âm thanh phát mượt mà | **PASS** |
| **TC-32** | Hiệu ứng âm thanh Trúng (Hit) | Gọi `PhatHit()` | Phát `hit.wav` chuẩn xác | Âm thanh phát mượt mà | **PASS** |
| **TC-33** | Hiệu ứng âm thanh Chìm tàu (Sunk) | Gọi `PhatSunk()` | Phát `sunk.wav` chuẩn xác | Âm thanh phát mượt mà | **PASS** |
| **TC-34** | Âm thanh Chiến thắng (Victory) | Gọi `PhatVictory()` | Tự động dừng BGM chiến đấu và phát `victory.wav` | BGM dừng, âm hưởng thắng trận vang lên | **PASS** |
| **TC-35** | Âm thanh Thất bại (Defeat) | Gọi `PhatDefeat()` | Tự động dừng BGM chiến đấu và phát `defeat.wav` | BGM dừng, âm thanh bại trận vang lên | **PASS** |
| **TC-72a** | Mute âm thanh trên thanh công cụ | Click tắt âm thanh | Kênh âm thanh chuyển sang im lặng tức thì | `DaBatAmThanh == false` | **PASS** |
| **TC-72b** | Unmute phục hồi âm thanh | Click bật lại âm thanh | Âm lượng và kênh nhạc nền được khôi phục | `DaBatAmThanh == true` | **PASS** |
| **TC-74** | Kẹp biên công thức tính điểm | Đầu vào `soTauConSong = 99` | Kẹp biên tối đa 5 tàu sống sót (17 ô) | Chống gian lận điểm số | **PASS** |
| **TC-75** | Tự động sinh thư mục lồng nhau | Đường dẫn file nằm trong subfolder chưa có | `Directory.CreateDirectory` tự động tạo cây thư mục | File được tạo an toàn | **PASS** |
| **TC-76** | Ghi danh Bảng Vàng riêng cho Bot AI | Đô đốc tùy chỉnh thắng máy | Lưu chuẩn xác tên Đô đốc và chế độ PvE | Bản ghi lưu chuẩn vào file | **PASS** |
| **TC-77** | Bẫy lỗi thiếu file âm thanh | Gọi phát file `.wav` không tồn tại | Tự động fallback sang `SystemSounds.Beep` không crash | `DaDungFallbackSystemSound == true` | **PASS** |
| **TC-78** | Bẫy lỗi mất file thanhtichs.txt | Xóa file hoặc trỏ đường dẫn mới | Tự động tạo file mẫu mới với 5 bản ghi chuẩn | File mẫu tự sinh 100% | **PASS** |
| **TC-79** | Bẫy lỗi cấm quyền ghi file đĩa | File bị đặt thuộc tính `ReadOnly` | Bắt `UnauthorizedAccessException` và fallback AppData | Không crash, ghi log an toàn | **PASS** |
| **TC-80** | Xử lý ngoại lệ đầu vào dị thường | Truyền `null`, chuỗi rỗng vào Audio & DAO | Hệ thống nuốt lỗi an toàn, trả về giá trị mặc định | Không văng lỗi (Crash-Free) | **PASS** |

---

### KẾT LUẬN KIỂM THỬ
- **Tỷ lệ Pass:** `82/82 Test Cases (100%)`
- **Khả năng chịu lỗi (Resilience):** Đáp ứng trọn vẹn điểm tối đa 1.0đ Exception Handling.
- **Tính ổn định:** Hệ thống đã được kiểm thử chéo, bẫy lỗi click chuột, timer và file I/O, đạt tiêu chuẩn **Code Freeze** sẵn sàng bàn giao và chấm điểm.
