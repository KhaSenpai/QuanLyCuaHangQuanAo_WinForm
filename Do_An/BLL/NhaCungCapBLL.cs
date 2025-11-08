using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Nhà Cung Cấp.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class NhaCungCapBLL
    {
        private readonly NhaCungCapDAL _nhaCungCapDAL;

        // Hàm khởi tạo, tạo một đối tượng NhaCungCapDAL để tương tác với cơ sở dữ liệu.
        public NhaCungCapBLL()
        {
            _nhaCungCapDAL = new NhaCungCapDAL();
        }

        // Lấy toàn bộ danh sách nhà cung cấp.
        // Trả về danh sách các đối tượng NhaCungCapDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<NhaCungCapDTO> LayTatCaNhaCungCap()
        {
            return _nhaCungCapDAL.LayTatCaNhaCungCap();
        }

        // Tìm kiếm nhà cung cấp dựa trên từ khóa.
        // Trả về danh sách các nhà cung cấp phù hợp.
        // Ném lỗi nếu từ khóa không hợp lệ hoặc có vấn đề trong quá trình tìm kiếm.
        public List<NhaCungCapDTO> TimKiemNhaCungCap(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                throw new ArgumentException("Từ khóa tìm kiếm không được để trống.");
            }
            return _nhaCungCapDAL.TimKiemNhaCungCap(tuKhoa.Trim());
        }

        // Tạo mã nhà cung cấp mới theo định dạng "NCCxxx" (xxx là số tăng dần).
        // Trả về mã nhà cung cấp mới.
        public string TaoMaNhaCungCap()
        {
            try
            {
                var danhSach = LayTatCaNhaCungCap();
                int maxId = 0;
                if (danhSach.Any())
                {
                    maxId = danhSach
                        .Select(ncc => int.Parse(ncc.MaNCC.Replace("NCC", "")))
                        .Max();
                }
                return $"NCC{(maxId + 1):D3}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo mã nhà cung cấp: {ex.Message}", ex);
            }
        }

        // Thêm một nhà cung cấp mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Ném lỗi nếu thêm thất bại.
        public void ThemNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            if (nhaCungCap == null)
            {
                throw new ArgumentNullException(nameof(nhaCungCap), "Đối tượng nhà cung cấp không được null.");
            }
            if (string.IsNullOrWhiteSpace(nhaCungCap.MaNCC) || string.IsNullOrWhiteSpace(nhaCungCap.TenNCC) ||
                string.IsNullOrWhiteSpace(nhaCungCap.DiaChi) || string.IsNullOrWhiteSpace(nhaCungCap.DienThoai))
            {
                throw new ArgumentException("Mã, tên, địa chỉ và số điện thoại nhà cung cấp không được để trống.");
            }
            if (!Regex.IsMatch(nhaCungCap.DienThoai, @"^0\d{9}$"))
            {
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và 10 chữ số.");
            }
            if (!string.IsNullOrEmpty(nhaCungCap.Email) && !Regex.IsMatch(nhaCungCap.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Email không đúng định dạng.");
            }
            _nhaCungCapDAL.ThemNhaCungCap(nhaCungCap);
        }

        // Cập nhật thông tin một nhà cung cấp.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            if (nhaCungCap == null)
            {
                throw new ArgumentNullException(nameof(nhaCungCap), "Đối tượng nhà cung cấp không được null.");
            }
            if (string.IsNullOrWhiteSpace(nhaCungCap.MaNCC) || string.IsNullOrWhiteSpace(nhaCungCap.TenNCC) ||
                string.IsNullOrWhiteSpace(nhaCungCap.DiaChi) || string.IsNullOrWhiteSpace(nhaCungCap.DienThoai))
            {
                throw new ArgumentException("Mã, tên, địa chỉ và số điện thoại nhà cung cấp không được để trống.");
            }
            if (!Regex.IsMatch(nhaCungCap.DienThoai, @"^0\d{9}$"))
            {
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và 10 chữ số.");
            }
            if (!string.IsNullOrEmpty(nhaCungCap.Email) && !Regex.IsMatch(nhaCungCap.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Email không đúng định dạng.");
            }
            _nhaCungCapDAL.CapNhatNhaCungCap(nhaCungCap);
        }

        // Xóa một nhà cung cấp dựa trên mã nhà cung cấp.
        // Ném lỗi nếu xóa thất bại hoặc nhà cung cấp đang được sử dụng.
        public void XoaNhaCungCap(string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maNCC))
            {
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");
            }
            _nhaCungCapDAL.XoaNhaCungCap(maNCC);
        }
    }
}