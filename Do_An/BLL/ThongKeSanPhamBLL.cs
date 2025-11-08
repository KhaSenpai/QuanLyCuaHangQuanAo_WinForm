using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ThongKeSanPhamBLL
    {
        // Đối tượng DAL để truy cập dữ liệu
        private readonly ThongKeSanPhamDAL _thongKeSanPhamDAL;

        // Constructor khởi tạo đối tượng DAL
        public ThongKeSanPhamBLL()
        {
            _thongKeSanPhamDAL = new ThongKeSanPhamDAL();
        }

        // Lấy danh sách tất cả sản phẩm và gán trạng thái
        public List<ThongKeSanPhamDTO> GetAllSanPham()
        {
            var sanPhamList = _thongKeSanPhamDAL.GetAllSanPham();
            foreach (var sp in sanPhamList)
            {
                // Gán trạng thái "Chuẩn bị hết hàng" nếu số lượng tồn <= 3
                sp.TrangThai = sp.SoLuongTon <= 3 ? "Chuẩn bị hết hàng" : "";
            }
            return sanPhamList;
        }

        // Lấy danh sách sản phẩm đã bán trong khoảng thời gian
        public List<ThongKeSanPhamDTO> GetSanPhamDaBan(DateTime startDate, DateTime endDate)
        {
            return _thongKeSanPhamDAL.GetSanPhamDaBan(startDate, endDate);
        }

        // Lấy danh sách sản phẩm tồn ít và gán trạng thái
        public List<ThongKeSanPhamDTO> GetSanPhamTonIt()
        {
            var sanPhamList = _thongKeSanPhamDAL.GetSanPhamTonIt();
            foreach (var sp in sanPhamList)
            {
                // Gán trạng thái "Chuẩn bị hết hàng" cho tất cả sản phẩm tồn ít
                sp.TrangThai = "Chuẩn bị hết hàng";
            }
            return sanPhamList;
        }

        // Lấy danh sách top 3 sản phẩm bán chạy trong khoảng thời gian
        public List<ThongKeSanPhamDTO> GetSanPhamBanChay(DateTime startDate, DateTime endDate)
        {
            return _thongKeSanPhamDAL.GetSanPhamBanChay(startDate, endDate);
        }

        // Lấy danh sách sản phẩm mới nhập và gán trạng thái
        public List<ThongKeSanPhamDTO> GetSanPhamMoiNhap()
        {
            var sanPhamList = _thongKeSanPhamDAL.GetSanPhamMoiNhap();
            foreach (var sp in sanPhamList)
            {
                // Gán trạng thái "Mới nhập" cho tất cả sản phẩm mới nhập
                sp.TrangThai = "Mới nhập";
            }
            return sanPhamList;
        }
    }
}