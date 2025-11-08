using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace View
{
    public partial class DoanhThu : Form
    {
        private readonly DoanhThuBLL doanhThuBLL = new DoanhThuBLL();

        public DoanhThu()
        {
            InitializeComponent();
            InitializeComboBoxes();
            CaiDatDataGridView();
            dgvDoanhThu.AutoGenerateColumns = true;
            chart1.Visible = false;
        }

        private void InitializeComboBoxes()
        {
            // Add report type options
            cboLoaiBaoCao.Items.AddRange(new object[] { "Trong ngày", "Theo tháng", "Theo năm" });
            cboLoaiBaoCao.SelectedIndex = 0;

            // Initialize year ComboBox
            for (int i = DateTime.Now.Year; i >= 2000; i--)
            {
                cboYear.Items.Add(i.ToString());
            }

            // Initialize month ComboBox
            cboThang.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
        }

        private void cboLoaiBaoCao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiBaoCao.SelectedItem?.ToString() == "Trong ngày")
            {
                dtpDate.Visible = true;
                cboYear.Visible = false;
                cboThang.Visible = false;
                lblNgay.Visible = true;
                lblThang.Visible = false;
                lblNam.Visible = false;
                cboThang.Visible = false;
                chart1.Visible = false;
            }
            else if (cboLoaiBaoCao.SelectedItem?.ToString() == "Theo tháng")
            {
                dtpDate.Visible = false;
                cboYear.Visible = true;
                cboThang.Visible = true;
                lblThang.Visible= true;
                lblNgay.Visible = false;
                lblNam.Visible = true;
                cboThang.Visible = true;
                chart1.Visible = true;
            }
            else if (cboLoaiBaoCao.SelectedItem?.ToString() == "Theo năm")
            {
                dtpDate.Visible = false;
                cboYear.Visible = true;
                cboThang.Visible = false;
                lblNgay.Visible = false;
                lblThang.Visible = false;
                lblNam.Visible = true;
                cboThang.Visible = false;
                chart1.Visible = true;
            }
        }
        private void CaiDatDataGridView()
        {
            dgvDoanhThu.AutoGenerateColumns = false;
            dgvDoanhThu.Columns.Clear();
            dgvDoanhThu.Columns.Add("MaHoaDon", "Mã Hóa Đơn");
            dgvDoanhThu.Columns.Add("NgayLapHoaDon", "Ngày Lập");
            dgvDoanhThu.Columns.Add("TongTien", "Tổng Tiền");
            dgvDoanhThu.Columns.Add("TenKhachHang", "Khách Hàng");
            dgvDoanhThu.Columns.Add("TenNhanVien", "Nhân Viên");
            dgvDoanhThu.Columns.Add("TenKhuyenMai", "Khuyến Mãi");
            dgvDoanhThu.Columns.Add("PhanTramKhuyenMai", "% Khuyến Mãi");
            dgvDoanhThu.Columns.Add("ThoiGian", "Tháng");
            dgvDoanhThu.Columns.Add("TongDoanhThu", "Tổng Doanh Thu");
            dgvDoanhThu.Columns["MaHoaDon"].DataPropertyName = "MaHoaDon";
            dgvDoanhThu.Columns["NgayLapHoaDon"].DataPropertyName = "NgayLapHoaDon";
            dgvDoanhThu.Columns["TongTien"].DataPropertyName = "TongTien";
            dgvDoanhThu.Columns["TenKhachHang"].DataPropertyName = "TenKhachHang";
            dgvDoanhThu.Columns["TenNhanVien"].DataPropertyName = "TenNhanVien";
            dgvDoanhThu.Columns["TenKhuyenMai"].DataPropertyName = "TenKhuyenMai";
            dgvDoanhThu.Columns["PhanTramKhuyenMai"].DataPropertyName = "PhanTramKhuyenMai";
            dgvDoanhThu.Columns["ThoiGian"].DataPropertyName = "ThoiGian";
            dgvDoanhThu.Columns["TongDoanhThu"].DataPropertyName = "TongDoanhThu";
        }
        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiBaoCao.SelectedItem == null)
                    throw new Exception("Vui lòng chọn loại báo cáo!");

                if (cboLoaiBaoCao.SelectedItem.ToString() == "Trong ngày")
                {
                    DateTime date = dtpDate.Value.Date;
                    var doanhThuList = doanhThuBLL.GetDoanhThuByDate(date);
                    dgvDoanhThu.DataSource = doanhThuList;
                    decimal tongDoanhThu = doanhThuBLL.GetTongDoanhThuByDate(date);
                    txtTongDoanhThu.Text = tongDoanhThu.ToString("N0") + " VNĐ";

                    if (doanhThuList.Count == 0)
                        MessageBox.Show("Không có dữ liệu doanh thu cho ngày đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chart1.Visible = false;
                }
                else if (cboLoaiBaoCao.SelectedItem.ToString() == "Theo tháng")
                {
                    if (cboYear.SelectedItem == null || cboThang.SelectedItem == null)
                        throw new Exception("Vui lòng chọn năm và tháng!");
                    int year = int.Parse(cboYear.SelectedItem.ToString());
                    int month = int.Parse(cboThang.SelectedItem.ToString());

                    var doanhThuList = doanhThuBLL.GetDoanhThuChiTietByMonth(year, month);
                    dgvDoanhThu.DataSource = doanhThuList;
                    decimal tongDoanhThu = doanhThuBLL.GetTongDoanhThuByMonth(year, month);
                    txtTongDoanhThu.Text = tongDoanhThu.ToString("N0") + " VNĐ";

                    if (doanhThuList.Count == 0)
                        MessageBox.Show($"Không có dữ liệu doanh thu cho tháng {month}/{year}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        var doanhThuTongHop = doanhThuBLL.GetDoanhThuByMonth(year, month);
                        CreateChart(doanhThuTongHop, $"Doanh Thu Theo Ngày Trong Tháng {month}/{year}");
                    }
                }
                else if (cboLoaiBaoCao.SelectedItem.ToString() == "Theo năm")
                {
                    if (cboYear.SelectedItem == null)
                        throw new Exception("Vui lòng chọn năm!");
                    int year = int.Parse(cboYear.SelectedItem.ToString());
                    var doanhThuList = doanhThuBLL.GetDoanhThuByYear(year);
                    dgvDoanhThu.DataSource = doanhThuList;
                    decimal tongDoanhThu = doanhThuBLL.GetTongDoanhThuByYear(year);
                    txtTongDoanhThu.Text = tongDoanhThu.ToString("N0") + " VNĐ";

                    if (doanhThuList.Count == 0)
                        MessageBox.Show("Không có dữ liệu doanh thu cho năm đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        CreateChart(doanhThuList, $"Doanh Thu Theo Tháng Trong Năm {year}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateChart(List<DoanhThuTongHopDTO> data, string title)
        {
            if (data == null || data.Count == 0)
            {
                chart1.Visible = false;
                return;
            }

            chart1.Visible = true;
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add(new Title(title));

            var series = new Series
            {
                Name = "Doanh Thu",
                ChartType = SeriesChartType.Column,
                Color = Color.Blue,
            };

            decimal maxValue = data.Max(d => d.TongDoanhThu) * 1.2M; // Increase by 20% for padding
            foreach (var item in data)
            {
                series.Points.AddXY(item.ThoiGian, item.TongDoanhThu);
            }

            chart1.Series.Add(series);
            chart1.ChartAreas[0].AxisY.Maximum = (double)maxValue;
            chart1.ChartAreas[0].AxisY.Interval = (double)(maxValue / 5);
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0:N0} VNĐ";
        }
    }
}