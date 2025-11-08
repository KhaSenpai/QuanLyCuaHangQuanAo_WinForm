using BLL;
using DTO;
using System;
using System.Windows.Forms;

namespace View
{
    public partial class TaiKhoan : Form
    {
        private readonly TaiKhoanBLL _taiKhoanBLL;
        private readonly NhanVienBLL _nhanVienBLL;

        public TaiKhoan()
        {
            _taiKhoanBLL = new TaiKhoanBLL(); // Khởi tạo đối tượng BLL để quản lý tài khoản
            _nhanVienBLL = new NhanVienBLL(); // Khởi tạo đối tượng BLL để quản lý nhân viên
            InitializeComponent(); // Khởi tạo các thành phần giao diện từ Designer
            InitializeForm(); // Gọi phương thức khởi tạo form
        }

        // Khởi tạo form, tải dữ liệu ban đầu và cấu hình DataGridView
        private void InitializeForm()
        {
            ConfigureDataGridView(); // Cấu hình các cột của DataGridView
            LoadNhanVien(); // Tải danh sách nhân viên vào ComboBox
            LoadTaiKhoan(); // Tải danh sách tài khoản vào DataGridView          
        }

        // Cấu hình DataGridView với các cột cần thiết
        private void ConfigureDataGridView()
        {
            dgvTaiKhoan.AutoGenerateColumns = false; // Tắt tự động tạo cột
            dgvTaiKhoan.Columns.Clear(); // Xóa tất cả cột hiện có
            dgvTaiKhoan.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "TenDangNhap", HeaderText = "Tên Đăng Nhập", DataPropertyName = "TenDangNhap" },
                new DataGridViewTextBoxColumn { Name = "MatKhau", HeaderText = "Mật Khẩu", DataPropertyName = "MatKhau" },
                new DataGridViewTextBoxColumn { Name = "MaNhanVien", HeaderText = "Mã Nhân Viên", DataPropertyName = "MaNhanVien" },
                new DataGridViewTextBoxColumn { Name = "TenNhanVien", HeaderText = "Tên Nhân Viên", DataPropertyName = "TenNhanVien" }
            );
        }

        // Tải danh sách nhân viên vào ComboBox
        private void LoadNhanVien()
        {
            var nhanVienList = _nhanVienBLL.LayTatCaNhanVien(); // Lấy danh sách nhân viên từ BLL
            cbTenNhanVien.DataSource = nhanVienList; // Gán dữ liệu cho ComboBox
            cbTenNhanVien.DisplayMember = "HoTen"; // Hiển thị tên nhân viên
            cbTenNhanVien.ValueMember = "MaNhanVien"; // Giá trị thực tế là mã nhân viên
        }

        // Tải danh sách tài khoản vào DataGridView
        private void LoadTaiKhoan()
        {
            try
            {
                var taiKhoanList = _taiKhoanBLL.GetAllTaiKhoan(); // Lấy danh sách tài khoản
                if (taiKhoanList == null)
                    throw new InvalidOperationException("Danh sách tài khoản trả về là null."); // Kiểm tra null
                dgvTaiKhoan.DataSource = null; // Xóa dữ liệu cũ
                dgvTaiKhoan.DataSource = taiKhoanList; // Gán dữ liệu mới
                dgvTaiKhoan.Refresh(); // Làm mới DataGridView
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi tải danh sách tài khoản", ex); // Hiển thị thông báo lỗi
            }
        }

        // Xử lý sự kiện tìm kiếm tài khoản
        private void btnTimKiemTK_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiemTK.Text?.Trim(); // Lấy từ khóa tìm kiếm và loại bỏ khoảng trắng thừa
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Thoát nếu không có từ khóa
                }

                var taiKhoanList = _taiKhoanBLL.SearchTaiKhoan(keyword); // Tìm kiếm tài khoản
                dgvTaiKhoan.DataSource = taiKhoanList; // Cập nhật DataGridView

                MessageBox.Show(taiKhoanList.Count == 0
                    ? "Không tìm thấy tài khoản nào!"
                    : $"Tìm thấy {taiKhoanList.Count} tài khoản!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); // Hiển thị kết quả
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi tìm kiếm tài khoản", ex); // Hiển thị thông báo lỗi
            }
        }

        // Xử lý sự kiện khi chọn một dòng trong DataGridView
        private void dgvTK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // Thoát nếu không chọn dòng hợp lệ

                var row = dgvTaiKhoan.Rows[e.RowIndex]; // Lấy dòng được chọn
                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value?.ToString() ?? ""; // Gán Tên Đăng Nhập
                txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString() ?? ""; // Gán Mật Khẩu
                cbTenNhanVien.SelectedValue = row.Cells["MaNhanVien"].Value?.ToString() ?? ""; // Gán Mã Nhân Viên
                // Ngăn chỉnh sửa Tên Đăng Nhập và Mã Nhân Viên khi sửa
                txtTenDangNhap.ReadOnly = true;
                cbTenNhanVien.Enabled = false;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi chọn dòng trong DataGridView", ex); // Hiển thị thông báo lỗi
            }
        }

        // Xử lý sự kiện thêm tài khoản
        private void btnThemTK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return; // Kiểm tra dữ liệu đầu vào

                var taiKhoan = new TaiKhoanDTO
                {
                    TenDangNhap = txtTenDangNhap.Text.Trim(), // Lấy Tên Đăng Nhập
                    MatKhau = txtMatKhau.Text.Trim(), // Lấy Mật Khẩu
                    MaNhanVien = cbTenNhanVien.SelectedValue.ToString(), // Lấy Mã Nhân Viên
                    QuyenTruyCap = "User" // Gán quyền mặc định
                };

                _taiKhoanBLL.AddTaiKhoan(taiKhoan); // Thêm tài khoản
                MessageBox.Show("Thêm tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTaiKhoan(); // Tải lại danh sách
                ClearFields(); // Xóa dữ liệu nhập
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi thêm tài khoản", ex); // Hiển thị thông báo lỗi
            }
        }

        // Xử lý sự kiện sửa tài khoản
        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTenDangNhap.Text?.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn tài khoản để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Thoát nếu không chọn tài khoản
                }

                // Chỉ kiểm tra Mật Khẩu khi sửa, vì Tên Đăng Nhập và Mã Nhân Viên không được chỉnh sửa
                if (string.IsNullOrEmpty(txtMatKhau.Text?.Trim()))
                {
                    MessageBox.Show("Vui lòng nhập Mật Khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Thoát nếu không nhập Mật Khẩu
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn sửa tài khoản này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return; // Thoát nếu không xác nhận
                }

                var taiKhoan = new TaiKhoanDTO
                {
                    TenDangNhap = txtTenDangNhap.Text.Trim(), // Lấy Tên Đăng Nhập
                    MatKhau = txtMatKhau.Text.Trim(), // Lấy Mật Khẩu
                    MaNhanVien = cbTenNhanVien.SelectedValue.ToString(), // Lấy Mã Nhân Viên
                    QuyenTruyCap = "User" // Gán quyền mặc định
                };

                _taiKhoanBLL.UpdateTaiKhoan(taiKhoan); // Cập nhật tài khoản
                MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTaiKhoan(); // Tải lại danh sách
                ClearFields(); // Xóa dữ liệu nhập
                // Cho phép chỉnh sửa lại sau khi sửa xong
                txtTenDangNhap.ReadOnly = false;
                cbTenNhanVien.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi cập nhật tài khoản", ex); // Hiển thị thông báo lỗi
            }
        }

        // Xử lý sự kiện xóa tài khoản
        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTenDangNhap.Text?.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn tài khoản để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Thoát nếu không chọn tài khoản
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return; // Thoát nếu không xác nhận
                }

                _taiKhoanBLL.DeleteTaiKhoan(txtTenDangNhap.Text.Trim()); // Xóa tài khoản
                MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTaiKhoan(); // Tải lại danh sách
                ClearFields(); // Xóa dữ liệu nhập
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi xóa tài khoản", ex); // Hiển thị thông báo lỗi
            }
        }

        // Xử lý sự kiện làm mới form
        private void btnLamMoiTK_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields(); // Xóa dữ liệu nhập
                LoadTaiKhoan(); // Tải lại danh sách tài khoản
                txtTimKiemTK.Text = ""; // Xóa từ khóa tìm kiếm
                txtTenDangNhap.ReadOnly = false; // Mở khóa Tên Đăng Nhập
                cbTenNhanVien.Enabled = true; // Mở khóa ComboBox Nhân Viên
                MessageBox.Show("Đã làm mới danh sách tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi làm mới", ex); // Hiển thị thông báo lỗi
            }
        }

        // Xóa dữ liệu trong các trường nhập liệu
        private void ClearFields()
        {
            txtTenDangNhap.Text = ""; // Xóa Tên Đăng Nhập
            txtMatKhau.Text = ""; // Xóa Mật Khẩu
            cbTenNhanVien.SelectedIndex = -1; // Bỏ chọn trong ComboBox
            txtTenDangNhap.ReadOnly = false; // Mở khóa để nhập mới
            cbTenNhanVien.Enabled = true; // Mở khóa để chọn lại
        }

        // Kiểm tra dữ liệu đầu vào
        private bool ValidateInput(bool checkTenDangNhap = true)
        {
            if (checkTenDangNhap && string.IsNullOrEmpty(txtTenDangNhap.Text?.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Tên Đăng Nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Trả về false nếu không hợp lệ
            }

            if (string.IsNullOrEmpty(txtMatKhau.Text?.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Mật Khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Trả về false nếu không hợp lệ
            }

            if (cbTenNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Mã Nhân Viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Trả về false nếu không hợp lệ
            }

            return true; // Trả về true nếu tất cả hợp lệ
        }

        // Hiển thị thông báo lỗi chung
        private void ShowErrorMessage(string message, Exception ex)
        {
            MessageBox.Show($"{message}: {ex.Message}\nChi tiết: {ex.StackTrace}",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hiển thị chi tiết lỗi
        }
    }
}