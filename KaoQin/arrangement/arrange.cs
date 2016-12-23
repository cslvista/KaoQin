using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.arrangement
{
    public partial class arrange : Form
    {
        DataTable Type = new DataTable();
        public arrange()
        {
            InitializeComponent();
        }

        private void arrange_Load(object sender, EventArgs e)
        {
            toolStripButtonRefresh_Click(null, null);
        }


        public void ButtonRefresh_Click(object sender, EventArgs e)
        {
            string sql = "select ID,BMLB from KQ_BMLB";

            try
            {
                Type = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Type;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            add_alter_Item form = new add_alter_Item();
            form.Show(this);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            add_alter_Item form = new add_alter_Item();
            form.Show(this);
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            add_alter_Type form = new add_alter_Type();
            form.Show(this);
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            try
            {
                add_alter_Type form = new add_alter_Type();
                form.alter = true;
                form.textBox1.Text = gridView1.GetFocusedRowCellDisplayText("BMLB").ToString();
                form.ID = gridView1.GetFocusedRowCellDisplayText("ID").ToString();
                form.Show(this);
            }
            catch { }
            
        }

        public void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            string sql = "select ID,BMLB from KQ_BMLB";

            try
            {
                Type = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Type;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string sql = "";


        }
    }
}
