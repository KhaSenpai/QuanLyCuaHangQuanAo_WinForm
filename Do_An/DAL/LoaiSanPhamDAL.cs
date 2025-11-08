using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Tầng truy cập dữ liệu (DAL) cho thực thể Loại Sản Phẩm.
    // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến loại sản phẩm.
    public class LoaiSanPhamDAL
    {
        private readonly string _connectionString;

        // Hàm khởi tạo lấy chuỗi kết nối từ cấu hình ứng dụng.
        public LoaiSanPhamDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách loại sản phẩm từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng LoaiSanPhamDTO.
        // Ném lỗi nếu truy vấn thất bại.
        public List<LoaiSanPhamDTO> LayTatCaLoaiSanPham()
        {
            var danhSachLoaiSanPham = new List<LoaiSanPhamDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaLoaiSP, TenLoaiSP, MoTa FROM LOAI_SAN_PHAM";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachLoaiSanPham.Add(new LoaiSanPhamDTO
                                {
                                    MaLoaiSP = reader["MaLoaiSP"].ToString(),
                                    TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách loại sản phẩm: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách loại sản phẩm: {ex.Message}", ex);
            }

            return danhSachLoaiSanPham;
        }

        // Tìm kiếm loại sản phẩm theo từ khóa dựa trên mã hoặc tên loại sản phẩm.
        // Trả về danh sách các loại sản phẩm phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<LoaiSanPhamDTO> TimKiemLoaiSanPham(string tuKhoa)
        {
            var danhSachLoaiSanPham = new List<LoaiSanPhamDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaLoaiSP, TenLoaiSP, MoTa FROM LOAI_SAN_PHAM " +
                                        "WHERE MaLoaiSP LIKE @TuKhoa OR TenLoaiSP LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachLoaiSanPham.Add(new LoaiSanPhamDTO
                                {
                                    MaLoaiSP = reader["MaLoaiSP"].ToString(),
                                    TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                    MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm loại sản phẩm: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm loại sản phẩm: {ex.Message}", ex);
            }

            return danhSachLoaiSanPham;
        }

        // Thêm một loại sản phẩm mới vào cơ sở dữ liệu.
        // Ném lỗi nếu thêm thất bại (ví dụ: trùng mã hoặc tên).
        public void ThemLoaiSanPham(LoaiSanPhamDTO loaiSanPham)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "INSERT INTO LOAI_SAN_PHAM (MaLoaiSP, TenLoaiSP, MoTa) " +
                                        "VALUES (@MaLoaiSP, @TenLoaiSP, @MoTa)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLoaiSP", loaiSanPham.MaLoaiSP);
                        cmd.Parameters.AddWithValue("@TenLoaiSP", loaiSanPham.TenLoaiSP);
                        cmd.Parameters.AddWithValue("@MoTa", (object)loaiSanPham.MoTa ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã hoặc tên loại sản phẩm đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm loại sản phẩm: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm loại sản phẩm: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin một loại sản phẩm trong cơ sở dữ liệu.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatLoaiSanPham(LoaiSanPhamDTO loaiSanPham)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "UPDATE LOAI_SAN_PHAM SET TenLoaiSP = @TenLoaiSP, MoTa = @MoTa " +
                                        "WHERE MaLoaiSP = @MaLoaiSP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLoaiSP", loaiSanPham.MaLoaiSP);
                        cmd.Parameters.AddWithValue("@TenLoaiSP", loaiSanPham.TenLoaiSP);
                        cmd.Parameters.AddWithValue("@MoTa", (object)loaiSanPham.MoTa ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Tên loại sản phẩm đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật loại sản phẩm: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật loại sản phẩm: {ex.Message}", ex);
            }
        }

        // Xóa một loại sản phẩm khỏi cơ sở dữ liệu dựa trên mã loại sản phẩm.
        // Kiểm tra ràng buộc khóa ngoại trước khi xóa.
        // Ném lỗi nếu xóa thất bại.
        public void XoaLoaiSanPham(string maLoaiSP)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string checkQuery = "SELECT COUNT(*) FROM SAN_PHAM WHERE MaLoaiSP = @MaLoaiSP";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaLoaiSP", maLoaiSP);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Không thể xóa loại sản phẩm vì đang được sử dụng trong sản phẩm.");
                        }
                    }

                    const string query = "DELETE FROM LOAI_SAN_PHAM WHERE MaLoaiSP = @MaLoaiSP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLoaiSP", maLoaiSP);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi xóa loại sản phẩm: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa loại sản phẩm: {ex.Message}", ex);
            }
        }
    }
}