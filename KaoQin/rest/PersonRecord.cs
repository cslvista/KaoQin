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
    public partial class PersonRecord : Form
    {
        DataTable Department = new DataTable();
        DataTable Staff = new DataTable();
        public PersonRecord()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void PersonRecord_Load(object sender, EventArgs e)
        {
            SearchDep();
        }

        private void SearchDep()
        {            
           try
            {
                string sql = "select BMID,BMMC,BMLB from KQ_BM where BMID>0";
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                gridDep.DataSource = Department;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }

        private void gridDep_Click(object sender, EventArgs e)
        {
            try
            {
                string sql =string.Format("select KQID,YGXM from KQ_YG where BMID='{0}'", gridViewDep.GetFocusedRowCellValue("BMID").ToString());
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                gridStaff.DataSource = Staff;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }

        private void gridStaff_Click(object sender, EventArgs e)
        {

        }
    }
}
