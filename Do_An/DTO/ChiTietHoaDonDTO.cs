namespace DTO
{
    // Đối tượng truyền dữ liệu cho chi tiết hóa đơn.
    public class ChiTietHoaDonDTO
    {
        public string MaHoaDon { get; set; } // Mã hóa đơn
        public string MaSanPham { get; set; } // Mã sản phẩm
        public string TenSanPham { get; set; } // Tên sản phẩm
        public int SoLuong { get; set; } // Số lượng sản phẩm
        public decimal DonGia { get; set; } // Đơn giá sản phẩm
        public decimal ThanhTien { get; set; } // Thành tiền (số lượng * đơn giá)
    }
}