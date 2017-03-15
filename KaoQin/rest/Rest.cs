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
    public partial class Rest : Form
    {
        DataTable Department = new DataTable();
        public Rest()
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
            string sql = "select BMID,BMMC,BMLB from KQ_BM where BMID>0";

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
            Rules form = new Rules();
            form.Show(this);
        }
    }
}
