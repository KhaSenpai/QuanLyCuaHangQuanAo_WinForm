using System;

namespace DTO
{
    // Đối tượng truyền dữ liệu (DTO) cho thực thể Khách Hàng.
    // Dùng để truyền dữ liệu khách hàng giữa các tầng mà không lộ chi tiết cơ sở dữ liệu.
    public class KhachHangDTO
    {
        // Mã khách hàng, định danh duy nhất.
        public string MaKhachHang { get; set; }

        // Tên khách hàng.
        public string TenKhachHang { get; set; }

        // Ngày sinh của khách hàng, có thể null.
        public DateTime? NgaySinh { get; set; }

        // Số điện thoại của khách hàng.
        public string SoDienThoai { get; set; }

        // Địa chỉ của khách hàng.
        public string DiaChi { get; set; }

        // Email của khách hàng, có thể null.
        public string Email { get; set; }

        // Giới tính của khách hàng (Nam/Nữ/Khác), có thể null.
        public string GioiTinh { get; set; }
    }
}