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
            SearchType();
        }

        private void SearchType()
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

        public void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {

        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {

        }
    }
}
