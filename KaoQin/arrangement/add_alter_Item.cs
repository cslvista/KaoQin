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

            textBox1.Enabled = false;
            if (alter == false)
            {
                textBox2.Text = "1";
                comboBox1.Text = "否";
                comboBox2.Text = "在用";
                comboBox2.Enabled = false;
                colorPickEdit1.EditValue = Color.Black;
            }
            else
            {
                
            }
        }

        private void UILocation()
        {
            int x = 3;
            int y = 2;
            label4.Location = new Point(textBox1.Location.X - label4.Width - x, textBox1.Location.Y + y);
            label9.Location = new Point(comboBox2.Location.X - label9.Width - x, comboBox2.Location.Y + y);
            label8.Location = new Point(comboBox1.Location.X - label8.Width - x, comboBox1.Location.Y + y);
            label7.Location = new Point(textBox3.Location.X - label7.Width - x, textBox3.Location.Y + y);
            label3.Location = new Point(timeEdit1.Location.X - label3.Width - x, timeEdit1.Location.Y );
            label1.Location = new Point(timeEdit2.Location.X - label1.Width - x, timeEdit2.Location.Y );
            label5.Location = new Point(textBox2.Location.X - label5.Width - x, textBox2.Location.Y + y);
            label2.Location = new Point(textBox4.Location.X - label2.Width - x, textBox4.Location.Y + y);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("请输入名称！");
                return;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入工作日！");
                return;
            }

            if (checkBox1.Checked==false && checkBox2.Checked == false && comboBox1.Text=="否")
            {
                if (timeEdit1.Text == timeEdit2.Text)
                {
                    MessageBox.Show("上班时间和下班时间不能相同！");
                    return;
                }

                if (comboBox1.Text == "否" && DateTime.Compare(Convert.ToDateTime(timeEdit1.Text), Convert.ToDateTime(timeEdit2.Text)) >= 0)
                {
                    MessageBox.Show("下班时间不能大于或等于上班时间！");
                    return;
                }
            }


            try
            {
                Convert.ToInt16(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("请输入正确的工作日！");
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
                MaxID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
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
                WorkTime = Convert.ToDateTime(timeEdit1.Text).ToString("HH:mm");
            }
            else
            {
                WorkTime = "";
            }

            if (checkBox2.Checked == false)
            {
                OffWorkTime = Convert.ToDateTime(timeEdit2.Text).ToString("HH:mm");
            }
            else
            {
                OffWorkTime = "";
            }

            string sql1 = string.Format("insert into KQ_BC (ID,LBID,ZT,KT,NAME,SBSJ,XBSJ,GZR,SM,COLOR,CJRID,CJR,CJSJ) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", ID, LBID, "0",comboBox1.SelectedIndex,textBox3.Text.Trim(),WorkTime,OffWorkTime, textBox2.Text.Trim(), textBox4.Text.Trim(), ColorTranslator.ToHtml(colorPickEdit1.Color) ,GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime());

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
            string WorkTime = "";//上班时间
            string OffWorkTime = "";//下班时间

            if (checkBox1.Checked == false)
            {
                WorkTime = Convert.ToDateTime(timeEdit1.Text).ToString("HH:mm");
            }else
            {
                WorkTime = "";
            }

            if (checkBox2.Checked == false)
            {
                OffWorkTime = Convert.ToDateTime(timeEdit2.Text).ToString("HH:mm");
            }
            else
            {
                OffWorkTime = "";
            }

            //更新或插入数据
            
            string sql = string.Format("update KQ_BC set NAME='{0}',SBSJ='{1}',XBSJ='{2}',GZR='{3}',SM='{4}',XGRID='{5}',XGR='{6}',XGSJ='{7}',ZT='{8}',KT='{9}',COLOR='{10}' where ID='{11}'", textBox3.Text.Trim(),WorkTime,OffWorkTime, textBox2.Text.Trim(), textBox4.Text.Trim(), GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime(),comboBox2.SelectedIndex,comboBox1.SelectedIndex, ColorTranslator.ToHtml(colorPickEdit1.Color), ID);

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timeEdit1.Enabled = !checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            timeEdit2.Enabled = !checkBox2.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "是")
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
                label1.Text = "下班时间(今)：";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                UILocation();
            }
        }

    }
}
