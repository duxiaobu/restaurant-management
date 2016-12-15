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
    public partial class frmPwd : Form
    {
        public frmPwd()
        {
            InitializeComponent();
        }
        public string names;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtPwd2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(sender, e);
            }
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPwd1.Text == "")
            {
                MessageBox.Show("请输入密码");
            }
            else
            {
                if (txtPwd1.Text != txtPwd2.Text)
                {
                    MessageBox.Show("两次输入的密码不一致");
                    txtPwd2.Focus();                   //焦点聚集在textBox2中
                }
                else
                {
                    MySqlConnection conn = BaseClass.DBConn.DxCon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("update tb_user set UserPwd='" + txtPwd1.Text + "' where UserName='" + names + "'",conn);
                    cmd.ExecuteNonQuery();
                    if(MessageBox.Show("密码修改成功","提示",MessageBoxButtons.OK,MessageBoxIcon.Asterisk)==DialogResult.OK)
                    {
                        this.Close();
                    }
                }
            }
        }
    }
}
