using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class Remark : Form
    {
        bool alter = false;
        public string BMID = "";
        public string BMMC = "";
        public string Year = "";
        public string Month = "";
        DataTable remark = new DataTable();
        public Remark()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (alter == false)
            {
                string sql = "select max(ID) from KQ_Remark";

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
                    return;
                }

                string sql1 = string.Format("insert into KQ_Remark(ID,BMID,Year,Month,Remark_TB,Remark_CQ,CJR,CJSJ) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", ID, BMID,Year,Month,textBox1.Text.Trim(), textBox2.Text.Trim(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime());

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误2:" + ex.Message, "提示");
                    return;
                }
            }else
            {               
                try
                {
                    string sql = string.Format("update KQ_Remark set Remark_TB='{0}',Remark_CQ='{1}',XGR='{2}',XGSJ='{3}' where BMID='{4}' and Year='{5}' and Month='{6}'", textBox1.Text.Trim(), textBox2.Text.Trim(),GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime(), BMID,Year,Month);
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误3:" + ex.Message, "提示");
                    return;
                }

                //写日志
                writeLog();
            }

            this.Close();


        }

        private void writeLog()
        {
            string sql_del = string.Format("select max(ID) from KQ_LOG");
            string ID = "";
            DataTable MaxID = new DataTable();
            try
            {
                MaxID = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql_del);
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
                MessageBox.Show("错误4:" + ex.Message);
                return;
            }

            string Record = string.Format("{0}修改了{1}{2}{3}的排班与考勤备注", GlobalHelper.UserHelper.User["U_NAME"].ToString(), BMMC,Year,Month);
            string sql1 = string.Format("insert into KQ_LOG (ID,Record,Time) values ('{0}','{1}','{2}')", ID, Record, GlobalHelper.IDBHelper.GetServerDateTime());
            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误5:" + ex.Message);
                return;
            }
        }

        private void Remark_Load(object sender, EventArgs e)
        {
            this.Text = BMMC + Year + Month + "调班与出勤备注";

            try
            {
                string sql = string.Format("select ID,Remark_TB,Remark_CQ from KQ_Remark where YEAR='{0}' and MONTH='{1}' and BMID='{2}'", Year,Month,BMID);
                remark = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                if (remark.Rows.Count > 0)
                {
                    alter = true;
                    textBox1.Text = remark.Rows[0][1].ToString();
                    textBox2.Text = remark.Rows[0][2].ToString();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取备注信息错误:" + ex.Message);

            }
        }
    }
}
