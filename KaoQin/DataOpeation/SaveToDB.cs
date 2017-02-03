using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.DataOpeation
{
    public partial class SaveToDB : Form
    {
        public DataTable Record_DKJ = new DataTable();
        DataTable Record = new DataTable();
        public bool Extract = false;
        public SaveToDB()
        {
            InitializeComponent();
        }

        private void SaveToDB_Load(object sender, EventArgs e)
        {
            
            Record.Columns.Add("ID", typeof(string));
            Record.Columns.Add("Check", typeof(bool));
            Record.Columns.Add("KSSJ", typeof(string));
            Record.Columns.Add("JSSJ", typeof(string));
            Record.Columns.Add("JLTS", typeof(string));
            Record.Columns.Add("BCR", typeof(string));
            Record.Columns.Add("BCSJ", typeof(string));
            ButtonRefresh_Click(sender, e);

            if (Record_DKJ.Rows.Count == 0)
            {
                ButtonAdd.Enabled = false;
            }

            if (Extract == true)
            {
                ButtonAdd.Visible = false;
                ButtonDelete.Visible = false;
                ButtonRefresh.Visible = false;
                ButtonImport.Location = new Point(ButtonAdd.Location.X, ButtonAdd.Location.Y);
            }else
            {
                ButtonImport.Visible = false;
            }
        }

        public void ButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from KQ_JL";
                Record = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Record;
                gridView1.BestFitColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (Record.Rows.Count == 0)
            {
                return;
            }

            try
            {
                string sql = string.Format("delete from KQ_JL where ID='{0}'",gridView1.GetFocusedRowCellValue("ID").ToString())
                           + string.Format("delete from KQ_JL_XB where ID='{0}'", gridView1.GetFocusedRowCellValue("ID").ToString());
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Add_DB form = new Add_DB();
            form.Show(this);
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            if (Record.Rows.Count == 0)
            {
                return;
            }

            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < Record.Rows.Count; i++)
            {
                if (Record.Rows[i]["Check"].ToString() == "0")
                {
                    try
                    {                        
                        sql.Append(string.Format("select * from KQ_JL_XB where ID='{0}'", Record.Rows[i]["ID"].ToString()));                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("错误1:" + ex.Message, "提示");
                    }
                }
            }

            Record_DKJ = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
            ImportData form = new ImportData();
            
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
