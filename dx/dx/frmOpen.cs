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
    public partial class frmOpen : Form
    {
        public frmOpen()
        {
            InitializeComponent();
        }
        public string name;
        public MySqlConnection conn;

        private void frmOpen_Load(object sender, EventArgs e)
        {
            conn = BaseClass.DBConn.DxCon();    
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tb_room",conn);
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                cbNum.Items.Add(msdr["RoomName"].ToString().Trim()); //读出所有的RoomName，去掉字符串前后的空格,再放入cbNum中
            }
            cbNum.SelectedItem = name.Trim();    //显示传入的桌号
            msdr.Close();
            cmd = new MySqlCommand("select * from tb_waiter",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                cbWaiter.Items.Add(msdr["WaiterName"].ToString().Trim());   // 读出所有的服务员的名字供选择
            }
            cbWaiter.SelectedIndex = 0;                             //传入列表的第一个
            msdr.Close();
       }
        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        { 
            //如果按键不是空格并且不是数字并且不是回车键
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                MessageBox.Show("请输入数字");
                e.Handled = true;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtNum.Text == "" || Convert.ToInt32(txtNum.Text) <= 0)
            {
                MessageBox.Show("请输入用餐人数");
            }
            else
            {
                string RoomName = cbNum.SelectedItem.ToString();
                MySqlCommand cmd1 = new MySqlCommand("update tb_room set GuestName='" + txtName.Text + "',zhangdanDate='" + dateTimePicker1.Value.ToString() + "',Num='" + Convert.ToInt32(txtNum.Text) + "',WaiterName='" + cbWaiter.SelectedItem.ToString() + "',RoomZT='使用',RoomBZ='" + txtBZ.Text + "' where RoomName ='" + name +"'",conn);
                cmd1.ExecuteNonQuery();                //更新相应的数据数据
                this.Close();                          //关闭窗体
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
