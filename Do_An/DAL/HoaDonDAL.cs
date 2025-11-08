using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Lớp truy xuất dữ liệu cho hóa đơn và chi tiết hóa đơn.
    // Chứa các phương thức để tương tác với bảng HOA_DON_BAN và CHI_TIET_HOA_DON_BAN trong cơ sở dữ liệu.
    public class HoaDonDAL
    {
        private readonly string _connectionString;

        // Khởi tạo HoaDonDAL, lấy chuỗi kết nối từ App.config.
        public HoaDonDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy danh sách tất cả hóa đơn từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng HoaDonDTO.
        public List<HoaDonDTO> LayDanhSachHoaDon()
        {
            var hoaDonList = new List<HoaDonDTO>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        SELECT hdb.MaHoaDon, hdb.NgayLapHoaDon, hdb.TongTien, hdb.MaNhanVien, nv.HoTen,
                               hdb.MaKhuyenMai, km.TenKhuyenMai, hdb.MaKhachHang, kh.TenKhachHang,
                               hdb.DiaChi, hdb.GhiChu
                        FROM HOA_DON_BAN hdb
                        LEFT JOIN NHAN_VIEN nv ON hdb.MaNhanVien = nv.MaNhanVien
                        LEFT JOIN KHUYEN_MAI km ON hdb.MaKhuyenMai = km.MaKhuyenMai
                        LEFT JOIN KHACH_HANG kh ON hdb.MaKhachHang = kh.MaKhachHang";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                hoaDonList.Add(new HoaDonDTO
                                {
                                    MaHoaDon = reader["MaHoaDon"].ToString(),
                                    NgayLapHoaDon = Convert.ToDateTime(reader["NgayLapHoaDon"]),
                                    TongTien = Convert.ToDecimal(reader["TongTien"]),
                                    MaNhanVien = reader["MaNhanVien"].ToString(),
                                    HoTen = reader["HoTen"] == DBNull.Value ? null : reader["HoTen"].ToString(),
                                    MaKhuyenMai = reader["MaKhuyenMai"] == DBNull.Value ? null : reader["MaKhuyenMai"].ToString(),
                                    TenKhuyenMai = reader["TenKhuyenMai"] == DBNull.Value ? null : reader["TenKhuyenMai"].ToString(),
                                    MaKhachHang = reader["MaKhachHang"].ToString(),
                                    TenKhachHang = reader["TenKhachHang"] == DBNull.Value ? null : reader["TenKhachHang"].ToString(),
                                    DiaChi = reader["DiaChi"] == DBNull.Value ? null : reader["DiaChi"].ToString(),
                                    GhiChu = reader["GhiChu"] == DBNull.Value ? null : reader["GhiChu"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách hóa đơn: {ex.Message}", ex);
            }
            return hoaDonList;
        }

        // Thêm một hóa đơn mới vào cơ sở dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemHoaDon(HoaDonDTO hoaDon)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        INSERT INTO HOA_DON_BAN (MaHoaDon, NgayLapHoaDon, TongTien, MaNhanVien, MaKhuyenMai, 
                                                MaKhachHang, DiaChi, GhiChu)
                        VALUES (@MaHoaDon, @NgayLapHoaDon, @TongTien, @MaNhanVien, @MaKhuyenMai, 
                                @MaKhachHang, @DiaChi, @GhiChu)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", hoaDon.MaHoaDon);
                        cmd.Parameters.AddWithValue("@NgayLapHoaDon", hoaDon.NgayLapHoaDon);
                        cmd.Parameters.AddWithValue("@TongTien", hoaDon.TongTien);
                        cmd.Parameters.AddWithValue("@MaNhanVien", hoaDon.MaNhanVien);
                        cmd.Parameters.AddWithValue("@MaKhuyenMai", (object)hoaDon.MaKhuyenMai ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaKhachHang", hoaDon.MaKhachHang);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)hoaDon.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@GhiChu", (object)hoaDon.GhiChu ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm hóa đơn: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin hóa đơn trong cơ sở dữ liệu.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        public bool CapNhatHoaDon(HoaDonDTO hoaDon)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        UPDATE HOA_DON_BAN
                        SET NgayLapHoaDon = @NgayLapHoaDon, TongTien = @TongTien, MaNhanVien = @MaNhanVien,
                            MaKhuyenMai = @MaKhuyenMai, MaKhachHang = @MaKhachHang, DiaChi = @DiaChi,
                            GhiChu = @GhiChu
                        WHERE MaHoaDon = @MaHoaDon";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", hoaDon.MaHoaDon);
                        cmd.Parameters.AddWithValue("@NgayLapHoaDon", hoaDon.NgayLapHoaDon);
                        cmd.Parameters.AddWithValue("@TongTien", hoaDon.TongTien);
                        cmd.Parameters.AddWithValue("@MaNhanVien", hoaDon.MaNhanVien);
                        cmd.Parameters.AddWithValue("@MaKhuyenMai", (object)hoaDon.MaKhuyenMai ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaKhachHang", hoaDon.MaKhachHang);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)hoaDon.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@GhiChu", (object)hoaDon.GhiChu ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật hóa đơn: {ex.Message}", ex);
            }
        }

        // Xóa hóa đơn khỏi cơ sở dữ liệu dựa trên mã hóa đơn.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaHoaDon(string maHoaDon)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "DELETE FROM HOA_DON_BAN WHERE MaHoaDon = @MaHoaDon";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa hóa đơn: {ex.Message}", ex);
            }
        }

        // Lấy danh sách chi tiết hóa đơn dựa trên mã hóa đơn.
        // Trả về danh sách các đối tượng ChiTietHoaDonDTO.
        public List<ChiTietHoaDonDTO> LayChiTietHoaDon(string maHoaDon)
        {
            var chiTietList = new List<ChiTietHoaDonDTO>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        SELECT cthdb.MaHoaDon, cthdb.MaSP, sp.TenSP, cthdb.SoLuongBan AS SoLuong, 
                               cthdb.DonGia, cthdb.ThanhTien
                        FROM CHI_TIET_HOA_DON_BAN cthdb
                        JOIN SAN_PHAM sp ON cthdb.MaSP = sp.MaSP
                        WHERE cthdb.MaHoaDon = @MaHoaDon";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chiTietList.Add(new ChiTietHoaDonDTO
                                {
                                    MaHoaDon = reader["MaHoaDon"].ToString(),
                                    MaSanPham = reader["MaSP"].ToString(),
                                    TenSanPham = reader["TenSP"] == DBNull.Value ? null : reader["TenSP"].ToString(),
                                    SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                    DonGia = Convert.ToDecimal(reader["DonGia"]),
                                    ThanhTien = Convert.ToDecimal(reader["ThanhTien"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết hóa đơn: {ex.Message}", ex);
            }
            return chiTietList;
        }

        // Thêm chi tiết hóa đơn mới vào cơ sở dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemChiTietHoaDon(ChiTietHoaDonDTO chiTiet)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        INSERT INTO CHI_TIET_HOA_DON_BAN (MaHoaDon, MaSP, SoLuongBan, DonGia, ThanhTien)
                        VALUES (@MaHoaDon, @MaSP, @SoLuong, @DonGia, @ThanhTien)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", chiTiet.MaHoaDon);
                        cmd.Parameters.AddWithValue("@MaSP", chiTiet.MaSanPham);
                        cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                        cmd.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                        cmd.Parameters.AddWithValue("@ThanhTien", chiTiet.ThanhTien);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm chi tiết hóa đơn: {ex.Message}", ex);
            }
        }

        // Xóa chi tiết hóa đơn khỏi cơ sở dữ liệu dựa trên mã hóa đơn và mã sản phẩm.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaChiTietHoaDon(string maHoaDon, string maSanPham)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "DELETE FROM CHI_TIET_HOA_DON_BAN WHERE MaHoaDon = @MaHoaDon AND MaSP = @MaSP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                        cmd.Parameters.AddWithValue("@MaSP", maSanPham);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chi tiết hóa đơn: {ex.Message}", ex);
            }
        }
        // Lấy danh sách khách hàng từ cơ sở dữ liệu.
        // Trả về Dictionary ánh xạ tên khách hàng với mã khách hàng.
        public Dictionary<string, string> LayDanhSachKhachHang()
        {
            var khachHangList = new Dictionary<string, string>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaKhachHang, TenKhachHang FROM KHACH_HANG";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tenKhachHang = reader["TenKhachHang"].ToString();
                                var maKhachHang = reader["MaKhachHang"].ToString();

                                if (!string.IsNullOrEmpty(tenKhachHang) && !khachHangList.ContainsKey(tenKhachHang))
                                {
                                    khachHangList.Add(tenKhachHang, maKhachHang);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khách hàng: {ex.Message}", ex);
            }
            return khachHangList;
        }

        // Lấy danh sách nhân viên từ cơ sở dữ liệu.
        // Trả về Dictionary ánh xạ tên nhân viên với mã nhân viên.
        public Dictionary<string, string> LayDanhSachNhanVien()
        {
            var nhanVienList = new Dictionary<string, string>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaNhanVien, HoTen FROM NHAN_VIEN";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var hoTen = reader["HoTen"].ToString();
                                var maNhanVien = reader["MaNhanVien"].ToString();

                                if (!string.IsNullOrEmpty(hoTen) && !nhanVienList.ContainsKey(hoTen))
                                {
                                    nhanVienList.Add(hoTen, maNhanVien);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
            return nhanVienList;
        }

        // Lấy danh sách sản phẩm từ cơ sở dữ liệu.
        // Trả về Dictionary ánh xạ tên sản phẩm với mã sản phẩm.
        public Dictionary<string, string> LayDanhSachSanPham()
        {
            var sanPhamList = new Dictionary<string, string>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaSP, TenSP FROM SAN_PHAM";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tenSP = reader["TenSP"].ToString();
                                var maSP = reader["MaSP"].ToString();

                                if (!string.IsNullOrEmpty(tenSP) && !sanPhamList.ContainsKey(tenSP))
                                {
                                    sanPhamList.Add(tenSP, maSP);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}", ex);
            }
            return sanPhamList;
        }

        // Lấy danh sách khuyến mãi còn hiệu lực từ cơ sở dữ liệu.
        // Trả về Dictionary ánh xạ tên khuyến mãi với mã khuyến mãi.
        public Dictionary<string, string> LayDanhSachKhuyenMai()
        {
            var khuyenMaiList = new Dictionary<string, string>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaKhuyenMai, TenKhuyenMai FROM KHUYEN_MAI WHERE GETDATE() BETWEEN NgayBatDau AND NgayKetThuc";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tenKhuyenMai = reader["TenKhuyenMai"].ToString();
                                var maKhuyenMai = reader["MaKhuyenMai"].ToString();

                                if (!string.IsNullOrEmpty(tenKhuyenMai) && !khuyenMaiList.ContainsKey(tenKhuyenMai))
                                {
                                    khuyenMaiList.Add(tenKhuyenMai, maKhuyenMai);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khuyến mãi: {ex.Message}", ex);
            }
            return khuyenMaiList;
        }

        // Lấy đơn giá của sản phẩm dựa trên mã sản phẩm.
        // Trả về đơn giá hoặc 0 nếu không tìm thấy.
        public decimal LayDonGiaSanPham(string maSanPham)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT DonGiaBan FROM SAN_PHAM WHERE MaSP = @MaSP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSP", maSanPham);
                        var result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn giá sản phẩm: {ex.Message}", ex);
            }
        }

        // Lấy phần trăm khuyến mãi dựa trên mã khuyến mãi.
        // Trả về phần trăm hoặc 0 nếu không tìm thấy.
        public decimal LayPhanTramKhuyenMai(string maKhuyenMai)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT PhanTramKhuyenMai FROM KHUYEN_MAI WHERE MaKhuyenMai = @MaKhuyenMai";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhuyenMai", maKhuyenMai);
                        var result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phần trăm khuyến mãi: {ex.Message}", ex);
            }
        }
        // Lấy số lượng tồn kho của sản phẩm dựa trên mã sản phẩm.
        // Trả về số lượng tồn kho hoặc 0 nếu không tìm thấy.
        public int LaySoLuongTonSanPham(string maSanPham)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT SoLuongTon FROM SAN_PHAM WHERE MaSP = @MaSP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSP", maSanPham);
                        var result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy số lượng tồn kho của sản phẩm: {ex.Message}", ex);
            }
        }
    }
}