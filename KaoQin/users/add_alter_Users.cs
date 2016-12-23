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
    public partial class add_alter_Users : Form
    {
        public bool alter = false;
        bool success = false;
        public add_alter_Users()
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
            if (success == true && alter==false)
            {
                textBox2.Text = "";
                textBox3.Text = "";
                dateEdit1.Text = "";
                textBox1.Text = "";
                MessageBox.Show("更新成功！");
            }else if (success == true && alter == true)
            {
                MainUsers form = (MainUsers)this.Owner;
                this.Close();
            }

        }

        private bool Add()
        {

            //检查编号是否有误
            string sql = string.Format("select HBFL from DM_BB_LX where HBFL='{0}'", textBox1.Text.Trim());

            DataTable isExist = new DataTable();
            try
            {
                isExist = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (isExist.Rows.Count > 0)
            {
                MessageBox.Show("该编号已存在，请更换其他编号！");
                return false;
            }

            string sql1 = string.Format("insert into DM_BB_LX (HBFL,MC) values ('{0}','{1}')", textBox1.Text.Trim());

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql1);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool Alter()
        {
            //更新或插入数据
            string sql = string.Format("update DM_BB_LX set MC='{0}' where HBFL='{1}'", textBox1.Text.Trim());

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void AddUsers_Load(object sender, EventArgs e)
        {
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";

            if (alter == false)
            {
                comboBox1.Text = "在职";
                comboBox1.Enabled = false;
            }
                 
        }
    }
}
