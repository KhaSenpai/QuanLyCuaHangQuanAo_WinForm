using System;

namespace DTO
{
    public class SanPhamDTO
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuongTon { get; set; }
        public decimal DonGiaBan { get; set; }
        public decimal DonGiaNhap { get; set; }
        public string MauSac { get; set; }
        public string KichCo { get; set; }
        public DateTime NgaySanXuat { get; set; }
        public string MoTa { get; set; }
        public string MaThuongHieu { get; set; }
        public string TenThuongHieu { get; set; }
        public string MaLoaiSP { get; set; }
        public string TenLoaiSP { get; set; }
        public string MaChatLieu { get; set; }
        public string TenChatLieu { get; set; }
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public HinhAnhDTO HinhAnh { get; set; } // Thay List<HinhAnhDTO> bằng HinhAnhDTO
    }
}