using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Intech_software.DAL
{
    class ConnectionFactory
    {
        SqlConnection conn = null;
        SqlCommand cmd = null;

        public static string strConn = new SqlConnectionStringBuilder()
        { 
            DataSource = ".\\SQLExpress",
            InitialCatalog = "IntectSoft",
            IntegratedSecurity =true
        }.ConnectionString;

        public ConnectionFactory()
        {
            conn = new SqlConnection(strConn);
        }

        public void OpenConnect()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        public void CloseConnect()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = conn;
            try
            {
                this.OpenConnect();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                this.CloseConnect();
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                this.CloseConnect();
                throw ex;
            }
            return dt;
        }

        public bool ReadData(string query)
        {
            cmd = new SqlCommand();
            cmd.Connection = conn;
            try
            {
                this.OpenConnect();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    this.CloseConnect();
                    return true; // đã tồn tại
                }
                else
                {
                    this.CloseConnect();
                    return false; // chưa có
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetData(string query)
        {
            cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            try
            {
                this.OpenConnect();
                cmd.ExecuteNonQuery();
                this.CloseConnect();
                return true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                this.CloseConnect();              
                throw ex;
                return false;
            }
        }
    }
}
