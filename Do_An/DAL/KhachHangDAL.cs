using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Tầng truy cập dữ liệu (DAL) cho thực thể Khách Hàng.
    // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến khách hàng.
    public class KhachHangDAL
    {
        private readonly string _connectionString;

        // Hàm khởi tạo lấy chuỗi kết nối từ tệp cấu hình.
        // Ném lỗi nếu chuỗi kết nối không tồn tại.
        public KhachHangDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách khách hàng từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng KhachHangDTO.
        // Ném lỗi nếu truy vấn thất bại.
        public List<KhachHangDTO> LayTatCa()
        {
            var danhSachKhachHang = new List<KhachHangDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"SELECT MaKhachHang, TenKhachHang, NgaySinh, SoDienThoai, DiaChi, Email, GioiTinh 
                                         FROM KHACH_HANG";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachKhachHang.Add(TaoKhachHangTuReader(reader));
                            }
                        }
                    }
                }
                return danhSachKhachHang;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách khách hàng: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khách hàng: {ex.Message}", ex);
            }
        }

        // Tìm kiếm khách hàng theo từ khóa dựa trên các trường MaKhachHang, TenKhachHang, SoDienThoai, DiaChi, Email, GioiTinh.
        // Trả về danh sách các khách hàng phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<KhachHangDTO> TimKiem(string tuKhoa)
        {
            var danhSachKhachHang = new List<KhachHangDTO>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"SELECT MaKhachHang, TenKhachHang, NgaySinh, SoDienThoai, DiaChi, Email, GioiTinh 
                                         FROM KHACH_HANG 
                                         WHERE MaKhachHang LIKE @TuKhoa 
                                            OR TenKhachHang LIKE @TuKhoa 
                                            OR SoDienThoai LIKE @TuKhoa 
                                            OR DiaChi LIKE @TuKhoa 
                                            OR Email LIKE @TuKhoa 
                                            OR GioiTinh LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachKhachHang.Add(TaoKhachHangTuReader(reader));
                            }
                        }
                    }
                }
                return danhSachKhachHang;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm khách hàng: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", ex);
            }
        }

        // Thêm khách hàng mới vào cơ sở dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình thêm.
        public bool Them(KhachHangDTO khachHang)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"INSERT INTO KHACH_HANG (MaKhachHang, TenKhachHang, NgaySinh, SoDienThoai, DiaChi, Email, GioiTinh) 
                                         VALUES (@MaKhachHang, @TenKhachHang, @NgaySinh, @SoDienThoai, @DiaChi, @Email, @GioiTinh)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhachHang", khachHang.MaKhachHang);
                        cmd.Parameters.AddWithValue("@TenKhachHang", khachHang.TenKhachHang);
                        cmd.Parameters.AddWithValue("@NgaySinh", khachHang.NgaySinh ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoDienThoai", khachHang.SoDienThoai);
                        cmd.Parameters.AddWithValue("@DiaChi", khachHang.DiaChi);
                        cmd.Parameters.AddWithValue("@Email", khachHang.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GioiTinh", khachHang.GioiTinh ?? (object)DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã khách hàng, số điện thoại hoặc email đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm khách hàng: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm khách hàng: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin khách hàng trong cơ sở dữ liệu.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình cập nhật.
        public bool CapNhat(KhachHangDTO khachHang)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"UPDATE KHACH_HANG 
                                         SET TenKhachHang = @TenKhachHang, NgaySinh = @NgaySinh, SoDienThoai = @SoDienThoai, 
                                             DiaChi = @DiaChi, Email = @Email, GioiTinh = @GioiTinh 
                                         WHERE MaKhachHang = @MaKhachHang";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhachHang", khachHang.MaKhachHang);
                        cmd.Parameters.AddWithValue("@TenKhachHang", khachHang.TenKhachHang);
                        cmd.Parameters.AddWithValue("@NgaySinh", khachHang.NgaySinh ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoDienThoai", khachHang.SoDienThoai);
                        cmd.Parameters.AddWithValue("@DiaChi", khachHang.DiaChi);
                        cmd.Parameters.AddWithValue("@Email", khachHang.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GioiTinh", khachHang.GioiTinh ?? (object)DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Số điện thoại hoặc email đã tồn tại.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật khách hàng: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật khách hàng: {ex.Message}", ex);
            }
        }

        // Xóa khách hàng khỏi cơ sở dữ liệu dựa trên mã khách hàng.
        // Trả về true nếu tìm thấy và xóa thành công, false nếu không tìm thấy.
        // Ném lỗi nếu có vấn đề trong quá trình xóa.
        public bool Xoa(string maKhachHang)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Kiểm tra khách hàng có liên quan đến hóa đơn hoặc đơn hàng không
                    if (KiemTraKhachHangDaDuocSuDung(maKhachHang, conn))
                    {
                        throw new Exception("Không thể xóa khách hàng vì đã có hóa đơn hoặc đơn đặt hàng liên quan.");
                    }

                    const string query = @"DELETE FROM KHACH_HANG WHERE MaKhachHang = @MaKhachHang";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new Exception("Không thể xóa khách hàng vì đã có liên quan đến dữ liệu khác.", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi xóa khách hàng: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa khách hàng: {ex.Message}", ex);
            }
        }

        // Kiểm tra xem khách hàng có được sử dụng trong hóa đơn hoặc đơn hàng không.
        // Trả về true nếu khách hàng đang được sử dụng, false nếu không.
        // Ném lỗi nếu truy vấn thất bại.
        private bool KiemTraKhachHangDaDuocSuDung(string maKhachHang, SqlConnection conn)
        {
            try
            {
                const string query = @"SELECT COUNT(*) 
                                     FROM HOA_DON_BAN 
                                     WHERE MaKhachHang = @MaKhachHang";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (Convert.ToInt32(reader[0]) > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (SqlException ex) when (ex.Number == 207) // Bảng không tồn tại
            {
                return false;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi kiểm tra khách hàng: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra khách hàng: {ex.Message}", ex);
            }
        }

        // Tạo đối tượng KhachHangDTO từ SqlDataReader.
        // Dùng để ánh xạ dữ liệu từ cơ sở dữ liệu sang DTO.
        private KhachHangDTO TaoKhachHangTuReader(SqlDataReader reader)
        {
            return new KhachHangDTO
            {
                MaKhachHang = reader["MaKhachHang"].ToString(),
                TenKhachHang = reader["TenKhachHang"].ToString(),
                NgaySinh = reader["NgaySinh"] == DBNull.Value ? null : (DateTime?)reader["NgaySinh"],
                SoDienThoai = reader["SoDienThoai"].ToString(),
                DiaChi = reader["DiaChi"].ToString(),
                Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                GioiTinh = reader["GioiTinh"] == DBNull.Value ? null : reader["GioiTinh"].ToString()
            };
        }
    }
}