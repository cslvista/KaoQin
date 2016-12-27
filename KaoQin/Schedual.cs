using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class Schedual : Form
    {
        DataTable Department = new DataTable();
        DataTable Staff = new DataTable();
        public Schedual()
        {
            InitializeComponent();
        }

        private void Schedual_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
               // TimeSpan timespan = (StartTime.Value - StopTime.Value);

                //if (timespan.TotalDays > 61)
                //{
                //    MessageBox.Show("不能生成超过两个月的计划!");
                //    return;
                //}

                DataTable depts = new DataTable();
                //string ksid = depts.Rows[KS.SelectedIndex]["UT_ID"].ToString();
                //gridControl1.DataSource = BuildGridData(jhid, ksid, KSSJ.Value, JSSJ.Value);
                gridControl1.ForceInitialize();
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("生成排班计划出错:{0} \r\n{1}", ex.Message, ex.StackTrace), "错误"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private DataTable BuildGridData(DateTime StartTime, DateTime StopTime)
        //{

        //}

        private void comboBoxDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("select KQID,YGXM from KQ_YG where BMID='{0}'");

            try
            {
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }
    }
}
