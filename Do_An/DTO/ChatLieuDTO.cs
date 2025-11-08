namespace DTO
{
    // Đối tượng truyền dữ liệu cho thực thể Chất Liệu.
    // Dùng để truyền dữ liệu giữa UI, BLL, và DAL một cách an toàn.
    public class ChatLieuDTO
    {
        // Mã định danh duy nhất của chất liệu (ví dụ: "CL001").
        public string MaChatLieu { get; set; }

        // Tên của chất liệu (ví dụ: "Vải cotton", "Da thật").
        public string TenChatLieu { get; set; }

        // Mô tả chi tiết của chất liệu
        public string MoTa { get; set; }
    }
}