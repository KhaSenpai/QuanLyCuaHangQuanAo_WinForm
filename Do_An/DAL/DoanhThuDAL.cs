using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class DoanhThuDAL
    {
        private readonly string _connectionString;

        public DoanhThuDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        public List<DoanhThuDTO> GetDoanhThuByDate(DateTime date)
        {
            List<DoanhThuDTO> list = new List<DoanhThuDTO>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT hdb.MaHoaDon, hdb.NgayLapHoaDon, hdb.TongTien,
                           kh.TenKhachHang, nv.HoTen AS TenNhanVien,
                           km.TenKhuyenMai, ISNULL(km.PhanTramKhuyenMai, 0) AS PhanTramKhuyenMai
                    FROM HOA_DON_BAN hdb
                    LEFT JOIN KHACH_HANG kh ON hdb.MaKhachHang = kh.MaKhachHang
                    LEFT JOIN NHAN_VIEN nv ON hdb.MaNhanVien = nv.MaNhanVien
                    LEFT JOIN KHUYEN_MAI km ON hdb.MaKhuyenMai = km.MaKhuyenMai
                    WHERE CAST(hdb.NgayLapHoaDon AS DATE) = @Date
                    ORDER BY hdb.NgayLapHoaDon DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Date", date.Date);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DoanhThuDTO
                    {
                        MaHoaDon = reader["MaHoaDon"].ToString(),
                        NgayLapHoaDon = Convert.ToDateTime(reader["NgayLapHoaDon"]),
                        TongTien = Convert.ToDecimal(reader["TongTien"]),
                        TenKhachHang = reader["TenKhachHang"].ToString(),
                        TenNhanVien = reader["TenNhanVien"].ToString(),
                        TenKhuyenMai = reader["TenKhuyenMai"]?.ToString() ?? "Không có",
                        PhanTramKhuyenMai = Convert.ToDecimal(reader["PhanTramKhuyenMai"])
                    });
                }
            }
            return list;
        }

        public List<DoanhThuTongHopDTO> GetDoanhThuByMonth(int year, int month)
        {
            var list = new List<DoanhThuTongHopDTO>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT CONVERT(VARCHAR, DAY(NgayLapHoaDon)) AS ThoiGian, 
                           SUM(TongTien) AS TongDoanhThu
                    FROM HOA_DON_BAN
                    WHERE YEAR(NgayLapHoaDon) = @Year AND MONTH(NgayLapHoaDon) = @Month
                    GROUP BY DAY(NgayLapHoaDon)
                    ORDER BY DAY(NgayLapHoaDon)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DoanhThuTongHopDTO
                    {
                        ThoiGian = reader["ThoiGian"].ToString(),
                        TongDoanhThu = Convert.ToDecimal(reader["TongDoanhThu"])
                    });
                }
            }
            return list;
        }

        public List<DoanhThuTongHopDTO> GetDoanhThuByYear(int year)
        {
            var list = new List<DoanhThuTongHopDTO>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT CONVERT(VARCHAR, MONTH(NgayLapHoaDon)) AS ThoiGian,
                           SUM(TongTien) AS TongDoanhThu
                    FROM HOA_DON_BAN
                    WHERE YEAR(NgayLapHoaDon) = @Year
                    GROUP BY MONTH(NgayLapHoaDon)
                    ORDER BY MONTH(NgayLapHoaDon)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Year", year);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DoanhThuTongHopDTO
                    {
                        ThoiGian = reader["ThoiGian"].ToString(),
                        TongDoanhThu = Convert.ToDecimal(reader["TongDoanhThu"])
                    });
                }
            }
            return list;
        }

        public decimal GetTongDoanhThuByDate(DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT SUM(TongTien) FROM HOA_DON_BAN WHERE CAST(NgayLapHoaDon AS DATE) = @Date";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Date", date.Date);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public decimal GetTongDoanhThuByMonth(int year, int month)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT SUM(TongTien) FROM HOA_DON_BAN WHERE YEAR(NgayLapHoaDon) = @Year AND MONTH(NgayLapHoaDon) = @Month";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
        public List<DoanhThuDTO> GetDoanhThuChiTietByMonth(int year, int month)
        {
            List<DoanhThuDTO> list = new List<DoanhThuDTO>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT hdb.MaHoaDon, hdb.NgayLapHoaDon, hdb.TongTien,
                   kh.TenKhachHang, nv.HoTen AS TenNhanVien,
                   km.TenKhuyenMai, ISNULL(km.PhanTramKhuyenMai, 0) AS PhanTramKhuyenMai
            FROM HOA_DON_BAN hdb
            LEFT JOIN KHACH_HANG kh ON hdb.MaKhachHang = kh.MaKhachHang
            LEFT JOIN NHAN_VIEN nv ON hdb.MaNhanVien = nv.MaNhanVien
            LEFT JOIN KHUYEN_MAI km ON hdb.MaKhuyenMai = km.MaKhuyenMai
            WHERE YEAR(hdb.NgayLapHoaDon) = @Year AND MONTH(hdb.NgayLapHoaDon) = @Month
            ORDER BY hdb.NgayLapHoaDon DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DoanhThuDTO
                    {
                        MaHoaDon = reader["MaHoaDon"].ToString(),
                        NgayLapHoaDon = Convert.ToDateTime(reader["NgayLapHoaDon"]),
                        TongTien = Convert.ToDecimal(reader["TongTien"]),
                        TenKhachHang = reader["TenKhachHang"].ToString(),
                        TenNhanVien = reader["TenNhanVien"].ToString(),
                        TenKhuyenMai = reader["TenKhuyenMai"]?.ToString() ?? "Không có",
                        PhanTramKhuyenMai = Convert.ToDecimal(reader["PhanTramKhuyenMai"])
                    });
                }
            }
            return list;
        }
        public decimal GetTongDoanhThuByYear(int year)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT SUM(TongTien) FROM HOA_DON_BAN WHERE YEAR(NgayLapHoaDon) = @Year";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Year", year);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
    }
}