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
    public partial class frmSerch : Form
    {
        public frmSerch()
        {
            InitializeComponent();
        }
        public string RName;
        private void frmSerch_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            MySqlDataAdapter msda = new MySqlDataAdapter("select foodname,foodsum,foodallprice,waitername,beizhu,zhuotai,datatime from tb_guestfood where zhuotai='" + RName + "'order by ID desc", conn);
            DataSet ds = new DataSet();
            msda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
