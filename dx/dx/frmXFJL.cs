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
using Microsoft.Office.Interop.Excel;

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

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = BaseClass.DBConn.DxCon();
            conn.Open();
            MySqlDataAdapter msda = new MySqlDataAdapter("select * from tb_moneyinfo",conn);
            DataSet ds1 = new DataSet();
            msda.Fill(ds1);
            ExportExcel(ds1,@"F:\CData\xiaofei.xls");
        }
        private void ExportExcel(DataSet ds, string saveFileName)
        { 
             try
            {
                if (ds == null)
                {
                    MessageBox.Show ("数据库为空");
                }
                bool fileSaved = false;
                Microsoft.Office.Interop.Excel.Application elApp = new Microsoft.Office.Interop.Excel.Application();
                if(elApp==null)
                {
                    MessageBox.Show ("无法创建Excel对象，您的电脑未安装Excel");
                }
                Microsoft.Office.Interop.Excel.Workbooks workbooks=elApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook=workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet=(Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                //写入字段名称
                for(int i=0;i<ds.Tables[0].Columns.Count;i++)
                {
                    worksheet.Cells[1,i+1]=ds.Tables[0].Columns[i].ColumnName;
                }
                //写入数据
                for(int r=0;r<ds.Tables[0].Rows.Count;r++)
                {
                    for(int i=0;i<ds.Tables[0].Columns.Count;i++)
                    {
                        worksheet.Cells[r+2,i+1]=ds.Tables[0].Rows[r][i];
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                worksheet.Columns.EntireColumn.AutoFit();            //列宽自适应
                if(saveFileName!="")
                {
                    try
                    {
                        workbook.Saved=true;
                        workbook.SaveCopyAs(saveFileName);
                        fileSaved=true;
                    }
                    catch(Exception ex)
                    {
                        fileSaved=false;
                        MessageBox.Show("导出文件出错，文件可能正在被打开！\n" + ex.Message);
                    }
                }
                else
                {
                    fileSaved=false;
                }
                elApp.Quit();
                GC.Collect();  //强制销毁
                if (fileSaved && System.IO.File.Exists(saveFileName))
                {
                    System.Diagnostics.Process.Start(saveFileName);
                    MessageBox.Show("成功保存到Excel");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show (ex.ToString());
            }
        }
        
    }
}
