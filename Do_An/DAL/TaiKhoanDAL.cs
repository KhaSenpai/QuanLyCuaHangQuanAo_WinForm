using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class TaiKhoanDAL
    {
        private readonly string _connectionString;

        public TaiKhoanDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy toàn bộ danh sách tài khoản từ cơ sở dữ liệu
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

        // Tìm kiếm tài khoản theo từ khóa
        public List<TaiKhoanDTO> SearchTaiKhoan(string keyword)
        {
            try
            {
                var taiKhoans = new List<TaiKhoanDTO>();
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT tk.*, nv.HoTen AS TenNhanVien 
                                    FROM TAI_KHOAN tk 
                                    LEFT JOIN NHAN_VIEN nv ON tk.MaNhanVien = nv.MaNhanVien 
                                    WHERE tk.TenDangNhap LIKE @Keyword 
                                       OR tk.MaNhanVien LIKE @Keyword 
                                       OR nv.HoTen LIKE @Keyword";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                taiKhoans.Add(CreateTaiKhoanFromReader(reader));
                            }
                        }
                    }
                }
                return taiKhoans;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm tài khoản trong cơ sở dữ liệu", ex);
            }
        }

        // Xác thực đăng nhập
        public TaiKhoanDTO Login(string tenDangNhap, string matKhau)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"SELECT tk.*, nv.HoTen AS TenNhanVien 
                                    FROM TAI_KHOAN tk 
                                    LEFT JOIN NHAN_VIEN nv ON tk.MaNhanVien = nv.MaNhanVien 
                                    WHERE tk.TenDangNhap = @TenDangNhap AND tk.MatKhau = @MatKhau";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);
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
                throw new Exception("Lỗi khi xác thực đăng nhập", ex);
            }
        }

        // Thêm tài khoản mới vào cơ sở dữ liệu
        public bool AddTaiKhoan(TaiKhoanDTO taiKhoan)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO TAI_KHOAN (MaNhanVien, TenDangNhap, MatKhau, QuyenTruyCap) 
                                    VALUES (@MaNhanVien, @TenDangNhap, @MatKhau, @QuyenTruyCap)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNhanVien", taiKhoan.MaNhanVien);
                        cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoan.TenDangNhap);
                        cmd.Parameters.AddWithValue("@MatKhau", taiKhoan.MatKhau);
                        cmd.Parameters.AddWithValue("@QuyenTruyCap", taiKhoan.QuyenTruyCap ?? "User");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tài khoản vào cơ sở dữ liệu", ex);
            }
        }

        // Cập nhật thông tin tài khoản trong cơ sở dữ liệu
        public bool UpdateTaiKhoan(TaiKhoanDTO taiKhoan)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE TAI_KHOAN 
                                    SET MatKhau = @MatKhau, MaNhanVien = @MaNhanVien, QuyenTruyCap = @QuyenTruyCap
                                    WHERE TenDangNhap = @TenDangNhap";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoan.TenDangNhap);
                        cmd.Parameters.AddWithValue("@MatKhau", taiKhoan.MatKhau);
                        cmd.Parameters.AddWithValue("@MaNhanVien", taiKhoan.MaNhanVien);
                        cmd.Parameters.AddWithValue("@QuyenTruyCap", taiKhoan.QuyenTruyCap ?? "User");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tài khoản trong cơ sở dữ liệu", ex);
            }
        }

        // Xóa tài khoản khỏi cơ sở dữ liệu
        public bool DeleteTaiKhoan(string tenDangNhap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"DELETE FROM TAI_KHOAN WHERE TenDangNhap = @TenDangNhap";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tài khoản khỏi cơ sở dữ liệu", ex);
            }
        }

        // Tạo đối tượng TaiKhoanDTO từ SqlDataReader
        private TaiKhoanDTO CreateTaiKhoanFromReader(SqlDataReader reader)
        {
            return new TaiKhoanDTO
            {
                MaNhanVien = reader["MaNhanVien"].ToString(),
                TenDangNhap = reader["TenDangNhap"].ToString(),
                MatKhau = reader["MatKhau"].ToString(),
                QuyenTruyCap = reader["QuyenTruyCap"].ToString(),
                TenNhanVien = reader["TenNhanVien"]?.ToString() ?? ""
            };
        }
    }
}