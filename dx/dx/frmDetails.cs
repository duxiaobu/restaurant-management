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
    public partial class frmDetails : Form
    {
        public frmDetails()
        {
            InitializeComponent();
        }
        public string TableName;

        private void frmDetails_Load(object sender, EventArgs e)
        {
            txtName.Text = TableName.Trim();
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tb_room where RoomName='" + txtName.Text + "'", conn);
            MySqlDataReader msdr = cmd.ExecuteReader();
            msdr.Read();
            txtNum.Text = msdr["ID"].ToString().Trim();
            txtJc.Text = msdr["RoomJC"].ToString().Trim();
            txtBjf.Text = msdr["RoomBJF"].ToString().Trim();
            txtWz.Text = msdr["RoomWZ"].ToString().Trim();
            txtZt.Text = msdr["RoomZT"].ToString().Trim();
            txtLx.Text = msdr["RoomType"].ToString().Trim();
            txtBz.Text = msdr["RoomBZ"].ToString().Trim();
            string qt = msdr["zhangdandate"].ToString() + "开始用餐" + "\n" + "用餐人数：" + msdr["Num"].ToString();
            if (txtZt.Text == "待用")
            {
                richTextBox1.Text = "暂时没有其他信息...";
            }
            else
            {
                richTextBox1.Text = qt;
            }
            msdr.Close();
            conn.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
