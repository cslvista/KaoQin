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
        public bool searchAllUsers = false;
        public add_alter_Users()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (alter == false)
            {
                MainUsers form = (MainUsers)this.Owner;
                form.simpleButton3_Click(null, null);               
            }
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
                dateEntry.Text = "";
                textBoxRemark.Text = "";
                dateBirthday.Text = "";
                MessageBox.Show("添加成功！");
            }else if (success == true && alter == true)
            {
                MainUsers form = (MainUsers)this.Owner;
                if (searchAllUsers==false)
                {
                    form.simpleButton3_Click(null, null);
                }                                
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
                isExist = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
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
            if (dateEntry.Text != "")
            {
                EntryDate = "'" + Convert.ToDateTime(dateEntry.Text).ToString("yyyy-MM-dd")+"'" ;
            }
            else
            {
                EntryDate = "null";
            }

            string birthday = "";
            if (dateBirthday.Text != "")
            {
                birthday = "'" + Convert.ToDateTime(dateBirthday.Text).ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                birthday = "null";
            }

            string sql1 = string.Format("insert into KQ_YG (KQID,YGXM,BMID,RZSJ,ZT,BZ,XB,CSRQ,CJRID,CJR,CJSJ) values ('{0}','{1}','{2}',{3},'{4}','{5}','{6}',{7},'{8}','{9}','{10}')", textBoxID.Text.Trim(), textBoxName.Text.Trim(), comboBoxDep.SelectedValue, EntryDate, "0", textBoxRemark.Text.Trim(),comboBoxSex.Text, birthday, GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime());

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql1);
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
            if (dateEntry.Text != "")
            {
                EntryDate = "'" + Convert.ToDateTime(dateEntry.Text).ToString("yyyy-MM-dd")+ "'" ;
            }
            else
            {
                EntryDate = "null";
            }

            string LeaveDate = "null";
            if (comboBoxState.Text == "离职")
            {
                if (dateLeave.Text != "")
                {
                    LeaveDate = "'"+Convert.ToDateTime(dateLeave.Text).ToString("yyyy-MM-dd")+"'";
                }
                else
                {
                    LeaveDate = "null";
                }
            }

            string birthday = "";
            if (dateBirthday.Text != "")
            {
                birthday = "'" + Convert.ToDateTime(dateBirthday.Text).ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                birthday = "null";
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
            StringBuilder sql = new StringBuilder();
            sql.Append (string.Format("update KQ_YG set KQID='{0}',YGXM='{1}',RZSJ={2},LZSJ={3},ZT='{4}',BZ='{5}',BMID='{6}',XB='{7}',CSRQ={8},XGRID='{9}',XGR='{10}',XGSJ='{11}' where KQID='{12}';", textBoxID.Text.Trim(), textBoxName.Text.Trim(), EntryDate, LeaveDate, State, textBoxRemark.Text, comboBoxDep.SelectedValue, comboBoxSex.Text,birthday, GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime(), KQID));

            if (textBoxID.Text.Trim() != KQID)
            {
                sql.Append(string.Format("update KQ_PB_XB set KQID='{0}' where KQID='{1}';",textBoxID.Text.Trim(),KQID)
                    + string.Format("update KQ_PB_LD set KQID='{0}' where KQID='{1}';", textBoxID.Text.Trim(), KQID));
            }
            
            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql.ToString());
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

            dateEntry.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEntry.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateLeave.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateLeave.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateBirthday.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateBirthday.Properties.Mask.EditMask = "yyyy-MM-dd";
            comboBoxSex.Text = "女";

            string sql = "select BMID,BMMC from KQ_BM where BMID>0";

            DataTable Department = new DataTable();
            Department.Columns.Add("BMID");
            Department.Columns.Add("BMMC");

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
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
                dateLeave.Enabled = false;
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
            label8.Location = new Point(comboBoxSex.Location.X - label4.Width - x, comboBoxSex.Location.Y + y);
            label3.Location = new Point(textBoxID.Location.X - label3.Width - x, textBoxID.Location.Y + y);
            label1.Location = new Point(textBoxName.Location.X - label1.Width - x, textBoxName.Location.Y + y);
            label9.Location = new Point(dateBirthday.Location.X - label1.Width - x, dateBirthday.Location.Y + y);
            label2.Location = new Point(dateEntry.Location.X - label2.Width - x, dateEntry.Location.Y + y);
            label7.Location = new Point(dateLeave.Location.X - label7.Width - x, dateLeave.Location.Y + y);
            label5.Location = new Point(textBoxRemark.Location.X - label5.Width - x, textBoxRemark.Location.Y + y);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxState.Text == "离职")
            {
                dateLeave.Enabled = true;
            }else
            {
                dateLeave.Enabled = false;
            }
        }
    }
}
