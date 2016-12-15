using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace dx
{
    public partial class frmLock : Form
    {
        public frmLock()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(*) from tb_user where UserPwd='" + textBox1.Text.Trim() + "'", conn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)
            {
                this.Close();
            }
            else
            {
                if (MessageBox.Show("密码错误", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    textBox1.Text = "";
                    textBox1.Focus();
                }

            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pictureBox1_Click(sender,e);
            }
        }
    }
}
