using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Tầng truy cập dữ liệu (DAL) cho thực thể Nhà Cung Cấp.
    // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến nhà cung cấp.
    public class NhaCungCapDAL
    {
        private readonly string _connectionString;

        // Hàm khởi tạo lấy chuỗi kết nối từ cấu hình ứng dụng.
        public NhaCungCapDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách nhà cung cấp từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng NhaCungCapDTO.
        // Ném lỗi nếu truy vấn thất bại.
        public List<NhaCungCapDTO> LayTatCaNhaCungCap()
        {
            var danhSachNhaCungCap = new List<NhaCungCapDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaNCC, TenNCC, DiaChi, DienThoai, Email FROM NHA_CUNG_CAP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachNhaCungCap.Add(new NhaCungCapDTO
                                {
                                    MaNCC = reader["MaNCC"].ToString(),
                                    TenNCC = reader["TenNCC"].ToString(),
                                    DiaChi = reader["DiaChi"].ToString(),
                                    DienThoai = reader["DienThoai"].ToString(),
                                    Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách nhà cung cấp: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}", ex);
            }

            return danhSachNhaCungCap;
        }

        // Tìm kiếm nhà cung cấp theo từ khóa dựa trên mã, tên hoặc số điện thoại.
        // Trả về danh sách các nhà cung cấp phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<NhaCungCapDTO> TimKiemNhaCungCap(string tuKhoa)
        {
            var danhSachNhaCungCap = new List<NhaCungCapDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaNCC, TenNCC, DiaChi, DienThoai, Email FROM NHA_CUNG_CAP " +
                                        "WHERE MaNCC LIKE @TuKhoa OR TenNCC LIKE @TuKhoa OR DienThoai LIKE @TuKhoa OR DiaChi LIKE @TuKhoa OR Email LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachNhaCungCap.Add(new NhaCungCapDTO
                                {
                                    MaNCC = reader["MaNCC"].ToString(),
                                    TenNCC = reader["TenNCC"].ToString(),
                                    DiaChi = reader["DiaChi"].ToString(),
                                    DienThoai = reader["DienThoai"].ToString(),
                                    Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm nhà cung cấp: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm nhà cung cấp: {ex.Message}", ex);
            }

            return danhSachNhaCungCap;
        }

        // Thêm một nhà cung cấp mới vào cơ sở dữ liệu.
        // Ném lỗi nếu thêm thất bại (ví dụ: trùng mã, số điện thoại hoặc email).
        public void ThemNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, DiaChi, DienThoai, Email) " +
                                        "VALUES (@MaNCC, @TenNCC, @DiaChi, @DienThoai, @Email)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNCC", nhaCungCap.MaNCC);
                        cmd.Parameters.AddWithValue("@TenNCC", nhaCungCap.TenNCC);
                        cmd.Parameters.AddWithValue("@DiaChi", nhaCungCap.DiaChi);
                        cmd.Parameters.AddWithValue("@DienThoai", nhaCungCap.DienThoai);
                        cmd.Parameters.AddWithValue("@Email", (object)nhaCungCap.Email ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã nhà cung cấp, số điện thoại hoặc email đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm nhà cung cấp: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm nhà cung cấp: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin một nhà cung cấp trong cơ sở dữ liệu.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "UPDATE NHA_CUNG_CAP SET TenNCC = @TenNCC, DiaChi = @DiaChi, " +
                                        "DienThoai = @DienThoai, Email = @Email WHERE MaNCC = @MaNCC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNCC", nhaCungCap.MaNCC);
                        cmd.Parameters.AddWithValue("@TenNCC", nhaCungCap.TenNCC);
                        cmd.Parameters.AddWithValue("@DiaChi", nhaCungCap.DiaChi);
                        cmd.Parameters.AddWithValue("@DienThoai", nhaCungCap.DienThoai);
                        cmd.Parameters.AddWithValue("@Email", (object)nhaCungCap.Email ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Số điện thoại hoặc email đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật nhà cung cấp: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật nhà cung cấp: {ex.Message}", ex);
            }
        }

        // Xóa một nhà cung cấp khỏi cơ sở dữ liệu dựa trên mã nhà cung cấp.
        // Kiểm tra ràng buộc khóa ngoại trước khi xóa.
        // Ném lỗi nếu xóa thất bại.
        public void XoaNhaCungCap(string maNCC)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string checkQuery = "SELECT COUNT(*) FROM SAN_PHAM WHERE MaNCC = @MaNCC";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaNCC", maNCC);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Không thể xóa nhà cung cấp vì đang được sử dụng trong sản phẩm.");
                        }
                    }

                    const string query = "DELETE FROM NHA_CUNG_CAP WHERE MaNCC = @MaNCC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi xóa nhà cung cấp: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa nhà cung cấp: {ex.Message}", ex);
            }
        }
    }
}