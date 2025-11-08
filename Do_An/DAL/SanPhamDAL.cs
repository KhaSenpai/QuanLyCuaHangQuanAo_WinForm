using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SanPhamDAL
    {
        private readonly string _connectionString;

        public SanPhamDAL()
        {
            _connectionString = DatabaseConnection.ConnectionString;
        }

        public List<SanPhamDTO> LayTatCaSanPham()
        {
            var danhSachSanPham = new List<SanPhamDTO>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT sp.*, th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC, ha.MaHinhAnh, ha.DuongDanHinh
                    FROM SAN_PHAM sp
                    LEFT JOIN THUONG_HIEU th ON sp.MaThuongHieu = th.MaThuongHieu
                    LEFT JOIN LOAI_SAN_PHAM lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
                    LEFT JOIN CHAT_LIEU cl ON sp.MaChatLieu = cl.MaChatLieu
                    LEFT JOIN NHA_CUNG_CAP ncc ON sp.MaNCC = ncc.MaNCC
                    LEFT JOIN HINH_ANH ha ON sp.MaSP = ha.MaSP";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sanPham = new SanPhamDTO
                            {
                                MaSP = reader["MaSP"].ToString(),
                                TenSP = reader["TenSP"].ToString(),
                                SoLuongTon = Convert.ToInt32(reader["SoLuongTon"]),
                                DonGiaBan = Convert.ToDecimal(reader["DonGiaBan"]),
                                DonGiaNhap = Convert.ToDecimal(reader["DonGiaNhap"]),
                                MauSac = reader["MauSac"] != DBNull.Value ? reader["MauSac"].ToString() : null,
                                KichCo = reader["KichCo"] != DBNull.Value ? reader["KichCo"].ToString() : null,
                                NgaySanXuat = reader["NgaySanXuat"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySanXuat"]) : DateTime.Now,
                                MoTa = reader["MoTa"] != DBNull.Value ? reader["MoTa"].ToString() : null,
                                MaThuongHieu = reader["MaThuongHieu"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                MaLoaiSP = reader["MaLoaiSP"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                MaChatLieu = reader["MaChatLieu"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString(),
                                MaNCC = reader["MaNCC"].ToString(),
                                TenNCC = reader["TenNCC"].ToString(),
                                HinhAnh = reader["MaHinhAnh"] != DBNull.Value ? new HinhAnhDTO
                                {
                                    MaHinhAnh = reader["MaHinhAnh"].ToString(),
                                    MaSP = reader["MaSP"].ToString(),
                                    DuongDanHinh = reader["DuongDanHinh"].ToString()
                                } : null
                            };
                            danhSachSanPham.Add(sanPham);
                        }
                    }
                }
            }
            return danhSachSanPham;
        }

        public bool ThemSanPham(SanPhamDTO sanPham)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Thêm sản phẩm
                        string querySanPham = @"
                            INSERT INTO SAN_PHAM (MaSP, TenSP, SoLuongTon, DonGiaBan, DonGiaNhap, MauSac, KichCo, NgaySanXuat, MoTa, MaThuongHieu, MaLoaiSP, MaChatLieu, MaNCC)
                            VALUES (@MaSP, @TenSP, @SoLuongTon, @DonGiaBan, @DonGiaNhap, @MauSac, @KichCo, @NgaySanXuat, @MoTa, @MaThuongHieu, @MaLoaiSP, @MaChatLieu, @MaNCC)";
                        using (var cmd = new SqlCommand(querySanPham, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaSP", sanPham.MaSP);
                            cmd.Parameters.AddWithValue("@TenSP", sanPham.TenSP);
                            cmd.Parameters.AddWithValue("@SoLuongTon", sanPham.SoLuongTon);
                            cmd.Parameters.AddWithValue("@DonGiaBan", sanPham.DonGiaBan);
                            cmd.Parameters.AddWithValue("@DonGiaNhap", sanPham.DonGiaNhap);
                            cmd.Parameters.AddWithValue("@MauSac", (object)sanPham.MauSac ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@KichCo", (object)sanPham.KichCo ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@NgaySanXuat", sanPham.NgaySanXuat);
                            cmd.Parameters.AddWithValue("@MoTa", (object)sanPham.MoTa ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaThuongHieu", sanPham.MaThuongHieu);
                            cmd.Parameters.AddWithValue("@MaLoaiSP", sanPham.MaLoaiSP);
                            cmd.Parameters.AddWithValue("@MaChatLieu", sanPham.MaChatLieu);
                            cmd.Parameters.AddWithValue("@MaNCC", sanPham.MaNCC);
                            cmd.ExecuteNonQuery();
                        }

                        // Thêm hình ảnh (nếu có)
                        if (sanPham.HinhAnh != null)
                        {
                            string queryHinhAnh = @"
                                INSERT INTO HINH_ANH (MaHinhAnh, MaSP, DuongDanHinh)
                                VALUES (@MaHinhAnh, @MaSP, @DuongDanHinh)";
                            using (var cmd = new SqlCommand(queryHinhAnh, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@MaHinhAnh", sanPham.HinhAnh.MaHinhAnh);
                                cmd.Parameters.AddWithValue("@MaSP", sanPham.HinhAnh.MaSP);
                                cmd.Parameters.AddWithValue("@DuongDanHinh", sanPham.HinhAnh.DuongDanHinh);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool CapNhatSanPham(SanPhamDTO sanPham)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Cập nhật sản phẩm
                        string querySanPham = @"
                            UPDATE SAN_PHAM
                            SET TenSP = @TenSP, SoLuongTon = @SoLuongTon, DonGiaBan = @DonGiaBan, DonGiaNhap = @DonGiaNhap,
                                MauSac = @MauSac, KichCo = @KichCo, NgaySanXuat = @NgaySanXuat, MoTa = @MoTa,
                                MaThuongHieu = @MaThuongHieu, MaLoaiSP = @MaLoaiSP, MaChatLieu = @MaChatLieu, MaNCC = @MaNCC
                            WHERE MaSP = @MaSP";
                        using (var cmd = new SqlCommand(querySanPham, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaSP", sanPham.MaSP);
                            cmd.Parameters.AddWithValue("@TenSP", sanPham.TenSP);
                            cmd.Parameters.AddWithValue("@SoLuongTon", sanPham.SoLuongTon);
                            cmd.Parameters.AddWithValue("@DonGiaBan", sanPham.DonGiaBan);
                            cmd.Parameters.AddWithValue("@DonGiaNhap", sanPham.DonGiaNhap);
                            cmd.Parameters.AddWithValue("@MauSac", (object)sanPham.MauSac ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@KichCo", (object)sanPham.KichCo ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@NgaySanXuat", sanPham.NgaySanXuat);
                            cmd.Parameters.AddWithValue("@MoTa", (object)sanPham.MoTa ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaThuongHieu", sanPham.MaThuongHieu);
                            cmd.Parameters.AddWithValue("@MaLoaiSP", sanPham.MaLoaiSP);
                            cmd.Parameters.AddWithValue("@MaChatLieu", sanPham.MaChatLieu);
                            cmd.Parameters.AddWithValue("@MaNCC", sanPham.MaNCC);
                            cmd.ExecuteNonQuery();
                        }

                        // Xóa hình ảnh cũ
                        string queryXoaHinhAnh = "DELETE FROM HINH_ANH WHERE MaSP = @MaSP";
                        using (var cmd = new SqlCommand(queryXoaHinhAnh, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaSP", sanPham.MaSP);
                            cmd.ExecuteNonQuery();
                        }

                        // Thêm hình ảnh mới (nếu có)
                        if (sanPham.HinhAnh != null)
                        {
                            string queryHinhAnh = @"
                                INSERT INTO HINH_ANH (MaHinhAnh, MaSP, DuongDanHinh)
                                VALUES (@MaHinhAnh, @MaSP, @DuongDanHinh)";
                            using (var cmd = new SqlCommand(queryHinhAnh, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@MaHinhAnh", sanPham.HinhAnh.MaHinhAnh);
                                cmd.Parameters.AddWithValue("@MaSP", sanPham.HinhAnh.MaSP);
                                cmd.Parameters.AddWithValue("@DuongDanHinh", sanPham.HinhAnh.DuongDanHinh);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool XoaSanPham(string maSP)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM SAN_PHAM WHERE MaSP = @MaSP";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSP", maSP);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<SanPhamDTO> TimKiemSanPham(string tuKhoa)
        {
            var danhSachSanPham = new List<SanPhamDTO>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            SELECT sp.*, th.TenThuongHieu, lsp.TenLoaiSP, cl.TenChatLieu, ncc.TenNCC, ha.MaHinhAnh, ha.DuongDanHinh
            FROM SAN_PHAM sp
            LEFT JOIN THUONG_HIEU th ON sp.MaThuongHieu = th.MaThuongHieu
            LEFT JOIN LOAI_SAN_PHAM lsp ON sp.MaLoaiSP = lsp.MaLoaiSP
            LEFT JOIN CHAT_LIEU cl ON sp.MaChatLieu = cl.MaChatLieu
            LEFT JOIN NHA_CUNG_CAP ncc ON sp.MaNCC = ncc.MaNCC
            LEFT JOIN HINH_ANH ha ON sp.MaSP = ha.MaSP
            WHERE sp.MaSP LIKE '%' + @TuKhoa + '%' 
               OR sp.TenSP LIKE '%' + @TuKhoa + '%' 
               OR th.TenThuongHieu LIKE '%' + @TuKhoa + '%' 
               OR lsp.TenLoaiSP LIKE '%' + @TuKhoa + '%' 
               OR sp.MauSac LIKE '%' + @TuKhoa + '%' 
               OR cl.TenChatLieu LIKE '%' + @TuKhoa + '%'";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TuKhoa", tuKhoa);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sanPham = new SanPhamDTO
                            {
                                MaSP = reader["MaSP"].ToString(),
                                TenSP = reader["TenSP"].ToString(),
                                SoLuongTon = Convert.ToInt32(reader["SoLuongTon"]),
                                DonGiaBan = Convert.ToDecimal(reader["DonGiaBan"]),
                                DonGiaNhap = Convert.ToDecimal(reader["DonGiaNhap"]),
                                MauSac = reader["MauSac"] != DBNull.Value ? reader["MauSac"].ToString() : null,
                                KichCo = reader["KichCo"] != DBNull.Value ? reader["KichCo"].ToString() : null,
                                NgaySanXuat = reader["NgaySanXuat"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySanXuat"]) : DateTime.Now,
                                MoTa = reader["MoTa"] != DBNull.Value ? reader["MoTa"].ToString() : null,
                                MaThuongHieu = reader["MaThuongHieu"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString(),
                                MaLoaiSP = reader["MaLoaiSP"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString(),
                                MaChatLieu = reader["MaChatLieu"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString(),
                                MaNCC = reader["MaNCC"].ToString(),
                                TenNCC = reader["TenNCC"].ToString(),
                                HinhAnh = reader["MaHinhAnh"] != DBNull.Value ? new HinhAnhDTO
                                {
                                    MaHinhAnh = reader["MaHinhAnh"].ToString(),
                                    MaSP = reader["MaSP"].ToString(),
                                    DuongDanHinh = reader["DuongDanHinh"].ToString()
                                } : null
                            };
                            danhSachSanPham.Add(sanPham);
                        }
                    }
                }
            }
            return danhSachSanPham;
        }

        public int LayTongSoSanPham()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM SAN_PHAM";
                using (var cmd = new SqlCommand(query, conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<ChatLieuDTO> LayTatCaChatLieu()
        {
            var danhSachChatLieu = new List<ChatLieuDTO>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT MaChatLieu, TenChatLieu FROM CHAT_LIEU";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var chatLieu = new ChatLieuDTO
                            {
                                MaChatLieu = reader["MaChatLieu"].ToString(),
                                TenChatLieu = reader["TenChatLieu"].ToString()
                            };
                            danhSachChatLieu.Add(chatLieu);
                        }
                    }
                }
            }
            return danhSachChatLieu;
        }

        public List<LoaiSanPhamDTO> LayTatCaLoaiSanPham()
        {
            var danhSachLoaiSP = new List<LoaiSanPhamDTO>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT MaLoaiSP, TenLoaiSP FROM LOAI_SAN_PHAM";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var loaiSP = new LoaiSanPhamDTO
                            {
                                MaLoaiSP = reader["MaLoaiSP"].ToString(),
                                TenLoaiSP = reader["TenLoaiSP"].ToString()
                            };
                            danhSachLoaiSP.Add(loaiSP);
                        }
                    }
                }
            }
            return danhSachLoaiSP;
        }

        public List<NhaCungCapDTO> LayTatCaNhaCungCap()
        {
            var danhSachNCC = new List<NhaCungCapDTO>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ncc = new NhaCungCapDTO
                            {
                                MaNCC = reader["MaNCC"].ToString(),
                                TenNCC = reader["TenNCC"].ToString()
                            };
                            danhSachNCC.Add(ncc);
                        }
                    }
                }
            }
            return danhSachNCC;
        }

        public List<ThuongHieuDTO> LayTatCaThuongHieu()
        {
            var danhSachThuongHieu = new List<ThuongHieuDTO>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT MaThuongHieu, TenThuongHieu FROM THUONG_HIEU";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var thuongHieu = new ThuongHieuDTO
                            {
                                MaThuongHieu = reader["MaThuongHieu"].ToString(),
                                TenThuongHieu = reader["TenThuongHieu"].ToString()
                            };
                            danhSachThuongHieu.Add(thuongHieu);
                        }
                    }
                }
            }
            return danhSachThuongHieu;
        }

        public string TaoMaSanPham()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 MaSP FROM SAN_PHAM ORDER BY MaSP DESC";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        string maSPMoi = "SP000";
                        if (reader.Read())
                        {
                            maSPMoi = reader["MaSP"].ToString();
                        }
                        int so = int.Parse(maSPMoi.Replace("SP", "")) + 1;
                        return "SP" + so.ToString("D3");
                    }
                }
            }
        }

        public string TaoMaHinhAnh()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 MaHinhAnh FROM HINH_ANH ORDER BY MaHinhAnh DESC";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        string maHinhAnhMoi = "HA000";
                        if (reader.Read())
                        {
                            maHinhAnhMoi = reader["MaHinhAnh"].ToString();
                        }
                        int so = int.Parse(maHinhAnhMoi.Replace("HA", "")) + 1;
                        return "HA" + so.ToString("D3");
                    }
                }
            }
        }
    }
}