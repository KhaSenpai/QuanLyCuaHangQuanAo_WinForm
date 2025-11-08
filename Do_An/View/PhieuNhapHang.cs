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
    // Form quản lý phiếu nhập hàng, hiển thị và thao tác với phiếu nhập và chi tiết phiếu nhập.
    public partial class PhieuNhapHang : Form
    {
        private readonly TaiKhoanDTO _taiKhoan;
        private readonly PhieuNhapBLL _phieuNhapBLL;
        private List<ChiTietPhieuNhapDTO> _chiTietPhieuNhapList = new List<ChiTietPhieuNhapDTO>();
        private Dictionary<string, string> _nhaCungCapDict = new Dictionary<string, string>(); // Ánh xạ tên NCC -> mã NCC
        private Dictionary<string, string> _nhanVienDict = new Dictionary<string, string>(); // Ánh xạ tên NV -> mã NV
        private Dictionary<string, string> _sanPhamDict = new Dictionary<string, string>(); // Ánh xạ tên SP -> mã SP
        private string _selectedMaSanPham; // Mã sản phẩm được chọn trong dgvChiTietPhieuNhap

        public PhieuNhapHang(TaiKhoanDTO taiKhoan = null)
        {
            _phieuNhapBLL = new PhieuNhapBLL();
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
                btnThemPN.Visible = true;
                btnSuaPN.Visible = true;
                btnXoaPN.Visible = true;
                btnThemCTPNH.Visible = true;
                btnXoaCTPN.Visible = true;
                btnXoaPN.Visible = true ;
                btnXuatEXCEL.Visible = true;
                return;
            }

            switch (_taiKhoan.QuyenTruyCap)
            {
                case "Admin":
                    break;

                case "Manager":
                    btnXoaPN.Visible = false;
                    break;

                case "User":
                    btnThemPN.Visible = false;
                    btnSuaPN.Visible = false;
                    btnXoaPN.Visible = false;
                    btnThemCTPNH.Visible = false;
                    btnXoaCTPN.Visible = false;
                    btnXoaPN.Visible = false;
                    btnXuatEXCEL.Visible=false;
                    break;

                default:
                    btnThemPN.Visible = false;
                    btnSuaPN.Visible = false;
                    btnXoaPN.Visible =false;
                    btnThemCTPNH.Visible = false;
                    btnXoaCTPN.Visible = false;
                    btnXoaPN.Visible = false;
                    btnXuatEXCEL.Visible = false;
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
            txtMaPhieuNhap.MaxLength = 20;
            txtDiaChi.MaxLength = 200;
            txtGhiChu.MaxLength = 200;
            txtMaPhieuNhap.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtThanhTienCTPN.ReadOnly = true;
            txtTongTien.ReadOnly = true;

            // Cấu hình NumericUpDown
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 100;

            dtpNgayLap.Enabled = false;
            // Gán sự kiện
            cbTenSanPham.SelectedIndexChanged += CbTenSanPham_SelectedIndexChanged;
            nudSoLuong.ValueChanged += NudSoLuong_ValueChanged;
            dgvPhieuNhap.CellClick += DgvPhieuNhap_CellClick;
            dgvChiTietPhieuNhap.CellClick += DgvChiTietPhieuNhap_CellClick;
        }

        // Tải dữ liệu ban đầu cho form.
        private void TaiDuLieu()
        {
            try
            {
                TaiDanhSachPhieuNhap();
                TaiComboBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tải danh sách phiếu nhập vào DataGridView.
        private void TaiDanhSachPhieuNhap()
        {
            try
            {
                var phieuNhapList = _phieuNhapBLL.LayDanhSachPhieuNhap();
                dgvPhieuNhap.DataSource = phieuNhapList;
                CauHinhDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cấu hình các cột của DataGridView phiếu nhập.
        private void CauHinhDataGridView()
        {
            if (dgvPhieuNhap.Columns.Contains("MaPhieuNhap"))
                dgvPhieuNhap.Columns["MaPhieuNhap"].HeaderText = "Mã Phiếu Nhập";
            if (dgvPhieuNhap.Columns.Contains("MaNCC"))
                dgvPhieuNhap.Columns["MaNCC"].HeaderText = "Mã Nhà Cung Cấp";
            if (dgvPhieuNhap.Columns.Contains("MaNhanVien"))
                dgvPhieuNhap.Columns["MaNhanVien"].HeaderText = "Mã Nhân Viên";
            if (dgvPhieuNhap.Columns.Contains("NgayLapPhieuNhap"))
                dgvPhieuNhap.Columns["NgayLapPhieuNhap"].HeaderText = "Ngày Lập";
            if (dgvPhieuNhap.Columns.Contains("TongTien"))
                dgvPhieuNhap.Columns["TongTien"].HeaderText = "Tổng Tiền";
            if (dgvPhieuNhap.Columns.Contains("DiaChi"))
                dgvPhieuNhap.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            if (dgvPhieuNhap.Columns.Contains("GhiChu"))
                dgvPhieuNhap.Columns["GhiChu"].HeaderText = "Ghi Chú";
        }

        // Tải dữ liệu vào các ComboBox.
        private void TaiComboBoxes()
        {
            try
            {
                // Tải nhà cung cấp
                _nhaCungCapDict = _phieuNhapBLL.LayDanhSachTenNhaCungCap();
                cboTenNhaCungCap.Items.Clear();
                cboTenNhaCungCap.Items.AddRange(_nhaCungCapDict.Keys.ToArray());
                cboTenNhaCungCap.SelectedIndex = -1;

                // Tải nhân viên
                _nhanVienDict = _phieuNhapBLL.LayDanhSachTenNhanVien();
                cbTenNhanVien.Items.Clear();
                cbTenNhanVien.Items.AddRange(_nhanVienDict.Keys.ToArray());
                cbTenNhanVien.SelectedIndex = -1;

                // Tải sản phẩm
                _sanPhamDict = _phieuNhapBLL.LayDanhSachTenSanPham();
                cbTenSanPham.Items.Clear();
                cbTenSanPham.Items.AddRange(_sanPhamDict.Keys.ToArray());
                cbTenSanPham.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách combobox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thêm phiếu nhập mới.
        private void btnThemPN_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTenNhaCungCap.SelectedIndex < 0 || cbTenNhanVien.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp và nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_chiTietPhieuNhapList.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một chi tiết phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtTongTien.Text, out var tongTien))
                {
                    MessageBox.Show("Tổng tiền phải là số hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var maPhieuNhap = txtMaPhieuNhap.Text.Trim();
                if (string.IsNullOrEmpty(maPhieuNhap))
                {
                    MessageBox.Show("Mã phiếu nhập không được để trống! Vui lòng làm mới để sinh mã mới.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var phieuNhap = new PhieuNhapDTO
                {
                    MaPhieuNhap = maPhieuNhap,
                    MaNCC = _nhaCungCapDict[cboTenNhaCungCap.SelectedItem.ToString()],
                    MaNhanVien = _nhanVienDict[cbTenNhanVien.SelectedItem.ToString()],
                    NgayLapPhieuNhap = dtpNgayLap.Value,
                    TongTien = tongTien,
                    DiaChi = string.IsNullOrEmpty(txtDiaChi.Text.Trim()) ? null : txtDiaChi.Text.Trim(),
                    GhiChu = string.IsNullOrEmpty(txtGhiChu.Text.Trim()) ? null : txtGhiChu.Text.Trim()
                };

                if (_phieuNhapBLL.ThemPhieuNhap(phieuNhap))
                {
                    foreach (var chiTiet in _chiTietPhieuNhapList)
                    {
                        chiTiet.MaPhieuNhap = maPhieuNhap;
                        if (!_phieuNhapBLL.ThemChiTietPhieuNhap(chiTiet))
                        {
                            MessageBox.Show($"Lỗi khi thêm chi tiết phiếu nhập cho sản phẩm mã {chiTiet.MaSanPham}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachPhieuNhap();
                    XoaDuLieuForm();
                }
                else
                {
                    MessageBox.Show("Thêm phiếu nhập thất bại! Vui lòng kiểm tra lại dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật thông tin phiếu nhập.
        private void btnSuaPN_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaPhieuNhap.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboTenNhaCungCap.SelectedIndex < 0 || cbTenNhanVien.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp và nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_chiTietPhieuNhapList.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một chi tiết phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtTongTien.Text, out var tongTien))
                {
                    MessageBox.Show("Tổng tiền phải là số hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var phieuNhap = new PhieuNhapDTO
                {
                    MaPhieuNhap = txtMaPhieuNhap.Text.Trim(),
                    MaNCC = _nhaCungCapDict[cboTenNhaCungCap.SelectedItem.ToString()],
                    MaNhanVien = _nhanVienDict[cbTenNhanVien.SelectedItem.ToString()],
                    NgayLapPhieuNhap = dtpNgayLap.Value,
                    TongTien = tongTien,
                    DiaChi = string.IsNullOrEmpty(txtDiaChi.Text.Trim()) ? null : txtDiaChi.Text.Trim(),
                    GhiChu = string.IsNullOrEmpty(txtGhiChu.Text.Trim()) ? null : txtGhiChu.Text.Trim()
                };

                if (_phieuNhapBLL.CapNhatPhieuNhap(phieuNhap))
                {
                    if (!_phieuNhapBLL.XoaTatCaChiTietPhieuNhap(phieuNhap.MaPhieuNhap))
                    {
                        MessageBox.Show("Lỗi khi xóa chi tiết phiếu nhập cũ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (var chiTiet in _chiTietPhieuNhapList)
                    {
                        chiTiet.MaPhieuNhap = phieuNhap.MaPhieuNhap;
                        if (!_phieuNhapBLL.ThemChiTietPhieuNhap(chiTiet))
                        {
                            MessageBox.Show($"Lỗi khi thêm chi tiết phiếu nhập mới cho sản phẩm mã {chiTiet.MaSanPham}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    MessageBox.Show("Cập nhật phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachPhieuNhap();
                    XoaDuLieuForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật phiếu nhập thất bại! Vui lòng kiểm tra lại dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa phiếu nhập.
        private void btnXoaPN_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaPhieuNhap.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu nhập này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_phieuNhapBLL.XoaPhieuNhap(txtMaPhieuNhap.Text.Trim()))
                    {
                        MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaiDanhSachPhieuNhap();
                        XoaDuLieuForm();
                    }
                    else
                    {
                        MessageBox.Show("Xóa phiếu nhập thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Làm mới form và danh sách phiếu nhập.
        private void btnLamMoiPN_Click(object sender, EventArgs e)
        {
            try
            {
                XoaDuLieuForm();
                TaiDanhSachPhieuNhap();
                MessageBox.Show("Đã làm mới danh sách phiếu nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thêm chi tiết phiếu nhập vào danh sách tạm thời.
        private void btnThemCTPN_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbTenSanPham.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtDonGia.Text, out var donGia) || donGia <= 0)
                {
                    MessageBox.Show("Đơn giá phải là số lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var tenSanPham = cbTenSanPham.SelectedItem.ToString();
                var maSanPham = _sanPhamDict[tenSanPham];
                var soLuong = (int)nudSoLuong.Value;
                var thanhTien = donGia * soLuong;

                var existingChiTiet = _chiTietPhieuNhapList.Find(ct => ct.MaSanPham == maSanPham);
                if (existingChiTiet != null)
                {
                    existingChiTiet.SoLuong += soLuong;
                    existingChiTiet.ThanhTien = existingChiTiet.SoLuong * donGia;
                }
                else
                {
                    _chiTietPhieuNhapList.Add(new ChiTietPhieuNhapDTO
                    {
                        MaPhieuNhap = txtMaPhieuNhap.Text.Trim(),
                        MaSanPham = maSanPham,
                        SoLuong = soLuong,
                        DonGia = donGia,
                        ThanhTien = thanhTien
                    });
                }

                CapNhatChiTietPhieuNhap();
                CapNhatTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa chi tiết phiếu nhập khỏi danh sách tạm thời.
        private void btnXoaCTPN_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedMaSanPham))
                {
                    MessageBox.Show("Vui lòng chọn một sản phẩm trong chi tiết phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var chiTiet = _chiTietPhieuNhapList.Find(ct => ct.MaSanPham == _selectedMaSanPham);
                if (chiTiet != null)
                {
                    _chiTietPhieuNhapList.Remove(chiTiet);
                    CapNhatChiTietPhieuNhap();
                    CapNhatTongTien();
                    _selectedMaSanPham = null;
                    cbTenSanPham.SelectedIndex = -1;
                    nudSoLuong.Value = 1;
                    txtDonGia.Text = "0";
                    txtThanhTienCTPN.Text = "0";
                    MessageBox.Show("Xóa chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại trong chi tiết phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi chọn một dòng trong DataGridView phiếu nhập.
        private void DgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvPhieuNhap.Rows[e.RowIndex];
                    txtMaPhieuNhap.Text = row.Cells["MaPhieuNhap"].Value?.ToString() ?? "";
                    cboTenNhaCungCap.SelectedItem = _nhaCungCapDict.FirstOrDefault(x => x.Value == (row.Cells["MaNCC"].Value?.ToString() ?? "")).Key;
                    cbTenNhanVien.SelectedItem = _nhanVienDict.FirstOrDefault(x => x.Value == (row.Cells["MaNhanVien"].Value?.ToString() ?? "")).Key;
                    dtpNgayLap.Value = row.Cells["NgayLapPhieuNhap"].Value != null && DateTime.TryParse(row.Cells["NgayLapPhieuNhap"].Value.ToString(), out var ngayLap) ? ngayLap : DateTime.Now;
                    txtTongTien.Text = row.Cells["TongTien"].Value?.ToString() ?? "0";
                    txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
                    txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString() ?? "";

                    _chiTietPhieuNhapList = _phieuNhapBLL.LayChiTietPhieuNhap(txtMaPhieuNhap.Text) ?? new List<ChiTietPhieuNhapDTO>();
                    if (_chiTietPhieuNhapList.Count == 0)
                    {
                        MessageBox.Show($"Không có chi tiết phiếu nhập cho mã {txtMaPhieuNhap.Text}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    CapNhatChiTietPhieuNhap();
                    CapNhatTongTien();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi chọn một dòng trong DataGridView chi tiết phiếu nhập.
        private void DgvChiTietPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvChiTietPhieuNhap.Rows[e.RowIndex];
                    _selectedMaSanPham = row.Cells["MaSanPham"].Value?.ToString();
                    var tenSanPham = _sanPhamDict.FirstOrDefault(x => x.Value == _selectedMaSanPham).Key;
                    cbTenSanPham.SelectedItem = tenSanPham;
                    var chiTiet = _chiTietPhieuNhapList.Find(ct => ct.MaSanPham == _selectedMaSanPham);
                    if (chiTiet != null)
                    {
                        nudSoLuong.Value = chiTiet.SoLuong;
                        txtDonGia.Text = chiTiet.DonGia.ToString("N0");
                        txtThanhTienCTPN.Text = chiTiet.ThanhTien.ToString("N0");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật DataGridView chi tiết phiếu nhập.
        private void CapNhatChiTietPhieuNhap()
        {
            try
            {
                dgvChiTietPhieuNhap.DataSource = null;
                if (_chiTietPhieuNhapList.Any())
                {
                    dgvChiTietPhieuNhap.DataSource = _chiTietPhieuNhapList;
                    if (dgvChiTietPhieuNhap.Columns.Contains("MaPhieuNhap"))
                        dgvChiTietPhieuNhap.Columns["MaPhieuNhap"].HeaderText = "Mã Phiếu Nhập";
                    if (dgvChiTietPhieuNhap.Columns.Contains("MaSanPham"))
                        dgvChiTietPhieuNhap.Columns["MaSanPham"].HeaderText = "Mã Sản Phẩm";
                    if (dgvChiTietPhieuNhap.Columns.Contains("SoLuong"))
                        dgvChiTietPhieuNhap.Columns["SoLuong"].HeaderText = "Số Lượng";
                    if (dgvChiTietPhieuNhap.Columns.Contains("DonGia"))
                        dgvChiTietPhieuNhap.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dgvChiTietPhieuNhap.Columns.Contains("ThanhTien"))
                        dgvChiTietPhieuNhap.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                }
                else
                {
                    dgvChiTietPhieuNhap.DataSource = new List<ChiTietPhieuNhapDTO>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật chi tiết phiếu nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật tổng tiền dựa trên danh sách chi tiết phiếu nhập.
        private void CapNhatTongTien()
        {
            try
            {
                var tongTien = _chiTietPhieuNhapList.Sum(ct => ct.ThanhTien);
                txtTongTien.Text = tongTien.ToString("N0");
                txtThanhTienCTPN.Text = tongTien.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tính tổng tiền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa dữ liệu trên form để nhập mới.
        private void XoaDuLieuForm()
        {
            try
            {
                txtMaPhieuNhap.Text = _phieuNhapBLL.SinhMaPhieuNhap();
                cboTenNhaCungCap.SelectedIndex = -1;
                cbTenNhanVien.SelectedIndex = -1;
                dtpNgayLap.Value = DateTime.Now;
                txtDiaChi.Text = "";
                txtGhiChu.Text = "";
                txtTongTien.Text = "0";
                txtThanhTienCTPN.Text = "0";
                cbTenSanPham.SelectedIndex = -1;
                nudSoLuong.Value = 1;
                txtDonGia.Text = "0";
                _chiTietPhieuNhapList.Clear();
                _selectedMaSanPham = null;
                CapNhatChiTietPhieuNhap();
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
                    var donGia = _phieuNhapBLL.LayDonGiaNhap(maSanPham);
                    txtDonGia.Text = donGia.ToString("N0");
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
                txtThanhTienCTPN.Text = (donGia * soLuong).ToString("N0");
            }
        }

        // Xuất phiếu nhập ra file PDF.
        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaPhieuNhap.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập để xuất PDF!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_chiTietPhieuNhapList.Count == 0)
                {
                    MessageBox.Show($"Không có chi tiết phiếu nhập cho mã {txtMaPhieuNhap.Text}!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"PhieuNhap_{txtMaPhieuNhap.Text.Trim()}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
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

                        var title = new Paragraph("PHIẾU NHẬP HÀNG", titleFont);
                        title.Alignment = Element.ALIGN_CENTER;
                        document.Add(title);

                        document.Add(new Paragraph($"Mã Phiếu Nhập: {txtMaPhieuNhap.Text.Trim()}", infoFont));
                        document.Add(new Paragraph($"Ngày Lập: {dtpNgayLap.Value:dd/MM/yyyy}", infoFont));
                        document.Add(new Paragraph($"Nhà Cung Cấp: {cboTenNhaCungCap.SelectedItem?.ToString() ?? "N/A"}", infoFont));
                        document.Add(new Paragraph($"Nhân Viên: {cbTenNhanVien.SelectedItem?.ToString() ?? "N/A"}", infoFont));
                        document.Add(new Paragraph($"Địa Chỉ: {txtDiaChi.Text ?? "N/A"}", infoFont));
                        document.Add(new Paragraph($"Ghi Chú: {txtGhiChu.Text ?? "N/A"}", infoFont));
                        document.Add(new Paragraph($"Tổng Tiền: {txtTongTien.Text} VNĐ", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD)));

                        var spacing = new Paragraph(" ") { SpacingBefore = 10f };
                        document.Add(spacing);

                        var table = new PdfPTable(4) { WidthPercentage = 100 };
                        table.AddCell(new PdfPCell(new Phrase("Tên Sản Phẩm", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Số Lượng", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Đơn Giá", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));
                        table.AddCell(new PdfPCell(new Phrase("Thành Tiền", new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD))));

                        foreach (var chiTiet in _chiTietPhieuNhapList)
                        {
                            var tenSanPham = _sanPhamDict.FirstOrDefault(x => x.Value == chiTiet.MaSanPham).Key ?? "N/A";
                            table.AddCell(new PdfPCell(new Phrase(tenSanPham, cellFont)));
                            table.AddCell(new PdfPCell(new Phrase(chiTiet.SoLuong.ToString(), cellFont)));
                            table.AddCell(new PdfPCell(new Phrase(chiTiet.DonGia.ToString("N0") + " VNĐ", cellFont)));
                            table.AddCell(new PdfPCell(new Phrase(chiTiet.ThanhTien.ToString("N0") + " VNĐ", cellFont)));
                        }

                        document.Add(table);
                        document.Close();

                        MessageBox.Show("Xuất phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnXuatEXCEL_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy toàn bộ danh sách phiếu nhập
                var phieuNhapList = _phieuNhapBLL.LayDanhSachPhieuNhap();
                if (phieuNhapList == null || phieuNhapList.Count == 0)
                {
                    MessageBox.Show("Không có phiếu nhập nào để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"DanhSachPhieuNhap_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Thêm BOM để hỗ trợ tiếng Việt trong Excel
                        writer.Write("\uFEFF");

                        // Tiêu đề chính
                        writer.WriteLine("\"DANH SÁCH PHIẾU NHẬP HÀNG\"");
                        writer.WriteLine();

                        // Tiêu đề cột
                        writer.WriteLine("\"Mã Phiếu Nhập\",\"Ngày Lập\",\"Nhà Cung Cấp\",\"Nhân Viên\",\"Địa Chỉ\",\"Ghi Chú\",\"Tổng Tiền\"");

                        // Ghi danh sách phiếu nhập
                        foreach (var phieuNhap in phieuNhapList)
                        {
                            var maPhieuNhap = $"\"{phieuNhap.MaPhieuNhap}\"";
                            var ngayLap = $"\"{phieuNhap.NgayLapPhieuNhap?.ToString("dd/MM/yyyy") ?? "N/A"}\"";
                            var tenNCC = $"\"{_nhaCungCapDict.FirstOrDefault(x => x.Value == phieuNhap.MaNCC).Key ?? "N/A"}\"";
                            var tenNV = $"\"{_nhanVienDict.FirstOrDefault(x => x.Value == phieuNhap.MaNhanVien).Key ?? "N/A"}\"";
                            var diaChi = $"\"{phieuNhap.DiaChi?.Replace("\"", "\"\"") ?? "N/A"}\"";
                            var ghiChu = $"\"{phieuNhap.GhiChu?.Replace("\"", "\"\"") ?? "N/A"}\"";
                            var tongTien = $"\"{phieuNhap.TongTien:N0}\""; // Chỉ số nguyên, không có VNĐ

                            writer.WriteLine($"{maPhieuNhap},{ngayLap},{tenNCC},{tenNV},{diaChi},{ghiChu},{tongTien}");
                        }
                    }

                    MessageBox.Show("Xuất danh sách phiếu nhập ra CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file CSV: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}