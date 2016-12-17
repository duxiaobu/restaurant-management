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
    public partial class frmXFJL : Form
    {
        public frmXFJL()
        {
            InitializeComponent();
        }
        private void frmXFJL_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlDataAdapter msda = new MySqlDataAdapter("select guestname,datatime,money from tb_moneyinfo",conn);
            DataSet ds = new DataSet();
            msda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
