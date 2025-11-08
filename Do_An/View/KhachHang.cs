using BLL;
using DTO;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace View
{
    // Form giao diện người dùng để quản lý khách hàng.
    public partial class KhachHang : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly KhachHangBLL _khachHangBLL;

        // Hàm khởi tạo form.
        public KhachHang(TaiKhoanDTO taiKhoan = null)
        {
            _khachHangBLL = new KhachHangBLL();
            _taiKhoan = taiKhoan; // Lưu TaiKhoanDTO
            InitializeComponent();
            KhoiTaoDieuKhien();
            TaiDanhSachKhachHang();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemKH.Visible = true;
                btnSuaKH.Visible = true;
                btnXoaKH.Visible = true;
                btnLuuKH.Visible = true;
                btnLamMoiKH.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaKH.Visible = false;
                    break;

                case "User":
                    btnXoaKH.Visible = false;
                    break;

                default:
                    btnThemKH.Visible = false;
                    btnSuaKH.Visible = false;
                    btnXoaKH.Visible = false;
                    btnLuuKH.Visible = false;
                    btnLamMoiKH.Visible = true;
                    HienThiCanhBao("Quyền truy cập không hợp lệ!");
                    break;
            }
        }

        // Khởi tạo các điều khiển trên form.
        private void KhoiTaoDieuKhien()
        {
            // Cấu hình DateTimePicker cho ngày sinh
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.Checked = false;

            // Thêm các giá trị cho ComboBox giới tính
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGioiTinh.SelectedIndex = -1;

            // Giới hạn độ dài các TextBox
            txtMaKhachHang.MaxLength = 20;
            txtMaKhachHang.ReadOnly = true; // Không cho phép chỉnh sửa mã khách hàng
            txtHoTen.MaxLength = 50;
            txtDienThoai.MaxLength = 15;
            txtDiaChi.MaxLength = 100;
            txtEmail.MaxLength = 50;

            // Hiển thị mã khách hàng mới
            HienThiMaKhachHangMoi();
        }

        // Tải danh sách khách hàng vào DataGridView.
        private void TaiDanhSachKhachHang()
        {
            try
            {
                var danhSachKhachHang = _khachHangBLL.LayTatCa();
                CauHinhDataGridView(); // Cấu hình cột DataGridView
                dgvKhachHang.DataSource = null; // Xóa dữ liệu cũ
                dgvKhachHang.DataSource = danhSachKhachHang; // Gán dữ liệu mới
                lblTongKhachHang.Text = $"Tổng khách hàng: {danhSachKhachHang.Count}"; // Cập nhật tổng số
                dgvKhachHang.Refresh(); // Làm mới DataGridView
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi tải danh sách khách hàng: {ex.Message}");
            }
        }

        // Cấu hình các cột của DataGridView.
        private void CauHinhDataGridView()
        {
            dgvKhachHang.AutoGenerateColumns = false; // Tắt tự động tạo cột
            if (dgvKhachHang.Columns.Count == 0)
            {
                // Thêm các cột thủ công
                dgvKhachHang.Columns.AddRange(
                    new DataGridViewTextBoxColumn { Name = "MaKhachHang", HeaderText = "Mã Khách Hàng", DataPropertyName = "MaKhachHang" },
                    new DataGridViewTextBoxColumn { Name = "TenKhachHang", HeaderText = "Tên Khách Hàng", DataPropertyName = "TenKhachHang" },
                    new DataGridViewTextBoxColumn { Name = "NgaySinh", HeaderText = "Ngày Sinh", DataPropertyName = "NgaySinh" },
                    new DataGridViewTextBoxColumn { Name = "SoDienThoai", HeaderText = "Số Điện Thoại", DataPropertyName = "SoDienThoai" },
                    new DataGridViewTextBoxColumn { Name = "DiaChi", HeaderText = "Địa Chỉ", DataPropertyName = "DiaChi" },
                    new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", DataPropertyName = "Email" },
                    new DataGridViewTextBoxColumn { Name = "GioiTinh", HeaderText = "Giới Tính", DataPropertyName = "GioiTinh" }
                );
            }
        }

        // Hiển thị mã khách hàng mới trong TextBox.
        private void HienThiMaKhachHangMoi()
        {
            try
            {
                txtMaKhachHang.Text = _khachHangBLL.SinhMaKhachHang();
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi sinh mã khách hàng: {ex.Message}");
            }
        }

        // Xử lý sự kiện nút Tìm kiếm.
        private void btnTimKiemKH_Click(object sender, EventArgs e)
        {
            try
            {
                var tuKhoa = txtTimKiemKH.Text?.Trim();
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    HienThiCanhBao("Vui lòng nhập từ khóa để tìm kiếm!");
                    return;
                }

                var danhSachKhachHang = _khachHangBLL.TimKiem(tuKhoa);
                dgvKhachHang.DataSource = danhSachKhachHang;
                lblTongKhachHang.Text = $"Tổng khách hàng: {danhSachKhachHang.Count}";
                if (danhSachKhachHang.Count == 0)
                {
                    HienThiThongBao("Không tìm thấy khách hàng nào!");
                }
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi tìm kiếm khách hàng: {ex.Message}");
            }
        }

        // Xử lý sự kiện nút Thêm khách hàng.
        private void btnThemKH_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraDuLieuNhap())
                {
                    return;
                }

                var maKhachHang = _khachHangBLL.SinhMaKhachHang();
                var khachHang = TaoKhachHangDTO(maKhachHang);

                _khachHangBLL.Them(khachHang);
                HienThiThongBao($"Thêm khách hàng thành công! Mã khách hàng: {maKhachHang}");
                TaiDanhSachKhachHang();
                XoaDuLieuNhap();
                HienThiMaKhachHangMoi();
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi thêm khách hàng: {ex.Message}");
            }
        }

        // Xử lý sự kiện nút Sửa khách hàng.
        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            try
            {
                var maKhachHang = txtMaKhachHang.Text?.Trim();
                if (string.IsNullOrEmpty(maKhachHang))
                {
                    HienThiCanhBao("Vui lòng chọn khách hàng để sửa!");
                    return;
                }

                if (!KiemTraDuLieuNhap())
                {
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn sửa khách hàng này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var khachHang = TaoKhachHangDTO(maKhachHang);
                _khachHangBLL.CapNhat(khachHang);
                HienThiThongBao("Cập nhật khách hàng thành công!");
                TaiDanhSachKhachHang();
                XoaDuLieuNhap();
                HienThiMaKhachHangMoi();
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi cập nhật khách hàng: {ex.Message}");
            }
        }

        // Xử lý sự kiện nút Xóa khách hàng.
        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            try
            {
                var maKhachHang = txtMaKhachHang.Text?.Trim();
                if (string.IsNullOrEmpty(maKhachHang))
                {
                    HienThiCanhBao("Vui lòng chọn khách hàng để xóa!");
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                _khachHangBLL.Xoa(maKhachHang);
                HienThiThongBao("Xóa khách hàng thành công!");
                TaiDanhSachKhachHang();
                XoaDuLieuNhap();
                HienThiMaKhachHangMoi();
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi xóa khách hàng: {ex.Message}");
            }
        }

        // Xử lý sự kiện nút Làm mới.
        private void btnLamMoiKH_Click(object sender, EventArgs e)
        {
            try
            {
                XoaDuLieuNhap();
                HienThiMaKhachHangMoi();
                TaiDanhSachKhachHang();
                txtTimKiemKH.Text = "";
                HienThiThongBao("Đã làm mới danh sách khách hàng!");
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi làm mới: {ex.Message}");
            }
        }

        // Xử lý sự kiện nút Lưu (Thêm hoặc Sửa).
        private void btnLuuKH_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraDuLieuNhap())
                {
                    return;
                }

                var laKhachHangMoi = !txtMaKhachHang.ReadOnly;
                var maKhachHang = laKhachHangMoi ? _khachHangBLL.SinhMaKhachHang() : txtMaKhachHang.Text?.Trim();
                var khachHang = TaoKhachHangDTO(maKhachHang);

                if (laKhachHangMoi)
                {
                    _khachHangBLL.Them(khachHang);
                    HienThiThongBao($"Thêm khách hàng thành công! Mã khách hàng: {maKhachHang}");
                }
                else
                {
                    if (string.IsNullOrEmpty(maKhachHang))
                    {
                        HienThiCanhBao("Vui lòng chọn khách hàng để sửa!");
                        return;
                    }
                    _khachHangBLL.CapNhat(khachHang);
                    HienThiThongBao("Cập nhật khách hàng thành công!");
                }

                TaiDanhSachKhachHang();
                XoaDuLieuNhap();
                txtMaKhachHang.ReadOnly = true;
                HienThiMaKhachHangMoi();
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi lưu khách hàng: {ex.Message}");
            }
        }

        // Xử lý sự kiện khi chọn một dòng trong DataGridView.
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                var row = dgvKhachHang.Rows[e.RowIndex];
                txtMaKhachHang.Text = row.Cells["MaKhachHang"].Value?.ToString() ?? "";
                txtHoTen.Text = row.Cells["TenKhachHang"].Value?.ToString() ?? "";
                if (row.Cells["NgaySinh"].Value != null && DateTime.TryParse(row.Cells["NgaySinh"].Value.ToString(), out var ngaySinh))
                {
                    dtpNgaySinh.Value = ngaySinh;
                    dtpNgaySinh.Checked = true;
                }
                else
                {
                    dtpNgaySinh.Checked = false;
                    dtpNgaySinh.Value = DateTime.Now;
                }
                cboGioiTinh.Text = row.Cells["GioiTinh"].Value?.ToString() ?? "";
                txtDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString() ?? "";
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                txtMaKhachHang.ReadOnly = true; // Khóa mã khách hàng khi sửa
            }
            catch (Exception ex)
            {
                HienThiLoi($"Lỗi khi chọn khách hàng: {ex.Message}");
            }
        }

        // Xóa dữ liệu trong các trường nhập liệu.
        private void XoaDuLieuNhap()
        {
            txtMaKhachHang.Text = "";
            txtHoTen.Text = "";
            dtpNgaySinh.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            txtDienThoai.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            cboGioiTinh.SelectedIndex = -1;
            txtMaKhachHang.ReadOnly = false; // Mở khóa để nhập mới
        }

        // Tạo đối tượng KhachHangDTO từ dữ liệu nhập trên form.
        private KhachHangDTO TaoKhachHangDTO(string maKhachHang)
        {
            return new KhachHangDTO
            {
                MaKhachHang = maKhachHang,
                TenKhachHang = txtHoTen.Text?.Trim(),
                NgaySinh = dtpNgaySinh.Checked ? (DateTime?)dtpNgaySinh.Value : null,
                SoDienThoai = txtDienThoai.Text?.Trim(),
                DiaChi = txtDiaChi.Text?.Trim(),
                Email = string.IsNullOrEmpty(txtEmail.Text?.Trim()) ? null : txtEmail.Text.Trim(),
                GioiTinh = cboGioiTinh.SelectedIndex >= 0 ? cboGioiTinh.SelectedItem?.ToString() : null
            };
        }

        // Kiểm tra dữ liệu đầu vào trên form.
        // Trả về true nếu hợp lệ, false nếu không.
        private bool KiemTraDuLieuNhap()
        {
            if (string.IsNullOrEmpty(txtHoTen.Text?.Trim()))
            {
                HienThiCanhBao("Vui lòng nhập tên khách hàng!");
                return false;
            }
            if (txtHoTen.Text.Trim().Length > 50)
            {
                HienThiCanhBao("Tên khách hàng không được vượt quá 50 ký tự!");
                return false;
            }
            if (!Regex.IsMatch(txtHoTen.Text.Trim(), @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                HienThiCanhBao("Tên khách hàng chỉ được chứa chữ cái, khoảng trắng và dấu tiếng Việt!");
                return false;
            }

            if (string.IsNullOrEmpty(txtDienThoai.Text?.Trim()))
            {
                HienThiCanhBao("Vui lòng nhập số điện thoại!");
                return false;
            }
            if (!Regex.IsMatch(txtDienThoai.Text.Trim(), @"^0\d{9,14}$"))
            {
                HienThiCanhBao("Số điện thoại phải bắt đầu bằng 0 và có độ dài từ 10 đến 15 chữ số!");
                return false;
            }

            if (string.IsNullOrEmpty(txtDiaChi.Text?.Trim()))
            {
                HienThiCanhBao("Vui lòng nhập địa chỉ!");
                return false;
            }
            if (txtDiaChi.Text.Trim().Length > 100)
            {
                HienThiCanhBao("Địa chỉ không được vượt quá 100 ký tự!");
                return false;
            }

            if (dtpNgaySinh.Checked && dtpNgaySinh.Value > DateTime.Now)
            {
                HienThiCanhBao("Ngày sinh không được lớn hơn ngày hiện tại!");
                return false;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text?.Trim()) && !Regex.IsMatch(txtEmail.Text.Trim(), @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                HienThiCanhBao("Email không đúng định dạng!");
                return false;
            }

            return true;
        }

        // Hiển thị thông báo lỗi.
        private void HienThiLoi(string thongBao)
        {
            MessageBox.Show(thongBao, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hiển thị thông báo cảnh báo.
        private void HienThiCanhBao(string thongBao)
        {
            MessageBox.Show(thongBao, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Hiển thị thông báo thông tin.
        private void HienThiThongBao(string thongBao)
        {
            MessageBox.Show(thongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}