using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class SanPhamBLL
    {
        private readonly SanPhamDAL _sanPhamDAL;

        public SanPhamBLL()
        {
            _sanPhamDAL = new SanPhamDAL();
        }

        public List<SanPhamDTO> LayTatCaSanPham()
        {
            return _sanPhamDAL.LayTatCaSanPham();
        }

        public bool ThemSanPham(SanPhamDTO sanPham)
        {
            if (sanPham.DonGiaBan <= 0 || sanPham.DonGiaNhap <= 0)
                throw new Exception("Đơn giá bán và nhập phải lớn hơn 0!");
            if (sanPham.SoLuongTon < 0)
                throw new Exception("Số lượng tồn phải lớn hơn hoặc bằng 0!");
            if (string.IsNullOrWhiteSpace(sanPham.TenSP) || string.IsNullOrWhiteSpace(sanPham.MaThuongHieu) ||
                string.IsNullOrWhiteSpace(sanPham.MaLoaiSP) || string.IsNullOrWhiteSpace(sanPham.MaChatLieu) ||
                string.IsNullOrWhiteSpace(sanPham.MaNCC))
                throw new Exception("Thông tin bắt buộc không được để trống!");
            if (sanPham.NgaySanXuat > DateTime.Now)
                throw new Exception("Ngày sản xuất không được lớn hơn ngày hiện tại!");

            // Kiểm tra hình ảnh
            if (sanPham.HinhAnh != null && string.IsNullOrWhiteSpace(sanPham.HinhAnh.DuongDanHinh))
                throw new Exception("Đường dẫn hình ảnh không được để trống!");

            return _sanPhamDAL.ThemSanPham(sanPham);
        }

        public bool CapNhatSanPham(SanPhamDTO sanPham)
        {
            if (sanPham.DonGiaBan <= 0 || sanPham.DonGiaNhap <= 0)
                throw new Exception("Đơn giá bán và nhập phải lớn hơn 0!");
            if (sanPham.SoLuongTon < 0)
                throw new Exception("Số lượng tồn phải lớn hơn hoặc bằng 0!");
            if (string.IsNullOrWhiteSpace(sanPham.TenSP) || string.IsNullOrWhiteSpace(sanPham.MaThuongHieu) ||
                string.IsNullOrWhiteSpace(sanPham.MaLoaiSP) || string.IsNullOrWhiteSpace(sanPham.MaChatLieu) ||
                string.IsNullOrWhiteSpace(sanPham.MaNCC))
                throw new Exception("Thông tin bắt buộc không được để trống!");
            if (sanPham.NgaySanXuat > DateTime.Now)
                throw new Exception("Ngày sản xuất không được lớn hơn ngày hiện tại!");

            // Kiểm tra hình ảnh
            if (sanPham.HinhAnh != null && string.IsNullOrWhiteSpace(sanPham.HinhAnh.DuongDanHinh))
                throw new Exception("Đường dẫn hình ảnh không được để trống!");

            return _sanPhamDAL.CapNhatSanPham(sanPham);
        }

        public bool XoaSanPham(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                throw new Exception("Mã sản phẩm không được để trống!");
            return _sanPhamDAL.XoaSanPham(maSP);
        }

        public List<SanPhamDTO> TimKiemSanPham(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return _sanPhamDAL.LayTatCaSanPham();
            return _sanPhamDAL.TimKiemSanPham(tuKhoa);
        }

        public int LayTongSoSanPham()
        {
            return _sanPhamDAL.LayTongSoSanPham();
        }

        public List<ChatLieuDTO> LayTatCaChatLieu()
        {
            return _sanPhamDAL.LayTatCaChatLieu();
        }

        public List<LoaiSanPhamDTO> LayTatCaLoaiSanPham()
        {
            return _sanPhamDAL.LayTatCaLoaiSanPham();
        }

        public List<NhaCungCapDTO> LayTatCaNhaCungCap()
        {
            return _sanPhamDAL.LayTatCaNhaCungCap();
        }

        public List<ThuongHieuDTO> LayTatCaThuongHieu()
        {
            return _sanPhamDAL.LayTatCaThuongHieu();
        }

        public string TaoMaSanPham()
        {
            return _sanPhamDAL.TaoMaSanPham();
        }

        public string TaoMaHinhAnh()
        {
            return _sanPhamDAL.TaoMaHinhAnh();
        }
    }
}