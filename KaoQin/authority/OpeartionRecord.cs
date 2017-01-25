using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.authority
{
    public partial class OpeartionRecord : Form
    {
        DataTable Record = new DataTable();
        public OpeartionRecord()
        {
            InitializeComponent();
        }

        private void OpeartionRecord_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "搜索操作记录";
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit1.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-01");
            dateEdit2.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");

            string sql = "select top 1000 * from KQ_LOG";
            
            try
            {
                Record = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Record;
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }

            SearchInfo();
        }

        private void SearchInfo()
        {
            if (Record.Rows.Count>0)
            {
                try
                {
                    string StopTime = Convert.ToDateTime(dateEdit2.Text).AddDays(1).ToString("yyyy-MM-dd");
                    Record.DefaultView.RowFilter = string.Format("Time>='{0}' and Time<='{1}' and  ( Record like '%{2}%')", dateEdit1.Text, StopTime, searchControl1.Text);
                }
                catch { }
                
            }

        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }

        private void dateEdit1_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }

        private void dateEdit2_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            OpeartionDetails form = new OpeartionDetails();
            form.Details = gridView1.GetFocusedRowCellDisplayText("Details").ToString();
            form.Show();
        }
    }
}
