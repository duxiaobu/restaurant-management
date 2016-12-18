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
            TreeNode newnode1 = tvFood.Nodes.Add("羊肉汤锅");       //为控件添加节点
            TreeNode newnode2 = tvFood.Nodes.Add("老火锅");
            TreeNode newnode3 = tvFood.Nodes.Add("酒水");
            TreeNode newnode11 = newnode1.Nodes.Add("汤锅类");
            TreeNode newnode12 = newnode1.Nodes.Add("爆炒类");
            TreeNode newnode13 = newnode1.Nodes.Add("素菜类");
            TreeNode newnode14 = newnode1.Nodes.Add("小吃类");
            TreeNode newnode21 = newnode2.Nodes.Add("特色系列");
            TreeNode newnode22 = newnode2.Nodes.Add("荤菜系列");
            TreeNode newnode23 = newnode2.Nodes.Add("素菜系列");
            TreeNode newnode24 = newnode2.Nodes.Add("特色小吃");
            TreeNode newnode25 = newnode2.Nodes.Add("锅底");
            TreeNode newnode31 = newnode3.Nodes.Add("酒");
            TreeNode newnode32 = newnode3.Nodes.Add("饮料");
           
            MySqlConnection conn = BaseClass.DBConn.DxCon();    //连接数据库
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tb_food where foodty='汤锅类'",conn);
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode11.Nodes.Add(msdr[3].ToString().Trim());   
            }
            msdr.Close();
           
            cmd = new MySqlCommand("select * from tb_food where foodty='爆炒类'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode12.Nodes.Add(msdr[3].ToString().Trim());    
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='素菜类'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode13.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='小吃类'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode14.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='特色菜系列'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode21.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='荤菜系列'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode22.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='素菜系列'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode23.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='特色小吃'", conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode24.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='锅底'", conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode25.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='酒'", conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode31.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select * from tb_food where foodty='饮料'", conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                newnode32.Nodes.Add(msdr[3].ToString().Trim());
            }
            msdr.Close();

            cmd = new MySqlCommand("select WaiterName from tb_room where RoomName='" + RName + "'",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                cbWaiter.Items.Add(msdr["WaiterName"].ToString().Trim());   //显示服务员的名字
            }
            cbWaiter.SelectedIndex = 0;                                     //显示第一个数据
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
            if (foodname == "火锅" || foodname == "汤锅" || foodname == "汤锅类" || foodname == "爆炒类" || foodname == "素菜类" || foodname == "小吃类" || foodname == "特色系列" || foodname == "荤菜系列" || foodname == "素菜系列" || foodname == "特色小吃" || foodname == "锅底")
            { }
            else
            {
                MySqlConnection conn = BaseClass.DBConn.DxCon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tb_food where foodname='" + foodname + "'", conn);
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
            if (txtpnum.Text != "")
            //根据数量计算出消费商品的价格
            {
                txtallprice.Text = Convert.ToString(Convert.ToInt32(txtpnum.Text) * Convert.ToInt32(txtprice.Text));
            }
        }
        private void GetData()         //该方法用于显示所有点菜信息
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();                                         //mysqldataadapter数据适配器，为处理脱机处理数据而设计的，在内部还是调用mysqldatareader
            MySqlDataAdapter msdr = new MySqlDataAdapter("select foodname,foodsum,foodallprice,waitername,beizhu,zhuotai,datatime from tb_guestfood where zhuotai='" + RName + "'order by ID desc",conn );
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
            frmMain main = new frmMain();
            
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
