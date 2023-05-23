using Intech_software.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech_software.BUS
{
    internal class AccountsBus
    {
        ConnectionFactory connectionFactory = new ConnectionFactory();

        DataTable dtGetLoginInfor = new DataTable();
        DataTable dt = new DataTable();

        public DataTable GetLoginInfo(string user, string pass)
        {
            try
            {
                dtGetLoginInfor = connectionFactory.GetData("select * from Accounts where tenTK = '" + user + "' and matKhau = '" + pass + "'");
                connectionFactory.CloseConnect();
                return dtGetLoginInfor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTable()
        {
            try
            {
                dt = connectionFactory.GetData("select row_number() over(order by maTK) as STT," +
                    " maTK," +
                    " tenTK," +
                    " matKhau from Accounts");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddAccount(string maTK, string tenTK, string matKhau)
        {
            try
            {
                return connectionFactory.SetData("insert into Accounts values ('" + maTK + "', '" + tenTK + "', '" + matKhau + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckId(string maTK)
        {
            try
            {
                return connectionFactory.ReadData("select * from Accounts where maTK = '" + maTK + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteAccount(string maTK)
        {
            try
            {
                return connectionFactory.SetData("delete from Accounts where maTK = '" + maTK + "'");
            }
            catch (Exception ex) { throw ex; }
        }

        public bool EditAccount(string maTK, string tenTK, string matKhau)
        {
            try
            {
                return connectionFactory.SetData("update Accounts set tenTK = '" + tenTK + "', matKhau = '" + matKhau + "' where maTK = '" + maTK + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
