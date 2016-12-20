using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.users
{
    public partial class MainUsers : Form
    {
        DataTable Staff = new DataTable();
        DataTable Department = new DataTable();
        public MainUsers()
        {
            InitializeComponent();
        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SearchStaff();
        }

        private void SearchStaff()
        {
            string sql = "select ";
           
            try
            {
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl2.DataSource = Staff;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ;
            }
        }

        private void MainUsers_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入考勤号或姓名";
        }
    }
}
