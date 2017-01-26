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
    public partial class AttendanceFilter : Form
    {
        DataTable Filter = new DataTable();
        public AttendanceFilter()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt32(txtLate.Text);
                Convert.ToInt32(txtLeaveEarly.Text);
            }
            catch
            {
                MessageBox.Show("填入的字符必须是整数！");
                return;
            }

            if (Convert.ToInt32(txtLate.Text) > 60)
            {
                MessageBox.Show("填入的数字不能大于60！");
                return;
            }

            if (Convert.ToInt32(txtLeaveEarly.Text) > 60)
            {
                MessageBox.Show("填入的数字不能大于60！");
                return;
            }

            
            try
            {
                string sql = string.Format("update KQ_FILTER set Time='{0}' where Name='Late';", txtLate.Text)
                + string.Format("update KQ_FILTER set Time='{0}' where Name='LeaveEarly';", txtLeaveEarly.Text);
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            MessageBox.Show("保存成功，请点击 '查询计算' 重新计算考勤结果！");
            this.Close();
        }

        private void AttendanceFilter_Load(object sender, EventArgs e)
        {
            
            try
            {
                string sql = "select Name,Time from KQ_FILTER";
                Filter = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            for (int i = 0; i < Filter.Rows.Count; i++)
            {
                if (Filter.Rows[i]["Name"].ToString() == "Late")
                {
                    txtLate.Text = Filter.Rows[i]["Time"].ToString();
                    continue;
                }

                if (Filter.Rows[i]["Name"].ToString() == "LeaveEarly")
                {
                    txtLeaveEarly.Text = Filter.Rows[i]["Time"].ToString();
                    continue;
                }
            }

            

        }
    }
}
