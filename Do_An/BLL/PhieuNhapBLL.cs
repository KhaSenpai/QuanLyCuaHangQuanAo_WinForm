using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL
{
    // Lớp xử lý logic nghiệp vụ cho phiếu nhập và chi tiết phiếu nhập.
    public class PhieuNhapBLL
    {
        private readonly PhieuNhapDAL _phieuNhapDAL;

        // Khởi tạo PhieuNhapBLL với một instance của PhieuNhapDAL.
        public PhieuNhapBLL()
        {
            _phieuNhapDAL = new PhieuNhapDAL();
        }

        // Lấy danh sách tất cả phiếu nhập từ DAL.
        // Trả về danh sách các đối tượng PhieuNhapDTO.
        public List<PhieuNhapDTO> LayDanhSachPhieuNhap()
        {
            try
            {
                return _phieuNhapDAL.LayDanhSachPhieuNhap();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách phiếu nhập: {ex.Message}", ex);
            }
        }

        // Lấy danh sách chi tiết phiếu nhập dựa trên mã phiếu nhập.
        // Trả về danh sách các đối tượng ChiTietPhieuNhapDTO.
        public List<ChiTietPhieuNhapDTO> LayChiTietPhieuNhap(string maPhieuNhap)
        {
            try
            {
                if (string.IsNullOrEmpty(maPhieuNhap))
                    throw new Exception("Mã phiếu nhập không được để trống!");
                return _phieuNhapDAL.LayChiTietPhieuNhap(maPhieuNhap);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        // Thêm một phiếu nhập mới sau khi kiểm tra tính hợp lệ của dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemPhieuNhap(PhieuNhapDTO phieuNhap)
        {
            try
            {
                if (phieuNhap == null)
                    throw new ArgumentNullException(nameof(phieuNhap), "Phiếu nhập không được null!");
                if (string.IsNullOrEmpty(phieuNhap.MaPhieuNhap))
                    throw new Exception("Mã phiếu nhập không được để trống!");
                if (string.IsNullOrEmpty(phieuNhap.MaNCC) || string.IsNullOrEmpty(phieuNhap.MaNhanVien))
                    throw new Exception("Nhà cung cấp và nhân viên không được để trống!");
                if (phieuNhap.NgayLapPhieuNhap > DateTime.Now)
                    throw new Exception("Ngày lập phiếu nhập không hợp lệ!");
                if (phieuNhap.TongTien < 0)
                    throw new Exception("Tổng tiền không hợp lệ!");

                return _phieuNhapDAL.ThemPhieuNhap(phieuNhap);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm phiếu nhập: {ex.Message}", ex);
            }
        }

        // Cập nhật thông tin phiếu nhập sau khi kiểm tra tính hợp lệ của dữ liệu.
        // Trả về true nếu cập nhật thành công, false nếu thất bại.
        public bool CapNhatPhieuNhap(PhieuNhapDTO phieuNhap)
        {
            try
            {
                if (phieuNhap == null)
                    throw new ArgumentNullException(nameof(phieuNhap), "Phiếu nhập không được null!");
                if (string.IsNullOrEmpty(phieuNhap.MaPhieuNhap))
                    throw new Exception("Mã phiếu nhập không được để trống!");
                if (string.IsNullOrEmpty(phieuNhap.MaNCC) || string.IsNullOrEmpty(phieuNhap.MaNhanVien))
                    throw new Exception("Nhà cung cấp và nhân viên không được để trống!");
                if (phieuNhap.NgayLapPhieuNhap > DateTime.Now)
                    throw new Exception("Ngày lập phiếu nhập không hợp lệ!");
                if (phieuNhap.TongTien < 0)
                    throw new Exception("Tổng tiền không hợp lệ!");

                return _phieuNhapDAL.CapNhatPhieuNhap(phieuNhap);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật phiếu nhập: {ex.Message}", ex);
            }
        }

        // Xóa phiếu nhập dựa trên mã phiếu nhập.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaPhieuNhap(string maPhieuNhap)
        {
            try
            {
                if (string.IsNullOrEmpty(maPhieuNhap))
                    throw new Exception("Mã phiếu nhập không được để trống!");
                return _phieuNhapDAL.XoaPhieuNhap(maPhieuNhap);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa phiếu nhập: {ex.Message}", ex);
            }
        }

        // Thêm chi tiết phiếu nhập mới sau khi kiểm tra tính hợp lệ của dữ liệu.
        // Trả về true nếu thêm thành công, false nếu thất bại.
        public bool ThemChiTietPhieuNhap(ChiTietPhieuNhapDTO chiTiet)
        {
            try
            {
                if (chiTiet == null)
                    throw new ArgumentNullException(nameof(chiTiet), "Chi tiết phiếu nhập không được null!");
                if (string.IsNullOrEmpty(chiTiet.MaPhieuNhap) || string.IsNullOrEmpty(chiTiet.MaSanPham))
                    throw new Exception("Mã phiếu nhập và mã sản phẩm không được để trống!");
                if (chiTiet.SoLuong <= 0)
                    throw new Exception("Số lượng phải lớn hơn 0!");
                if (chiTiet.DonGia <= 0)
                    throw new Exception("Đơn giá phải lớn hơn 0!");

                return _phieuNhapDAL.ThemChiTietPhieuNhap(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        // Xóa chi tiết phiếu nhập dựa trên mã phiếu nhập và mã sản phẩm.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaChiTietPhieuNhap(string maPhieuNhap, string maSanPham)
        {
            try
            {
                if (string.IsNullOrEmpty(maPhieuNhap) || string.IsNullOrEmpty(maSanPham))
                    throw new Exception("Mã phiếu nhập và mã sản phẩm không được để trống!");
                return _phieuNhapDAL.XoaChiTietPhieuNhap(maPhieuNhap, maSanPham);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        // Xóa tất cả chi tiết phiếu nhập dựa trên mã phiếu nhập.
        // Trả về true nếu xóa thành công, false nếu thất bại.
        public bool XoaTatCaChiTietPhieuNhap(string maPhieuNhap)
        {
            try
            {
                if (string.IsNullOrEmpty(maPhieuNhap))
                    throw new Exception("Mã phiếu nhập không được để trống!");
                return _phieuNhapDAL.XoaTatCaChiTietPhieuNhap(maPhieuNhap);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa tất cả chi tiết phiếu nhập: {ex.Message}", ex);
            }
        }

        // Lấy danh sách tên nhà cung cấp từ DAL.
        // Trả về Dictionary ánh xạ tên nhà cung cấp với mã nhà cung cấp.
        public Dictionary<string, string> LayDanhSachTenNhaCungCap()
        {
            try
            {
                return _phieuNhapDAL.LayDanhSachTenNhaCungCap();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}", ex);
            }
        }

        // Lấy danh sách tên nhân viên từ DAL.
        // Trả về Dictionary ánh xạ tên nhân viên với mã nhân viên.
        public Dictionary<string, string> LayDanhSachTenNhanVien()
        {
            try
            {
                return _phieuNhapDAL.LayDanhSachTenNhanVien();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }

        // Lấy danh sách tên sản phẩm từ DAL.
        // Trả về Dictionary ánh xạ tên sản phẩm với mã sản phẩm.
        public Dictionary<string, string> LayDanhSachTenSanPham()
        {
            try
            {
                return _phieuNhapDAL.LayDanhSachTenSanPham();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}", ex);
            }
        }

        // Sinh mã phiếu nhập mới từ DAL.
        // Trả về mã phiếu nhập dạng PNxxx.
        public string SinhMaPhieuNhap()
        {
            try
            {
                return _phieuNhapDAL.SinhMaPhieuNhap();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi sinh mã phiếu nhập: {ex.Message}", ex);
            }
        }

        // Lấy đơn giá nhập của sản phẩm từ DAL dựa trên mã sản phẩm.
        // Trả về đơn giá hoặc ném ngoại lệ nếu mã sản phẩm không hợp lệ.
        public decimal LayDonGiaNhap(string maSanPham)
        {
            try
            {
                if (string.IsNullOrEmpty(maSanPham))
                    throw new Exception("Mã sản phẩm không được để trống!");
                return _phieuNhapDAL.LayDonGiaNhap(maSanPham);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy đơn giá nhập: {ex.Message}", ex);
            }
        }
    }
}