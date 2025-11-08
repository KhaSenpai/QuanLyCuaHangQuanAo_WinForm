namespace DTO
{
    // Đối tượng truyền dữ liệu cho chi tiết phiếu nhập.
    public class ChiTietPhieuNhapDTO
    {
        public string MaPhieuNhap { get; set; } // Mã phiếu nhập
        public string MaSanPham { get; set; } // Mã sản phẩm
        public int SoLuong { get; set; } // Số lượng sản phẩm nhập
        public decimal DonGia { get; set; } // Đơn giá nhập
        public decimal ThanhTien { get; set; } // Thành tiền (số lượng * đơn giá)
    }
}