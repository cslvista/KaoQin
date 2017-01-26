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
    public partial class Authority : Form
    {
        DataTable users = new DataTable();
        public Authority()
        {
            InitializeComponent();
        }

        private void Authority_Load(object sender, EventArgs e)
        {
            ButtonRefresh_Click(null,null);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            add_alter_authority form = new add_alter_authority();
            form.Show();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            
            try
            {
                string sql = "select * from KQ_SQ";
                users = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OpeartionRecord form = new OpeartionRecord();
            form.Show();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            add_alter_authority form = new add_alter_authority();
            form.alter = true;
            form.ID = gridView1.GetFocusedRowCellValue("ID").ToString();
            form.Show();
        }
    }
}
