using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    // Form giao diện người dùng để quản lý loại sản phẩm.
    public partial class LoaiSanPham : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly LoaiSanPhamBLL _loaiSanPhamBLL;

        // Hàm khởi tạo form, khởi tạo BLL và cấu hình giao diện.
        public LoaiSanPham(TaiKhoanDTO taiKhoan = null)
        {
            _loaiSanPhamBLL = new LoaiSanPhamBLL();
            _taiKhoan = taiKhoan; 
            InitializeComponent();
            CaiDatDataGridView();
            TaiDanhSachLoaiSanPham();
            XoaCacTruong();
            ThietLapMaLoaiSanPhamMoi();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemLSP.Visible = true;
                btnSuaLSP.Visible = true;
                btnXoaLSP.Visible = true;
                btnLamMoiLSP.Visible = true;
                btnTimKiemLSP.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaLSP.Visible = false;
                    break;

                case "User":
                    btnThemLSP.Visible = false;
                    btnSuaLSP.Visible = false;
                    btnXoaLSP.Visible = false;
                    break;

                default:
                    btnThemLSP.Visible = false;
                    btnSuaLSP.Visible = false;
                    btnXoaLSP.Visible = false;
                    btnLamMoiLSP.Visible = false;
                    btnTimKiemLSP.Visible = false;
                    break;
            }
        }

        // Cấu hình DataGridView để hiển thị danh sách loại sản phẩm.
        private void CaiDatDataGridView()
        {
            dgvLoaiSanPham.AutoGenerateColumns = false;
            dgvLoaiSanPham.Columns.Clear();
            dgvLoaiSanPham.Columns.Add("MaLoaiSP", "Mã Loại Sản Phẩm");
            dgvLoaiSanPham.Columns.Add("TenLoaiSP", "Tên Loại Sản Phẩm");
            dgvLoaiSanPham.Columns.Add("MoTa", "Mô Tả");
            dgvLoaiSanPham.Columns["MaLoaiSP"].DataPropertyName = "MaLoaiSP";
            dgvLoaiSanPham.Columns["TenLoaiSP"].DataPropertyName = "TenLoaiSP";
            dgvLoaiSanPham.Columns["MoTa"].DataPropertyName = "MoTa";
        }

        // Tải danh sách loại sản phẩm từ BLL và hiển thị lên DataGridView.
        private void TaiDanhSachLoaiSanPham()
        {
            try
            {
                var danhSach = _loaiSanPhamBLL.LayTatCaLoaiSanPham();
                if (danhSach == null)
                {
                    throw new InvalidOperationException("Danh sách loại sản phẩm trả về là null.");
                }
                dgvLoaiSanPham.DataSource = null;
                dgvLoaiSanPham.DataSource = danhSach;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sinh mã loại sản phẩm mới và vô hiệu hóa trường nhập mã.
        private void ThietLapMaLoaiSanPhamMoi()
        {
            try
            {
                txtMaLoaiSP.Text = _loaiSanPhamBLL.TaoMaLoaiSanPham();
                txtMaLoaiSP.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa trắng các trường nhập liệu.
        private void XoaCacTruong()
        {
            txtMaLoaiSP.Text = string.Empty;
            txtTenLoaiSP.Text = string.Empty;
            txtMoTaLoaiSP.Text = string.Empty;
        }

        // Xử lý sự kiện nhấn nút Thêm.
        private void btnThemLSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraCacTruongBatBuoc())
                {
                    return;
                }

                var loaiSanPham = TaoLoaiSanPhamTuInput();
                _loaiSanPhamBLL.ThemLoaiSanPham(loaiSanPham);
                MessageBox.Show("Thêm loại sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachLoaiSanPham();
                XoaCacTruong();
                ThietLapMaLoaiSanPhamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Sửa.
        private void btnSuaLSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaLoaiSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!KiemTraCacTruongBatBuoc(true))
                {
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa loại sản phẩm này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var loaiSanPham = TaoLoaiSanPhamTuInput();
                _loaiSanPhamBLL.CapNhatLoaiSanPham(loaiSanPham);
                MessageBox.Show("Cập nhật loại sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachLoaiSanPham();
                XoaCacTruong();
                ThietLapMaLoaiSanPhamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Xóa.
        private void btnXoaLSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaLoaiSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa loại sản phẩm này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                _loaiSanPhamBLL.XoaLoaiSanPham(txtMaLoaiSP.Text.Trim());
                MessageBox.Show("Xóa loại sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDanhSachLoaiSanPham();
                XoaCacTruong();
                ThietLapMaLoaiSanPhamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Làm mới.
        private void btnLamMoiLSP_Click(object sender, EventArgs e)
        {
            try
            {
                XoaCacTruong();
                txtTimKiemLSP.Text = string.Empty;
                TaiDanhSachLoaiSanPham();
                ThietLapMaLoaiSanPhamMoi();
                MessageBox.Show("Đã làm mới danh sách loại sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Tìm kiếm.
        private void btnTimKiemLSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimKiemLSP.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var danhSach = _loaiSanPhamBLL.TimKiemLoaiSanPham(txtTimKiemLSP.Text.Trim());
                dgvLoaiSanPham.DataSource = danhSach;
                MessageBox.Show(danhSach.Count == 0
                    ? "Không tìm thấy loại sản phẩm nào!"
                    : $"Tìm thấy {danhSach.Count} loại sản phẩm!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện chọn một dòng trong DataGridView.
        private void dgvLoaiSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvLoaiSanPham.Rows[e.RowIndex];
                    txtMaLoaiSP.Text = row.Cells["MaLoaiSP"].Value?.ToString() ?? string.Empty;
                    txtTenLoaiSP.Text = row.Cells["TenLoaiSP"].Value?.ToString() ?? string.Empty;
                    txtMoTaLoaiSP.Text = row.Cells["MoTa"].Value?.ToString() ?? string.Empty;
                    txtMaLoaiSP.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kiểm tra các trường bắt buộc (Mã và Tên loại sản phẩm).
        // Nếu boQuaMaLoaiSP = true, bỏ qua kiểm tra mã (dùng khi sửa).
        private bool KiemTraCacTruongBatBuoc(bool boQuaMaLoaiSP = false)
        {
            if ((!boQuaMaLoaiSP && string.IsNullOrWhiteSpace(txtMaLoaiSP.Text)) ||
                string.IsNullOrWhiteSpace(txtTenLoaiSP.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc (Mã loại sản phẩm, Tên loại sản phẩm)!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Tạo đối tượng LoaiSanPhamDTO từ dữ liệu nhập vào.
        private LoaiSanPhamDTO TaoLoaiSanPhamTuInput()
        {
            return new LoaiSanPhamDTO
            {
                MaLoaiSP = txtMaLoaiSP.Text.Trim(),
                TenLoaiSP = txtTenLoaiSP.Text.Trim(),
                MoTa = string.IsNullOrWhiteSpace(txtMoTaLoaiSP.Text) ? null : txtMoTaLoaiSP.Text.Trim()
            };
        }
    }
}