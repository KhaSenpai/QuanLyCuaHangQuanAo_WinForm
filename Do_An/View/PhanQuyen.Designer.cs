namespace View
{
    partial class PhanQuyen
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLuuPQ = new System.Windows.Forms.Button();
            this.clbPhanQuyen = new System.Windows.Forms.CheckedListBox();
            this.cbTenTaiKhoan = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLuuPQ);
            this.groupBox1.Controls.Add(this.clbPhanQuyen);
            this.groupBox1.Controls.Add(this.cbTenTaiKhoan);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(226, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(793, 265);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chọn tài khoản";
            // 
            // btnLuuPQ
            // 
            this.btnLuuPQ.BackColor = System.Drawing.Color.Transparent;
            this.btnLuuPQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuuPQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnLuuPQ.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnLuuPQ.Image = global::View.Properties.Resources.Save_as_icon;
            this.btnLuuPQ.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLuuPQ.Location = new System.Drawing.Point(583, 21);
            this.btnLuuPQ.Name = "btnLuuPQ";
            this.btnLuuPQ.Size = new System.Drawing.Size(169, 61);
            this.btnLuuPQ.TabIndex = 57;
            this.btnLuuPQ.Text = "Lưu";
            this.btnLuuPQ.UseVisualStyleBackColor = false;
            this.btnLuuPQ.Click += new System.EventHandler(this.btnLuuPQ_Click);
            // 
            // clbPhanQuyen
            // 
            this.clbPhanQuyen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.clbPhanQuyen.FormattingEnabled = true;
            this.clbPhanQuyen.Location = new System.Drawing.Point(32, 99);
            this.clbPhanQuyen.Name = "clbPhanQuyen";
            this.clbPhanQuyen.Size = new System.Drawing.Size(720, 136);
            this.clbPhanQuyen.TabIndex = 64;
            // 
            // cbTenTaiKhoan
            // 
            this.cbTenTaiKhoan.FormattingEnabled = true;
            this.cbTenTaiKhoan.Location = new System.Drawing.Point(178, 38);
            this.cbTenTaiKhoan.Name = "cbTenTaiKhoan";
            this.cbTenTaiKhoan.Size = new System.Drawing.Size(209, 24);
            this.cbTenTaiKhoan.TabIndex = 62;
            this.cbTenTaiKhoan.SelectedIndexChanged += new System.EventHandler(this.cbTenTaiKhoan_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(28, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 20);
            this.label2.TabIndex = 57;
            this.label2.Text = "Tên đăng nhập";
            // 
            // PhanQuyen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(1310, 776);
            this.Controls.Add(this.groupBox1);
            this.Name = "PhanQuyen";
            this.Text = "`";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTenTaiKhoan;
        private System.Windows.Forms.CheckedListBox clbPhanQuyen;
        private System.Windows.Forms.Button btnLuuPQ;
    }
}