## ĐỀ TÀI: MINIGAME BẮN TÀU - NHÓM WINDOWN (BATTLESHIP TACTICAL WARFARE)

---

### 1. Phân Công Nhiệm Vụ 3 Thành Viên Trong Nhóm
* **Huỳnh Minh Trí - 51.01.104.105** (Thành viên 1 - Trưởng nhóm / Backend & AI Lead)
  * Module phụ trách: `cO.cs`, `cTau.cs`, `cBanCo.cs`, `cMayBanAI.cs`, `cQuanLyVanDau.cs`.
  * Nhiệm vụ Ngày 1: Tạo Repository GitHub, cấu hình `.gitignore`, lập trình các lớp thực thể (`cO.cs`, `cTau.cs`, các enum `TrangThaiO`, `HuongTau`), viết khung lớp `cBanCo.cs`.

* **Trần Trọng Hưng - 51.01.104.132** (Thành viên 2 - Frontend & UI/UX Lead)
  * Module phụ trách: `frmMenu.cs`, `frmBanTau.cs` (giao diện), `frmAbout.cs`, thiết kế Icon/Asset đồ họa.
  * Nhiệm vụ Ngày 1:
    1. Tạo cấu trúc Solution WinForms 3 tầng (`Models/`, `Logic/`, `DataAccess/`, `Views/`, `Assets/`).
    2. Thiết kế Form Menu chính (`frmMenu.cs`) với hỗ trợ toàn màn hình (Maximize responsive), hiệu ứng Radar xoay mượt mà, viền Neon phát sáng.
    3. Tạo giao diện tĩnh chuẩn kích thước cho Form Trận Đấu (`frmBanTau.cs`) hỗ trợ mở rộng toàn màn hình, định hình 2 bàn cờ 10x10, Dashboard thông số, kho tàu, kỹ năng và màn che sương mù.

* **Trần Vũ Hòa Phát - 51.01.104.072** (Thành viên 3 - System I/O, Audio & QA/Tester)
  * Module phụ trách: `cAmThanh.cs`, `cThanhTichDAO.cs`, `frmThanhTich.cs`, Báo cáo Word/Slide.
  * Nhiệm vụ Ngày 1: Chuẩn bị kho tài nguyên âm thanh (`.wav`), thiết kế cấu trúc lưu trữ `thanhtichs.txt` và viết lớp thực thể `ThanhTichItem.cs`.

---

### 2. Tiến Độ Thực Hiện Ngày 2 (Thứ Ba) - Trọng Tâm Thành Viên 2 (Frontend & UI/UX Lead)
Theo đúng phân công và yêu cầu tài liệu đề tài, **Thành viên 2 (Trần Trọng Hưng)** đã hoàn thành trọn vẹn các hạng mục giao diện của Ngày 2, không can thiệp vào mã nguồn của Thành viên 1 (`Models/`, `Logic/`) và Thành viên 3 (`DataAccess/`):

1. **Hàm sinh động 10x10 = 100 Button cho 2 bàn cờ (`Form_Load`)**:
   * Tại sự kiện `frmBanTau_Load`, gọi hàm `SinhBanCoDong()` khởi tạo ma trận 100 Button cho **Bàn cờ Đội nhà** (`pnlPlayerBoard`) và 100 Button cho **Bàn cờ Tác chiến / Radar** (`pnlRadarBoard`).
   * Tự động dựng hệ thống nhãn tọa độ quy chuẩn: Hàng dọc **A – J** và Cột ngang **1 – 10**.
   * Quản lý mảng 2 chiều Button (`_btnPlayerGrid[10, 10]` và `_btnRadarGrid[10, 10]`) với thuộc tính phẳng (`FlatStyle.Flat`), viền kim loại `#475569`, màu nền nước biển sâu `#172554`.

2. **Hiệu ứng bóng mờ (Preview Shadow) khi xếp tàu**:
   * Gắn sự kiện `MouseEnter`, `MouseLeave` trên từng Button của bàn cờ Đội nhà.
   * Khi rê chuột vào bất kỳ ô nào, hệ thống tính toán vị trí chiếm dụng của chiến hạm đang chọn (2 đến 5 ô):
     * **Vị trí HỢP LỆ**: Tô màu **Xanh lá** (`#10B981`) kèm biểu tượng `⚓`, viền ngọc lục bảo và thông báo hướng dẫn click đặt.
     * **Vị trí KHÔNG HỢP LỆ** (tràn mép bàn cờ hoặc đè lên tàu đã xếp): Tô màu **Đỏ** (`#EF4444`) kèm biểu tượng `✕` và vô hiệu hóa đặt tàu.
   * Khi chuột rời khỏi ô (`MouseLeave`), toàn bộ bàn cờ lập tức hoàn trả màu sắc nguyên bản theo đúng trạng thái thực tế.

3. **Lập trình phím tắt Space và R đổi hướng Dọc ↔ Ngang**:
   * Xử lý sự kiện `frmBanTau_KeyDown` với thuộc tính `KeyPreview = true`.
   * Khi bấm phím **Space** hoặc **R**, biến hướng tàu tự động đảo chiều (`Ngang ↔ Dọc`).
   * **Đặc biệt**: Tự động render lại bóng mờ (Preview) ngay lập tức dưới con trỏ chuột mà người chơi không cần rê chuột ra ngoài rồi rê lại.

4. **Tương tác xếp tàu hoàn chỉnh & hỗ trợ Xếp ngẫu nhiên (Auto Place)**:
   * Cho phép click chọn 5 loại chiến hạm từ kho tàu, click đặt tàu, tự động đánh dấu `[Đã đặt]` và chuyển sang tàu kế tiếp.
   * Nút **Đặt lại (Reset Board)**: Thu hồi toàn bộ 5 tàu về kho chỉ với 1 click.
   * Nút **Xếp ngẫu nhiên (Auto Place)**: Tự động xếp ngẫu nhiên hợp lệ toàn bộ 5 tàu hiển thị ngay ngắn trên bàn cờ.
   * Khi đủ 5/5 tàu, nút **Bắt đầu chiến đấu** tự động kích hoạt chuyển sang giai đoạn tác chiến.
