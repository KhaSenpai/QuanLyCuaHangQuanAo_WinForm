using System;

namespace DTO
{
    // Đối tượng truyền dữ liệu cho phiếu nhập.
    public class PhieuNhapDTO
    {
        public string MaPhieuNhap { get; set; } // Mã phiếu nhập
        public string MaNCC { get; set; } // Mã nhà cung cấp
        public string MaNhanVien { get; set; } // Mã nhân viên lập phiếu
        public DateTime? NgayLapPhieuNhap { get; set; } // Ngày lập phiếu nhập
        public decimal TongTien { get; set; } // Tổng tiền phiếu nhập
        public string DiaChi { get; set; } // Địa chỉ nhà cung cấp
        public string GhiChu { get; set; } // Ghi chú phiếu nhập
    }
}