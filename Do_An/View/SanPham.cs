using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace View
{
    public partial class SanPham : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly SanPhamBLL _sanPhamBLL;
        private string _selectedMaSP;
        private HinhAnhDTO _hinhAnhTamThoi;

        public SanPham(TaiKhoanDTO taiKhoan = null)
        {
            _sanPhamBLL = new SanPhamBLL();
            _selectedMaSP = string.Empty;
            _hinhAnhTamThoi = null;
            _taiKhoan = taiKhoan;
            InitializeComponent();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemSP.Visible = true;
                btnSuaSP.Visible = true;
                btnXoaSP.Visible = true;
                btnLamMoi.Visible = true;
                btnTimKiem.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaSP.Visible = false;
                    break;

                case "User":
                    btnThemSP.Visible = false;
                    btnSuaSP.Visible = false;
                    btnXoaSP.Visible = false;
                    btnXoaHinhAnh.Visible = false;
                    btnTaiHinhAnh.Visible = false;
                    break;

                default:
                    btnThemSP.Visible = false;
                    btnSuaSP.Visible = false;
                    btnXoaSP.Visible = false;
                    btnTimKiem.Visible = true;
                    btnLamMoi.Visible = true;
                    break;
            }
        }

        private void SanPham_Load(object sender, EventArgs e)
        {
            try
            {
                TaiDuLieu();
                TaiComboBox();
                CapNhatTongSoSanPham();
                TaoMaSanPham();
                XoaCacTruong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiDuLieu()
        {
            try
            {
                var danhSachSanPham = _sanPhamBLL.LayTatCaSanPham();
                if (danhSachSanPham != null)
                    dgvSanPham.DataSource = danhSachSanPham;
                else
                    MessageBox.Show("Không tải được dữ liệu sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiComboBox()
        {
            try
            {
                var danhSachChatLieu = _sanPhamBLL.LayTatCaChatLieu();
                if (danhSachChatLieu != null && danhSachChatLieu.Count > 0)
                {
                    cboChatLieu.DataSource = danhSachChatLieu;
                    cboChatLieu.DisplayMember = "TenChatLieu";
                    cboChatLieu.ValueMember = "MaChatLieu";
                    cboChatLieu.SelectedIndex = -1;
                }
                else
                {
                    cboChatLieu.DataSource = null;
                    MessageBox.Show("Không có dữ liệu chất liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                var danhSachLoaiSP = _sanPhamBLL.LayTatCaLoaiSanPham();
                if (danhSachLoaiSP != null && danhSachLoaiSP.Count > 0)
                {
                    cboLoaiSP.DataSource = danhSachLoaiSP;
                    cboLoaiSP.DisplayMember = "TenLoaiSP";
                    cboLoaiSP.ValueMember = "MaLoaiSP";
                    cboLoaiSP.SelectedIndex = -1;
                }
                else
                {
                    cboLoaiSP.DataSource = null;
                    MessageBox.Show("Không có dữ liệu loại sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                var danhSachNCC = _sanPhamBLL.LayTatCaNhaCungCap();
                if (danhSachNCC != null && danhSachNCC.Count > 0)
                {
                    cboNhaCungCap.DataSource = danhSachNCC;
                    cboNhaCungCap.DisplayMember = "TenNCC";
                    cboNhaCungCap.ValueMember = "MaNCC";
                    cboNhaCungCap.SelectedIndex = -1;
                }
                else
                {
                    cboNhaCungCap.DataSource = null;
                    MessageBox.Show("Không có dữ liệu nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                var danhSachThuongHieu = _sanPhamBLL.LayTatCaThuongHieu();
                if (danhSachThuongHieu != null && danhSachThuongHieu.Count > 0)
                {
                    cboThuongHieu.DataSource = danhSachThuongHieu;
                    cboThuongHieu.DisplayMember = "TenThuongHieu";
                    cboThuongHieu.ValueMember = "MaThuongHieu";
                    cboThuongHieu.SelectedIndex = -1;
                }
                else
                {
                    cboThuongHieu.DataSource = null;
                    MessageBox.Show("Không có dữ liệu thương hiệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu combobox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatTongSoSanPham(IList<SanPhamDTO> danhSachSanPham = null)
        {
            try
            {
                int tongSo = danhSachSanPham != null ? danhSachSanPham.Count : _sanPhamBLL.LayTongSoSanPham();
                lblTongSanPham.Text = $"Tổng sản phẩm: {tongSo}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tính tổng sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTongSanPham.Text = "Tổng sản phẩm: 0";
            }
        }

        private void TaoMaSanPham()
        {
            try
            {
                txtMaSP.Text = _sanPhamBLL.TaoMaSanPham() ?? "SP001";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaSP.Text = "SP001";
            }
        }

        private void XoaCacTruong()
        {
            try
            {
                _selectedMaSP = string.Empty;
                txtTenSP.Text = string.Empty;
                nudSoLuongTon.Value = 0;
                txtDonGiaBan.Text = string.Empty;
                txtDonGiaNhap.Text = string.Empty;
                txtMauSac.Text = string.Empty;
                txtKichCo.Text = string.Empty;
                dgtNgaySanXuat.Value = DateTime.Now;
                txtMoTaSP.Text = string.Empty;
                cboThuongHieu.SelectedIndex = -1;
                cboLoaiSP.SelectedIndex = -1;
                cboChatLieu.SelectedIndex = -1;
                cboNhaCungCap.SelectedIndex = -1;
                txtTimKiem.Text = string.Empty;
                pbThemHinhAnh.Image = null;
                _hinhAnhTamThoi = null;
                txtMaSP.ReadOnly = true;
                nudSoLuongTon.Minimum = 0;
                nudSoLuongTon.Maximum = 1000;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa các trường: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraDuLieuHopLe())
                    return;

                var sanPham = TaoSanPhamTuForm();
                sanPham.HinhAnh = _hinhAnhTamThoi;

                if (_sanPhamBLL.ThemSanPham(sanPham))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieu();
                    XoaCacTruong();
                    TaoMaSanPham();
                    CapNhatTongSoSanPham();
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_selectedMaSP))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!KiemTraDuLieuHopLe())
                    return;

                var sanPham = TaoSanPhamTuForm();
                sanPham.MaSP = _selectedMaSP;
                sanPham.HinhAnh = _hinhAnhTamThoi;

                if (_sanPhamBLL.CapNhatSanPham(sanPham))
                {
                    MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieu();
                    XoaCacTruong();
                    TaoMaSanPham();
                    CapNhatTongSoSanPham();
                }
                else
                {
                    MessageBox.Show("Sửa sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_selectedMaSP))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này? Hình ảnh liên quan cũng sẽ bị xóa.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (_sanPhamBLL.XoaSanPham(_selectedMaSP))
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaiDuLieu();
                        XoaCacTruong();
                        TaoMaSanPham();
                        CapNhatTongSoSanPham();
                    }
                    else
                    {
                        MessageBox.Show("Xóa sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                TaiDuLieu();
                XoaCacTruong();
                TaoMaSanPham();
                CapNhatTongSoSanPham(); // Cập nhật lại tổng số sản phẩm khi làm mới
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    TaiDuLieu();
                    CapNhatTongSoSanPham(); // Cập nhật tổng số sản phẩm cho toàn bộ danh sách
                    return;
                }

                var danhSachLoc = _sanPhamBLL.TimKiemSanPham(tuKhoa);
                if (danhSachLoc != null && danhSachLoc.Count > 0)
                {
                    dgvSanPham.DataSource = danhSachLoc;
                    CapNhatTongSoSanPham(danhSachLoc); // Cập nhật tổng số sản phẩm dựa trên danh sách đã lọc
                }
                else
                {
                    dgvSanPham.DataSource = null;
                    MessageBox.Show("Không tìm thấy sản phẩm nào!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CapNhatTongSoSanPham(); // Cập nhật tổng số sản phẩm là 0 khi không có kết quả
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CapNhatTongSoSanPham(); // Cập nhật tổng số sản phẩm là 0 khi có lỗi
            }
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    var row = dgvSanPham.Rows[e.RowIndex];
                    _selectedMaSP = row.Cells["MaSP"].Value?.ToString() ?? string.Empty;
                    txtMaSP.Text = _selectedMaSP;

                    txtTenSP.Text = row.Cells["TenSP"].Value?.ToString() ?? string.Empty;
                    nudSoLuongTon.Value = row.Cells["SoLuongTon"].Value != null ? Convert.ToInt32(row.Cells["SoLuongTon"].Value) : 0;
                    txtDonGiaBan.Text = row.Cells["DonGiaBan"].Value?.ToString() ?? string.Empty;
                    txtDonGiaNhap.Text = row.Cells["DonGiaNhap"].Value?.ToString() ?? string.Empty;
                    txtMauSac.Text = row.Cells["MauSac"].Value?.ToString() ?? string.Empty;
                    txtKichCo.Text = row.Cells["KichCo"].Value?.ToString() ?? string.Empty;
                    dgtNgaySanXuat.Value = row.Cells["NgaySanXuat"].Value != null ? Convert.ToDateTime(row.Cells["NgaySanXuat"].Value) : DateTime.Now;
                    txtMoTaSP.Text = row.Cells["MoTa"].Value?.ToString() ?? string.Empty;

                    if (cboThuongHieu.DataSource != null && row.Cells["MaThuongHieu"].Value != null)
                        cboThuongHieu.SelectedValue = row.Cells["MaThuongHieu"].Value.ToString();
                    else
                        cboThuongHieu.SelectedIndex = -1;

                    if (cboLoaiSP.DataSource != null && row.Cells["MaLoaiSP"].Value != null)
                        cboLoaiSP.SelectedValue = row.Cells["MaLoaiSP"].Value.ToString();
                    else
                        cboLoaiSP.SelectedIndex = -1;

                    if (cboChatLieu.DataSource != null && row.Cells["MaChatLieu"].Value != null)
                        cboChatLieu.SelectedValue = row.Cells["MaChatLieu"].Value.ToString();
                    else
                        cboChatLieu.SelectedIndex = -1;

                    if (cboNhaCungCap.DataSource != null && row.Cells["MaNCC"].Value != null)
                        cboNhaCungCap.SelectedValue = row.Cells["MaNCC"].Value.ToString();
                    else
                        cboNhaCungCap.SelectedIndex = -1;

                    var sanPham = (SanPhamDTO)row.DataBoundItem;
                    _hinhAnhTamThoi = sanPham.HinhAnh;
                    pbThemHinhAnh.Image = _hinhAnhTamThoi != null && File.Exists(_hinhAnhTamThoi.DuongDanHinh)
                        ? new Bitmap(_hinhAnhTamThoi.DuongDanHinh)
                        : null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chọn sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTaiHinhAnh_Click(object sender, EventArgs e)
        {
            try
            {
                if (ofdThemHinhAnh.ShowDialog() == DialogResult.OK)
                {
                    string duongDanHinh = ofdThemHinhAnh.FileName;
                    pbThemHinhAnh.Image = new Bitmap(duongDanHinh);

                    string maHinhAnh = _sanPhamBLL.TaoMaHinhAnh();
                    _hinhAnhTamThoi = new HinhAnhDTO
                    {
                        MaHinhAnh = maHinhAnh,
                        MaSP = txtMaSP.Text,
                        DuongDanHinh = duongDanHinh
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaHinhAnh_Click(object sender, EventArgs e)
        {
            try
            {
                if (_hinhAnhTamThoi == null)
                {
                    MessageBox.Show("Không có hình ảnh để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa hình ảnh này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _hinhAnhTamThoi = null;
                    pbThemHinhAnh.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraDuLieuHopLe()
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (nudSoLuongTon.Value < 0)
            {
                MessageBox.Show("Số lượng tồn phải là số nguyên không âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtDonGiaBan.Text, out decimal donGiaBan) || donGiaBan <= 0)
            {
                MessageBox.Show("Đơn giá bán phải là số dương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtDonGiaNhap.Text, out decimal donGiaNhap) || donGiaNhap <= 0)
            {
                MessageBox.Show("Đơn giá nhập phải là số dương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboChatLieu.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chất liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboLoaiSP.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboNhaCungCap.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboThuongHieu.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn thương hiệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private SanPhamDTO TaoSanPhamTuForm()
        {
            return new SanPhamDTO
            {
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                SoLuongTon = (int)nudSoLuongTon.Value,
                DonGiaBan = decimal.Parse(txtDonGiaBan.Text),
                DonGiaNhap = decimal.Parse(txtDonGiaNhap.Text),
                MauSac = string.IsNullOrWhiteSpace(txtMauSac.Text) ? null : txtMauSac.Text,
                KichCo = string.IsNullOrWhiteSpace(txtKichCo.Text) ? null : txtKichCo.Text,
                NgaySanXuat = dgtNgaySanXuat.Value,
                MoTa = string.IsNullOrWhiteSpace(txtMoTaSP.Text) ? null : txtMoTaSP.Text,
                MaThuongHieu = cboThuongHieu.SelectedValue?.ToString(),
                MaLoaiSP = cboLoaiSP.SelectedValue?.ToString(),
                MaChatLieu = cboChatLieu.SelectedValue?.ToString(),
                MaNCC = cboNhaCungCap.SelectedValue?.ToString()
            };
        }
    }
}