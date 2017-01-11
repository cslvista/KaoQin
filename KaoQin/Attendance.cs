using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace KaoQin
{
    public partial class Attendance : Form
    {
        zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机
        DataTable Machine = new DataTable();//机器信息
        DataTable Department = new DataTable();//部门
        DataTable AttendanceResult = new DataTable();//考勤结果
        DataTable Staff = new DataTable();//员工信息
        DataTable Staff_Orign = new DataTable();//打卡机的原始员工数据，包括考勤号和姓名
        DataTable WorkShift = new DataTable();//班次信息
        DataTable PBID = new DataTable();//排班ID
        DataTable ArrangementItem = new DataTable();//排班细表
        DataTable PersonShift = new DataTable();//个人单日的排班
        DataTable Record_Dep = new DataTable();//选定部门员工的打卡数据
        DataTable Record_DKJ = new DataTable();//考勤机原始数据
        DataTable Record_Person = new DataTable();//个人单日打卡数据
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
                if (MessageBox.Show("从考勤机下载数据需要约1分钟的时间，是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            Staff_Orign.Clear();
            Record_DKJ.Clear();            
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


            DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[0]["Password"].ToString()));
            DKJ.Connect_Net(Machine.Rows[0]["IP"].ToString(), Convert.ToInt32(Machine.Rows[0]["Port"].ToString()));
            bool bIsConnected = false;//判断设备是否可连接   
            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            this.Text = "正在读取员工数据...";
            DKJ.ReadAllUserID(0);
            while (DKJ.SSR_GetAllUserInfo(0, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                int position = sName.IndexOf("\0");
                string name = sName.Substring(0, position);//过滤sName中多余字符
                Staff_Orign.Rows.Add(new object[] { sdwEnrollNumber, name });
                
            }

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

            this.Text = "正在下载考勤数据，请稍候...";
            for (int i = 0; i < Machine.Rows.Count; i++)
            {
                DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[i]["Password"].ToString()));
                bIsConnected = DKJ.Connect_Net(Machine.Rows[i]["IP"].ToString(), Convert.ToInt32(Machine.Rows[i]["Port"].ToString()));
                if (bIsConnected == false)
                {
                    MessageBox.Show(string.Format("'{0}'无法连接！", Machine.Rows[i]["Machine"].ToString()));
                    return;
                }
                int j = 0;
                DKJ.ReadAllGLogData(iMachineNumber);//read all the user information to the memory
                while (DKJ.SSR_GetGeneralLogData(iMachineNumber, out dwEnrollNumber, out VerifyMode, out InOutMode, out Year, out Month, out Day, out Hour, out Minute, out Second, ref Workcode))
                {
                    j++;
                    string time = string.Format("{0}-{1}-{2} {3}:{4}:{5}", Year, Month, Day, Hour, Minute, Second);
                    string time1 = Convert.ToDateTime(time).ToString("yyyy-MM-dd  HH:mm:ss");
                    Record_DKJ.Rows.Add(new object[] { dwEnrollNumber, time1, Machine.Rows[i]["Machine"].ToString() });
                    this.Text = string.Format("正在读取{0}第{1}条数据", Machine.Rows[i]["Machine"].ToString(), j);
                }
            }
            //下载数据完毕
            this.Text = "考勤管理";
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

            comboBox1.Text = "在职员工";
            ButtonCal.Enabled = true;
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
            comboBox1.Location = new Point(comboBox1.Location.X, (panelControl2.Height - comboBox1.Height) / 2);
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
            Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl1.Text);
        }

        private void ButtonCal_Click(object sender, EventArgs e)
        {
            bandedGridView2.Columns.Clear();
            bandedGridView2.Bands.Clear();
            AttendanceResult.Columns.Clear();
            searchControl2.Text = "";
            Record_Dep.Clear();

            DateTime StartDate;
            DateTime StopDate;
            TimeSpan Timespan;
            try
            {
                string year = comboBoxYear.Text.Substring(0, 4);
                string month = comboBoxMonth.Text.Substring(0, comboBoxMonth.Text.IndexOf("月"));
                string startDate = Convert.ToDateTime(year + "-" + month + "-" + "1").ToString("yyyy-MM-dd");
                StartDate = Convert.ToDateTime(startDate);
                StopDate = StartDate.AddMonths(1).AddDays(-1);
                Timespan = StopDate - StartDate;

                if (Timespan.Days < 0)
                {
                    MessageBox.Show("结束时间不得小于开始时间！");
                    return;
                }

                if (Timespan.Days > 62)
                {
                    MessageBox.Show("时间跨度不能超过两个月！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            string StaffState = "";
            switch (comboBox1.Text)
            {
                case "全部员工": StaffState = ">='0'"; break;
                case "在职员工": StaffState = "='0'"; break;
                case "离职员工": StaffState = "='1'"; break;
            }

            //读取员工信息            
            try
            {
                string sql1 = string.Format("select KQID,YGXM from KQ_YG where BMID='{0}' and ZT{1}", gridView1.GetFocusedRowCellValue("BMID").ToString(), StaffState);
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
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
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }

            //读取排班主表
            string sql3 = string.Format("select PBID from KQ_PB where YEAR='{0}',MONTH='{1}'",comboBoxYear.Text, comboBoxMonth.Text);
            try
            {
                PBID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误4:" + ex.Message);
                return;
            }

            //读取排班细表
            if (PBID.Rows.Count > 0)
            {
                string sql4 = string.Format("select * from KQ_PB_XB where BMID='{0}' and PBID='{1}'", gridView1.GetFocusedRowCellValue("BMID").ToString(), PBID.Rows[0][0].ToString());
                try
                {
                    ArrangementItem = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误5:" + ex.Message);
                    return;
                }
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
            for (int i = 0; i <= Timespan.Days; i++)
            {
                GridBand Day_band = new GridBand();
                Day_band.Caption = Week(StartDate);
                bandedGridView2.Bands.Add(Day_band);
                BandedGridColumn Day_Column = new BandedGridColumn();
                Day_Column.Caption = StartDate.ToString("yyyy-MM-dd");
                Day_Column.FieldName = Day_Column.Caption;
                Day_Column.Name = Day_Column.Caption;
                Day_Column.Visible = true;
                Day_Column.OptionsColumn.AllowEdit = true;
                Day_band.Columns.Add(Day_Column);
                AttendanceResult.Columns.Add(Day_Column.Name);
                bandedGridView2.Columns.Add(Day_Column);
                StartDate = StartDate.AddDays(1);
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

            //进行数据分析
            AnalysisData(StartDate, Timespan.Days);
        }

        private void AnalysisData(DateTime StartDate, int Timespan)
        {
            AttendanceResult.Clear();
            for (int i = 0; i < Staff.Rows.Count; i++)
            {
                AttendanceResult.Rows.Add(new object[] { });
                for (int j = 0; j <= Timespan ; j++)
                {
                    //查询当日考勤数据
                    Record_Person.Clear();
                    string KQID = Staff.Rows[i]["KQID"].ToString();
                    string Date = StartDate.AddDays(Timespan).ToString("yyyy-MM-dd");
                    var query = from rec in Record_Dep.AsEnumerable()
                                where rec.Field<string>("KQID") == KQID && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date)) >= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(1)) < 0
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
                    if (j == 0)
                    {
                        //第一日只查当日排班数据
                        var query1 = from arrangement in ArrangementItem.AsEnumerable()
                                     join workshift in WorkShift.AsEnumerable()
                                     on arrangement.Field<string>("ID") equals workshift.Field<string>("ID")
                                     select new
                                     {
                                         ID = arrangement.Field<string>("ID"),//ID号
                                         Name = arrangement.Field<string>("ID"),
                                         SBSJ = arrangement.Field<string>("ID"),//上班时间
                                         XBSJ = arrangement.Field<string>("ID"),//下班时间
                                         KT = arrangement.Field<string>("ID"),//跨天
                                     };
                        foreach (var obj in query1)
                        {
                            PersonShift.Rows.Add(obj.ID, obj.Name, obj.SBSJ, obj.XBSJ, obj.KT);
                        }

                    }
                    else
                    {
                        //第二日查昨日和当日排班数据
                        var query1 = from arrangement in ArrangementItem.AsEnumerable()
                                     join workshift in WorkShift.AsEnumerable()
                                     on arrangement.Field<string>("ID") equals workshift.Field<string>("ID")
                                     select new
                                     {
                                         ID = arrangement.Field<string>("ID"),//ID号
                                         Name = arrangement.Field<string>("ID"),
                                         SBSJ = arrangement.Field<string>("ID"),//上班时间
                                         XBSJ = arrangement.Field<string>("ID"),//下班时间
                                         KT = arrangement.Field<string>("ID"),//跨天
                                     };
                        foreach (var obj in query1)
                        {
                            PersonShift.Rows.Add(obj.ID, obj.Name, obj.SBSJ, obj.XBSJ, obj.KT);
                        }
                    }

                    AttendanceResult.Rows[i][j+2] = Result(Record_Person,PersonShift);
                }
            }
            gridControl2.DataSource = AttendanceResult;
            bandedGridView2.BestFitColumns();
        }

        private string Result(DataTable Record_Person,DataTable PersonShift)
        {
           
            if (Record_Person.Rows.Count == 0)
            {
                //MessageBox.Show(string.Format("{0}在{1}没有签到记录！", Name, Convert.ToDateTime(Date).ToString("yyyy年MM月dd日")));              
            }
            else
            {
            }
            return "正常";
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
            Record_Person.Clear();

            try
            {
                string Name = bandedGridView2.GetFocusedRowCellDisplayText("YGXM").ToString();
                string Date = bandedGridView2.FocusedColumn.Caption;
                var query = from rec in Record_Dep.AsEnumerable()
                            where rec.Field<string>("Name") == Name && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date)) >= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(Date).AddDays(1)) < 0
                            select new
                            {
                                Time = rec.Field<string>("Time"),
                                Source = rec.Field<string>("Source"),
                            };
                foreach (var obj in query)
                {
                    Record_Person.Rows.Add(obj.Time, obj.Source);
                }

                if (Record_Person.Rows.Count == 0)
                {
                    MessageBox.Show(string.Format("{0}在{1}没有签到记录！", Name, Convert.ToDateTime(Date).ToString("yyyy年MM月dd日")));
                    return;
                }
                else
                {
                    PersonData form = new PersonData();
                    form.PersonRecord = Record_Person.Copy();
                    form.name = Name;
                    form.Date = Date;
                    form.Show();
                }


            }
            catch { }

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx";//过滤文件类型
            ofd.RestoreDirectory = true; //记忆上次浏览路径
            string FileName = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileName = ofd.FileName;
            }
            else
            {
                return;
            }

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
                MessageBox.Show("文件格式错误！");
                return;
            }


            ButtonOrignData.Enabled = true;
            MessageBox.Show("导入成功，请点击‘查询计算’查看考勤结果！");
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {

        }

        private void ButtonFilter_Click(object sender, EventArgs e)
        {

        }
    }
}
