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
        public string KQID;
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
            if (textBoxID.Text == "")
            {
                MessageBox.Show("请输入考勤号！");
                return;
            }

            if (textBoxName.Text == "")
            {
                MessageBox.Show("请输入姓名！");
                return;
            }

            if (comboBoxDep.Text == "")
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
                textBoxID.Text = "";
                textBoxName.Text = "";
                dateEdit1.Text = "";
                textBoxRemark.Text = "";
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
            string sql = string.Format("select KQID from KQ_YG where KQID='{0}'", textBoxID.Text.Trim());

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

            string EntryDate = "";
            if (dateEdit1.Text != "")
            {
                EntryDate = "'" + Convert.ToDateTime(dateEdit1.Text).ToString("yyyy-MM-dd")+"'" ;
            }
            else
            {
                EntryDate = "null";
            }

            string sql1 = string.Format("insert into KQ_YG (KQID,YGXM,BMID,RZSJ,ZT,SM) values ('{0}','{1}','{2}',{3},'{4}','{5}')", textBoxID.Text.Trim(), textBoxName.Text.Trim(), comboBoxDep.SelectedValue, EntryDate, "0", textBoxRemark.Text.Trim());

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
            string EntryDate = "null";
            if (dateEdit1.Text != "")
            {
                EntryDate = "'" + Convert.ToDateTime(dateEdit1.Text).ToString("yyyy-MM-dd")+ "'" ;
            }
            else
            {
                EntryDate = "null";
            }

            string LeaveDate = "null";
            if (comboBoxState.Text == "离职")
            {
                if (dateEdit2.Text != "")
                {
                    LeaveDate = "'"+Convert.ToDateTime(dateEdit2.Text).ToString("yyyy-MM-dd")+"'";
                }
                else
                {
                    LeaveDate = "null";
                }
            }

            string State = "";
            if (comboBoxState.Text == "在职")
            {
                State = "0";
            }else
            {
                State = "1";
            }


            //更新或插入数据
            string sql = string.Format("update KQ_YG set KQID='{0}',YGXM='{1}',RZSJ={2},LZSJ={3},ZT='{4}',SM='{5}',BMID='{6}' where KQID='{7}'", textBoxID.Text.Trim(), textBoxName.Text.Trim(),EntryDate ,LeaveDate,State,textBoxRemark.Text ,comboBoxDep.SelectedValue,KQID);

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
                comboBoxDep.DataSource = Department;
                comboBoxDep.DisplayMember = "BMMC";
                comboBoxDep.ValueMember = "BMID";
                comboBoxDep.Text = department;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            if (alter == false)
            {
                comboBoxState.Text = "在职";
                comboBoxState.Enabled = false;
                dateEdit2.Enabled = false;
            }
            else
            {
                comboBoxDep.Text = department;
                this.Text = "修改员工信息";
            }

        }

        private void UILocation()
        {
            int x = 3;
            int y = 2;
            label6.Location = new Point(comboBoxState.Location.X - label6.Width - x, comboBoxState.Location.Y + y);
            label4.Location = new Point(comboBoxDep.Location.X - label4.Width - x, comboBoxDep.Location.Y + y);
            label3.Location = new Point(textBoxID.Location.X - label3.Width - x, textBoxID.Location.Y + y);
            label1.Location = new Point(textBoxName.Location.X - label1.Width - x, textBoxName.Location.Y + y);
            label2.Location = new Point(dateEdit1.Location.X - label2.Width - x, dateEdit1.Location.Y + y);
            label7.Location = new Point(dateEdit2.Location.X - label7.Width - x, dateEdit2.Location.Y + y);
            label5.Location = new Point(textBoxRemark.Location.X - label5.Width - x, textBoxRemark.Location.Y + y);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxState.Text == "离职")
            {
                dateEdit2.Enabled = true;
            }else
            {
                dateEdit2.Enabled = false;
            }
        }
    }
}
