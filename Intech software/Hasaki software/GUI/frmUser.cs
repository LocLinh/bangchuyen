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
    public partial class frmUser : Form
    {
        AccountsBus accountsBus = new AccountsBus();

        public frmUser()
        {
            InitializeComponent();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            ShowAccountInfo();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckYourInfo())
            {
                MessageBox.Show("Thông tin không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (accountsBus.CheckId(txtMaTK.Text))
            {
                MessageBox.Show("Tài khoản này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (accountsBus.AddAccount(txtMaTK.Text, txtTenTK.Text, txtMatKhau.Text))
            {
                ShowAccountInfo();
                txtMaTK.Text = string.Empty;
                txtTenTK.Text = string.Empty;
                txtMatKhau.Text = string.Empty;
                MessageBox.Show("Đã thêm mới tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Tài khoản chưa được thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMaTK.Text == string.Empty)
            {
                MessageBox.Show("Mã tài khoản không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaTK.Text == "TK00")
            {
                MessageBox.Show("Không thể xóa tài khoản này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (accountsBus.DeleteAccount(txtMaTK.Text))
            {
                ShowAccountInfo();
                txtMaTK.Text = string.Empty;
                txtTenTK.Text = string.Empty;
                txtMatKhau.Text = string.Empty;
                MessageBox.Show("Tài khoản đã được xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Tài khoản chưa được xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (CheckYourInfo())
            {
                MessageBox.Show("Thông tin không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (accountsBus.EditAccount(txtMaTK.Text, txtTenTK.Text, txtMatKhau.Text))
            {
                ShowAccountInfo();
                txtMaTK.Text = string.Empty;
                txtTenTK.Text = string.Empty;
                txtMatKhau.Text = string.Empty;
                MessageBox.Show("Tài khoản đã được thay đổi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Tài khoản chưa được thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        void ShowAccountInfo()
        {
            DataTable dt = new DataTable();
            dt = accountsBus.GetTable();
            dataGridView1.DataSource = dt;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
        }

        bool CheckYourInfo()
        {
            if (txtMaTK.Text == string.Empty)
                return true;
            else if (txtTenTK.Text == string.Empty)
                return true;
            else if (txtMatKhau.Text == string.Empty)
                return true;
            else
                return false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idRows;
            idRows = e.RowIndex;
            try
            {
                txtMaTK.Text = dataGridView1.Rows[idRows].Cells[1].Value.ToString();
                txtTenTK.Text = dataGridView1.Rows[idRows].Cells[2].Value.ToString();
                txtMatKhau.Text = dataGridView1.Rows[idRows].Cells[3].Value.ToString();
            }
            catch 
            {
                return;
            }
        }

        private void frmUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();

        }
    }
}
