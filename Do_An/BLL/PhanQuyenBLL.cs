using DTO;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PhanQuyenBLL
    {
        private readonly PhanQuyenDAL _phanQuyenDAL;
        private readonly List<string> _validRoles = new List<string> { "Admin", "User", "Manager" };

        public PhanQuyenBLL()
        {
            _phanQuyenDAL = new PhanQuyenDAL();
        }

        // Lấy danh sách tất cả tài khoản
        public List<TaiKhoanDTO> GetAllTaiKhoan()
        {
            try
            {
                return _phanQuyenDAL.GetAllTaiKhoan();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tài khoản", ex);
            }
        }

        // Lấy thông tin tài khoản theo tên đăng nhập
        public TaiKhoanDTO GetTaiKhoanByTenDangNhap(string tenDangNhap)
        {
            try
            {
                if (string.IsNullOrEmpty(tenDangNhap))
                {
                    throw new Exception("Tên đăng nhập không được để trống.");
                }
                return _phanQuyenDAL.GetTaiKhoanByTenDangNhap(tenDangNhap);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin tài khoản", ex);
            }
        }

        // Cập nhật quyền truy cập
        public bool UpdateQuyenTruyCap(string tenDangNhap, string quyenTruyCap)
        {
            try
            {
                // Kiểm tra quyền hợp lệ
                if (!_validRoles.Contains(quyenTruyCap))
                {
                    throw new Exception("Quyền truy cập không hợp lệ. Quyền phải là Admin, User hoặc Manager.");
                }

                // Kiểm tra tài khoản tồn tại
                var taiKhoan = _phanQuyenDAL.GetTaiKhoanByTenDangNhap(tenDangNhap);
                if (taiKhoan == null)
                {
                    throw new Exception($"Tài khoản với tên đăng nhập '{tenDangNhap}' không tồn tại.");
                }

                return _phanQuyenDAL.UpdateQuyenTruyCap(tenDangNhap, quyenTruyCap);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật quyền truy cập", ex);
            }
        }

        // Lấy danh sách quyền hợp lệ
        public List<string> GetValidRoles()
        {
            return _validRoles;
        }
    }
}