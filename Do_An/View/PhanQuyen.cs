using BLL;
using DTO;
using System;
using System.Windows.Forms;

namespace View
{
    public partial class PhanQuyen : Form
    {
        private readonly PhanQuyenBLL _phanQuyenBLL = new PhanQuyenBLL();

        public PhanQuyen()
        {
            InitializeComponent();
            LoadTaiKhoan();
            LoadQuyen();
            cbTenTaiKhoan.SelectedIndexChanged += cbTenTaiKhoan_SelectedIndexChanged;
            btnLuuPQ.Click += btnLuuPQ_Click;
            clbPhanQuyen.ItemCheck += clbPhanQuyen_ItemCheck;
        }

        private void clbPhanQuyen_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Chỉ cho phép chọn một quyền duy nhất
            for (int i = 0; i < clbPhanQuyen.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    clbPhanQuyen.SetItemChecked(i, false);
                }
            }
        }

        private void LoadTaiKhoan()
        {
            try
            {
                var taiKhoanList = _phanQuyenBLL.GetAllTaiKhoan();
                if (taiKhoanList == null || taiKhoanList.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy tài khoản nào trong cơ sở dữ liệu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbTenTaiKhoan.DataSource = null;
                    return;
                }

                cbTenTaiKhoan.DataSource = null; // Xóa dữ liệu cũ
                cbTenTaiKhoan.DataSource = taiKhoanList;
                cbTenTaiKhoan.DisplayMember = "TenDangNhap";
                cbTenTaiKhoan.ValueMember = "TenDangNhap";
                cbTenTaiKhoan.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadQuyen()
        {
            try
            {
                clbPhanQuyen.Items.Clear();
                var roles = _phanQuyenBLL.GetValidRoles();
                if (roles == null || roles.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy danh sách quyền hợp lệ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (string role in roles)
                {
                    clbPhanQuyen.Items.Add(role);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuuPQ_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbTenTaiKhoan.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản để phân quyền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string tenDangNhap = cbTenTaiKhoan.SelectedValue?.ToString();
                if (string.IsNullOrWhiteSpace(tenDangNhap))
                {
                    MessageBox.Show("Tài khoản được chọn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (clbPhanQuyen.CheckedItems.Count > 1)
                {
                    MessageBox.Show("Chỉ được chọn 1 quyền cho tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string selectedRole = clbPhanQuyen.CheckedItems.Count == 1 ? clbPhanQuyen.CheckedItems[0].ToString() : null;
                if (string.IsNullOrEmpty(selectedRole))
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 quyền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật quyền
                bool success = _phanQuyenBLL.UpdateQuyenTruyCap(tenDangNhap, selectedRole);
                if (success)
                {
                    MessageBox.Show("Phân quyền thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Lưu tên đăng nhập trước khi làm mới
                    string selectedTenDangNhap = tenDangNhap;
                    LoadTaiKhoan();
                    // Chọn lại tài khoản vừa cập nhật
                    cbTenTaiKhoan.SelectedValue = selectedTenDangNhap;
                    if (cbTenTaiKhoan.SelectedValue == null)
                    {
                        MessageBox.Show($"Tài khoản '{selectedTenDangNhap}' không tồn tại sau khi cập nhật. Vui lòng làm mới trang hoặc kiểm tra cơ sở dữ liệu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadTaiKhoan(); // Thử làm mới lại để đồng bộ
                    }
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật quyền. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu phân quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbTenTaiKhoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbTenTaiKhoan.SelectedIndex == -1 || cbTenTaiKhoan.SelectedValue == null)
                {
                    // Xóa trạng thái quyền nếu không có tài khoản được chọn
                    for (int i = 0; i < clbPhanQuyen.Items.Count; i++)
                    {
                        clbPhanQuyen.SetItemChecked(i, false);
                    }
                    return;
                }

                string tenDangNhap = cbTenTaiKhoan.SelectedValue.ToString();
                if (string.IsNullOrWhiteSpace(tenDangNhap))
                {
                    return;
                }

                var taiKhoan = _phanQuyenBLL.GetTaiKhoanByTenDangNhap(tenDangNhap);
                if (taiKhoan == null)
                {                 
                    LoadTaiKhoan(); // Làm mới danh sách để đồng bộ
                    cbTenTaiKhoan.SelectedIndex = -1; // Đặt lại về không chọn
                    return;
                }

                // Cập nhật trạng thái quyền
                for (int i = 0; i < clbPhanQuyen.Items.Count; i++)
                {
                    clbPhanQuyen.SetItemChecked(i, false);
                }

                if (!string.IsNullOrEmpty(taiKhoan.QuyenTruyCap))
                {
                    int index = clbPhanQuyen.Items.IndexOf(taiKhoan.QuyenTruyCap);
                    if (index != -1)
                    {
                        clbPhanQuyen.SetItemChecked(index, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}