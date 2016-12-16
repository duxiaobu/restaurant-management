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
        public string guestName;
        private void frmJZ_Load(object sender, EventArgs e)
        {
            this.Text = Rname + "结账";
            groupBox1.Text = "当前桌台-" + Rname;
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            MySqlDataAdapter sda = new MySqlDataAdapter("select foodname,foodsum,foodallprice,waitername,beizhu,zhuotai,datatime from tb_guestfood where zhuotai='" + Rname + "'order by ID desc", conn);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            dgvRecord.DataSource = ds.Tables[0];
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select sum(foodallprice) from tb_GuestFood where zhuotai='" + Rname + "'", conn);
            price = Convert.ToString(cmd.ExecuteScalar());
            if (price == "")
            {
                txtallprice.Text = "0";
                btnJZ.Enabled = false;
            }
            else
            {
                txtallprice.Text = price;
                btnJZ.Enabled = true;
                conn.Close();
            }
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
                txtallprice.Text = "0";
            }
            else
            {
                if (txtmoney.Text == "")
                {
                    
                    txtzl.Text = "0";
                    btnJZ.Enabled = false;
                }
                else
                { 
                    //如果都有值，计算出应该支付给顾客的余额
                    txtzl.Text=Convert.ToString(Convert.ToDouble(txtmoney.Text.Trim())-Convert.ToDouble(price)*Convert.ToDouble(txtzk.Text.Trim()));

                }
            }
        }
        private void btnJZ_Click(object sender, EventArgs e)
        {
            if (txtmoney.Text == "" || txtallprice.Text == "0")
            {
                MessageBox.Show("请先结账");
                return;
            }
            else
            {
                if (txtzl.Text.Substring(0, 1) == "-")                  //判断支付的金额是否大于消费金额
                {
                    MessageBox.Show("金额不足");
                    return;
                }
                else
                {
                    MySqlConnection conn = BaseClass.DBConn.DxCon();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("select count(*) from tb_cpinfo where foodname='" + dgvRecord.SelectedCells[0].Value.ToString() + "'",conn);
                    int i = Convert.ToInt32(cmd.ExecuteScalar());
                    if(i>0)
                    {
                        cmd=new MySqlCommand("select foodname,foodnum from tb_cpinfo where foodname='" + dgvRecord.SelectedCells[0].Value.ToString() + "'",conn);
                        MySqlDataReader msdr=cmd.ExecuteReader();
                        msdr.Read();
                        string name = msdr["foodname"].ToString().Trim();
                        int beforenumber = Convert.ToInt32(msdr["foodnum"].ToString().Trim());
                        string afternumber = Convert.ToString(beforenumber + Convert.ToInt32(dgvRecord.SelectedCells[1].Value.ToString()));
                        msdr.Close();
                        cmd = new MySqlCommand("update tb_cpinfo set foodnum='" + afternumber + "' where foodname='" + name + "'", conn);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new MySqlCommand("insert into tb_cpinfo(foodname,foodnum) values('" + dgvRecord.SelectedCells[0].Value.ToString() + "','" + dgvRecord.SelectedCells[1].Value.ToString() + "')", conn);
                        cmd.ExecuteNonQuery();
                    }
                    cmd = new MySqlCommand("delete from tb_guestfood where zhuotai='" + Rname +"'",conn);
                    cmd.ExecuteNonQuery();
                    string allPrice = Convert.ToString(Convert.ToDouble(price) * Convert.ToDouble(txtzk.Text.Trim()));
                    cmd = new MySqlCommand("select GuestName from tb_room where RoomName='" + Rname + "'",conn);
                    MySqlDataReader msdr1 = cmd.ExecuteReader();
                    msdr1.Read();
                    guestName = msdr1[7].ToString().Trim();
                    msdr1.Close();
                    cmd = new MySqlCommand("insert into tb_moneyinfo(guestname,datatime,money) values('" + guestName +"','" + DateTime.Now.ToString() + "','" + allPrice + "')",conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand("update tb_room set RoomZT='待用',Num=0,WaiterName=''，RoomBZ='',GuestName='',WaiterName='' where RoomName='" + Rname + "'",conn);
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

        private void txtzk_TextChanged(object sender, EventArgs e)
        {
            if (price == "")
            {
                MessageBox.Show("请输入消费总价");
            }
            else
            {
                if (txtmoney.Text == "")
                {
                    MessageBox.Show("请输入收银");
                }
                else
                {
                    if (txtzk.Text != "" && txtzk.Text != "0")
                    {
                        txtzl.Text = Convert.ToString(Convert.ToDouble(txtmoney.Text.Trim()) - Convert.ToDouble(price) * Convert.ToDouble(txtzk.Text.Trim()));
                    }
                }
            }
        }
    }
}
