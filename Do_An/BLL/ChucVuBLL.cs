using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Chức Vụ.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class ChucVuBLL
    {
        private readonly ChucVuDAL _chucVuDAL;

        // Hàm khởi tạo, tạo một đối tượng ChucVuDAL để tương tác với cơ sở dữ liệu.
        public ChucVuBLL()
        {
            _chucVuDAL = new ChucVuDAL();
        }

        // Lấy toàn bộ danh sách chức vụ.
        // Trả về danh sách các đối tượng ChucVuDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<ChucVuDTO> LayTatCaChucVu()
        {
            return _chucVuDAL.LayTatCaChucVu();
        }

        // Tìm kiếm chức vụ dựa trên từ khóa.
        // Trả về danh sách các chức vụ phù hợp.
        // Ném lỗi nếu có vấn đề trong quá trình tìm kiếm.
        public List<ChucVuDTO> TimKiemChucVu(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return LayTatCaChucVu();
            }
            return _chucVuDAL.TimKiemChucVu(tuKhoa.Trim());
        }

        // Tạo mã chức vụ mới theo định dạng "CVxxx" (xxx là số tăng dần).
        // Trả về mã chức vụ mới.
        public string TaoMaChucVu()
        {
            try
            {
                var danhSach = LayTatCaChucVu();
                int maxId = 0;
                if (danhSach.Any())
                {
                    maxId = danhSach
                        .Select(cv => int.Parse(cv.MaChucVu.Replace("CV", "")))
                        .Max();
                }
                return $"CV{(maxId + 1):D3}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo mã chức vụ: {ex.Message}", ex);
            }
        }

        // Thêm một chức vụ mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình thêm.
        public bool ThemChucVu(ChucVuDTO chucVu)
        {
            if (chucVu == null)
            {
                throw new ArgumentNullException(nameof(chucVu), "Đối tượng chức vụ không được null.");
            }
            if (string.IsNullOrWhiteSpace(chucVu.MaChucVu) || string.IsNullOrWhiteSpace(chucVu.TenChucVu))
            {
                throw new ArgumentException("Mã và tên chức vụ không được để trống.");
            }
            if (chucVu.MaChucVu.Length > 20)
            {
                throw new ArgumentException("Mã chức vụ không được vượt quá 20 ký tự.");
            }
            if (chucVu.TenChucVu.Length > 50)
            {
                throw new ArgumentException("Tên chức vụ không được vượt quá 50 ký tự.");
            }
            return _chucVuDAL.ThemChucVu(chucVu);
        }

        // Cập nhật thông tin một chức vụ.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình cập nhật.
        public bool CapNhatChucVu(ChucVuDTO chucVu)
        {
            if (chucVu == null)
            {
                throw new ArgumentNullException(nameof(chucVu), "Đối tượng chức vụ không được null.");
            }
            if (string.IsNullOrWhiteSpace(chucVu.MaChucVu) || string.IsNullOrWhiteSpace(chucVu.TenChucVu))
            {
                throw new ArgumentException("Mã và tên chức vụ không được để trống.");
            }
            if (chucVu.MaChucVu.Length > 20)
            {
                throw new ArgumentException("Mã chức vụ không được vượt quá 20 ký tự.");
            }
            if (chucVu.TenChucVu.Length > 50)
            {
                throw new ArgumentException("Tên chức vụ không được vượt quá 50 ký tự.");
            }
            return _chucVuDAL.CapNhatChucVu(chucVu);
        }

        // Xóa một chức vụ dựa trên mã chức vụ.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        // Ném lỗi nếu xóa thất bại hoặc chức vụ đang được sử dụng.
        public bool XoaChucVu(string maChucVu)
        {
            if (string.IsNullOrWhiteSpace(maChucVu))
            {
                throw new ArgumentException("Mã chức vụ không được để trống.");
            }
            return _chucVuDAL.XoaChucVu(maChucVu);
        }
    }
}