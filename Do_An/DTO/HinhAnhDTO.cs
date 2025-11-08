namespace DTO
{
    // Lớp DTO cho hình ảnh, chứa thông tin chi tiết của một hình ảnh sản phẩm.
    public class HinhAnhDTO
    {
        // Mã hình ảnh (khóa chính).
        public string MaHinhAnh { get; set; }

        // Mã sản phẩm liên kết.
        public string MaSP { get; set; }

        // Đường dẫn hình ảnh.
        public string DuongDanHinh { get; set; }
    }
}