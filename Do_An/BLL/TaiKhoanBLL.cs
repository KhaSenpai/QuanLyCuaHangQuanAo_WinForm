using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL _taiKhoanDAL;

        public TaiKhoanBLL()
        {
            _taiKhoanDAL = new TaiKhoanDAL();
        }

        // Lấy toàn bộ danh sách tài khoản
        public List<TaiKhoanDTO> GetAllTaiKhoan()
        {
            try
            {
                var taiKhoans = _taiKhoanDAL.GetAllTaiKhoan();
                if (taiKhoans == null)
                    throw new InvalidOperationException("Danh sách tài khoản trả về là null.");
                return taiKhoans;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tài khoản", ex);
            }
        }

        // Tìm kiếm tài khoản theo từ khóa
        public List<TaiKhoanDTO> SearchTaiKhoan(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                    throw new ArgumentException("Từ khóa tìm kiếm không được để trống.");

                return _taiKhoanDAL.SearchTaiKhoan(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm tài khoản", ex);
            }
        }

        // Xác thực đăng nhập tài khoản
        public TaiKhoanDTO Login(string tenDangNhap, string matKhau)
        {
            try
            {
                if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
                    throw new ArgumentException("Tên đăng nhập và mật khẩu không được để trống.");

                var taiKhoan = _taiKhoanDAL.Login(tenDangNhap, matKhau);
                if (taiKhoan == null)
                    throw new InvalidOperationException("Tên đăng nhập hoặc mật khẩu không đúng.");
                return taiKhoan;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đăng nhập");
            }
        }

        // Thêm tài khoản mới
        public void AddTaiKhoan(TaiKhoanDTO taiKhoan)
        {
            try
            {
                ValidateTaiKhoan(taiKhoan);

                // Kiểm tra trùng lặp TenDangNhap
                var existingAccounts = GetAllTaiKhoan();
                if (existingAccounts.Exists(tk => tk.TenDangNhap == taiKhoan.TenDangNhap))
                    throw new InvalidOperationException($"Tên đăng nhập '{taiKhoan.TenDangNhap}' đã tồn tại.");

                bool success = _taiKhoanDAL.AddTaiKhoan(taiKhoan);
                if (!success)
                    throw new InvalidOperationException("Thêm tài khoản thất bại.");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tài khoản", ex);
            }
        }

        // Cập nhật thông tin tài khoản
        public void UpdateTaiKhoan(TaiKhoanDTO taiKhoan)
        {
            try
            {
                ValidateTaiKhoan(taiKhoan);

                bool success = _taiKhoanDAL.UpdateTaiKhoan(taiKhoan);
                if (!success)
                    throw new InvalidOperationException($"Không tìm thấy tài khoản với Tên Đăng Nhập '{taiKhoan.TenDangNhap}' để cập nhật.");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tài khoản", ex);
            }
        }

        // Xóa tài khoản
        public void DeleteTaiKhoan(string tenDangNhap)
        {
            try
            {
                if (string.IsNullOrEmpty(tenDangNhap))
                    throw new ArgumentException("Tên đăng nhập không được để trống.");

                bool success = _taiKhoanDAL.DeleteTaiKhoan(tenDangNhap);
                if (!success)
                    throw new InvalidOperationException($"Không tìm thấy tài khoản với Tên Đăng Nhập '{tenDangNhap}' để xóa.");
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tài khoản", ex);
            }
        }

        // Kiểm tra dữ liệu đầu vào của TaiKhoanDTO
        private void ValidateTaiKhoan(TaiKhoanDTO taiKhoan)
        {
            if (taiKhoan == null)
                throw new ArgumentNullException(nameof(taiKhoan), "Vui lòng nhập thông tin tài khoản..");

            if (string.IsNullOrEmpty(taiKhoan.TenDangNhap))
                throw new ArgumentException("Vui lòng nhập tên đăng nhập .");

            if (string.IsNullOrEmpty(taiKhoan.MatKhau))
                throw new ArgumentException("Vui lòng nhập Mật khẩu.");

            if (string.IsNullOrEmpty(taiKhoan.MaNhanVien))
                throw new ArgumentException("Vui lòng nhập Mã nhân viên.");
        }
    }
}