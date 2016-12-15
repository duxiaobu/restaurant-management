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
    public partial class frmJZ : Form
    {
        public frmJZ()
        {
            InitializeComponent();
        }
        public string Rname;
        public string price;
        public string bjf;
        private void frmJZ_Load(object sender, EventArgs e)
        {

        }
        private void txtmoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                MessageBox.Show("请输入数字");
                e.Handled = true;
            }
        }
        private void txtmoney_TextChanged(object sender, EventArgs e)
        {
            if (price == "")                                   //判断金额为空
            {
                lbl0.Text = "0";
            }
            else
            {
                if (txtmoney.Text == "")
                {
                    txtmoney.Text = "0";
                    lbl0.Text = "0";
                }
                else
                { 
                    //如果都有值，计算出应该支付给顾客的余额
                    lbl0.Text=Convert.ToDecimal(Convert.ToDouble(txtmoney.Text.Trim())-Convert.ToDouble(price)*Convert.ToDouble(0.95)-Convert.ToDouble(bjf)).ToString("C");

                }
            }
        }
        private void btnJZ_Click(object sender, EventArgs e)
        {
            if (txtmoney.Text == "" || lbl0.Text == "0")
            {
                MessageBox.Show("请先结账");
                return;
            }
            else
            {
                if (lbl0.Text.Substring(1, 1) == "-")                  //判断支付的金额是否大于消费金额
                {
                    MessageBox.Show("金额不足");
                    return;
                }
                else
                {
                    MySqlConnection conn = BaseClass.DBConn.DxCon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("delete from tb_guestfood where zhuotai='" + Rname +"'",conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand("update tb_room set RoomZT='待用'，Num=0,Waitername='' where RoomName='" + Rname + "'",conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    this.Close();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
