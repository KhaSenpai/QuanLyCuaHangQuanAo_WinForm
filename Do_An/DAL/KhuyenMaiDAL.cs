using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Tầng truy cập dữ liệu (DAL) cho thực thể Khuyến Mãi.
    // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến khuyến mãi.
    public class KhuyenMaiDAL
    {
        private readonly string _connectionString;

        // Hàm khởi tạo lấy chuỗi kết nối từ cấu hình ứng dụng.
        public KhuyenMaiDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách khuyến mãi từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng KhuyenMaiDTO.
        // Ném lỗi nếu truy vấn thất bại.
        public List<KhuyenMaiDTO> LayTatCaKhuyenMai()
        {
            var danhSachKhuyenMai = new List<KhuyenMaiDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaKhuyenMai, TenKhuyenMai, PhanTramKhuyenMai, NgayBatDau, NgayKetThuc, MoTa FROM KHUYEN_MAI";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachKhuyenMai.Add(new KhuyenMaiDTO
                                {
                                    MaKhuyenMai = reader["MaKhuyenMai"].ToString(),
                                    TenKhuyenMai = reader["TenKhuyenMai"].ToString(),
                                    PhanTramKhuyenMai = Convert.ToDecimal(reader["PhanTramKhuyenMai"]),
                                    NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                                    NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách khuyến mãi: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khuyến mãi: {ex.Message}", ex);
            }

            return danhSachKhuyenMai;
        }

        // Thêm một khuyến mãi mới vào cơ sở dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình thêm.
        public bool ThemKhuyenMai(KhuyenMaiDTO khuyenMai)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "INSERT INTO KHUYEN_MAI (MaKhuyenMai, TenKhuyenMai, PhanTramKhuyenMai, NgayBatDau, NgayKetThuc, MoTa) " +
                                        "VALUES (@MaKhuyenMai, @TenKhuyenMai, @PhanTramKhuyenMai, @NgayBatDau, @NgayKetThuc, @MoTa)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhuyenMai", khuyenMai.MaKhuyenMai);
                        cmd.Parameters.AddWithValue("@TenKhuyenMai", khuyenMai.TenKhuyenMai);
                        cmd.Parameters.AddWithValue("@PhanTramKhuyenMai", khuyenMai.PhanTramKhuyenMai);
                        cmd.Parameters.AddWithValue("@NgayBatDau", khuyenMai.NgayBatDau);
                        cmd.Parameters.AddWithValue("@NgayKetThuc", khuyenMai.NgayKetThuc);
                        cmd.Parameters.AddWithValue("@MoTa", (object)khuyenMai.MoTa ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã khuyến mãi đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm khuyến mãi: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm khuyến mãi: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin một khuyến mãi trong cơ sở dữ liệu.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình cập nhật.
        public bool CapNhatKhuyenMai(KhuyenMaiDTO khuyenMai)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "UPDATE KHUYEN_MAI SET TenKhuyenMai = @TenKhuyenMai, PhanTramKhuyenMai = @PhanTramKhuyenMai, " +
                                        "NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc, MoTa = @MoTa WHERE MaKhuyenMai = @MaKhuyenMai";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhuyenMai", khuyenMai.MaKhuyenMai);
                        cmd.Parameters.AddWithValue("@TenKhuyenMai", khuyenMai.TenKhuyenMai);
                        cmd.Parameters.AddWithValue("@PhanTramKhuyenMai", khuyenMai.PhanTramKhuyenMai);
                        cmd.Parameters.AddWithValue("@NgayBatDau", khuyenMai.NgayBatDau);
                        cmd.Parameters.AddWithValue("@NgayKetThuc", khuyenMai.NgayKetThuc);
                        cmd.Parameters.AddWithValue("@MoTa", (object)khuyenMai.MoTa ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật khuyến mãi: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật khuyến mãi: {ex.Message}", ex);
            }
        }

        // Xóa một khuyến mãi khỏi cơ sở dữ liệu dựa trên mã khuyến mãi.
        // Kiểm tra ràng buộc khóa ngoại trước khi xóa.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình xóa.
        public bool XoaKhuyenMai(string maKhuyenMai)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string checkQuery = "SELECT COUNT(*) FROM HOA_DON_BAN WHERE MaKhuyenMai = @MaKhuyenMai";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaKhuyenMai", maKhuyenMai);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Không thể xóa khuyến mãi vì đang được sử dụng trong hóa đơn.");
                        }
                    }

                    const string query = "DELETE FROM KHUYEN_MAI WHERE MaKhuyenMai = @MaKhuyenMai";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhuyenMai", maKhuyenMai);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi xóa khuyến mãi: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa khuyến mãi: {ex.Message}", ex);
            }
        }

        // Tìm kiếm khuyến mãi theo từ khóa dựa trên mã, tên hoặc mô tả.
        // Trả về danh sách các khuyến mãi phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<KhuyenMaiDTO> TimKiemKhuyenMai(string tuKhoa)
        {
            var danhSachKhuyenMai = new List<KhuyenMaiDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaKhuyenMai, TenKhuyenMai, PhanTramKhuyenMai, NgayBatDau, NgayKetThuc, MoTa " +
                                        "FROM KHUYEN_MAI WHERE MaKhuyenMai LIKE @TuKhoa OR TenKhuyenMai LIKE @TuKhoa OR MoTa LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachKhuyenMai.Add(new KhuyenMaiDTO
                                {
                                    MaKhuyenMai = reader["MaKhuyenMai"].ToString(),
                                    TenKhuyenMai = reader["TenKhuyenMai"].ToString(),
                                    PhanTramKhuyenMai = Convert.ToDecimal(reader["PhanTramKhuyenMai"]),
                                    NgayBatDau = Convert.ToDateTime(reader["NgayBatDau"]),
                                    NgayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm khuyến mãi: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm khuyến mãi: {ex.Message}", ex);
            }

            return danhSachKhuyenMai;
        }
    }
}