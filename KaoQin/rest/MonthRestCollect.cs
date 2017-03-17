using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.rest
{
    public partial class MonthRestCollect : Form
    {
        DataTable Department = new DataTable();
        public MonthRestCollect()
        {
            InitializeComponent();
        }

        private void Rest_Load(object sender, EventArgs e)
        {
            SearchDepartment();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SearchDepartment();
        }

        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC from KQ_BM where BMID>0";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                gridDep.DataSource = Department;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }

        private void ButtonCal_Click(object sender, EventArgs e)
        {
            RestRules form = new RestRules();
            form.Show(this);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            MonthRecord form = new MonthRecord();
            form.Show(this);
        }

        private void gridMonthRecord_DoubleClick(object sender, EventArgs e)
        {
            ButtonAlter_Click(sender, e);
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
