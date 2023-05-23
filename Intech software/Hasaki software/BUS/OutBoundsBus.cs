using Intech_software.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech_software.BUS
{
    internal class OutBoundsBus
    {
        ConnectionFactory connectionFactory = new ConnectionFactory();
        DataTable dt = new DataTable();

        public DataTable GetTable()
        {
            try
            {
                dt = connectionFactory.GetData("select row_number() over(order by thoiGian) as STT," +
                    " ngay," +
                    " thoiGian," +
                    " maKienHang," +
                    "tenTK from OutBounds");
                connectionFactory.CloseConnect();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetTable(string ngay, string thoiGian, string maKH, string tenTK)
        {
            try
            {
                return connectionFactory.SetData("insert into OutBounds values ('" + ngay + "', '" + thoiGian + "', '" + maKH + "', '" + tenTK + "')");
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
                    "tenTK from OutBounds where ngay = '" + date + "'");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
