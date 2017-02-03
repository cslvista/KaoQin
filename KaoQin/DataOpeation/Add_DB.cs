using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.DataOpeation
{
    public partial class Add_DB : Form
    {
        public DataTable Record_DKJ = new DataTable();
        public Add_DB()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveToDB form = (SaveToDB)this.Owner;
            form.ButtonRefresh_Click(sender, e);
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                for (int i=0;i< Record_DKJ.Rows.Count; i++)
                {
                    sql.Append(string.Format("")
                             + string.Format(""));
                }
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }
        }

        private void Add_DB_Load(object sender, EventArgs e)
        {

        }
    }
}
