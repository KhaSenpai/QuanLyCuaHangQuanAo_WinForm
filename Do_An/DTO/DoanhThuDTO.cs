using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DoanhThuDTO
    {
        public string MaHoaDon { get; set; }
        public DateTime NgayLapHoaDon { get; set; }
        public decimal TongTien { get; set; }
        public string TenKhachHang { get; set; }
        public string TenNhanVien { get; set; }
        public string TenKhuyenMai { get; set; }
        public decimal PhanTramKhuyenMai { get; set; }
    }
}
