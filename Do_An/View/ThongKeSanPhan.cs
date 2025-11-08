using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class ThongKeSanPhan : Form
    {
        // Đối tượng BLL để xử lý logic nghiệp vụ
        private readonly ThongKeSanPhamBLL _thongKeBLL;

        public ThongKeSanPhan()
        {
            InitializeComponent();
            _thongKeBLL = new ThongKeSanPhamBLL();
            InitializeDataGridViews();
            LoadData();
            // Gắn sự kiện CellFormatting cho các DataGridView
            dgvSanPham.CellFormatting += dgvSanPham_CellFormatting;
            dgvSanPhamTonIt.CellFormatting += dgvSanPhamTonIt_CellFormatting;
            dgvSanPhamMoiNhap.CellFormatting += dgvSanPhamMoiNhap_CellFormatting;
            dgvSanPhamBanChay.CellFormatting += dgvSanPhamBanChay_CellFormatting;
        }

        // Khởi tạo các DateTimePicker cho khoảng thời gian
        // Khởi tạo các cột cho DataGridView theo yêu cầu
        private void InitializeDataGridViews()
        {
            // Cấu hình cột cho dgvSanPham
            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.Columns.Clear();
            dgvSanPham.DefaultCellStyle = null; // Xóa định dạng mặc định
            dgvSanPham.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "MaSP", DataPropertyName = "MaSP", HeaderText = "Mã SP" },
                new DataGridViewTextBoxColumn { Name = "TenSP", DataPropertyName = "TenSP", HeaderText = "Tên SP" },
                new DataGridViewTextBoxColumn { Name = "SoLuongTon", DataPropertyName = "SoLuongTon", HeaderText = "Số lượng" },
                new DataGridViewTextBoxColumn { Name = "DonGiaBan", DataPropertyName = "DonGiaBan", HeaderText = "Đơn giá" },
                new DataGridViewTextBoxColumn { Name = "MauSac", DataPropertyName = "MauSac", HeaderText = "Màu sắc" },
                new DataGridViewTextBoxColumn { Name = "KichCo", DataPropertyName = "KichCo", HeaderText = "Kích cỡ" },
                new DataGridViewTextBoxColumn { Name = "TenThuongHieu", DataPropertyName = "TenThuongHieu", HeaderText = "Thương hiệu" },
                new DataGridViewTextBoxColumn { Name = "TenLoaiSP", DataPropertyName = "TenLoaiSP", HeaderText = "Loại SP" }
            });

            // Cấu hình cột cho dgvSanPhamTonIt
            dgvSanPhamTonIt.AutoGenerateColumns = false;
            dgvSanPhamTonIt.Columns.Clear();
            dgvSanPhamTonIt.DefaultCellStyle = null; // Xóa định dạng mặc định
            dgvSanPhamTonIt.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "MaSP", DataPropertyName = "MaSP", HeaderText = "Mã SP" },
                new DataGridViewTextBoxColumn { Name = "TenSP", DataPropertyName = "TenSP", HeaderText = "Tên SP" },
                new DataGridViewTextBoxColumn { Name = "DonGiaBan", DataPropertyName = "DonGiaBan", HeaderText = "Đơn giá" },
                new DataGridViewTextBoxColumn { Name = "MauSac", DataPropertyName = "MauSac", HeaderText = "Màu sắc" },
                new DataGridViewTextBoxColumn { Name = "KichCo", DataPropertyName = "KichCo", HeaderText = "Kích cỡ" },
                new DataGridViewTextBoxColumn { Name = "SoLuongTon", DataPropertyName = "SoLuongTon", HeaderText = "Số lượng" },
                new DataGridViewTextBoxColumn { Name = "TrangThai", DataPropertyName = "TrangThai", HeaderText = "Trạng thái" }
            });

            // Cấu hình cột cho dgvSanPhamMoiNhap
            dgvSanPhamMoiNhap.AutoGenerateColumns = false;
            dgvSanPhamMoiNhap.Columns.Clear();
            dgvSanPhamMoiNhap.DefaultCellStyle = null; // Xóa định dạng mặc định
            dgvSanPhamMoiNhap.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "MaSP", DataPropertyName = "MaSP", HeaderText = "Mã SP" },
                new DataGridViewTextBoxColumn { Name = "TenSP", DataPropertyName = "TenSP", HeaderText = "Tên SP" },
                new DataGridViewTextBoxColumn { Name = "SoLuongTon", DataPropertyName = "SoLuongTon", HeaderText = "Số lượng tồn" },
                new DataGridViewTextBoxColumn { Name = "DonGiaBan", DataPropertyName = "DonGiaBan", HeaderText = "Đơn giá" },
                new DataGridViewTextBoxColumn { Name = "MauSac", DataPropertyName = "MauSac", HeaderText = "Màu sắc" },
                new DataGridViewTextBoxColumn { Name = "KichCo", DataPropertyName = "KichCo", HeaderText = "Kích cỡ" },
                new DataGridViewTextBoxColumn { Name = "TenThuongHieu", DataPropertyName = "TenThuongHieu", HeaderText = "Thương hiệu" },
                new DataGridViewTextBoxColumn { Name = "TenLoaiSP", DataPropertyName = "TenLoaiSP", HeaderText = "Loại SP" },
                new DataGridViewTextBoxColumn { Name = "TenChatLieu", DataPropertyName = "TenChatLieu", HeaderText = "Chất liệu" },
                new DataGridViewTextBoxColumn { Name = "TenNCC", DataPropertyName = "TenNCC", HeaderText = "Nhà cung cấp" },
                new DataGridViewTextBoxColumn { Name = "NgayNhapGanNhat", DataPropertyName = "NgayNhapGanNhat", HeaderText = "Ngày nhập" },
                new DataGridViewTextBoxColumn { Name = "TrangThai", DataPropertyName = "TrangThai", HeaderText = "Trạng thái" }
            });

            // Cấu hình cột cho dgvSanPhamDaBan
            dgvSanPhamDaBan.AutoGenerateColumns = false;
            dgvSanPhamDaBan.Columns.Clear();
            dgvSanPhamDaBan.DefaultCellStyle = null; // Xóa định dạng mặc định
            dgvSanPhamDaBan.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "MaSP", DataPropertyName = "MaSP", HeaderText = "Mã SP" },
                new DataGridViewTextBoxColumn { Name = "TenSP", DataPropertyName = "TenSP", HeaderText = "Tên SP" },
                new DataGridViewTextBoxColumn { Name = "MauSac", DataPropertyName = "MauSac", HeaderText = "Màu sắc" },
                new DataGridViewTextBoxColumn { Name = "KichCo", DataPropertyName = "KichCo", HeaderText = "Kích cỡ" },
                new DataGridViewTextBoxColumn { Name = "TenThuongHieu", DataPropertyName = "TenThuongHieu", HeaderText = "Thương hiệu" },
                new DataGridViewTextBoxColumn { Name = "DonGiaBan", DataPropertyName = "DonGiaBan", HeaderText = "Đơn giá" },
                new DataGridViewTextBoxColumn { Name = "SoLuongBan", DataPropertyName = "SoLuongBan", HeaderText = "Số lượng bán" }
            });

            // Cấu hình cột cho dgvSanPhamBanChay
            dgvSanPhamBanChay.AutoGenerateColumns = false;
            dgvSanPhamBanChay.Columns.Clear();
            dgvSanPhamBanChay.DefaultCellStyle = null; // Xóa định dạng mặc định
            dgvSanPhamBanChay.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "MaSP", DataPropertyName = "MaSP", HeaderText = "Mã SP" },
                new DataGridViewTextBoxColumn { Name = "TenSP", DataPropertyName = "TenSP", HeaderText = "Tên SP" },
                new DataGridViewTextBoxColumn { Name = "MauSac", DataPropertyName = "MauSac", HeaderText = "Màu sắc" },
                new DataGridViewTextBoxColumn { Name = "KichCo", DataPropertyName = "KichCo", HeaderText = "Kích cỡ" },
                new DataGridViewTextBoxColumn { Name = "TenThuongHieu", DataPropertyName = "TenThuongHieu", HeaderText = "Thương hiệu" },
                new DataGridViewTextBoxColumn { Name = "SoLuongBan", DataPropertyName = "SoLuongBan", HeaderText = "Số lượng bán" },
                new DataGridViewTextBoxColumn { Name = "TongTien", DataPropertyName = "TongTien", HeaderText = "Doanh thu" }
            });
        }

        // Tải dữ liệu ban đầu cho các DataGridView
        private void LoadData()
        {
            try
            {
                // Tải dữ liệu tất cả sản phẩm
                var sanPhamList = _thongKeBLL.GetAllSanPham();
                dgvSanPham.DataSource = sanPhamList;
                Console.WriteLine($"Số sản phẩm: {sanPhamList.Count}");

                // Tải dữ liệu sản phẩm tồn ít
                var sanPhamTonItList = _thongKeBLL.GetSanPhamTonIt();
                dgvSanPhamTonIt.DataSource = sanPhamTonItList;
                Console.WriteLine($"Số sản phẩm tồn ít: {sanPhamTonItList.Count}");

                // Tải dữ liệu sản phẩm mới nhập
                var sanPhamMoiNhapList = _thongKeBLL.GetSanPhamMoiNhap();
                dgvSanPhamMoiNhap.DataSource = sanPhamMoiNhapList;
                Console.WriteLine($"Số sản phẩm mới nhập: {sanPhamMoiNhapList.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\nChi tiết: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút xem sản phẩm đã bán
        private void btnXemSanPhamDaBan_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = dtpStartDate.Value.Date;
                DateTime endDate = dtpEndDate.Value.Date;

                // Kiểm tra ngày bắt đầu không được lớn hơn ngày kết thúc
                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var sanPhamDaBanList = _thongKeBLL.GetSanPhamDaBan(startDate, endDate);
                dgvSanPhamDaBan.DataSource = sanPhamDaBanList;
                Console.WriteLine($"Số sản phẩm đã bán: {sanPhamDaBanList.Count}");

                // Thông báo nếu không có dữ liệu
                if (sanPhamDaBanList.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu sản phẩm đã bán trong khoảng thời gian này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\nChi tiết: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi nhấn nút xem sản phẩm bán chạy
        private void btnXemSanPhamBanChay_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = dtpStartDateBanChay.Value.Date;
                DateTime endDate = dtpEndDateBanChay.Value.Date;

                // Kiểm tra ngày bắt đầu không được lớn hơn ngày kết thúc
                if (startDate > endDate)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var sanPhamBanChayList = _thongKeBLL.GetSanPhamBanChay(startDate, endDate);
                dgvSanPhamBanChay.DataSource = sanPhamBanChayList;
                Console.WriteLine($"Số sản phẩm bán chạy: {sanPhamBanChayList.Count}");

                // Thông báo nếu không có dữ liệu
                if (sanPhamBanChayList.Count == 0)
                {
                    MessageBox.Show($"Không có dữ liệu sản phẩm bán chạy trong khoảng thời gian từ {startDate.ToShortDateString()} đến {endDate.ToShortDateString()}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\nChi tiết: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Định dạng ô trong dgvSanPham
        private void dgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                // Định dạng màu cho trạng thái "Chuẩn bị hết hàng"
                if (e.Value.ToString() == "Chuẩn bị hết hàng")
                {
                    e.CellStyle.BackColor = Color.OrangeRed;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        // Định dạng ô trong dgvSanPhamTonIt
        private void dgvSanPhamTonIt_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPhamTonIt.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                // Định dạng màu cho trạng thái "Chuẩn bị hết hàng"
                if (e.Value.ToString() == "Chuẩn bị hết hàng")
                {
                    e.CellStyle.BackColor = Color.OrangeRed;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        // Định dạng ô trong dgvSanPhamMoiNhap
        private void dgvSanPhamMoiNhap_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPhamMoiNhap.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                // Định dạng màu cho trạng thái "Mới nhập"
                if (e.Value.ToString() == "Mới nhập")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        // Định dạng ô trong dgvSanPhamBanChay
        private void dgvSanPhamBanChay_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSanPhamBanChay.Rows[e.RowIndex].DataBoundItem is ThongKeSanPhamDTO sp)
            {
                // Highlight dòng nếu số lượng bán lớn hơn 0
                if (sp.SoLuongBan.HasValue && sp.SoLuongBan > 0)
                {
                    dgvSanPhamBanChay.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                    dgvSanPhamBanChay.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
    }
}