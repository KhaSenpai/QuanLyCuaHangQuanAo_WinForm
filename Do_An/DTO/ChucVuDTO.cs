namespace DTO
{
    // Đối tượng truyền dữ liệu (DTO) cho thực thể Chức Vụ.
    // Dùng để truyền dữ liệu chức vụ giữa các tầng mà không lộ chi tiết cơ sở dữ liệu.
    public class ChucVuDTO
    {
        // Mã định danh duy nhất của chức vụ (ví dụ: "CV001").
        public string MaChucVu { get; set; }

        // Tên của chức vụ.
        public string TenChucVu { get; set; }

        // Mô tả của chức vụ, có thể null.
        public string MoTa { get; set; }
    }
}