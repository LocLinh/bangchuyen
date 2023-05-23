using Intech_software.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech_software.BUS
{
    internal class HasakiSystemBus
    {
        ConnectionFactory conn = new ConnectionFactory();

        DataTable dt = new DataTable();

        //public DataTable GetTable(string maKienHang, string ngay)
        //{
        //    try
        //    {
        //        dt = conn.GetData("select ngay as N'Ngày'," +
        //            " maKienHang as N'Mã kiện hàng'," +
        //            " maZone as N'Zone'," +
        //            " trangThai as N'Trạng thái' from HasakiSystem where maKienHang = '" + maKienHang + "' and ngay = '"+ngay+"'");
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetTable(string maKienHang)
        {
            try
            {
                dt = conn.GetData("select ngay as N'Ngày'," +
                    " maKienHang as N'Mã kiện hàng'," +
                    " maZone as N'Zone'," +
                    " trangThai as N'Trạng thái' from HasakiSystem where maKienHang = '" + maKienHang + "'");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetTable(string ngay, string maKienHang, string maZone, string trangThai)
        {
            try
            {
                if (conn.SetData("insert into HasakiSystem(ngay, maKienHang, maZone, trangThai) values ('"+ngay+"', '"+maKienHang+"', '"+maZone+"', '"+trangThai+"')"))               
                    return true;
                else
                    return false;
                
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckDate(string ngay)
        {
            try
            {
                if(conn.ReadData("select * from HasakiSystem where ngay = '"+ngay+"'"))
                    return true;
                else 
                    return false;

            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckCode(string code)
        {
            try
            {
                if (conn.ReadData("select * from HasakiSystem where maKienHang = '" + code + "'"))
                    return true;
                else 
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
