using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Tầng truy cập dữ liệu (DAL) cho thực thể Thương Hiệu.
    // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến thương hiệu.
    public class ThuongHieuDAL
    {
        private readonly string _connectionString;

        // Hàm khởi tạo lấy chuỗi kết nối từ cấu hình ứng dụng.
        public ThuongHieuDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách thương hiệu từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng ThuongHieuDTO.
        // Ném lỗi nếu truy vấn thất bại.
        public List<ThuongHieuDTO> LayTatCaThuongHieu()
        {
            var danhSachThuongHieu = new List<ThuongHieuDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaThuongHieu, TenThuongHieu, MoTa FROM THUONG_HIEU";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachThuongHieu.Add(new ThuongHieuDTO
                                {
                                    MaThuongHieu = reader["MaThuongHieu"].ToString(),
                                    TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách thương hiệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách thương hiệu: {ex.Message}", ex);
            }

            return danhSachThuongHieu;
        }

        // Tìm kiếm thương hiệu theo từ khóa dựa trên mã hoặc tên thương hiệu.
        // Trả về danh sách các thương hiệu phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<ThuongHieuDTO> TimKiemThuongHieu(string tuKhoa)
        {
            var danhSachThuongHieu = new List<ThuongHieuDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaThuongHieu, TenThuongHieu, MoTa FROM THUONG_HIEU " +
                                        "WHERE MaThuongHieu LIKE @TuKhoa OR TenThuongHieu LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachThuongHieu.Add(new ThuongHieuDTO
                                {
                                    MaThuongHieu = reader["MaThuongHieu"].ToString(),
                                    TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm thương hiệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm thương hiệu: {ex.Message}", ex);
            }

            return danhSachThuongHieu;
        }

        // Thêm một thương hiệu mới vào cơ sở dữ liệu.
        // Ném lỗi nếu thêm thất bại (ví dụ: trùng mã).
        public void ThemThuongHieu(ThuongHieuDTO thuongHieu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "INSERT INTO THUONG_HIEU (MaThuongHieu, TenThuongHieu, MoTa) " +
                                        "VALUES (@MaThuongHieu, @TenThuongHieu, @MoTa)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaThuongHieu", thuongHieu.MaThuongHieu);
                        cmd.Parameters.AddWithValue("@TenThuongHieu", thuongHieu.TenThuongHieu);
                        cmd.Parameters.AddWithValue("@MoTa", (object)thuongHieu.MoTa ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã hoặc tên thương hiệu đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm thương hiệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm thương hiệu: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin một thương hiệu trong cơ sở dữ liệu.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatThuongHieu(ThuongHieuDTO thuongHieu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "UPDATE THUONG_HIEU SET TenThuongHieu = @TenThuongHieu, MoTa = @MoTa " +
                                        "WHERE MaThuongHieu = @MaThuongHieu";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaThuongHieu", thuongHieu.MaThuongHieu);
                        cmd.Parameters.AddWithValue("@TenThuongHieu", thuongHieu.TenThuongHieu);
                        cmd.Parameters.AddWithValue("@MoTa", (object)thuongHieu.MoTa ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Tên thương hiệu đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật thương hiệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật thương hiệu: {ex.Message}", ex);
            }
        }

        // Xóa một thương hiệu khỏi cơ sở dữ liệu dựa trên mã thương hiệu.
        // Ném lỗi nếu xóa thất bại.
        public void XoaThuongHieu(string maThuongHieu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "DELETE FROM THUONG_HIEU WHERE MaThuongHieu = @MaThuongHieu";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaThuongHieu", maThuongHieu);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi xóa thương hiệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa thương hiệu: {ex.Message}", ex);
            }
        }

        // Kiểm tra xem thương hiệu có đang được sử dụng trong sản phẩm hay không.
        // Trả về true nếu thương hiệu đang được sử dụng, false nếu không.
        public bool KiemTraThuongHieuDuocSuDung(string maThuongHieu)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT COUNT(*) FROM SAN_PHAM WHERE MaThuongHieu = @MaThuongHieu";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaThuongHieu", maThuongHieu);
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi kiểm tra thương hiệu: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra thương hiệu: {ex.Message}", ex);
            }
        }
    }
}