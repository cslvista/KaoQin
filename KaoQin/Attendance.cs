using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace KaoQin
{
    public partial class Attendance : Form
    {
        zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机
        DataTable Machine = new DataTable();
        DataTable Department = new DataTable();
        DataTable Staff = new DataTable();
        public Attendance()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("查询需要1-2分钟的时间，是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
                
            //读取考勤机数据
            string sql = "select ID,Machine,IP,Port,Password from KQ_Machine";

            try
            {
                Machine = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            if (Machine.Rows.Count == 0)
            {
                MessageBox.Show("没有可用设备！");
                return;
            }

            bool bIsConnected = false;//判断设备是否可连接

            for (int i = 0; i < Machine.Rows.Count; i++)
            {
                try
                {
                    DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[i]["Password"].ToString()));
                    bIsConnected = DKJ.Connect_Net(Machine.Rows[i]["IP"].ToString(), Convert.ToInt32(Machine.Rows[i]["Port"].ToString()));
                    if (bIsConnected == false)
                    {
                        MessageBox.Show(string.Format("'{0}'无法连接！", Machine.Rows[i]["Machine"].ToString()));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误2:" + ex.Message);
                    return;
                }
            }
            
            //读取员工信息
            string sql1 =string.Format("select KQID,YGXM from KQ_YG where BMID='{0}'");

            try
            {
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }

            string sql2 = "";

            DataTable Record_DKJ = new DataTable();
            Record_DKJ.Columns.Add("ID", typeof(string));
            Record_DKJ.Columns.Add("Time", typeof(string));
            Record_DKJ.Columns.Add("LY", typeof(string));

            int iMachineNumber = 0;
            int VerifyMode = 0;
            int InOutMode = 0;
            int Year = 0;
            int Month = 0;
            int Day = 0;
            int Hour = 0;
            int Minute = 0;
            int Second = 0;
            int Workcode = 0;
            string dwEnrollNumber = "";

            int j = 0;
            for (int i = 0; i < Machine.Rows.Count; i++)
            {
                DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[i]["Password"].ToString()));
                bIsConnected = DKJ.Connect_Net(Machine.Rows[i]["IP"].ToString(), Convert.ToInt32(Machine.Rows[i]["Port"].ToString()));
                if (bIsConnected == false)
                {
                    MessageBox.Show(string.Format("'{0}'无法连接！", Machine.Rows[i]["Machine"].ToString()));
                    return;
                }

                DKJ.ReadAllGLogData(iMachineNumber);//read all the user information to the memory
                while (DKJ.SSR_GetGeneralLogData(iMachineNumber, out dwEnrollNumber, out VerifyMode, out InOutMode, out Year, out Month, out Day, out Hour, out Minute, out Second, ref Workcode))
                {
                    j++;
                    string time = string.Format("{0}-{1}-{2} {3}:{4}:{5}", Year, Month, Day, Hour, Minute, Second);
                    string time1 = Convert.ToDateTime(time).ToString("yyyy-MM-dd  HH:mm:ss");
                    Record_DKJ.Rows.Add(new object[] { dwEnrollNumber, time1, Machine.Rows[i]["Machine"].ToString() });
                    this.Text = string.Format("已读取{0}第{1}条数据", j, Machine.Rows[i]["Machine"].ToString());
                }
            }
               
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入部门名称";
            searchControl2.Properties.NullValuePrompt = "请输入考勤号或姓名";
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit1.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-01");
            dateEdit2.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");

            SearchDepartment();
        }

        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC from KQ_BM where BMID>0";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Department;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl1.Text);
        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //
            //TimeSpan timespan = Convert.ToDateTime(dateEdit2.Text).CompareTo(Convert.ToDateTime(dateEdit2.Text));

            //if (timespan.TotalDays > 61)
            //{
            //    MessageBox.Show("不能生成超过两个月的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

        }

        private string Week(DateTime Day)
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return weekdays[Convert.ToInt32(Day.DayOfWeek)];
        }
        private void InitGridBandData()
        {
            bandedGridView1.Columns.Clear();
            bandedGridView1.Bands.Clear();

            GridBand Staff = new GridBand();
            Staff.Caption = " ";
            Staff.Width = 80;

            BandedGridColumn column = new BandedGridColumn();
            column.Caption = "考勤号";
            column.FieldName = "KQID";
            column.Visible = true;
            column.Width = 40;
            Staff.Columns.Add(column);
            bandedGridView1.Columns.Add(column);

            column = new BandedGridColumn();
            column.Caption = "姓名";
            column.FieldName = "YGXM";
            column.OptionsColumn.AllowEdit = false;
            column.Visible = true;
            column.Width = 40;
            Staff.Columns.Add(column);
            bandedGridView1.Columns.Add(column);

            DateTime time = DateTime.Now;
            DataRow[] rows;

            for (int i=0;i<10;i++)
            {
                GridBand Day = new GridBand();
                Day.Caption = string.Format(time.ToString("yyyy-MM-dd")+"({0})", Week(time));
                BandedGridColumn Morning = new BandedGridColumn();
                Morning.Caption = "上午";
                Morning.OptionsColumn.AllowEdit = false;
                BandedGridColumn Afternoon = new BandedGridColumn();
                Afternoon.Caption = "下午";
                Afternoon.OptionsColumn.AllowEdit = false;
                Day.Columns.Add(Morning);
                Day.Columns.Add(Afternoon);              
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SearchDepartment();
        }
    }
}
