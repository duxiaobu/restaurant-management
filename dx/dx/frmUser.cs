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
    public partial class frmUser : Form
    {
        public frmUser()
        {
            InitializeComponent();
        }
        //绑定数据，在dataGridView中显示数据
        private void BindData()
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            MySqlDataAdapter sda = new MySqlDataAdapter("select WaiterName,CardNum,WaiterNum,Sex,Age,Tel,ID from tb_waiter order by ID desc", conn);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //页面加载时，性别显示为第一个
        private void frmUser_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
        //调用BindData函数，显示数据
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
        }
        //点击数据会显示在textbox中
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtname.Text = dataGridView1.SelectedCells[0].Value.ToString();
            txtcard.Text = dataGridView1.SelectedCells[1].Value.ToString();
            txtbh.Text = dataGridView1.SelectedCells[2].Value.ToString();
            comboBox1.SelectedItem = dataGridView1.SelectedCells[3].Value.ToString().Trim();
            txtage.Text = dataGridView1.SelectedCells[4].Value.ToString();
            txttel.Text = dataGridView1.SelectedCells[5].Value.ToString();
            btnModify.Enabled = true;
            btnDelete.Enabled = true;
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtname.Text = "";
            txtcard.Text = "";
            txtbh.Text = "";
            txttel.Text = "";
            txtage.Text = "";
            txtname.Enabled = true;
            txtcard.Enabled = true;
            txtbh.Enabled = true;
            comboBox1.Enabled = true;
            txtage.Enabled = true;
            txttel.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnModify.Enabled = false;
        }
        private void btnModify_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            txtname.Enabled = false;
            txtcard.Enabled = true;
            txtbh.Enabled = true;
            this.comboBox1.Enabled = true;
            txtage.Enabled = true;
            txttel.Enabled = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(*) from tb_waiter where WaiterName='" + txtname.Text + "'", conn);  //查询保存前数据库中是否有数据
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)
            {
                cmd = new MySqlCommand("update tb_waiter set WaiterName='" + txtname.Text + "',CardNum='" + txtcard.Text + "',WaiterNum='" + txtbh.Text + "',Sex='" + comboBox1.SelectedItem.ToString() + "',Age='" + txtage.Text + "',Tel='" + txttel.Text + "' where ID='" + dataGridView1.SelectedCells[6].Value.ToString() + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                BindData();
                btnReset.Enabled = true;
                btnModify.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnQuery.Enabled = true;
                btnDelete.Enabled = false;
                btnQuit.Enabled = true;
                txtname.Enabled = false;
            }
            else
            {
                cmd = new MySqlCommand("insert into tb_waiter(WaiterName,CardNum,WaiterNum,Sex,Age,Tel) values('" + txtname.Text + "','" + txtcard.Text + "','" + txtbh.Text + "','" + comboBox1.SelectedItem.ToString() + "','" + txtage.Text + "','" + txttel.Text + "')", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                BindData();
                btnReset.Enabled = true;
                btnModify.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnQuery.Enabled = true;
                btnDelete.Enabled = false;
                btnQuit.Enabled = true;
                txtname.Enabled = false;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = true;
            btnModify.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            txtname.Enabled = false;
            txtcard.Enabled = false;
            txtbh.Enabled = false;
            this.comboBox1.Enabled = false;
            txtage.Enabled = false;
            txttel.Enabled = false;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("delete from tb_waiter where ID='" + dataGridView1.SelectedCells[6].Value.ToString() + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            BindData();
        }
    }
}
