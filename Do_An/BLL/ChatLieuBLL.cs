using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Chất Liệu.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class ChatLieuBLL
    {
        private readonly ChatLieuDAL _chatLieuDAL;

        // Hàm khởi tạo, tạo một đối tượng ChatLieuDAL để tương tác với cơ sở dữ liệu.
        public ChatLieuBLL()
        {
            _chatLieuDAL = new ChatLieuDAL();
        }

        // Lấy toàn bộ danh sách chất liệu.
        // Trả về danh sách các đối tượng ChatLieuDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<ChatLieuDTO> LayTatCaChatLieu()
        {
            return _chatLieuDAL.LayTatCaChatLieu();
        }

        // Tìm kiếm chất liệu dựa trên từ khóa.
        // Trả về danh sách các chất liệu phù hợp.
        // Ném lỗi nếu từ khóa không hợp lệ hoặc có vấn đề trong quá trình tìm kiếm.
        public List<ChatLieuDTO> TimKiemChatLieu(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                throw new ArgumentException("Từ khóa tìm kiếm không được để trống.");
            }
            return _chatLieuDAL.TimKiemChatLieu(tuKhoa.Trim());
        }

        // Tạo mã chất liệu mới theo định dạng "CLxxx" (xxx là số tăng dần).
        // Trả về mã chất liệu mới.
        public string TaoMaChatLieu()
        {
            try
            {
                var danhSach = LayTatCaChatLieu();
                int maxId = 0;
                if (danhSach.Any())
                {
                    maxId = danhSach
                        .Select(cl => int.Parse(cl.MaChatLieu.Replace("CL", "")))
                        .Max();
                }
                return $"CL{(maxId + 1):D3}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo mã chất liệu: {ex.Message}", ex);
            }
        }

        // Thêm một chất liệu mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Ném lỗi nếu thêm thất bại.
        public void ThemChatLieu(ChatLieuDTO chatLieu)
        {
            if (chatLieu == null)
            {
                throw new ArgumentNullException(nameof(chatLieu), "Đối tượng chất liệu không được null.");
            }
            if (string.IsNullOrWhiteSpace(chatLieu.MaChatLieu) || string.IsNullOrWhiteSpace(chatLieu.TenChatLieu))
            {
                throw new ArgumentException("Mã và tên chất liệu không được để trống.");
            }
            if (chatLieu.MaChatLieu.Length > 20)
            {
                throw new ArgumentException("Mã chất liệu không được vượt quá 20 ký tự.");
            }
            if (chatLieu.TenChatLieu.Length > 50)
            {
                throw new ArgumentException("Tên chất liệu không được vượt quá 50 ký tự.");
            }
            _chatLieuDAL.ThemChatLieu(chatLieu);
        }

        // Cập nhật thông tin một chất liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatChatLieu(ChatLieuDTO chatLieu)
        {
            if (chatLieu == null)
            {
                throw new ArgumentNullException(nameof(chatLieu), "Đối tượng chất liệu không được null.");
            }
            if (string.IsNullOrWhiteSpace(chatLieu.MaChatLieu) || string.IsNullOrWhiteSpace(chatLieu.TenChatLieu))
            {
                throw new ArgumentException("Mã và tên chất liệu không được để trống.");
            }
            if (chatLieu.MaChatLieu.Length > 20)
            {
                throw new ArgumentException("Mã chất liệu không được vượt quá 20 ký tự.");
            }
            if (chatLieu.TenChatLieu.Length > 50)
            {
                throw new ArgumentException("Tên chất liệu không được vượt quá 50 ký tự.");
            }
            _chatLieuDAL.CapNhatChatLieu(chatLieu);
        }

        // Xóa một chất liệu dựa trên mã chất liệu.
        // Ném lỗi nếu xóa thất bại hoặc chất liệu đang được sử dụng.
        public void XoaChatLieu(string maChatLieu)
        {
            if (string.IsNullOrWhiteSpace(maChatLieu))
            {
                throw new ArgumentException("Mã chất liệu không được để trống.");
            }
            _chatLieuDAL.XoaChatLieu(maChatLieu);
        }
    }
}