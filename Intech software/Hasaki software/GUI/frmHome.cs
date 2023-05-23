using Intech_software.BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech_software.GUI
{
    public partial class frmHome : Form
    {
        AccountsBus accountsBus = new AccountsBus();
        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            btnInBound.Enabled = false;
            btnOutBound.Enabled = false;
            btnUser.Enabled = false;
        }

        private Form currentForm;
        frmInBound frmInBound = new frmInBound();
        frmOutBound frmOutBound = new frmOutBound();
        private void OpenFrom(Form form)
        {          
            if (currentForm != null)
            {
                currentForm.Visible = false;
            }
            currentForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelBody.Controls.Add(form);
            panelBody.Tag = form;
            form.BringToFront();
            form.Show();
        }

        private void btnInBound_Click(object sender, EventArgs e)
        {
            btnInBound.BackColor = Color.LimeGreen;
            btnOutBound.BackColor = Color.SeaGreen;
            btnUser.BackColor = Color.SeaGreen;  
            OpenFrom(frmInBound);   
            
        }

        private void btnOutBound_Click(object sender, EventArgs e)
        {
            btnInBound.BackColor = Color.SeaGreen;
            btnOutBound.BackColor = Color.LimeGreen;
            btnUser.BackColor = Color.SeaGreen;
            OpenFrom(frmOutBound);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmUser fu = new frmUser();
            fu.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (currentForm != null)
            {
                currentForm.Visible = false;
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Do you want to logout system?", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dg == DialogResult.OK)
            {
                if (currentForm != null)
                {
                    currentForm.Visible = false;
                }
                btnOutBound.Enabled = false;
                btnInBound.Enabled = false;
                btnUser.Enabled = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text == string.Empty)
                {
                    MessageBox.Show("Tài khoản không được để trống.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtPassword.Text == string.Empty)
                {
                    MessageBox.Show("Mật khẩu không được để trống.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataTable dt = new DataTable();
                dt = accountsBus.GetLoginInfo(txtUserName.Text, txtPassword.Text);
                if (dt.Rows.Count > 0)
                {
                    frmInBound.revAccountName = dt.Rows[0][1].ToString();
                    frmOutBound.revAccountName= dt.Rows[0][1].ToString();

                    btnInBound.Enabled = true;
                    btnOutBound.Enabled = true;
                    btnUser.Enabled= true;
                }
                else
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Do you want to exit program?", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dg == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void frmHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
                txtPassword.UseSystemPasswordChar = true;
            else
                txtPassword.UseSystemPasswordChar = false;
        }

        

    }
}
