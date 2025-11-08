using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    // Lớp truy xuất dữ liệu cho phiếu nhập và chi tiết phiếu nhập.
    public class PhieuNhapDAL
    {
        private readonly string _connectionString;

        // Khởi tạo PhieuNhapDAL, lấy chuỗi kết nối từ App.config.
        public PhieuNhapDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        // Lấy danh sách tất cả phiếu nhập từ cơ sở dữ liệu.
        // Trả về danh sách các đối tượng PhieuNhapDTO.
        public List<PhieuNhapDTO> LayDanhSachPhieuNhap()
        {
            var phieuNhapList = new List<PhieuNhapDTO>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        SELECT pn.MaPhieuNhap, pn.MaNCC, ncc.TenNCC, pn.MaNhanVien, nv.HoTen, 
                               pn.NgayLapPhieuNhap, pn.TongTien, pn.DiaChi, pn.GhiChu
                        FROM PHIEU_NHAP pn
                        LEFT JOIN NHA_CUNG_CAP ncc ON pn.MaNCC = ncc.MaNCC
                        LEFT JOIN NHAN_VIEN nv ON pn.MaNhanVien = nv.MaNhanVien";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                phieuNhapList.Add(new PhieuNhapDTO
                                {
                                    MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                                    MaNCC = reader["MaNCC"].ToString(),
                                    MaNhanVien = reader["MaNhanVien"].ToString(),
                                    NgayLapPhieuNhap = reader["NgayLapPhieuNhap"] == DBNull.Value ? null : (DateTime?)reader["NgayLapPhieuNhap"],
                                    TongTien = Convert.ToDecimal(reader["TongTien"]),
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
                throw new Exception($"Lỗi khi lấy danh sách phiếu nhập: {ex.Message}", ex);
            }
            return phieuNhapList;
        }

        // Lấy danh sách chi tiết phiếu nhập dựa trên mã phiếu nhập.
        // Trả về danh sách các đối tượng ChiTietPhieuNhapDTO.
        public List<ChiTietPhieuNhapDTO> LayChiTietPhieuNhap(string maPhieuNhap)
        {
            var chiTietList = new List<ChiTietPhieuNhapDTO>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        SELECT ctpn.MaPhieuNhap, ctpn.MaSP, sp.TenSP, ctpn.SoLuong, ctpn.DonGia, ctpn.ThanhTien
                        FROM CHI_TIET_PHIEU_NHAP ctpn
                        JOIN SAN_PHAM sp ON ctpn.MaSP = sp.MaSP
                        WHERE ctpn.MaPhieuNhap = @MaPhieuNhap";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chiTietList.Add(new ChiTietPhieuNhapDTO
                                {
                                    MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                                    MaSanPham = reader["MaSP"].ToString(),
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
                throw new Exception($"Lỗi khi lấy chi tiết phiếu nhập: {ex.Message}", ex);
            }
            return chiTietList;
        }

        // Thêm một phiếu nhập mới vào cơ sở dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemPhieuNhap(PhieuNhapDTO phieuNhap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        INSERT INTO PHIEU_NHAP (MaPhieuNhap, MaNCC, MaNhanVien, NgayLapPhieuNhap, TongTien, DiaChi, GhiChu)
                        VALUES (@MaPhieuNhap, @MaNCC, @MaNhanVien, @NgayLapPhieuNhap, @TongTien, @DiaChi, @GhiChu)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", phieuNhap.MaPhieuNhap);
                        cmd.Parameters.AddWithValue("@MaNCC", phieuNhap.MaNCC);
                        cmd.Parameters.AddWithValue("@MaNhanVien", phieuNhap.MaNhanVien);
                        cmd.Parameters.AddWithValue("@NgayLapPhieuNhap", phieuNhap.NgayLapPhieuNhap ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TongTien", phieuNhap.TongTien);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)phieuNhap.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@GhiChu", (object)phieuNhap.GhiChu ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                    throw new Exception("Mã phiếu nhập đã tồn tại!");
                throw new Exception($"Lỗi SQL khi thêm phiếu nhập: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm phiếu nhập: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin phiếu nhập trong cơ sở dữ liệu.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        public bool CapNhatPhieuNhap(PhieuNhapDTO phieuNhap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        UPDATE PHIEU_NHAP 
                        SET MaNCC = @MaNCC, MaNhanVien = @MaNhanVien, NgayLapPhieuNhap = @NgayLapPhieuNhap, 
                            TongTien = @TongTien, DiaChi = @DiaChi, GhiChu = @GhiChu
                        WHERE MaPhieuNhap = @MaPhieuNhap";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", phieuNhap.MaPhieuNhap);
                        cmd.Parameters.AddWithValue("@MaNCC", phieuNhap.MaNCC);
                        cmd.Parameters.AddWithValue("@MaNhanVien", phieuNhap.MaNhanVien);
                        cmd.Parameters.AddWithValue("@NgayLapPhieuNhap", phieuNhap.NgayLapPhieuNhap ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TongTien", phieuNhap.TongTien);
                        cmd.Parameters.AddWithValue("@DiaChi", (object)phieuNhap.DiaChi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@GhiChu", (object)phieuNhap.GhiChu ?? DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật phiếu nhập: {ex.Message}", ex);
            }
        }

        // Xóa phiếu nhập khỏi cơ sở dữ liệu dựa trên mã phiếu nhập.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaPhieuNhap(string maPhieuNhap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Xóa chi tiết phiếu nhập trước
                    const string deleteChiTietQuery = "DELETE FROM CHI_TIET_PHIEU_NHAP WHERE MaPhieuNhap = @MaPhieuNhap";
                    using (var cmd = new SqlCommand(deleteChiTietQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        cmd.ExecuteNonQuery();
                    }

                    // Xóa phiếu nhập
                    const string query = "DELETE FROM PHIEU_NHAP WHERE MaPhieuNhap = @MaPhieuNhap";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa phiếu nhập: {ex.Message}", ex);
            }
        }

        // Thêm chi tiết phiếu nhập mới vào cơ sở dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemChiTietPhieuNhap(ChiTietPhieuNhapDTO chiTiet)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = @"
                        INSERT INTO CHI_TIET_PHIEU_NHAP (MaPhieuNhap, MaSP, SoLuong, DonGia, ThanhTien)
                        VALUES (@MaPhieuNhap, @MaSP, @SoLuong, @DonGia, @ThanhTien)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", chiTiet.MaPhieuNhap);
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
                throw new Exception($"Lỗi khi thêm chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        // Xóa chi tiết phiếu nhập dựa trên mã phiếu nhập và mã sản phẩm.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaChiTietPhieuNhap(string maPhieuNhap, string maSanPham)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "DELETE FROM CHI_TIET_PHIEU_NHAP WHERE MaPhieuNhap = @MaPhieuNhap AND MaSP = @MaSP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        cmd.Parameters.AddWithValue("@MaSP", maSanPham);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        // Xóa tất cả chi tiết phiếu nhập dựa trên mã phiếu nhập.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaTatCaChiTietPhieuNhap(string maPhieuNhap)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "DELETE FROM CHI_TIET_PHIEU_NHAP WHERE MaPhieuNhap = @MaPhieuNhap";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa tất cả chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        // Lấy danh sách tên nhà cung cấp từ cơ sở dữ liệu.
        // Trả về Dictionary ánh xạ tên nhà cung cấp với mã nhà cung cấp.
        public Dictionary<string, string> LayDanhSachTenNhaCungCap()
        {
            var nhaCungCapDict = new Dictionary<string, string>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tenNCC = reader["TenNCC"].ToString();
                                var maNCC = reader["MaNCC"].ToString();
                                if (!string.IsNullOrEmpty(tenNCC) && !nhaCungCapDict.ContainsKey(tenNCC))
                                    nhaCungCapDict.Add(tenNCC, maNCC);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}", ex);
            }
            return nhaCungCapDict;
        }

        // Lấy danh sách tên nhân viên từ cơ sở dữ liệu.
        // Trả về Dictionary ánh xạ tên nhân viên với mã nhân viên.
        public Dictionary<string, string> LayDanhSachTenNhanVien()
        {
            var nhanVienDict = new Dictionary<string, string>();
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
                                if (!string.IsNullOrEmpty(hoTen) && !nhanVienDict.ContainsKey(hoTen))
                                    nhanVienDict.Add(hoTen, maNhanVien);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
            return nhanVienDict;
        }

        // Lấy danh sách tên sản phẩm từ cơ sở dữ liệu.
        // Trả về Dictionary ánh xạ tên sản phẩm với mã sản phẩm.
        public Dictionary<string, string> LayDanhSachTenSanPham()
        {
            var sanPhamDict = new Dictionary<string, string>();
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
                                if (!string.IsNullOrEmpty(tenSP) && !sanPhamDict.ContainsKey(tenSP))
                                    sanPhamDict.Add(tenSP, maSP);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}", ex);
            }
            return sanPhamDict;
        }

        // Sinh mã phiếu nhập mới dựa trên mã hiện có.
        // Trả về mã phiếu nhập dạng PNxxx (VD: PN001).
        public string SinhMaPhieuNhap()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT MAX(CAST(SUBSTRING(MaPhieuNhap, 3, LEN(MaPhieuNhap)) AS INT)) FROM PHIEU_NHAP WHERE MaPhieuNhap LIKE 'PN[0-9]%'";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        int nextId = (result == DBNull.Value ? 0 : Convert.ToInt32(result)) + 1;
                        return $"PN{nextId:D3}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi sinh mã phiếu nhập: {ex.Message}", ex);
            }
        }

        // Lấy đơn giá nhập của sản phẩm dựa trên mã sản phẩm.
        // Trả về đơn giá hoặc 0 nếu không tìm thấy.
        public decimal LayDonGiaNhap(string maSanPham)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT DonGiaNhap FROM SAN_PHAM WHERE MaSP = @MaSP";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSP", maSanPham);
                        var result = cmd.ExecuteScalar();
                        return result == null ? 0 : Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn giá nhập: {ex.Message}", ex);
            }
        }
    }
}