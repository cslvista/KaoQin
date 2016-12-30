using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace KaoQin
{
    public partial class Schedual : Form
    {
        DataTable Department = new DataTable();
        DataTable Staff = new DataTable();
        DataTable WorkShift = new DataTable();
        DataTable Staff_WorkShift = new DataTable();
        public string DepartmentID;
        public string DepartmentName;
        public bool alter = false;
        public Schedual()
        {
            InitializeComponent();
        }

        private void Schedual_Load(object sender, EventArgs e)
        {
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit1.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-01");
            dateEdit2.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            if (alter == true)
            {
                comboBox1.Enabled = false;
                simpleButton1.Enabled = false;
            }
            SearchDepartment();
        }

        private void SearchDepartment()
        {
            Department.Columns.Add("BMID");
            Department.Columns.Add("BMMC");

            string sql = "select BMID,BMMC from KQ_BM where BMID>0";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                comboBox1.DataSource = Department;
                comboBox1.DisplayMember = "BMMC";
                comboBox1.ValueMember = "BMID";
                comboBox1.Text = DepartmentName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

        }

        private string Week(DateTime Day)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return weekdays[Convert.ToInt32(Day.DayOfWeek)];
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //校验日期
            DateTime StartDate;
            DateTime StopDate;
            TimeSpan Timespan;
            try
            {
                StartDate =Convert.ToDateTime(dateEdit1.Text);
                StopDate = Convert.ToDateTime(dateEdit2.Text);
                Timespan = StopDate-StartDate;

                if (Timespan.Days < 0)
                {
                    MessageBox.Show("结束时间不得小于开始时间！");
                    return;
                }

                if (Timespan.Days > 62)
                {
                    MessageBox.Show("不能生成超过两个月的计划！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:"+ex.Message);
                return;                   
            }

            //读取员工信息
            string sql1 = string.Format("select KQID,YGXM from KQ_YG where BMID='{0}'",comboBox1.SelectedValue);

            try
            {
                Staff=GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            //读取班次信息
            //string sql2 = string.Format("select ID,YGXM from KQ_YG where BMID='{0}'", DepartmentID);
            //try
            //{
            //    WorkShift = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("错误3:" + ex.Message);
            //    return;
            //}
            bandedGridView1.Columns.Clear();
            bandedGridView1.Bands.Clear();
            Staff_WorkShift.Columns.Clear();
            Staff_WorkShift.Clear();

            GridBand band = new GridBand();
            band.Caption = " ";
            band.Width = 30;
            bandedGridView1.Bands.Add(band);

            //生成列
            BandedGridColumn Staff_ID = new BandedGridColumn();
            Staff_ID.Caption = "考勤号";
            Staff_ID.Name = "Staff_ID";
            Staff_ID.FieldName = "KQID";
            Staff_ID.Visible = false;
            Staff_ID.OptionsColumn.AllowEdit = false;
            band.Columns.Add(Staff_ID);
            bandedGridView1.Columns.Add(Staff_ID);
            Staff_WorkShift.Columns.Add("KQID");

            BandedGridColumn Staff_Name = new BandedGridColumn();
            Staff_Name.Caption = "姓名";
            Staff_Name.Name = "Staff_Name";
            Staff_Name.Visible = true;
            Staff_Name.FieldName = "YGXM";
            Staff_Name.OptionsColumn.AllowEdit = false;
            band.Columns.Add(Staff_Name);
            bandedGridView1.Columns.Add(Staff_Name);
            Staff_WorkShift.Columns.Add("YGXM");

            for (int i = 0; i <= Timespan.Days; i++)
            {
                GridBand Day_band = new GridBand();
                Day_band.Caption = Week(StartDate);
                bandedGridView1.Bands.Add(Day_band);
                BandedGridColumn Day_Column = new BandedGridColumn();
                Day_Column.Caption = StartDate.ToString("yyyy-MM-dd");
                Day_Column.FieldName = StartDate.ToString("yyyy-MM-dd");
                Day_Column.Name= Day_Column.FieldName;
                Day_Column.Visible = true;
                Day_Column.OptionsColumn.AllowEdit = true;
                Day_band.Columns.Add(Day_Column);
                bandedGridView1.Columns.Add(Day_Column);
                Staff_WorkShift.Columns.Add(Day_Column.FieldName);
                StartDate =StartDate.AddDays(1);
            }

            foreach (DataRow Row in Staff.Rows)
            {
                Staff_WorkShift.Rows.Add(new object[] {Row["KQID"],Row["YGXM"]});
            }

            gridControl1.DataSource = Staff_WorkShift;
            bandedGridView1.BestFitColumns();

        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
