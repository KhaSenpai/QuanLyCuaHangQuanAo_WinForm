using BLL;
using DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace View
{
    // Form quản lý hóa đơn, hiển thị và thao tác với hóa đơn và chi tiết hóa đơn.
    public partial class HoaDon : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly HoaDonBLL _hoaDonBLL;
        private List<ChiTietHoaDonDTO> _chiTietHoaDonList = new List<ChiTietHoaDonDTO>(); // Danh sách chi tiết hóa đơn tạm thời
        private Dictionary<string, string> _khachHangDict = new Dictionary<string, string>(); // Ánh xạ tên khách hàng -> mã khách hàng
        private Dictionary<string, string> _nhanVienDict = new Dictionary<string, string>(); // Ánh xạ tên nhân viên -> mã nhân viên
        private Dictionary<string, string> _sanPhamDict = new Dictionary<string, string>(); // Ánh xạ tên sản phẩm -> mã sản phẩm
        private Dictionary<string, string> _khuyenMaiDict = new Dictionary<string, string>(); // Ánh xạ tên khuyến mãi -> mã khuyến mãi
        private string _selectedMaSanPham = null; // Mã sản phẩm được chọn trong DataGridView chi tiết hóa đơn

        public HoaDon(TaiKhoanDTO taiKhoan = null)
        {
            _hoaDonBLL = new HoaDonBLL();            
            _taiKhoan = taiKhoan;
            InitializeComponent();
            KhoiTaoControls();
            TaiDuLieu();
            XoaDuLieuForm();
            KiemTraQuyenTruyCap();
        }

        private void KiemTraQuyenTruyCap()
        {
            if (_taiKhoan == null || string.IsNullOrEmpty(_taiKhoan.QuyenTruyCap))
            {
                btnThemHD.Visible = true;
                btnSuaHD.Visible = true;
                btnXoaHD.Visible = true;
                btnThemCTHD.Visible = true;
                btnXoaCTHD.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaHD.Visible = false;
                    break;

                case "User":
                    btnSuaHD.Visible = false;
                    btnXoaHD.Visible = false;
                    break;

                default:
                    btnThemHD.Visible = false;
                    btnSuaHD.Visible = false;
                    btnXoaHD.Visible = false;
                    btnThemCTHD.Visible = false;
                    btnXoaCTHD.Visible = false;
                    break;
            }
        }
        // Khởi tạo các control trên form.
        private void KhoiTaoControls()
        {
            // Cấu hình DateTimePicker
            dtpNgayLap.Format = DateTimePickerFormat.Custom;
            dtpNgayLap.CustomFormat = "dd/MM/yyyy";

            // Cấu hình TextBox
            txtMaHoaDon.MaxLength = 20;
            txtDiaChi.MaxLength = 200;
            txtGhiChuHD.MaxLength = 500;
            txtMaHoaDon.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtTongTien.ReadOnly = true;

            // Cấu hình NumericUpDown
            nudSoLuong.Minimum = 1;
            // Cấu hình DateTimePicker
            dtpNgayLap.Enabled = false;

            // Gán sự kiện
            cbTenSanPham.SelectedIndexChanged += CbTenSanPham_SelectedIndexChanged;
            cbTenKhuyenMai.SelectedIndexChanged += CbTenKhuyenMai_SelectedIndexChanged;
            nudSoLuong.ValueChanged += NudSoLuong_ValueChanged;
            dgvHoaDon.CellClick += DgvHoaDon_CellClick;
            dgvChiTietHoaDon.CellClick += DgvChiTietHoaDon_CellClick;
        }

        // Tải dữ liệu ban đầu cho form.
        private void TaiDuLieu()
        {
            try
            {
                TaiDanhSachHoaDon();
                TaiComboBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tải danh sách hóa đơn vào DataGridView.
        private void TaiDanhSachHoaDon()
        {
            try
            {
                var hoaDonList = _hoaDonBLL.LayDanhSachHoaDon();
                dgvHoaDon.DataSource = hoaDonList;
                CauHinhDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cấu hình các cột của DataGridView hóa đơn.
        private void CauHinhDataGridView()
        {
            if (dgvHoaDon.Columns.Contains("MaHoaDon"))
                dgvHoaDon.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
            if (dgvHoaDon.Columns.Contains("TenKhachHang"))
                dgvHoaDon.Columns["TenKhachHang"].HeaderText = "Tên Khách Hàng";
            if (dgvHoaDon.Columns.Contains("HoTen"))
                dgvHoaDon.Columns["HoTen"].HeaderText = "Tên Nhân Viên";
            if (dgvHoaDon.Columns.Contains("NgayLapHoaDon"))
                dgvHoaDon.Columns["NgayLapHoaDon"].HeaderText = "Ngày Lập";
            if (dgvHoaDon.Columns.Contains("TenKhuyenMai"))
                dgvHoaDon.Columns["TenKhuyenMai"].HeaderText = "Tên Khuyến Mãi";
            if (dgvHoaDon.Columns.Contains("DiaChi"))
                dgvHoaDon.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            if (dgvHoaDon.Columns.Contains("TongTien"))
                dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";
            if (dgvHoaDon.Columns.Contains("GhiChu"))
                dgvHoaDon.Columns["GhiChu"].HeaderText = "Ghi Chú";

            // Ẩn các cột không cần thiết
            if (dgvHoaDon.Columns.Contains("MaKhachHang"))
                dgvHoaDon.Columns["MaKhachHang"].Visible = false;
            if (dgvHoaDon.Columns.Contains("MaNhanVien"))
                dgvHoaDon.Columns["MaNhanVien"].Visible = false;
            if (dgvHoaDon.Columns.Contains("MaKhuyenMai"))
                dgvHoaDon.Columns["MaKhuyenMai"].Visible = false;
        }

        // Tải dữ liệu vào các ComboBox.
        private void TaiComboBoxes()
        {
            try
            {
                // Tải khách hàng
                _khachHangDict = _hoaDonBLL.LayDanhSachKhachHang();
                cbTenKhachHang.Items.Clear();
                cbTenKhachHang.Items.AddRange(_khachHangDict.Keys.ToArray());
                cbTenKhachHang.SelectedIndex = -1;

                // Tải nhân viên
                _nhanVienDict = _hoaDonBLL.LayDanhSachNhanVien();
                cbTenNhanVien.Items.Clear();
                cbTenNhanVien.Items.AddRange(_nhanVienDict.Keys.ToArray());
                cbTenNhanVien.SelectedIndex = -1;

                // Tải sản phẩm
                _sanPhamDict = _hoaDonBLL.LayDanhSachSanPham();
                cbTenSanPham.Items.Clear();
                cbTenSanPham.Items.AddRange(_sanPhamDict.Keys.ToArray());
                cbTenSanPham.SelectedIndex = -1;

                // Tải khuyến mãi
                _khuyenMaiDict = _hoaDonBLL.LayDanhSachKhuyenMai();
                cbTenKhuyenMai.Items.Clear();
                cbTenKhuyenMai.Items.AddRange(_khuyenMaiDict.Keys.ToArray());
                cbTenKhuyenMai.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách combobox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thêm hóa đơn mới.
        private void btnThemHD_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbTenKhachHang.SelectedIndex < 0 || cbTenNhanVien.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng và nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtTongTien.Text, out decimal tongTien))
                {
                    MessageBox.Show("Tổng tiền phải là số hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_chiTietHoaDonList.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một chi tiết hóa đơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()))
                {
                    MessageBox.Show("Mã hóa đơn không được để trống! Vui lòng làm mới để sinh mã mới.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var hoaDon = new HoaDonDTO
                {
                    MaHoaDon = txtMaHoaDon.Text.Trim(),
                    MaKhachHang = _khachHangDict[cbTenKhachHang.SelectedItem.ToString()],
                    MaNhanVien = _nhanVienDict[cbTenNhanVien.SelectedItem.ToString()],
                    NgayLapHoaDon = dtpNgayLap.Value,
                    MaKhuyenMai = cbTenKhuyenMai.SelectedIndex >= 0 ? _khuyenMaiDict[cbTenKhuyenMai.SelectedItem.ToString()] : null,
                    DiaChi = string.IsNullOrEmpty(txtDiaChi.Text.Trim()) ? null : txtDiaChi.Text.Trim(),
                    TongTien = tongTien,
                    GhiChu = string.IsNullOrEmpty(txtGhiChuHD.Text.Trim()) ? null : txtGhiChuHD.Text.Trim()
                };

                if (_hoaDonBLL.ThemHoaDon(hoaDon))
                {
                    foreach (var chiTiet in _chiTietHoaDonList)
                    {
                        chiTiet.MaHoaDon = hoaDon.MaHoaDon;
                        if (!_hoaDonBLL.ThemChiTietHoaDon(chiTiet))
                        {
                            MessageBox.Show($"Lỗi khi thêm chi tiết hóa đơn cho sản phẩm {chiTiet.TenSanPham}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    MessageBox.Show("Thêm hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachHoaDon();
                    XoaDuLieuForm();
                }
                else
                {
                    MessageBox.Show("Thêm hóa đơn thất bại! Vui lòng kiểm tra lại dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm hóa đơn: {ex.Message}\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật thông tin hóa đơn.
        private void btnSuaHD_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbTenKhachHang.SelectedIndex < 0 || cbTenNhanVien.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng và nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtTongTien.Text, out decimal tongTien))
                {
                    MessageBox.Show("Tổng tiền phải là số hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_chiTietHoaDonList.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một chi tiết hóa đơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var hoaDon = new HoaDonDTO
                {
                    MaHoaDon = txtMaHoaDon.Text.Trim(),
                    MaKhachHang = _khachHangDict[cbTenKhachHang.SelectedItem.ToString()],
                    MaNhanVien = _nhanVienDict[cbTenNhanVien.SelectedItem.ToString()],
                    NgayLapHoaDon = dtpNgayLap.Value,
                    MaKhuyenMai = cbTenKhuyenMai.SelectedIndex >= 0 ? _khuyenMaiDict[cbTenKhuyenMai.SelectedItem.ToString()] : null,
                    DiaChi = string.IsNullOrEmpty(txtDiaChi.Text.Trim()) ? null : txtDiaChi.Text.Trim(),
                    TongTien = tongTien,
                    GhiChu = string.IsNullOrEmpty(txtGhiChuHD.Text.Trim()) ? null : txtGhiChuHD.Text.Trim()
                };

                if (_hoaDonBLL.CapNhatHoaDon(hoaDon))
                {
                    var chiTietCuList = _hoaDonBLL.LayChiTietHoaDon(hoaDon.MaHoaDon);
                    foreach (var chiTiet in chiTietCuList)
                    {
                        if (!_hoaDonBLL.XoaChiTietHoaDon(chiTiet.MaHoaDon, chiTiet.MaSanPham))
                        {
                            MessageBox.Show($"Lỗi khi xóa chi tiết hóa đơn cũ cho sản phẩm {chiTiet.TenSanPham}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    foreach (var chiTiet in _chiTietHoaDonList)
                    {
                        chiTiet.MaHoaDon = hoaDon.MaHoaDon;
                        if (!_hoaDonBLL.ThemChiTietHoaDon(chiTiet))
                        {
                            MessageBox.Show($"Lỗi khi thêm chi tiết hóa đơn mới cho sản phẩm {chiTiet.TenSanPham}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachHoaDon();
                    XoaDuLieuForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật hóa đơn thất bại! Vui lòng kiểm tra lại dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật hóa đơn: {ex.Message}\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa hóa đơn.
        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_hoaDonBLL.XoaHoaDon(txtMaHoaDon.Text.Trim()))
                    {
                        MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaiDanhSachHoaDon();
                        XoaDuLieuForm();
                    }
                    else
                    {
                        MessageBox.Show("Xóa hóa đơn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Làm mới form và danh sách hóa đơn.
        private void btnLamMoiHD_Click(object sender, EventArgs e)
        {
            try
            {
                XoaDuLieuForm();
                TaiDanhSachHoaDon();
                MessageBox.Show("Đã làm mới danh sách hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thêm chi tiết hóa đơn vào danh sách tạm thời.
       private void btnThemCTHD_Click(object sender, EventArgs e)
{
    try
    {
        if (cbTenSanPham.SelectedIndex < 0)
        {
            MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!decimal.TryParse(txtDonGia.Text, out decimal donGia) || donGia <= 0)
        {
            MessageBox.Show("Đơn giá phải là số hợp lệ và lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var tenSanPham = cbTenSanPham.SelectedItem.ToString();
        var maSanPham = _sanPhamDict[tenSanPham];
        var soLuong = (int)nudSoLuong.Value;

        // Kiểm tra số lượng tồn kho
        var soLuongTon = _hoaDonBLL.LaySoLuongTonSanPham(maSanPham);
        if (soLuong > soLuongTon)
        {
            MessageBox.Show($"Số lượng sản phẩm chỉ còn lại {soLuongTon} !", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var thanhTien = donGia * soLuong;

        var existingChiTiet = _chiTietHoaDonList.Find(ct => ct.MaSanPham == maSanPham);
        if (existingChiTiet != null)
        {
            var newSoLuong = existingChiTiet.SoLuong + soLuong;
            if (newSoLuong > soLuongTon)
            {
                MessageBox.Show($"Tổng số lượng chỉ còn lại {soLuongTon}!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            existingChiTiet.SoLuong = newSoLuong;
            existingChiTiet.ThanhTien = existingChiTiet.SoLuong * donGia;
        }
        else
        {
            _chiTietHoaDonList.Add(new ChiTietHoaDonDTO
            {
                MaHoaDon = txtMaHoaDon.Text.Trim(),
                MaSanPham = maSanPham,
                TenSanPham = tenSanPham,
                SoLuong = soLuong,
                DonGia = donGia,
                ThanhTien = thanhTien
            });
        }

        CapNhatChiTietHoaDon();
        CapNhatTongTien();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi khi thêm chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

        // Xóa chi tiết hóa đơn khỏi danh sách tạm thời.
        private void btnXoaCTHD_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedMaSanPham))
                {
                    MessageBox.Show("Vui lòng chọn một sản phẩm trong chi tiết hóa đơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var chiTiet = _chiTietHoaDonList.Find(ct => ct.MaSanPham == _selectedMaSanPham);
                if (chiTiet != null)
                {
                    _chiTietHoaDonList.Remove(chiTiet);
                    CapNhatChiTietHoaDon();
                    CapNhatTongTien();
                    _selectedMaSanPham = null;
                    cbTenSanPham.SelectedIndex = -1;
                    nudSoLuong.Value = 1;
                    txtDonGia.Text = "0";
                    MessageBox.Show("Xóa chi tiết hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại trong chi tiết hóa đơn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi chọn một dòng trong DataGridView hóa đơn.
        private void DgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvHoaDon.Rows[e.RowIndex];
                    txtMaHoaDon.Text = row.Cells["MaHoaDon"].Value?.ToString() ?? "";
                    cbTenKhachHang.SelectedItem = row.Cells["TenKhachHang"].Value?.ToString() ?? "";
                    cbTenNhanVien.SelectedItem = row.Cells["HoTen"].Value?.ToString() ?? "";
                    if (row.Cells["NgayLapHoaDon"].Value != null && DateTime.TryParse(row.Cells["NgayLapHoaDon"].Value.ToString(), out var ngayLap))
                        dtpNgayLap.Value = ngayLap;
                    cbTenKhuyenMai.SelectedItem = row.Cells["TenKhuyenMai"].Value?.ToString() ?? "";
                    txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
                    txtTongTien.Text = row.Cells["TongTien"].Value?.ToString() ?? "0";
                    txtGhiChuHD.Text = row.Cells["GhiChu"].Value?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(txtMaHoaDon.Text))
                    {
                        _chiTietHoaDonList = _hoaDonBLL.LayChiTietHoaDon(txtMaHoaDon.Text) ?? new List<ChiTietHoaDonDTO>();
                        if (_chiTietHoaDonList.Count == 0)
                        {
                            MessageBox.Show($"Không có chi tiết hóa đơn cho mã hóa đơn {txtMaHoaDon.Text}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        CapNhatChiTietHoaDon();
                        CapNhatTongTien();
                    }
                    else
                    {
                        MessageBox.Show("Mã hóa đơn không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _chiTietHoaDonList.Clear();
                        CapNhatChiTietHoaDon();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn hóa đơn: {ex.Message}\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi chọn một dòng trong DataGridView chi tiết hóa đơn.
        private void DgvChiTietHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvChiTietHoaDon.Rows[e.RowIndex];
                    _selectedMaSanPham = row.Cells["MaSanPham"].Value?.ToString();
                    var tenSanPham = _sanPhamDict.FirstOrDefault(x => x.Value == _selectedMaSanPham).Key;
                    cbTenSanPham.SelectedItem = tenSanPham;
                    var chiTiet = _chiTietHoaDonList.Find(ct => ct.MaSanPham == _selectedMaSanPham);
                    if (chiTiet != null)
                    {
                        nudSoLuong.Value = chiTiet.SoLuong;
                        txtDonGia.Text = chiTiet.DonGia.ToString("N0");

                        // Cập nhật Maximum cho nudSoLuong dựa trên số lượng tồn kho
                        var soLuongTon = _hoaDonBLL.LaySoLuongTonSanPham(_selectedMaSanPham);
                        nudSoLuong.Maximum = soLuongTon > 0 ? soLuongTon : 1; // Đảm bảo Maximum ít nhất là 1
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật DataGridView chi tiết hóa đơn.
        private void CapNhatChiTietHoaDon()
        {
            try
            {
                dgvChiTietHoaDon.DataSource = null;
                if (_chiTietHoaDonList != null && _chiTietHoaDonList.Any())
                {
                    dgvChiTietHoaDon.DataSource = _chiTietHoaDonList;
                    if (dgvChiTietHoaDon.Columns.Contains("MaHoaDon"))
                        dgvChiTietHoaDon.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";
                    if (dgvChiTietHoaDon.Columns.Contains("MaSanPham"))
                        dgvChiTietHoaDon.Columns["MaSanPham"].HeaderText = "Mã Sản Phẩm";
                    if (dgvChiTietHoaDon.Columns.Contains("TenSanPham"))
                        dgvChiTietHoaDon.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                    if (dgvChiTietHoaDon.Columns.Contains("SoLuong"))
                        dgvChiTietHoaDon.Columns["SoLuong"].HeaderText = "Số Lượng";
                    if (dgvChiTietHoaDon.Columns.Contains("DonGia"))
                        dgvChiTietHoaDon.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dgvChiTietHoaDon.Columns.Contains("ThanhTien"))
                        dgvChiTietHoaDon.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                }
                else
                {
                    dgvChiTietHoaDon.DataSource = new List<ChiTietHoaDonDTO>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật tổng tiền dựa trên danh sách chi tiết hóa đơn và khuyến mãi.
        private void CapNhatTongTien()
        {
            try
            {
                var tongTien = _chiTietHoaDonList.Sum(ct => ct.ThanhTien);

                if (cbTenKhuyenMai.SelectedIndex >= 0 && _khuyenMaiDict.ContainsKey(cbTenKhuyenMai.SelectedItem.ToString()))
                {
                    var maKhuyenMai = _khuyenMaiDict[cbTenKhuyenMai.SelectedItem.ToString()];
                    var phanTramKhuyenMai = _hoaDonBLL.LayPhanTramKhuyenMai(maKhuyenMai);
                    var giamGia = tongTien * (phanTramKhuyenMai / 100);
                    tongTien -= giamGia;
                }

                txtTongTien.Text = tongTien.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tính tổng tiền: {ex.Message}\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa dữ liệu trên form để nhập mới.
        private void XoaDuLieuForm()
        {
            try
            {
                txtMaHoaDon.Text = _hoaDonBLL.TaoMaHoaDon();
                cbTenKhachHang.SelectedIndex = -1;
                cbTenNhanVien.SelectedIndex = -1;
                dtpNgayLap.Value = DateTime.Now;
                cbTenKhuyenMai.SelectedIndex = -1;
                txtDiaChi.Text = "";
                txtTongTien.Text = "0";
                txtGhiChuHD.Text = "";
                cbTenSanPham.SelectedIndex = -1;
                nudSoLuong.Value = 1;
                txtDonGia.Text = "0";
                _chiTietHoaDonList.Clear();
                _selectedMaSanPham = null;
                CapNhatChiTietHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi thay đổi lựa chọn sản phẩm trong ComboBox.
        private void CbTenSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbTenSanPham.SelectedIndex >= 0)
                {
                    var tenSanPham = cbTenSanPham.SelectedItem.ToString();
                    var maSanPham = _sanPhamDict[tenSanPham];
                    var donGia = _hoaDonBLL.LayDonGiaSanPham(maSanPham);
                    txtDonGia.Text = donGia.ToString("N0");

                    // Lấy số lượng tồn kho và thiết lập Maximum cho nudSoLuong
                    var soLuongTon = _hoaDonBLL.LaySoLuongTonSanPham(maSanPham);
                    nudSoLuong.Maximum = soLuongTon > 0 ? soLuongTon : 1; // Đảm bảo Maximum ít nhất là 1
                    nudSoLuong.Value = 1; // Đặt lại giá trị mặc định về 1 khi chọn sản phẩm mới

                    CapNhatThanhTien();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi thay đổi số lượng sản phẩm.
        private void NudSoLuong_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CapNhatThanhTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi số lượng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật thành tiền khi thay đổi số lượng hoặc đơn giá.
        private void CapNhatThanhTien()
        {
            if (decimal.TryParse(txtDonGia.Text, out var donGia))
            {
                var soLuong = (int)nudSoLuong.Value;
                var thanhTien = donGia * soLuong;
            }
        }

        // Xử lý sự kiện khi thay đổi lựa chọn khuyến mãi trong ComboBox.
        private void CbTenKhuyenMai_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CapNhatTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn khuyến mãi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xuất hóa đơn ra file PDF.
        private void btnXuatFilePDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaHoaDon.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn để xuất PDF!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CapNhatTongTien();

                if (_chiTietHoaDonList == null || _chiTietHoaDonList.Count == 0)
                {
                    MessageBox.Show($"Không có chi tiết hóa đơn cho mã hóa đơn {txtMaHoaDon.Text.Trim()}!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"HoaDon_{txtMaHoaDon.Text.Trim()}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        var document = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter.GetInstance(document, fs);
                        document.Open();

                        var baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        var titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);
                        var infoFont = new iTextSharp.text.Font(baseFont, 12);
                        var cellFont = new iTextSharp.text.Font(baseFont, 10);

                        var title = new Paragraph("PHIẾU HÓA ĐƠN", titleFont);
                        title.Alignment = Element.ALIGN_CENTER;
                        document.Add(title);

                        document.Add(new Paragraph($"Mã Hóa Đơn: {txtMaHoaDon.Text.Trim()}", infoFont));
                        document.Add(new Paragraph($"Ngày Lập: {dtpNgayLap.Value:dd/MM/yyyy}", infoFont));
                        document.Add(new Paragraph($"Khách Hàng: {cbTenKhachHang.SelectedItem?.ToString() ?? "N/A"}", infoFont));
                        document.Add(new Paragraph($"Nhân Viên: {cbTenNhanVien.SelectedItem?.ToString() ?? "N/A"}", infoFont));
                        document.Add(new Paragraph($"Khuyến Mãi: {cbTenKhuyenMai.SelectedItem?.ToString() ?? "Không có"}", infoFont));
                        document.Add(new Paragraph($"Địa Chỉ: {txtDiaChi.Text ?? "N/A"}", infoFont));
                        document.Add(new Paragraph($"Ghi Chú: {txtGhiChuHD.Text ?? "Không có"}", infoFont));
                        document.Add(new Paragraph($"Tổng Tiền: {txtTongTien.Text} VNĐ", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD)));

                        var spacing = new Paragraph(" ") { SpacingBefore = 10f };
                        document.Add(spacing);

                        var table = new PdfPTable(4) { WidthPercentage = 100 };
                        table.AddCell(new PdfPCell(new Phrase("Tên Sản Phẩm", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Số Lượng", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Đơn Giá", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Thành Tiền", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));

                        foreach (var chiTiet in _chiTietHoaDonList)
                        {
                            table.AddCell(new PdfPCell(new Phrase(chiTiet.TenSanPham ?? "N/A", cellFont)));
                            table.AddCell(new PdfPCell(new Phrase(chiTiet.SoLuong.ToString(), cellFont)));
                            table.AddCell(new PdfPCell(new Phrase(chiTiet.DonGia.ToString("N0") + " VNĐ", cellFont)));
                            table.AddCell(new PdfPCell(new Phrase(chiTiet.ThanhTien.ToString("N0") + " VNĐ", cellFont)));
                        }

                        document.Add(table);
                        document.Close();

                        MessageBox.Show("In hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file PDF: {ex.Message}\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatEXCEL_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy toàn bộ danh sách hóa đơn
                var hoaDonList = _hoaDonBLL.LayDanhSachHoaDon();
                if (hoaDonList == null || hoaDonList.Count == 0)
                {
                    MessageBox.Show("Không có hóa đơn nào để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"DanhSachHoaDon_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Thêm BOM để hỗ trợ tiếng Việt trong Excel
                        writer.Write("\uFEFF");

                        // Tiêu đề chính
                        writer.WriteLine("\"DANH SÁCH HÓA ĐƠN\"");
                        writer.WriteLine();

                        // Tiêu đề cột
                        writer.WriteLine("\"Mã Hóa Đơn\",\"Ngày Lập\",\"Tên Khách Hàng\",\"Tên Nhân Viên\",\"Tên Khuyến Mãi\",\"Địa Chỉ\",\"Ghi Chú\",\"Tổng Tiền\"");

                        // Ghi danh sách hóa đơn
                        foreach (var hoaDon in hoaDonList)
                        {
                            var maHoaDon = $"\"{hoaDon.MaHoaDon}\"";
                            var ngayLap = $"\"{hoaDon.NgayLapHoaDon.ToString("dd/MM/yyyy") ?? "N/A"}\"";
                            var tenKhachHang = $"\"{hoaDon.TenKhachHang?.Replace("\"", "\"\"") ?? "N/A"}\"";
                            var tenNhanVien = $"\"{hoaDon.HoTen?.Replace("\"", "\"\"") ?? "N/A"}\"";
                            var tenKhuyenMai = $"\"{hoaDon.TenKhuyenMai?.Replace("\"", "\"\"") ?? "Không có"}\"";
                            var diaChi = $"\"{hoaDon.DiaChi?.Replace("\"", "\"\"") ?? "N/A"}\"";
                            var ghiChu = $"\"{hoaDon.GhiChu?.Replace("\"", "\"\"") ?? "N/A"}\"";
                            var tongTien = $"\"{hoaDon.TongTien:N0}\""; // Chỉ số nguyên, không có VNĐ

                            writer.WriteLine($"{maHoaDon},{ngayLap},{tenKhachHang},{tenNhanVien},{tenKhuyenMai},{diaChi},{ghiChu},{tongTien}");
                        }
                    }

                    MessageBox.Show("Xuất danh sách hóa đơn ra CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file CSV: {ex.Message}\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}