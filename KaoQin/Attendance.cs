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
using DevExpress.XtraGrid.Views.BandedGrid;

namespace KaoQin
{
    public partial class Attendance : Form
    {
        zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机
        DataTable Machine = new DataTable();//机器信息
        DataTable Department = new DataTable();//部门
        public DataTable AttendanceResult = new DataTable();//考勤结果
        DataTable AttendanceCollect = new DataTable();//考勤汇总
        DataTable Staff = new DataTable();//整个部门的所有员工，包括在职离职
        public DataTable Staff_Orign = new DataTable();//打卡机的原始员工数据，包括考勤号和姓名
        DataTable WorkShift = new DataTable();//班次信息
        DataTable PBID = new DataTable();//排班ID
        DataTable ArrangementItem = new DataTable();//排班细表
        DataTable PersonShift = new DataTable();//个人单日的排班
        DataTable PersonShiftAll = new DataTable();//个人单日的排班的所有信息
        DataTable Filter = new DataTable();//过滤数据
        DataTable Record_Dep = new DataTable();//选定部门员工的打卡数据
        public DataTable Record_DKJ = new DataTable();//考勤机原始数据
        DataTable Record_Person = new DataTable();//个人单日打卡数据
        DateTime StartDate;
        DateTime StopDate;
        TimeSpan Timespan;
        bool HasDownload = false;//是否已下载数据
        public Attendance()
        {
            InitializeComponent();
        }

        private void ButtonDownload_Click(object sender, EventArgs e)
        {
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
            Record_DKJ.Clear();
            //读取考勤机数据
            LoadingForm form = new LoadingForm();
            form.ShowDialog(this);
            //string sql = "select ID,Machine,IP,Port,Password from KQ_Machine";

            //try
            //{
            //    Machine = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("错误1:" + ex.Message);
            //    return;
            //}

            //if (Machine.Rows.Count == 0)
            //{
            //    MessageBox.Show("没有可用设备！");
            //    return;
            //}


            //DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[0]["Password"].ToString()));
            //DKJ.Connect_Net(Machine.Rows[0]["IP"].ToString(), Convert.ToInt32(Machine.Rows[0]["Port"].ToString()));
            //bool bIsConnected = false;//判断设备是否可连接   
            //string sdwEnrollNumber = "";
            //string sName = "";
            //string sPassword = "";
            //int iPrivilege = 0;
            //bool bEnabled = false;

            //this.Text = "正在读取员工数据...";
            //DKJ.ReadAllUserID(0);
            //while (DKJ.SSR_GetAllUserInfo(0, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            //{
            //    int position = sName.IndexOf("\0");
            //    string name = sName.Substring(0, position);//过滤sName中多余字符
            //    Staff_Orign.Rows.Add(new object[] { sdwEnrollNumber, name });

            //}

            //int iMachineNumber = 0;
            //int VerifyMode = 0;
            //int InOutMode = 0;
            //int Year = 0;
            //int Month = 0;
            //int Day = 0;
            //int Hour = 0;
            //int Minute = 0;
            //int Second = 0;
            //int Workcode = 0;
            //string dwEnrollNumber = "";

            //this.Text = "正在下载考勤数据，请稍候...";
            //for (int i = 0; i < Machine.Rows.Count; i++)
            //{
            //    DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[i]["Password"].ToString()));
            //    bIsConnected = DKJ.Connect_Net(Machine.Rows[i]["IP"].ToString(), Convert.ToInt32(Machine.Rows[i]["Port"].ToString()));
            //    if (bIsConnected == false)
            //    {
            //        MessageBox.Show(string.Format("'{0}'无法连接！", Machine.Rows[i]["Machine"].ToString()));
            //        return;
            //    }
            //    int j = 0;
            //    DKJ.ReadAllGLogData(iMachineNumber);//read all the user information to the memory
            //    while (DKJ.SSR_GetGeneralLogData(iMachineNumber, out dwEnrollNumber, out VerifyMode, out InOutMode, out Year, out Month, out Day, out Hour, out Minute, out Second, ref Workcode))
            //    {
            //        j++;
            //        string time = string.Format("{0}-{1}-{2} {3}:{4}:{5}", Year, Month, Day, Hour, Minute, Second);
            //        string time1 = Convert.ToDateTime(time).ToString("yyyy-MM-dd  HH:mm:ss");
            //        Record_DKJ.Rows.Add(new object[] { dwEnrollNumber, time1, Machine.Rows[i]["Machine"].ToString() });
            //        this.Text = string.Format("正在读取{0}第{1}条数据", Machine.Rows[i]["Machine"].ToString(), j);
            //    }
            //}
            //下载数据完毕
            MessageBox.Show("数据已经下载完成，请选择相应部门并点击'查询计算'按钮查看考勤结果！");
            HasDownload = true;
            ButtonCal.Enabled = true;
            ButtonOrignData.Enabled = true;
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            UILocation();
            searchControl1.Properties.NullValuePrompt = "请输入部门名称";
            searchControl2.Properties.NullValuePrompt = "请输入姓名";

            ButtonCal.Enabled = false;
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
            PersonShiftAll.Columns.Add("SBSJ", typeof(string));
            PersonShiftAll.Columns.Add("XBSJ", typeof(string));
            PersonShiftAll.Columns.Add("KT", typeof(string));

            AttendanceCollect.Columns.Add("KQID", typeof(string));
            AttendanceCollect.Columns.Add("YGXM", typeof(string));
            AttendanceCollect.Columns.Add("TotalDays", typeof(string));
            AttendanceCollect.Columns.Add("Normal", typeof(string));
            AttendanceCollect.Columns.Add("Absent", typeof(string));
            AttendanceCollect.Columns.Add("Rest", typeof(string));
            AttendanceCollect.Columns.Add("Late", typeof(string));
            AttendanceCollect.Columns.Add("LeaveEarly", typeof(string));
            AttendanceCollect.Columns.Add("Morning", typeof(string));
            AttendanceCollect.Columns.Add("Afternoon", typeof(string));
            AttendanceCollect.Columns.Add("OverTime", typeof(string));
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
            ButtonDownload.Location = new Point(ButtonDownload.Location.X, height);
            ButtonFilter.Location = new Point(ButtonFilter.Location.X, height);
            ButtonImport.Location = new Point(ButtonImport.Location.X, height);
            ButtonExport.Location = new Point(ButtonExport.Location.X, height);
            searchControl2.Location= new Point(searchControl2.Location.X, (panelControl2.Height - searchControl2.Height) / 2);
            
        }
        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC,BMLB from KQ_BM where BMID>0";

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
            if (Department.Rows.Count>0)
            {
                Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl1.Text);
            }
            
        }

        private void ButtonCal_Click(object sender, EventArgs e)
        {
            gridControl2.DataSource = null;
            gridControl3.DataSource = null;
            bandedGridView2.Columns.Clear();
            bandedGridView2.Bands.Clear();
            AttendanceResult.Columns.Clear();
            searchControl2.Text = "";
            Record_Dep.Clear();

            try
            {
                string year = comboBoxYear.Text.Substring(0, 4);
                string month = comboBoxMonth.Text.Substring(0, comboBoxMonth.Text.IndexOf("月"));
                string timeNow = GlobalHelper.IDBHelper.GetServerDateTime();
                string yearNow = Convert.ToDateTime(timeNow).Year.ToString() + "年";
                string monthNow = Convert.ToDateTime(timeNow).Month.ToString() + "月";
                string startDate = Convert.ToDateTime(year + "-" + month + "-" + "1").ToString("yyyy-MM-dd");
                StartDate = Convert.ToDateTime(startDate);
                //判断是否大于本月
                if (StartDate.CompareTo(Convert.ToDateTime(timeNow))>0)
                {
                    MessageBox.Show("无法查看此月考勤结果！");
                    return;
                }
                //如果查询本月的记录，则最终日期是今天
                if (yearNow == comboBoxYear.Text && monthNow == comboBoxMonth.Text)
                {
                    StopDate =Convert.ToDateTime(Convert.ToDateTime(timeNow).ToString("yyyy-MM-dd"));
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


            //读取班次信息
            string sql2 = string.Format("select ID,NAME,SBSJ,XBSJ,KT from KQ_BC where LBID='{0}' or LBID='{1}'", "0",gridView1.GetFocusedRowCellValue("BMLB").ToString());
            try
            {
                WorkShift = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            //读取排班主表
            string sql3 = string.Format("select PBID from KQ_PB where YEAR='{0}' and MONTH='{1}'",comboBoxYear.Text, comboBoxMonth.Text);
            try
            {
                PBID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql3);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }


            //读取排班细表
            if (PBID.Rows.Count > 0)
            {
                string sql4 = string.Format("select * from KQ_PB_XB where BMID='{0}' and PBID='{1}'", gridView1.GetFocusedRowCellValue("BMID").ToString(), PBID.Rows[0][0].ToString());
                try
                {
                    ArrangementItem = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql4);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误4:" + ex.Message);
                    return;
                }
            }else
            {
                MessageBox.Show(string.Format("{0}在{1}{2}没有排班记录，无法查看考勤结果！",gridView1.GetFocusedRowCellValue("BMMC").ToString(),comboBoxYear.Text,comboBoxMonth.Text));
                return;
            }

            //读取过滤数据
            string sql = "select Name,Time from KQ_FILTER";
            try
            {
                Filter = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误5:" + ex.Message);
                return;
            }

            //读取员工信息            
            try
            {
                string sql1 = string.Format("select b.* from KQ_PB_XB a left join KQ_YG b on a.KQID=b.KQID where a.BMID='{0}' and a.PBID='{1}'", gridView1.GetFocusedRowCellValue("BMID").ToString(), PBID.Rows[0][0].ToString());
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误6:" + ex.Message);
                return;
            }

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
                Day_Column.FieldName = Day_Column.Caption;
                Day_Column.Name = Day_Column.Caption;
                Day_Column.Visible = true;
                Day_Column.OptionsColumn.AllowEdit = true;
                Day_band.Columns.Add(Day_Column);
                AttendanceResult.Columns.Add(Day_Column.Name);
                bandedGridView2.Columns.Add(Day_Column);
                day = day.AddDays(1);
            }

            //从原始打卡数据中得到指定部门的员工打卡数据
            var query = from rec in Record_DKJ.AsEnumerable()
                        join staff in Staff.AsEnumerable()
                        on rec.Field<string>("ID") equals staff.Field<string>("KQID")
                        where Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(StartDate) > 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(StopDate.AddDays(1)) < 0
                        select new
                        {
                            ID = rec.Field<string>("ID"),
                            Name = staff.Field<string>("YGXM"),
                            Time = rec.Field<string>("Time"),
                            Source = rec.Field<string>("Source"),
                        };

            foreach (var obj in query)
            {
                Record_Dep.Rows.Add(obj.ID, obj.Name, obj.Time, obj.Source);
            }

            //进行考勤明细分析
            AnalysisData(StartDate, Timespan.Days);
            //进行考勤汇总分析
            DataCollect(StartDate, Timespan.Days);
        }

        /// <summary>
        /// 考勤明细分析
        /// </summary>
        private void AnalysisData(DateTime StartDate, int Timespan)
        {
            AttendanceResult.Clear();

            for (int i = 0; i < Staff.Rows.Count; i++)
            {
                AttendanceResult.Rows.Add(new object[] { });
                //添加姓名与考勤号
                AttendanceResult.Rows[i]["KQID"] = Staff.Rows[i]["KQID"];
                AttendanceResult.Rows[i]["YGXM"] = Staff.Rows[i]["YGXM"];
                for (int j = 0; j <= Timespan ; j++)
                {
                                       
                    //查询当日考勤数据
                    Record_Person.Clear();
                    string KQID = Staff.Rows[i]["KQID"].ToString();
                    string Date = StartDate.AddDays(j).ToString("yyyy-MM-dd");
                    var query = from rec in Record_Dep.AsEnumerable()
                                where rec.Field<string>("ID") == KQID && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date)) >= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(1)) < 0
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
                        //第一天的排班
                        string yesterday = string.Format("D{0}T", j + 1);
                        var query1 = from Item in ArrangementItem.AsEnumerable()
                                     where Item.Field<string>("KQID") == Staff.Rows[i]["KQID"].ToString()
                                     select new
                                     {
                                         Today = Item.Field<string>(yesterday).ToString(),
                                     };
                        foreach (var obj in query1)
                        {
                            PersonShift.Rows[0]["ID"] = "";
                            PersonShift.Rows[0]["PD"] = "0";
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
                                     PD= personshift.Field<string>("PD"),
                                     SBSJ = workshift.Field<string>("SBSJ"),
                                     XBSJ = workshift.Field<string>("XBSJ"),
                                     KT = workshift.Field<int>("KT").ToString(),
                                 };

                    PersonShiftAll.Clear();
                    foreach (var obj in query2)
                    {
                        PersonShiftAll.Rows.Add(obj.PD,obj.ID, obj.SBSJ, obj.XBSJ, obj.KT);
                    }

                    AttendanceResult.Rows[i][j + 2] = Result(Record_Person, PersonShiftAll);
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
            int absent;
            int rest;
            int leaveEarly;
            int morning;
            int afternoon;
            int overTime;
            for (int i = 0; i < Staff.Rows.Count; i++)
            {
                AttendanceCollect.Rows.Add(new object[] {});
                normal = 0;
                late = 0;
                absent = 0;
                rest = 0;
                leaveEarly = 0;
                morning = 0;
                afternoon = 0;
                overTime = 0;
                for (int j=0;j<=Timespan; j++)
                {
                    if (AttendanceResult.Rows[i][j + 2].ToString() == "正常")
                    {
                        normal = normal + 1;
                        continue;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString() == "休")
                    {
                        rest = rest + 1;
                        continue;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString() == "加班")
                    {
                        overTime = overTime + 1;
                        continue;
                    }

                    if (AttendanceResult.Rows[i][j + 2].ToString() == "全天未签")
                    {
                        absent = absent + 1;
                        continue;
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
                AttendanceCollect.Rows[i]["TotalDays"] = (Timespan+1).ToString();
                AttendanceCollect.Rows[i] ["Normal"] = normal.ToString();
                AttendanceCollect.Rows[i]["Rest"] = rest.ToString();
                AttendanceCollect.Rows[i]["Absent"] = absent.ToString();
                AttendanceCollect.Rows[i]["Late"] =late.ToString();
                AttendanceCollect.Rows[i]["LeaveEarly"] = leaveEarly.ToString();
                AttendanceCollect.Rows[i]["Morning"] =morning.ToString();
                AttendanceCollect.Rows[i]["Afternoon"] =afternoon .ToString();
                AttendanceCollect.Rows[i]["OverTime"] = overTime.ToString();
                if (string.IsNullOrEmpty(Staff.Rows[i]["RZSJ"].ToString()))
                {
                    AttendanceCollect.Rows[i]["WorkYear"] = "";
                }else
                {
                    DateTime EntryDate = Convert.ToDateTime(Staff.Rows[i]["RZSJ"].ToString());
                    DateTime NowDate   = Convert.ToDateTime(string.Format("{0}-{1}-01", comboBoxYear.Text.Substring(0, 4), comboBoxMonth.Text.Substring(0, comboBoxMonth.Text.IndexOf("月"))));
                    int TotalMonth = NowDate.Year*12+NowDate.Month-EntryDate.Year*12-EntryDate.Month;
                    int year = TotalMonth / 12;
                    int month = TotalMonth % 12;
                    string workYear = "";
                    if (year == 0)
                    {
                        workYear = string.Format("{0}个月",month);
                    }
                    else if (month == 0)
                    {
                        workYear = string.Format("{0}年整", year);
                    }
                    else
                    {
                        workYear = string.Format("{0}年{1}个月", year, month);
                    }
                    
                    AttendanceCollect.Rows[i]["WorkYear"] =  workYear;
                }                                
            }
            gridControl3.DataSource = AttendanceCollect;
            gridView3.BestFitColumns();
            
        }

        private string Result(DataTable Record_Person, DataTable PersonShiftAll)
        {

            StringBuilder result = new StringBuilder();
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

            for (int i = 0; i < PersonShiftAll.Rows.Count; i++)
            {
                //昨日
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
                        for (int j = 0; j < Record_Person.Rows.Count; j++)
                        {
                            if (Record_Person.Rows.Count == 0)
                            {
                                result.Append("/下班未签");
                                break;
                            }

                            DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Person.Rows[j]["Time"]).ToShortTimeString());
                            DateTime XBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["XBSJ"]).AddMinutes(-leaveEarly).ToShortTimeString());
                            double subtract = (record - XBSJ).TotalMinutes;
                            if (subtract >= 0)
                            {
                                result.Append("/正常下班");
                                break;
                            }
                            else if (subtract < 0 && subtract > -60)
                            {
                                result.Append("/早退");
                                break;
                            }
                        }
                    }
                }

                //今日
                if (PersonShiftAll.Rows[i]["PD"].ToString() == "1")
                {
                    if (PersonShiftAll.Rows[i]["KT"].ToString() == "0")
                    {
                        //不跨天，判断上下班时间

                        if (PersonShiftAll.Rows[i]["SBSJ"].ToString()=="" && PersonShiftAll.Rows[i]["XBSJ"].ToString() == "" && Record_Person.Rows.Count == 0)
                        {
                            result.Append("休");
                            continue;
                        }

                        if (PersonShiftAll.Rows[i]["SBSJ"].ToString() == "" && PersonShiftAll.Rows[i]["XBSJ"].ToString() == "" && Record_Person.Rows.Count != 0)
                        {
                            result.Append("加班");
                            continue;
                        }

                        if ((PersonShiftAll.Rows[i]["SBSJ"].ToString() != "" || PersonShiftAll.Rows[i]["XBSJ"].ToString() != "") && Record_Person.Rows.Count == 0)
                        {
                            result.Append("全天未签");
                            continue;
                        }

                        //上班时间
                        if (PersonShiftAll.Rows[i]["SBSJ"].ToString() != "")
                        {
                            DateTime SBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["SBSJ"]).AddMinutes(late).ToShortTimeString());
                            for (int j = 0; j < Record_Person.Rows.Count; j++)
                            {
                                DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Person.Rows[j]["Time"]).ToShortTimeString());                                
                                double subtract = (SBSJ - record).TotalMinutes;
                                if (subtract >= 0 && subtract < 120)
                                {
                                    result.Append("/准点上班");
                                    break;
                                }
                                else if (subtract < 0 && subtract > -90)
                                {
                                    result.Append("/迟到");
                                    break;
                                }else if (j== Record_Person.Rows.Count-1)
                                {
                                    result.Append("/上班未签");
                                }
                            }
                        }

                        //下班时间
                        if (PersonShiftAll.Rows[i]["XBSJ"].ToString() != "")
                        {
                            for (int j = 0; j < Record_Person.Rows.Count; j++)
                            {
                                DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Person.Rows[j]["Time"]).ToShortTimeString());
                                DateTime XBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["XBSJ"]).AddMinutes(-leaveEarly).ToShortTimeString());
                                double subtract = (record - XBSJ).TotalMinutes;
                                if (subtract >= 0)
                                {
                                    result.Append("/正常下班");
                                    break;
                                }
                                else if (subtract < 0 && subtract > -90 )
                                {
                                    result.Append("/早退");
                                    break;
                                }
                                else if (j == Record_Person.Rows.Count-1)
                                {
                                    result.Append("/下班未签");
                                }
                            }
                        }

                    }
                    else
                    {
                        //跨天，只判断上班时间
                        if (Record_Person.Rows.Count == 0)
                        {
                            result.Append("/上班未签");
                            continue;
                        }
                        for (int j = 0; j < Record_Person.Rows.Count; j++)
                        {
                            DateTime record = Convert.ToDateTime(Convert.ToDateTime(Record_Person.Rows[j]["Time"]).ToShortTimeString());
                            DateTime SBSJ = Convert.ToDateTime(Convert.ToDateTime(PersonShiftAll.Rows[i]["SBSJ"]).AddMinutes(late).ToShortTimeString());
                            double subtract = (SBSJ - record).TotalMinutes;
                            if (subtract >=0 && subtract< 120)
                            {
                                result.Append("/准点上班");
                                break;
                            }
                            else if (subtract < 0 && subtract > -120)
                            {
                                result.Append("/迟到");
                                break;
                            }
                            else if (j == Record_Person.Rows.Count - 1)
                            {
                                result.Append("/上班未签");
                            }
                        }
                        
                    }
                }
            }

            switch (result.ToString())
            {
                case "/准点上班/正常下班": result.Clear(); result.Append("正常");break;               
                case "/准点上班/早退": result.Clear(); result.Append("早退"); break;
                case "/准点上班/下班未签": result.Clear(); result.Append("下班未签"); break;
                case "/正常下班/准点上班": result.Clear(); result.Append("正常"); break;
                case "/迟到/正常下班": result.Clear(); result.Append("迟到"); break;
                case "/上班未签/正常下班": result.Clear(); result.Append("上班未签"); break;
                case "/早退/正常下班": result.Clear(); result.Append("早退"); break;
                case "/上班未签/下班未签": result.Clear(); result.Append("全天未签"); break;
            }

         
            if (result.ToString().IndexOf("/") == 0)
            {
                string str = result.ToString().Remove(0, 1);
                result.Clear();
                result.Append(str);
            }
                                          
            return result.ToString();
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
        }


        private void bandedGridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                string name = Week(DateTime.Parse(e.Column.Caption));
                if (name == "周六" || name == "周日")
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                }
            }
            catch { }

            if (e.Column.FieldName != "YGXM")
            switch (e.CellValue.ToString())
            {
                case "休": e.Appearance.ForeColor = Color.DarkGreen; break;
                case "加班": e.Appearance.ForeColor = Color.DarkMagenta; break;
                case "正常": e.Appearance.ForeColor = Color.Blue; break;
                case "准点上班": e.Appearance.ForeColor = Color.Blue; break;
                case "正常下班": e.Appearance.ForeColor = Color.Blue; break;
                default: e.Appearance.ForeColor = Color.Red; break;
            }

        }

        private void ButtonOrignData_Click(object sender, EventArgs e)
        {
            OrignData form = new OrignData();
            form.Record_DKJ = Record_DKJ.Copy();
            form.Staff = Staff_Orign.Copy();
            form.Show();
        }

        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //还要考虑点击名字的时候
            Record_Person.Clear();
            PersonShift.Clear();
            PersonShift.Rows.Add(new object[] { });
            PersonShift.Rows.Add(new object[] { });
            PersonShiftAll.Clear();
            try
            {
                string KQID = bandedGridView2.GetFocusedRowCellDisplayText("KQID").ToString();
                string Name = bandedGridView2.GetFocusedRowCellDisplayText("YGXM").ToString();
                string Date = bandedGridView2.FocusedColumn.Caption;
                var query = from rec in Record_Dep.AsEnumerable()
                            where rec.Field<string>("ID") == KQID && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date)) >= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(1)) < 0
                            select new
                            {
                                Time = rec.Field<string>("Time"),
                                Source = rec.Field<string>("Source"),
                            };
                foreach (var obj in query)
                {
                    Record_Person.Rows.Add(obj.Time, obj.Source);
                }

                int day = Convert.ToDateTime(Date).Day;                
                string today = "D" + (day) + "T";
                string yesterday = "D" + (day-1) + "T";

                if (day == 1)
                {
                    //第一天的排班
                    var query1 = from Item in ArrangementItem.AsEnumerable()
                                 where Item.Field<string>("KQID") == KQID
                                 select new
                                 {
                                     Today = Item.Field<string>(today).ToString(),
                                 };
                    foreach (var obj in query1)
                    {
                        PersonShift.Rows[0]["ID"] = "";
                        PersonShift.Rows[0]["PD"] = "0";
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
                                 PD = personshift.Field<string>("PD"),
                                 SBSJ = workshift.Field<string>("SBSJ"),
                                 XBSJ = workshift.Field<string>("XBSJ"),
                                 KT = workshift.Field<int>("KT").ToString(),
                             };

                PersonShiftAll.Clear();
                foreach (var obj in query2)
                {
                    PersonShiftAll.Rows.Add(obj.PD, obj.ID, obj.SBSJ, obj.XBSJ, obj.KT);
                }

                PersonData form = new PersonData();
                form.PersonRecord = Record_Person.Copy();
                form.PersonShift = PersonShiftAll.Copy();
                form.WorkShift = WorkShift.Copy();
                form.name = Name;
                form.Date = Date;
                form.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void ButtonImport_Click(object sender, EventArgs e)
        {
            string FileName = "";

            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx";//过滤文件类型
            //ofd.RestoreDirectory = true; //记忆上次浏览路径            
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    FileName = ofd.FileName;
            //}
            //else
            //{
            //    return;
            //}
            FileName = "C:\\Users\\Administrator\\Desktop\\考勤.xlsx";

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

            try
            {
                for (int i = 1; i < dtExcel.Rows.Count; i++)
                {
                    Record_DKJ.Rows.Add(new object[] { dtExcel.Rows[i][0].ToString(), dtExcel.Rows[i][2].ToString(), dtExcel.Rows[i][3].ToString() });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }

            ButtonCal.Enabled = true;
            MessageBox.Show("导入成功，请点击‘查询计算’查看考勤结果！");
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabPage1")
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "Excel文件(*.xlsx)|*.xlsx";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string path = sf.FileName.ToString();
                        gridControl2.ExportToXlsx(path);
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
        }

        private void ButtonFilter_Click(object sender, EventArgs e)
        {
            AttendanceFilter form = new AttendanceFilter();
            form.Show();
        }

        private void searchControl2_TextChanged(object sender, EventArgs e)
        {
            if (AttendanceResult.Rows.Count > 0)
            {
                AttendanceResult.DefaultView.RowFilter = string.Format("YGXM like '%{0}%'", searchControl2.Text);
                AttendanceCollect.DefaultView.RowFilter = string.Format("YGXM like '%{0}%'", searchControl2.Text);
            }
            
        }

        private void 修改为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AttendanceAlter form = new AttendanceAlter();
                form.Name = bandedGridView2.GetFocusedRowCellValue("YGXM").ToString();
                form.Date = bandedGridView2.FocusedColumn.Caption;
                form.Result = bandedGridView2.GetFocusedRowCellValue(bandedGridView2.FocusedColumn.Caption).ToString(); ;
                form.Row = bandedGridView2.GetDataSourceRowIndex(bandedGridView2.FocusedRowHandle);
                form.StartDate = StartDate;
                form.Timespan = Timespan.Days;
                form.Show(this);
            }
            catch { }
            
        }

        private void 修改全列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AttendanceAlter form = new AttendanceAlter();
                form.Name = gridView1.GetFocusedRowCellValue("BMMC").ToString();
                form.Date = bandedGridView2.FocusedColumn.Caption;
                form.Row = bandedGridView2.GetDataSourceRowIndex(bandedGridView2.FocusedRowHandle);
                form.StartDate = StartDate;
                form.Timespan = Timespan.Days;
                form.AlterColumn = true;
                form.Show(this);
            }
            catch { }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gridControl2_MouseDoubleClick(null, null);
        }
    }
}
