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
    public partial class frmQxGl : Form
    {
        public frmQxGl()
        {
            InitializeComponent();
        }
        private void frmQxGl_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tb_user", conn);
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                comboBox1.Items.Add(msdr["UserName"].ToString().Trim());
            }
            comboBox1.SelectedIndex = 0;
            msdr.Close();
            comboBox2.SelectedIndex = 0;
            conn.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string userpower = "";
            switch (comboBox2.SelectedItem.ToString())
            {
                case "超级管理员": userpower = "0"; break;
                case "经理": userpower = "1"; break;
                case "一般用户": userpower = "2"; break;
            }
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("update tb_user set power='" + userpower + "'", conn);
            cmd.ExecuteNonQuery();
            if (MessageBox.Show("权限修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}
