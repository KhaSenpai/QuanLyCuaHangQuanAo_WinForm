using System;

namespace DTO
{
    // Đối tượng truyền dữ liệu (DTO) cho thực thể Nhân Viên.
    // Dùng để truyền dữ liệu nhân viên giữa các tầng mà không lộ chi tiết cơ sở dữ liệu.
    public class NhanVienDTO
    {
        // Mã định danh duy nhất của nhân viên (ví dụ: "NV001").
        public string MaNhanVien { get; set; }

        // Họ và tên của nhân viên.
        public string HoTen { get; set; }

        // Ngày sinh của nhân viên.
        public DateTime NgaySinh { get; set; }

        // Giới tính của nhân viên (Nam/Nữ).
        public string GioiTinh { get; set; }

        // Địa chỉ của nhân viên.
        public string DiaChi { get; set; }

        // Số điện thoại của nhân viên.
        public string SoDienThoai { get; set; }

        // Mã chức vụ của nhân viên.
        public string MaChucVu { get; set; }

        // Tên chức vụ tương ứng, dùng để hiển thị.
        public string TenChucVu { get; set; }
    }
}