using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;
using System.Collections;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Columns;

namespace KaoQin
{
    public partial class Attendance : Form
    {
        
        DataTable Machine = new DataTable();//机器信息
        DataTable Department = new DataTable();//部门
        public DataTable AttendanceResult = new DataTable();//考勤结果
        DataTable AttendanceCollect = new DataTable();//考勤汇总
        DataTable AttendanceShiftCollect = new DataTable();//考勤班次汇总
        DataTable Staff = new DataTable();//整个部门的所有员工，包括在职离职
        public DataTable Staff_Orign = new DataTable();//打卡机的原始员工数据，包括考勤号和姓名
        DataTable WorkShift = new DataTable();//班次信息
        DataTable PBID = new DataTable();//排班ID
        DataTable ArrangementItem = new DataTable();//排班细表
        DataTable ArrangementItem_LastDay = new DataTable();//上个月排班细表最后一天
        DataTable PersonShift = new DataTable();//个人单日的排班
        DataTable PersonShiftAll = new DataTable();//个人单日的排班的所有信息
        DataTable Filter = new DataTable();//过滤数据
        DataTable Record_Dep = new DataTable();//选定部门员工的打卡数据
        public DataTable Record_DKJ = new DataTable();//考勤机原始数据
        DataTable Record_Person = new DataTable();//个人单日打卡数据
        DateTime StartDate;
        DateTime StopDate;
        DateTime LastMonth;
        TimeSpan Timespan;
        public double[][] WorkDayCount;
        public bool HasDownload = false;//是否已下载数据
        delegate void UpdateUI();
        string Record_startTime = "";//考勤原始数据的起始时间
        string Record_stopTime = "";//考勤原始数据的结束时间
        public bool Authority_Attendance_VisitMachine = false;
        public bool Authority_Attendance_DelDB = false;
        public Attendance()
        {
            InitializeComponent();

        }

        private void FromMachine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Authority_Attendance_VisitMachine == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }


            if (HasDownload)
            {
                if (MessageBox.Show("数据已经下载到本地，是否还需要重新下载？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                if (MessageBox.Show("从考勤机下载数据需要约1-3分钟的时间，是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            Staff_Orign.Clear();

            //读取考勤机数据
            try
            {
                zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机
                LoadingForm form = new LoadingForm();
                form.ShowDialog(this);
            }
            catch
            {
                MessageBox.Show("本机没有安装相应的软件，请联系信息部安装！");
            }
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            UILocation();
            searchControl1.Properties.NullValuePrompt = "请输入部门名称";
            searchControl2.Properties.NullValuePrompt = "请输入姓名";

            ButtonOrignData.Enabled = false;
            Record_DKJ.Columns.Add("ID", typeof(string));
            Record_DKJ.Columns.Add("Time", typeof(string));
            Record_DKJ.Columns.Add("Source", typeof(string));

            Record_Dep.Columns.Add("ID", typeof(string));
            Record_Dep.Columns.Add("Name", typeof(string));
            Record_Dep.Columns.Add("Time", typeof(string));
            Record_Dep.Columns.Add("Source", typeof(string));

            Record_Person.Columns.Add("Time", typeof(string));
            Record_Person.Columns.Add("Source", typeof(string));

            Staff_Orign.Columns.Add("ID", typeof(string));
            Staff_Orign.Columns.Add("Name", typeof(string));

            PersonShift.Columns.Add("PD", typeof(string));//判断，昨日或今日
            PersonShift.Columns.Add("ID", typeof(string));

            PersonShiftAll.Columns.Add("PD", typeof(string));
            PersonShiftAll.Columns.Add("ID", typeof(string));
            PersonShiftAll.Columns.Add("NAME", typeof(string));
            PersonShiftAll.Columns.Add("SBSJ", typeof(string));
            PersonShiftAll.Columns.Add("XBSJ", typeof(string));
            PersonShiftAll.Columns.Add("WorkDay", typeof(double));
            PersonShiftAll.Columns.Add("KT", typeof(string));

            AttendanceCollect.Columns.Add("KQID", typeof(string));
            AttendanceCollect.Columns.Add("YGXM", typeof(string));
            AttendanceCollect.Columns.Add("TotalDays", typeof(int));
            AttendanceCollect.Columns.Add("Normal", typeof(int));
            AttendanceCollect.Columns.Add("Rest", typeof(int));
            AttendanceCollect.Columns.Add("Late", typeof(int));
            AttendanceCollect.Columns.Add("LeaveEarly", typeof(int));
            AttendanceCollect.Columns.Add("Morning", typeof(int));
            AttendanceCollect.Columns.Add("Afternoon", typeof(int));
            AttendanceCollect.Columns.Add("WorkDay", typeof(double));
            AttendanceCollect.Columns.Add("WorkYear", typeof(string));

            string TimeNow = GlobalHelper.IDBHelper.GetServerDateTime();
            comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).Year.ToString() + "年");
            comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-1).Year.ToString() + "年");
            comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-2).Year.ToString() + "年");
            comboBoxYear.Text = Convert.ToDateTime(TimeNow).Year.ToString() + "年";
            comboBoxMonth.Text = Convert.ToDateTime(TimeNow).Month.ToString() + "月";
            SearchDepartment();
        }

        private void UILocation()
        {
            int height = (panelControl2.Height - ButtonCal.Height) / 2;
            ButtonCal.Location = new Point(ButtonCal.Location.X, height);
            ButtonOrignData.Location = new Point(ButtonOrignData.Location.X, height);
            ButtonFilter.Location = new Point(ButtonFilter.Location.X, height);
            ButtonImport.Location = new Point(ButtonImport.Location.X, height);
            ButtonExport.Location = new Point(ButtonExport.Location.X, height);
            ButtonRefresh1.Location = new Point(ButtonRefresh1.Location.X, height);
            test.Location = new Point(test.Location.X, height);
            test.Visible = false;
            comboBoxYear.Location = new Point(comboBoxYear.Location.X, (panelControl2.Height - comboBoxYear.Height) / 2);
            comboBoxMonth.Location = new Point(comboBoxMonth.Location.X, (panelControl2.Height - comboBoxMonth.Height) / 2);
            searchControl2.Location = new Point(searchControl2.Location.X, (panelControl2.Height - searchControl2.Height) / 2);
            searchControl1.Location = new Point(searchControl1.Location.X, (panelControl1.Height - searchControl1.Height) / 2);
            label4.Location = new Point(comboBoxYear.Location.X - label4.Width - 2, comboBoxYear.Location.Y + 2);
        }

        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC,BMLB from KQ_BM where BMID>0";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
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
            if (Department.Rows.Count > 0)
            {
                Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl1.Text);
            }

        }

        private void ButtonCal_Click(object sender, EventArgs e)
        {
            //初始化
            gridControl2.DataSource = null;
            gridControl3.DataSource = null;
            gridControl4.DataSource = null;
            bandedGridView2.IndicatorWidth = 40;
            gridView3.IndicatorWidth = 40;
            gridView4.IndicatorWidth = 40;
            bandedGridView2.Columns.Clear();
            bandedGridView2.Bands.Clear();
            gridView4.Columns.Clear();
            searchControl2.Text = "";
            //如果没有这一句，就会导致在增加新行的时候报错
            AttendanceResult.DefaultView.RowFilter = "";
            AttendanceResult.Clear();
            AttendanceResult.Columns.Clear();
            Record_Dep.Clear();
            AttendanceShiftCollect.DefaultView.RowFilter = "";
            AttendanceShiftCollect.Clear();
            AttendanceShiftCollect.Columns.Clear();
            try
            {
                string year = comboBoxYear.Text.Substring(0, 4);
                string month = comboBoxMonth.Text.Substring(0, comboBoxMonth.Text.IndexOf("月"));
                string timeNow = GlobalHelper.IDBHelper.GetServerDateTime();
                string yearNow = Convert.ToDateTime(timeNow).Year.ToString() + "年";
                string monthNow = Convert.ToDateTime(timeNow).Month.ToString() + "月";
                string startDate = Convert.ToDateTime(year + "-" + month + "-" + "1").ToString("yyyy-MM-dd");
                StartDate = Convert.ToDateTime(startDate);
                LastMonth = StartDate.AddMonths(-1);
                //判断是否大于本月
                if (StartDate.CompareTo(Convert.ToDateTime(timeNow)) > 0)
                {
                    MessageBox.Show("无法查看此月考勤结果！");
                    return;
                }
                //如果查询本月的记录，则最终日期是今天
                if (yearNow == comboBoxYear.Text && monthNow == comboBoxMonth.Text)
                {
                    StopDate = Convert.ToDateTime(Convert.ToDateTime(timeNow).ToString("yyyy-MM-dd"));
                }
                else
                {
                    StopDate = StartDate.AddMonths(1).AddDays(-1);
                }
                Timespan = StopDate - StartDate;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            //读取数据库信息，包括班次、排班、员工信息、过滤信息
            if (ReadDatabase() == false)
            {
                return;
            }

            //判断考勤记录是否包含本月的记录，如果没有就从数据库下载
            bool judgeTime = JudgeTime();

            if (Record_DKJ.Rows.Count > 0)
            {
                ButtonOrignData.Enabled = true;
            }

             if (judgeTime == false)
            {
                if (MessageBox.Show(string.Format("数据库中的考勤记录不完整，是否继续？"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            //计算出勤天数
            WorkDayCount = new double[ArrangementItem.Rows.Count][];

            GridBand band = new GridBand();
            band.Caption = " ";
            band.Width = 30;
            band.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            bandedGridView2.Bands.Add(band);

            //生成列
            BandedGridColumn Staff_ID = new BandedGridColumn();
            Staff_ID.Caption = "考勤号";
            Staff_ID.Name = "Staff_ID";
            Staff_ID.FieldName = "KQID";
            Staff_ID.Visible = false;
            Staff_ID.OptionsColumn.AllowEdit = false;
            band.Columns.Add(Staff_ID);
            AttendanceResult.Columns.Add("KQID");
            bandedGridView2.Columns.Add(Staff_ID);

            BandedGridColumn Staff_Name = new BandedGridColumn();
            Staff_Name.Caption = "姓名";
            Staff_Name.Name = "Staff_Name";
            Staff_Name.Visible = true;
            Staff_Name.FieldName = "YGXM";
            Staff_Name.OptionsColumn.AllowEdit = false;
            band.Columns.Add(Staff_Name);
            AttendanceResult.Columns.Add("YGXM");
            bandedGridView2.Columns.Add(Staff_Name);

            //生成日期列
            DateTime day = StartDate;
            for (int i = 0; i <= Timespan.Days; i++)    
            {
                GridBand Day_band = new GridBand();
                Day_band.Caption = Week(day);
                bandedGridView2.Bands.Add(Day_band);
                BandedGridColumn Day_Column = new BandedGridColumn();
                Day_Column.Caption = day.ToString("yyyy-MM-dd");
                Day_Column.FieldName = day.ToString("yyyy-MM-dd");
                Day_Column.Name = Day_Column.Caption;
                Day_Column.Visible = true;
                Day_Column.OptionsColumn.AllowEdit = true;
                Day_band.Columns.Add(Day_Column);
                AttendanceResult.Columns.Add(Day_Column.FieldName);
                bandedGridView2.Columns.Add(Day_Column);
                day = day.AddDays(1);
            }

            //从原始打卡数据中得到指定部门的员工打卡数据
            var query = (from rec in Record_DKJ.AsEnumerable()
                        join staff in Staff.AsEnumerable()
                        on rec.Field<string>("ID") equals staff.Field<string>("KQID")
                        where Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(StartDate) > 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(StopDate.AddDays(1)) < 0
                        select new
                        {
                            ID = rec.Field<string>("ID"),
                            Name = staff.Field<string>("YGXM"),
                            Time = rec.Field<string>("Time"),
                            Source = rec.Field<string>("Source"),
                        }).Distinct();

            foreach (var obj in query)
            {
                Record_Dep.Rows.Add(obj.ID, obj.Name, obj.Time, obj.Source);
            }

            //进行考勤明细分析
            AnalysisData(StartDate, Timespan.Days);
            //进行考勤汇总分析
            DataCollect(StartDate, Timespan.Days);
            //进行班次汇总分析
            ShiftCollect();
        }

        private bool ReadDatabase()
        {
            //读取班次信息            
            try
            {
                string sql = string.Format("select ID,NAME,SBSJ,XBSJ,GZR,KT from KQ_BC where LBID='{0}' or LBID='{1}'", "0", gridView1.GetFocusedRowCellValue("BMLB").ToString());
                WorkShift = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取班次信息错误:" + ex.Message);
                return false;
            }

            //读取排班主表            
            try
            {
                string sql = string.Format("select PBID from KQ_PB where YEAR='{0}' and MONTH='{1}' and BMID='{2}'", comboBoxYear.Text, comboBoxMonth.Text, gridView1.GetFocusedRowCellValue("BMID").ToString());
                PBID = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取排班主表错误:" + ex.Message);
                return false;
            }


            //读取排班细表和上个月最后一天的排班情况
            if (PBID.Rows.Count > 0)
            {
                string sql1 = string.Format("select * from KQ_PB_XB where BMID='{0}' and PBID='{1}' order by KQID", gridView1.GetFocusedRowCellValue("BMID").ToString(), PBID.Rows[0][0].ToString());
                string sql2 = string.Format("select * from KQ_PB_LD where YEAR='{0}' and MONTH='{1}' and BMID='{2}' order by KQID", LastMonth.Year.ToString() + "年", LastMonth.Month.ToString() + "月", gridView1.GetFocusedRowCellValue("BMID").ToString());
                try
                {
                    ArrangementItem = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql1);
                    ArrangementItem_LastDay = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取排班细表错误:" + ex.Message);
                    return false;
                }
            }
            else
            {
                MessageBox.Show(string.Format("{0}在{1}{2}没有排班记录，无法查看考勤结果！", gridView1.GetFocusedRowCellValue("BMMC").ToString(), comboBoxYear.Text, comboBoxMonth.Text));
                return false;
            }

            //读取过滤数据          
            try
            {
                string sql = "select Name,Time from KQ_FILTER";
                Filter = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取过滤数据错误:" + ex.Message);
                return false;
            }

            //读取员工信息            
            try
            {
                string sql = string.Format("select b.* from KQ_PB_XB a left join KQ_YG b on a.KQID=b.KQID where a.BMID='{0}' and a.PBID='{1}' order by a.KQID", gridView1.GetFocusedRowCellValue("BMID").ToString(), PBID.Rows[0][0].ToString());
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取员工信息错误:" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 考勤明细分析
        /// </summary>
        private void AnalysisData(DateTime StartDate, int Timespan)
        {


            for (int i = 0; i < Staff.Rows.Count; i++)
            {
                AttendanceResult.Rows.Add();
                WorkDayCount[i] = new double[Timespan + 1];

                //添加姓名与考勤号
                AttendanceResult.Rows[i]["KQID"] = Staff.Rows[i]["KQID"];
                AttendanceResult.Rows[i]["YGXM"] = Staff.Rows[i]["YGXM"];
                for (int j = 0; j <= Timespan; j++)
                {

                    //查询昨日、今日、明日的考勤数据
                    Record_Person.Clear();
                    string KQID = Staff.Rows[i]["KQID"].ToString();
                    string Date = StartDate.AddDays(j).ToString("yyyy-MM-dd");
                    var query = from rec in Record_Dep.AsEnumerable()
                                where rec.Field<string>("ID") == KQID && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(-1)) >= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(2)) < 0
                                select new
                                {
                                    Time = rec.Field<string>("Time"),
                                    Source = rec.Field<string>("Source"),
                                };
                    foreach (var obj in query)
                    {
                        Record_Person.Rows.Add(obj.Time, obj.Source);
                    }

                    //查询当日和昨日排班记录，查昨日是因为有跨天的情况
                    PersonShift.Clear();
                    PersonShift.Rows.Add(new object[] { });
                    PersonShift.Rows.Add(new object[] { });
                    if (j == 0)
                    {
                        //第一天的排班，取两个数据表进行操作
                        string yesterday = string.Format("D{0}T", j + 1);
                        var query1 = from Item in ArrangementItem.AsEnumerable()
                                     where Item.Field<string>("KQID") == Staff.Rows[i]["KQID"].ToString()
                                     select new
                                     {
                                         Today = Item.Field<string>(yesterday)
                                     };

                        var query_lastday = from lastday in ArrangementItem_LastDay.AsEnumerable()
                                            where lastday.Field<string>("KQID") == Staff.Rows[i]["KQID"].ToString()
                                            select new
                                            {
                                                yesterday = lastday.Field<string>("LastDay")
                                            };

                        foreach (var obj in query_lastday)
                        {
                            PersonShift.Rows[0]["ID"] = obj.yesterday;
                            PersonShift.Rows[0]["PD"] = "0";
                        }

                        foreach (var obj in query1)
                        {
                            PersonShift.Rows[1]["ID"] = obj.Today;
                            PersonShift.Rows[1]["PD"] = "1";
                        }
                    }
                    else
                    {
                        string yesterday = string.Format("D{0}T", j);
                        string today = string.Format("D{0}T", j + 1);
                        var query1 = from Item in ArrangementItem.AsEnumerable()
                                     where Item.Field<string>("KQID") == Staff.Rows[i]["KQID"].ToString()
                                     select new
                                     {
                                         Yesterday = Item.Field<string>(yesterday),
                                         Today = Item.Field<string>(today),
                                     };
                        foreach (var obj in query1)
                        {
                            PersonShift.Rows[0]["ID"] = obj.Yesterday;
                            PersonShift.Rows[0]["PD"] = "0";
                            PersonShift.Rows[1]["ID"] = obj.Today;
                            PersonShift.Rows[1]["PD"] = "1";
                        }
                    }

                    //将个人排班添加上下班时间和跨天说明
                    var query2 = from personshift in PersonShift.AsEnumerable()
                                 join workshift in WorkShift.AsEnumerable().DefaultIfEmpty()
                                 on personshift.Field<string>("ID") equals workshift.Field<int>("ID").ToString()
                                 select new
                                 {
                                     ID = personshift.Field<string>("ID"),
                                     Name = workshift.Field<string>("NAME"),
                                     PD = personshift.Field<string>("PD"),
                                     SBSJ = workshift.Field<string>("SBSJ"),
                                     XBSJ = workshift.Field<string>("XBSJ"),
                                     WorkDay = workshift.Field<double>("GZR").ToString(),
                                     KT = workshift.Field<int>("KT").ToString(),
                                 };

                    PersonShiftAll.Clear();
                    foreach (var obj in query2)
                    {
                        PersonShiftAll.Rows.Add(obj.PD, obj.ID, obj.Name, obj.SBSJ, obj.XBSJ, obj.WorkDay, obj.KT);
                    }

                    string[] ResultAll = Result(Record_Person, PersonShiftAll, Date);
                    AttendanceResult.Rows[i][j + 2] = ResultAll[0];
                    WorkDayCount[i][j] = Convert.ToDouble(ResultAll[1]);
                }
            }
            gridControl2.DataSource = AttendanceResult;
            bandedGridView2.BestFitColumns();
        }

        public void DataCollect(DateTime StartDate, int Timespan)
        {
            AttendanceCollect.Clear();
            int normal;
            int late;
            int absent;//全天未签
            int rest;//休假
            int leaveEarly;//早退
            int morning;//上午未签
            int afternoon;//下午未签

            double workDay;//出勤
            for (int i = 0; i < Staff.Rows.Count; i++)
            {
                AttendanceCollect.Rows.Add(new object[] { });
                normal = 0;
                late = 0;
                rest = 0;
                leaveEarly = 0;
                morning = 0;
                afternoon = 0;
                workDay = 0;
                for (int j = 0; j <= Timespan; j++)
                {
                    workDay = workDay + WorkDayCount[i][j];

                    if (AttendanceResult.Rows[i][j + 2].ToString() == "正常")
                    {
                        normal = normal + 1;
                        continue;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString() == "上下班未签")
                    {
                        morning = morning + 1;
                        afternoon = afternoon + 1;
                        continue;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString().IndexOf("休") > -1)
                    {
                        rest = rest + 1;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString().IndexOf("迟到") > -1)
                    {
                        late = late + 1;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString().IndexOf("早退") > -1)
                    {
                        leaveEarly = leaveEarly + 1;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString().IndexOf("上班未签") > -1)
                    {
                        morning = morning + 1;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString().IndexOf("下班未签") > -1)
                    {
                        afternoon = afternoon + 1;
                    }



                }
                AttendanceCollect.Rows[i]["KQID"] = AttendanceResult.Rows[i]["KQID"];
                AttendanceCollect.Rows[i]["YGXM"] = AttendanceResult.Rows[i]["YGXM"];
                AttendanceCollect.Rows[i]["TotalDays"] = (Timespan + 1).ToString();
                AttendanceCollect.Rows[i]["Normal"] = normal.ToString();
                AttendanceCollect.Rows[i]["Rest"] = rest.ToString();
                AttendanceCollect.Rows[i]["Late"] = late.ToString();
                AttendanceCollect.Rows[i]["LeaveEarly"] = leaveEarly.ToString();
                AttendanceCollect.Rows[i]["Morning"] = morning.ToString();
                AttendanceCollect.Rows[i]["Afternoon"] = afternoon.ToString();
                AttendanceCollect.Rows[i]["WorkDay"] = workDay.ToString();

                //工作年限计算
                if (string.IsNullOrEmpty(Staff.Rows[i]["RZSJ"].ToString()))
                {
                    AttendanceCollect.Rows[i]["WorkYear"] = "";
                } else
                {
                    DateTime EntryDate = Convert.ToDateTime(Staff.Rows[i]["RZSJ"].ToString());
                    DateTime NowDate = Convert.ToDateTime(string.Format("{0}-{1}-01", comboBoxYear.Text.Substring(0, 4), comboBoxMonth.Text.Substring(0, comboBoxMonth.Text.IndexOf("月"))));
                    int TotalMonth = NowDate.Year * 12 + NowDate.Month - EntryDate.Year * 12 - EntryDate.Month;
                    int year = TotalMonth / 12;
                    int month = TotalMonth % 12;
                    string workYear = "";
                    if (year == 0)
                    {
                        workYear = string.Format("{0}个月", month);
                    }
                    else if (month == 0)
                    {
                        workYear = string.Format("{0}年整", year);
                    }
                    else
                    {
                        workYear = string.Format("{0}年{1}个月", year, month);
                    }

                    AttendanceCollect.Rows[i]["WorkYear"] = workYear;
                }
            }
            gridControl3.DataSource = AttendanceCollect;
            gridView3.BestFitColumns();

        }

        private string[] Result(DataTable Record_Person, DataTable PersonShiftAll, string Date)
        {
            StringBuilder result = new StringBuilder();
            double workDay = 0;
            string[] resultAll = new string[2];
            //过滤，放宽迟到和早退的时间
            int late = 0;
            int leaveEarly = 0;
            for (int i = 0; i < Filter.Rows.Count; i++)
            {
                if (Filter.Rows[i]["Name"].ToString() == "Late")
                {
                    late = Convert.ToInt32(Filter.Rows[i]["Time"].ToString());
                    continue;
                }

                if (Filter.Rows[i]["Name"].ToString() == "LeaveEarly")
                {
                    leaveEarly = Convert.ToInt32(Filter.Rows[i]["Time"].ToString());
                    continue;
                }
            }
            //昨日、今日、明日的记录
            DataTable Record_Yesterday = new DataTable();
            DataTable Record_Today = new DataTable();
            DataTable Record_Tomorrow = new DataTable();
            Record_Yesterday.Columns.Add("Time", typeof(DateTime));
            Record_Yesterday.Columns.Add("Source", typeof(string));
            Record_Today.Columns.Add("Time", typeof(DateTime));
            Record_Today.Columns.Add("Source", typeof(string));
            Record_Tomorrow.Columns.Add("Time", typeof(DateTime));
            Record_Tomorrow.Columns.Add("Source", typeof(string));


            for (int i = -1; i <= 1; i++)
            {
                var query = from rec in Record_Person.AsEnumerable()
                            where Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(i)) >= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(i + 1)) < 0
                            select new
                            {
                                Time = rec.Field<string>("Time"),
                                Source = rec.Field<string>("Source"),
                            };

                foreach (var obj in query)
                {
                    if (i == -1)
                    {
                        Record_Yesterday.Rows.Add(Convert.ToDateTime(obj.Time), obj.Source);
                    }

                    if (i == 0)
                    {
                        Record_Today.Rows.Add(Convert.ToDateTime(obj.Time), obj.Source);
                    }

                    if (i == 1)
                    {
                        Record_Tomorrow.Rows.Add(Convert.ToDateTime(obj.Time), obj.Source);
                    }
                }
            }


            for (int i = 0; i < PersonShiftAll.Rows.Count; i++)
            {
                //昨日考勤判断
                if (PersonShiftAll.Rows[i]["PD"].ToString() == "0")
                {

                    if (PersonShiftAll.Rows[i]["KT"].ToString() == "0")
                    {
                        //不跨天
                        continue;
                    }
                    else
                    {
                        //跨天，只判断下班时间
                        DateTime XBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["XBSJ"]).ToShortTimeString());
                        result.Append(JudgeOffWork(Record_Tomorrow, Record_Today, XBSJ, leaveEarly));
                    }
                }

                //今日考勤判断
                if (PersonShiftAll.Rows[i]["PD"].ToString() == "1")
                {
                    if (PersonShiftAll.Rows[i]["KT"].ToString() == "0")
                    {
                        //不跨天，判断上下班时间

                        //上下班时间都为空
                        if (PersonShiftAll.Rows[i]["SBSJ"].ToString() == "" && PersonShiftAll.Rows[i]["XBSJ"].ToString() == "")
                        {
                            if (Convert.ToDouble(PersonShiftAll.Rows[i]["WorkDay"].ToString()) > 0)
                            {
                                //无论昨天明天如何排班，今天就是无条件正常
                                result.Clear();
                                result.Append("正常");
                                continue;
                            }else
                            {
                                //出勤记为0天的
                                if (Record_Today.Rows.Count == 0)
                                {
                                    result.Append("/休");
                                    continue;
                                }

                                for (int j = 0; j < Record_Today.Rows.Count; j++)
                                {
                                    //上班时间是20:00以后的，不算加班
                                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Today.Rows[j]["Time"]).ToShortTimeString());
                                    if ((Convert.ToDateTime("20:00") - record).TotalMinutes > 0)
                                    {
                                        result.Append("/休(有签到)");
                                        break;
                                    }

                                    if (j == Record_Today.Rows.Count - 1)
                                    {
                                        result.Append("/休");
                                        continue;
                                    }
                                }
                                continue;
                            }
                          
                        }

                        //上班时间
                        if (PersonShiftAll.Rows[i]["SBSJ"].ToString() != "")
                        {
                            DateTime SBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["SBSJ"]).ToShortTimeString());
                            result.Append(JudgeWork(Record_Yesterday, Record_Today, Record_Tomorrow, SBSJ, late));
                        }

                        //下班时间
                        if (PersonShiftAll.Rows[i]["XBSJ"].ToString() != "")
                        {
                            DateTime XBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["XBSJ"]).ToShortTimeString());
                            result.Append(JudgeOffWork(Record_Tomorrow, Record_Today, XBSJ, leaveEarly));
                        }

                    }
                    else
                    {
                        //跨天，只判断上班时间
                        DateTime SBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["SBSJ"]).ToShortTimeString());
                        result.Append(JudgeWork(Record_Yesterday, Record_Today, Record_Tomorrow, SBSJ, late));
                    }
                }
            }

            //融合
            switch (result.ToString())
            {
                case "/正常/正常": result.Clear(); result.Append("正常"); break;
                case "/正常/正常/正常": result.Clear(); result.Append("正常"); break;
                case "/上班未签/下班未签": result.Clear(); result.Append("上下班未签"); break;
                case "/下班未签/上班未签": result.Clear(); result.Append("上下班未签"); break;
                case "/下班未签/上班未签/下班未签": result.Clear(); result.Append("上下班未签"); break;
            }

            //去除掉正常的考勤结果

            //跨天班第二天为休的情况
            if (result.ToString().IndexOf("/休(有签到)") >= 1)
            {
                string str = result.ToString().Replace("/休(有签到)", "/休");
                result.Clear();
                result.Append(str);
            }
            
            //去掉一个或者两个正常
            if (result.ToString().IndexOf("/正常") >= 0 && result.ToString()!="/正常")
            {
                string str = result.ToString().Replace("/正常", "");
                result.Clear();
                result.Append(str);
            }


            if (result.ToString().IndexOf("/") == 0)
            {
                string str = result.ToString().Remove(0, 1);
                result.Clear();
                result.Append(str);
            }

            //计算出勤
            for (int i = 0; i < PersonShiftAll.Rows.Count; i++)
            {
                //如果是昨天的排班，则略过
                if (PersonShiftAll.Rows[i]["PD"].ToString() == "0")
                {
                    continue;
                }
                else
                {
                    //不跨天的计算
                    if (PersonShiftAll.Rows[i]["KT"].ToString() == "0")
                    {

                        if (PersonShiftAll.Rows[i]["SBSJ"].ToString() == "" && PersonShiftAll.Rows[i]["XBSJ"].ToString() == "")
                        {
                            //记为出勤不为0的，不看签到
                            if (Convert.ToDouble(PersonShiftAll.Rows[i]["WorkDay"].ToString()) > 0)
                            {
                                workDay = Convert.ToDouble(PersonShiftAll.Rows[i]["WorkDay"].ToString());
                            }
                            else
                            {
                                workDay = 0;
                            }
                          
                        } else
                        {
                            //上下班时间有一个不为空
                            if (Record_Today.Rows.Count > 0)
                            {
                                workDay = Convert.ToDouble(PersonShiftAll.Rows[i]["WorkDay"].ToString());
                            }
                            else
                            {
                                workDay = 0;
                            }
                        }

                    } else
                    {
                        //跨天的计算
                        if (Record_Today.Rows.Count > 0)
                        {
                            workDay = Convert.ToDouble(PersonShiftAll.Rows[i]["WorkDay"].ToString());
                        }
                        else
                        {
                            if (Record_Tomorrow.Rows.Count > 0)
                            {
                                workDay = Convert.ToDouble(PersonShiftAll.Rows[i]["WorkDay"].ToString());
                            }
                            else
                            {
                                workDay = 0;
                            }
                        }
                    }
                }
            }

            resultAll = new string[] { result.ToString(), workDay.ToString() };

            return resultAll;
        }
        /// <summary>
        /// 判断下班
        /// </summary>
        private string JudgeOffWork(DataTable Record_Tomorrow, DataTable Record_Today, DateTime XBSJ, int leaveEarly)
        {
            DateTime time1 = Convert.ToDateTime("21:00");
            DateTime XBTime = new DateTime();

            if (XBSJ.AddMinutes(-leaveEarly).CompareTo(XBSJ) <= 0)
            {
                XBTime = XBSJ.AddMinutes(-leaveEarly);
            }
            else
            {
                XBTime = Convert.ToDateTime("00:01");
            }

            if (XBTime.CompareTo(time1) <= 0)
            {
                if (Record_Today.Rows.Count == 0)
                {
                    return "/下班未签";
                }
                Record_Today.DefaultView.Sort = "Time desc";
                Record_Today = Record_Today.DefaultView.ToTable();

                for (int j = 0; j < Record_Today.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Today.Rows[j]["Time"]).ToShortTimeString());
                    double subtract = (record - XBTime).TotalMinutes;
                    if (subtract >= 0)
                    {
                        return "/正常";
                    }
                    else if (subtract < 0 && subtract > -120)
                    {
                        return "/早退";
                    }
                    else if (j == Record_Today.Rows.Count - 1)
                    {
                        return "/下班未签";
                    }
                }
            } else
            {
                //这个点后下班的，可能会用到明天的数据
                for (int j = 0; j < Record_Today.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Today.Rows[j]["Time"]).ToShortTimeString());
                    double subtract = (record - XBTime).TotalMinutes;
                    if (subtract >= 0)
                    {
                        return "/正常";
                    }
                    else if (subtract < 0 && subtract > -90)
                    {
                        return "/早退";
                    }
                }

                for (int j = 0; j < Record_Tomorrow.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Tomorrow.Rows[j]["Time"]).ToShortTimeString());
                    DateTime value = Convert.ToDateTime("03:00");
                    if (record.CompareTo(value) <= 0)
                    {
                        return "/正常";
                    }
                }
            }

            return "/下班未签";

        }
        /// <summary>
        /// 判断上班
        /// </summary>
        private string JudgeWork(DataTable Record_Yesterday, DataTable Record_Today, DataTable Record_Tomorrow, DateTime SBSJ, int late)
        {
            DateTime time1 = Convert.ToDateTime("03:00");
            DateTime time2 = Convert.ToDateTime("22:00");
            DateTime SBTime = new DateTime();

            if (SBSJ.AddMinutes(late).CompareTo(SBSJ) >= 0)
            {
                SBTime = SBSJ.AddMinutes(late);
            } else
            {
                SBTime = Convert.ToDateTime("23:59");
            }

            if (SBTime.CompareTo(time1) >= 0 && SBTime.CompareTo(time2) <= 0)
            {
                if (Record_Today.Rows.Count == 0)
                {
                    return "/上班未签";
                }

                for (int j = 0; j < Record_Today.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Today.Rows[j]["Time"]).ToShortTimeString());
                    double subtract = (SBTime - record).TotalMinutes;
                    if (subtract >= 0 && subtract <= 180)
                    {
                        return "/正常";
                    }
                    else if (subtract < 0 && subtract > -120)
                    {
                        return "/迟到";
                    }
                    else if (j == Record_Today.Rows.Count - 1)
                    {
                        return "/上班未签";
                    }
                }
            }
            else if (SBTime.CompareTo(time1) < 0)
            {
                for (int j = 0; j < Record_Today.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Today.Rows[j]["Time"]).ToShortTimeString());
                    double subtract = (SBTime - record).TotalMinutes;
                    if (record.CompareTo(SBTime) <= 0)
                    {
                        return "/正常";
                    }
                    else if (subtract < 0 && subtract >= -60)
                    {
                        return "/迟到";
                    }
                }

                for (int j = 0; j < Record_Yesterday.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Yesterday.Rows[j]["Time"]).ToShortTimeString());
                    DateTime value = Convert.ToDateTime("22:30");
                    if (record.CompareTo(value) >= 0)
                    {
                        return "/正常";
                    }
                }
            } else if (SBTime.CompareTo(time2) > 0)
            {
                for (int j = 0; j < Record_Today.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Today.Rows[j]["Time"]).ToShortTimeString());
                    double subtract = (SBTime - record).TotalMinutes;
                    if (record.CompareTo(SBTime) <= 0)
                    {
                        return "/正常";
                    }
                    else if (subtract < 0 && subtract > -60)
                    {
                        return "/迟到";
                    }
                }

                for (int j = 0; j < Record_Tomorrow.Rows.Count; j++)
                {
                    DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Tomorrow.Rows[j]["Time"]).ToShortTimeString());
                    DateTime value = Convert.ToDateTime("02:00");
                    if (record.CompareTo(value) <= 0)
                    {
                        return "/迟到";
                    }
                }
            }

            return "/上班未签";
        }
        private string Week(DateTime Day)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return weekdays[Convert.ToInt32(Day.DayOfWeek)];
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SearchDepartment();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            bandedGridView2.Columns.Clear();
            bandedGridView2.Bands.Clear();
            gridControl2.DataSource = null;
            gridControl3.DataSource = null;
            gridControl4.DataSource = null;
            gridView4.Columns.Clear();
        }


        private void bandedGridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                string name = Week(DateTime.Parse(e.Column.FieldName));
                if (name == "周六" || name == "周日")
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                }
            }
            catch { }

            if (e.Column.FieldName != "YGXM")
            {
                if (e.CellValue.ToString() == "休")
                {
                    e.Appearance.ForeColor = Color.DarkGreen;
                    return;
                }

                if (e.CellValue.ToString() == "休(有签到)")
                {
                    e.Appearance.ForeColor = Color.DarkMagenta;
                    return;
                }

                if (e.CellValue.ToString().IndexOf("迟到")>=0)
                {
                    e.Appearance.ForeColor = Color.Red;
                    return;
                }

                if (e.CellValue.ToString().IndexOf("早退") >= 0)
                {
                    e.Appearance.ForeColor = Color.Red;
                    return;
                }

                if (e.CellValue.ToString().IndexOf("未签") >= 0)
                {
                    e.Appearance.ForeColor = Color.Red;
                    return;
                }else
                {
                    e.Appearance.ForeColor = Color.Blue;
                }

            }


        }

        private void ButtonOrignData_Click(object sender, EventArgs e)
        {
            try
            {
                OrignData form = new OrignData();
                form.Record_DKJ = Record_DKJ.Copy();
                form.Staff = Staff_Orign.Copy();
                form.Show(this);
            }catch
            {
                MessageBox.Show("本机没有安装相应的软件，请联系信息部安装！");
            }
            
        }

        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //还要考虑点击名字的时候
            Record_Person.Clear();
            PersonShift.Clear();
            PersonShift.Rows.Add();
            PersonShift.Rows.Add();
            PersonShiftAll.Clear();
            int Row = 0;
            try
            {
                string KQID = bandedGridView2.GetFocusedRowCellDisplayText("KQID").ToString();
                for (int i = 0; i < ArrangementItem.Rows.Count; i++)
                {
                    if (ArrangementItem.Rows[i]["KQID"].ToString() == KQID)
                    {
                        Row = i;
                        break;
                    }
                }
                //查找当天的考勤记录
                string Name = bandedGridView2.GetFocusedRowCellDisplayText("YGXM").ToString();
                string Date = bandedGridView2.FocusedColumn.Caption;
                var query = (from rec in Record_Dep.AsEnumerable()
                            where rec.Field<string>("ID") == KQID && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date)) >= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(1)) < 0
                            select new
                            {
                                Time = rec.Field<string>("Time"),
                                Source = rec.Field<string>("Source"),
                            }).Distinct();
                foreach (var obj in query)
                {
                    Record_Person.Rows.Add(obj.Time, obj.Source);
                }

                int day = Convert.ToDateTime(Date).Day;
                string today = "D" + (day) + "T";
                string yesterday = "D" + (day - 1) + "T";

                if (day == 1)
                {
                    //第一天的排班
                    var query1 = from Item in ArrangementItem.AsEnumerable()
                                 where Item.Field<string>("KQID") == KQID
                                 select new
                                 {
                                     Today = Item.Field<string>(today),
                                 };

                    var query_lastday = from lastday in ArrangementItem_LastDay.AsEnumerable()
                                        where lastday.Field<string>("KQID") == KQID
                                        select new
                                        {
                                            yesterday = lastday.Field<string>("LastDay"),
                                        };

                    foreach (var obj in query_lastday)
                    {
                        PersonShift.Rows[0]["ID"] = obj.yesterday;
                        PersonShift.Rows[0]["PD"] = "0";
                    }

                    foreach (var obj in query1)
                    {
                        PersonShift.Rows[1]["ID"] = obj.Today;
                        PersonShift.Rows[1]["PD"] = "1";
                    }
                }
                else
                {
                    var query1 = from Item in ArrangementItem.AsEnumerable()
                                 where Item.Field<string>("KQID") == KQID
                                 select new
                                 {
                                     Yesterday = Item.Field<string>(yesterday),
                                     Today = Item.Field<string>(today),
                                 };
                    foreach (var obj in query1)
                    {
                        PersonShift.Rows[0]["ID"] = obj.Yesterday;
                        PersonShift.Rows[0]["PD"] = "0";
                        PersonShift.Rows[1]["ID"] = obj.Today;
                        PersonShift.Rows[1]["PD"] = "1";
                    }
                }

                //将个人排班添加上下班时间和跨天说明
                var query2 = from personshift in PersonShift.AsEnumerable()
                             join workshift in WorkShift.AsEnumerable().DefaultIfEmpty()
                             on personshift.Field<string>("ID") equals workshift.Field<int>("ID").ToString()
                             select new
                             {
                                 ID = personshift.Field<string>("ID"),
                                 Name = workshift.Field<string>("NAME"),
                                 PD = personshift.Field<string>("PD"),
                                 SBSJ = workshift.Field<string>("SBSJ"),
                                 XBSJ = workshift.Field<string>("XBSJ"),
                                 WorkDay = workshift.Field<double>("GZR").ToString(),
                                 KT = workshift.Field<int>("KT").ToString(),
                             };

                PersonShiftAll.Clear();
                foreach (var obj in query2)
                {
                    PersonShiftAll.Rows.Add(obj.PD, obj.ID, obj.Name, obj.SBSJ, obj.XBSJ, obj.WorkDay, obj.KT);
                }

                PersonData form = new PersonData();
                form.PersonRecord = Record_Person.Copy();
                form.PersonShift = PersonShiftAll.Copy();
                form.WorkShift = WorkShift.Copy();
                form.name = Name;
                form.workDay = WorkDayCount[Row][day - 1];
                form.Date = Date;
                form.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void FromExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string FileName = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx";//过滤文件类型
            ofd.RestoreDirectory = true; //记忆上次浏览路径            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileName = ofd.FileName;
            }
            else
            {
                return;
            }
            //FileName = "C:\\Users\\Administrator\\Desktop\\考勤.xlsx";

            string FileExtension = "";
            DataTable dtExcel = new DataTable(); //数据表   
            DataSet ds = new DataSet();
            OleDbConnection ExcelConn = null;

            try
            {
                FileExtension = System.IO.Path.GetExtension(FileName);
                switch (FileExtension)
                {
                    case ".xls": ExcelConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\""); break;
                    case ".xlsx": ExcelConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";" + "Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\""); break;
                    default: ExcelConn = null; break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            try
            {
                ExcelConn.Open();
                //获取Excel中所有Sheet表的信息
                DataTable schemaTable = ExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //获取Excel的第一个Sheet表名
                string tableName = schemaTable.Rows[0][2].ToString().Trim();
                string strSql = "select * from [" + tableName + "]";
                //获取Excel指定Sheet表中的信息
                OleDbDataAdapter myData = new OleDbDataAdapter(strSql, ExcelConn);
                myData.Fill(ds, tableName);//填充数据
                dtExcel = ds.Tables[tableName];
                ExcelConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            if (dtExcel.Rows[0][0].ToString() != "考勤号" || dtExcel.Rows[0][1].ToString() != "姓名"
                || dtExcel.Rows[0][2].ToString() != "打卡时间" || dtExcel.Rows[0][3].ToString() != "来源")
            {
                MessageBox.Show("导入的文件错误！");
                return;
            }

            DataView dVExcel = dtExcel.DefaultView;
            Staff_Orign = dVExcel.ToTable(true, "F1", "F2");
            Staff_Orign.Columns["F1"].ColumnName = "ID";
            Staff_Orign.Columns["F2"].ColumnName = "Name";
            Staff_Orign.Rows.RemoveAt(0);

            //移除最后一行空行
            if (dtExcel.Rows[dtExcel.Rows.Count - 1][2].ToString() == "")
            {
                dtExcel.Rows.RemoveAt(dtExcel.Rows.Count - 1);
            }

            try
            {
                for (int i = 1; i < dtExcel.Rows.Count; i++)
                {
                    Convert.ToDateTime(dtExcel.Rows[i][2].ToString());
                    Record_DKJ.Rows.Add(new object[] { dtExcel.Rows[i][0].ToString(), dtExcel.Rows[i][2].ToString(), dtExcel.Rows[i][3].ToString() });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                Record_DKJ.Clear();
                return;
            }

            TimeSort();
            ButtonOrignData.Enabled = true;
            MessageBox.Show("导入成功，请点击‘查询计算’查看考勤结果！");
        }

        public void ButtonExport_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabPage1")
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "Excel文件(*.xlsx)|*.xlsx";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DevExpress.XtraPrinting.XlsxExportOptions options = new DevExpress.XtraPrinting.XlsxExportOptions();
                        options.ShowGridLines = true;
                        options.RawDataMode = true;
                        string path = sf.FileName.ToString();
                        gridControl2.ExportToXlsx(path, options);
                        MessageBox.Show("导出成功！");
                    }
                    catch { }
                    if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        return;
                    try
                    {
                        System.Diagnostics.Process.Start(sf.FileName);
                    }
                    catch { }
                }

            }
            else if (tabControl1.SelectedTab.Name == "tabPage2")
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "Excel文件(*.xlsx)|*.xlsx";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string path = sf.FileName.ToString();
                        gridControl3.ExportToXlsx(path);
                        MessageBox.Show("导出成功！");
                    }
                    catch { }

                    if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        return;
                    try
                    {
                        System.Diagnostics.Process.Start(sf.FileName);
                    }
                    catch { }
                }
            }
            else if (tabControl1.SelectedTab.Name == "tabPage3")
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "Excel文件(*.xlsx)|*.xlsx";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string path = sf.FileName.ToString();
                        gridControl4.ExportToXlsx(path);
                        MessageBox.Show("导出成功！");
                    }
                    catch { }

                    if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        return;
                    try
                    {
                        System.Diagnostics.Process.Start(sf.FileName);
                    }
                    catch { }
                }
            }
        }

        private void ButtonFilter_Click(object sender, EventArgs e)
        {
            AttendanceFilter form = new AttendanceFilter();
            form.Show();
        }

        public void searchControl2_TextChanged(object sender, EventArgs e)
        {
            if (AttendanceResult.Rows.Count > 0)
            {
                AttendanceResult.DefaultView.RowFilter = string.Format("YGXM like '%{0}%'", searchControl2.Text);
                AttendanceCollect.DefaultView.RowFilter = string.Format("YGXM like '%{0}%'", searchControl2.Text);
                AttendanceShiftCollect.DefaultView.RowFilter = string.Format("YGXM like '%{0}%'", searchControl2.Text);
            }

        }

        private void 修改为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AttendanceAlter form = new AttendanceAlter();
                form.name = bandedGridView2.GetFocusedRowCellValue("YGXM").ToString();
                form.Date = bandedGridView2.FocusedColumn.FieldName;
                form.Result = bandedGridView2.GetFocusedRowCellValue(bandedGridView2.FocusedColumn.Caption).ToString();
                for (int i = 0; i < AttendanceResult.Rows.Count; i++)
                {
                    if (bandedGridView2.GetFocusedRowCellValue("KQID").ToString() == AttendanceResult.Rows[i]["KQID"].ToString())
                    {
                        form.Row = i;
                        break;
                    }

                }
                form.StartDate = StartDate;
                form.Timespan = Timespan.Days;
                form.Column = Convert.ToDateTime(form.Date).Day - 1;
                form.WorkDay = WorkDayCount[form.Row][form.Column];
                form.Show(this);
            }
            catch { }

        }

        private void 修改全列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AttendanceAlter form = new AttendanceAlter();
                form.name = gridView1.GetFocusedRowCellValue("BMMC").ToString();
                form.Date = bandedGridView2.FocusedColumn.Caption;
                form.Row = bandedGridView2.GetDataSourceRowIndex(bandedGridView2.FocusedRowHandle);
                form.StartDate = StartDate;
                form.Timespan = Timespan.Days;
                form.Column = Convert.ToDateTime(form.Date).Day - 1;
                form.AlterColumn = true;
                form.Show(this);
            }
            catch { }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                gridControl2_MouseDoubleClick(null, null);
            }
            catch { }
        }

        private void bandedGridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView3_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void test_Click(object sender, EventArgs e)
        {
            LoadingForm form = new LoadingForm();
            form.Show(this);
        }

        private void FromDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataOpeation.SaveToDB form = new DataOpeation.SaveToDB();
            form.Authority_Attendance_DelDB = Authority_Attendance_DelDB;
            form.Show(this);
        }

        public void TimeSort()
        {
            if (Record_DKJ.Rows.Count == 0)
            {
                Record_startTime = "";
                Record_stopTime = "";
                return;
            }

            DataTable Record_DKJ_copy = new DataTable();
            Record_DKJ_copy.Columns.Add("ID", typeof(string));
            Record_DKJ_copy.Columns.Add("Time", typeof(DateTime));
            Record_DKJ_copy.Columns.Add("Source", typeof(string));

            for (int i = 0; i < Record_DKJ.Rows.Count; i++)
            {
                Record_DKJ_copy.Rows.Add(new object[] { Record_DKJ.Rows[i]["ID"], Convert.ToDateTime(Record_DKJ.Rows[i]["Time"]).ToString("yyyy-MM-dd HH:mm:ss"), Record_DKJ.Rows[i]["Source"] });
            }

            Record_DKJ_copy.DefaultView.Sort = "Time";
            Record_DKJ_copy = Record_DKJ_copy.DefaultView.ToTable();

            Record_startTime = Convert.ToDateTime(Record_DKJ_copy.Rows[0]["Time"].ToString()).ToString("yyyy-MM-dd");
            Record_stopTime = Convert.ToDateTime(Record_DKJ_copy.Rows[Record_DKJ.Rows.Count - 1]["Time"].ToString()).ToString("yyyy-MM-dd");
        }


        private bool JudgeTime()
        {
            string startTime = StartDate.AddDays(-1).ToString("yyyy-MM-dd");
            string timeNow = GlobalHelper.IDBHelper.GetServerDateTime();
            string yearNow = Convert.ToDateTime(timeNow).Year.ToString() + "年";
            string monthNow = Convert.ToDateTime(timeNow).Month.ToString() + "月";
            string stopTime = "";

            //如果查询本月的记录，则最终日期是今天
            if (yearNow == comboBoxYear.Text && monthNow == comboBoxMonth.Text)
            {
                stopTime = StopDate.ToString("yyyy-MM-dd");
            }
            else
            {
                stopTime = StartDate.AddMonths(1).ToString("yyyy-MM-dd");
            }
            

            //判断现有考勤记录中是否有要查这个月的考勤记录
            if (Record_startTime != "" && Convert.ToDateTime(Record_startTime).CompareTo(Convert.ToDateTime(startTime)) <= 0 && Convert.ToDateTime(Record_stopTime).CompareTo(Convert.ToDateTime(stopTime)) >=0)
            {
                return true;
            }else
            {
                if (MessageBox.Show(string.Format("考勤记录不完整，是否需要从数据库中下载{0}{1}的考勤记录？", comboBoxYear.Text, comboBoxMonth.Text), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }
            }

            //下载一个月考勤记录
            DataTable Record_Month = new DataTable();
            try
            {
                string sql = string.Format("select ID,KQSJ,LY from KQ_JL_XB where KQSJ between '{0}' and '{1} 23:59:59' order by KQSJ", startTime,stopTime);
                Record_Month = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return false;
            }

            //分析所下的数据是否完整
            if (Record_Month.Rows.Count == 0)
            {
                return false;
            }
          
            //取头尾的时间分析
            DateTime Record_Month_startTime = Convert.ToDateTime(Convert.ToDateTime(Record_Month.Rows[0]["KQSJ"]).ToString("yyyy-MM-dd"));
            DateTime Record_Month_stopTime = Convert.ToDateTime(Convert.ToDateTime(Record_Month.Rows[Record_Month.Rows.Count - 1]["KQSJ"]).ToString("yyyy-MM-dd"));

            for (int i = 0; i < Record_Month.Rows.Count; i++)
            {
                Record_DKJ.Rows.Add(new object[] { Record_Month.Rows[i]["ID"],Convert.ToDateTime(Record_Month.Rows[i]["KQSJ"]).ToString("yyyy-MM-dd HH:mm:ss"), Record_Month.Rows[i]["LY"] });
            }
            //重新获取考勤数据的起始与结束时间
            TimeSort();

            if (Record_Month_startTime.CompareTo(Convert.ToDateTime(startTime))==0 && Record_Month_stopTime.CompareTo(Convert.ToDateTime(stopTime)) == 0)
            {
                return true;
            }else
            {
                return false;
            }

        }

        private void ShiftCollect()
        {
            Hashtable ShiftCount = new Hashtable();

            //检查有几个班次的类型，作为表的列名
            for (int i = 0; i < ArrangementItem.Rows.Count; i++)
            {
                //天数
                for (int j = 0; j <31; j++)
                {
                    if (ArrangementItem.Rows[i][j + 3].ToString() != "")
                    {
                        if (ShiftCount.Contains(ArrangementItem.Rows[i][j + 3]) == false)
                        {
                            for (int k = 0; k < WorkShift.Rows.Count; k++)
                            {
                                if (WorkShift.Rows[k]["ID"].ToString()== ArrangementItem.Rows[i][j + 3].ToString())
                                {
                                    ShiftCount.Add(ArrangementItem.Rows[i][j + 3], WorkShift.Rows[k]["NAME"]);
                                    break;
                                }
                            }                            
                        }
                    }
                }
            }

            GridColumn Staff_ID = new GridColumn();
            Staff_ID.Caption = "考勤号";
            Staff_ID.Name = "Staff_ID";
            Staff_ID.FieldName = "KQID";
            Staff_ID.Visible = false;
            Staff_ID.OptionsColumn.AllowEdit = false;
            AttendanceShiftCollect.Columns.Add("KQID", typeof(string));
            gridView4.Columns.Add(Staff_ID);

            GridColumn Staff_Name = new GridColumn();
            Staff_Name.Caption = "姓名";
            Staff_Name.Name = "Staff_Name";
            Staff_Name.Visible = true;
            Staff_Name.FieldName = "YGXM";
            Staff_Name.OptionsColumn.AllowEdit = false;
            AttendanceShiftCollect.Columns.Add("YGXM", typeof(string));
            gridView4.Columns.Add(Staff_Name);

            GridColumn Collect = new GridColumn();
            Collect.Caption = "总计";
            Collect.Name = "Collect";
            Collect.Visible = true;
            Collect.FieldName = "Collect";
            Collect.OptionsColumn.AllowEdit = false;
            AttendanceShiftCollect.Columns.Add("Collect", typeof(int));
            gridView4.Columns.Add(Collect);

            foreach (DictionaryEntry de in ShiftCount)
            {
                GridColumn Shift = new GridColumn();
                Shift.Caption = de.Value.ToString();
                Shift.Name = de.Value.ToString();
                Shift.Visible = true;
                Shift.FieldName = de.Key.ToString();
                Shift.OptionsColumn.AllowEdit = false;
                AttendanceShiftCollect.Columns.Add(Shift.FieldName, typeof(int));
                gridView4.Columns.Add(Shift);
            }

            for (int i = 0; i < ArrangementItem.Rows.Count; i++)
            {
                AttendanceShiftCollect.Rows.Add();
                AttendanceShiftCollect.Rows[i]["KQID"] = ArrangementItem.Rows[i]["KQID"];
                AttendanceShiftCollect.Rows[i]["YGXM"] = Staff.Rows[i]["YGXM"];
                for (int j = 0; j < 31; j++)
                {
                    if (ArrangementItem.Rows[i][j + 3].ToString() != "")
                    {
                       if (AttendanceShiftCollect.Rows[i][ArrangementItem.Rows[i][j + 3].ToString()].ToString() != "")
                        {
                            AttendanceShiftCollect.Rows[i][ArrangementItem.Rows[i][j + 3].ToString()] = Convert.ToInt32(AttendanceShiftCollect.Rows[i][ArrangementItem.Rows[i][j + 3].ToString()])+1;
                        }
                        else
                        {
                            AttendanceShiftCollect.Rows[i][ArrangementItem.Rows[i][j + 3].ToString()] = 1;
                        }
                    }

                    //if (j == 30)
                    //{
                    //    for (int k = 0; k < AttendanceShiftCollect.Columns.Count; k++)
                    //    {
                    //        if (AttendanceShiftCollect.Rows[i][k].ToString() == "")
                    //        {
                    //            AttendanceShiftCollect.Rows[i][k] = 0;
                    //        }
                    //    }
                    //}
                }
            }
            //计算总计
            for (int i = 0; i < AttendanceShiftCollect.Rows.Count; i++)
            {
                for (int j = 0; j < AttendanceShiftCollect.Columns.Count-3; j++)
                {
                    if (j == 0)
                    {
                        AttendanceShiftCollect.Rows[i]["Collect"] = 0;
                    }

                    if (AttendanceShiftCollect.Rows[i][j + 3].ToString()!="")
                    {
                        AttendanceShiftCollect.Rows[i]["Collect"] = Convert.ToInt32(AttendanceShiftCollect.Rows[i]["Collect"]) + Convert.ToInt32(AttendanceShiftCollect.Rows[i][j + 3]);
                    }
                    
                }
            }

                gridControl4.DataSource = AttendanceShiftCollect;
            gridView4.BestFitColumns();
        }

        private void gridView4_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView4_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            for (int i = 0; i < gridView4.Columns.Count; i++)
            {
                if (i!= e.Column.ColumnHandle)
                {
                    gridView4.Columns[i].AppearanceHeader.ForeColor = Color.Black;
                }else
                {
                    gridView4.Columns[e.Column.ColumnHandle].AppearanceHeader.ForeColor = Color.Red;
                }                
            }
            
        }

        private void gridView3_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //for (int i = 0; i < gridView3.Columns.Count; i++)
            //{
            //    if (i != e.Column.ColumnHandle)
            //    {
            //        gridView3.Columns[i].AppearanceHeader.ForeColor = Color.Black;
            //    }
            //    else
            //    {
            //        gridView3.Columns[e.Column.ColumnHandle].AppearanceHeader.ForeColor = Color.Red;
            //    }
            //}
        }

        private void gridView4_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void gridView4_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            for (int i = 0; i < gridView4.Columns.Count; i++)
            {
                if (i != e.Column.ColumnHandle)
                {
                    gridView4.Columns[i].AppearanceHeader.ForeColor = Color.Black;
                }
                else
                {
                    gridView4.Columns[e.Column.ColumnHandle].AppearanceHeader.ForeColor = Color.Red;
                }
            }
        }
    }
}
