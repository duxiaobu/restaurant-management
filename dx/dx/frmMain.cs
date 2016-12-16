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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public MySqlDataReader msdr;
        public string power;
        public string Names;
        public string Times;
        private void frmMain_Load(object sender, EventArgs e)
        {
           // toolStripStatusLabel13.Text = "超级管理员";

            switch (power)
            {
                case "0": toolStripStatusLabel13.Text = "超级管理员"; break;
                case "1": toolStripStatusLabel13.Text = "经理"; break;
                case "2": toolStripStatusLabel13.Text="一班用户";break;
            }
            toolStripStatusLabel10.Text = Names;             //在状态栏中显示登录用户身份
            toolStripStatusLabel16.Text = Times;             //将登录时间显示在状态栏中
            if (power == "2")
            {
                系统维护SToolStripMenuItem.Enabled = false;
                基础信息MToolStripMenuItem.Enabled = false;
            }
            if (power == "1")
            {
                系统维护SToolStripMenuItem.Enabled = false;
            }
        }

        private void frmMain_Activated(object sender, EventArgs e)                //将该窗体设为当前活动窗体时候的操作，从数据库中检索所有桌台信息
        {
            lvDesk.Items.Clear();                                            //清空ListView控件
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tb_room",conn);
            msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                string zt = msdr["RoomZT"].ToString().Trim();                   //取出每个桌台当前的状态
                AddItems(zt);                                             //添加桌台
            }
        }

        private void AddItems(string rzt)
        {
            if (rzt == "使用")                       //如果状态是“使用”，则添加索引为1的图
            {
                lvDesk.LargeImageList = imageList1;
                lvDesk.Items.Add(msdr["RoomName"].ToString(),1);
            }
            else                                    //否则添加索引为0的图
            {
                lvDesk.LargeImageList = imageList1;
                lvDesk.Items.Add(msdr["RoomName"].ToString(), 0);
            }
        }

        private void 桌台信息ToolStripMenuIteml_Click(object sender, EventArgs e)
        {
            frmDesk desk = new frmDesk();
            desk.ShowDialog();
        }
        private void 职员信息ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmUser user = new frmUser();
            user.ShowDialog();
        }
        private void 日历ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmCalender calender = new frmCalender();
            calender.ShowDialog();
        }
        private void 记事本ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }
        private void 计算器ToolStripMuneItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }
        private void 权限管理ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmQxGl qx = new frmQxGl();
            qx.ShowDialog();
        }

        private void 口令设置ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmPwd pwd = new frmPwd();
            pwd.names = Names;
            pwd.ShowDialog();
        }
        private void 锁定系统ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmLock locksystem = new frmLock();
            locksystem.Owner = this;
            locksystem.ShowDialog();
        }
        
        private void 退出系统ToolStripMenuItem1_Click(object sender, EventArgs e)
        { 
            if(MessageBox.Show("确定退出本系统吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void lvDesk_Click(object sender, EventArgs e)
        {
            string names = lvDesk.SelectedItems[0].SubItems[0].Text;
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tb_room where RoomName='" + names + "'",conn);
            MySqlDataReader msdr = cmd.ExecuteReader();
            msdr.Read();
            string zt = msdr["RoomZT"].ToString().Trim();
            msdr.Close();
            if (zt == "使用")
            {
                this.contextMenuStrip1.Items[0].Enabled = false;
                this.contextMenuStrip1.Items[1].Enabled = true;
                this.contextMenuStrip1.Items[3].Enabled = true;
                this.contextMenuStrip1.Items[5].Enabled = true;
                this.contextMenuStrip1.Items[6].Enabled = true;
            }
            if (zt == "待用")
            {
                this.contextMenuStrip1.Items[0].Enabled = true;
                this.contextMenuStrip1.Items[1].Enabled = false;
                this.contextMenuStrip1.Items[3].Enabled = false;
                this.contextMenuStrip1.Items[5].Enabled = false;
                this.contextMenuStrip1.Items[6].Enabled = false;
            }
            conn.Close();
        }
        private void 开台ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvDesk.SelectedItems.Count != 0)
            {
                string names = lvDesk.SelectedItems[0].SubItems[0].Text;  //lvDesk选中的第一项的第一个子项的文本值
                frmOpen openroom = new frmOpen();
                openroom.name = names;
                openroom.ShowDialog();    //窗体显示为模式窗体，知道对话框关闭后，才执行此方法后面的代码
            }
            else
            {
                MessageBox.Show("请选择桌台");
            }
        }
        private void 取消开台toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvDesk.SelectedItems.Count != 0)               //判断是否有选中项
            {
                string names = lvDesk.SelectedItems[0].SubItems[0].Text;
                MySqlConnection conn = BaseClass.DBConn.DxCon();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("update tb_room set RoomZT='待用',Num=0 where RoomName='" + names + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                frmMain_Activated(sender, e);
            }
            else
            {
                MessageBox.Show("请选择桌台");
            }
        }
        private void 点菜ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvDesk.SelectedItems.Count != 0)
            {
                string names = lvDesk.SelectedItems[0].SubItems[0].Text;
                frmDC dc = new frmDC();
                dc.RName = names;
                dc.ShowDialog();
            }
            else
            {
                MessageBox.Show("请选择桌台");
            }
        }
        private void 消费查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvDesk.SelectedItems.Count != 0)
            {
                string names = lvDesk.SelectedItems[0].SubItems[0].Text;
                frmSerch serch = new frmSerch();
                serch.RName = names;
                serch.ShowDialog();
            }
            else
            {
                MessageBox.Show("请选择桌台");
            }
        }
        private void 结账ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvDesk.SelectedItems.Count != 0)
            {
                string names = lvDesk.SelectedItems[0].SubItems[0].Text;
                frmJZ jz = new frmJZ();
                jz.Rname = names;
                jz.ShowDialog();
            }
            else
            {
                MessageBox.Show("请选择桌台");
            }
        }
        private void lvDesk_DoubleClick(object sender, EventArgs e)
        {
            frmDetails details = new frmDetails();
            details.TableName = lvDesk.SelectedItems[0].SubItems[0].Text;
            details.ShowDialog();
        }

        private void lvDesk_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 菜品信息ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmCpInfo cp = new frmCpInfo();
            cp.ShowDialog();
        }

        private void 菜品记录toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmCPJL order = new frmCPJL();
            order.ShowDialog();
        }
    }
}
