namespace DTO
{
    // Đối tượng truyền dữ liệu (DTO) cho thực thể Nhà Cung Cấp.
    // Dùng để truyền dữ liệu nhà cung cấp giữa các tầng mà không lộ chi tiết cơ sở dữ liệu.
    public class NhaCungCapDTO
    {
        // Mã định danh duy nhất của nhà cung cấp (ví dụ: "NCC001").
        public string MaNCC { get; set; }

        // Tên của nhà cung cấp.
        public string TenNCC { get; set; }

        // Địa chỉ của nhà cung cấp.
        public string DiaChi { get; set; }

        // Số điện thoại của nhà cung cấp.
        public string DienThoai { get; set; }

        // Email của nhà cung cấp, có thể null.
        public string Email { get; set; }
    }
}