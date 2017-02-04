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
        DataTable Record_DKJ = new DataTable();
        DataTable Record = new DataTable();
        public bool Extract = false;
        public SaveToDB()
        {
            InitializeComponent();
        }

        private void SaveToDB_Load(object sender, EventArgs e)
        {
            ButtonRefresh_Click(sender, e);
        }

        public void ButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from KQ_JL";
                Record = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                if (Record.Columns.Contains("Check")==false)
                {
                    Record.Columns.Add("Check", typeof(bool));
                }              
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
                           + string.Format("delete from KQ_JL_XB where ZBID='{0}'", gridView1.GetFocusedRowCellValue("ID").ToString());
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }

            ButtonRefresh_Click(sender, e);
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
                if (Record.Rows[i]["Check"].ToString() == "True")
                {
                    try
                    {                        
                        sql.Append(string.Format("select ID,KQSJ,LY from KQ_JL_XB where ZBID='{0}';", Record.Rows[i]["ID"].ToString()));
                        Record_DKJ = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("错误1:" + ex.Message, "提示");
                        return;
                    }
                }
            }

            Record_DKJ.Columns["KQSJ"].ColumnName = "Time";
            Record_DKJ.Columns["LY"].ColumnName = "Source";

            Attendance form = (Attendance)this.Owner;
            form.Record_DKJ = Record_DKJ.Copy();
            form.ButtonCal.Enabled = true;
            form.ButtonOrignData.Enabled = true;
            this.Close();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
