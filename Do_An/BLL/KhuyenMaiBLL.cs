using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Khuyến Mãi.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class KhuyenMaiBLL
    {
        private readonly KhuyenMaiDAL _khuyenMaiDAL;

        // Hàm khởi tạo, tạo một đối tượng KhuyenMaiDAL để tương tác với cơ sở dữ liệu.
        public KhuyenMaiBLL()
        {
            _khuyenMaiDAL = new KhuyenMaiDAL();
        }

        // Lấy toàn bộ danh sách khuyến mãi.
        // Trả về danh sách các đối tượng KhuyenMaiDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<KhuyenMaiDTO> LayTatCaKhuyenMai()
        {
            return _khuyenMaiDAL.LayTatCaKhuyenMai();
        }

        // Tìm kiếm khuyến mãi dựa trên từ khóa.
        // Trả về danh sách các khuyến mãi phù hợp.
        // Ném lỗi nếu từ khóa không hợp lệ hoặc có vấn đề trong quá trình tìm kiếm.
        public List<KhuyenMaiDTO> TimKiemKhuyenMai(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return LayTatCaKhuyenMai();
            }
            return _khuyenMaiDAL.TimKiemKhuyenMai(tuKhoa.Trim());
        }

        // Tạo mã khuyến mãi mới theo định dạng "KMxxx" (xxx là số tăng dần).
        // Trả về mã khuyến mãi mới.
        public string TaoMaKhuyenMai()
        {
            try
            {
                var danhSach = LayTatCaKhuyenMai();
                int maxId = 0;
                if (danhSach.Any())
                {
                    maxId = danhSach
                        .Select(km => int.Parse(km.MaKhuyenMai.Replace("KM", "")))
                        .Max();
                }
                return $"KM{(maxId + 1):D3}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo mã khuyến mãi: {ex.Message}", ex);
            }
        }

        // Thêm một khuyến mãi mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình thêm.
        public bool ThemKhuyenMai(KhuyenMaiDTO khuyenMai)
        {
            if (khuyenMai == null)
            {
                throw new ArgumentNullException(nameof(khuyenMai), "Đối tượng khuyến mãi không được null.");
            }
            if (string.IsNullOrWhiteSpace(khuyenMai.MaKhuyenMai) || string.IsNullOrWhiteSpace(khuyenMai.TenKhuyenMai))
            {
                throw new ArgumentException("Mã và tên khuyến mãi không được để trống.");
            }

            if (khuyenMai.PhanTramKhuyenMai < 0 || khuyenMai.PhanTramKhuyenMai > 100)
            {
                throw new ArgumentException("Phần trăm khuyến mãi phải nằm trong khoảng 0 đến 100.");
            }
            if (khuyenMai.NgayBatDau > khuyenMai.NgayKetThuc)
            {
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
            }
            return _khuyenMaiDAL.ThemKhuyenMai(khuyenMai);
        }

        // Cập nhật thông tin một khuyến mãi.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình cập nhật.
        public bool CapNhatKhuyenMai(KhuyenMaiDTO khuyenMai)
        {
            if (khuyenMai == null)
            {
                throw new ArgumentNullException(nameof(khuyenMai), "Đối tượng khuyến mãi không được null.");
            }
            if (string.IsNullOrWhiteSpace(khuyenMai.MaKhuyenMai) || string.IsNullOrWhiteSpace(khuyenMai.TenKhuyenMai))
            {
                throw new ArgumentException("Mã và tên khuyến mãi không được để trống.");
            }
            if (khuyenMai.MaKhuyenMai.Length > 20)
            {
                throw new ArgumentException("Mã khuyến mãi không được vượt quá 20 ký tự.");
            }
            if (khuyenMai.PhanTramKhuyenMai < 0 || khuyenMai.PhanTramKhuyenMai > 100)
            {
                throw new ArgumentException("Phần trăm khuyến mãi phải nằm trong khoảng 0 đến 100.");
            }
            if (khuyenMai.NgayBatDau > khuyenMai.NgayKetThuc)
            {
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
            }
            return _khuyenMaiDAL.CapNhatKhuyenMai(khuyenMai);
        }

        // Xóa một khuyến mãi dựa trên mã khuyến mãi.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        // Ném lỗi nếu xóa thất bại hoặc khuyến mãi đang được sử dụng.
        public bool XoaKhuyenMai(string maKhuyenMai)
        {
            if (string.IsNullOrWhiteSpace(maKhuyenMai))
            {
                throw new ArgumentException("Mã khuyến mãi không được để trống.");
            }
            return _khuyenMaiDAL.XoaKhuyenMai(maKhuyenMai);
        }
    }
}