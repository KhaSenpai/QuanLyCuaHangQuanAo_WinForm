using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Nhân Viên.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class NhanVienBLL
    {
        private readonly NhanVienDAL _nhanVienDAL;

        // Hàm khởi tạo, tạo một đối tượng NhanVienDAL để tương tác với cơ sở dữ liệu.
        public NhanVienBLL()
        {
            _nhanVienDAL = new NhanVienDAL();
        }

        // Lấy toàn bộ danh sách nhân viên.
        // Trả về danh sách các đối tượng NhanVienDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<NhanVienDTO> LayTatCaNhanVien()
        {
            return _nhanVienDAL.LayTatCaNhanVien();
        }

        // Tạo mã nhân viên mới.
        // Trả về mã nhân viên mới.
        // Ném lỗi nếu có vấn đề trong quá trình tạo mã.
        public string TaoMaNhanVien()
        {
            return _nhanVienDAL.TaoMaNhanVien();
        }

        // Lấy danh sách tên chức vụ.
        // Trả về danh sách các tên chức vụ.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<string> LayTatCaChucVu()
        {
            return _nhanVienDAL.LayTatCaChucVu();
        }

        // Lấy mã chức vụ dựa trên tên chức vụ.
        // Trả về mã chức vụ hoặc chuỗi rỗng nếu không tìm thấy.
        // Ném lỗi nếu có vấn đề trong quá trình lấy mã.
        public string LayMaChucVuTheoTen(string tenChucVu)
        {
            if (string.IsNullOrWhiteSpace(tenChucVu))
            {
                throw new ArgumentException("Tên chức vụ không được để trống.");
            }
            return _nhanVienDAL.LayMaChucVuTheoTen(tenChucVu);
        }

        // Thêm một nhân viên mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình thêm.
        public bool ThemNhanVien(NhanVienDTO nhanVien)
        {
            if (_nhanVienDAL.KiemTraMaNhanVienTonTai(nhanVien.MaNhanVien))
            {
                throw new ArgumentException("Mã nhân viên đã tồn tại.");
            }
            KiemTraDuLieuNhanVien(nhanVien);
            return _nhanVienDAL.ThemNhanVien(nhanVien);
        }

        // Cập nhật thông tin một nhân viên.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        // Ném lỗi nếu có vấn đề trong quá trình cập nhật.
        public bool CapNhatNhanVien(NhanVienDTO nhanVien)
        {
            KiemTraDuLieuNhanVien(nhanVien);
            return _nhanVienDAL.CapNhatNhanVien(nhanVien);
        }

        // Xóa một nhân viên dựa trên mã nhân viên.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        // Ném lỗi nếu xóa thất bại hoặc nhân viên đang được sử dụng.
        public bool XoaNhanVien(string maNhanVien)
        {
            if (string.IsNullOrWhiteSpace(maNhanVien))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.");
            }
            return _nhanVienDAL.XoaNhanVien(maNhanVien);
        }

        // Tìm kiếm nhân viên dựa trên từ khóa.
        // Trả về danh sách các nhân viên phù hợp.
        // Ném lỗi nếu có vấn đề trong quá trình tìm kiếm.
        public List<NhanVienDTO> TimKiemNhanVien(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return LayTatCaNhanVien();
            }
            return _nhanVienDAL.TimKiemNhanVien(tuKhoa.Trim());
        }

        // Kiểm tra tính hợp lệ của dữ liệu nhân viên.
        // Ném lỗi nếu dữ liệu không hợp lệ.
        private void KiemTraDuLieuNhanVien(NhanVienDTO nhanVien)
        {
            if (nhanVien == null)
            {
                throw new ArgumentNullException(nameof(nhanVien), "Đối tượng nhân viên không được null.");
            }
            if (string.IsNullOrWhiteSpace(nhanVien.MaNhanVien))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(nhanVien.HoTen))
            {
                throw new ArgumentException("Họ tên không được để trống.");
            }
            if (!Regex.IsMatch(nhanVien.HoTen, @"^[a-zA-ZÀ-ỹ\s]*$"))
            {
                throw new ArgumentException("Họ tên chỉ được chứa chữ cái và khoảng trắng, không chứa số hoặc ký tự đặc biệt.");
            }
            if (nhanVien.NgaySinh > DateTime.Now)
            {
                throw new ArgumentException("Ngày sinh không được lớn hơn ngày hiện tại.");
            }
            if (nhanVien.NgaySinh < DateTime.Now.AddYears(-100))
            {
                throw new ArgumentException("Ngày sinh không hợp lệ (quá 100 năm).");
            }
            if (nhanVien.GioiTinh != "Nam" && nhanVien.GioiTinh != "Nữ")
            {
                throw new ArgumentException("Giới tính phải là 'Nam' hoặc 'Nữ'.");
            }
            if (string.IsNullOrWhiteSpace(nhanVien.DiaChi))
            {
                throw new ArgumentException("Địa chỉ không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(nhanVien.SoDienThoai))
            {
                throw new ArgumentException("Số điện thoại không được để trống.");
            }
            if (!Regex.IsMatch(nhanVien.SoDienThoai, @"^0\d{9}$"))
            {
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và có đúng 10 chữ số.");
            }
            if (string.IsNullOrWhiteSpace(nhanVien.MaChucVu))
            {
                throw new ArgumentException("Mã chức vụ không được để trống.");
            }
            if (nhanVien.MaNhanVien.Length > 20)
            {
                throw new ArgumentException("Mã nhân viên không được vượt quá 20 ký tự.");
            }
            if (nhanVien.HoTen.Length > 50)
            {
                throw new ArgumentException("Họ tên không được vượt quá 50 ký tự.");
            }
            if (nhanVien.DiaChi.Length > 100)
            {
                throw new ArgumentException("Địa chỉ không được vượt quá 100 ký tự.");
            }
        }
    }
}