using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Loại Sản Phẩm.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class LoaiSanPhamBLL
    {
        private readonly LoaiSanPhamDAL _loaiSanPhamDAL;

        // Hàm khởi tạo, tạo một đối tượng LoaiSanPhamDAL để tương tác với cơ sở dữ liệu.
        public LoaiSanPhamBLL()
        {
            _loaiSanPhamDAL = new LoaiSanPhamDAL();
        }

        // Lấy toàn bộ danh sách loại sản phẩm.
        // Trả về danh sách các đối tượng LoaiSanPhamDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<LoaiSanPhamDTO> LayTatCaLoaiSanPham()
        {
            return _loaiSanPhamDAL.LayTatCaLoaiSanPham();
        }

        // Tìm kiếm loại sản phẩm dựa trên từ khóa.
        // Trả về danh sách các loại sản phẩm phù hợp.
        // Ném lỗi nếu từ khóa không hợp lệ hoặc có vấn đề trong quá trình tìm kiếm.
        public List<LoaiSanPhamDTO> TimKiemLoaiSanPham(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                throw new ArgumentException("Vui lòng nhập từ khóa tìm kiếm");
            }
            return _loaiSanPhamDAL.TimKiemLoaiSanPham(tuKhoa.Trim());
        }

        // Tạo mã loại sản phẩm mới theo định dạng "LSPxxx" (xxx là số tăng dần).
        // Trả về mã loại sản phẩm mới.
        public string TaoMaLoaiSanPham()
        {
            try
            {
                var danhSach = LayTatCaLoaiSanPham();
                int maxId = 0;
                if (danhSach.Any())
                {
                    maxId = danhSach
                        .Select(lsp => int.Parse(lsp.MaLoaiSP.Replace("LSP", "")))
                        .Max();
                }
                return $"LSP{(maxId + 1):D3}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo mã loại sản phẩm: {ex.Message}", ex);
            }
        }

        // Thêm một loại sản phẩm mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Ném lỗi nếu thêm thất bại.
        public void ThemLoaiSanPham(LoaiSanPhamDTO loaiSanPham)
        {
            if (loaiSanPham == null)
            {
                throw new ArgumentNullException(nameof(loaiSanPham), "Đối tượng loại sản phẩm không được null.");
            }
            if (string.IsNullOrWhiteSpace(loaiSanPham.MaLoaiSP) || string.IsNullOrWhiteSpace(loaiSanPham.TenLoaiSP))
            {
                throw new ArgumentException("Tên loại sản phẩm không được để trống.");
            }
            _loaiSanPhamDAL.ThemLoaiSanPham(loaiSanPham);
        }

        // Cập nhật thông tin một loại sản phẩm.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Ném lỗi nếu cập nhật thất bại.
        public void CapNhatLoaiSanPham(LoaiSanPhamDTO loaiSanPham)
        {
            if (loaiSanPham == null)
            {
                throw new ArgumentNullException(nameof(loaiSanPham), "Đối tượng loại sản phẩm không được null.");
            }
            if (string.IsNullOrWhiteSpace(loaiSanPham.MaLoaiSP) || string.IsNullOrWhiteSpace(loaiSanPham.TenLoaiSP))
            {
                throw new ArgumentException("Vui lòng chọn loại sản phẩm");
            }
            _loaiSanPhamDAL.CapNhatLoaiSanPham(loaiSanPham);
        }

        // Xóa một loại sản phẩm dựa trên mã loại sản phẩm.
        // Ném lỗi nếu xóa thất bại hoặc loại sản phẩm đang được sử dụng.
        public void XoaLoaiSanPham(string maLoaiSP)
        {
            if (string.IsNullOrWhiteSpace(maLoaiSP))
            {
                throw new ArgumentException("Vui lòng chọn loại sản phẩm");
            }
            _loaiSanPhamDAL.XoaLoaiSanPham(maLoaiSP);
        }
    }
}