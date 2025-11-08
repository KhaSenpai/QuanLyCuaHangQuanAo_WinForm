using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    // Form giao diện người dùng để quản lý nhân viên.
    public partial class NhanVien : Form
    {
        private readonly NhanVienBLL _nhanVienBLL;
        private readonly TaiKhoanDTO _taiKhoan;

        // Hàm khởi tạo form, khởi tạo BLL và cấu hình giao diện.
        public NhanVien(TaiKhoanDTO taiKhoan = null)
        {
            _nhanVienBLL = new NhanVienBLL();
            _taiKhoan = taiKhoan; 
            InitializeComponent();
            KhoiTaoDieuKhien();
            TaiDanhSachNhanVien();
            TaiDanhSachChucVu();
            XoaCacTruong();
            ThietLapMaNhanVienMoi();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemNV.Visible = true;
                btnSuaNV.Visible = true;
                btnXoaNV.Visible = true;
                btnTimKiemNV.Visible = true;
                BtnLamMoiNV.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaNV.Visible = false;
                    break;

                case "User":
                    btnThemNV.Visible = false;
                    btnSuaNV.Visible = false;
                    btnXoaNV.Visible = false;
                    break;

                default:
                    btnThemNV.Visible = false;
                    btnSuaNV.Visible = false;
                    btnXoaNV.Visible = false;
                    btnTimKiemNV.Visible = false;
                    BtnLamMoiNV.Visible = false;
                    break;
            }
        }

        // Khởi tạo các điều khiển trên form.
        private void KhoiTaoDieuKhien()
        {
            txtMaNhanVien.MaxLength = 20;
            txtHoTen.MaxLength = 50;
            txtDiaChi.MaxLength = 100;
            txtSoDienThoai.MaxLength = 10;
            CaiDatDataGridView();
        }

        // Cấu hình DataGridView để hiển thị danh sách nhân viên.
            private void CaiDatDataGridView()
            {
                dgvNhanVien.AutoGenerateColumns = false;
                dgvNhanVien.Columns.Clear();
                dgvNhanVien.Columns.Add("MaNhanVien", "Mã Nhân Viên");
                dgvNhanVien.Columns.Add("HoTen", "Họ Tên");
                dgvNhanVien.Columns.Add("NgaySinh", "Ngày Sinh");
                dgvNhanVien.Columns.Add("GioiTinh", "Giới Tính");
                dgvNhanVien.Columns.Add("DiaChi", "Địa Chỉ");
                dgvNhanVien.Columns.Add("SoDienThoai", "Số Điện Thoại");
                dgvNhanVien.Columns.Add("TenChucVu", "Chức Vụ");
                dgvNhanVien.Columns["MaNhanVien"].DataPropertyName = "MaNhanVien";
                dgvNhanVien.Columns["HoTen"].DataPropertyName = "HoTen";
                dgvNhanVien.Columns["NgaySinh"].DataPropertyName = "NgaySinh";
                dgvNhanVien.Columns["GioiTinh"].DataPropertyName = "GioiTinh";
                dgvNhanVien.Columns["DiaChi"].DataPropertyName = "DiaChi";
                dgvNhanVien.Columns["SoDienThoai"].DataPropertyName = "SoDienThoai";
                dgvNhanVien.Columns["TenChucVu"].DataPropertyName = "TenChucVu";
            }

        // Tải danh sách nhân viên từ BLL và hiển thị lên DataGridView.
        private void TaiDanhSachNhanVien()
        {
            try
            {
                var danhSach = _nhanVienBLL.LayTatCaNhanVien();
                if (danhSach == null)
                {
                    throw new InvalidOperationException("Danh sách nhân viên trả về là null.");
                }
                dgvNhanVien.DataSource = null;
                dgvNhanVien.DataSource = danhSach;
                lblTongNhanVien.Text = $"Tổng nhân viên: {danhSach.Count}"; // Hiển thị tổng số nhân viên
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tải danh sách chức vụ vào ComboBox.
        private void TaiDanhSachChucVu()
        {
            try
            {
                cboChucVu.DataSource = _nhanVienBLL.LayTatCaChucVu();
                cboChucVu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chức vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sinh mã nhân viên mới và vô hiệu hóa trường nhập mã.
        private void ThietLapMaNhanVienMoi()
        {
            try
            {
                txtMaNhanVien.Text = _nhanVienBLL.TaoMaNhanVien();
                txtMaNhanVien.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa trắng các trường nhập liệu.
        private void XoaCacTruong()
        {
            txtMaNhanVien.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            dtpNgaySinh.Value = DateTime.Now;
            rbNam.Checked = true;
            txtDiaChi.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            cboChucVu.SelectedIndex = -1;
            txtTimKiemNV.Text = string.Empty;
            ThietLapMaNhanVienMoi();
        }

        // Xử lý sự kiện nhấn nút Thêm.
        private void btnThemNV_Click(object sender, EventArgs e)
        {
            try
            {
                var nhanVien = TaoNhanVienTuInput();
                if (_nhanVienBLL.ThemNhanVien(nhanVien))
                {
                    MessageBox.Show($"Thêm nhân viên thành công! Mã: {nhanVien.MaNhanVien}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachNhanVien();
                    XoaCacTruong();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Sửa.
        private void btnSuaNV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa nhân viên này?", "Xác nhận nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var nhanVien = TaoNhanVienTuInput();
                if (_nhanVienBLL.CapNhatNhanVien(nhanVien))
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachNhanVien();
                    XoaCacTruong();
                }
                else
                {
                    MessageBox.Show("Cập nhật nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Xóa.
        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                if (_nhanVienBLL.XoaNhanVien(txtMaNhanVien.Text.Trim()))
                {
                    MessageBox.Show("Xóa nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachNhanVien();
                    XoaCacTruong();
                }
                else
                {
                    MessageBox.Show("Xóa nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Tìm kiếm.
        private void btnTimKiemNV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimKiemNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var danhSach = _nhanVienBLL.TimKiemNhanVien(txtTimKiemNV.Text.Trim());
                dgvNhanVien.DataSource = danhSach;
                lblTongNhanVien.Text = $"Tổng nhân viên: {danhSach.Count}"; // Hiển thị tổng số nhân viên sau tìm kiếm
                MessageBox.Show(danhSach.Count == 0
                    ? "Không tìm thấy nhân viên nào!"
                    : $"Tìm thấy {danhSach.Count} nhân viên!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Làm mới.
        private void btnLamMoiNV_Click(object sender, EventArgs e)
        {
            try
            {
                XoaCacTruong();
                TaiDanhSachNhanVien();
                MessageBox.Show("Đã làm mới danh sách nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện chọn một dòng trong DataGridView.
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvNhanVien.Rows[e.RowIndex];
                    txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value?.ToString() ?? string.Empty;
                    txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? string.Empty;
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                    rbNam.Checked = row.Cells["GioiTinh"].Value.ToString() == "Nam";
                    rbNu.Checked = row.Cells["GioiTinh"].Value.ToString() == "Nữ";
                    txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? string.Empty;
                    txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString() ?? string.Empty;
                    cboChucVu.Text = row.Cells["TenChucVu"].Value?.ToString() ?? string.Empty;
                    txtMaNhanVien.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tạo đối tượng NhanVienDTO từ dữ liệu nhập vào.
        private NhanVienDTO TaoNhanVienTuInput()
        {
            return new NhanVienDTO
            {
                MaNhanVien = txtMaNhanVien.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                GioiTinh = rbNam.Checked ? "Nam" : "Nữ",
                DiaChi = txtDiaChi.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                MaChucVu = _nhanVienBLL.LayMaChucVuTheoTen(cboChucVu.Text)
            };
        }
    }
}