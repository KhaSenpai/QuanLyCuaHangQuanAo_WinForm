using DTO;
using System;
using System.Windows.Forms;

namespace View
{
    public partial class TrangChu : Form
    {
        private readonly TaiKhoanDTO taiKhoan; // Lưu thông tin tài khoản người dùng

        // Constructor mặc định
        public TrangChu()
        {
            InitializeComponent();
        }

        // Constructor nhận tham số TaiKhoanDTO
        public TrangChu(TaiKhoanDTO taiKhoan) : this()
        {
            this.taiKhoan = taiKhoan;
        }

        // Sự kiện khi form được tải, kiểm tra quyền truy cập và thiết lập giao diện
        private void TrangChu_Load(object sender, EventArgs e)
        {
            // Kiểm tra nếu tài khoản hoặc quyền truy cập rỗng
            if (taiKhoan == null || string.IsNullOrEmpty(taiKhoan.QuyenTruyCap))
            {
                MessageBox.Show("Lỗi: Không xác định được tài khoản hoặc quyền truy cập!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // Thiết lập hiển thị menu và button dựa trên quyền truy cập
            ConfigureMenuByRole(taiKhoan.QuyenTruyCap);
        }

        // Thiết lập hiển thị menu và button dựa trên quyền truy cập của tài khoản
        private void ConfigureMenuByRole(string role)
        {
            // Mặc định hiển thị tất cả cho Admin
            if (role == "Admin") return;

            // Quyền Manager: ẩn menu Hệ thống
            if (role == "Manager")
            {
                hệThốngToolStripMenuItem.Visible = false;
                return;
            }

            // Quyền User: ẩn các menu và button không cần thiết
            if (role == "User")
            {
                nhânViênToolStripMenuItem.Visible = false;
                chứcVụToolStripMenuItem.Visible = false;
                nhàCungCấpToolStripMenuItem.Visible = false;
                hệThốngToolStripMenuItem.Visible = false;
                PhiếuNhapToolStripMenuItem.Visible = false;
                báoCáoToolStripMenuItem.Visible = false;

                btnNhaCungCap.Visible = false;
                btnNhanVien.Visible = false;
                return;
            }

            // Quyền không xác định: ẩn toàn bộ menu và button
            HideAllMenusAndButtons();
        }

        // Ẩn toàn bộ menu và button
        private void HideAllMenusAndButtons()
        {
            PhiếuNhapToolStripMenuItem.Visible = false;
            nhânViênToolStripMenuItem.Visible = false;
            kháchHàngToolStripMenuItem.Visible = false;
            sảnPhẩmToolStripMenuItem.Visible = false;
            chứcVụToolStripMenuItem.Visible = false;
            thươngHiệuToolStripMenuItem.Visible = false;
            nhàCungCấpToolStripMenuItem.Visible = false;
            loạiSảnPhẩmToolStripMenuItem.Visible = false;
            khuyếnMãiToolStripMenuItem.Visible = false;
            thốngKêToolStripMenuItem.Visible = false;
            doanhThuToolStripMenuItem.Visible = false;
            tàiKhoảnToolStripMenuItem.Visible = false;
            nhậpHàngToolStripMenuItem.Visible = false;
            phânQuyềnToolStripMenuItem.Visible = false;
            thôngTinHóaĐơnToolStripMenuItem.Visible = false;
            chấtLiệuToolStripMenuItem.Visible = false;
            giớiThiệuToolStripMenuItem1.Visible = false;

            btnNhanVien.Visible = false;
            btnSanPham.Visible = false;
            btnHoaDon.Visible = false;
            btnKhachHang.Visible = false;
            btnThuonngHieu.Visible = false;
            btnNhaCungCap.Visible = false;
        }

        // Hiển thị form trong panel chính
        private void ShowFormInPanel(Form form)
        {
            panel1.Controls.Clear(); // Xóa nội dung hiện tại trong panel
            form.TopLevel = false; // Không hiển thị như cửa sổ độc lập
            form.FormBorderStyle = FormBorderStyle.None; // Xóa viền form
            form.Dock = DockStyle.Fill; // Đặt form chiếm toàn bộ panel
            panel1.Controls.Add(form); // Thêm form vào panel
            form.Show();
        }

        // Sự kiện click cho menu Nhân viên
        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new NhanVien(taiKhoan));
        }

        // Sự kiện click cho menu Khách hàng
        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new KhachHang(taiKhoan));
        }

        // Sự kiện click cho menu Sản phẩm
        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new SanPham(taiKhoan));
        }

        // Sự kiện click cho menu Nhà cung cấp
        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new NhaCungCap(taiKhoan));
        }

        // Sự kiện click cho menu Chất liệu
        private void chấtLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ChatLieu(taiKhoan));
        }

        // Sự kiện click cho menu Chức vụ
        private void chứcVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ChucVu(taiKhoan));
        }

        // Sự kiện click cho menu Thương hiệu
        private void thươngHiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ThuongHieu(taiKhoan));
        }

        // Sự kiện click cho menu Loại sản phẩm
        private void loạiSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new LoaiSanPham(taiKhoan));
        }

        // Sự kiện click cho menu Tài khoản
        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new TaiKhoan());
        }

        // Sự kiện click cho menu Nhập hàng
        private void nhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new PhieuNhapHang(taiKhoan));
        }

        // Sự kiện click cho menu Hóa đơn
        private void thôngTinHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new HoaDon(taiKhoan));
        }

        // Sự kiện click cho menu Khuyến mãi
        private void khuyếnMãiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new KhuyenMai(taiKhoan));
        }

        // Sự kiện click cho menu Doanh thu
        private void doanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new DoanhThu());
        }

        // Sự kiện click cho menu Phân quyền
        private void phânQuyềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new PhanQuyen());
        }

        // Sự kiện click cho menu Thống kê
        private void thốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ThongKeSanPhan());
        }

        // Sự kiện click cho menu Trợ giúp
        private void TrợGiúpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new TroGiup());
        }

        // Sự kiện click cho button Nhân viên
        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new NhanVien(taiKhoan));
        }

        // Sự kiện click cho button Sản phẩm
        private void btnSanPham_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new SanPham(taiKhoan));
        }

        // Sự kiện click cho button Hóa đơn
        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new HoaDon(taiKhoan));
        }

        // Sự kiện click cho button Khách hàng
        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new KhachHang(taiKhoan));
        }

        // Sự kiện click cho button Thương hiệu
        private void btnThuonngHieu_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new ThuongHieu(taiKhoan));
        }

        // Sự kiện click cho button Nhà cung cấp
        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new NhaCungCap(taiKhoan));
        }

        // Thoát ứng dụng
        private void btnThoatTC_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Đăng xuất và quay lại form Đăng nhập
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                new DangNhap().Show();
                this.Close();
            }
        }

        // Xóa nội dung panel để quay lại trang chính
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
        }
    }
}