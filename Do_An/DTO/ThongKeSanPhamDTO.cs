using System;

namespace DTO
{
    // Lớp DTO để truyền dữ liệu thống kê sản phẩm giữa các tầng
    public class ThongKeSanPhamDTO
    {
        // Mã sản phẩm
        public string MaSP { get; set; }
        // Tên sản phẩm
        public string TenSP { get; set; }
        // Số lượng tồn kho
        public int SoLuongTon { get; set; }
        // Đơn giá bán
        public decimal DonGiaBan { get; set; }
        public decimal TongTien { get; set; }
        // Màu sắc của sản phẩm
        public string MauSac { get; set; }
        // Kích cỡ của sản phẩm
        public string KichCo { get; set; }
        // Tên thương hiệu
        public string TenThuongHieu { get; set; }
        // Tên loại sản phẩm
        public string TenLoaiSP { get; set; }
        // Tên chất liệu
        public string TenChatLieu { get; set; }
        // Tên nhà cung cấp
        public string TenNCC { get; set; }
        // Số lượng sản phẩm đã bán
        public int? SoLuongBan { get; set; }
        // Ngày nhập hàng gần nhất
        public DateTime? NgayNhapGanNhat { get; set; }
        // Trạng thái sản phẩm (Chuẩn bị hết hàng, Mới nhập,...)
        public string TrangThai { get; set; }
    }
}