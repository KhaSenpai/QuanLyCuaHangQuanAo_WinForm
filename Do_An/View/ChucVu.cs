using BLL;
using DTO;
using Guna.UI2.AnimatorNS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    // Form giao diện người dùng để quản lý chức vụ.
    public partial class ChucVu : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly ChucVuBLL _chucVuBLL;

        // Hàm khởi tạo form, khởi tạo BLL và cấu hình giao diện.
        public ChucVu(TaiKhoanDTO taiKhoan = null)
        {
            _chucVuBLL = new ChucVuBLL();
            _taiKhoan = taiKhoan; // Lưu TaiKhoanDTO
            InitializeComponent();
            TaiDanhSachChucVu();
            XoaCacTruong();
            ThietLapMaChucVuMoi();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThem.Visible = true;
                btnSua.Visible = true;
                btnXoa.Visible = true;
                btnLamMoi.Visible = true;
                btnTimKiem.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnThem.Visible = false;
                    btnSua.Visible = false;
                    btnXoa.Visible = false;
                    break;

                case "User":
                    btnThem.Visible = false;
                    btnSua.Visible = false;
                    btnXoa.Visible = false;
                    break;

                default:
                    btnThem.Visible = false;
                    btnSua.Visible = false;
                    btnXoa.Visible = false;
                    btnLamMoi.Visible=false;
                    btnTimKiem.Visible=false;
                    break;
            }
        }
        // Khởi tạo các điều khiển trên form.

        // Cấu hình DataGridView để hiển thị danh sách chức vụ.
        private void CaiDatDataGridView()
        {
            dgvChucVu.AutoGenerateColumns = false;
            dgvChucVu.Columns.Clear();
            dgvChucVu.Columns.Add("MaChucVu", "Mã Chức Vụ");
            dgvChucVu.Columns.Add("TenChucVu", "Tên Chức Vụ");
            dgvChucVu.Columns.Add("MoTa", "Mô Tả");
            dgvChucVu.Columns["MaChucVu"].DataPropertyName = "MaChucVu";
            dgvChucVu.Columns["TenChucVu"].DataPropertyName = "TenChucVu";
            dgvChucVu.Columns["MoTa"].DataPropertyName = "MoTa";
        }

        // Tải danh sách chức vụ từ BLL và hiển thị lên DataGridView.
        private void TaiDanhSachChucVu()
        {
            try
            {
                var danhSach = _chucVuBLL.LayTatCaChucVu();
                if (danhSach == null)
                {
                    throw new InvalidOperationException("Danh sách chức vụ trả về là null.");
                }
                dgvChucVu.DataSource = null;
                dgvChucVu.DataSource = danhSach;            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chức vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sinh mã chức vụ mới và vô hiệu hóa trường nhập mã.
        private void ThietLapMaChucVuMoi()
        {
            try
            {
                txtMaChucVu.Text = _chucVuBLL.TaoMaChucVu();
                txtMaChucVu.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã chức vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa trắng các trường nhập liệu.
        private void XoaCacTruong()
        {
            txtMaChucVu.Text = string.Empty;
            txtTenChucVu.Text = string.Empty;
            txtMoTa.Text = string.Empty;
            txtTimKiem.Text = string.Empty;
            txtMaChucVu.ReadOnly = false;
            ThietLapMaChucVuMoi();
        }

        // Xử lý sự kiện nhấn nút Thêm.
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var chucVu = TaoChucVuTuInput();
                if (_chucVuBLL.ThemChucVu(chucVu))
                {
                    MessageBox.Show($"Thêm chức vụ thành công! Mã: {chucVu.MaChucVu}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachChucVu();
                    XoaCacTruong();
                }
                else
                {
                    MessageBox.Show("Thêm chức vụ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chức vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Sửa.
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaChucVu.Text))
                {
                    MessageBox.Show("Vui lòng chọn chức vụ để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa chức vụ này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var chucVu = TaoChucVuTuInput();
                if (_chucVuBLL.CapNhatChucVu(chucVu))
                {
                    MessageBox.Show("Cập nhật chức vụ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachChucVu();
                    XoaCacTruong();
                }
                else
                {
                    MessageBox.Show("Cập nhật chức vụ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chức vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Xóa.
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaChucVu.Text))
                {
                    MessageBox.Show("Vui lòng chọn chức vụ để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa chức vụ này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                if (_chucVuBLL.XoaChucVu(txtMaChucVu.Text.Trim()))
                {
                    MessageBox.Show("Xóa chức vụ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachChucVu();
                    XoaCacTruong();
                }
                else
                {
                    MessageBox.Show("Xóa chức vụ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chức vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Làm mới.
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                XoaCacTruong();
                TaiDanhSachChucVu();
                MessageBox.Show("Đã làm mới danh sách chức vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Tìm kiếm.
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var danhSach = _chucVuBLL.TimKiemChucVu(txtTimKiem.Text.Trim());
                dgvChucVu.DataSource = danhSach;
                MessageBox.Show(danhSach.Count == 0
                    ? "Không tìm thấy chức vụ nào!"
                    : $"Tìm thấy {danhSach.Count} chức vụ!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm chức vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện chọn một dòng trong DataGridView.
        private void dgvChucVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvChucVu.Rows[e.RowIndex];
                    txtMaChucVu.Text = row.Cells["MaChucVu"].Value?.ToString() ?? string.Empty;
                    txtTenChucVu.Text = row.Cells["TenChucVu"].Value?.ToString() ?? string.Empty;
                    txtMoTa.Text = row.Cells["MoTa"].Value?.ToString() ?? string.Empty;
                    txtMaChucVu.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tạo đối tượng ChucVuDTO từ dữ liệu nhập vào.
        private ChucVuDTO TaoChucVuTuInput()
        {
            return new ChucVuDTO
            {
                MaChucVu = txtMaChucVu.Text.Trim(),
                TenChucVu = txtTenChucVu.Text.Trim(),
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim()
            };
        }
    }
}