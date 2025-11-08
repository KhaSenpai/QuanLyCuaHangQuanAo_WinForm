namespace View
{
    partial class ThongKeSanPhan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.dtpStartDateBanChay = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnXemSanPhamBanChay = new System.Windows.Forms.Button();
            this.dtpEndDateBanChay = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvSanPhamBanChay = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.btnXemSanPhamDaBan = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dgvSanPhamDaBan = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvSanPhamTonIt = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvSanPhamMoiNhap = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvSanPham = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamBanChay)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamDaBan)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamTonIt)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamMoiNhap)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.DarkCyan;
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPage2.Location = new System.Drawing.Point(4, 41);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1302, 778);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Thống kê bán hàng";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox7);
            this.groupBox2.Controls.Add(this.dgvSanPhamBanChay);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox2.Location = new System.Drawing.Point(14, 401);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1254, 326);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sản phẩm bán chạy:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.dtpStartDateBanChay);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.btnXemSanPhamBanChay);
            this.groupBox7.Controls.Add(this.dtpEndDateBanChay);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox7.Location = new System.Drawing.Point(20, 33);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(447, 108);
            this.groupBox7.TabIndex = 61;
            this.groupBox7.TabStop = false;
            // 
            // dtpStartDateBanChay
            // 
            this.dtpStartDateBanChay.CustomFormat = "dd/MM/yyyy";
            this.dtpStartDateBanChay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.dtpStartDateBanChay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDateBanChay.Location = new System.Drawing.Point(16, 59);
            this.dtpStartDateBanChay.Name = "dtpStartDateBanChay";
            this.dtpStartDateBanChay.Size = new System.Drawing.Size(135, 30);
            this.dtpStartDateBanChay.TabIndex = 61;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(11, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 25);
            this.label1.TabIndex = 60;
            this.label1.Text = "Từ";
            // 
            // btnXemSanPhamBanChay
            // 
            this.btnXemSanPhamBanChay.BackColor = System.Drawing.Color.Transparent;
            this.btnXemSanPhamBanChay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemSanPhamBanChay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXemSanPhamBanChay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnXemSanPhamBanChay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXemSanPhamBanChay.Location = new System.Drawing.Point(325, 53);
            this.btnXemSanPhamBanChay.Margin = new System.Windows.Forms.Padding(4);
            this.btnXemSanPhamBanChay.Name = "btnXemSanPhamBanChay";
            this.btnXemSanPhamBanChay.Size = new System.Drawing.Size(108, 36);
            this.btnXemSanPhamBanChay.TabIndex = 60;
            this.btnXemSanPhamBanChay.Text = "XEM";
            this.btnXemSanPhamBanChay.UseVisualStyleBackColor = false;
            this.btnXemSanPhamBanChay.Click += new System.EventHandler(this.btnXemSanPhamBanChay_Click);
            // 
            // dtpEndDateBanChay
            // 
            this.dtpEndDateBanChay.CustomFormat = "dd/MM/yyyy";
            this.dtpEndDateBanChay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.dtpEndDateBanChay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDateBanChay.Location = new System.Drawing.Point(173, 59);
            this.dtpEndDateBanChay.Name = "dtpEndDateBanChay";
            this.dtpEndDateBanChay.Size = new System.Drawing.Size(134, 30);
            this.dtpEndDateBanChay.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(168, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 25);
            this.label2.TabIndex = 61;
            this.label2.Text = "Đến";
            // 
            // dgvSanPhamBanChay
            // 
            this.dgvSanPhamBanChay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSanPhamBanChay.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvSanPhamBanChay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPhamBanChay.Location = new System.Drawing.Point(18, 147);
            this.dgvSanPhamBanChay.Name = "dgvSanPhamBanChay";
            this.dgvSanPhamBanChay.RowHeadersWidth = 51;
            this.dgvSanPhamBanChay.RowTemplate.Height = 24;
            this.dgvSanPhamBanChay.Size = new System.Drawing.Size(1214, 166);
            this.dgvSanPhamBanChay.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.dgvSanPhamDaBan);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(14, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1250, 356);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sản phẩm đã bán:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dtpStartDate);
            this.groupBox6.Controls.Add(this.btnXemSanPhamDaBan);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.dtpEndDate);
            this.groupBox6.Location = new System.Drawing.Point(18, 28);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(449, 108);
            this.groupBox6.TabIndex = 60;
            this.groupBox6.TabStop = false;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "dd/MM/yyyy";
            this.dtpStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(18, 58);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(135, 30);
            this.dtpStartDate.TabIndex = 14;
            // 
            // btnXemSanPhamDaBan
            // 
            this.btnXemSanPhamDaBan.BackColor = System.Drawing.Color.Transparent;
            this.btnXemSanPhamDaBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXemSanPhamDaBan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXemSanPhamDaBan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnXemSanPhamDaBan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXemSanPhamDaBan.Location = new System.Drawing.Point(327, 52);
            this.btnXemSanPhamDaBan.Margin = new System.Windows.Forms.Padding(4);
            this.btnXemSanPhamDaBan.Name = "btnXemSanPhamDaBan";
            this.btnXemSanPhamDaBan.Size = new System.Drawing.Size(108, 36);
            this.btnXemSanPhamDaBan.TabIndex = 59;
            this.btnXemSanPhamDaBan.Text = "XEM";
            this.btnXemSanPhamDaBan.UseVisualStyleBackColor = false;
            this.btnXemSanPhamDaBan.Click += new System.EventHandler(this.btnXemSanPhamDaBan_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(170, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 25);
            this.label5.TabIndex = 16;
            this.label5.Text = "Đến";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(13, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 25);
            this.label4.TabIndex = 15;
            this.label4.Text = "Từ";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "dd/MM/yyyy";
            this.dtpEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(175, 58);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(134, 30);
            this.dtpEndDate.TabIndex = 7;
            // 
            // dgvSanPhamDaBan
            // 
            this.dgvSanPhamDaBan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSanPhamDaBan.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvSanPhamDaBan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPhamDaBan.Location = new System.Drawing.Point(18, 147);
            this.dgvSanPhamDaBan.Name = "dgvSanPhamDaBan";
            this.dgvSanPhamDaBan.RowHeadersWidth = 51;
            this.dgvSanPhamDaBan.RowTemplate.Height = 24;
            this.dgvSanPhamDaBan.Size = new System.Drawing.Size(1214, 193);
            this.dgvSanPhamDaBan.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.DarkCyan;
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPage1.Location = new System.Drawing.Point(4, 41);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1302, 778);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Thống kê sản phẩm";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgvSanPhamTonIt);
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox5.Location = new System.Drawing.Point(48, 509);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1219, 214);
            this.groupBox5.TabIndex = 31;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sản phẩm sắp hết hàng";
            // 
            // dgvSanPhamTonIt
            // 
            this.dgvSanPhamTonIt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSanPhamTonIt.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvSanPhamTonIt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPhamTonIt.Location = new System.Drawing.Point(19, 35);
            this.dgvSanPhamTonIt.Name = "dgvSanPhamTonIt";
            this.dgvSanPhamTonIt.RowHeadersWidth = 51;
            this.dgvSanPhamTonIt.RowTemplate.Height = 24;
            this.dgvSanPhamTonIt.Size = new System.Drawing.Size(1181, 155);
            this.dgvSanPhamTonIt.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgvSanPhamMoiNhap);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox4.Location = new System.Drawing.Point(48, 303);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1219, 200);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sản phẩm mới nhập hàng";
            // 
            // dgvSanPhamMoiNhap
            // 
            this.dgvSanPhamMoiNhap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSanPhamMoiNhap.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvSanPhamMoiNhap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPhamMoiNhap.Location = new System.Drawing.Point(19, 42);
            this.dgvSanPhamMoiNhap.Name = "dgvSanPhamMoiNhap";
            this.dgvSanPhamMoiNhap.RowHeadersWidth = 51;
            this.dgvSanPhamMoiNhap.RowTemplate.Height = 24;
            this.dgvSanPhamMoiNhap.Size = new System.Drawing.Size(1181, 139);
            this.dgvSanPhamMoiNhap.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.DarkCyan;
            this.groupBox3.Controls.Add(this.dgvSanPham);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox3.Location = new System.Drawing.Point(48, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1219, 270);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tất cả sản phẩm ";
            // 
            // dgvSanPham
            // 
            this.dgvSanPham.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSanPham.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvSanPham.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSanPham.Location = new System.Drawing.Point(19, 33);
            this.dgvSanPham.Name = "dgvSanPham";
            this.dgvSanPham.RowHeadersWidth = 51;
            this.dgvSanPham.RowTemplate.Height = 24;
            this.dgvSanPham.Size = new System.Drawing.Size(1181, 217);
            this.dgvSanPham.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold);
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1310, 823);
            this.tabControl1.TabIndex = 0;
            // 
            // ThongKeSanPhan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1310, 776);
            this.Controls.Add(this.tabControl1);
            this.Name = "ThongKeSanPhan";
            this.Text = "ThongKeSanPhan";
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamBanChay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamDaBan)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamTonIt)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPhamMoiNhap)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSanPham)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgvSanPhamTonIt;
        private System.Windows.Forms.DataGridView dgvSanPhamMoiNhap;
        private System.Windows.Forms.DataGridView dgvSanPham;
        private System.Windows.Forms.DataGridView dgvSanPhamBanChay;
        private System.Windows.Forms.DataGridView dgvSanPhamDaBan;
        private System.Windows.Forms.Button btnXemSanPhamDaBan;
        private System.Windows.Forms.Button btnXemSanPhamBanChay;
        private System.Windows.Forms.DateTimePicker dtpEndDateBanChay;
        private System.Windows.Forms.DateTimePicker dtpStartDateBanChay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}