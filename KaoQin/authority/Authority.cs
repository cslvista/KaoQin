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
        public bool Authority_Mangement_Edit = false;
        public bool Authority_Mangement_Del = false;
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
            if (Authority_Mangement_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            add_alter_authority form = new add_alter_authority();
            form.Show(this);
        }

        public void ButtonRefresh_Click(object sender, EventArgs e)
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


        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            if (Authority_Mangement_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            add_alter_authority form = new add_alter_authority();
            form.alter = true;
            form.ID = gridView1.GetFocusedRowCellValue("ID").ToString();
            form.Name= gridView1.GetFocusedRowCellValue("Name").ToString();
            form.Show(this);
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            ButtonAlter_Click(sender, e);
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (Authority_Mangement_Del == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            try
            {
                string sql = string.Format("delete from KQ_SQ where ID='{0}'",gridView1.GetFocusedRowCellValue("ID").ToString());
                GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            ButtonRefresh_Click(null, null);
        }
    }
}
