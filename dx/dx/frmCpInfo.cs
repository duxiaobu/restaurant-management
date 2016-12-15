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
    public partial class frmCpInfo : Form
    {
        public frmCpInfo()
        {
            InitializeComponent();
        }
        private void BindData()
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            MySqlDataAdapter msda = new MySqlDataAdapter("select foodty,foodnum,foodname,foodprice,ID from tb_food order by ID ASC", conn);
            DataSet ds = new DataSet();
            msda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void frmCpInfo_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(*) from tb_food where foodname='" + txtname.Text + "'",conn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)
            {
                MessageBox.Show("已有该菜品");
            }
            else
            {
                cmd = new MySqlCommand("insert into tb_food(foodty,foodnum,foodname,foodprice) values('" + comboBox1.SelectedItem.ToString() +"','" + txtnum.Text + "','" + txtname.Text + "','" + txtprice.Text +"')",conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                BindData();
                MessageBox.Show("添加成功");
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("update tb_food set foodty='" + comboBox1.SelectedItem.ToString() + "',foodname='" + txtname.Text + "',foodprice='" + txtprice.Text + "' where ID='" + dataGridView1.SelectedCells[4].Value.ToString() + "'",conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            BindData();
            MessageBox.Show("修改成功");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.SelectedItem = dataGridView1.SelectedCells[0].Value.ToString().Trim();
            txtname.Text = dataGridView1.SelectedCells[1].Value.ToString();
            txtnum.Text = dataGridView1.SelectedCells[2].Value.ToString();
            txtprice.Text = dataGridView1.SelectedCells[3].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("delete from tb_food where ID='" + dataGridView1.SelectedCells[4].Value.ToString() + "'",conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            BindData();
            MessageBox.Show("删除成功");
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !char.IsDigit(e.KeyChar) && e.KeyChar != 13)
            {
                MessageBox.Show("请输入数字");
                e.Handled = true;
            }
        }
    }
}
