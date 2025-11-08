using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class ThongKeSanPhamDAL
    {
        // Chuỗi kết nối tới cơ sở dữ liệu
        private readonly string _connectionString;

        // Constructor khởi tạo chuỗi kết nối từ file cấu hình
        public ThongKeSanPhamDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy danh sách tất cả sản phẩm
        public List<ThongKeSanPhamDTO> GetAllSanPham()
        {
            var sanPhamList = new List<ThongKeSanPhamDTO>();
            // Câu truy vấn SQL lấy thông tin sản phẩm và các bảng liên quan
            string query = @"
                SELECT sp.MaSP, sp.TenSP, sp.SoLuongTon, sp.DonGiaBan, sp.MauSac, sp.KichCo,
                       th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC
                FROM SAN_PHAM sp
                JOIN THUONG_HIEU th ON sp.MaThuongHieu = th.MaThuongHieu
                JOIN LOAI_SAN_PHAM lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
                JOIN CHAT_LIEU cl ON sp.MaChatLieu = cl.MaChatLieu
                JOIN NHA_CUNG_CAP ncc ON sp.MaNCC = ncc.MaNCC";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sp = new ThongKeSanPhamDTO
                            {
                                MaSP = reader["MaSP"].ToString(),
                                TenSP = reader["TenSP"].ToString(),
                                SoLuongTon = Convert.ToInt32(reader["SoLuongTon"]),
                                DonGiaBan = Convert.ToDecimal(reader["DonGiaBan"]),
                                MauSac = reader["MauSac"].ToString(),
                                KichCo = reader["KichCo"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString(),
                                TenNCC = reader["TenNCC"].ToString()
                            };
                            sanPhamList.Add(sp);
                        }
                    }
                }
            }
            return sanPhamList;
        }

        // Lấy danh sách sản phẩm đã bán trong khoảng thời gian
        public List<ThongKeSanPhamDTO> GetSanPhamDaBan(DateTime startDate, DateTime endDate)
        {
            var sanPhamList = new List<ThongKeSanPhamDTO>();
            // Câu truy vấn SQL lấy thông tin sản phẩm đã bán
            string query = @"
                SELECT sp.MaSP, sp.TenSP, SUM(cthdb.SoLuongBan) AS SoLuongBan, sp.DonGiaBan,
                       sp.MauSac, sp.KichCo, th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC
                FROM CHI_TIET_HOA_DON_BAN cthdb
                JOIN SAN_PHAM sp ON cthdb.MaSP = sp.MaSP
                JOIN HOA_DON_BAN hdb ON cthdb.MaHoaDon = hdb.MaHoaDon
                JOIN THUONG_HIEU th ON sp.MaThuongHieu = th.MaThuongHieu
                JOIN LOAI_SAN_PHAM lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
                JOIN CHAT_LIEU cl ON sp.MaChatLieu = cl.MaChatLieu
                JOIN NHA_CUNG_CAP ncc ON sp.MaNCC = ncc.MaNCC
                WHERE hdb.NgayLapHoaDon BETWEEN @StartDate AND @EndDate
                GROUP BY sp.MaSP, sp.TenSP, sp.DonGiaBan, sp.MauSac, sp.KichCo,
                         th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.AddDays(1).AddTicks(-1)); // Bao gồm cả ngày cuối
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sp = new ThongKeSanPhamDTO
                            {
                                MaSP = reader["MaSP"].ToString(),
                                TenSP = reader["TenSP"].ToString(),
                                SoLuongBan = Convert.ToInt32(reader["SoLuongBan"]),
                                DonGiaBan = Convert.ToDecimal(reader["DonGiaBan"]),
                                MauSac = reader["MauSac"].ToString(),
                                KichCo = reader["KichCo"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString(),
                                TenNCC = reader["TenNCC"].ToString()
                            };
                            sanPhamList.Add(sp);
                        }
                    }
                }
            }
            return sanPhamList;
        }

        // Lấy danh sách sản phẩm tồn ít (số lượng <= 5)
        public List<ThongKeSanPhamDTO> GetSanPhamTonIt()
        {
            var sanPhamList = new List<ThongKeSanPhamDTO>();
            // Câu truy vấn SQL lấy sản phẩm có số lượng tồn <= 5
            string query = @"
                SELECT sp.MaSP, sp.TenSP, sp.SoLuongTon, sp.DonGiaBan, sp.MauSac, sp.KichCo,
                       th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC
                FROM SAN_PHAM sp
                JOIN THUONG_HIEU th ON sp.MaThuongHieu = th.MaThuongHieu
                JOIN LOAI_SAN_PHAM lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
                JOIN CHAT_LIEU cl ON sp.MaChatLieu = cl.MaChatLieu
                JOIN NHA_CUNG_CAP ncc ON sp.MaNCC = ncc.MaNCC
                WHERE sp.SoLuongTon <= 5";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sp = new ThongKeSanPhamDTO
                            {
                                MaSP = reader["MaSP"].ToString(),
                                TenSP = reader["TenSP"].ToString(),
                                SoLuongTon = Convert.ToInt32(reader["SoLuongTon"]),
                                DonGiaBan = Convert.ToDecimal(reader["DonGiaBan"]),
                                MauSac = reader["MauSac"].ToString(),
                                KichCo = reader["KichCo"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString(),
                                TenNCC = reader["TenNCC"].ToString()
                            };
                            sanPhamList.Add(sp);
                        }
                    }
                }
            }
            return sanPhamList;
        }

        // Lấy danh sách top 3 sản phẩm bán chạy trong khoảng thời gian
        public List<ThongKeSanPhamDTO> GetSanPhamBanChay(DateTime startDate, DateTime endDate)
        {
            var sanPhamList = new List<ThongKeSanPhamDTO>();
            // Câu truy vấn SQL lấy top 3 sản phẩm bán chạy theo số lượng bán và tổng tiền
            string query = @"
                SELECT TOP 3 
                    sp.MaSP, 
                    sp.TenSP, 
                    SUM(cthdb.SoLuongBan) AS SoLuongBan, 
                    sp.DonGiaBan,
                    SUM(cthdb.SoLuongBan * sp.DonGiaBan) AS TongTien,
                    sp.MauSac, 
                    sp.KichCo, 
                    th.TenThuongHieu, 
                    lsp.TenLoaiSP, 
                    cl.TenChatLieu, 
                    ncc.TenNCC
                FROM CHI_TIET_HOA_DON_BAN cthdb
                JOIN SAN_PHAM sp ON cthdb.MaSP = sp.MaSP
                JOIN HOA_DON_BAN hdb ON cthdb.MaHoaDon = hdb.MaHoaDon
                JOIN THUONG_HIEU th ON sp.MaThuongHieu = th.MaThuongHieu
                JOIN LOAI_SAN_PHAM lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
                JOIN CHAT_LIEU cl ON sp.MaChatLieu = cl.MaChatLieu
                JOIN NHA_CUNG_CAP ncc ON sp.MaNCC = ncc.MaNCC
                WHERE hdb.NgayLapHoaDon BETWEEN @StartDate AND @EndDate
                GROUP BY sp.MaSP, sp.TenSP, sp.DonGiaBan, sp.MauSac, sp.KichCo,
                         th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC
                ORDER BY SUM(cthdb.SoLuongBan) DESC, SUM(cthdb.SoLuongBan * sp.DonGiaBan) DESC";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.AddDays(1).AddTicks(-1)); // Bao gồm cả ngày cuối
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sp = new ThongKeSanPhamDTO
                            {
                                MaSP = reader["MaSP"].ToString(),
                                TenSP = reader["TenSP"].ToString(),
                                SoLuongBan = Convert.ToInt32(reader["SoLuongBan"]),
                                DonGiaBan = Convert.ToDecimal(reader["DonGiaBan"]),
                                TongTien = Convert.ToDecimal(reader["TongTien"]),
                                MauSac = reader["MauSac"].ToString(),
                                KichCo = reader["KichCo"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString(),
                                TenNCC = reader["TenNCC"].ToString()
                            };
                            sanPhamList.Add(sp);
                        }
                    }
                }
            }
            return sanPhamList;
        }

        // Lấy danh sách sản phẩm mới nhập trong 7 ngày gần nhất
        public List<ThongKeSanPhamDTO> GetSanPhamMoiNhap()
        {
            var sanPhamList = new List<ThongKeSanPhamDTO>();
            // Câu truy vấn SQL lấy sản phẩm mới nhập trong 7 ngày
            string query = @"
                SELECT sp.MaSP, sp.TenSP, sp.SoLuongTon, sp.DonGiaBan, sp.MauSac, sp.KichCo,
                       th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC,
                       MAX(pn.NgayLapPhieuNhap) AS NgayNhapGanNhat
                FROM SAN_PHAM sp
                JOIN CHI_TIET_PHIEU_NHAP ctpn ON sp.MaSP = ctpn.MaSP
                JOIN PHIEU_NHAP pn ON ctpn.MaPhieuNhap = pn.MaPhieuNhap
                JOIN THUONG_HIEU th ON sp.MaThuongHieu = th.MaThuongHieu
                JOIN LOAI_SAN_PHAM lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
                JOIN CHAT_LIEU cl ON sp.MaChatLieu = cl.MaChatLieu
                JOIN NHA_CUNG_CAP ncc ON sp.MaNCC = ncc.MaNCC
                GROUP BY sp.MaSP, sp.TenSP, sp.SoLuongTon, sp.DonGiaBan, sp.MauSac, sp.KichCo,
                         th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC
                HAVING MAX(pn.NgayLapPhieuNhap) >= DATEADD(DAY, -7, GETDATE())";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sp = new ThongKeSanPhamDTO
                            {
                                MaSP = reader["MaSP"].ToString(),
                                TenSP = reader["TenSP"].ToString(),
                                SoLuongTon = Convert.ToInt32(reader["SoLuongTon"]),
                                DonGiaBan = Convert.ToDecimal(reader["DonGiaBan"]),
                                MauSac = reader["MauSac"].ToString(),
                                KichCo = reader["KichCo"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString(),
                                TenNCC = reader["TenNCC"].ToString(),
                                NgayNhapGanNhat = Convert.ToDateTime(reader["NgayNhapGanNhat"])
                            };
                            sanPhamList.Add(sp);
                        }
                    }
                }
            }
            return sanPhamList;
        }
    }
}