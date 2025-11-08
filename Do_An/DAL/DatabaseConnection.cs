using System;

namespace DAL
{
    public static class DatabaseConnection
    {
        private static readonly string _connectionString;

        static DatabaseConnection()
        {
            // Định nghĩa chuỗi kết nối trực tiếp
            _connectionString = "Server=TRANMINHKHA;Database=QuanLyCuaHangLocalBrand;Trusted_Connection=True;";

            // Kiểm tra chuỗi kết nối có hợp lệ không
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Chuỗi kết nối không được để trống.");
            }
        }

        public static string ConnectionString => _connectionString;
    }
}