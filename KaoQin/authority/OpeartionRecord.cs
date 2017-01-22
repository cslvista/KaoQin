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
            searchControl1.Properties.NullValuePrompt = " ";
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";

            string sql = "select * from KQ_LOG";
            
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
        }
    }
}
