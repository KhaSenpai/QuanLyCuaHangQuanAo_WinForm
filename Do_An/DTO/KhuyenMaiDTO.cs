using System;

namespace DTO
{
    // Đối tượng truyền dữ liệu (DTO) cho thực thể Khuyến Mãi.
    // Dùng để truyền dữ liệu khuyến mãi giữa các tầng mà không lộ chi tiết cơ sở dữ liệu.
    public class KhuyenMaiDTO
    {
        // Mã định danh duy nhất của khuyến mãi (ví dụ: "KM001").
        public string MaKhuyenMai { get; set; }

        // Tên của khuyến mãi.
        public string TenKhuyenMai { get; set; }

        // Phần trăm giảm giá của khuyến mãi (0-100).
        public decimal PhanTramKhuyenMai { get; set; }

        // Ngày bắt đầu áp dụng khuyến mãi.
        public DateTime NgayBatDau { get; set; }

        // Ngày kết thúc khuyến mãi.
        public DateTime NgayKetThuc { get; set; }

        // Mô tả của khuyến mãi, có thể null.
        public string MoTa { get; set; }
    }
}