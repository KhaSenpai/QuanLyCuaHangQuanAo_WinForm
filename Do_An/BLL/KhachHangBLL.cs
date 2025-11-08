using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    // Tầng logic nghiệp vụ (BLL) cho thực thể Khách Hàng.
    // Chứa các quy tắc nghiệp vụ và tương tác với tầng DAL để xử lý dữ liệu.
    public class KhachHangBLL
    {
        private readonly KhachHangDAL _khachHangDAL;

        // Hàm khởi tạo, tạo một đối tượng KhachHangDAL để tương tác với cơ sở dữ liệu.
        public KhachHangBLL()
        {
            _khachHangDAL = new KhachHangDAL();
        }

        // Lấy toàn bộ danh sách khách hàng.
        // Trả về danh sách các đối tượng KhachHangDTO.
        // Ném lỗi nếu có vấn đề trong quá trình lấy dữ liệu.
        public List<KhachHangDTO> LayTatCa()
        {
            try
            {
                var danhSach = _khachHangDAL.LayTatCa();
                if (danhSach == null)
                {
                    throw new InvalidOperationException("Danh sách khách hàng trả về là null.");
                }
                return danhSach;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khách hàng: {ex.Message}", ex);
            }
        }

        // Tìm kiếm khách hàng dựa trên từ khóa.
        // Trả về danh sách các khách hàng phù hợp.
        // Ném lỗi nếu có vấn đề trong quá trình tìm kiếm.
        public List<KhachHangDTO> TimKiem(string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    return LayTatCa();
                }
                return _khachHangDAL.TimKiem(tuKhoa.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", ex);
            }
        }

        // Sinh mã khách hàng mới theo định dạng KHxxxx (VD: KH0001, KH0002).
        // Trả về mã khách hàng mới không trùng lặp.
        // Ném lỗi nếu có vấn đề trong quá trình sinh mã.
        public string SinhMaKhachHang()
        {
            try
            {
                var danhSach = LayTatCa();
                int soThuTu = danhSach.Count + 1;
                string maKhachHangMoi;

                do
                {
                    maKhachHangMoi = $"KH{soThuTu:D4}";
                    soThuTu++;
                } while (danhSach.Exists(kh => kh.MaKhachHang.Equals(maKhachHangMoi, StringComparison.OrdinalIgnoreCase)));

                return maKhachHangMoi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi sinh mã khách hàng: {ex.Message}", ex);
            }
        }

        // Thêm khách hàng mới vào cơ sở dữ liệu.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi thêm.
        // Ném lỗi nếu thêm thất bại hoặc dữ liệu không hợp lệ.
        public void Them(KhachHangDTO khachHang)
        {
            try
            {
                KiemTraDuLieuKhachHang(khachHang);

                // Kiểm tra trùng lặp MaKhachHang
                var danhSach = LayTatCa();
                if (danhSach.Exists(kh => kh.MaKhachHang.Equals(khachHang.MaKhachHang, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Mã khách hàng '{khachHang.MaKhachHang}' đã tồn tại.");
                }

                if (!_khachHangDAL.Them(khachHang))
                {
                    throw new InvalidOperationException("Thêm khách hàng thất bại.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm khách hàng: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin khách hàng.
        // Kiểm tra tính hợp lệ của dữ liệu trước khi cập nhật.
        // Ném lỗi nếu cập nhật thất bại hoặc dữ liệu không hợp lệ.
        public void CapNhat(KhachHangDTO khachHang)
        {
            try
            {
                KiemTraDuLieuKhachHang(khachHang);

                if (!_khachHangDAL.CapNhat(khachHang))
                {
                    throw new InvalidOperationException($"Không tìm thấy khách hàng với mã '{khachHang.MaKhachHang}'.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật khách hàng: {ex.Message}", ex);
            }
        }

        // Xóa khách hàng dựa trên mã khách hàng.
        // Ném lỗi nếu xóa thất bại hoặc mã khách hàng không hợp lệ.
        public void Xoa(string maKhachHang)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maKhachHang))
                {
                    throw new ArgumentException("Mã khách hàng không được để trống.");
                }

                if (!_khachHangDAL.Xoa(maKhachHang.Trim()))
                {
                    throw new InvalidOperationException($"Không tìm thấy khách hàng với mã '{maKhachHang}'.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa khách hàng: {ex.Message}", ex);
            }
        }

        // Kiểm tra tính hợp lệ của dữ liệu khách hàng.
        // Ném lỗi nếu dữ liệu không hợp lệ.
        private void KiemTraDuLieuKhachHang(KhachHangDTO khachHang)
        {
            if (khachHang == null)
            {
                throw new ArgumentNullException(nameof(khachHang), "Đối tượng khách hàng không được null.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.MaKhachHang))
            {
                throw new ArgumentException("Mã khách hàng không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.TenKhachHang))
            {
                throw new ArgumentException("Tên khách hàng không được để trống.");
            }
            if (!Regex.IsMatch(khachHang.TenKhachHang, @"^[a-zA-ZÀ-ỹ\s]*$"))
            {
                throw new ArgumentException("Họ tên chỉ được chứa chữ cái và khoảng trắng, không chứa số hoặc ký tự đặc biệt.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.SoDienThoai))
            {
                throw new ArgumentException("Số điện thoại không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.DiaChi))
            {
                throw new ArgumentException("Địa chỉ không được để trống.");
            }
            
            if (!Regex.IsMatch(khachHang.TenKhachHang, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                throw new ArgumentException("Tên khách hàng chỉ được chứa chữ cái, khoảng trắng và dấu tiếng Việt.");
            }
            if (!Regex.IsMatch(khachHang.SoDienThoai, @"^0\d{9}$"))
            {
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và 10 chữ số.");
            }
            if (khachHang.NgaySinh.HasValue && khachHang.NgaySinh > DateTime.Now)
            {
                throw new ArgumentException("Ngày sinh không được lớn hơn ngày hiện tại.");
            }
            if (!string.IsNullOrEmpty(khachHang.Email) && !Regex.IsMatch(khachHang.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                throw new ArgumentException("Email không đúng định dạng.");
            }
            if (!string.IsNullOrEmpty(khachHang.GioiTinh) && khachHang.GioiTinh != "Nam" && khachHang.GioiTinh != "Nữ" && khachHang.GioiTinh != "Khác")
            {
                throw new ArgumentException("Giới tính phải là 'Nam', 'Nữ' hoặc 'Khác'.");
            }
        }
    }
}