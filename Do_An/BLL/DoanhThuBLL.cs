using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class DoanhThuBLL
    {
        private readonly DoanhThuDAL dal = new DoanhThuDAL();

        public List<DoanhThuDTO> GetDoanhThuByDate(DateTime date)
        {
            return dal.GetDoanhThuByDate(date);
        }

        public List<DoanhThuTongHopDTO> GetDoanhThuByMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Tháng không hợp lệ!");
            return dal.GetDoanhThuByMonth(year, month);
        }

        public List<DoanhThuTongHopDTO> GetDoanhThuByYear(int year)
        {
            if (year < 1900 || year > DateTime.Now.Year)
                throw new ArgumentException("Năm không hợp lệ!");
            return dal.GetDoanhThuByYear(year);
        }

        public decimal GetTongDoanhThuByDate(DateTime date)
        {
            return dal.GetTongDoanhThuByDate(date);
        }

        public decimal GetTongDoanhThuByMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Tháng không hợp lệ!");
            return dal.GetTongDoanhThuByMonth(year, month);
        }
        public List<DoanhThuDTO> GetDoanhThuChiTietByMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Tháng không hợp lệ!");
            if (year < 1900 || year > DateTime.Now.Year)
                throw new ArgumentException("Năm không hợp lệ!");
            return dal.GetDoanhThuChiTietByMonth(year, month);
        }
        public decimal GetTongDoanhThuByYear(int year)
        {
            if (year < 1900 || year > DateTime.Now.Year)
                throw new ArgumentException("Năm không hợp lệ!");
            return dal.GetTongDoanhThuByYear(year);
        }
    }
}
