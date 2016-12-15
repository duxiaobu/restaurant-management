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
using  System.Diagnostics;

namespace dx
{
    public partial class frmBF : Form
    {
        public frmBF()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmBF_Load(object sender, EventArgs e)
        {

        }
        public String bakpath = "d:\\db_bak\\";
        public String appDirecroty = @"D:\mysql\mysql-5.7.16-winx64\bin";
        public String uname = "root";
        public String upass = "dx050609";
        public String dbname = "chuanhaozi"; 
        
        public static void StartCmd(String workingDirectory,String command)
        {
            Process p = new Process();  
            p.StartInfo.FileName = "cmd.exe";  
            p.StartInfo.WorkingDirectory = workingDirectory;  
            p.StartInfo.UseShellExecute = false;  
            p.StartInfo.RedirectStandardInput = true;  
            p.StartInfo.RedirectStandardOutput = true;  
            p.StartInfo.RedirectStandardError = true;  
            p.StartInfo.CreateNoWindow = true;  
            p.Start();  
            p.StandardInput.WriteLine(command);  
            p.StandardInput.WriteLine("exit");  
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try  
            {  
                //String command = "mysqldump --quick --host=localhost --default-character-set=gb2312 --lock-tables --verbose  --force --port=端口号 --user=用户名 --password=密码 数据库名 -r 备份到的地址";  
                //构建执行的命令  
                StringBuilder sbcommand = new StringBuilder();  
  
                StringBuilder sbfileName = new StringBuilder();  
                sbfileName.AppendFormat("{0}", DateTime.Now.ToShortDateString()).Replace("-", "").Replace(":", "").Replace(" ", "").Replace("/", "");  
                String fileName = sbfileName.ToString();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.AddExtension = false;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.CheckPathExists = false;
                saveFileDialog.FileName = fileName;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    String directory = bakpath + fileName + ".bak";
                    sbcommand.AppendFormat("mysqldump --quick --host=localhost --default-character-set=utf8 --lock-tables --verbose  --force --port=3306 --user={0} --password={1} {2} -r \"{3}\"", uname, upass, dbname, directory);
                    String command = sbcommand.ToString();

                    //获取mysqldump.exe所在路径  
                    String appDirecroty = System.Windows.Forms.Application.StartupPath + "\\";
                    StartCmd(appDirecroty, command);

                    MessageBox.Show(@"数据库已成功备份到 " + directory + " 文件中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }  
            catch (Exception ex)  
            {  
                MessageBox.Show("数据库备份失败！");  
            }  
  
        }  
        }
    }

