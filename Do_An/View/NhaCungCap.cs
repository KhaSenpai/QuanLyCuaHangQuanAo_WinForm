using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    // Form giao diện người dùng để quản lý nhà cung cấp.
    public partial class NhaCungCap : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly NhaCungCapBLL _nhaCungCapBLL;

        // Hàm khởi tạo form, khởi tạo BLL và cấu hình giao diện.
        public NhaCungCap(TaiKhoanDTO taiKhoan = null)
        {
            _nhaCungCapBLL = new NhaCungCapBLL();
            _taiKhoan = taiKhoan; // Lưu TaiKhoanDTO
            InitializeComponent();
            CaiDatDataGridView();
            TaiDanhSachNhaCungCap();
            XoaCacTruong();
            ThietLapMaNhaCungCapMoi();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemNCC.Visible = true;
                btnSuaNCC.Visible = true;
                btnXoaNCC.Visible = true;
                btnLamMoiNCC.Visible = true;
                btnTimKiemNCC.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaNCC.Visible = false;
                    break;

                case "User":
                    btnThemNCC.Visible = false;
                    btnSuaNCC.Visible = false;
                    btnXoaNCC.Visible = false;
                    btnLamMoiNCC.Visible = false;
                    btnTimKiemNCC.Visible = false;
                    break;

                default:
                    btnThemNCC.Visible = false;
                    btnSuaNCC.Visible = false;
                    btnXoaNCC.Visible = false;
                    btnLamMoiNCC.Visible = false;
                    btnTimKiemNCC.Visible = false;
                    break;
            }
        }
        // Cấu hình DataGridView để hiển thị danh sách nhà cung cấp.
        private void CaiDatDataGridView()
        {
            dgvNhaCungCap.AutoGenerateColumns = false;
            dgvNhaCungCap.Columns.Clear();
            dgvNhaCungCap.Columns.Add("MaNCC", "Mã Nhà Cung Cấp");
            dgvNhaCungCap.Columns.Add("TenNCC", "Tên Nhà Cung Cấp");
            dgvNhaCungCap.Columns.Add("DiaChi", "Địa Chỉ");
            dgvNhaCungCap.Columns.Add("DienThoai", "Số Điện Thoại");
            dgvNhaCungCap.Columns.Add("Email", "Email");
            dgvNhaCungCap.Columns["MaNCC"].DataPropertyName = "MaNCC";
            dgvNhaCungCap.Columns["TenNCC"].DataPropertyName = "TenNCC";
            dgvNhaCungCap.Columns["DiaChi"].DataPropertyName = "DiaChi";
            dgvNhaCungCap.Columns["DienThoai"].DataPropertyName = "DienThoai";
            dgvNhaCungCap.Columns["Email"].DataPropertyName = "Email";
        }

        // Tải danh sách nhà cung cấp từ BLL và hiển thị lên DataGridView.
        private void TaiDanhSachNhaCungCap()
        {
            try
            {
                var danhSach = _nhaCungCapBLL.LayTatCaNhaCungCap();
                if (danhSach == null)
                {
                    throw new InvalidOperationException("Danh sách nhà cung cấp trả về là null.");
                }
                dgvNhaCungCap.DataSource = null;
                dgvNhaCungCap.DataSource = danhSach;
                lblTongNhaCungCap.Text = $"Tổng nhà cung cấp: {danhSach.Count}"; // Hiển thị tổng số nhà cung cấp
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sinh mã nhà cung cấp mới và vô hiệu hóa trường nhập mã.
        private void ThietLapMaNhaCungCapMoi()
        {
            try
            {
                txtMaNCC.Text = _nhaCungCapBLL.TaoMaNhaCungCap();
                txtMaNCC.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa trắng các trường nhập liệu.
        private void XoaCacTruong()
        {
            txtMaNCC.Text = string.Empty;
            txtTenNCC.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            txtDienThoai.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMaNCC.ReadOnly = false; // Mở khóa để nhập mã mới
            ThietLapMaNhaCungCapMoi(); // Sinh mã mới sau khi xóa
        }

        // Xử lý sự kiện nhấn nút Thêm.
        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            try
            {
                var nhaCungCap = TaoNhaCungCapTuInput();
                _nhaCungCapBLL.ThemNhaCungCap(nhaCungCap);
                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachNhaCungCap();
                XoaCacTruong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Sửa.
        private void btnSuaNCC_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa nhà cung cấp này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var nhaCungCap = TaoNhaCungCapTuInput();
                _nhaCungCapBLL.CapNhatNhaCungCap(nhaCungCap);
                MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachNhaCungCap();
                XoaCacTruong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Xóa.
        private void btnXoaNCC_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                _nhaCungCapBLL.XoaNhaCungCap(txtMaNCC.Text.Trim());
                MessageBox.Show("Xóa nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachNhaCungCap();
                XoaCacTruong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Làm mới.
        private void btnLamMoiNCC_Click(object sender, EventArgs e)
        {
            try
            {
                XoaCacTruong();
                txtTimKiemNCC.Text = string.Empty;
                TaiDanhSachNhaCungCap();
                MessageBox.Show("Đã làm mới danh sách nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Tìm kiếm.
        private void btnTimKiemNCC_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimKiemNCC.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var danhSach = _nhaCungCapBLL.TimKiemNhaCungCap(txtTimKiemNCC.Text.Trim());
                dgvNhaCungCap.DataSource = danhSach;
                lblTongNhaCungCap.Text = $"Tổng nhà cung cấp: {danhSach.Count}"; // Hiển thị tổng số nhà cung cấp sau tìm kiếm
                MessageBox.Show(danhSach.Count == 0
                    ? "Không tìm thấy nhà cung cấp nào!"
                    : $"Tìm thấy {danhSach.Count} nhà cung cấp!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện chọn một dòng trong DataGridView.
        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvNhaCungCap.Rows[e.RowIndex];
                    txtMaNCC.Text = row.Cells["MaNCC"].Value?.ToString() ?? string.Empty;
                    txtTenNCC.Text = row.Cells["TenNCC"].Value?.ToString() ?? string.Empty;
                    txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? string.Empty;
                    txtDienThoai.Text = row.Cells["DienThoai"].Value?.ToString() ?? string.Empty;
                    txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
                    txtMaNCC.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tạo đối tượng NhaCungCapDTO từ dữ liệu nhập vào.
        private NhaCungCapDTO TaoNhaCungCapTuInput()
        {
            return new NhaCungCapDTO
            {
                MaNCC = txtMaNCC.Text.Trim(),
                TenNCC = txtTenNCC.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                DienThoai = txtDienThoai.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim()
            };
        }
    }
}