using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class Schedual : Form
    {
        DataTable Department = new DataTable();
        public Schedual()
        {
            InitializeComponent();
        }

        private void Schedual_Load(object sender, EventArgs e)
        {
            string sql = "select * from KQ_YG";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
