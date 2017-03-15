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
    public partial class add_alter_Item : Form
    {
        public bool alter = false;
        public string LBID="";
        public string ID = "";
        bool success = false;
        public add_alter_Item()
        {
            InitializeComponent();
        }

        private void AddItem_Load(object sender, EventArgs e)
        {
            UILocation();

            textBoxType.Enabled = false;
            if (alter == false)
            {
                textBoxCQ.Text = "1";
                comKT.Text = "否";
                comState.Text = "在用";
                comState.Enabled = false;
                comShiftType.Text = "上班";
                colorPickEdit1.EditValue = Color.Black;
            }
            else
            {
                this.Text = "修改班次";
            }
        }

        private void UILocation()
        {
            int x = 3;
            int y = 2;
            label4.Location = new Point(textBoxType.Location.X - label4.Width - x, textBoxType.Location.Y + y);
            label9.Location = new Point(comState.Location.X - label9.Width - x, comState.Location.Y + y);
            label8.Location = new Point(comKT.Location.X - label8.Width - x, comKT.Location.Y + y);
            label11.Location = new Point(comShiftType.Location.X - label8.Width - x, comShiftType.Location.Y + y);
            label7.Location = new Point(textBoxShift.Location.X - label7.Width - x, textBoxShift.Location.Y + y);
            label3.Location = new Point(timeWork.Location.X - label3.Width - x, timeWork.Location.Y );
            label1.Location = new Point(timeOffWork.Location.X - label1.Width - x, timeOffWork.Location.Y );
            label5.Location = new Point(textBoxCQ.Location.X - label5.Width - x, textBoxCQ.Location.Y + y);
            label2.Location = new Point(textBoxRemark.Location.X - label2.Width - x, textBoxRemark.Location.Y + y);
            label10.Location = new Point(colorPickEdit1.Location.X - label10.Width - x, colorPickEdit1.Location.Y + y);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBoxShift.Text == "")
            {
                MessageBox.Show("请输入班次名称！");
                return;
            }

            if (textBoxCQ.Text == "")
            {
                MessageBox.Show("请输入出勤日！");
                return;
            }

            if (checkBox1.Checked==false && checkBox2.Checked == false && comKT.Text=="否")
            {
                if (timeWork.Text == timeOffWork.Text)
                {
                    MessageBox.Show("上班时间和下班时间不能相同！");
                    return;
                }

                if (comKT.Text == "否" && DateTime.Compare(Convert.ToDateTime(timeWork.Text), Convert.ToDateTime(timeOffWork.Text)) >= 0)
                {
                    MessageBox.Show("下班时间不能大于或等于上班时间！");
                    return;
                }
            }


            try
            {
                Convert.ToDouble(textBoxCQ.Text);
            }
            catch
            {
                MessageBox.Show("请输入正确的出勤日！");
                return;
            }


            if (checkBox1.Checked == true && checkBox2.Checked == true && textBoxCQ.Text == "0" && comShiftType.Text=="")
            {
                MessageBox.Show("请选择班次类型！");
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
                form.ButtonRefresh_Click(null, null);
                this.Close();
            }

        }

        private bool Add()
        {
            string sql = string.Format("select max(ID) from KQ_BC");
            string ID = "";
            DataTable MaxID = new DataTable();
            try
            {
                MaxID = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                if (MaxID.Rows[0][0].ToString() == "")
                {
                    ID = "1";
                }
                else
                {
                    ID = (Convert.ToInt32(MaxID.Rows[0][0].ToString()) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            string WorkTime = "";//上班时间
            string OffWorkTime = "";//下班时间

            if (checkBox1.Checked == false)
            {
                WorkTime = Convert.ToDateTime(timeWork.Text).ToString("HH:mm");
            }
            else
            {
                WorkTime = "";
            }

            if (checkBox2.Checked == false)
            {
                OffWorkTime = Convert.ToDateTime(timeOffWork.Text).ToString("HH:mm");
            }
            else
            {
                OffWorkTime = "";
            }

            string sql1 = string.Format("insert into KQ_BC (ID,LBID,ZT,KT,NAME,SBSJ,XBSJ,GZR,SM,COLOR,CJRID,CJR,CJSJ,Type) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')", ID, LBID, "0",comKT.SelectedIndex,textBoxShift.Text.Trim(),WorkTime,OffWorkTime, textBoxCQ.Text.Trim(), textBoxRemark.Text.Trim(), ColorTranslator.ToHtml(colorPickEdit1.Color) ,GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime(),comShiftType.Text);

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
            string WorkTime = "";//上班时间
            string OffWorkTime = "";//下班时间

            if (checkBox1.Checked == false)
            {
                WorkTime = Convert.ToDateTime(timeWork.Text).ToString("HH:mm");
            }else
            {
                WorkTime = "";
            }

            if (checkBox2.Checked == false)
            {
                OffWorkTime = Convert.ToDateTime(timeOffWork.Text).ToString("HH:mm");
            }
            else
            {
                OffWorkTime = "";
            }

            //更新或插入数据
            
            string sql = string.Format("update KQ_BC set NAME='{0}',SBSJ='{1}',XBSJ='{2}',GZR='{3}',SM='{4}',XGRID='{5}',XGR='{6}',XGSJ='{7}',ZT='{8}',KT='{9}',COLOR='{10}',Type='{11}' where ID='{12}'", textBoxShift.Text.Trim(),WorkTime,OffWorkTime, textBoxCQ.Text.Trim(), textBoxRemark.Text.Trim(), GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime(),comState.SelectedIndex,comKT.SelectedIndex, ColorTranslator.ToHtml(colorPickEdit1.Color), comShiftType.Text,ID);

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timeWork.Enabled = !checkBox1.Checked;
        }

        private void TypeEnable()
        {
            if (checkBox1.Checked==true && checkBox2.Checked==true && textBoxCQ.Text == "0")
            {
                comShiftType.Enabled = true;
            }else
            {
                comShiftType.Enabled = false;
                comShiftType.Text = null;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            timeOffWork.Enabled = !checkBox2.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comKT.Text == "是")
            {
                label1.Text = "下班时间(明)：";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                UILocation();
            }
            else
            {
                label1.Text = "下班时间：";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                UILocation();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxCQ_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
