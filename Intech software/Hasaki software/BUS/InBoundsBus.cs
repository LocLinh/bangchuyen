using Intech_software.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech_software.BUS
{
    internal class InBoundsBus
    {
        ConnectionFactory connectionFactory = new ConnectionFactory();

        DataTable dt = new DataTable();
        public DataTable GetTable()
        {
            try
            {
                dt = connectionFactory.GetData("select row_number() over(order by thoiGian) as STT," +
                    "ngay," +
                    "thoiGian," +
                    "maKienHang," +
                    "khoiLuong," +
                    "chieuDai," +
                    "chieuRong," +
                    "chieuCao," +
                    "maZone," +
                    "trangThai," +
                    "tenTK  from InBounds");

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetTable(string ngay, string thoiGian, string maKienHang, string khoiLuong, string chieuDai, string chieuRong, string chieuCao, string maZone, string trangThai, string tenTK)
        {
            try
            {
                if (connectionFactory.SetData("insert into InBounds values('" + ngay + "', '" + thoiGian + "', '" + maKienHang + "', '" + khoiLuong + "', '" + chieuDai + "', '" + chieuRong + "', '" + chieuCao + "', '" + maZone + "', '" + trangThai + "' , '" + tenTK + "')"))
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(string date)
        {
            try
            {
                dt = connectionFactory.GetData("select row_number() over(order by thoiGian) as STT," +
                    " ngay," +
                    " thoiGian," +
                    " maKienHang," +
                    "khoiLuong," +
                    "chieuDai," +
                    "chieuRong," +
                    "chieuCao," +
                    "maZone," +
                    "trangThai," +
                    "tenTK from InBounds where ngay = '" + date + "'");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetTableDate(string date)
        {
            try
            {
                dt = connectionFactory.GetData("select row_number() over(order by thoiGian) as STT," +
                    "ngay," +
                    "thoiGian," +
                    "maKienHang," +
                    "khoiLuong," +
                    "chieuDai," +
                    "chieuRong," +
                    "chieuCao," +
                    "maZone," +
                    "trangThai," +
                    "tenTK  from InBounds where ngay = '"+date+"'");

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
