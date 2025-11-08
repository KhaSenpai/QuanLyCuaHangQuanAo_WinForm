using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Thương Hiệu.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class ThuongHieuBLL
    {
        private readonly ThuongHieuDAL _thuongHieuDAL;

        // Hàm khởi tạo, tạo một đối tượng ThuongHieuDAL để tương tác với cơ sở dữ liệu.
        public ThuongHieuBLL()
        {
            _thuongHieuDAL = new ThuongHieuDAL();
        }

        // Lấy toàn bộ danh sách thương hiệu.
        // Trả về danh sách các đối tượng ThuongHieuDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<ThuongHieuDTO> LayTatCaThuongHieu()
        {
            return _thuongHieuDAL.LayTatCaThuongHieu();
        }

        // Tìm kiếm thương hiệu dựa trên từ khóa.
        // Trả về danh sách các thương hiệu phù hợp.
        // Ném lỗi nếu có vấn đề trong quá trình tìm kiếm.
        public List<ThuongHieuDTO> TimKiemThuongHieu(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                throw new ArgumentException("Vui lòng nhập từ khóa tìm kiếm.");
            }
            return _thuongHieuDAL.TimKiemThuongHieu(tuKhoa.Trim());
        }

        // Tạo mã thương hiệu mới theo định dạng "THxxx" (xxx là số tăng dần).
        // Trả về mã thương hiệu mới.
        public string TaoMaThuongHieu()
        {
            var danhSach = LayTatCaThuongHieu();
            int max = 0;
            foreach (var thuongHieu in danhSach)
            {
                if (thuongHieu.MaThuongHieu.StartsWith("TH") && int.TryParse(thuongHieu.MaThuongHieu.Substring(2), out int num))
                {
                    max = Math.Max(max, num);
                }
            }
            return $"TH{(max + 1):D3}";
        }

        // Thêm một thương hiệu mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Ném lỗi nếu thêm thất bại.
        public void ThemThuongHieu(ThuongHieuDTO thuongHieu)
        {
            if (thuongHieu == null)
            {
                throw new ArgumentNullException(nameof(thuongHieu), "Thương hiệu không trồng.");
            }
            if (string.IsNullOrWhiteSpace(thuongHieu.MaThuongHieu) || string.IsNullOrWhiteSpace(thuongHieu.TenThuongHieu))
            {
                throw new ArgumentException("Tên thương hiệu không được để trống.");
            }          
            _thuongHieuDAL.ThemThuongHieu(thuongHieu);
        }

        // Cập nhật thông tin một thương hiệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatThuongHieu(ThuongHieuDTO thuongHieu)
        {
            if (thuongHieu == null)
            {
                throw new ArgumentNullException(nameof(thuongHieu), "Đhương hiệu không trồng.");
            }
            if (string.IsNullOrWhiteSpace(thuongHieu.MaThuongHieu) || string.IsNullOrWhiteSpace(thuongHieu.TenThuongHieu))
            {
                throw new ArgumentException("Vui lòng chọn thương hiệu để sửa.");
            }
            _thuongHieuDAL.CapNhatThuongHieu(thuongHieu);
        }

        // Xóa một thương hiệu dựa trên mã thương hiệu.
        // Kiểm tra ràng buộc khóa ngoại trước khi xóa.
        // Ném lỗi nếu xóa thất bại.
        public void XoaThuongHieu(string maThuongHieu)
        {
            if (string.IsNullOrWhiteSpace(maThuongHieu))
            {
                throw new ArgumentException("Vui lòng chọn thương hiệu để sửa.");
            }
            if (_thuongHieuDAL.KiemTraThuongHieuDuocSuDung(maThuongHieu))
            {
                throw new InvalidOperationException("Không thể xóa thương hiệu vì đang được sử dụng trong sản phẩm.");
            }
            _thuongHieuDAL.XoaThuongHieu(maThuongHieu);
        }
    }
}