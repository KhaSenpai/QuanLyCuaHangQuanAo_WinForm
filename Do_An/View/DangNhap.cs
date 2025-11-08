using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class DangNhap : Form
    {
        private TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();

        public DangNhap()
        {
            InitializeComponent();
        }

        private void ckbHienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbHienMK.Checked)
            {
                // Hiện mật khẩu: tắt cả hai chế độ ẩn
                txtMatKhau.UseSystemPasswordChar = false;
                txtMatKhau.PasswordChar = '\0'; // Không sử dụng ký tự ẩn
            }
            else
            {
                // Ẩn mật khẩu: sử dụng ký tự *
                txtMatKhau.UseSystemPasswordChar = false;
                txtMatKhau.PasswordChar = '*';
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                TaiKhoanDTO taiKhoan = taiKhoanBLL.Login(tenDangNhap, matKhau);
                if (taiKhoan != null)
                {
                    TrangChu trangChu = new TrangChu(taiKhoan);
                    trangChu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbtnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Thoát nếu không còn form nào
        }
    }
}