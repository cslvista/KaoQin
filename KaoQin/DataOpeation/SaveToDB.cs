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
        DataTable Record_DKJ_Copy = new DataTable();
        DataTable Record = new DataTable();
        public bool Extract = false;
        public bool Authority_Attendance_DelDB = false;
        public SaveToDB()
        {
            InitializeComponent();
        }

        private void SaveToDB_Load(object sender, EventArgs e)
        {
            Record_DKJ.Columns.Add("ID", typeof(string));
            Record_DKJ.Columns.Add("Time", typeof(string));
            Record_DKJ.Columns.Add("Source", typeof(string));
            ButtonRefresh_Click(sender, e);
        }

        public void ButtonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from KQ_JL order by ID desc";
                Record = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
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
            if (Authority_Attendance_DelDB == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

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
                        sql.Append(string.Format("delete from KQ_JL where ID='{0}'", Record.Rows[i]["ID"].ToString())
                                 + string.Format("delete from KQ_JL_XB where ZBID='{0}'", Record.Rows[i]["ID"].ToString()));                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("错误1:" + ex.Message, "提示");
                        return;
                    }
                }
            }

            if (sql.ToString().Length == 0)
            {
                MessageBox.Show("没有选择要删除的内容！");
                return;
            }

            if (MessageBox.Show(string.Format("是否确定删除？"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql.ToString());            
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
                    sql.Clear();
                    sql.Append(string.Format("select ID,KQSJ,LY from KQ_JL_XB where ZBID='{0}';", Record.Rows[i]["ID"].ToString()));
                    try
                    {
                        Record_DKJ_Copy.Merge(GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql.ToString()));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("错误1:" + ex.Message, "提示");
                        return;
                    }
                }
            }

            for (int i = 0; i < Record_DKJ_Copy.Rows.Count; i++)
            {
                Record_DKJ.Rows.Add(new object[] { Record_DKJ_Copy.Rows[i]["ID"],Convert.ToDateTime(Record_DKJ_Copy.Rows[i]["KQSJ"]).ToString("yyyy-MM-dd HH:mm:ss") , Record_DKJ_Copy.Rows[i]["LY"] });
            }


            Attendance form = (Attendance)this.Owner;
            form.Record_DKJ.Merge(Record_DKJ);
            form.TimeSort();
            form.ButtonOrignData.Enabled = true;
            this.Close();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
