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
    public partial class add_alter_Dep : Form
    {
        public bool alter = false;
        bool success = false;
        public string type = "";
        public string BMID = "";
        public add_alter_Dep()
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
                MessageBox.Show("请输入部门名称！");
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
                MainUsers form = (MainUsers)this.Owner;
                form.toolStripButtonRefresh_Click(null, null);
                this.Close();
            }
        }

        private bool Add()
        {

            string sql = "select max(BMID) from KQ_BM";

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

            string sql1 = string.Format("insert into KQ_BM (BMID,BMMC,BMLB) values ('{0}','{1}','{2}')", ID, textBox1.Text.Trim(),comboBox1.SelectedValue);

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

        private bool Alter()
        {
            //更新或插入数据
            string sql = string.Format("update KQ_BM set BMMC='{0}',BMLB='{1}' where BMID='{2}'", textBox1.Text.Trim(),comboBox1.SelectedValue,BMID);

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void AddDep_Load(object sender, EventArgs e)
        {
            string sql = "select ID,BMLB from KQ_BMLB where ID>0";

            DataTable Type = new DataTable();
            Type.Columns.Add("ID");
            Type.Columns.Add("BMLB");

            try
            {
                Type = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                comboBox1.DataSource = Type;
                comboBox1.DisplayMember = "BMLB";
                comboBox1.ValueMember = "ID";
                comboBox1.Text = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            if (alter == true)
            {
                this.Text = "修改部门";
                comboBox1.Text = type;
            }


        }
    }
}
