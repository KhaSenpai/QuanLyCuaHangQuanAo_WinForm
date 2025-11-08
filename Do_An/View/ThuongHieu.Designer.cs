namespace View
{
    partial class ThuongHieu
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtTimKiemTH = new System.Windows.Forms.TextBox();
            this.txtMoTaTH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTenThuongHieu = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMaThuongHieu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTimKiemTH = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLamMoiTH = new System.Windows.Forms.Button();
            this.btnXoaTH = new System.Windows.Forms.Button();
            this.btnSuaTH = new System.Windows.Forms.Button();
            this.btnThemTH = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvThuongHieu = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuongHieu)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTimKiemTH
            // 
            this.txtTimKiemTH.Location = new System.Drawing.Point(28, 27);
            this.txtTimKiemTH.Multiline = true;
            this.txtTimKiemTH.Name = "txtTimKiemTH";
            this.txtTimKiemTH.Size = new System.Drawing.Size(1136, 36);
            this.txtTimKiemTH.TabIndex = 48;
            // 
            // txtMoTaTH
            // 
            this.txtMoTaTH.Location = new System.Drawing.Point(172, 65);
            this.txtMoTaTH.Multiline = true;
            this.txtMoTaTH.Name = "txtMoTaTH";
            this.txtMoTaTH.Size = new System.Drawing.Size(1046, 85);
            this.txtMoTaTH.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(30, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Mô tả";
            // 
            // txtTenThuongHieu
            // 
            this.txtTenThuongHieu.Location = new System.Drawing.Point(586, 26);
            this.txtTenThuongHieu.Name = "txtTenThuongHieu";
            this.txtTenThuongHieu.Size = new System.Drawing.Size(214, 22);
            this.txtTenThuongHieu.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(428, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Tên thương hiệu";
            // 
            // txtMaThuongHieu
            // 
            this.txtMaThuongHieu.Location = new System.Drawing.Point(172, 26);
            this.txtMaThuongHieu.Name = "txtMaThuongHieu";
            this.txtMaThuongHieu.Size = new System.Drawing.Size(214, 22);
            this.txtMaThuongHieu.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(26, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mã thương hiệu";
            // 
            // btnTimKiemTH
            // 
            this.btnTimKiemTH.Image = global::View.Properties.Resources.search6;
            this.btnTimKiemTH.Location = new System.Drawing.Point(1170, 26);
            this.btnTimKiemTH.Name = "btnTimKiemTH";
            this.btnTimKiemTH.Size = new System.Drawing.Size(48, 36);
            this.btnTimKiemTH.TabIndex = 49;
            this.btnTimKiemTH.UseVisualStyleBackColor = true;
            this.btnTimKiemTH.Click += new System.EventHandler(this.btnTimKiemTH_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLamMoiTH);
            this.groupBox1.Controls.Add(this.txtMoTaTH);
            this.groupBox1.Controls.Add(this.btnXoaTH);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnSuaTH);
            this.groupBox1.Controls.Add(this.btnThemTH);
            this.groupBox1.Controls.Add(this.txtMaThuongHieu);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTenThuongHieu);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(22, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1244, 271);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin thương hiệu";
            // 
            // btnLamMoiTH
            // 
            this.btnLamMoiTH.BackColor = System.Drawing.Color.Transparent;
            this.btnLamMoiTH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoiTH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnLamMoiTH.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLamMoiTH.Image = global::View.Properties.Resources._48px_Crystal_Clear_action_reload;
            this.btnLamMoiTH.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLamMoiTH.Location = new System.Drawing.Point(1012, 182);
            this.btnLamMoiTH.Name = "btnLamMoiTH";
            this.btnLamMoiTH.Size = new System.Drawing.Size(206, 46);
            this.btnLamMoiTH.TabIndex = 61;
            this.btnLamMoiTH.Text = "Làm mới";
            this.btnLamMoiTH.UseVisualStyleBackColor = false;
            this.btnLamMoiTH.Click += new System.EventHandler(this.btnLamMoiTH_Click);
            // 
            // btnXoaTH
            // 
            this.btnXoaTH.BackColor = System.Drawing.Color.Transparent;
            this.btnXoaTH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaTH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnXoaTH.ForeColor = System.Drawing.Color.Black;
            this.btnXoaTH.Image = global::View.Properties.Resources._1439854729_DeleteRed;
            this.btnXoaTH.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXoaTH.Location = new System.Drawing.Point(705, 182);
            this.btnXoaTH.Margin = new System.Windows.Forms.Padding(4);
            this.btnXoaTH.Name = "btnXoaTH";
            this.btnXoaTH.Size = new System.Drawing.Size(206, 46);
            this.btnXoaTH.TabIndex = 60;
            this.btnXoaTH.Text = "XÓA";
            this.btnXoaTH.UseVisualStyleBackColor = false;
            this.btnXoaTH.Click += new System.EventHandler(this.btnXoaTH_Click);
            // 
            // btnSuaTH
            // 
            this.btnSuaTH.BackColor = System.Drawing.Color.Transparent;
            this.btnSuaTH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuaTH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnSuaTH.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSuaTH.Image = global::View.Properties.Resources._48px_Crystal_Clear_app_package_settings;
            this.btnSuaTH.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSuaTH.Location = new System.Drawing.Point(432, 182);
            this.btnSuaTH.Name = "btnSuaTH";
            this.btnSuaTH.Size = new System.Drawing.Size(206, 46);
            this.btnSuaTH.TabIndex = 59;
            this.btnSuaTH.Text = "SỬA";
            this.btnSuaTH.UseVisualStyleBackColor = false;
            this.btnSuaTH.Click += new System.EventHandler(this.btnSuaTH_Click);
            // 
            // btnThemTH
            // 
            this.btnThemTH.BackColor = System.Drawing.Color.Transparent;
            this.btnThemTH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemTH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemTH.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnThemTH.Image = global::View.Properties.Resources._48px_Crystal_Clear_action_db_add;
            this.btnThemTH.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThemTH.Location = new System.Drawing.Point(172, 182);
            this.btnThemTH.Margin = new System.Windows.Forms.Padding(4);
            this.btnThemTH.Name = "btnThemTH";
            this.btnThemTH.Size = new System.Drawing.Size(206, 46);
            this.btnThemTH.TabIndex = 58;
            this.btnThemTH.Text = "THÊM";
            this.btnThemTH.UseVisualStyleBackColor = false;
            this.btnThemTH.Click += new System.EventHandler(this.btnThemTH_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnTimKiemTH);
            this.groupBox2.Controls.Add(this.txtTimKiemTH);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(22, 307);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1244, 80);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tìm kiếm";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvThuongHieu);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox3.Location = new System.Drawing.Point(28, 413);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1238, 330);
            this.groupBox3.TabIndex = 60;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Chi tiết loại sản  phẩm";
            // 
            // dgvThuongHieu
            // 
            this.dgvThuongHieu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThuongHieu.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvThuongHieu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvThuongHieu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThuongHieu.Location = new System.Drawing.Point(12, 30);
            this.dgvThuongHieu.Name = "dgvThuongHieu";
            this.dgvThuongHieu.RowHeadersWidth = 51;
            this.dgvThuongHieu.RowTemplate.Height = 24;
            this.dgvThuongHieu.Size = new System.Drawing.Size(1214, 270);
            this.dgvThuongHieu.TabIndex = 1;
            this.dgvThuongHieu.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThuongHieu_CellClick);
            this.dgvThuongHieu.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThuongHieu_CellClick);
            // 
            // ThuongHieu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(1310, 776);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Name = "ThuongHieu";
            this.Text = "ThuongHieu";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuongHieu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnTimKiemTH;
        private System.Windows.Forms.TextBox txtTimKiemTH;
        private System.Windows.Forms.TextBox txtMoTaTH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTenThuongHieu;
        private System.Windows.Forms.TextBox txtMaThuongHieu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnLamMoiTH;
        private System.Windows.Forms.Button btnXoaTH;
        private System.Windows.Forms.Button btnSuaTH;
        private System.Windows.Forms.Button btnThemTH;
        private System.Windows.Forms.DataGridView dgvThuongHieu;
    }
}