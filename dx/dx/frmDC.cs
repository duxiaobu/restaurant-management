﻿using System;
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
    public partial class frmDC : Form
    {
        public frmDC()
        {
            InitializeComponent();
        }
        public string RName;
        private void frmDC_Load(object sender, EventArgs e)
        {
            this.Text = RName + "点/加菜";                      //设置窗体显示问题
            TreeNode newnode1 = tvFood.Nodes.Add("火锅");       //为控件添加节点
            TreeNode newnode2 = tvFood.Nodes.Add("汤锅");
            MySqlConnection conn = BaseClass.DBConn.DxCon();    //连接数据库
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tb_type where footy='1'",conn);
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode1.Nodes.Add(msdr[2].ToString().Trim());   //为锅底添加子节点
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_type where foodty='2'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode2.Nodes.Add(msdr[2].ToString().Trim());    //为配菜添加子节点
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_waiter",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                cbWaiter.Items.Add(msdr["WaiterName"].ToString().Trim());   //显示服务员的名字
            }
            cbWaiter.SelectedIndex = 0;                   //显示第一个数据
            msdr.Close();

            cmd = new MySqlCommand("select RoomZT from tb_room where RoomName='"+RName+"'",conn);
            string zt = Convert.ToString(cmd.ExecuteScalar());         //获取桌台的状态,ExecuteScalar返回第一行第一列
            if (zt.Trim() == "待用")                                   //如果处在“待用”状态，则停止所有操作
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
            }
            conn.Close();                                             //关闭数据库连接
            GetData();                                                //重新绑定数据
            tvFood.ExpandAll();                                       //展开treeview控件
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)   //双击某个菜系将会显示该菜系的详细信息
        {
            string foodname = tvFood.SelectedNode.Text;               //获取选择的商品名称
            if (foodname == "锅底" || foodname == "配菜" || foodname == "烟酒" || foodname == "主食")
            { }
            else
            {
                MySqlConnection conn = BaseClass.DBConn.DxCon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tb_food where foodname='" + foodname + "'",conn);
                MySqlDataReader msdr = cmd.ExecuteReader();
                msdr.Read();
                txtNum.Text = msdr["foodnum"].ToString().Trim();         //读取商品的编号
                txtName.Text = foodname;                                            //显示商品的名称
                txtprice.Text = msdr["foodprice"].ToString().Trim();          //读取商品的单价
                conn.Close();                                            //关闭数据库连接

                if (txtpnum.Text == "")
                {
                    MessageBox.Show("数量不能为空");
                    return;
                }
                else
                {
                    //根据消费商品的数量计算出消费商品的价格，txtbox中只能传string类型，但计算必须转换成整型
                    txtallprice.Text = Convert.ToString(Convert.ToInt32(txtprice.Text) * Convert.ToInt32(txtpnum.Text));  
                }
            }
        }

        private void txtpnum_KeyPress(object sender,KeyPressEventArgs e)  //判断数量框中填入的是否是数字
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                MessageBox.Show("请输入数字！");
                e.Handled = true;
            }
        }
        private void txtpnum_TextChanged(object sender, EventArgs e)
        {
            if (txtpnum.Text == "")
            {
                MessageBox.Show("数量不能为空！");
                return;
            }
            else
            {
                if (Convert.ToInt32(txtpnum.Text) < 1)
                {
                    MessageBox.Show("不能为小于1的数字");
                    return;
                }
                else
                { 
                    //根据数量计算出消费商品的价格
                    txtallprice.Text = Convert.ToString(Convert.ToInt32(txtpnum.Text)*Convert.ToInt32(txtprice.Text));
                }
            }
        }
        private void GetData()         //该方法用于显示所有点菜信息
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();                                         //mysqldataadapter数据适配器，为处理脱机处理数据而设计的，在内部还是调用mysqldatareader
            MySqlDataAdapter msdr = new MySqlDataAdapter("select foodnum,foodallprice,waitername,beizhu,zhuotai,datatime from tb_guestfood where zhuotai='" + RName + "'order by ID desc",conn );
            DataSet ds = new DataSet();                //创建DataSet对象
            msdr.Fill(ds);                //调用fill方法会将查询结果保存进DataSet中，调用完后会自动关闭数据库，DataSset会创建一个新表DataTable,其包含查询所有的字段，其名称默认为Table,
            dgvFoods.DataSource = ds.Tables[0];   //将数据绑定到sgvFoods上，并完成显示
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtNum.Text == "" || txtprice.Text == "")
            {
                MessageBox.Show("请选择菜系");
                return;
            }
            else
            {
                if (txtpnum.Text == "")
                {
                    MessageBox.Show("数量不能为空");
                    return;
                }
                else
                {
                    if (Convert.ToInt32(txtpnum.Text) < 0)
                    {
                        MessageBox.Show("请输入消费数量");
                        return;
                    }
                    else
                    {
                        MySqlConnection conn = BaseClass.DBConn.DxCon();
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("insert into tb_guestfood(foodnum,foodname,foodsum,foodallprice,waitername,beizhu,zhuotai,datatime) values('" + txtNum.Text.Trim() + "','" + txtName.Text.Trim() + "','" + txtpnum.Text.Trim() + "','" + Convert.ToDecimal(txtallprice.Text.Trim()) + "','" + cbWaiter.SelectedItem.ToString() + "','" + txtbz.Text.Trim() + "','" + RName + "','" + DateTime.Now.ToString() + "')",conn);
                        cmd.ExecuteNonQuery();   //执行查询，返回影响的行数，为int型；
                        conn.Close();
                        GetData();               //调用GetData方法显示所有点菜信息
                    }
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvFoods.SelectedRows.Count > 0)                     //判断是否选中某条信息
            { 
                //获取删除信息名称
                string names = dgvFoods.SelectedCells[0].Value.ToString();
                MySqlConnection conn = BaseClass.DBConn.DxCon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("delete from tb_guestfood where foodname='" + names +"' and zhuotai='" + RName + "'",conn);
                cmd.ExecuteNonQuery();
                GetData();                            //删除后，通过GetData函数显示所有点菜的信息
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}