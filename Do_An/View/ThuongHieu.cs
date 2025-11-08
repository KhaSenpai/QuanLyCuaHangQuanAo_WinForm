using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    // Form giao diện người dùng để quản lý thương hiệu.
    public partial class ThuongHieu : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly ThuongHieuBLL _thuongHieuBLL;

        // Hàm khởi tạo form, khởi tạo BLL và cấu hình giao diện.
        public ThuongHieu(TaiKhoanDTO taiKhoan = null)
        {
            _thuongHieuBLL = new ThuongHieuBLL();
            _taiKhoan = taiKhoan; // Lưu TaiKhoanDTO
            InitializeComponent();
            CaiDatDataGridView();
            TaiDanhSachThuongHieu();
            XoaNoiDung();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemTH.Visible = true;
                btnSuaTH.Visible = true;
                btnXoaTH.Visible = true;
                btnLamMoiTH.Visible = true;
                btnTimKiemTH.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaTH.Visible = false;
                    break;

                case "User":
                    btnThemTH.Visible = false;
                    btnSuaTH.Visible = false;
                    btnXoaTH.Visible = false;
                    break;

                default:
                    btnThemTH.Visible = false;
                    btnSuaTH.Visible = false;
                    btnXoaTH.Visible = false;
                    btnLamMoiTH.Visible = false;
                    btnTimKiemTH.Visible = false;
                    break;
            }
        }
        // Cấu hình DataGridView để hiển thị danh sách thương hiệu.
        private void CaiDatDataGridView()
        {
            dgvThuongHieu.AutoGenerateColumns = false;
            dgvThuongHieu.Columns.Clear();
            dgvThuongHieu.Columns.Add("MaThuongHieu", "Mã Thương Hiệu");
            dgvThuongHieu.Columns.Add("TenThuongHieu", "Tên Thương Hiệu");
            dgvThuongHieu.Columns.Add("MoTa", "Mô Tả");
            dgvThuongHieu.Columns["MaThuongHieu"].DataPropertyName = "MaThuongHieu";
            dgvThuongHieu.Columns["TenThuongHieu"].DataPropertyName = "TenThuongHieu";
            dgvThuongHieu.Columns["MoTa"].DataPropertyName = "MoTa";
        }

        // Tải danh sách thương hiệu từ BLL và hiển thị lên DataGridView.
        private void TaiDanhSachThuongHieu()
        {
            try
            {
                List<ThuongHieuDTO> danhSach = _thuongHieuBLL.LayTatCaThuongHieu();
                dgvThuongHieu.DataSource = null;
                dgvThuongHieu.DataSource = danhSach;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách thương hiệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa nội dung các ô nhập liệu và tạo mã thương hiệu mới.
        private void XoaNoiDung()
        {
            txtMaThuongHieu.Text = _thuongHieuBLL.TaoMaThuongHieu();
            txtMaThuongHieu.ReadOnly = true;
            txtTenThuongHieu.Text = string.Empty;
            txtMoTaTH.Text = string.Empty;
        }

        // Xử lý sự kiện nhấn nút Thêm.
        private void btnThemTH_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenThuongHieu.Text))
                {
                    MessageBox.Show("Tên thương hiệu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var thuongHieu = new ThuongHieuDTO
                {
                    MaThuongHieu = txtMaThuongHieu.Text.Trim(),
                    TenThuongHieu = txtTenThuongHieu.Text.Trim(),
                    MoTa = string.IsNullOrWhiteSpace(txtMoTaTH.Text) ? null : txtMoTaTH.Text.Trim()
                };

                _thuongHieuBLL.ThemThuongHieu(thuongHieu);
                MessageBox.Show("Thêm thương hiệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachThuongHieu();
                XoaNoiDung();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm thương hiệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Sửa.
        private void btnSuaTH_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaThuongHieu.Text))
                {
                    MessageBox.Show("Vui lòng chọn thương hiệu để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtTenThuongHieu.Text))
                {
                    MessageBox.Show("Tên thương hiệu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa thương hiệu này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var thuongHieu = new ThuongHieuDTO
                {
                    MaThuongHieu = txtMaThuongHieu.Text.Trim(),
                    TenThuongHieu = txtTenThuongHieu.Text.Trim(),
                    MoTa = string.IsNullOrWhiteSpace(txtMoTaTH.Text) ? null : txtMoTaTH.Text.Trim()
                };

                _thuongHieuBLL.CapNhatThuongHieu(thuongHieu);
                MessageBox.Show("Cập nhật thương hiệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachThuongHieu();
                XoaNoiDung();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thương hiệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Xóa.
        private void btnXoaTH_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaThuongHieu.Text))
                {
                    MessageBox.Show("Vui lòng chọn thương hiệu để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa thương hiệu này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                _thuongHieuBLL.XoaThuongHieu(txtMaThuongHieu.Text.Trim());
                MessageBox.Show("Xóa thương hiệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachThuongHieu();
                XoaNoiDung();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa thương hiệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Làm mới.
        private void btnLamMoiTH_Click(object sender, EventArgs e)
        {
            try
            {
                XoaNoiDung();
                txtTimKiemTH.Text = string.Empty;
                TaiDanhSachThuongHieu();
                MessageBox.Show("Đã làm mới danh sách thương hiệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Tìm kiếm.
        private void btnTimKiemTH_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimKiemTH.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var danhSach = _thuongHieuBLL.TimKiemThuongHieu(txtTimKiemTH.Text.Trim());
                dgvThuongHieu.DataSource = danhSach;

                MessageBox.Show(danhSach.Count == 0
                    ? "Không tìm thấy thương hiệu nào!"
                    : $"Tìm thấy {danhSach.Count} thương hiệu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm thương hiệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi chọn một dòng trong DataGridView.
        private void dgvThuongHieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvThuongHieu.Rows[e.RowIndex];
                    txtMaThuongHieu.Text = row.Cells["MaThuongHieu"].Value?.ToString() ?? string.Empty;
                    txtTenThuongHieu.Text = row.Cells["TenThuongHieu"].Value?.ToString() ?? string.Empty;
                    txtMoTaTH.Text = row.Cells["MoTa"].Value?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}