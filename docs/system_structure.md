# CẤU TRÚC HỆ THỐNG DỰ ÁN BATTLESHIP GAME (PHƯƠNG ÁN A - CLEAN CODE)

```
BattleshipGame/                         <--- Thư mục gốc Repo GitHub
├── docs/                               <--- Tài liệu báo cáo, sơ đồ thiết kế
│   ├── requirements.docx               <--- Đề cương yêu cầu từ Giảng viên
│   └── system_structure.md             <--- Tài liệu kiến trúc hệ thống
│
├── bg/                                 <--- Thư mục âm thanh (Duy nhất tại gốc, clone về thấy ngay)
│   ├── bgm_battle.wav, shot.wav, sunk.wav, victory.wav, defeat.wav, radar.wav
│   ├── true.wav                        <--- Âm thanh phát bắn trúng (alias hit.wav)
│   └── false.wav                       <--- Âm thanh phát bắn trượt (alias miss.wav)
│
├── img/                                <--- Thư mục hình ảnh & logo (Duy nhất tại gốc, clone về thấy ngay)
│   └── logo.png                        <--- Logo huy hiệu hải quân Nhóm 10 WINDOWN
│
├── thanhtichs.txt                      <--- Bảng thành tích dữ liệu mẫu ban đầu (Duy nhất tại gốc)
│
├── BattleshipGame.sln                  <--- File Solution mở bằng Visual Studio
│
└── src/                                <--- Thư mục chứa toàn bộ mã nguồn sạch
    └── BattleshipGame/                 <--- Project Windows Forms App (.NET 8.0)
        │
        ├── Properties/                 <--- Cấu hình WinForms chuẩn (5 files theo Lab mẫu)
        │   ├── AssemblyInfo.cs         <--- Metadata dự án Nhóm 10 WINDOWN (v1.0.0.0)
        │   ├── Resources.resx          <--- Quản lý chuỗi tài nguyên XML
        │   ├── Resources.Designer.cs   <--- Code-behind cho Resources
        │   ├── Settings.settings       <--- Quản lý cấu hình người dùng
        │   └── Settings.Designer.cs    <--- Code-behind cho Settings
        │
        ├── Models/                     <--- TẦNG DỮ LIỆU (Data Entities & Enums)
        │   ├── Enums.cs                <--- Các enum: TrangThaiO, HuongTau, CheDoChoi, KetQuaBan
        │   ├── cO.cs                   <--- Thực thể 1 ô tọa độ (X, Y, đã bắn chưa, có tàu không)
        │   ├── cTau.cs                 <--- Thực thể chiến hạm (kích thước, hướng, tọa độ, kiểm tra chìm)
        │   └── ThanhTichItem.cs        <--- Đối tượng lưu 1 dòng thành tích lịch sử đấu
        │
        ├── Logic/                      <--- TẦNG NGHIỆP VỤ (Business Logic & AI)
        │   ├── cBanCo.cs               <--- Xử lý ma trận 10x10, thuật toán kiểm tra biên & va chạm
        │   ├── cMayBanAI.cs            <--- Thuật toán FSM Bot AI (Hunt Parity & Target Mode)
        │   ├── cQuanLyVanDau.cs        <--- Điều phối lượt chơi, đếm ngược 30s, tính điểm, thắng/thua
        │   └── cAmThanh.cs             <--- Quản lý phát BGM qua winmm và SFX qua SoundPlayer
        │
        ├── DataAccess/                 <--- TẦNG LƯU TRỮ (Data Access Layer - DAO)
        │   └── cThanhTichDAO.cs        <--- Đọc/ghi, lọc và sắp xếp thanhtichs.txt qua System.IO
        │
        ├── Views/                      <--- TẦNG GIAO DIỆN (UI Forms & Custom Controls)
        │   ├── frmMenu.cs              <--- Màn hình chính (Chọn chế độ, sảnh chờ)
        │   ├── frmBanTau.cs            <--- Màn hình bàn cờ chiến đấu 10x10 chính
        │   ├── frmThanhTich.cs         <--- Màn hình bảng xếp hạng (DataGridView)
        │   ├── frmAbout.cs             <--- Màn hình giới thiệu thành viên & Luật chơi (có logo nạp try...catch)
        │   ├── frmKetQuaTranDau.cs     <--- Hộp thoại thông báo kết quả & nhập tên Đô đốc
        │   ├── TacticalMessageBox.cs   <--- Hộp thoại quân sự tùy biến thay thế MessageBox
        │   ├── TacticalVolumeSlider.cs <--- Control kéo chỉnh âm lượng thời gian thực
        │   ├── TacticalBoardControl.cs <--- Control bàn cờ đồ họa
        │   ├── SonarScannerControl.cs  <--- Control radar xoay góc GDI+
        │   ├── TacticalButton.cs       <--- Nút bấm phong cách quân sự viền neon
        │   └── UITheme.cs              <--- Bảng mã màu & font chữ dùng chung
        │
        ├── Program.cs                  <--- Điểm bắt đầu chạy ứng dụng (Main Entry)
        └── BattleshipGame.csproj       <--- Cấu hình link tài nguyên gốc và build dự án
```
