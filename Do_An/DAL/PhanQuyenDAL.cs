using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public class PhanQuyenDAL
    {
        private readonly string _connectionString;

        public PhanQuyenDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy danh sách tất cả tài khoản
        public List<TaiKhoanDTO> GetAllTaiKhoan()
        {
            try
            {
                var taiKhoans = new List<TaiKhoanDTO>();
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT tk.*, nv.HoTen AS TenNhanVien 
                                    FROM TAI_KHOAN tk 
                                    LEFT JOIN NHAN_VIEN nv ON tk.MaNhanVien = nv.MaNhanVien";
                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taiKhoans.Add(CreateTaiKhoanFromReader(reader));
                        }
                    }
                }
                return taiKhoans;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tài khoản từ cơ sở dữ liệu", ex);
            }
        }

        // Lấy thông tin tài khoản theo tên đăng nhập
        public TaiKhoanDTO GetTaiKhoanByTenDangNhap(string tenDangNhap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT tk.*, nv.HoTen AS TenNhanVien 
                                    FROM TAI_KHOAN tk 
                                    LEFT JOIN NHAN_VIEN nv ON tk.MaNhanVien = nv.MaNhanVien 
                                    WHERE tk.TenDangNhap = @TenDangNhap";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateTaiKhoanFromReader(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin tài khoản từ cơ sở dữ liệu", ex);
            }
        }

        // Cập nhật quyền truy cập cho tài khoản
        public bool UpdateQuyenTruyCap(string tenDangNhap, string quyenTruyCap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE TAI_KHOAN 
                                    SET QuyenTruyCap = @QuyenTruyCap
                                    WHERE TenDangNhap = @TenDangNhap";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        cmd.Parameters.AddWithValue("@QuyenTruyCap", quyenTruyCap ?? "User");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật quyền truy cập trong cơ sở dữ liệu", ex);
            }
        }

        // Tạo đối tượng TaiKhoanDTO từ SqlDataReader
        private TaiKhoanDTO CreateTaiKhoanFromReader(SqlDataReader reader)
        {
            return new TaiKhoanDTO
            {
                MaNhanVien = reader["MaNhanVien"]?.ToString(),
                TenDangNhap = reader["TenDangNhap"].ToString(),
                MatKhau = reader["MatKhau"].ToString(),
                QuyenTruyCap = reader["QuyenTruyCap"]?.ToString() ?? "User",
                TenNhanVien = reader["TenNhanVien"]?.ToString() ?? ""
            };
        }
    }
}