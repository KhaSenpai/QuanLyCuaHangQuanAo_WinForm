using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Tầng truy cập dữ liệu (DAL) cho thực thể Chất Liệu.
    // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến chất liệu.
    public class ChatLieuDAL
    {
        private readonly string _connectionString;

        // Hàm khởi tạo lấy chuỗi kết nối từ cấu hình ứng dụng.
        public ChatLieuDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách chất liệu từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng ChatLieuDTO.
        // Ném lỗi nếu truy vấn thất bại.
        public List<ChatLieuDTO> LayTatCaChatLieu()
        {
            var danhSachChatLieu = new List<ChatLieuDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaChatLieu, TenChatLieu, MoTa FROM CHAT_LIEU";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachChatLieu.Add(new ChatLieuDTO
                                {
                                    MaChatLieu = reader["MaChatLieu"].ToString(),
                                    TenChatLieu = reader["TenChatLieu"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách chất liệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách chất liệu: {ex.Message}", ex);
            }

            return danhSachChatLieu;
        }

        // Tìm kiếm chất liệu theo từ khóa dựa trên mã hoặc tên.
        // Trả về danh sách các chất liệu phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<ChatLieuDTO> TimKiemChatLieu(string tuKhoa)
        {
            var danhSachChatLieu = new List<ChatLieuDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaChatLieu, TenChatLieu, MoTa FROM CHAT_LIEU " +
                                        "WHERE MaChatLieu LIKE @TuKhoa OR TenChatLieu LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachChatLieu.Add(new ChatLieuDTO
                                {
                                    MaChatLieu = reader["MaChatLieu"].ToString(),
                                    TenChatLieu = reader["TenChatLieu"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm chất liệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm chất liệu: {ex.Message}", ex);
            }

            return danhSachChatLieu;
        }

        // Thêm một chất liệu mới vào cơ sở dữ liệu.
        // Ném lỗi nếu thêm thất bại (ví dụ: trùng mã hoặc tên chất liệu).
        public void ThemChatLieu(ChatLieuDTO chatLieu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "INSERT INTO CHAT_LIEU (MaChatLieu, TenChatLieu, MoTa) " +
                                        "VALUES (@MaChatLieu, @TenChatLieu, @MoTa)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChatLieu", chatLieu.MaChatLieu);
                        cmd.Parameters.AddWithValue("@TenChatLieu", chatLieu.TenChatLieu);
                        cmd.Parameters.AddWithValue("@MoTa", (object)chatLieu.MoTa ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã hoặc tên chất liệu đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm chất liệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm chất liệu: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin một chất liệu trong cơ sở dữ liệu.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatChatLieu(ChatLieuDTO chatLieu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "UPDATE CHAT_LIEU SET TenChatLieu = @TenChatLieu, MoTa = @MoTa " +
                                        "WHERE MaChatLieu = @MaChatLieu";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChatLieu", chatLieu.MaChatLieu);
                        cmd.Parameters.AddWithValue("@TenChatLieu", chatLieu.TenChatLieu);
                        cmd.Parameters.AddWithValue("@MoTa", (object)chatLieu.MoTa ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Tên chất liệu đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật chất liệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật chất liệu: {ex.Message}", ex);
            }
        }

        // Xóa một chất liệu khỏi cơ sở dữ liệu dựa trên mã chất liệu.
        // Kiểm tra ràng buộc khóa ngoại trước khi xóa.
        // Ném lỗi nếu xóa thất bại.
        public void XoaChatLieu(string maChatLieu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string checkQuery = "SELECT COUNT(*) FROM SAN_PHAM WHERE MaChatLieu = @MaChatLieu";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaChatLieu", maChatLieu);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Không thể xóa chất liệu vì đang được sử dụng trong sản phẩm.");
                        }
                    }

                    const string query = "DELETE FROM CHAT_LIEU WHERE MaChatLieu = @MaChatLieu";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChatLieu", maChatLieu);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi xóa chất liệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chất liệu: {ex.Message}", ex);
            }
        }
    }
}