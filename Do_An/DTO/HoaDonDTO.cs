using System;

namespace DTO
{
    // Đối tượng truyền dữ liệu cho hóa đơn.
    public class HoaDonDTO
    {
        public string MaHoaDon { get; set; } // Mã hóa đơn
        public DateTime NgayLapHoaDon { get; set; } // Ngày lập hóa đơn
        public decimal TongTien { get; set; } // Tổng tiền hóa đơn
        public string MaNhanVien { get; set; } // Mã nhân viên lập hóa đơn
        public string HoTen { get; set; } // Tên nhân viên
        public string MaKhuyenMai { get; set; } // Mã khuyến mãi
        public string TenKhuyenMai { get; set; } // Tên khuyến mãi
        public string MaKhachHang { get; set; } // Mã khách hàng
        public string TenKhachHang { get; set; } // Tên khách hàng
        public string DiaChi { get; set; } // Địa chỉ khách hàng
        public string GhiChu { get; set; } // Ghi chú hóa đơn
    }
}