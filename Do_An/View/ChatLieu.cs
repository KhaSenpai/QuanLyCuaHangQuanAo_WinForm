using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class ChatLieu : Form
    {
        private readonly ChatLieuBLL _chatLieuBLL;
        private readonly TaiKhoanDTO _taiKhoan;

        // Hàm khởi tạo không tham số (giữ nguyên)
        public ChatLieu(TaiKhoanDTO taiKhoan = null)
        {
            InitializeComponent();
            _chatLieuBLL = new ChatLieuBLL();
            _taiKhoan = taiKhoan;
            CaiDatDataGridView();
            TaiDanhSachChatLieu();
            XoaCacTruong();
            ThietLapMaChatLieuMoi();
            KiemTraQuyenTruyCap();
        }

        // Kiểm tra quyền truy cập và ẩn các nút nếu là User
        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                // Mặc định hiển thị tất cả nếu không có thông tin tài khoản
                btnThemCL.Visible = true;
                btnSuaCL.Visible = true;
                btnXoaCL.Visible = true;
                txtMaChatLieu.ReadOnly = false;
                txtTenChatLieu.ReadOnly = false;
                txtMoTaChatLieu.ReadOnly = false;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    // Admin có toàn quyền, hiển thị tất cả nút
                    break;

                case "Manager":
                    // Manager có thể thêm và sửa, nhưng không thể xóa
                    btnThemCL.Visible = true;
                    btnSuaCL.Visible = true;
                    btnXoaCL.Visible = false;
                    txtMaChatLieu.ReadOnly = true; // Mã chất liệu luôn chỉ đọc
                    txtTenChatLieu.ReadOnly = false;
                    txtMoTaChatLieu.ReadOnly = false;
                    break;

                case "User":
                    // User chỉ có thể xem, ẩn tất cả nút và đặt các trường thành chỉ đọc
                    btnThemCL.Visible = false;
                    btnSuaCL.Visible = false;
                    btnXoaCL.Visible = false;
                    txtMaChatLieu.ReadOnly = true;
                    txtTenChatLieu.ReadOnly = true;
                    txtMoTaChatLieu.ReadOnly = true;
                    break;

                default:
                    // Quyền không xác định, ẩn tất cả nút để đảm bảo an toàn
                    btnThemCL.Visible = false;
                    btnSuaCL.Visible = false;
                    btnXoaCL.Visible = false;
                    txtMaChatLieu.ReadOnly = true;
                    txtTenChatLieu.ReadOnly = true;
                    txtMoTaChatLieu.ReadOnly = true;
                    break;
            }
        }

        private void CaiDatDataGridView()
        {
            dgvChatLieu.AutoGenerateColumns = false;
            dgvChatLieu.Columns.Clear();
            dgvChatLieu.Columns.Add("MaChatLieu", "Mã Chất Liệu");
            dgvChatLieu.Columns.Add("TenChatLieu", "Tên Chất Liệu");
            dgvChatLieu.Columns.Add("MoTa", "Mô Tả");
            dgvChatLieu.Columns["MaChatLieu"].DataPropertyName = "MaChatLieu";
            dgvChatLieu.Columns["TenChatLieu"].DataPropertyName = "TenChatLieu";
            dgvChatLieu.Columns["MoTa"].DataPropertyName = "MoTa";
        }

        private void TaiDanhSachChatLieu()
        {
            try
            {
                var danhSach = _chatLieuBLL.LayTatCaChatLieu();
                if (danhSach == null)
                {
                    throw new InvalidOperationException("Danh sách chất liệu trả về là null.");
                }
                dgvChatLieu.DataSource = null;
                dgvChatLieu.DataSource = danhSach;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chất liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ThietLapMaChatLieuMoi()
        {
            try
            {
                txtMaChatLieu.Text = _chatLieuBLL.TaoMaChatLieu();
                txtMaChatLieu.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã chất liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XoaCacTruong()
        {
            txtMaChatLieu.Text = string.Empty;
            txtTenChatLieu.Text = string.Empty;
            txtMoTaChatLieu.Text = string.Empty;
            txtTimKiemCL.Text = string.Empty;
            ThietLapMaChatLieuMoi();
        }

        private void btnThemCL_Click(object sender, EventArgs e)
        {
            try
            {
                var chatLieu = TaoChatLieuTuInput();
                _chatLieuBLL.ThemChatLieu(chatLieu);
                MessageBox.Show("Thêm chất liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachChatLieu();
                XoaCacTruong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chất liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaCL_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaChatLieu.Text))
                {
                    MessageBox.Show("Vui lòng chọn chất liệu để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa chất liệu này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var chatLieu = TaoChatLieuTuInput();
                _chatLieuBLL.CapNhatChatLieu(chatLieu);
                MessageBox.Show("Cập nhật chất liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachChatLieu();
                XoaCacTruong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chất liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaCL_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaChatLieu.Text))
                {
                    MessageBox.Show("Vui lòng chọn chất liệu để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa chất liệu này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                _chatLieuBLL.XoaChatLieu(txtMaChatLieu.Text.Trim());
                MessageBox.Show("Xóa chất liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachChatLieu();
                XoaCacTruong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chất liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoiCL_Click(object sender, EventArgs e)
        {
            try
            {
                XoaCacTruong();
                TaiDanhSachChatLieu();
                MessageBox.Show("Đã làm mới danh sách chất liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiemCL_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimKiemCL.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var danhSach = _chatLieuBLL.TimKiemChatLieu(txtTimKiemCL.Text.Trim());
                dgvChatLieu.DataSource = danhSach;
                MessageBox.Show(danhSach.Count == 0
                    ? "Không tìm thấy chất liệu nào!"
                    : $"Tìm thấy {danhSach.Count} chất liệu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm chất liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvChatLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvChatLieu.Rows[e.RowIndex];
                    txtMaChatLieu.Text = row.Cells["MaChatLieu"].Value?.ToString() ?? string.Empty;
                    txtTenChatLieu.Text = row.Cells["TenChatLieu"].Value?.ToString() ?? string.Empty;
                    txtMoTaChatLieu.Text = row.Cells["MoTa"].Value?.ToString() ?? string.Empty;
                    txtMaChatLieu.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ChatLieuDTO TaoChatLieuTuInput()
        {
            return new ChatLieuDTO
            {
                MaChatLieu = txtMaChatLieu.Text.Trim(),
                TenChatLieu = txtTenChatLieu.Text.Trim(),
                MoTa = string.IsNullOrWhiteSpace(txtMoTaChatLieu.Text) ? null : txtMoTaChatLieu.Text.Trim()
            };
        }
    }
}