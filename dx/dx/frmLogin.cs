using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace dx
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        //打开登录界面账号栏中就会聚焦
        private void Form1_Load(object sender, EventArgs e)
        {
            txtName.Focus();
        }
        private void txtPwd_KeyPress(object sender, KeyPressEventArgs e)   //Enter键实现登录按钮效果
        {
            if (e.KeyChar == 13)
            {
                btnSubmit_Click(sender, e);
            }
        }
        private void btnConcel_Click(object sender, EventArgs e)           //点击取消会弹出提示框
        {
            if (MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)  //MessageBoxIcon.Asterisk图标样式
            {
                Application.Exit();
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")                                   //判断用户名是否为空
            {
                MessageBox.Show("请输入用户名", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);  //警告框样式
            }
            else
            { 
                if(txtPwd.Text=="")                                   //判断密码是否为空
                {
                    MessageBox.Show("请输入密码","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else
                {
                    MySqlConnection conn = BaseClass.DBConn.DxCon();   //连接数据库                     
                    conn.Open();                                       //打开数据库
                    MySqlCommand cmd = new MySqlCommand("select * from tb_user where UserName='" + txtName.Text + "' and UserPwd='" + txtPwd.Text + "'",conn);                                         //输入sql语句
                    MySqlDataReader msdr = cmd.ExecuteReader();        //执行sql语句,将结果放入到msdr
                    msdr.Read();                                       //读取数据
                    if (msdr.HasRows)                                  //HasRows获取一个布尔值，指示msdr中是否包含一行或多行
                    {
                        msdr.Close();                                  //断开查询连接
                        cmd = new MySqlCommand("select * from tb_user where UserName='" + txtName.Text + "'", conn);
                        MySqlDataReader msdr1 = cmd.ExecuteReader();
                        msdr1.Read();
                        string UserPower = msdr1["power"].ToString().Trim();  //根据不同账号返回对应的用户权限
                        conn.Clone();                                  //关闭数据库连接
                        frmMain main = new frmMain();
                        main.power = UserPower;
                        main.Names = txtName.Text;
                        main.Times = DateTime.Now.ToShortDateString();
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误");
                    }
                }

            }
        }

    }
}
