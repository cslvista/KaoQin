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
            textBox1.Enabled = false;
            if (alter == false)
            {
                textBox2.Text = "1";
            }
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

            if (checkBox1.Checked==false && checkBox2.Checked == false)
            {
                if (timeEdit1.Text == timeEdit2.Text)
                {
                    MessageBox.Show("上班时间和下班时间不能相同！");
                    return;
                }

                if (DateTime.Compare(Convert.ToDateTime(timeEdit1.Text), Convert.ToDateTime(timeEdit2.Text)) >= 0)
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

            string SBSJ = "";//上班时间
            string XBSJ = "";//下班时间

            if (checkBox1.Checked==false)
            {
                SBSJ = Convert.ToDateTime(timeEdit1.Text).ToString("HH:mm");
            }

            if (checkBox2.Checked == false)
            {
                XBSJ = Convert.ToDateTime(timeEdit2.Text).ToString("HH:mm");
            }

            string sql1 = string.Format("insert into KQ_BC (ID,LBID,NAME,SBSJ,XBSJ,GZR,SM,CJRID,CJR,CJSJ) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", ID, LBID, textBox3.Text.Trim(),SBSJ,XBSJ, textBox2.Text.Trim(), textBox4.Text.Trim(), GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime());

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
            string SBSJ = "";//上班时间
            string XBSJ = "";//下班时间

            if (checkBox1.Checked == false)
            {
                SBSJ = Convert.ToDateTime(timeEdit1.Text).ToString("HH:mm");
            }

            if (checkBox2.Checked == false)
            {
                XBSJ = Convert.ToDateTime(timeEdit2.Text).ToString("HH:mm");
            }

            //更新或插入数据
            string sql = string.Format("update KQ_BC set NAME='{0}',SBSJ='{1}',XBSJ='{2}',GZR='{3}',SM='{4}',XGRID='{5}',XGR='{6}',XGSJ='{7}' where ID='{8}'", textBox3.Text.Trim(),SBSJ,XBSJ, textBox2.Text.Trim(), textBox4.Text.Trim(), GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime(),ID);

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
    }
}
