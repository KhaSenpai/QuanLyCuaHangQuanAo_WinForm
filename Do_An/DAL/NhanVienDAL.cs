    using DTO;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;

    namespace DAL
    {
        // Tầng truy cập dữ liệu (DAL) cho thực thể Nhân Viên.
        // Chịu trách nhiệm cho tất cả các thao tác với cơ sở dữ liệu liên quan đến nhân viên.
        public class NhanVienDAL
        {
            private readonly string _connectionString;

            public NhanVienDAL()
            {
                _connectionString = DatabaseConnection.ConnectionString;
            }

            // Lấy toàn bộ danh sách nhân viên từ cơ sở dữ liệu.
            // Kết hợp với bảng CHUC_VU để lấy tên chức vụ.
            // Trả về danh sách các đối tượng NhanVienDTO.
            // Ném lỗi nếu truy vấn thất bại.
            public List<NhanVienDTO> LayTatCaNhanVien()
            {
                var danhSachNhanVien = new List<NhanVienDTO>();

                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        const string query = "SELECT nv.MaNhanVien, nv.HoTen, nv.NgaySinh, nv.GioiTinh, nv.DiaChi, nv.SoDienThoai, cv.MaChucVu, cv.TenChucVu " +
                                            "FROM NHAN_VIEN nv JOIN CHUC_VU cv ON nv.MaChucVu = cv.MaChucVu";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    danhSachNhanVien.Add(new NhanVienDTO
                                    {
                                        MaNhanVien = reader["MaNhanVien"].ToString(),
                                        HoTen = reader["HoTen"].ToString(),
                                        NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                                        GioiTinh = reader["GioiTinh"].ToString(),
                                        DiaChi = reader["DiaChi"].ToString(),
                                        SoDienThoai = reader["SoDienThoai"].ToString(),
                                        MaChucVu = reader["MaChucVu"].ToString(),
                                        TenChucVu = reader["TenChucVu"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách nhân viên: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
                }

                return danhSachNhanVien;
            }

            // Tạo mã nhân viên mới theo định dạng "NVxxx" (xxx là số tăng dần).
            // Trả về mã nhân viên mới.
            public string TaoMaNhanVien()
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        const string query = "SELECT TOP 1 MaNhanVien FROM NHAN_VIEN ORDER BY MaNhanVien DESC";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                string lastMaNV = "NV000";
                                if (reader.Read())
                                {
                                    lastMaNV = reader["MaNhanVien"].ToString();
                                }
                                int number = int.Parse(lastMaNV.Replace("NV", "")) + 1;
                                return $"NV{number:D3}";
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Lỗi cơ sở dữ liệu khi tạo mã nhân viên: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi tạo mã nhân viên: {ex.Message}", ex);
                }
            }
        public bool KiemTraMaNhanVienTonTai(string maNhanVien)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT COUNT(*) FROM NHAN_VIEN WHERE MaNhanVien = @MaNhanVien";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra mã nhân viên: {ex.Message}", ex);
            }
        }
        // Lấy danh sách tên chức vụ từ bảng CHUC_VU.
        // Trả về danh sách các tên chức vụ.
        // Ném lỗi nếu truy vấn thất bại.
        public List<string> LayTatCaChucVu()
            {
                var danhSachChucVu = new List<string>();

                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        const string query = "SELECT TenChucVu FROM CHUC_VU";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    danhSachChucVu.Add(reader["TenChucVu"].ToString());
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Lỗi cơ sở dữ liệu khi lấy danh sách tên chức vụ: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi lấy danh sách tên chức vụ: {ex.Message}", ex);
                }

                return danhSachChucVu;
            }

            // Lấy mã chức vụ dựa trên tên chức vụ.
            // Trả về mã chức vụ hoặc chuỗi rỗng nếu không tìm thấy.
            // Ném lỗi nếu truy vấn thất bại.
            public string LayMaChucVuTheoTen(string tenChucVu)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        const string query = "SELECT MaChucVu FROM CHUC_VU WHERE TenChucVu = @TenChucVu";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@TenChucVu", tenChucVu ?? string.Empty);
                            var result = cmd.ExecuteScalar();
                            return result?.ToString() ?? string.Empty;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Lỗi cơ sở dữ liệu khi lấy mã chức vụ theo tên: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi lấy mã chức vụ theo tên: {ex.Message}", ex);
                }
            }

            // Thêm một nhân viên mới vào cơ sở dữ liệu.
            // Trả về true nếu thêm thành công, false nếu thất bại.
            // Ném lỗi nếu có vấn đề trong quá trình thêm.
            public bool ThemNhanVien(NhanVienDTO nhanVien)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        const string query = "INSERT INTO NHAN_VIEN (MaNhanVien, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, MaChucVu) " +
                                            "VALUES (@MaNhanVien, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @MaChucVu)";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNhanVien", nhanVien.MaNhanVien);
                            cmd.Parameters.AddWithValue("@HoTen", nhanVien.HoTen);
                            cmd.Parameters.AddWithValue("@NgaySinh", nhanVien.NgaySinh);
                            cmd.Parameters.AddWithValue("@GioiTinh", nhanVien.GioiTinh);
                            cmd.Parameters.AddWithValue("@DiaChi", nhanVien.DiaChi);
                            cmd.Parameters.AddWithValue("@SoDienThoai", nhanVien.SoDienThoai);
                            cmd.Parameters.AddWithValue("@MaChucVu", nhanVien.MaChucVu);
                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
                catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Mã nhân viên đã tồn tại.", ex);
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    throw new Exception("Mã chức vụ không tồn tại.", ex);
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Lỗi cơ sở dữ liệu khi thêm nhân viên: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi thêm nhân viên: {ex.Message}", ex);
                }
            }

            // Cập nhật thông tin một nhân viên trong cơ sở dữ liệu.
            // Trả về true nếu cập nhật thành công, false nếu thất bại.
            // Ném lỗi nếu có vấn đề trong quá trình cập nhật.
            public bool CapNhatNhanVien(NhanVienDTO nhanVien)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        const string query = "UPDATE NHAN_VIEN SET HoTen = @HoTen, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, " +
                                            "DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, MaChucVu = @MaChucVu " +
                                            "WHERE MaNhanVien = @MaNhanVien";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNhanVien", nhanVien.MaNhanVien);
                            cmd.Parameters.AddWithValue("@HoTen", nhanVien.HoTen);
                            cmd.Parameters.AddWithValue("@NgaySinh", nhanVien.NgaySinh);
                            cmd.Parameters.AddWithValue("@GioiTinh", nhanVien.GioiTinh);
                            cmd.Parameters.AddWithValue("@DiaChi", nhanVien.DiaChi);
                            cmd.Parameters.AddWithValue("@SoDienThoai", nhanVien.SoDienThoai);
                            cmd.Parameters.AddWithValue("@MaChucVu", nhanVien.MaChucVu);
                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    throw new Exception("Mã chức vụ không tồn tại.", ex);
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Lỗi cơ sở dữ liệu khi cập nhật nhân viên: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi cập nhật nhân viên: {ex.Message}", ex);
                }
            }

            // Xóa một nhân viên khỏi cơ sở dữ liệu dựa trên mã nhân viên.
            // Kiểm tra ràng buộc khóa ngoại trước khi xóa.
            // Trả về true nếu xóa thành công, false nếu thất bại.
            // Ném lỗi nếu có vấn đề trong quá trình xóa.
            public bool XoaNhanVien(string maNhanVien)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        // Kiểm tra xem nhân viên có được sử dụng trong bảng khác không
                        const string checkQuery = "SELECT COUNT(*) FROM HOA_DON_BAN WHERE MaNhanVien = @MaNhanVien";
                        using (var checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                            int count = (int)checkCmd.ExecuteScalar();
                            if (count > 0)
                            {
                                throw new InvalidOperationException("Không thể xóa nhân viên vì đã lập hóa đơn.");
                            }
                        }

                        const string query = "DELETE FROM NHAN_VIEN WHERE MaNhanVien = @MaNhanVien";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Lỗi cơ sở dữ liệu khi xóa nhân viên: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi xóa nhân viên: {ex.Message}", ex);
                }
            }

        // Tìm kiếm nhân viên theo từ khóa dựa trên mã hoặc tên.
        // Kết hợp với bảng CHUC_VU để lấy tên chức vụ.
        // Trả về danh sách các nhân viên phù hợp.
        // Ném lỗi nếu truy vấn thất bại.
        public List<NhanVienDTO> TimKiemNhanVien(string tuKhoa)
        {
            var danhSachNhanVien = new List<NhanVienDTO>();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    const string query = "SELECT nv.MaNhanVien, nv.HoTen, nv.NgaySinh, nv.GioiTinh, nv.DiaChi, nv.SoDienThoai, cv.MaChucVu, cv.TenChucVu " +
                                        "FROM NHAN_VIEN nv JOIN CHUC_VU cv ON nv.MaChucVu = cv.MaChucVu " +
                                        "WHERE nv.MaNhanVien LIKE @TuKhoa OR cv.TenChucVu LIKE @TuKhoa OR nv.HoTen LIKE @TuKhoa";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TuKhoa", $"%{tuKhoa ?? string.Empty}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachNhanVien.Add(new NhanVienDTO
                                {
                                    MaNhanVien = reader["MaNhanVien"].ToString(),
                                    HoTen = reader["HoTen"].ToString(),
                                    NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                                    GioiTinh = reader["GioiTinh"].ToString(),
                                    DiaChi = reader["DiaChi"].ToString(),
                                    SoDienThoai = reader["SoDienThoai"].ToString(),
                                    MaChucVu = reader["MaChucVu"].ToString(),
                                    TenChucVu = reader["TenChucVu"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Lỗi cơ sở dữ liệu khi tìm kiếm nhân viên: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm nhân viên: {ex.Message}", ex);
            }
            return danhSachNhanVien;
        }
    }
    }