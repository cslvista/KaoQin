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
    
    public partial class add_alter_Type : Form
    {
        public bool alter = false;
        public string ID = "";
        bool success = false;
        public add_alter_Type()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("类别名称不得为空！");
                return;
            }

            if (alter == true)
            {
                success = Alter();
            }
            else
            {
                success = Add();
            }

            //更新主界面
            if (success == true)
            {
                arrange form = (arrange)this.Owner;
                form.toolStripButtonRefresh_Click(null,null);
                this.Close();
            }      

        }

        private bool Alter()
        {

            string sql = string.Format("update KQ_BMLB set BMLB='{0}' where ID='{1}'", textBox1.Text.Trim(), ID);

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return false;
            }
        }

        private bool Add()
        {
            string sql = "select max(ID) from KQ_BMLB";

            DataTable Max_ID = new DataTable();
            string ID = "";
            try
            {
                Max_ID = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                if (Max_ID.Rows[0][0].ToString() == "")
                {
                    ID = "1";
                }
                else
                {
                    ID = (Convert.ToInt32(Max_ID.Rows[0][0].ToString()) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return false;
            }

            string sql1 = string.Format("insert into KQ_BMLB (ID,BMLB) values ('{0}','{1}')", ID, textBox1.Text.Trim());

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql1);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message, "提示");
                return false;
            }
        }

        private void add_alter_Class_Load(object sender, EventArgs e)
        {
            if (alter == true)
            {
                this.Text = "修改类别";
            }
        }
    }
}
