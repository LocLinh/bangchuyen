using Intech_software.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Intech_software.GUI
{
    public partial class frmServerConnection : Form
    {
        string strConnect = string.Empty;

        public frmServerConnection()
        {
            InitializeComponent();
        }

        private void frmServerConnection_Load(object sender, EventArgs e)
        {
            txtPassWord.UseSystemPasswordChar = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Do you want to exit?", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dg == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {


            if (txtServerName.Text == string.Empty)
            {
                MessageBox.Show("Tên server không được để trống!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtDatabaseName.Text == string.Empty)
            {
                MessageBox.Show("Tên Database không được để trống!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtUserName.Text == string.Empty)
            {
                MessageBox.Show("tên đăng nhập không được để trống!", "notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassWord.Text == string.Empty)
            {
                MessageBox.Show("mật khẩu không được để trống!", "notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //strConnect = "Data Source="+txtServerName.Text+";Initial Catalog="+txtDatabaseName.Text+";User ID="+txtUserName.Text+";Password="+txtPassWord.Text+"";
            strConnect = new SqlConnectionStringBuilder()
            {
                DataSource = txtServerName.Text,
                InitialCatalog = txtDatabaseName.Text,
                UserID = txtUserName.Text,
                Password = txtPassWord.Text
            }.ConnectionString;
            SqlConnection conn = new SqlConnection(strConnect);
            try
            {
                conn.Open();
                ConnectionFactory.strConn = strConnect;
                this.Hide();
                frmHome frmHome = new frmHome();
                frmHome.ShowDialog();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void cboShowPassword_Click(object sender, EventArgs e)
        {
            if (cboShowPassword.Checked)
                txtPassWord.UseSystemPasswordChar = false;
            else
                txtPassWord.UseSystemPasswordChar = true;
        }
        
    }
}
