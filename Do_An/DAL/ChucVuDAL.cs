using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Tầng truy cập dữ liệu (DAL) cho thực thể Chức Vụ.
    // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến chức vụ.
    public class ChucVuDAL
    {
        private readonly string _connectionString;

        // Hàm khởi tạo lấy chuỗi kết nối từ cấu hình ứng dụng.
        public ChucVuDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách chức vụ từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng ChucVuDTO.
        // Ném lỗi nếu truy vấn thất bại.
        public List<ChucVuDTO> LayTatCaChucVu()
        {
            var danhSachChucVu = new List<ChucVuDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaChucVu, TenChucVu, MoTa FROM CHUC_VU";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachChucVu.Add(new ChucVuDTO
                                {
                                    MaChucVu = reader["MaChucVu"].ToString(),
                                    TenChucVu = reader["TenChucVu"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách chức vụ: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách chức vụ: {ex.Message}", ex);
            }

            return danhSachChucVu;
        }

        // Tìm kiếm chức vụ theo từ khóa dựa trên mã hoặc tên.
        // Trả về danh sách các chức vụ phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<ChucVuDTO> TimKiemChucVu(string tuKhoa)
        {
            var danhSachChucVu = new List<ChucVuDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaChucVu, TenChucVu, MoTa FROM CHUC_VU " +
                                        "WHERE MaChucVu LIKE @TuKhoa OR TenChucVu LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachChucVu.Add(new ChucVuDTO
                                {
                                    MaChucVu = reader["MaChucVu"].ToString(),
                                    TenChucVu = reader["TenChucVu"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm chức vụ: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm chức vụ: {ex.Message}", ex);
            }

            return danhSachChucVu;
        }

        // Thêm một chức vụ mới vào cơ sở dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình thêm.
        public bool ThemChucVu(ChucVuDTO chucVu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "INSERT INTO CHUC_VU (MaChucVu, TenChucVu, MoTa) " +
                                        "VALUES (@MaChucVu, @TenChucVu, @MoTa)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChucVu", chucVu.MaChucVu);
                        cmd.Parameters.AddWithValue("@TenChucVu", chucVu.TenChucVu);
                        cmd.Parameters.AddWithValue("@MoTa", (object)chucVu.MoTa ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã hoặc tên chức vụ đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm chức vụ: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm chức vụ: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin một chức vụ trong cơ sở dữ liệu.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình cập nhật.
        public bool CapNhatChucVu(ChucVuDTO chucVu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "UPDATE CHUC_VU SET TenChucVu = @TenChucVu, MoTa = @MoTa " +
                                        "WHERE MaChucVu = @MaChucVu";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChucVu", chucVu.MaChucVu);
                        cmd.Parameters.AddWithValue("@TenChucVu", chucVu.TenChucVu);
                        cmd.Parameters.AddWithValue("@MoTa", (object)chucVu.MoTa ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Tên chức vụ đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật chức vụ: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật chức vụ: {ex.Message}", ex);
            }
        }

        // Xóa một chức vụ khỏi cơ sở dữ liệu dựa trên mã chức vụ.
        // Kiểm tra ràng buộc khóa ngoại trước khi xóa.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình xóa.
        public bool XoaChucVu(string maChucVu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string checkQuery = "SELECT COUNT(*) FROM NHAN_VIEN WHERE MaChucVu = @MaChucVu";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaChucVu", maChucVu);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Không thể xóa chức vụ vì đang được sử dụng trong nhân viên.");
                        }
                    }

                    const string query = "DELETE FROM CHUC_VU WHERE MaChucVu = @MaChucVu";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChucVu", maChucVu);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi xóa chức vụ: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chức vụ: {ex.Message}", ex);
            }
        }
    }
}