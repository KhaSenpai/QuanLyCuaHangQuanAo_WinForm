namespace DTO
{
    // Đối tượng truyền dữ liệu (DTO) cho thực thể Loại Sản Phẩm.
    // Dùng để truyền dữ liệu loại sản phẩm giữa các tầng mà không lộ chi tiết cơ sở dữ liệu.
    public class LoaiSanPhamDTO
    {
        // Mã định danh duy nhất của loại sản phẩm (ví dụ: "LSP001").
        public string MaLoaiSP { get; set; }

        // Tên của loại sản phẩm.
        public string TenLoaiSP { get; set; }

        // Mô tả của loại sản phẩm, có thể null.
        public string MoTa { get; set; }
    }
}