using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    // Form giao diện người dùng để quản lý khuyến mãi.
    public partial class KhuyenMai : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly KhuyenMaiBLL _khuyenMaiBLL;

        // Hàm khởi tạo form, khởi tạo BLL và cấu hình giao diện.
        public KhuyenMai(TaiKhoanDTO taiKhoan = null)
        {
            _khuyenMaiBLL = new KhuyenMaiBLL();
            _taiKhoan = taiKhoan;
            InitializeComponent();
            KhoiTaoDieuKhien();
            TaiDanhSachKhuyenMai();
            XoaCacTruong();
            ThietLapMaKhuyenMaiMoi();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemKM.Visible = true;
                btnSuaKM.Visible = true;
                btnXoaKM.Visible = true;
                btnLamMoiKM.Visible = true;
                btnTimKiemKM.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaKM.Visible = false;
                    break;

                case "User":
                    btnThemKM.Visible = false;
                    btnSuaKM.Visible = false;
                    btnXoaKM.Visible = false;
                    break;

                default:
                    btnThemKM.Visible = false;
                    btnSuaKM.Visible = false;
                    btnXoaKM.Visible = false;
                    btnLamMoiKM.Visible = false;
                    btnTimKiemKM.Visible = false;
                    break;
            }
        }
        // Khởi tạo các điều khiển trên form.
        private void KhoiTaoDieuKhien()
        {
            dtpNgayBatDau.Format = DateTimePickerFormat.Custom;
            dtpNgayBatDau.CustomFormat = "dd/MM/yyyy";
            dtpNgayKetThuc.Format = DateTimePickerFormat.Custom;
            dtpNgayKetThuc.CustomFormat = "dd/MM/yyyy";
            txtMaKhuyenMai.MaxLength = 20;
            txtTenKhuyenMai.MaxLength = 100;
            txtMoTaKM.MaxLength = 200;
            txtMucGiamGia.Text = "0";

            CaiDatDataGridView();
        }

        // Cấu hình DataGridView để hiển thị danh sách khuyến mãi.
        private void CaiDatDataGridView()
        {
            dvgKhuyenMai.AutoGenerateColumns = false;
            dvgKhuyenMai.Columns.Clear();
            dvgKhuyenMai.Columns.Add("MaKhuyenMai", "Mã Khuyến Mãi");
            dvgKhuyenMai.Columns.Add("TenKhuyenMai", "Tên Khuyến Mãi");
            dvgKhuyenMai.Columns.Add("PhanTramKhuyenMai", "Phần Trăm Giảm");
            dvgKhuyenMai.Columns.Add("NgayBatDau", "Ngày Bắt Đầu");
            dvgKhuyenMai.Columns.Add("NgayKetThuc", "Ngày Kết Thúc");
            dvgKhuyenMai.Columns.Add("MoTa", "Mô Tả");
            dvgKhuyenMai.Columns["MaKhuyenMai"].DataPropertyName = "MaKhuyenMai";
            dvgKhuyenMai.Columns["TenKhuyenMai"].DataPropertyName = "TenKhuyenMai";
            dvgKhuyenMai.Columns["PhanTramKhuyenMai"].DataPropertyName = "PhanTramKhuyenMai";
            dvgKhuyenMai.Columns["NgayBatDau"].DataPropertyName = "NgayBatDau";
            dvgKhuyenMai.Columns["NgayKetThuc"].DataPropertyName = "NgayKetThuc";
            dvgKhuyenMai.Columns["MoTa"].DataPropertyName = "MoTa";
        }

        // Tải danh sách khuyến mãi từ BLL và hiển thị lên DataGridView.
        private void TaiDanhSachKhuyenMai()
        {
            try
            {
                var danhSach = _khuyenMaiBLL.LayTatCaKhuyenMai();
                if (danhSach == null)
                {
                    throw new InvalidOperationException("Danh sách khuyến mãi trả về là null.");
                }
                dvgKhuyenMai.DataSource = null;
                dvgKhuyenMai.DataSource = danhSach;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sinh mã khuyến mãi mới và vô hiệu hóa trường nhập mã.
        private void ThietLapMaKhuyenMaiMoi()
        {
            try
            {
                txtMaKhuyenMai.Text = _khuyenMaiBLL.TaoMaKhuyenMai();
                txtMaKhuyenMai.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa trắng các trường nhập liệu.
        private void XoaCacTruong()
        {
            txtTenKhuyenMai.Text = string.Empty;
            txtMucGiamGia.Text = "0";
            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now;
            txtMoTaKM.Text = string.Empty;
            txtTimKiem.Text = string.Empty;
        }

        // Xử lý sự kiện nhấn nút Thêm.
        private void btnThemKM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtMucGiamGia.Text, out decimal phanTramKhuyenMai))
                {
                    MessageBox.Show("Phần trăm giảm giá phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var khuyenMai = TaoKhuyenMaiTuInput();
                if (_khuyenMaiBLL.ThemKhuyenMai(khuyenMai))
                {
                    MessageBox.Show("Thêm khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachKhuyenMai();
                    XoaCacTruong();
                    ThietLapMaKhuyenMaiMoi();
                }
                else
                {
                    MessageBox.Show("Thêm khuyến mãi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Sửa.
        private void btnSuaKM_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaKhuyenMai.Text))
                {
                    MessageBox.Show("Vui lòng chọn khuyến mãi để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!decimal.TryParse(txtMucGiamGia.Text, out decimal phanTramKhuyenMai))
                {
                    MessageBox.Show("Phần trăm giảm giá phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa khuyến mãi này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var khuyenMai = TaoKhuyenMaiTuInput();
                if (_khuyenMaiBLL.CapNhatKhuyenMai(khuyenMai))
                {
                    MessageBox.Show("Cập nhật khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachKhuyenMai();
                    XoaCacTruong();
                    ThietLapMaKhuyenMaiMoi();
                }
                else
                {
                    MessageBox.Show("Cập nhật khuyến mãi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Xóa.
        private void btnXoaKM_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaKhuyenMai.Text))
                {
                    MessageBox.Show("Vui lòng chọn khuyến mãi để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa khuyến mãi này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                if (_khuyenMaiBLL.XoaKhuyenMai(txtMaKhuyenMai.Text.Trim()))
                {
                    MessageBox.Show("Xóa khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachKhuyenMai();
                    XoaCacTruong();
                    ThietLapMaKhuyenMaiMoi();
                }
                else
                {
                    MessageBox.Show("Xóa khuyến mãi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Làm mới.
        private void btnLamMoiKM_Click(object sender, EventArgs e)
        {
            try
            {
                XoaCacTruong();
                TaiDanhSachKhuyenMai();
                ThietLapMaKhuyenMaiMoi();
                MessageBox.Show("Đã làm mới danh sách khuyến mãi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới danh sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nhấn nút Tìm kiếm.
        private void btnTimKiemKM_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var danhSach = _khuyenMaiBLL.TimKiemKhuyenMai(txtTimKiem.Text.Trim());
                dvgKhuyenMai.DataSource = danhSach;
                MessageBox.Show(danhSach.Count == 0
                    ? "Không tìm thấy khuyến mãi nào!"
                    : $"Tìm thấy {danhSach.Count} khuyến mãi!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện chọn một dòng trong DataGridView.
        private void dvgKhuyenMai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dvgKhuyenMai.Rows[e.RowIndex];
                    txtMaKhuyenMai.Text = row.Cells["MaKhuyenMai"].Value?.ToString() ?? string.Empty;
                    txtTenKhuyenMai.Text = row.Cells["TenKhuyenMai"].Value?.ToString() ?? string.Empty;
                    txtMucGiamGia.Text = row.Cells["PhanTramKhuyenMai"].Value?.ToString() ?? "0";
                    if (row.Cells["NgayBatDau"].Value != null && DateTime.TryParse(row.Cells["NgayBatDau"].Value.ToString(), out DateTime ngayBatDau))
                    {
                        dtpNgayBatDau.Value = ngayBatDau;
                    }
                    if (row.Cells["NgayKetThuc"].Value != null && DateTime.TryParse(row.Cells["NgayKetThuc"].Value.ToString(), out DateTime ngayKetThuc))
                    {
                        dtpNgayKetThuc.Value = ngayKetThuc;
                    }
                    txtMoTaKM.Text = row.Cells["MoTa"].Value?.ToString() ?? string.Empty;
                    txtMaKhuyenMai.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tạo đối tượng KhuyenMaiDTO từ dữ liệu nhập vào.
        private KhuyenMaiDTO TaoKhuyenMaiTuInput()
        {
            return new KhuyenMaiDTO
            {
                MaKhuyenMai = txtMaKhuyenMai.Text.Trim(),
                TenKhuyenMai = txtTenKhuyenMai.Text.Trim(),
                PhanTramKhuyenMai = decimal.Parse(txtMucGiamGia.Text.Trim()),
                NgayBatDau = dtpNgayBatDau.Value,
                NgayKetThuc = dtpNgayKetThuc.Value,
                MoTa = string.IsNullOrWhiteSpace(txtMoTaKM.Text) ? null : txtMoTaKM.Text.Trim()
            };
        }
    }
}