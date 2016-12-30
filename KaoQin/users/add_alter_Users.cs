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
        public string department;
        bool success = false;
        public add_alter_Users()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            MainUsers form = (MainUsers)this.Owner;
            form.simpleButton3_Click(null, null);
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入考勤号！");
                return;
            }

            if (textBox3.Text == "")
            {
                MessageBox.Show("请输入姓名！");
                return;
            }

            if (comboBox2.Text == "")
            {
                MessageBox.Show("请选择部门！");
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
                MessageBox.Show("添加成功！");
            }else if (success == true && alter == true)
            {
                MainUsers form = (MainUsers)this.Owner;
                form.simpleButton3_Click(null, null);
                this.Close();
            }

        }

        private bool Add()
        {

            //检查考勤号是否有误
            string sql = string.Format("select KQID from KQ_YG where KQID='{0}'", textBox2.Text.Trim());

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
                MessageBox.Show("该考勤号已存在，请更换其他考勤号！");
                return false;
            }

            string sql1 = string.Format("insert into KQ_YG (KQID,YGXM,BMID,RZSJ,ZT,SM) values ('{0}','{1}','{2}','{3}','{4}','{5}')", textBox2.Text.Trim(), textBox3.Text.Trim(), comboBox2.SelectedValue, Convert.ToDateTime(dateEdit1.Text).ToString("yyyy-MM-dd"),"0", textBox1.Text.Trim());

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
            string sql = string.Format("update KQ_YG set KQID='{0}',YGXM='{1}',RZSJ='{2}',LZSJ='{3}',ZT='{4}',SM='{5}' where KQID='{6}'", textBox1.Text.Trim());

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
            UILocation();

            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";

            string sql = "select BMID,BMMC from KQ_BM where BMID>0";

            DataTable Department = new DataTable();
            Department.Columns.Add("BMID");
            Department.Columns.Add("BMMC");

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                comboBox2.DataSource = Department;
                comboBox2.DisplayMember = "BMMC";
                comboBox2.ValueMember = "BMID";
                comboBox2.Text = department;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            if (alter == false)
            {
                comboBox1.Text = "在职";
                comboBox1.Enabled = false;
                dateEdit2.Enabled = false;
            }
            else
            {
                comboBox2.Text = department;
                this.Text = "修改员工信息";
            }

        }

        private void UILocation()
        {
            int x = 3;
            int y = 2;
            label6.Location = new Point(comboBox1.Location.X - label6.Width - x, comboBox1.Location.Y + y);
            label4.Location = new Point(comboBox2.Location.X - label4.Width - x, comboBox2.Location.Y + y);
            label3.Location = new Point(textBox2.Location.X - label3.Width - x, textBox2.Location.Y + y);
            label1.Location = new Point(textBox3.Location.X - label1.Width - x, textBox3.Location.Y + y);
            label2.Location = new Point(dateEdit1.Location.X - label2.Width - x, dateEdit1.Location.Y + y);
            label7.Location = new Point(dateEdit2.Location.X - label7.Width - x, dateEdit2.Location.Y + y);
            label5.Location = new Point(textBox1.Location.X - label5.Width - x, textBox1.Location.Y + y);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "离职")
            {
                dateEdit2.Enabled = true;
            }else
            {
                dateEdit2.Enabled = false;
            }
        }
    }
}
