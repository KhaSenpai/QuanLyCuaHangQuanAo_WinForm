using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Lớp xử lý logic nghiệp vụ cho hóa đơn và chi tiết hóa đơn.
    public class HoaDonBLL
    {
        private readonly HoaDonDAL _hoaDonDAL;

        // Khởi tạo HoaDonBLL với một instance của HoaDonDAL.
        public HoaDonBLL()
        {
            _hoaDonDAL = new HoaDonDAL();
        }

        // Lấy danh sách tất cả hóa đơn từ DAL.
        // Trả về danh sách các đối tượng HoaDonDTO.
        public List<HoaDonDTO> LayDanhSachHoaDon()
        {
            try
            {
                return _hoaDonDAL.LayDanhSachHoaDon();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách hóa đơn: {ex.Message}", ex);
            }
        }

        // Thêm một hóa đơn mới sau khi kiểm tra tính hợp lệ của dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemHoaDon(HoaDonDTO hoaDon)
        {
            try
            {
                if (hoaDon == null)
                    throw new ArgumentNullException(nameof(hoaDon), "Hóa đơn không được null!");
                if (string.IsNullOrEmpty(hoaDon.MaKhachHang) || string.IsNullOrEmpty(hoaDon.MaNhanVien))
                    throw new Exception("Khách hàng và nhân viên không được để trống!");
                if (hoaDon.NgayLapHoaDon > DateTime.Now)
                    throw new Exception("Ngày lập hóa đơn không hợp lệ!");
                if (hoaDon.TongTien < 0)
                    throw new Exception("Tổng tiền không hợp lệ!");

                return _hoaDonDAL.ThemHoaDon(hoaDon);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm hóa đơn: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin hóa đơn sau khi kiểm tra tính hợp lệ của dữ liệu.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        public bool CapNhatHoaDon(HoaDonDTO hoaDon)
        {
            try
            {
                if (hoaDon == null)
                    throw new ArgumentNullException(nameof(hoaDon), "Hóa đơn không được null!");
                if (string.IsNullOrEmpty(hoaDon.MaKhachHang) || string.IsNullOrEmpty(hoaDon.MaNhanVien))
                    throw new Exception("Khách hàng và nhân viên không được để trống!");
                if (hoaDon.NgayLapHoaDon > DateTime.Now)
                    throw new Exception("Ngày lập hóa đơn không hợp lệ!");
                if (hoaDon.TongTien < 0)
                    throw new Exception("Tổng tiền không hợp lệ!");

                return _hoaDonDAL.CapNhatHoaDon(hoaDon);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật hóa đơn: {ex.Message}", ex);
            }
        }

        // Xóa hóa đơn dựa trên mã hóa đơn.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaHoaDon(string maHoaDon)
        {
            try
            {
                if (string.IsNullOrEmpty(maHoaDon))
                    throw new Exception("Mã hóa đơn không được để trống!");
                return _hoaDonDAL.XoaHoaDon(maHoaDon);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa hóa đơn: {ex.Message}", ex);
            }
        }

        // Lấy danh sách chi tiết hóa đơn dựa trên mã hóa đơn.
        // Trả về danh sách các đối tượng ChiTietHoaDonDTO.
        public List<ChiTietHoaDonDTO> LayChiTietHoaDon(string maHoaDon)
        {
            try
            {
                if (string.IsNullOrEmpty(maHoaDon))
                    throw new Exception("Mã hóa đơn không được để trống!");
                return _hoaDonDAL.LayChiTietHoaDon(maHoaDon);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết hóa đơn: {ex.Message}", ex);
            }
        }

        // Thêm chi tiết hóa đơn mới sau khi kiểm tra tính hợp lệ của dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemChiTietHoaDon(ChiTietHoaDonDTO chiTiet)
        {
            try
            {
                if (chiTiet == null)
                    throw new ArgumentNullException(nameof(chiTiet), "Chi tiết hóa đơn không được null!");
                if (string.IsNullOrEmpty(chiTiet.MaHoaDon) || string.IsNullOrEmpty(chiTiet.MaSanPham))
                    throw new Exception("Mã hóa đơn và mã sản phẩm không được để trống!");
                if (chiTiet.SoLuong <= 0)
                    throw new Exception("Số lượng phải lớn hơn 0!");
                if (chiTiet.DonGia <= 0)
                    throw new Exception("Đơn giá phải lớn hơn 0!");

                return _hoaDonDAL.ThemChiTietHoaDon(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm chi tiết hóa đơn: {ex.Message}", ex);
            }
        }

        // Xóa chi tiết hóa đơn dựa trên mã hóa đơn và mã sản phẩm.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaChiTietHoaDon(string maHoaDon, string maSanPham)
        {
            try
            {
                if (string.IsNullOrEmpty(maHoaDon) || string.IsNullOrEmpty(maSanPham))
                    throw new Exception("Mã hóa đơn và mã sản phẩm không được để trống!");
                return _hoaDonDAL.XoaChiTietHoaDon(maHoaDon, maSanPham);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chi tiết hóa đơn: {ex.Message}", ex);
            }
        }

        // Sinh mã hóa đơn mới dựa trên số lượng hóa đơn hiện có.
        // Trả về mã hóa đơn dạng HDxxxxxx (VD: HD000001).
        public string TaoMaHoaDon()
        {
            try
            {
                var danhSachHoaDon = LayDanhSachHoaDon();
                int maxId = 0;
                if (danhSachHoaDon.Any())
                {
                    maxId = danhSachHoaDon
                        .Select(hd => int.Parse(hd.MaHoaDon.Replace("HD", "")))
                        .Max();
                }
                return $"HD{(maxId + 1):D6}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi sinh mã hóa đơn: {ex.Message}", ex);
            }
        }

        // Lấy danh sách khách hàng từ DAL.
        // Trả về Dictionary ánh xạ tên khách hàng với mã khách hàng.
        public Dictionary<string, string> LayDanhSachKhachHang()
        {
            try
            {
                return _hoaDonDAL.LayDanhSachKhachHang();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khách hàng: {ex.Message}", ex);
            }
        }

        // Lấy danh sách nhân viên từ DAL.
        // Trả về Dictionary ánh xạ tên nhân viên với mã nhân viên.
        public Dictionary<string, string> LayDanhSachNhanVien()
        {
            try
            {
                return _hoaDonDAL.LayDanhSachNhanVien();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }

        // Lấy danh sách sản phẩm từ DAL.
        // Trả về Dictionary ánh xạ tên sản phẩm với mã sản phẩm.
        public Dictionary<string, string> LayDanhSachSanPham()
        {
            try
            {
                return _hoaDonDAL.LayDanhSachSanPham();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}", ex);
            }
        }

        // Lấy danh sách khuyến mãi từ DAL.
        // Trả về Dictionary ánh xạ tên khuyến mãi với mã khuyến mãi.
        public Dictionary<string, string> LayDanhSachKhuyenMai()
        {
            try
            {
                return _hoaDonDAL.LayDanhSachKhuyenMai();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khuyến mãi: {ex.Message}", ex);
            }
        }

        // Lấy đơn giá sản phẩm từ DAL dựa trên mã sản phẩm.
        // Trả về đơn giá hoặc ném ngoại lệ nếu mã sản phẩm không hợp lệ.
        public decimal LayDonGiaSanPham(string maSanPham)
        {
            try
            {
                if (string.IsNullOrEmpty(maSanPham))
                    throw new Exception("Mã sản phẩm không được để trống!");
                return _hoaDonDAL.LayDonGiaSanPham(maSanPham);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn giá sản phẩm: {ex.Message}", ex);
            }
        }

        // Lấy phần trăm khuyến mãi từ DAL dựa trên mã khuyến mãi.
        // Trả về phần trăm khuyến mãi hoặc ném ngoại lệ nếu mã khuyến mãi không hợp lệ.
        public decimal LayPhanTramKhuyenMai(string maKhuyenMai)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhuyenMai))
                    throw new Exception("Mã khuyến mãi không được để trống!");
                return _hoaDonDAL.LayPhanTramKhuyenMai(maKhuyenMai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phần trăm khuyến mãi: {ex.Message}", ex);
            }
        }
        // Lấy số lượng tồn kho của sản phẩm từ DAL dựa trên mã sản phẩm.
        // Trả về số lượng tồn kho hoặc ném ngoại lệ nếu mã sản phẩm không hợp lệ.
        public int LaySoLuongTonSanPham(string maSanPham)
        {
            try
            {
                if (string.IsNullOrEmpty(maSanPham))
                    throw new Exception("Mã sản phẩm không được để trống!");
                return _hoaDonDAL.LaySoLuongTonSanPham(maSanPham);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy số lượng tồn kho của sản phẩm: {ex.Message}", ex);
            }
        }
    }
}