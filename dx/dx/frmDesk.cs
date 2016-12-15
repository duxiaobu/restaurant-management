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
    public partial class frmDesk : Form
    {
        public frmDesk()
        {
            InitializeComponent();
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()             //绑定数据，显示桌台信息
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();                   //desc为降序排列，ASC是升序排列
            MySqlDataAdapter msda = new MySqlDataAdapter("select RoomName,RoomBJF,RoomType,RoomBZ,ID from tb_room order by ID desc",conn);
            DataSet ds = new DataSet();
            msda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        //单击dataGridView把具体的信息显示在textBox中
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtname.Text = dataGridView1.SelectedCells[0].Value.ToString();
            txtbjf.Text = dataGridView1.SelectedCells[1].Value.ToString();
            txtlx.Text = dataGridView1.SelectedCells[2].Value.ToString();
            txtbz.Text = dataGridView1.SelectedCells[3].Value.ToString();
            btnModify.Enabled = true;
            btnDelete.Enabled = true;
        }
        //点击取消按钮，退出本窗口
        private void btnQuit_Click(object sender, EventArgs e)    
        {
            this.Close();
        }
        //根据桌台名称，删除相关信息
        private void btnDelete_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("delete from tb_room where RoomName='" + dataGridView1.SelectedCells[0].Value.ToString() + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            BindData();
        }
        //单击修改信息
        private void btnModify_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            txtbjf.Enabled = true;
            txtlx.Enabled = true;
            txtbz.Enabled = true;
        }
        //点击取消按钮
        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnReset.Enabled = true;
            btnModify.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            txtname.Enabled = false;
            txtbjf.Enabled = false;
            txtlx.Enabled = false;
            txtbz.Enabled = false;
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select count(*) from tb_room where RoomName='" + txtname.Text + "'", conn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)                              //判断保存前数据库中是否有数据
            {
                cmd = new MySqlCommand("update tb_room set RoomName='" + txtname.Text + "',RoomBJF='" + txtbjf.Text + "',RoomType='" + txtlx.Text + "',RoomBZ='" + txtbz.Text + "' where ID='" + dataGridView1.SelectedCells[4].Value.ToString() + "'", conn);
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
                cmd = new MySqlCommand("insert into tb_room(RoomName,RoomBJF,RoomType,RoomBZ) values('" + txtname.Text + "','" + txtbjf.Text + "','" + txtlx.Text + "','" + txtbz.Text + "')", conn);
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
        //单击重填按钮
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtname.Text = "";
            txtlx.Text = "";
            txtbz.Text = "";
            txtbjf.Text = "";
            txtname.Enabled = true;
            txtbjf.Enabled = true;
            txtlx.Enabled = true;
            txtbz.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnModify.Enabled = false;
        }
    }
}
