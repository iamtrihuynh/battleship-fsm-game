namespace BattleshipGame.Views
{
    partial class frmThanhTich
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnSortLuotBan = new System.Windows.Forms.Button();
            this.btnSortThoiGian = new System.Windows.Forms.Button();
            this.btnSortDiem = new System.Windows.Forms.Button();
            this.cboCheDo = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.dgvThanhTich = new System.Windows.Forms.DataGridView();
            this.colHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLuot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThoiGian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheDo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKetQua = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblTongSo = new System.Windows.Forms.Label();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThanhTich)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(920, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSubTitle.Location = new System.Drawing.Point(0, 48);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(920, 24);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "LỊCH SỬ THI ĐẤU & KỶ LỤC TÁC CHIẾN HẢI QUÂN";
            this.lblSubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(218)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(920, 48);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "⚓ BẢNG VINH DANH CHIẾN HẠM ⚓";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlControls.Controls.Add(this.btnSortLuotBan);
            this.pnlControls.Controls.Add(this.btnSortThoiGian);
            this.pnlControls.Controls.Add(this.btnSortDiem);
            this.pnlControls.Controls.Add(this.cboCheDo);
            this.pnlControls.Controls.Add(this.lblFilter);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 80);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(920, 50);
            this.pnlControls.TabIndex = 1;
            // 
            // btnSortLuotBan
            // 
            this.btnSortLuotBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnSortLuotBan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSortLuotBan.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnSortLuotBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSortLuotBan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSortLuotBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnSortLuotBan.Location = new System.Drawing.Point(695, 9);
            this.btnSortLuotBan.Name = "btnSortLuotBan";
            this.btnSortLuotBan.Size = new System.Drawing.Size(140, 32);
            this.btnSortLuotBan.TabIndex = 4;
            this.btnSortLuotBan.Text = "🎯 Lượt bắn ít nhất";
            this.btnSortLuotBan.UseVisualStyleBackColor = false;
            this.btnSortLuotBan.Click += new System.EventHandler(this.btnSortLuotBan_Click);
            // 
            // btnSortThoiGian
            // 
            this.btnSortThoiGian.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnSortThoiGian.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSortThoiGian.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnSortThoiGian.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSortThoiGian.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSortThoiGian.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnSortThoiGian.Location = new System.Drawing.Point(535, 9);
            this.btnSortThoiGian.Name = "btnSortThoiGian";
            this.btnSortThoiGian.Size = new System.Drawing.Size(150, 32);
            this.btnSortThoiGian.TabIndex = 3;
            this.btnSortThoiGian.Text = "⏱️ Thời gian nhanh nhất";
            this.btnSortThoiGian.UseVisualStyleBackColor = false;
            this.btnSortThoiGian.Click += new System.EventHandler(this.btnSortThoiGian_Click);
            // 
            // btnSortDiem
            // 
            this.btnSortDiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(165)))), ((int)(((byte)(233)))));
            this.btnSortDiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSortDiem.FlatAppearance.BorderSize = 0;
            this.btnSortDiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSortDiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSortDiem.ForeColor = System.Drawing.Color.White;
            this.btnSortDiem.Location = new System.Drawing.Point(395, 9);
            this.btnSortDiem.Name = "btnSortDiem";
            this.btnSortDiem.Size = new System.Drawing.Size(130, 32);
            this.btnSortDiem.TabIndex = 2;
            this.btnSortDiem.Text = "🏆 Điểm cao nhất";
            this.btnSortDiem.UseVisualStyleBackColor = false;
            this.btnSortDiem.Click += new System.EventHandler(this.btnSortDiem_Click);
            // 
            // cboCheDo
            // 
            this.cboCheDo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.cboCheDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCheDo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCheDo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCheDo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.cboCheDo.FormattingEnabled = true;
            this.cboCheDo.Items.AddRange(new object[] {
            "Tất cả",
            "PvE",
            "PvP Hotseat",
            "Custom"});
            this.cboCheDo.Location = new System.Drawing.Point(135, 12);
            this.cboCheDo.Name = "cboCheDo";
            this.cboCheDo.Size = new System.Drawing.Size(180, 25);
            this.cboCheDo.TabIndex = 1;
            this.cboCheDo.SelectedIndexChanged += new System.EventHandler(this.cboCheDo_SelectedIndexChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.lblFilter.Location = new System.Drawing.Point(20, 16);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(107, 17);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Lọc theo chế độ:";
            // 
            // dgvThanhTich
            // 
            this.dgvThanhTich.AllowUserToAddRows = false;
            this.dgvThanhTich.AllowUserToDeleteRows = false;
            this.dgvThanhTich.AllowUserToResizeRows = false;
            this.dgvThanhTich.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvThanhTich.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvThanhTich.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvThanhTich.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvThanhTich.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvThanhTich.ColumnHeadersHeight = 38;
            this.dgvThanhTich.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvThanhTich.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHang,
            this.colNgay,
            this.colTen,
            this.colDiem,
            this.colLuot,
            this.colThoiGian,
            this.colCheDo,
            this.colKetQua});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(105)))), ((int)(((byte)(161)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvThanhTich.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvThanhTich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThanhTich.EnableHeadersVisualStyles = false;
            this.dgvThanhTich.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.dgvThanhTich.Location = new System.Drawing.Point(0, 130);
            this.dgvThanhTich.MultiSelect = false;
            this.dgvThanhTich.Name = "dgvThanhTich";
            this.dgvThanhTich.ReadOnly = true;
            this.dgvThanhTich.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvThanhTich.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvThanhTich.RowTemplate.Height = 32;
            this.dgvThanhTich.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThanhTich.Size = new System.Drawing.Size(920, 390);
            this.dgvThanhTich.TabIndex = 2;
            // 
            // colHang
            // 
            this.colHang.HeaderText = "Hạng";
            this.colHang.Name = "colHang";
            this.colHang.ReadOnly = true;
            this.colHang.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colHang.Width = 60;
            // 
            // colNgay
            // 
            this.colNgay.HeaderText = "Thời gian";
            this.colNgay.Name = "colNgay";
            this.colNgay.ReadOnly = true;
            this.colNgay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNgay.Width = 150;
            // 
            // colTen
            // 
            this.colTen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTen.HeaderText = "Thuyền trưởng";
            this.colTen.Name = "colTen";
            this.colTen.ReadOnly = true;
            this.colTen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDiem
            // 
            this.colDiem.HeaderText = "Điểm số";
            this.colDiem.Name = "colDiem";
            this.colDiem.ReadOnly = true;
            this.colDiem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDiem.Width = 90;
            // 
            // colLuot
            // 
            this.colLuot.HeaderText = "Số phát";
            this.colLuot.Name = "colLuot";
            this.colLuot.ReadOnly = true;
            this.colLuot.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLuot.Width = 90;
            // 
            // colThoiGian
            // 
            this.colThoiGian.HeaderText = "Thời lượng";
            this.colThoiGian.Name = "colThoiGian";
            this.colThoiGian.ReadOnly = true;
            this.colThoiGian.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colThoiGian.Width = 95;
            // 
            // colCheDo
            // 
            this.colCheDo.HeaderText = "Chế độ";
            this.colCheDo.Name = "colCheDo";
            this.colCheDo.ReadOnly = true;
            this.colCheDo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCheDo.Width = 130;
            // 
            // colKetQua
            // 
            this.colKetQua.HeaderText = "Kết quả";
            this.colKetQua.Name = "colKetQua";
            this.colKetQua.ReadOnly = true;
            this.colKetQua.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colKetQua.Width = 85;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlBottom.Controls.Add(this.lblTongSo);
            this.pnlBottom.Controls.Add(this.btnLamMoi);
            this.pnlBottom.Controls.Add(this.btnDong);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 520);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(920, 60);
            this.pnlBottom.TabIndex = 3;
            // 
            // lblTongSo
            // 
            this.lblTongSo.AutoSize = true;
            this.lblTongSo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongSo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblTongSo.Location = new System.Drawing.Point(20, 21);
            this.lblTongSo.Name = "lblTongSo";
            this.lblTongSo.Size = new System.Drawing.Size(126, 17);
            this.lblTongSo.TabIndex = 2;
            this.lblTongSo.Text = "Tổng số bản ghi: 0";
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnLamMoi.Location = new System.Drawing.Point(670, 14);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 32);
            this.btnLamMoi.TabIndex = 1;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.Location = new System.Drawing.Point(790, 14);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(110, 32);
            this.btnDong.TabIndex = 0;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // frmThanhTich
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(920, 580);
            this.Controls.Add(this.dgvThanhTich);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmThanhTich";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bảng Xếp Hạng Kỷ Lục";
            this.Load += new System.EventHandler(this.frmThanhTich_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThanhTich)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cboCheDo;
        private System.Windows.Forms.Button btnSortDiem;
        private System.Windows.Forms.Button btnSortThoiGian;
        private System.Windows.Forms.Button btnSortLuotBan;
        private System.Windows.Forms.DataGridView dgvThanhTich;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblTongSo;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLuot;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheDo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKetQua;
    }
}
